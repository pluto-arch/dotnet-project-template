using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using PlutoNetCoreTemplate.Infrastructure;
using PlutoData;
using PlutoNetCoreTemplate.Controllers;
using Serilog;
using PlutoNetCoreTemplate.Application;
using PlutoNetCoreTemplate.Domain;

namespace PlutoNetCoreTemplate.Test
{
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
            services.AddApplicationLayer();
            services.AddDomainLayer();
            var _conntctionString = config.GetConnectionString("EfCore.MSSQL");
            services.AddInfrastructureLayer(config,DbContextCreateFactory.OptionsAction(_conntctionString));
            serviceProvider=services.BuildServiceProvider();
        }
    }
}