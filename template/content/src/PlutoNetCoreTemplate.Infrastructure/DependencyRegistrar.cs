namespace PlutoNetCoreTemplate.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using PlutoData;
    using PlutoData.Interface;

    using PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore;
    using PlutoNetCoreTemplate.Infrastructure.Idempotency;

    public static class DependencyRegistrar
    {
        public static IServiceCollection AddInfrastructureLayer(
            this IServiceCollection services, 
            IConfiguration configuration, 
            Action<DbContextOptionsBuilder> options)
        {

            services.AddHybridUnitOfWork<EfCoreDbContext>(options);
            services.AddTransient<IRequestManager, RequestManager>();
            services.AddScoped(typeof(IEfRepository<>),typeof(PlutoNetCoreTemplateEfRepository<>));
            services.AddRepository(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
