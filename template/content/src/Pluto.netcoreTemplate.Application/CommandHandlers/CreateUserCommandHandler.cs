using MediatR;

using Microsoft.Extensions.Logging;

using Pluto.netcoreTemplate.Application.Commands;
using Pluto.netcoreTemplate.Infrastructure.Providers;

using System.Threading;
using System.Threading.Tasks;
using Pluto.netcoreTemplate.Domain.AggregatesModel.UserAggregate;

namespace Pluto.netcoreTemplate.Application.CommandHandlers
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, bool>
    {

        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        /// <param name="eventIdProvider"></param>
        /// <param name="userRepository"></param>
        public CreateUserCommandHandler(
            IMediator mediator,IUserRepository userRepository)
        {
            _mediator = mediator;
            _userRepository = userRepository;
        }


        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new UserEntity
            {
                UserName = request.UserName,
            };
            user.SetPasswordHash(request.Password); 
            await _userRepository.CreateAsync(user, cancellationToken);
            await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken); 
            return await Task.FromResult(true);
        }
    }
}