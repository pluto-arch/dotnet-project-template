namespace PlutoNetCoreTemplate.Infrastructure
{
    using ConnectionString;
    using Constants;
    using Domain.Aggregates.TenantAggregate;
    using Domain.Entities;
    using Domain.Repositories;
    using Domain.SeedWork;
    using Domain.UnitOfWork;
    using EntityFrameworkCore;
    using EntityFrameworkCore.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using PlutoNetCoreTemplate.Domain.Aggregates.ProductAggregate;
    using PlutoNetCoreTemplate.Infrastructure.Providers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class DependencyRegistrar
    {
        public static IServiceCollection AddTenantComponent(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TenantConfigurationOptions>(configuration);
            services.AddTransient<IConnectionStringProvider, TenantConnectionStringProvider>();
            services.AddSingleton<ICurrentTenantAccessor, CurrentTenantAccessor>();
            services.AddTransient<ICurrentTenant, CurrentTenant>();
            services.AddTransient<ITenantProvider, EFCoreTenantProvider>();
            return services;
        }


        public static IServiceCollection AddInfrastructureLayer(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<ILazyLoadServiceProvider, NativeLazyLoadServiceProvider>();
            services.AddTransient<IDomainEventDispatcher, MediatrDomainEventDispatcher>();
            services.AddEntityFrameworkSqlServer();
            services.AddDbContextPool<DeviceCenterDbContext>((serviceProvider, optionsBuilder) =>
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString(DbConstants.DefaultConnectionStringName), sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
                });
                var mediator = serviceProvider.GetService<IDomainEventDispatcher>() ?? NullDomainEventDispatcher.Instance;
                optionsBuilder.AddInterceptors(new CustomSaveChangesInterceptor(mediator));

#if DEBUG
                optionsBuilder.EnableSensitiveDataLogging();
#endif

                // 多租户模式下解析租户连接字符串使用
                var connectionStringProvider = serviceProvider.GetRequiredService<IConnectionStringProvider>();
                optionsBuilder.AddInterceptors(new TenantDbConnectionInterceptor(connectionStringProvider, DbConstants.DefaultConnectionStringName));

                ////分表使用 - 替换ef的缓存，造成性能会下降
                //optionsBuilder.ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactory>();

                optionsBuilder.UseInternalServiceProvider(serviceProvider);
            });

            services.AddDbContextPool<SystemDbContext>((serviceProvider, optionsBuilder) =>
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString(DbConstants.SystemConnectionStringName), sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
                });
                var mediator = serviceProvider.GetService<IDomainEventDispatcher>();
                optionsBuilder.AddInterceptors(new CustomSaveChangesInterceptor(mediator));

#if DEBUG
                optionsBuilder.EnableSensitiveDataLogging();
#endif


                optionsBuilder.UseInternalServiceProvider(serviceProvider);
            });


            // 设置实体默认显示加载的导航属性
            services.Configure<IncludeRelatedPropertiesOptions>(options =>
            {
                options.ConfigIncludes<Product>(e => e.Include(e => e.Devices).ThenInclude(e => e.Address));
            });

            var assembly = Assembly.GetExecutingAssembly();
            var context = assembly.GetTypes().Where(x => x.GetInterface(nameof(IUowDbContext)) != null && !x.Name.Contains("Migration")).ToList();
            services.AddScoped(typeof(IUnitOfWork<>), typeof(EFCoreUnitOfWork<>));
            services.Configure<UnitOfWorkCollectionOptions>(s =>
            {
                foreach (var item in context)
                {
                    var uowType = typeof(IUnitOfWork<>).MakeGenericType(item);
                    s.DbContexts.Add(item.Name, uowType);
                }
            });
            services.AddDefaultRepository(assembly, context);
            services.AddRepository(assembly);

            return services;
        }


        public static void AddRepository(this IServiceCollection services, Assembly assembly = null)
        {
            var implTypes = assembly?.GetTypes().Where(c => !c.IsInterface && c.Name.EndsWith("Repository")).ToList();
            if (implTypes == null)
                return;

            foreach (var impltype in implTypes)
            {
                var interfaces = impltype.GetInterfaces()
                    .Where(c => c.Name.StartsWith("I") && c.Name.EndsWith("Repository"));
                IEnumerable<Type> enumerable = interfaces as Type[] ?? interfaces.ToArray();
                if (!enumerable.Any())
                    continue;
                foreach (var inter in enumerable)
                {
                    services.AddTransient(inter, impltype);
                }
            }
        }

        public static void AddDefaultRepository(this IServiceCollection services, Assembly assembly = null, List<Type> context = null)
        {
            assembly ??= Assembly.GetExecutingAssembly();
            context ??= assembly.GetTypes().Where(x => x.GetInterface(nameof(IUowDbContext)) != null).ToList();
            if (context is null or { Count: <= 0 })
            {
                return;
            }
            foreach (var item in context)
            {
                var properties = item.GetProperties().Where(x => x.PropertyType.IsGenericType);
                foreach (var p in properties)
                {
                    var entityType = p.PropertyType.GenericTypeArguments.FirstOrDefault(x => x.IsAssignableTo(typeof(BaseEntity)));
                    if (entityType == null)
                    {
                        continue;
                    }
                    var baseImpl = typeof(EFCoreRepository<,>).MakeGenericType(item, entityType);
                    var baseRep = typeof(IRepository<>).MakeGenericType(entityType);
                    services.RegisterType(baseRep, baseImpl);
                    var primaryKeyType = EntityHelper.FindPrimaryKeyType(entityType);
                    if (primaryKeyType != null)
                    {
                        var keyImpl = typeof(EFCoreRepository<,,>).MakeGenericType(item, entityType, primaryKeyType);
                        var keyRep = typeof(IRepository<,>).MakeGenericType(entityType, primaryKeyType);
                        services.RegisterType(keyRep, keyImpl);
                    }

                }
            }
        }

        public static IServiceCollection RegisterType(this IServiceCollection services, Type type, Type implementationType)
        {
            if (type.IsAssignableFrom(implementationType))
            {
                services.TryAddTransient(type, implementationType);
            }
            return services;
        }

    }
}
