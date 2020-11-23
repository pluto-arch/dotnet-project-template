using PlutoNetCoreTemplate.Infrastructure.EventBus.Event;

namespace PlutoNetCoreTemplate.Infrastructure.EventBus.Abstractions
{
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
    }
}