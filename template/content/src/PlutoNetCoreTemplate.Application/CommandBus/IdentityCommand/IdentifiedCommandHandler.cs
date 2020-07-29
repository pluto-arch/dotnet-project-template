using MediatR;

using Microsoft.Extensions.Logging;

using System.Threading;
using System.Threading.Tasks;

namespace PlutoNetCoreTemplate.Application.CommandBus.IdentityCommand
{
    public class IdentifiedCommandHandler<T, R> : IRequestHandler<IdentifiedCommand<T, R>, R>
        where T : IRequest<R>
    {

        private readonly ILogger<IdentifiedCommandHandler<T, R>> _logger;

        public IdentifiedCommandHandler(ILogger<IdentifiedCommandHandler<T, R>> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 这个方法处理命令。它只是确保不存在具有相同ID的其他请求，如果是这种情况，则只对原始内部命令进行排队。
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<R> Handle(IdentifiedCommand<T, R> request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("");
            throw new System.NotImplementedException();
        }


        /// <summary>
        /// 如果前一个command已经处理了，直接返回
        /// 防止重复command
        /// </summary>
        /// <returns></returns>
        protected virtual R CreateResultForDuplicateRequest()
        {
            return default(R);
        }




    }
}