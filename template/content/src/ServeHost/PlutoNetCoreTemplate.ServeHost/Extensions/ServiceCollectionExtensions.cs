namespace PlutoNetCoreTemplate.ServeHost.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.OpenApi.Models;

    using Newtonsoft.Json.Serialization;

    using PlutoNetCoreTemplate.ServeHost.HealthChecks;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomerControllers(this IServiceCollection services)
        {

            services.AddControllers()
                .AddNewtonsoftJson(opt=> 
                {
                    opt.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    opt.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            //services.Configure<ApiBehaviorOptions>(opt=> 
            //{
            //    opt.SuppressModelStateInvalidFilter = true;
            //});

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            services.AddRouting(options => options.LowercaseUrls = true);

            return services;
        }


        /// <summary>
        /// 健康检查
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddCustomerHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MemoryCheckOptions>(options =>
            {
                options.Threshold = configuration.GetValue<long>("Options:MemoryChkOpt:Threshold");
            });
            services.AddHealthChecks()
                .AddCheck<DatabaseHealthCheck>("database_check", failureStatus: HealthStatus.Unhealthy,
                    tags: new string[] { "database", "sqlServer" })
                .AddCheck<MemoryHealthCheck>("memory_check", failureStatus: HealthStatus.Degraded);
            return services;
        }


        /// <summary>
        /// swagger
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomerSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlutoNetCoreTemplate", Version = "v1" });
                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme.",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer"
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            return services;
        }

        /// <summary>
        /// 跨域
        /// </summary>
        /// <param name="services"></param>
        /// <param name="corsName"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomerCors(this IServiceCollection services, string corsName, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(corsName,
                    builder =>
                    {
                        builder.SetIsOriginAllowed(_ => true).AllowAnyHeader()
                            .AllowAnyMethod();
                        builder.AllowAnyOrigin();
                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                    });
            });
            return services;
        }

    }
}
