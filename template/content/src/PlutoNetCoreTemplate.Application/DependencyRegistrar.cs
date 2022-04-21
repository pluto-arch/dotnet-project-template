namespace PlutoNetCoreTemplate.Application
{
    using Behaviors;
    using Infrastructure.Idempotency;
    using Permissions;
    using System.Reflection;

    public static class DependencyRegistrar
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
            services.AddTransient<IRequestManager, RequestManager>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(IdentityCommandBehaviour<,>));
            services.AddAppServices();

            services.AddPermissionDefinitionProvider();
            services.AddScoped<IPermissionChecker, PermissionChecker>();
            services.AddSingleton<IPermissionDefinitionManager, PermissionDefinitionManager>();
            services.AddTransient<IPermissionStore, PermissionStore>();
            services.AddPermissionValueProvider();

            //event bus
            //services.AddEventBusRabbitMQ();
            // OR
            //services.AddEventBusInMemory();

            return services;
        }

        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
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
                    services.AddTransient(item2, item);
                }
            }
            return services;
        }


        public static IServiceCollection AddPermissionDefinitionProvider(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            List<Type> list = (from c in assembly?.GetTypes()
                               where !c.IsInterface && c.Name.EndsWith("PermissionDefinitionProvider")
                               select c).ToList();
            if (list == null)
            {
                return services;
            }

            foreach (Type item in list)
            {
                IEnumerable<Type> enumerable = from c in item.GetInterfaces()
                                               where c.Name.StartsWith("I") && c.Name.EndsWith("PermissionDefinitionProvider")
                                               select c;
                if (!enumerable.Any())
                {
                    continue;
                }

                foreach (Type item2 in enumerable)
                {
                    services.AddSingleton(item2, item);
                }
            }
            return services;
        }


        public static IServiceCollection AddPermissionValueProvider(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            List<Type> list = (from c in assembly?.GetTypes()
                               where !c.IsInterface && c.Name.EndsWith("PermissionValueProvider")
                               select c).ToList();
            if (list == null)
            {
                return services;
            }

            foreach (Type item in list)
            {
                IEnumerable<Type> enumerable = from c in item.GetInterfaces()
                                               where c.Name.StartsWith("I") && c.Name.EndsWith("PermissionValueProvider")
                                               select c;
                if (!enumerable.Any())
                {
                    continue;
                }

                foreach (Type item2 in enumerable)
                {
                    services.AddTransient(item2, item);
                }
            }
            return services;
        }


        #region 事件总线  需引用BuildingBlocks中的 EventBus.RabbitMQ 或者 EventBus.InMemory
        /************************************************************************
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
                    Port = 5672
                };
                factory.UserName = "guest";
                factory.Password = "guest";
                var retryCount = 5;
                return new DefaultRabbitMqPersistentConnection(factory, logger, retryCount);
            });
            // 注册内存订阅管理
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            // 注册mq事件总线
            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var rabbitMqPersistentConnection = sp.GetRequiredService<IRabbitMqPersistentConnection>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                var retryCount = 5;
                var bus = new EventBusRabbitMQ(
                    sp,
                    logger,
                    directExchangeName: "directExchange",
                    fanoutExchangeName: "fanoutExchange",
                    directQueueName: "PlutoNetCoreTemplate_Application",
                    retryCount);
                return bus;
            });
            // 注册事件处理程序
            services.AddTransient<DisableUserIntegrationEventHandler>();
            services.AddTransient<DisableUserIntegrationDynamicEventHandler>();
            return services;
        }


        /// <summary>
        /// 添加eventBus in memoryQueue
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddEventBusInMemory(this IServiceCollection services)
        {
            // 注册内存订阅管理
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            // 注册mq事件总线
            services.AddSingleton<IEventBus, EventBusMemoryQueue>(sp => {
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                var bus = new EventBusMemoryQueue(sp, eventBusSubcriptionsManager);
                return new EventBusMemoryQueue(sp, eventBusSubcriptionsManager);
            });
            // 注册事件处理程序
            services.AddTransient<DisableUserIntegrationEventHandler>();
            services.AddTransient<DisableUserIntegrationDynamicEventHandler>();
            return services;
        }

        *********************************************************************************/
        #endregion

    }
}
