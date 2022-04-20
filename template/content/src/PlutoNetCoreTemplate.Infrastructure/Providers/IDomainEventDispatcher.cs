namespace PlutoNetCoreTemplate.Infrastructure.Providers
{
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;


    public interface IDomainEventDispatcher
    {
        Task Dispatch(INotification domainEvent);
    }


    public class MediatrDomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<MediatrDomainEventDispatcher> _log;
        public MediatrDomainEventDispatcher(IServiceScopeFactory serviceScopeFactory, ILogger<MediatrDomainEventDispatcher> log)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _log = log;
        }

        public async Task Dispatch(INotification domainEvent)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            _log.LogDebug("Dispatching Domain Event as MediatR notification.  EventType: {eventType}", domainEvent.GetType());
            await mediator.Publish(domainEvent);
        }
    }


}