using System;
using System.Runtime.InteropServices.ComTypes;
using MediatR;
using Microsoft.Extensions.Logging;
using Pluto.netcoreTemplate.Infrastructure.Providers;
using System.Threading;
using System.Threading.Tasks;
using Serilog.Context;

namespace Pluto.netcoreTemplate.Application.Behaviors
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
            using (LogContext.PushProperty("CommondHandler", typeof(TRequest) + " Handler"))
            {
                try
                {
                    _logger.LogInformation(_eventIdProvider.EventId, "command：{@Command}", request);
                    var response = await next();
                    _logger.LogInformation(_eventIdProvider.EventId, "command result: {@Response}", response);
                    return response;
                }
                catch (Exception e)
                {
                    _logger.LogError(_eventIdProvider.EventId, e, $"{typeof(TRequest)} handler error ：{e.Message}");
                    return default;
                }
            }
        }
    }
}