using System;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PlutoNetCoreTemplate.Models;

namespace PlutoNetCoreTemplate.Middlewares
{
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


		private async Task HandlerExceptionAsync(HttpContext context, Exception e)
		{
			context.Response.ContentType = "application/json;charset=utf-8";
			context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
			var apiResponse = ApiResponse.Error(e.Message);
			var serializerResult = JsonConvert.SerializeObject(apiResponse);
			await context.Response.WriteAsync(serializerResult);
		}
	}
}