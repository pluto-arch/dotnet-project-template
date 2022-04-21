namespace PlutoNetCoreTemplate.Domain.SeedWork
{
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;


    public interface IDomainEventDispatcher
    {
        Task Dispatch(INotification domainEvent, CancellationToken cancellationToken = default);
    }

}