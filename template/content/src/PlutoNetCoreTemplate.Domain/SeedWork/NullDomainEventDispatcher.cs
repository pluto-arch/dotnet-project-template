namespace PlutoNetCoreTemplate.Domain.SeedWork
{
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class NullDomainEventDispatcher : IDomainEventDispatcher
    {
        /// <inheritdoc />
        public Task Dispatch(INotification domainEvent, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public static IDomainEventDispatcher Instance { get; } = new NullDomainEventDispatcher();
    }
}