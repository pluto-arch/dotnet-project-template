using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Pluto.netcoreTemplate.Infrastructure.Providers;
using Serilog.Context;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pluto.netcoreTemplate.API.Models;

namespace Pluto.netcoreTemplate.API.Middlewares
{
    /// <summary>
    /// 
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseExceptionProcess(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomerExceptionHandler>();
            return app;
        }
    }

    internal class CustomerExceptionHandler
    {
        private readonly RequestDelegate _next;

        private readonly EventIdProvider _eventIdProvider;

        private readonly IDictionary<int, string> _exceptionStatusCodeDic;

        private readonly ILogger logger;


        public CustomerExceptionHandler(EventIdProvider eventIdProvider, RequestDelegate next, ILogger<CustomerExceptionHandler> logger)
        {
            _eventIdProvider = eventIdProvider;
            _next = next;
            this.logger = logger;

            _exceptionStatusCodeDic = new Dictionary<int, string>
            {
                { 401, "未授权的请求" },
                { 404, "找不到该页面" },
                { 403, "访问被拒绝" },
                { 500, "服务器发生意外的错误" }
            };

        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            using (LogContext.PushProperty("Event", _eventIdProvider.EventId.ToString()))
            {
                try
                {
                    await _next.Invoke(httpContext);
                }
                catch (Exception e)
                {
                    logger.LogError(_eventIdProvider.EventId,e, $"{httpContext.Request.Path} has an error. {e.Message}");
                    await HandlerExceptionAsync(httpContext, e);
                }
            }
        }


        private async Task HandlerExceptionAsync(HttpContext context, Exception e)
        {
            context.Response.StatusCode = StatusCodes.Status200OK;
            context.Response.ContentType = "application/json;charset=utf-8";
            string message = e.Message;
            if (_exceptionStatusCodeDic.ContainsKey(context.Response.StatusCode))
            {
                message = _exceptionStatusCodeDic[context.Response.StatusCode];
            }
            var apiResponse = ApiResponse.DefaultFail(message);
            var serializerResult = JsonConvert.SerializeObject(apiResponse);
            await context.Response.WriteAsync(serializerResult);
        }

    }
}