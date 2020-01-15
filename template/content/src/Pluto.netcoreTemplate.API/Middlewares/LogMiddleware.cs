using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using Pluto.netcoreTemplate.Infrastructure.Providers;

using Serilog.Context;

using System.Threading.Tasks;

namespace Pluto.netcoreTemplate.API.Middlewares
{
    public static class LogExtensions
    {
        public static IApplicationBuilder UseLogProcess(this IApplicationBuilder app)
        {
            app.UseMiddleware<LogProcessMiddleware>();
            return app;
        }
    }


    class LogProcessMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly EventIdProvider _eventIdProvider;

        public LogProcessMiddleware(EventIdProvider eventIdProvider, RequestDelegate next)
        {
            _eventIdProvider = eventIdProvider;
            _next = next;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            using (LogContext.PushProperty("Event", _eventIdProvider.EventId.ToString()))
            {
                await _next.Invoke(httpContext);
            }
        }
    }
}