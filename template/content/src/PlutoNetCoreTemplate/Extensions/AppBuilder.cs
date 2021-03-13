namespace PlutoNetCoreTemplate.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using PlutoNetCoreTemplate.Extensions.Exceptions;
    using PlutoNetCoreTemplate.Extensions.Logger;
    using PlutoNetCoreTemplate.Extensions.Tenant;

    public static class AppBuilder
    {
        /// <summary>
		/// 记录http上下文的中间件
		/// </summary>
		/// <param name="builder"></param>
		/// <returns></returns>
        public static IApplicationBuilder UseHttpContextLog(this IApplicationBuilder builder)
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
            app.UseMiddleware<ExceptionMiddlewareHandler>();
            return app;
        }

        /// <summary>
        /// 多租户
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseTenant(this IApplicationBuilder app)
        {
            app.UseMiddleware<TenantMiddleware>();
            return app;
        }
    }
}
