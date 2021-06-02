using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using PlutoNetCoreTemplate.Api.Extensions.Tenant;
using PlutoNetCoreTemplate.Api.Filters;
using PlutoNetCoreTemplate.Api.HealthChecks;

#pragma warning disable IDE0060 // 删除未使用的参数


namespace PlutoNetCoreTemplate.Api.Extensions
{
    using Application.Permissions;
    using Microsoft.AspNetCore.Authorization;

    public static class ServiceCollection
    {
        /// <summary>
        /// 控制器
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddCustomerControllers(this IServiceCollection services)
        {
            services.AddControllers(options => { options.Filters.Add<ModelValidateFilter>(); })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            services.Configure<ApiBehaviorOptions>(
                options => { options.SuppressModelStateInvalidFilter = true; }
                );

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
        /// API文档
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddCustomerSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlutoNetCoreTemplate", Version = "v1" });
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


        /// <summary>
        /// 多租户
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddTenant(this IServiceCollection services)
        {
            services.AddTransient<TenantMiddleware>();
            return services;
        }


        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddPermission(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();
            services.AddScoped<IPermissionChecker, PermissionChecker>();
            services.AddScoped<IAuthorizationHandler, PermissionRequirementHandler>();
            return services;
        }
    }
}
