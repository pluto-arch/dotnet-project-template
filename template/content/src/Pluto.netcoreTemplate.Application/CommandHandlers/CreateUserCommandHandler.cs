using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Pluto.netcoreTemplate.Application.Commands;
using Pluto.netcoreTemplate.Domain.Entities.UserAggregate;
using Pluto.netcoreTemplate.Infrastructure.Providers;

namespace Pluto.netcoreTemplate.Application.CommandHandlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, bool>
    {

        private readonly IMediator _mediator;
        private readonly ILogger<CreateUserCommandHandler> _logger;
        private readonly EventIdProvider _eventIdProvider;
        private readonly IUserRepository _userRepository;
        public CreateUserCommandHandler(
            IMediator mediator, 
            ILogger<CreateUserCommandHandler> logger, 
            EventIdProvider eventIdProvider, 
            IUserRepository userRepository)
        {
            _mediator = mediator;
            _logger = logger;
            _eventIdProvider = eventIdProvider;
            _userRepository = userRepository;
        }


        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User(request.UserName,request.Tel,1);
            user.AddUserBook(1,"123123",2.33M);
            user.AddUserBook(2, "1231232222", 2.88M);
            _logger.LogInformation("----- 创建User: {@Order}", user);
            user.DisableUser();
            _userRepository.Add(user);
            return await _userRepository.UnitOfWork
                .SaveEntitiesAsync(cancellationToken);
        }
    }
}