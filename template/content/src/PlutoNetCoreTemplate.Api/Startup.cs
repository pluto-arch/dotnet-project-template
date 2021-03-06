using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;

using PlutoNetCoreTemplate.Api.Extensions;
using PlutoNetCoreTemplate.Application;
using PlutoNetCoreTemplate.Domain;
using PlutoNetCoreTemplate.Infrastructure;

using Serilog;

using System.IO;

namespace PlutoNetCoreTemplate.Api
{
    using Extensions.SeedData;

    using Infrastructure.ConnectionString;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.Primitives;
    using Microsoft.IdentityModel.Logging;
    using Microsoft.IdentityModel.Tokens;

    using System.Text;

#if (Grpc)
    using Application.Grpc;
    using PlutoNetCoreTemplate.Application.Grpc.Services;
#endif


    public class Startup
    {
        private const string DefaultCorsName = "default";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
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
                .AddInfrastructureLayer(Configuration);

            services.Configure<TenantStoreOptions>(Configuration);
            services.AddTenant();
#if (Grpc)
            services.AddGrpc();
            services.AddSingleton<GrpcCallerService>();
#endif
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "pluto",
                        ValidAudience = "12312",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("715B59F3CDB1CF8BC3E7C8F13794CEA9")),
                    };
                });
            services.AddAuthorization()
                .AddPermission();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            IdentityModelEventSource.ShowPII = true;
            app.UseSerilogRequestLogging(config =>
            {
                config.EnrichDiagnosticContext = (context, httpContext) =>
                {
                    var xForwardedFor = new StringValues();
                    if (httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                    {
                        xForwardedFor = httpContext.Request.Headers["X-Forwarded-For"];
                    }
                    context.Set("request_path", httpContext.Request.Path);
                    context.Set("request_method", httpContext.Request.Method);
                    context.Set("x_forwarded_for", xForwardedFor.ToString());
                };
            });

            if (env.IsDevelopment())
            {
                app.DataSeederAsync().Wait();
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlutoNetCoreTemplate.API v1"));
            }
            if (env.IsProduction())
            {
                app.UseExceptionProcess();
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseCors(DefaultCorsName);
            app.UseRouting();
            app.UseAuthentication();
            app.UseTenant();
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

}