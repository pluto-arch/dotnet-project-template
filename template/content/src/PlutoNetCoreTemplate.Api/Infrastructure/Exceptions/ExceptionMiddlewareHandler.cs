using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using System;
using System.Net;
using System.Threading.Tasks;

namespace PlutoNetCoreTemplate.Api
{
    using Microsoft.AspNetCore.Mvc;

    public class ExceptionMiddlewareHandler
    {
        private readonly RequestDelegate _next;

        private readonly ILogger _logger;


        public ExceptionMiddlewareHandler(RequestDelegate next,
                                        ILogger<ExceptionMiddlewareHandler> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{httpContext.Request.Path} has an error. {e.Message}");
                await HandlerExceptionAsync(httpContext, e);
            }
        }


        private static async Task HandlerExceptionAsync(HttpContext context, Exception e)
        {
            context.Response.ContentType = "application/json;charset=utf-8";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var traceId = context.TraceIdentifier;
            var apiResponse = ServiceResponse<dynamic>.Fatal($"服务异常",new {traceId=traceId});
            var serializeSetting = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var serializerResult = JsonConvert.SerializeObject(apiResponse, serializeSetting);
            await context.Response.WriteAsync(serializerResult);
        }
    }
}
