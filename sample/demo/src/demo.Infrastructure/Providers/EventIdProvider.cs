using Microsoft.Extensions.Logging;
using System;

namespace Demo.Infrastructure.Providers
{
    public class EventIdProvider
    {
        public EventIdProvider()
        {
            var id = (int)(DateTime.Now.Ticks % int.MaxValue);
            var name = $"Demo_{DateTime.Now.ToString("yyyyMMddHHmmssfffff")}";
            EventId = new EventId(id, name);
        }
        public EventId EventId { get; }

    }

}