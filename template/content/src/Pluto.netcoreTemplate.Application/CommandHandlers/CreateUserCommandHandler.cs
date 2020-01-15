using MediatR;

using Microsoft.Extensions.Logging;

using Pluto.netcoreTemplate.Application.Commands;
using Pluto.netcoreTemplate.Infrastructure.Providers;

using System.Threading;
using System.Threading.Tasks;

namespace Pluto.netcoreTemplate.Application.CommandHandlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, bool>
    {

        private readonly IMediator _mediator;
        private readonly ILogger<CreateUserCommandHandler> _logger;
        private readonly EventIdProvider _eventIdProvider;
        public CreateUserCommandHandler(
            IMediator mediator,
            ILogger<CreateUserCommandHandler> logger,
            EventIdProvider eventIdProvider)
        {
            _mediator = mediator;
            _logger = logger;
            _eventIdProvider = eventIdProvider;
        }


        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(true);
        }
    }
}