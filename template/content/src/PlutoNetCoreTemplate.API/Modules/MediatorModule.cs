using Autofac;
using MediatR;
using PlutoNetCoreTemplate.Application.Behaviors;
using System.Reflection;
using PlutoNetCoreTemplate.Application.CommandBus.Commands;
using PlutoNetCoreTemplate.Application.EventBus.Users;
using PlutoNetCoreTemplate.Infrastructure.Idempotency;

namespace PlutoNetCoreTemplate.API.Modules
{
    public class MediatorModule : Autofac.Module
    {
        /*
         * https://github.com/jbogard/MediatR/wiki
         */
        protected override void Load(ContainerBuilder builder)
        {

            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();
            
            builder.RegisterAssemblyTypes(typeof(BaseCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>)).InstancePerDependency();


            builder.RegisterAssemblyTypes(typeof(DisableUserEventHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(INotificationHandler<>));


            builder.Register<ServiceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
            });


            builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>)).InstancePerDependency(); 
            builder.RegisterGeneric(typeof(AutoSaveChangeBehavior<,>)).As(typeof(IPipelineBehavior<,>)).InstancePerDependency(); 
            builder.RegisterGeneric(typeof(TransactionBehaviour<,>)).As(typeof(IPipelineBehavior<,>)).InstancePerDependency(); 
            
            builder.RegisterType<RequestManager>().As<IRequestManager>().InstancePerDependency(); 
            
        }
    }
}