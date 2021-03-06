using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using PlutoNetCoreTemplate.Application;
using PlutoNetCoreTemplate.Domain;
using PlutoNetCoreTemplate.Infrastructure;
using PlutoNetCoreTemplate.ServeHost.Extensions;

using Test;

#if (Grpc)
    using Application.Grpc;
    using PlutoNetCoreTemplate.Application.Grpc.Services;
#endif

namespace PlutoNetCoreTemplate.ServeHost
{
    public class Startup
    {
        private const string DefaultCorsName = "default";

        private readonly string _conntctionString = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _conntctionString = configuration.GetConnectionString("EfCore.MSSQL");
        }

        /// <summary>
        /// IServiceCollection
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomerControllers()
                 .AddCustomerHealthCheck(Configuration)
                 .AddCustomerSwagger()
                 .AddCustomerCors(DefaultCorsName, Configuration)
                 .AddHttpContextAccessor()
                 .AddApplicationLayer()
                 .AddDomainLayer()
                 .AddInfrastructureLayer(Configuration, DesignTimeDbContextFactory.OptionsAction(_conntctionString));

#if (Grpc)
            services.AddGrpc();
            services.AddSingleton<GrpcCallerService>();
#endif

            var assembly = Assembly.GetAssembly(typeof(ControllerPartAssembly));
            var controllerAssemblyPart = new AssemblyPart(assembly);
            var mvcBuilders = services.AddMvcCore();
            mvcBuilders.ConfigureApplicationPartManager(apm=> 
            {
                apm.ApplicationParts.Add(controllerAssemblyPart);
            });
            mvcBuilders.SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Latest);
        }

        /// <summary>
        /// IApplicationBuilder
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpContextLog();
            if (env.IsProduction())
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlutoNetCoreTemplate"); });

            app.UseCors(DefaultCorsName);
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
#if (Grpc)
                endpoints.MapGrpcService<OrderGrpc>();
#endif
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    ResponseWriter = async (c, r) =>
                    {
                        c.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(r.Entries);
                        await c.Response.WriteAsync(result);
                    }
                });
                endpoints.MapControllers();
            });

        }




        #region design time
        /// <summary>
        /// DesignTimeDbContextFactory
        /// </summary>
        public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EfCoreDbContext>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="args"></param>
            /// <returns></returns>
            public EfCoreDbContext CreateDbContext(string[] args)
            {
                var configbuild = new ConfigurationBuilder();
                configbuild.AddJsonFile("appsettings.json", optional: true);
                var config = configbuild.Build();
                string conn = config.GetConnectionString("EfCore.MSSQL");

                var optionsBuilder = new DbContextOptionsBuilder<EfCoreDbContext>();
                OptionsAction(conn).Invoke(optionsBuilder);
                return new EfCoreDbContext(optionsBuilder.Options);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="sqlConnStr"></param>
            /// <returns></returns>
            public static Action<DbContextOptionsBuilder> OptionsAction(string sqlConnStr)
            {
                return options =>
                {
                    options.UseSqlServer(sqlConnStr,
                                         sqlServerOptionsAction: sqlOptions =>
                                         {
                                             sqlOptions.MigrationsAssembly(typeof(Startup)
                                                                           .GetTypeInfo().Assembly.GetName().Name);
                                             sqlOptions.EnableRetryOnFailure(maxRetryCount: 5,
                                                                             maxRetryDelay: TimeSpan.FromSeconds(30),
                                                                             errorNumbersToAdd: null);
                                         });
                };
            }


        }
        #endregion
    }
}
