using System;
using System.Runtime.InteropServices.ComTypes;
using MediatR;
using Microsoft.Extensions.Logging;
using PlutoNetCoreTemplate.Infrastructure.Providers;
using System.Threading;
using System.Threading.Tasks;
using Serilog.Context;

namespace PlutoNetCoreTemplate.Application.Behaviors
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        private readonly EventIdProvider _eventIdProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="eventIdProvider"></param>
        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger, EventIdProvider eventIdProvider)
        {
            _logger = logger;
            _eventIdProvider = eventIdProvider;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation(_eventIdProvider.EventId, "执行{@command}：{@Command}",request.GetType().Name, request);
            var response = await next();
            _logger.LogInformation(_eventIdProvider.EventId, "执行{@command} result: {@Response}",request.GetType().Name, response);
            return response;
        }
    }
}