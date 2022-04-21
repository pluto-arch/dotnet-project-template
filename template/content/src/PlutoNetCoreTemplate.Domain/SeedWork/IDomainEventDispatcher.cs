namespace PlutoNetCoreTemplate.Domain.SeedWork
{
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;


    public interface IDomainEventDispatcher
    {
        Task Dispatch(INotification domainEvent);
    }

}