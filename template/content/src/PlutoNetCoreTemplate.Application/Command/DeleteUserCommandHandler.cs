using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PlutoNetCoreTemplate.Domain.IRepositories;
using PlutoNetCoreTemplate.Infrastructure;
using PlutoData.Uows;

namespace PlutoNetCoreTemplate.Application.Command
{
    public class DeleteUserCommandHandler:IRequestHandler<DeleteUserCommand,bool>
    {

        private readonly IMediator _mediator;

        private readonly IEfUnitOfWork<EfCoreDbContext> _unitOfWork;

        public DeleteUserCommandHandler(
            IMediator mediator, IEfUnitOfWork<EfCoreDbContext> unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }


        /// <inheritdoc />
        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var rep = _unitOfWork.GetRepository<IUserRepository>();
            var user = rep.Find(request.Id);
            rep.Delete(user);
            return (await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken)) > 0;
        }
    }
}