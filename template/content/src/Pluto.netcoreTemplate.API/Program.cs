using Autofac.Extensions.DependencyInjection;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Events;

using System;
using System.IO;
using System.Net;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pluto.netcoreTemplate.Infrastructure;

namespace Pluto.netcoreTemplate.API
{
    public class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
        public static void Main(string[] args)
        {
            var configuration = GetConfiguration();
            Log.Logger = CreateSerilogLogger(configuration);
            try
            {
                Log.Information("配置web主机 ({ApplicationContext})...", AppName);
                var host = BuildWebHost(configuration, args);
                Log.Information("主机配置完毕，开始启动 ({ApplicationContext})...", AppName);
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "应用程序异常终止 ({ApplicationContext})!", AppName);
            }
            finally
            {
                Log.CloseAndFlush();
            }

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                Log.Error(e.ExceptionObject as Exception, $"UnhandledException");
            };
        }

        private static IWebHost BuildWebHost(IConfiguration configuration, string[] args)
        {
            var webHost = WebHost.CreateDefaultBuilder(args)
                .CaptureStartupErrors(false)
                .UseUrls("http://0.0.0.0:5000")
                //.ConfigureKestrel(options =>
                //{
                //    var ports = GetDefinedPorts(configuration);
                //    options.Listen(IPAddress.Any, ports, listenOptions =>
                //    {
                //        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                //    });
                //})
                .ConfigureServices(services => services.AddAutofac())
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(configuration)
                .UseSerilog()
                .Build();

            webHost.MigrateDbContext<PlutonetcoreTemplateDbContext>((context, services) =>
            {
                var logger = services.GetService<ILogger<PlutonetcoreTemplateDbContext>>();
            });

            return webHost;
        }

        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            const string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine("logs", @"log.log"), rollingInterval: RollingInterval.Day,
                    outputTemplate: outputTemplate)
                .CreateLogger();
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            return builder.Build();

        }
    }



    public static class WebHostExtension
    {
        public static void MigrateDbContext<TContext>(this IWebHost webHost,
            Action<TContext, IServiceProvider> seeder)
            where TContext : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation("迁移数据库 ({DbContextName})", typeof(TContext).Name);
                    if (context.Database.GetPendingMigrations().Any())
                    {
                        context.Database.Migrate();
                    }
                    logger.LogInformation("已迁移数据库 {DbContextName}", typeof(TContext).Name);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "迁移数据库时出错 {DbContextName}", typeof(TContext).Name);
                }

            }
        }
    }

}
