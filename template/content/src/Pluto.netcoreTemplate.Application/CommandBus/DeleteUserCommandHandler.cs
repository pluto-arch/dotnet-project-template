using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Pluto.netcoreTemplate.Application.Commands;
using Pluto.netcoreTemplate.Domain.AggregatesModel.UserAggregate;
using Pluto.netcoreTemplate.Infrastructure;
using PlutoData.Interface;



namespace Pluto.netcoreTemplate.Application.CommandBus
{
    public class DeleteUserCommandHandler:IRequestHandler<DeleteUserCommand,bool>
    {

        private readonly IMediator _mediator;

        private readonly IUnitOfWork<PlutonetcoreTemplateDbContext> _unitOfWork;

        public DeleteUserCommandHandler(
            IMediator mediator, IUnitOfWork<PlutonetcoreTemplateDbContext> unitOfWork)
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