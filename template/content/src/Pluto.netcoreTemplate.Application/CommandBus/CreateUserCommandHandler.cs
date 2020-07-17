using System;
using MediatR;
using Pluto.netcoreTemplate.Application.Commands;
using System.Threading;
using System.Threading.Tasks;
using Pluto.netcoreTemplate.Domain.AggregatesModel.UserAggregate;
using Pluto.netcoreTemplate.Infrastructure;
using Pluto.netcoreTemplate.Infrastructure.Extensions;
using PlutoData.Interface;

namespace Pluto.netcoreTemplate.Application.CommandBus
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, bool>
    {

        private readonly IMediator _mediator;
        private readonly IUnitOfWork<PlutonetcoreTemplateDbContext> _unitOfWork;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="unitOfWork"></param>
        public CreateUserCommandHandler(
            IMediator mediator, 
            IUnitOfWork<PlutonetcoreTemplateDbContext> unitOfWork)
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
            await _mediator.DispatchDomainEventsAsync(_unitOfWork.DbContext);
            var res= await _unitOfWork.SaveChangesAsync(cancellationToken);
            return res>0;
        }
    }
}