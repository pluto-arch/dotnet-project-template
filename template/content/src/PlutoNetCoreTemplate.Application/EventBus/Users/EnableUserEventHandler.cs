using MediatR;
using Microsoft.Extensions.Logging;
using PlutoNetCoreTemplate.Domain.Events.UserEvents;
using System.Threading;
using System.Threading.Tasks;

namespace PlutoNetCoreTemplate.Application.EventBus.Users
{
    public class EnableUserEventHandler : INotificationHandler<EnableUserEvent>
    {
        private readonly ILogger<EnableUserEventHandler> _logger;
        public EnableUserEventHandler(ILogger<EnableUserEventHandler> logger)
        {
            this._logger = logger;
        }

        public Task Handle(EnableUserEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("event:{notificationType} 。{@notification}", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
}