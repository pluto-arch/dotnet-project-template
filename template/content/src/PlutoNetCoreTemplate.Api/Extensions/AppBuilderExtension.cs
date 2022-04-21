namespace PlutoNetCoreTemplate.Api.Extensions
{
    using Infrastructure.UnitOfWork;

    public static class AppBuilderExtension
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseUnitOfWork(this IApplicationBuilder app)
        {
            app.UseMiddleware<UnitOfWorkMiddleware>();
            return app;
        }




        /// <summary>
        /// 请求日志
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseHttpRequestLogging(this IApplicationBuilder app)
        {
            app.UseSerilogRequestLogging(config =>
            {
                config.EnrichDiagnosticContext = (context, httpContext) =>
                {
                    if (httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                        context.Set("x_forwarded_for", httpContext.Request.Headers["X-Forwarded-For"]);
                    context.Set("request_path", httpContext.Request.Path);
                    context.Set("request_method", httpContext.Request.Method);
                };
            });
            return app;
        }


        /// <summary>
        /// 本地化
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCustomLocalization(this IApplicationBuilder app)
        {
            var supportCultures = new string[] { "zh-CN", "en-US" };
            var localizationOptions = new RequestLocalizationOptions
            {
                ApplyCurrentCultureToResponseHeaders = false,
            };
            localizationOptions.SetDefaultCulture(supportCultures.First())
                .AddSupportedCultures(supportCultures)
                .AddSupportedUICultures(supportCultures);
            app.UseRequestLocalization(localizationOptions);
            return app;
        }



        /// <summary>
        /// Swagger
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlutoNetCoreTemplate.API v1"));
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
