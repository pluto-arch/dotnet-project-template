using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PlutoNetCoreTemplate.Middlewares;
using PlutoNetCoreTemplate.Modules;
using PlutoNetCoreTemplate.Infrastructure;
using PlutoData;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using PlutoNetCoreTemplate.Extensions;
using PlutoNetCoreTemplate.Filters;
using PlutoNetCoreTemplate.HealthChecks;


namespace PlutoNetCoreTemplate
{
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

		public ILifetimeScope AutofacContainer { get; private set; }

		public void ConfigureServices(IServiceCollection services)
		{
            services.AddCustomerControllers()
                .AddCustomerHealthCheck(Configuration)
                .AddCustomerSwagger()
                .AddCustomerCors(DefaultCorsName,Configuration)
                .AddHttpContextAccessor()
                .AddAutoMapper(cfg =>
                {
                    cfg.AddProfile<AutoMapperProfile>();
                    cfg.AddProfile<Application.AutoMapperProfile>();
                }, Assembly.GetExecutingAssembly())
                .AddUnitOfWorkDbContext<EfCoreDbContext>(
                    DbContextCreateFactory.OptionsAction(_conntctionString), ServiceLifetime.Scoped)
                .AddRepository();
		}


		public void ConfigureContainer(ContainerBuilder builder)
		{
            builder.RegisterModule(new MediatorModule());
            builder.RegisterModule(new ApplicationModule());
		}


		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();
            app.UseHttpContextLog();
			if (env.IsProduction())
			{
				app.UseHsts();
				app.UseHttpsRedirection();
			}
			app.UseExceptionProcess();
			app.UseStaticFiles();
			app.UseSwagger();
			app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlutoNetCoreTemplate"); });
			app.UseCors(DefaultCorsName);
			app.UseRouting();
			app.UseEndpoints(endpoints =>
			{
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
	/// 指定设计时dbcontext 工厂
	/// code first 迁移时使用
	/// </summary>
	/// 当program中没有默认的：
	/// public static IHostBuilder CreateHostBuilder(string[] args) =>
	/// Host.CreateDefaultBuilder(args)
	/// .ConfigureWebHostDefaults(webBuilder =>
	/// {
	/// });
	/// 时，必须指定如何初始化创建dbcontext
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