namespace EventBus.Abstractions
{
    using Event;

    public interface IEventBus
    {
        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="event"></param>
        void Publish(IntegrationEvent @event);


        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="T">继承事件对象</typeparam>
        /// <typeparam name="TH">对应事件的处理程序</typeparam>
        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;


        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        void Unsubscribe<T, TH>()
            where TH : IIntegrationEventHandler<T>
            where T : IntegrationEvent;


        /// <summary>
        /// 订阅动态事件
        /// </summary>
        /// <typeparam name="TH"></typeparam>
        /// <param name="eventName"></param>
        void SubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;


        /// <summary>
        /// 取消订阅动态事件
        /// </summary>
        /// <typeparam name="TH"></typeparam>
        /// <param name="eventName"></param>
        void UnsubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;



        /// <summary>
        /// 发出通知给所有订阅者
        /// </summary>
        /// <param name="event"></param>
        void Notice(IntegrationEvent @event);

        /// <summary>
        /// 监听通知
        /// </summary>
        /// <typeparam name="TH"></typeparam>
        /// <param name="eventName"></param>
        void ListeningDynamic<TH>(string eventName,string queueName) where TH : IDynamicIntegrationEventHandler;

        /// <summary>
        /// 取消监听通知
        /// </summary>
        /// <typeparam name="TH"></typeparam>
        /// <param name="eventName"></param>
        /// <param name="queueName"></param>
        void UnListeningDynamic<TH>(string eventName,string queueName)
            where TH : IDynamicIntegrationEventHandler;
    }
}