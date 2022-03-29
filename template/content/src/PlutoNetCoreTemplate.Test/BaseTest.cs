using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using NUnit.Framework;

using PlutoNetCoreTemplate.Application;

using Serilog;

using System;

namespace PlutoNetCoreTemplate.Test
{
    using Domain.Aggregates.TenantAggregate;

    using Infrastructure.EntityFrameworkCore;

    using PlutoNetCoreTemplate.Domain;
    using PlutoNetCoreTemplate.Infrastructure;

    public class BaseTest
    {
        protected IServiceProvider serviceProvider;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            services.AddSingleton<IConfiguration>(s => config);



            services.AddControllers()
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
            services.AddLogging(options => { options.AddSerilog(); });
            services.AddApplicationLayer()
                .AddDomainLayerNoSeed()
                .AddTenantComponent(config)
                .AddInfrastructureLayer(config);
            services.AddSingleton<ICurrentTenantAccessor, CurrentTenantAccessor>();
            services.AddTransient<ICurrentTenant, CurrentTenant>();
            services.AddTransient<ITenantProvider, EFCoreTenantProvider>();
            serviceProvider = services.BuildServiceProvider();
        }
    }
}