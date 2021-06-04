namespace PlutoNetCoreTemplate.Infrastructure
{
    using System;
    using System.Reflection;
    using ConnectionString;
    using Constants;
    using Domain.SeedWork;
    using global::EntityFrameworkCore.Extension;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore;
    using PlutoNetCoreTemplate.Infrastructure.Idempotency;
    using Providers;
    using Serilog;

    public static class DependencyRegistrar
    {
        public static IServiceCollection AddInfrastructureLayer(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddTransient<IConnectionStringProvider, TenantConnectionStringProvider>();
            services.AddDbContext<PlutoNetTemplateDbContext>((serviceProvider, optionsBuilder) =>
                {
                    optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddFilter((category, level) =>
                            category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                        .AddSerilog()));

                    optionsBuilder.UseSqlServer(configuration.GetConnectionString(DbConstants.DefaultConnectionStringName),
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(Assembly.GetEntryAssembly()?.GetName().Name);
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 5,
                                maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorNumbersToAdd: null);
                        });

                    optionsBuilder.EnableSensitiveDataLogging();

                    IMediator mediator = serviceProvider.GetService<IMediator>() ?? new NullMediator();
                    optionsBuilder.AddInterceptors(new CustomSaveChangeInterceptor(mediator));

                    var connectionStringProvider = serviceProvider.GetRequiredService<IConnectionStringProvider>();
                    optionsBuilder.AddInterceptors(new TenantDbConnectionInterceptor(connectionStringProvider));

                })
                .AddEfUnitOfWork<PlutoNetTemplateDbContext>();


            services.AddDbContext<TenantDbContext>((serviceProvider, optionsBuilder) =>
                {
                    optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddFilter((category, level) =>
                            category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                        .AddSerilog()));
                    optionsBuilder.UseSqlServer(configuration.GetConnectionString(DbConstants.TenantConnectionStringName),
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(Assembly.GetEntryAssembly()?.GetName().Name);
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 5,
                                maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorNumbersToAdd: null);
                        });
                })
                .AddEfUnitOfWork<TenantDbContext>();


            services.AddTransient<IRequestManager, RequestManager>();
            services.AddTransient(typeof(IPlutoNetCoreTemplateBaseRepository<>),typeof(PlutoNetCoreTemplateBaseRepository<>));
            services.AddTransient(typeof(ITenantRepository<>),typeof(TenantRepository<>));
            services.AddRepository(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
