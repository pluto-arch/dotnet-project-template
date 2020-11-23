using System;

namespace PlutoNetCoreTemplate.Infrastructure.EventBus.Event
{
    /// <summary>
    /// 集成事件基类
    /// </summary>
    public class IntegrationEvent
    {
        public Guid Id { get; set; }

        public IntegrationEvent()
        {
            Id=Guid.NewGuid();
        }
    }
}