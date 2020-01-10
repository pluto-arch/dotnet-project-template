using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Pluto.netcoreTemplate.Infrastructure.Providers;

namespace Pluto.netcoreTemplate.Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        private readonly EventIdProvider _eventIdProvider;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger, EventIdProvider eventIdProvider)
        {
            _logger = logger;
            _eventIdProvider = eventIdProvider;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation(_eventIdProvider.EventId,"-----处理命令： {CommandName} ({@Command})", request.GetType().Name, request);
            var response = await next();
            _logger.LogInformation(_eventIdProvider.EventId, "-----命令：{CommandName} 处理完成 - response: {@Response}", request.GetType().Name, response);
            return response;
        }
    }
}