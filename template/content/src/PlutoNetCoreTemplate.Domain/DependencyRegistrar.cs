using Microsoft.Extensions.DependencyInjection;

using PlutoNetCoreTemplate.Domain.SeedWork;

using System;
using System.Linq;

namespace PlutoNetCoreTemplate.Domain
{
    public static class DependencyRegistrar
    {

        public static IServiceCollection AddDomainLayer(this IServiceCollection services)
        {
            var dataSeedProviders = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.ExportedTypes).Where(t => t.IsAssignableTo(typeof(IDataSeedProvider)) && t.IsClass);
            dataSeedProviders.ToList().ForEach(t => services.AddTransient(typeof(IDataSeedProvider), t));
            return services;
        }


        public static IServiceCollection AddDomainLayerNoSeed(this IServiceCollection services)
        {
            return services;
        }
    }
}
