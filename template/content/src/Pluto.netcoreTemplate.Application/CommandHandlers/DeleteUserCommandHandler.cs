using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Pluto.netcoreTemplate.Application.Commands;
using Pluto.netcoreTemplate.Domain.AggregatesModel.UserAggregate;



//  ===================
// 2020-03-24
//  ===================

namespace Pluto.netcoreTemplate.Application.CommandHandlers
{
    public class DeleteUserCommandHandler:IRequestHandler<DeleteUserCommand,bool>
    {

        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(
            IUserRepository userRepository, IMediator mediator)
        {
            _userRepository = userRepository;
            _mediator = mediator;
        }


        /// <inheritdoc />
        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            _userRepository.Delete(x => x.Id == (int)request.Id);
            var res= await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return res>0;
        }
    }
}