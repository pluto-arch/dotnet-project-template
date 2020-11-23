using MediatR;
using Microsoft.Extensions.Logging;
using PlutoNetCoreTemplate.Domain.Events.UserEvents;
using System.Threading;
using System.Threading.Tasks;

namespace PlutoNetCoreTemplate.Application.DomainEventHandler
{
    public class DisableUserEventHandler : INotificationHandler<DisableUserEvent>
    {

        private readonly ILogger<EnableUserEventHandler> _logger;


        public DisableUserEventHandler(ILogger<EnableUserEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(DisableUserEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation( "event:{notificationType} 。{@notification}", notification.GetType().Name, notification);

            var aaa = notification.Message;

            return Task.CompletedTask;
        }
    }
}