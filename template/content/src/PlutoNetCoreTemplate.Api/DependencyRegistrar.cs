#pragma warning disable IDE0060 // 删除未使用的参数

using Microsoft.Extensions.DependencyInjection;

namespace PlutoNetCoreTemplate.Api
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.OpenApi.Models;

    using PlutoNetCoreTemplate.Api.Filters;
    using PlutoNetCoreTemplate.Api.HealthChecks;
    using PlutoNetCoreTemplate.Application.AppServices.Generics;

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using Microsoft.AspNetCore.Mvc.Controllers;

    public static class DependencyRegistrar
    {
        /// <summary>
        /// 添加Authorization服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization();
            services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionRequirementHandler>();
            return services;
        }


        /// <summary>
        /// web 防火墙
        /// </summary>
        /// <param name="services"></param>
        public static void AddFirewall(this IServiceCollection services)
        {
            services.AddTransient<FirewallAttribute>();
        }


        /// <summary>
        /// 健康检查
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddCustomHealthCheck(this IServiceCollection services, IConfiguration configuration)
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
        /// API文档
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlutoNetCoreTemplate", Version = "v1" });

                c.AddServer(new OpenApiServer()
                {
                    Url = "",
                    Description = "PlutoNetCoreTemplate"
                });
                c.CustomOperationIds(apiDesc =>
                {
                    var controllerAction = apiDesc.ActionDescriptor as ControllerActionDescriptor;
                    return  controllerAction?.ControllerName+"-"+controllerAction?.ActionName;
                });

                c.SupportNonNullableReferenceTypes();

                c.UseAllOfToExtendReferenceSchemas();

                c.AddSecurityDefinition("Bearer", //Name the security scheme
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme.",
                        Type = SecuritySchemeType.Http, //We set the scheme type to http since we're using bearer authentication
                        Scheme = "Bearer" //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer", //The name of the previously defined security scheme.
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });

                c.MapType<IEnumerable<SortingDescriptor>>(() => new OpenApiSchema { Type = "string", Format = "json" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            return services;
        }


        /// <summary>
        /// 跨域
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddCustomCors(this IServiceCollection services, string corsName, IConfiguration configuration)
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