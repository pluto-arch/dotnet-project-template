using MediatR;

using Microsoft.Extensions.Logging;

using Pluto.netcoreTemplate.Application.Commands;
using Pluto.netcoreTemplate.Infrastructure.Providers;

using System.Threading;
using System.Threading.Tasks;
using Pluto.netcoreTemplate.Domain.AggregatesModel.UserAggregate;
using Pluto.netcoreTemplate.Infrastructure;
using PlutoData.Interface;

namespace Pluto.netcoreTemplate.Application.CommandHandlers
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, bool>
    {

        private readonly IMediator _mediator;

        private readonly IUnitOfWork<PlutonetcoreTemplateDbContext> _unitOfWork;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="unitOfWork"></param>
        public CreateUserCommandHandler(
            IMediator mediator, IUnitOfWork<PlutonetcoreTemplateDbContext> unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }


        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var rep = _unitOfWork.GetRepository<IUserRepository>();
            var user = new UserEntity
            {
                UserName = request.UserName,
                Email= request.UserName+"@qq.com"
            };
            user.SetPasswordHash(request.Password);
            rep.Insert(user);
            return (await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken))>0;
        }
    }
}