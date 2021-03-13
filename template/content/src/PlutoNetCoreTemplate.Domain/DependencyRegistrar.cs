using System;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using PlutoNetCoreTemplate.Domain.Aggregates.TenantAggregate;
using PlutoNetCoreTemplate.Domain.SeedWork;
using PlutoNetCoreTemplate.Domain.Services.Account;

namespace PlutoNetCoreTemplate.Domain
{
    public static class DependencyRegistrar
    {
        public static IServiceCollection AddDomainLayer(this IServiceCollection services)
        {
            services.AddTransient<ISystemDomainService, SystemDomainService>();
            services.AddSingleton<ICurrentTenantAccessor, CurrentTenantAccessor>();
            services.AddTransient<ICurrentTenant, CurrentTenant>();


            var dataSeedProviders = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.ExportedTypes).Where(t => t.IsAssignableTo(typeof(IDataSeedProvider)) && t.IsClass);
            dataSeedProviders.ToList().ForEach(t => services.AddTransient(typeof(IDataSeedProvider), t));

            return services;
        }
    }
}
