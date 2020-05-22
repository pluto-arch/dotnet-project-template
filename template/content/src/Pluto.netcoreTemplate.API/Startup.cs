using Autofac;
using Autofac.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using Pluto.netcoreTemplate.API.Middlewares;
using Pluto.netcoreTemplate.API.Modules;
using Pluto.netcoreTemplate.Infrastructure;
using Pluto.netcoreTemplate.Infrastructure.Extensions;
using Pluto.netcoreTemplate.Infrastructure.Providers;

using Serilog;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Pluto.netcoreTemplate.API.Filters;
using Pluto.netcoreTemplate.Application.Queries;
using Pluto.netcoreTemplate.Application.Queries.Interfaces;
using PlutoData;


namespace Pluto.netcoreTemplate.API
{
    public class Startup
    {
        private const string DefaultCorsName = "default";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
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


            #region EventIdProvider
            services.AddScoped(typeof(EventIdProvider));
            #endregion


            #region efcore  根据实际情况使用数据库
            services
                .AddDbContext<PlutonetcoreTemplateDbContext>(options =>
                    {
                        options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddSerilog()));
                        options.UseSqlServer(Configuration["ConnectionString"],
                            sqlServerOptionsAction: sqlOptions =>
                            {
                                sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                                sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                            });
                    }, ServiceLifetime.Scoped
                )
                .AddUnitOfWork<PlutonetcoreTemplateDbContext>()
                .AddRepository();
            #endregion


            #region swagger
                        services.AddSwaggerGen(c =>
                        {
                            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pluto.netcoreTemplate.API", Version = "v1" });

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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionProcess();
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pluto.netcoreTemplate.API");
            });
            app.UseCors(DefaultCorsName);
            app.UseRouting();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
