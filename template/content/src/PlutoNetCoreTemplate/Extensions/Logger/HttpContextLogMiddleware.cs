using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog.Context;


namespace PlutoNetCoreTemplate.Api.Extensions.Logger
{
    /// <summary>
    /// 扩展日志中间件
    /// </summary>
    public class HttpContextLogMiddleware
    {
        private readonly RequestDelegate _next;

        public HttpContextLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var serviceProvider = context.RequestServices;
            using (LogContext.Push(new ServiceTraceIdEnricher(serviceProvider)))
            {
                await _next(context);
            }
        }
    }
}
