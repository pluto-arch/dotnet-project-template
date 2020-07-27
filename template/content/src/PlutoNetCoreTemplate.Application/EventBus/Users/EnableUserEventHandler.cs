using MediatR;

using Microsoft.Extensions.Logging;

using PlutoNetCoreTemplate.Domain.Events.UserEvents;
using PlutoNetCoreTemplate.Infrastructure.Providers;

using System.Threading;
using System.Threading.Tasks;

namespace PlutoNetCoreTemplate.Application.EventBus.Users
{
    public class EnableUserEventHandler : INotificationHandler<EnableUserEvent>
    {
        private readonly ILogger<EnableUserEventHandler> _logger;
        private readonly EventIdProvider _eventIdProvider;
        public EnableUserEventHandler(ILogger<EnableUserEventHandler> logger, EventIdProvider eventIdProvider)
        {
            this._logger = logger;
            _eventIdProvider = eventIdProvider;
        }

        public Task Handle(EnableUserEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(_eventIdProvider.EventId, "event:{notificationType} 。{@notification}", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
}