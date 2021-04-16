using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PlutoNetCoreTemplate.Infrastructure;
using Serilog;
using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Design;
using PlutoNetCoreTemplate.Extensions;
using PlutoNetCoreTemplate.Domain;
using PlutoNetCoreTemplate.Application;
using PlutoNetCoreTemplate.Domain.SeedWork;

namespace PlutoNetCoreTemplate
{
#if (Grpc)
    using Application.Grpc;
    using PlutoNetCoreTemplate.Application.Grpc.Services;
#endif


    public class Startup
    {
        private const string DefaultCorsName = "default";

        private readonly string _conntctionString = string.Empty;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _conntctionString = configuration.GetConnectionString("EfCore.MSSQL");
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomerControllers()
                .AddCustomerHealthCheck(Configuration)
                .AddCustomerSwagger()
                .AddCustomerCors(DefaultCorsName, Configuration)
                .AddHttpContextAccessor()
                .AddApplicationLayer()
                .AddDomainLayer()
                .AddInfrastructureLayer(Configuration, DbContextCreateFactory.OptionsAction(_conntctionString));

            services.AddTenant();
#if (Grpc)
            services.AddGrpc();
            services.AddSingleton<GrpcCallerService>();
#endif
            services.AddAuthentication();
            services.AddAuthorization()
                .AddPermission();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ZeroStack.DeviceCenter.API v1"));
            }
            if (env.IsProduction())
            {
                app.UseExceptionProcess();
                app.UseHsts();
                app.UseHttpsRedirection();
                app.UseHttpContextLog();
            }
            app.UseTenant();
            app.UseCors(DefaultCorsName);
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
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
    }


    /// <summary>
    /// efcore 设计时工厂
    /// </summary>
    public class DbContextCreateFactory : IDesignTimeDbContextFactory<EfCoreDbContext>
    {
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


        public static Action<DbContextOptionsBuilder> OptionsAction(string sqlConnStr)
        {
            return options =>
            {
                options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddFilter((category, level) =>
                                                                                               category ==
                                                                                               DbLoggerCategory
                                                                                                   .Database.Command
                                                                                                   .Name
                                                                                               && level == LogLevel
                                                                                                   .Information)
                                                                                .AddSerilog()));

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
}