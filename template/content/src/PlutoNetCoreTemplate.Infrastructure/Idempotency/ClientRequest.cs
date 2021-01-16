using System;

namespace PlutoNetCoreTemplate.Infrastructure.Idempotency
{
    public class ClientRequest<TCommand>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }

        public TCommand Command { get; set; }
    }
}