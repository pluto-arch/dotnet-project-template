namespace PlutoNetCoreTemplate.Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using AutoMapper;

    using MediatR;

    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyRegistrar
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
            services.AddQueries();
            return services;
        }

        public static IServiceCollection AddQueries(this IServiceCollection services)
        {
            var assembly= Assembly.GetExecutingAssembly();
            List<Type> list = (from c in assembly?.GetTypes()
                               where !c.IsInterface && c.Name.EndsWith("Queries")
                               select c).ToList();
            if (list == null)
            {
                return services;
            }

            foreach (Type item in list)
            {
                IEnumerable<Type> enumerable = from c in item.GetInterfaces()
                                               where c.Name.StartsWith("I") && c.Name.EndsWith("Queries")
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
    }
}
