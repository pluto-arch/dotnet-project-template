using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Polly;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client.Events;
using EventBus.Abstractions;
using EventBus.Event;

namespace EventBus.RabbitMQ
{
    public class EventBusRabbitMQ : IEventBus
    {
        private readonly string _directExchangeName;
        private readonly string _fanoutExchangeName;
        private string _directQueueName;
        private readonly int _retryCount;

        private readonly IRabbitMqPersistentConnection _persistentConnection;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EventBusRabbitMQ> _logger;
        private IModel _consumerChannel;
        private IModel _productChannel;
        private IModel _noticeChannel;
        private readonly IModel _listenChannel;


        public EventBusRabbitMQ(
            IServiceProvider serviceProvider,
            ILogger<EventBusRabbitMQ> logger,
            string directExchangeName = "",
            string fanoutExchangeName = "",
            string directQueueName = "",
            int retryCount = 5)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? NullLogger<EventBusRabbitMQ>.Instance;
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

            // 创建直连模式消费者信道
            _consumerChannel = CreateConsumerChannel();
            // 创建广播模式消费者信道
            _listenChannel = CreateListingChannel();

            _subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
        }

        public Task PublishAsync(IntegrationEvent @event)
        {
            if (_productChannel == null || !_productChannel.IsOpen)
            {
                _productChannel = CreateProductorChannel();
            }

            Push(@event, _productChannel, _directExchangeName);
            return Task.CompletedTask;
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


        /// <summary>
        /// 发布通知
        /// </summary>
        /// <param name="event"></param>
        /// <remarks>广播模式，绑定的队列都会收到信息</remarks>
        public Task NoticeAsync(IntegrationEvent @event)
        {
            if (_noticeChannel == null || !_noticeChannel.IsOpen)
            {
                _noticeChannel = CreateProductorChannel();
            }

            Push(@event, _noticeChannel, _fanoutExchangeName);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 配置广播模式队列和对应动态事件
        /// </summary>
        /// <typeparam name="TH">对应事件的处理程序</typeparam>
        /// <param name="eventName">事件民名称</param>
        /// <param name="queueName">队列名</param>
        public void ListeningDynamic<TH>(string eventName, string queueName) where TH : IDynamicIntegrationEventHandler
        {
            DoInternalListening(eventName, queueName);
            _subsManager.AddDynamicSubscription<TH>(eventName);
            StartBasicListeningConsume(queueName);
        }

        /// <summary>
        /// 取消配置广播模式队列和对应动态事件
        /// </summary>
        /// <typeparam name="TH"></typeparam>
        /// <param name="eventName"></param>
        /// <param name="queueName"></param>
        public void UnListeningDynamic<TH>(string eventName, string queueName)
            where TH : IDynamicIntegrationEventHandler
        {
            _subsManager.RemoveDynamicListener<TH>(eventName);
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
                }

                channel.Close();
            }
        }


        #region 直接交换模式

        /// <summary>
        /// 初始化消费绑定
        /// </summary>
        /// <param name="eventName"></param>
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

        /// <summary>
        /// 开始消费 - 基础手动应答模式
        /// </summary>
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

        /// <summary>
        /// 消费事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        /// <returns></returns>
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
                _logger.LogError(ex, "handle event has an error : {@message}", ex.Message);
            }

            _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }

        #endregion


        #region 广播模式

        /// <summary>
        /// 初始化广播模式绑定
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="queueName">队列名称，将绑定到fanoutExchange</param>
        private void DoInternalListening(string eventName, string queueName)
        {
            var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
            if (!containsKey)
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

        /// <summary>
        /// 开始基本广播模式消费
        /// </summary>
        /// <param name="queueName"></param>
        private void StartBasicListeningConsume(string queueName)
        {
            if (_listenChannel != null && !_listenChannel.IsClosed)
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

        /// <summary>
        /// 广播模式消息接受事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        /// <returns></returns>
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
                _logger.LogError(ex, "process notice has an error: {@message}", ex.Message);
            }

            _listenChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }

        #endregion


        #region 接受消息后进行处理

        private async Task ProcessEvent(string eventName, string message)
        {
            if (!_subsManager.HasSubscriptionsForEvent(eventName))
            {
                _logger.LogWarning("no handler for：{eventName}。body：{message}", eventName, message);
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
                    if (_serviceProvider.GetService(subscription.HandlerType) is not IDynamicIntegrationEventHandler
                        handler) continue;
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
                    await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] {integrationEvent});
                }
            }
        }

        #endregion


        #region private

        /// <summary>
        /// 推送消息到队列
        /// </summary>
        /// <param name="event"></param>
        /// <param name="channel"></param>
        /// <param name="exchangeName"></param>
        private void Push(IntegrationEvent @event, IModel channel, string exchangeName)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var policy = Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (ex, time) =>
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

        /// <summary>
        /// 订阅管理者事件移除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventName"></param>
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
                    _consumerChannel.Close();
                }

                channel.Close();
            }
        }

        /// <summary>
        /// 直连模式消费者信道 - 复用
        /// </summary>
        /// <returns></returns>
        private IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var channel = _persistentConnection.CreateModel();
            channel.ExchangeDeclare(exchange: _directExchangeName,
                type: ExchangeType.Direct);
            channel.QueueDeclare(queue: _directQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception,
                    $"error with create rabbitmq channel (CreateConsumerChannel): {ea.Exception.Message}");
                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
                StartBasicConsume();
            };
            return channel;
        }

        /// <summary>
        /// 广播模式消费者信道  - 复用
        /// </summary>
        /// <returns></returns>
        private IModel CreateListingChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var channel = _persistentConnection.CreateModel();
            channel.ExchangeDeclare(exchange: _fanoutExchangeName,
                type: ExchangeType.Fanout);
            return channel;
        }

        /// <summary>
        /// 直连模式身生产者信道 - 复用
        /// </summary>
        /// <returns></returns>
        private IModel CreateProductorChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var channel = _persistentConnection.CreateModel();
            channel.ExchangeDeclare(exchange: _directExchangeName, type: ExchangeType.Direct);
            channel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception,
                    $"error with create rabbitmq channel with direct exchange (CreateProductorChannel): {ea.Exception.Message}");
                _productChannel.Close();
                _productChannel.Dispose();
                _productChannel = CreateProductorChannel();
            };
            return channel;
        }

        #endregion
    }
}