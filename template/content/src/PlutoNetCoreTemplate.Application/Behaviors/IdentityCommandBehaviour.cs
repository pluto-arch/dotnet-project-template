namespace PlutoNetCoreTemplate.Application.Behaviors
{
    using System.Reflection;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using PlutoNetCoreTemplate.Application.Command;
    using PlutoNetCoreTemplate.Infrastructure.Commons;
    using PlutoNetCoreTemplate.Infrastructure.Exceptions;
    using PlutoNetCoreTemplate.Infrastructure.Extensions;
    using PlutoNetCoreTemplate.Infrastructure.Idempotency;

    public class IdentityCommandBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : BaseCommand<TResponse>
    {

        private readonly IRequestManager _requestManager;
        private readonly ILogger<IdentityCommandBehaviour<TRequest, TResponse>> _logger;
        public IdentityCommandBehaviour(IRequestManager requestManager, ILogger<IdentityCommandBehaviour<TRequest, TResponse>> logger)
        {
            _requestManager = requestManager;
            _logger = logger;
        }



        /// <inheritdoc />
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var typeName = request.GetGenericTypeName();
            var type = request.GetType();
            if (type.GetCustomAttribute(typeof(DisableIdentityCommandCheckAttribute), true) is DisableIdentityCommandCheckAttribute)
            {
                return await next();
            }

            try
            {
                if (await _requestManager.ExistAsync(request.Id))
                {
                    throw new RepeatedCommandException($"请勿重复执行：{request.Id}");
                }
                return await next();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "命令重复执行:  {CommandName}: ({@Command})", typeName, request);
                return CreateResultForDuplicateRequest();
            }
        }

        /// <summary>
        /// 如果前一个command已经处理了，直接返回
        /// 防止重复command
        /// </summary>
        /// <returns></returns>
        protected virtual TResponse CreateResultForDuplicateRequest()
        {
            return default;
        }
    }
}