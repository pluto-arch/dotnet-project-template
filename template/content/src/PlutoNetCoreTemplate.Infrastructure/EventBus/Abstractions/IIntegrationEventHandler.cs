using System.Threading.Tasks;
using PlutoNetCoreTemplate.Infrastructure.EventBus.Event;

namespace PlutoNetCoreTemplate.Infrastructure.EventBus.Abstractions
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> 
        : IIntegrationEventHandler where TIntegrationEvent : IntegrationEvent
    {
        /// <summary>
        /// 处理事件
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        Task Handle(TIntegrationEvent @event);
    }
    public interface IIntegrationEventHandler
    {
    }
}