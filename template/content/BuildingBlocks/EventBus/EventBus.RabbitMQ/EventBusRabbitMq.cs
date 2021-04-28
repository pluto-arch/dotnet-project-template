
namespace EventBus.RabbitMQ
{
    using System;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using Abstractions;
    using Event;
    using global::RabbitMQ.Client;
    using global::RabbitMQ.Client.Events;
    using global::RabbitMQ.Client.Exceptions;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Polly;

    public class EventBusRabbitMq:IEventBus
    {
        private readonly string _directExchangeName ;
        private readonly string _fanoutExchangeName ;
        private string _directQueueName;
        private readonly int _retryCount;

        private readonly IRabbitMqPersistentConnection _persistentConnection;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EventBusRabbitMq> _logger;
        private IModel _consumerChannel;
        private IModel _productChannel;
        private IModel _noticeChannel;
        private IModel _listenChannel;


        public EventBusRabbitMq( 
            IServiceProvider serviceProvider,
            ILogger<EventBusRabbitMq> logger,
            string directExchangeName = "",
            string fanoutExchangeName = "",
            string directQueueName = "",
            int retryCount = 5)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? NullLogger<EventBusRabbitMq>.Instance;
            _persistentConnection = serviceProvider.GetService<IRabbitMqPersistentConnection>()
                                    ?? throw new ArgumentNullException(nameof(_persistentConnection));
            _subsManager = serviceProvider.GetService<IEventBusSubscriptionsManager>() 
                           ?? new InMemoryEventBusSubscriptionsManager();

            if (directExchangeName == fanoutExchangeName)
            {
                throw new InvalidOperationException("direct exchange mast different from fanout exchange");
            }

            _directExchangeName = directExchangeName;
            _fanoutExchangeName = fanoutExchangeName;
            _directQueueName = directQueueName;
            _retryCount = retryCount;
            _consumerChannel = CreateConsumerChannel();
            _listenChannel = CreateListingChannel();
            _subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
        }


        public void Publish(IntegrationEvent @event)
        {
            if (_productChannel == null||!_productChannel.IsOpen)
            {
                _productChannel = CreateProductorChannel();
            }
            Push(@event, _productChannel, _directExchangeName);
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>(); 
            DoInternalSubscription(eventName);
            _subsManager.AddSubscription<T, TH>();
            StartBasicConsume();
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();
            _logger.LogInformation("Unsubscribing from event {EventName}", eventName);
            _subsManager.RemoveSubscription<T, TH>();
        }

