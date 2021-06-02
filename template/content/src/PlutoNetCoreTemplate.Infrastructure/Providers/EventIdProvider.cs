namespace PlutoNetCoreTemplate.Infrastructure.Providers
{
    using System;
    using Microsoft.Extensions.Logging;

    public interface IEventIdProvider
    {
        EventId EventId { get; }
    }
    public class EventIdProvider:IEventIdProvider
    {
        private static Random r = new Random();

        public EventIdProvider()
        {
            this.EventId=new EventId(1,$"pnct_{DateTime.Now.Ticks}");
        }

        public EventId EventId { get; private set; }
    }

    public class NullEventIdProvider:IEventIdProvider
    {
        public NullEventIdProvider()
        {
            this.EventId=new EventId(default,string.Empty);
        }

        public EventId EventId { get; private set; }
    }
}