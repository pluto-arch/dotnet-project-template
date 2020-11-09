using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PlutoNetCoreTemplate.Extensions;
using Serilog.Context;

namespace PlutoNetCoreTemplate.Middlewares
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
            using (LogContext.Push(new HttpContextEnricher(serviceProvider)))
            {
                await _next(context);
            }
        }
    }
}