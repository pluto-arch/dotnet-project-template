using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PlutoNetCoreTemplate.Infrastructure;
using PlutoData.Uows;

namespace PlutoNetCoreTemplate.Application.Command
{
    using Domain.Aggregates.System;
    using Microsoft.EntityFrameworkCore;

    public class DeleteUserCommandHandler:IRequestHandler<DeleteUserCommand,bool>
    {

        //private readonly IMediator _mediator;

        private readonly IEfUnitOfWork<EfCoreDbContext> _unitOfWork;

        public DeleteUserCommandHandler(IEfUnitOfWork<EfCoreDbContext> unitOfWork)
        {
            //_mediator = mediator;
            _unitOfWork = unitOfWork;
        }


        /// <inheritdoc />
        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var rep = _unitOfWork.GetRepository<IUserRepository>();
            var user = await rep.FirstOrDefaultAsync(x=>x.Id==request.Id, cancellationToken: cancellationToken);
            await rep.DeleteAsync(user, cancellationToken: cancellationToken);
            return (await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken)) > 0;
        }
    }
}