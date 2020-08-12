using System;
using MediatR;

using Microsoft.Extensions.Logging;

using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PlutoNetCoreTemplate.Application.CommandBus.Commands;
using PlutoNetCoreTemplate.Infrastructure.Exceptions;
using PlutoNetCoreTemplate.Infrastructure.Extensions;
using PlutoNetCoreTemplate.Infrastructure.Idempotency;

namespace PlutoNetCoreTemplate.Application.CommandBus.IdentityCommand
{
    public class IdentifiedCommandHandler<T, R> : IRequestHandler<IdentifiedCommand<T, R>, R>
        where T : IRequest<R>
    {

        private readonly IMediator _mediator;
        private readonly IRequestManager _requestManager;
        private readonly ILogger<IdentifiedCommandHandler<T, R>> _logger;

        public IdentifiedCommandHandler(
            ILogger<IdentifiedCommandHandler<T, R>> logger, 
            IRequestManager requestManager, 
            IMediator mediator)
        {
            _logger = logger;
            _requestManager = requestManager;
            _mediator = mediator;
        }

        /// <summary>
        /// 这个方法处理命令。它只是确保不存在具有相同ID的其他请求
        /// 重复执行将返回默认值
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<R> Handle(IdentifiedCommand<T, R> message, CancellationToken cancellationToken)
        {
            var command = message.Command;
            var commandName = command.GetGenericTypeName();
            try
            {
                await _requestManager.CreateRequestForCommandAsync<T>(message.Id, JsonConvert.SerializeObject(command));
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (RepeatedCommandException e)
            {
                _logger.LogError(e, "命令重复执行:  {CommandName}: ({@Command})",
                                 commandName,
                                 command);
                return CreateResultForDuplicateRequest();
            }
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