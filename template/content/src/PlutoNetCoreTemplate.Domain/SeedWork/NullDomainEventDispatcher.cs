namespace PlutoNetCoreTemplate.Domain.SeedWork
{
    using System.Threading.Tasks;
    using MediatR;

    public class NullDomainEventDispatcher:IDomainEventDispatcher
    {
        /// <inheritdoc />
        public Task Dispatch(INotification domainEvent)
        {
            return Task.CompletedTask;
        }
    }
}