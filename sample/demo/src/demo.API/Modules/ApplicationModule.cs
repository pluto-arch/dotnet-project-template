using System;
using Autofac;

namespace Demo.API.Modules
{
    public class ApplicationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(t => t.Name.EndsWith("Queries"))
                .AsImplementedInterfaces().InstancePerLifetimeScope();

        }
    }
}