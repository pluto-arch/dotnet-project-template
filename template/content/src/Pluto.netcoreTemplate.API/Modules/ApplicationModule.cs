using Autofac;
using Pluto.netcoreTemplate.Application.Queries.UserQueries;
using Pluto.netcoreTemplate.Domain.Entities.UserAggregate;
using Pluto.netcoreTemplate.Infrastructure.Repositories;

namespace Pluto.netcoreTemplate.API.Modules
{
    public class ApplicationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UserQuery>().InstancePerLifetimeScope();
        }
    }
}