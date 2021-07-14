using EventBus.Abstractions;
using EventBus.Event;

using Newtonsoft.Json;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace EventBus.InMemory
{
    /// <summary>
    /// 内存中事件总线
    /// </summary>
    public class EventBusMemoryQueue : IEventBus
    {
        private readonly IEventBusSubscriptionsManager _subsManager;

        private readonly IServiceProvider _serviceProvider;

        public EventBusMemoryQueue(IServiceProvider serviceProvider, IEventBusSubscriptionsManager subsManager)
        {
            _serviceProvider = serviceProvider;
            _subsManager = subsManager;
        }

        public void ListeningDynamic<TH>(string eventName, string queueName) where TH : IDynamicIntegrationEventHandler
        {
            _subsManager.AddDynamicSubscription<TH>(eventName);
        }

        public async Task NoticeAsync(IntegrationEvent @event)
        {
            string eventName = @event.GetType().Name;
            string message = JsonConvert.SerializeObject(@event);
            await ProcessEvent(eventName, message);
        }

        public async Task PublishAsync(IntegrationEvent @event)
        {
            string eventName = @event.GetType().Name;
            string message = JsonConvert.SerializeObject(@event);
            await ProcessEvent(eventName, message);
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            _subsManager.AddSubscription<T, TH>();
        }

        public void SubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            _subsManager.AddDynamicSubscription<TH>(eventName);
        }

        public void UnListeningDynamic<TH>(string eventName, string queueName) where TH : IDynamicIntegrationEventHandler
        {
            _subsManager.RemoveDynamicSubscription<TH>(eventName);
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            _subsManager.RemoveSubscription<T, TH>();
        }

        public void UnsubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            _subsManager.RemoveDynamicSubscription<TH>(eventName);
        }



        #region private
        private async Task ProcessEvent(string eventName, string message)
        {
            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                var subscriptions = _subsManager.GetHandlersForEvent(eventName);

                foreach (var subscription in subscriptions)
                {
                    if (subscription.IsDynamic)
                    {
                        
                        if (_serviceProvider.GetService(subscription.HandlerType) is IDynamicIntegrationEventHandler handler)
                        {
                            dynamic eventData = JsonConvert.DeserializeObject<ExpandoObject>(message);
                            await handler.Handle(eventData);
                        }
                    }
                    else
                    {
                        var handler = _serviceProvider.GetService(subscription.HandlerType);
                        if (handler is not null)
                        {
                            var eventType = _subsManager.GetEventTypeByName(eventName);
                            object integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                            await (Task)concreteType.GetMethod(nameof(IDynamicIntegrationEventHandler.Handle))!.Invoke(handler, new object[] { integrationEvent! })!;
                        }
                    }
                }
            }

        }
        #endregion
    }
}
