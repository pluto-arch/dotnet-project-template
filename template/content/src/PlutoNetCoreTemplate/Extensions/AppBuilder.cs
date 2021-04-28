namespace PlutoNetCoreTemplate.Extensions
{
    using Application.IntegrationEvent.Event;
    using Application.IntegrationEvent.EventHandler;
    using EventBus.Abstractions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
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

        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder Subscription(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            // 订阅普通事件 routeKey为 DisableUseIntegrationEvent
            eventBus.Subscribe<DisableUseIntegrationEvent,DisableUserIntegrationEventHandler>();
            // 订阅动态事件  参数为routeKey
            eventBus.SubscribeDynamic<DisableUserIntegrationDynamicEventHandler>("DisableUseIntegrationDynamicEvent");
            return app;
        }
    }
}
