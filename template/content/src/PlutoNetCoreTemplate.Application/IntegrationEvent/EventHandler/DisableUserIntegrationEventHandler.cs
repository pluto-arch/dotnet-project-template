namespace PlutoNetCoreTemplate.Application.IntegrationEvent.EventHandler
{
    using System.Threading.Tasks;
    using Event;
    using EventBus.Abstractions;
    using Microsoft.Extensions.Logging;

    public class DisableUserIntegrationEventHandler: IIntegrationEventHandler<DisableUseIntegrationEvent>
    {
        private readonly ILogger<DisableUserIntegrationEventHandler> _logger;

        public DisableUserIntegrationEventHandler(ILogger<DisableUserIntegrationEventHandler> logger)
        {
            _logger = logger;
        }


        public async Task Handle(DisableUseIntegrationEvent @event)
        {
            await Task.Delay(1);
            _logger.LogInformation("接收到[DisableUseIntegrationEvent] : {@event}",@event);
        }
    }
}