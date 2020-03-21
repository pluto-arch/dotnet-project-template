using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using Pluto.netcoreTemplate.API;
using Pluto.netcoreTemplate.API.Controllers;
using Pluto.netcoreTemplate.API.Filters;
using Pluto.netcoreTemplate.API.Modules;
using Pluto.netcoreTemplate.Infrastructure;
using Pluto.netcoreTemplate.Infrastructure.Extensions;
using Pluto.netcoreTemplate.Infrastructure.Providers;
using Serilog;

namespace Pluto.netcoreTemplate.Test
{
    public class BaseTest
    {

        internal IContainer _Container;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();

            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            services.AddControllers(options =>
                {
                    options.Filters.Add<ModelValidateFilter>();
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddScoped(typeof(EventIdProvider));
            services
                .AddEntityFrameworkSqlServer()
                .AddDbContext<PlutonetcoreTemplateDbContext>(options =>
                    {
                        options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddSerilog()));
                        options.UseSqlServer(config["ConnectionString"],
                            sqlServerOptionsAction: sqlOptions =>
                            {
                                sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                                sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                            });
                    },
                    ServiceLifetime.Scoped );
            services.AddLogging(options => { options.AddSerilog(); });

            var autofac= ConfigureContainer(new ContainerBuilder());
            autofac.Populate(services);
            _Container = autofac.Build();

        }


        
        public ContainerBuilder ConfigureContainer(ContainerBuilder builder)
        {

            var dataAccess = Assembly.GetAssembly(typeof(ApiBaseController<>));
            builder.RegisterAssemblyTypes(dataAccess)
                .Where(t => t.Name.EndsWith("Controller"))
                .InstancePerLifetimeScope();


            #region MediatoR
            builder.RegisterModule(new MediatorModule());
            #endregion


            #region Application
            builder.RegisterModule(new ApplicationModule());
            #endregion

            return builder;
        }

    }
}