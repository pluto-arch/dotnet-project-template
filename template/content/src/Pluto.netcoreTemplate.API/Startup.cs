using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Pluto.netcoreTemplate.API.Middlewares;
using Pluto.netcoreTemplate.API.Modules;
using Pluto.netcoreTemplate.Infrastructure;
using Pluto.netcoreTemplate.Infrastructure.Providers;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;


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
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });


            #region EventIdProvider
            services.AddScoped(typeof(EventIdProvider));
            #endregion


            #region efcore
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<PlutonetcoreTemplateDbContext>(options =>
                    {
                        options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddSerilog()));
                        options.UseSqlServer(Configuration["ConnectionString"],
                            sqlServerOptionsAction: sqlOptions =>
                            {
                                sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                                sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                            });
                    },
                    ServiceLifetime.Scoped  //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
                );
            #endregion


            #region swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pluto.netcoreTemplate.API", Version = "v1" });

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

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pluto.netcoreTemplate.API");
            });
            app.UseLogProcess();
            app.UseCors(DefaultCorsName);
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
