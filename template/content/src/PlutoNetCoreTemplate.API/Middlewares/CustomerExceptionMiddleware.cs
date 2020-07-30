using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PlutoNetCoreTemplate.Infrastructure.Providers;
using Serilog.Context;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PlutoNetCoreTemplate.API.Models;

namespace PlutoNetCoreTemplate.API.Middlewares
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

		private readonly ILogger _logger;


		public CustomerExceptionHandler(EventIdProvider eventIdProvider, RequestDelegate next,
		                                ILogger<CustomerExceptionHandler> logger)
		{
			_eventIdProvider = eventIdProvider;
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
				_logger.LogError(_eventIdProvider.EventId, e, $"{httpContext.Request.Path} has an error. {e.Message}");
				await HandlerExceptionAsync(httpContext, e);
			}
		}


		private async Task HandlerExceptionAsync(HttpContext context, Exception e)
		{
			context.Response.ContentType = "application/json;charset=utf-8";
			var message = e.Message;
			context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
			var apiResponse = ApiResponse.DefaultFail("服务器异常").AddDebugMessage(message);
			var serializerResult = JsonConvert.SerializeObject(apiResponse);
			await context.Response.WriteAsync(serializerResult);
		}
	}
}