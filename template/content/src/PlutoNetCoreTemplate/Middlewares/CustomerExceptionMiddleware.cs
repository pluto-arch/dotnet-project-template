using System;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace PlutoNetCoreTemplate.Middlewares
{
    using System.Runtime.Serialization.Formatters;
    using Infrastructure.Commons;
    using Newtonsoft.Json.Serialization;

    /// <summary>
	/// 
	/// </summary>
	public static class ApplicationBuilderExtensions
	{
		/// <summary>
		/// 记录http上下文的中间件
		/// </summary>
		/// <param name="builder"></param>
		/// <returns></returns>
        public static IApplicationBuilder UseHttpContextLog(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpContextLogMiddleware>();
        }

		/// <summary>
		/// 异常处理中间件
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

		private readonly ILogger _logger;


		public CustomerExceptionHandler(RequestDelegate next,
		                                ILogger<CustomerExceptionHandler> logger)
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
			context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            var traceId=context.TraceIdentifier;
			var apiResponse = ServiceResponse<string>.Failure($"服务异常:{traceId}");
            var serializeSetting=new JsonSerializerSettings
                                 {
                                     NullValueHandling = NullValueHandling.Ignore,
                                     DateFormatString = "yyyy-MM-dd HH:mm:ss",
                                     ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                     ContractResolver = new CamelCasePropertyNamesContractResolver()
                                 };
			var serializerResult = JsonConvert.SerializeObject(apiResponse,serializeSetting);
			await context.Response.WriteAsync(serializerResult);
		}
	}
}