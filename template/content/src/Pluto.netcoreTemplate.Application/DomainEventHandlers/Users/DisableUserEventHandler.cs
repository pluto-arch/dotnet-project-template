using MediatR;

using Microsoft.Extensions.Logging;

using Pluto.netcoreTemplate.Domain.Events.UserEvents;
using Pluto.netcoreTemplate.Infrastructure.Providers;

using System.Threading;
using System.Threading.Tasks;

namespace Pluto.netcoreTemplate.Application.DomainEventHandlers.Users
{
    public class DisableUserEventHandler : INotificationHandler<DisableUserEvent>
    {

        private readonly ILogger<EnableUserEventHandler> _logger;
        private readonly EventIdProvider _eventIdProvider;


        public DisableUserEventHandler(ILogger<EnableUserEventHandler> logger, EventIdProvider eventIdProvider)
        {
            _logger = logger;
            _eventIdProvider = eventIdProvider;
        }

        public Task Handle(DisableUserEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(_eventIdProvider.EventId, "event:{notificationType} 。{@notification}", notification.GetType().Name, notification);

            var aaa = notification.Message;

            return Task.CompletedTask;
        }
    }
}