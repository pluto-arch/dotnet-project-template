namespace PlutoNetCoreTemplate.Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Behaviors;
    using EventBus;
    using EventBus.Abstractions;
    using EventBus.RabbitMQ;
    using IntegrationEvent.EventHandler;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Permissions;
    using RabbitMQ.Client;

    public static class DependencyRegistrar
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            services.AddTransient(typeof(IPipelineBehavior<,>),typeof(AutoSaveBehavior<,>));

            services.AddTransient(typeof(IPipelineBehavior<,>),typeof(TransactionBehaviour<,>));

            services.AddAppServices();

            services.AddSingleton<IPermissionDefinitionProvider, ProductPermissionDefinitionProvider>();
            services.AddSingleton<IPermissionDefinitionProvider, TenantPermissionDefinitionProvider>();
            services.AddSingleton<IPermissionDefinitionManager, PermissionDefinitionManager>();
            services.AddTransient<IPermissionStore, PermissionStore>();
            services.AddTransient<IPermissionValueProvider, RolePermissionValueProvider>();
            services.AddTransient<IPermissionValueProvider, UserPermissionValueProvider>();

            /*
             * event bus
             */
            //services.AddEventBusRabbitMQ();

            return services;
        }

        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            var assembly= Assembly.GetExecutingAssembly();
            List<Type> list = (from c in assembly?.GetTypes()
                               where !c.IsInterface && c.Name.EndsWith("AppService")
                               select c).ToList();
            if (list == null)
            {
                return services;
            }

            foreach (Type item in list)
            {
                IEnumerable<Type> enumerable = from c in item.GetInterfaces()
                                               where c.Name.StartsWith("I") && c.Name.EndsWith("AppService")
                                               select c;
                if (!enumerable.Any())
                {
                    continue;
                }

                foreach (Type item2 in enumerable)
                {
                    services.AddScoped(item2, item);
                }
            }
            return services;
        }

        /// <summary>
        /// 添加rabbitmq eventBus 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddEventBusRabbitMQ(this IServiceCollection services)
        {
            // 注入 mq链接对象
            services.AddSingleton<IRabbitMqPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMqPersistentConnection>>();
                var factory = new ConnectionFactory()
                {
                    HostName = "",
                    DispatchConsumersAsync = true,
                    Port=5672
                };
                factory.UserName ="guest";
                factory.Password = "guest";
                var retryCount = 5;
                return new DefaultRabbitMqPersistentConnection(factory, logger, retryCount);
            });

            // 注册内存订阅管理
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();


            // 注册mq事件总线
            services.AddSingleton<IEventBus, EventBusRabbitMq>(sp =>
            {
                var rabbitMqPersistentConnection = sp.GetRequiredService<IRabbitMqPersistentConnection>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMq>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                var retryCount = 5;
                return new EventBusRabbitMq(
                    sp,
                    logger,
                    directExchangeName: "directExchange",
                    fanoutExchangeName: "fanoutExchange",
                    directQueueName: "PlutoNetCoreTemplate_Application",
                    retryCount);
            });

            // 注册事件处理程序
            services.AddTransient<DisableUserIntegrationEventHandler>();
            services.AddTransient<DisableUserIntegrationDynamicEventHandler>();
            return services;
        }

    }
}
