namespace PlutoNetCoreTemplate.Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using AutoMapper;
    using Behaviors;
    using MediatR;

    using Microsoft.Extensions.DependencyInjection;
    using Permissions;

    public static class DependencyRegistrar
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            services.AddTransient(typeof(IPipelineBehavior<,>),typeof(AutoSaveBehavior<,>));

            services.AddTransient(typeof(IPipelineBehavior<,>),typeof(TransactionBehaviour<,>));

            services.AddAppServices();

            services.AddSingleton<IPermissionDefinitionProvider, PlutoNetCoreTemplatePermissionDefinitionProvider>();
            services.AddSingleton<IPermissionDefinitionManager, PermissionDefinitionManager>();
            services.AddTransient<IPermissionStore, PermissionStore>();
            services.AddTransient<IPermissionValueProvider, RolePermissionValueProvider>();

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
    }
}
