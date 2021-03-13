using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using PlutoNetCoreTemplate.Infrastructure;
using PlutoNetCoreTemplate.Infrastructure.Extensions;
using PlutoData.Uows;

namespace PlutoNetCoreTemplate.Application.Command
{
    using Domain.Aggregates.System;

    /// <summary>
    /// 
    /// </summary>
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, bool>
    {

        private readonly IMediator _mediator;
        private readonly IEfUnitOfWork<EfCoreDbContext> _unitOfWork;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="unitOfWork"></param>
        public CreateUserCommandHandler(
            IMediator mediator,
            IEfUnitOfWork<EfCoreDbContext> unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _userRepository =unitOfWork.GetRepository<IUserRepository>();
        }


        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new UserEntity
            {
                UserName = request.UserName,
                Email= request.UserName+"@qq.com"
            };
            user.SetPasswordHash(request.Password);  // 有可能会注册领域事件
            _userRepository.Insert(user);

            // 如果要触发领域事件，
            await _mediator.DispatchDomainEventsAsync(_unitOfWork.DbContext,cancellationToken);
            var res= await _unitOfWork.SaveChangesAsync(cancellationToken);
            return res>0;
        }
    }
}