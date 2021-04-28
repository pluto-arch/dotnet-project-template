namespace PlutoNetCoreTemplate.Application.IntegrationEvent.EventHandler
{
    using System.Threading.Tasks;
    using Event;
    using EventBus.Abstractions;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// 动态事件
    /// </summary>
    public class DisableUserIntegrationDynamicEventHandler:IDynamicIntegrationEventHandler
    {
        private readonly ILogger<DisableUserIntegrationDynamicEventHandler> _logger;

        public DisableUserIntegrationDynamicEventHandler(ILogger<DisableUserIntegrationDynamicEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(dynamic eventData)
        {
            await Task.Delay(1);
            if (eventData is DisableUseIntegrationEvent @event)
            {
                _logger.LogInformation("接受到动态事件, 解析类型后为[DisableUseIntegrationEvent]类型 ：{@eventData}", @event);
            }
        }
    }
}