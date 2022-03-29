namespace PlutoNetCoreTemplate.Infrastructure.Providers
{
    using Microsoft.Extensions.Logging;

    using System;

    public interface IEventIdProvider
    {
        EventId EventId { get; }
    }
    public class EventIdProvider : IEventIdProvider
    {
        public EventIdProvider()
        {
            this.EventId = new EventId(1, $"pnct_{DateTime.Now.Ticks}");
        }

        public EventId EventId { get; private set; }
    }

    public class NullEventIdProvider : IEventIdProvider
    {
        public NullEventIdProvider()
        {
            this.EventId = new EventId(default, string.Empty);
        }

        public EventId EventId { get; private set; }
    }
}