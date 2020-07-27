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
using Demo.API.Filters;
using Demo.API.Middlewares;
using Demo.API.Modules;
using Demo.Infrastructure;
using Demo.Infrastructure.Providers;
using PlutoData;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Demo.API.HealthChecks;
using Demo.Application.GrpcServices;
using Demo.Application.GrpcServices.GrpcCallers;
using Demo.Application.GrpcServices.GrpcCallers.Interfaces;
using Demo.Application.HttpClientHandlers;
using Serilog.Events;
using Serilog.Extensions.Logging;
using Serilog.Sinks.Elasticsearch;


namespace Demo.API
{
    public class Startup
    {
        private const string DefaultCorsName = "default";

        private readonly string conntctionString = string.Empty;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            conntctionString = Configuration["ConnectionStrings:Demo"];
        }

        public IConfiguration Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; }



       


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region 链接字符串
            var sqlConnStr = Configuration.GetConnectionString("Default");
            #endregion


            #region api controller
            services.AddControllers(options => { options.Filters.Add<ModelValidateFilter>(); })
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
            #endregion


            #region HealthChecks
            services.Configure<MemoryCheckOptions>(options =>
            {
                options.Threshold = Configuration.GetValue<long>("Options:MemoryChkOpt:Threshold");
            });
            services.AddHealthChecks()
                .AddCheck<DatabaseHealthCheck>("database_check", failureStatus: HealthStatus.Unhealthy, tags: new string[] { "database", "sqlServer" })
                .AddCheck<MemoryHealthCheck>("memory_check", failureStatus: HealthStatus.Degraded);
            #endregion

            #region EventIdProvider
            services.AddScoped(typeof(EventIdProvider));
            #endregion


            #region efcore  根据实际情况使用数据库

            services.AddUnitOfWorkDbContext<DemoDbContext>(DbContextCreateFactory.DbContextOptionsAction(sqlConnStr),
                                                               ServiceLifetime.Scoped)
                        .AddRepository();
            #endregion


            #region swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo.API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", //Name the security scheme
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme.",
                        Type = SecuritySchemeType.Http, //We set the scheme type to http since we're using bearer authentication
                        Scheme = "bearer" //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                                {
                                    new OpenApiSecurityScheme{
                                        Reference = new OpenApiReference{
                                            Id = "Bearer", //The name of the previously defined security scheme.
                                            Type = ReferenceType.SecurityScheme
                                        }
                                    },new List<string>()
                                }
                });


                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            #endregion


            #region cors

            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsName,
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                    });
            });

            #endregion

            #region httpcontext accessor
            services.AddHttpContextAccessor();
            #endregion



            #region other grpc
            services.AddTransient(typeof(GrpcCallerService));
            services.Configure<GrpcUrlConfig>(Configuration.GetSection(GrpcUrlConfig.ConfigKey));
            services.AddHttpClient<IOrderGrpcService, OrderGrpcService>();
            #endregion

        }




        /// <summary>
        /// 配置第三方(autofac)容器
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            #region MediatoR
            builder.RegisterModule(new MediatorModule());
            #endregion


            #region Application
            builder.RegisterModule(new ApplicationModule());
            #endregion
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILoggerFactory loggerFactory)
        {
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            app.UseExceptionProcess();
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionProcess();
            //}
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo.API");
            });
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
    /// <remarks>
    /// 当program中没有默认的：
    /// public static IHostBuilder CreateHostBuilder(string[] args) =>
    /// Host.CreateDefaultBuilder(args)
    /// .ConfigureWebHostDefaults(webBuilder =>
    /// {
    /// webBuilder.UseStartup<Startup>();
    /// });
    /// 时，必须指定如何初始化创建dbcontext
    /// </remarks>
    public class DbContextCreateFactory : IDesignTimeDbContextFactory<DemoDbContext>
    {
        public DemoDbContext CreateDbContext(string[] args)
        {
            var configbuild = new ConfigurationBuilder();
            configbuild.AddJsonFile("appsettings.json", optional: true);
            var config = configbuild.Build();
            string conn = config.GetConnectionString("Default"); ;

            var optionsBuilder = new DbContextOptionsBuilder<DemoDbContext>();
            DbContextOptionsAction(conn).Invoke(optionsBuilder);
            return new DemoDbContext(optionsBuilder.Options);

        }


        public static Action<DbContextOptionsBuilder> DbContextOptionsAction(string sqlConnStr)
        {
            return options =>
            {
                options.UseLoggerFactory(LoggerFactory.Create(
                    builder => builder.AddFilter((category, level) =>
                                                        category == DbLoggerCategory.Database.Command.Name
                                                        && level == LogLevel.Information)
                                              .AddSerilog()))
                                        ;//.EnableSensitiveDataLogging(); //开启敏感数据记录// sql变量值
                options.UseSqlServer(sqlConnStr,
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
            };
        }
    }

}
