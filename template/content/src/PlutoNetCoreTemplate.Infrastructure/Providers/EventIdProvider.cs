using Microsoft.Extensions.Logging;

using System;

namespace PlutoNetCoreTemplate.Infrastructure.Providers
{
    public class EventIdProvider
    {
        public EventIdProvider()
        {
            var id = (int)(DateTime.Now.Ticks % int.MaxValue);
            var name = $"PlutoNetCoreTemplate_{DateTime.Now:yyyyMMddHHmmssfffff}";
            EventId = new EventId(id, name);
        }
        public EventId EventId { get; }

    }
}