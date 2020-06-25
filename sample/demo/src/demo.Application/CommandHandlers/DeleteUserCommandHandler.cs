using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Demo.Application.Commands;
using Demo.Domain.AggregatesModel.UserAggregate;
using Demo.Infrastructure;
using PlutoData.Interface;


//  ===================
// 2020-03-24
//  ===================

namespace Demo.Application.CommandHandlers
{
    public class DeleteUserCommandHandler:IRequestHandler<DeleteUserCommand,bool>
    {

        private readonly IMediator _mediator;

        private readonly IUnitOfWork<DemoDbContext> _unitOfWork;

        public DeleteUserCommandHandler(
            IMediator mediator, IUnitOfWork<DemoDbContext> unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }


        /// <inheritdoc />
        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var rep = _unitOfWork.GetRepository<IUserRepository>();
            rep.Delete(request.Id);
            return (await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken)) > 0;
        }
    }
}