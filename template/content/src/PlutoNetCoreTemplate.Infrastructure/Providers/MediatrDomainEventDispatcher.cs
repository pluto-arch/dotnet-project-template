namespace PlutoNetCoreTemplate.Infrastructure.Providers
{
    using System;
    using System.Threading.Tasks;
    using Domain.SeedWork;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class MediatrDomainEventDispatcher: IDomainEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MediatrDomainEventDispatcher> _log;
        public MediatrDomainEventDispatcher(IServiceProvider serviceProvider, ILogger<MediatrDomainEventDispatcher> log)
        {
            _serviceProvider = serviceProvider;
            _log = log;
        }

        public async Task Dispatch(INotification domainEvent)
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            _log.LogDebug("Dispatching Domain Event as MediatR notification.  EventType: {eventType}", domainEvent.GetType());
            await mediator.Publish(domainEvent);
        }
    }
}