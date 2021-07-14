namespace PlutoNetCoreTemplate.Api.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using PlutoNetCoreTemplate.Api.Extensions.Exceptions;
    using PlutoNetCoreTemplate.Api.Extensions.Tenant;

    public static class AppBuilder
    {
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


        #region 事件总线 订阅信息  service.AddApplicationLayer 中 注入总线，然后在这里配置对应的订阅者
        /**********************************
        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder Subscription(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            // 订阅普通事件 routeKey为 DisableUseIntegrationEvent
            eventBus.Subscribe<DisableUseIntegrationEvent, DisableUserIntegrationEventHandler>();
            // 订阅动态事件  参数为routeKey
            eventBus.SubscribeDynamic<DisableUserIntegrationDynamicEventHandler>("DisableUseIntegrationDynamicEvent");

            //// 示例 ：广播模式 添加消费者，共同工作
            //// 订单支付成功  语音播报
            //eventBus.ListeningDynamic<DisableUserIntegrationDynamicEventHandler>("OrderPayment", "voice_announcement");
            //// 订单支付成功  小票打印
            //eventBus.ListeningDynamic<DisableUserIntegrationDynamicEventHandler>("OrderPayment", "ticket_print");
            return app;
        }
        **************************************/
        #endregion


    }
}
