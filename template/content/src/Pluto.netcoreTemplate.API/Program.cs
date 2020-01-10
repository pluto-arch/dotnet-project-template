using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pluto.netcoreTemplate.Infrastructure;
using Serilog;
using Serilog.Events;

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
                .ConfigureKestrel(options =>
                {
                    var ports = GetDefinedPorts(configuration);
                    options.Listen(IPAddress.Any, ports, listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                    });
                })
                .ConfigureServices(services => services.AddAutofac())
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(configuration)
                .UseSerilog()
                .Build();
            return webHost;
        }

        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            const string outputTemplate = "[{Timestamp:HH:mm:ss.FFF} {Level}]{NewLine}{Event}。{RequestId}{NewLine}{Message}{NewLine}({SourceContext:l}){NewLine}{Exception}";
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .MinimumLevel.Override("System", LogEventLevel.Error)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(configuration["Seq:SeqUrl"])
                //.WriteTo.File(Path.Combine("logs", @"log.log"), rollingInterval: RollingInterval.Day,
                //    outputTemplate: outputTemplate)
                .ReadFrom.Configuration(configuration)
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


        private static int  GetDefinedPorts(IConfiguration config)
        {
            var port = config.GetValue("PORT", 5000);
            return port;
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });



    }
}