        public void SubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            DoInternalSubscription(eventName);
            _subsManager.AddDynamicSubscription<TH>(eventName);
            StartBasicConsume();
        }

        public void UnsubscribeDynamic<TH>(string eventName) 
            where TH : IDynamicIntegrationEventHandler
        {
            _subsManager.RemoveDynamicSubscription<TH>(eventName);
        }

        public void Notice(IntegrationEvent @event)
        {
            if (_noticeChannel == null||!_noticeChannel.IsOpen)
            {
                _noticeChannel = CreateProductorChannel();
            }
            Push(@event, _noticeChannel,_fanoutExchangeName);
        }

        public void ListeningDynamic<TH>(string eventName, string queueName) where TH : IDynamicIntegrationEventHandler
        {
            DoInternalListening(eventName, queueName);
            _subsManager.AddDynamicSubscription<TH>(eventName);
            StartBasicListeningConsume(queueName);
        }

        public void UnListeningDynamic<TH>(string eventName, string queueName) where TH : IDynamicIntegrationEventHandler
        {
            _subsManager.RemoveDynamicSubscription<TH>(eventName);
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }
            using (var channel = _persistentConnection.CreateModel())
            {
                channel.QueueUnbind(queue: eventName,
                    exchange: _fanoutExchangeName,
                    routingKey: eventName);

                if (_subsManager.IsEmpty)
                {
                    _listenChannel.Close();
                    _listenChannel.Dispose();
                }
                channel.Close();
            }
        }

        #region private

        private void Push(IntegrationEvent @event,IModel channel,string exchangeName)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }
            var policy = Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogWarning("send message with {ex}. retry after {time}s ", ex.Message, time);
                });

            var eventName = @event.GetType().Name;
            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);
            policy.Execute(() =>
            {
                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2;
                channel.BasicPublish(
                    exchange: exchangeName,
                    routingKey: eventName,
                    mandatory: true,
                    basicProperties: properties,
                    body: body);
            });
        }

        private void DoInternalSubscription(string eventName)
        {
            var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
            if (containsKey)
            {
                return;
            }
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }
            using (var channel = _persistentConnection.CreateModel())
            {
                channel.QueueBind(queue: _directQueueName,
                    exchange: _directExchangeName,
                    routingKey: eventName);
                channel.Close();
            }
        }


        private void SubsManager_OnEventRemoved(object sender, string eventName)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            using (var channel = _persistentConnection.CreateModel())
            {
                channel.QueueUnbind(queue: _directQueueName,
                    exchange: _directExchangeName,
                    routingKey: eventName);

                if (_subsManager.IsEmpty)
                {
                    _directQueueName = string.Empty;
                    _consumerChannel.Close();
                    _consumerChannel.Dispose();
                }
                channel.Close();
            }
        }

        private IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }
            var channel = _persistentConnection.CreateModel();
            channel.ExchangeDeclare(exchange: _directExchangeName,
                type: "direct");
            channel.QueueDeclare(queue: _directQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, $"error with create rabbitmq channel (CreateConsumerChannel): {ea.Exception.Message}");
                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
                StartBasicConsume();
            };
            return channel;
        }

        private IModel CreateListingChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }
            var channel = _persistentConnection.CreateModel();
            channel.ExchangeDeclare(exchange: _fanoutExchangeName,
                type: "fanout");
            return channel;
        }

        private IModel CreateProductorChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }
            var channel = _persistentConnection.CreateModel();
            channel.ExchangeDeclare(exchange: _directExchangeName, type: "direct");
            channel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, $"error with create rabbitmq channel with direct exchange (CreateProductorChannel): {ea.Exception.Message}");
                _productChannel.Close();
                _productChannel.Dispose();
                _productChannel = CreateProductorChannel();
            };
            return channel;
        }

        private void StartBasicConsume()
        {
            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);
                consumer.Received += Consumer_Received;
                _consumerChannel.BasicConsume(
                    queue: _directQueueName,
                    autoAck: false,
                    consumer: consumer);
            }
            else
            {
                _logger.LogError("StartBasicConsume can't call on _consumerChannel == null");
            }
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

            try
            {
                await ProcessEvent(eventName, message);
            }
            catch (Exception ex)
            {
                _logger.LogError("接收到信息，在处理信息时发生异常 {@ex}", ex);
            }
            _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (!_subsManager.HasSubscriptionsForEvent(eventName))
            {
                _logger.LogWarning("no handler for：{eventName}。body：{message}", eventName,message);
                return;
            }

            var subscriptions = _subsManager.GetHandlersForEvent(eventName);
            foreach (var subscription in subscriptions)
            {
                if (!subscription.Enabled)
                {
                    _logger.LogWarning($"{subscription.HandlerType.Name} handler has been disabled");
                    continue;
                }
                if (subscription.IsDynamic)
                {
                    if (_serviceProvider.GetService(subscription.HandlerType) is not IDynamicIntegrationEventHandler handler) continue;
                    dynamic eventData = JsonConvert.DeserializeObject<dynamic>(message);
                    await Task.Yield();
                    await handler.Handle(eventData);
                }
                else
                {
                    var handler = _serviceProvider.GetService(subscription.HandlerType);
                    if (handler == null) continue;
                    var eventType = _subsManager.GetEventTypeByName(eventName);
                    var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                    var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                    await Task.Yield();
                    await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                }
            }
        }


        private void DoInternalListening(string eventName, string queueName)
        {
            var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
            if (containsKey)
            {
                return;
            }
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }
            using (var channel = _persistentConnection.CreateModel())
            {
                channel.QueueDeclare(queueName);
                channel.QueueBind(queue: queueName,
                    exchange: _fanoutExchangeName,
                    routingKey: eventName);
                channel.Close();
            }
        }

        private void StartBasicListeningConsume(string queueName)
        {
            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_listenChannel);
                consumer.Received += Notice_Received;
                _listenChannel.BasicConsume(
                    queue: queueName,
                    autoAck: false,
                    consumer: consumer);
            }
            else
            {
                _logger.LogError("StartBasicConsume can't call on _consumerChannel == null");
            }
        }

        private async Task Notice_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

            try
            {
                await ProcessEvent(eventName, message);
            }
            catch (Exception ex)
            {
                _logger.LogError("接收到信息，在处理信息时发生异常 {@ex}", ex);
            }
            _listenChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }

        #endregion

        
    }
}