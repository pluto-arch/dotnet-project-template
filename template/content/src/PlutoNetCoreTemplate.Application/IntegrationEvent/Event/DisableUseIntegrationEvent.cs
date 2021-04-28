namespace PlutoNetCoreTemplate.Application.IntegrationEvent.Event
{
    using EventBus.Event;

    public class DisableUseIntegrationEvent: IntegrationEvent
    {
        public string UserOpenId { get; set; }
    }
}