namespace PlutoNetCoreTemplate.ServeHost.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Builder;

    using PlutoNetCoreTemplate.ServeHost.Middlewares;

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
    }
}
