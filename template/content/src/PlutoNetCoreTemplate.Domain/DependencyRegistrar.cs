using Microsoft.Extensions.DependencyInjection;

using PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate;
using PlutoNetCoreTemplate.Domain.SeedWork;

using System;
using System.Linq;

namespace PlutoNetCoreTemplate.Domain
{
    using Services.TenantDomainService;

    public static class DependencyRegistrar
    {
        public static IServiceCollection AddDomainLayer(this IServiceCollection services)
        {
            services.AddSingleton<ICurrentTenantAccessor, CurrentTenantAccessor>();
            services.AddTransient<ICurrentTenant, CurrentTenant>();
            services.AddTransient<ITenantProvider, TenantProvider>();
            services.AddTransient<TenantManager>();

            var dataSeedProviders = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.ExportedTypes).Where(t => t.IsAssignableTo(typeof(IDataSeedProvider)) && t.IsClass);
            dataSeedProviders.ToList().ForEach(t => services.AddTransient(typeof(IDataSeedProvider), t));

            return services;
        }


        public static IServiceCollection AddDomainLayerNoSeed(this IServiceCollection services)
        {
            services.AddSingleton<ICurrentTenantAccessor, CurrentTenantAccessor>();
            services.AddTransient<ICurrentTenant, CurrentTenant>();
            services.AddTransient<ITenantProvider, TenantProvider>();
            services.AddTransient<TenantManager>();

            return services;
        }

    }
}
