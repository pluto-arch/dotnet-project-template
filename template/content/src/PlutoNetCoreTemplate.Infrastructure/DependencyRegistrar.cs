namespace PlutoNetCoreTemplate.Infrastructure
{
    using System;
    using System.Linq;
    using System.Reflection;
    using DapperCore;
    using Domain.SeedWork;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using PlutoData;
    using PlutoData.Enums;
    using PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore;
    using PlutoNetCoreTemplate.Infrastructure.Idempotency;

    public static class DependencyRegistrar
    {
        public static IServiceCollection AddInfrastructureLayer(
            this IServiceCollection services, 
            IConfiguration configuration, 
            Action<DbContextOptionsBuilder> options)
        {

            services.AddDbContext<EfCoreDbContext>(options)
                .AddEfUnitOfWork<EfCoreDbContext>();

            services.AddDapperDbContext<PlutoNetCoreDapperDbContext>(op =>
                {
                    op.DependOnEf = true;
                    op.DbType = EnumDbType.SQLServer;
                    op.EfDbContextType = typeof(EfCoreDbContext);
                })
                .AddDapperUnitOfWork<PlutoNetCoreDapperDbContext>();


            services.AddTransient<IRequestManager, RequestManager>();
            services.AddTransient(typeof(IPlutoNetCoreTemplateEfRepository<>),typeof(PlutoNetCoreTemplateEfRepository<>));
            services.AddTransient(typeof(IPlutoNetCoreTemplateDapperRepository<>),typeof(PlutoNetCoreTemplateDapperRepository<>));
            services.AddRepository(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
