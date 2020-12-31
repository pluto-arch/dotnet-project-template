using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PlutoNetCoreTemplate.Domain.IRepositories;
using PlutoNetCoreTemplate.Infrastructure;
using PlutoData.Interface;


namespace PlutoNetCoreTemplate.Application.Command
{
    public class DeleteUserCommandHandler:IRequestHandler<DeleteUserCommand,bool>
    {

        private readonly IMediator _mediator;

        private readonly IUnitOfWork<EfCoreDbContext> _unitOfWork;

        public DeleteUserCommandHandler(
            IMediator mediator, IUnitOfWork<EfCoreDbContext> unitOfWork)
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