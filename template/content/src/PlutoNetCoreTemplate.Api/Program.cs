using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Serilog;

using System;
using System.IO;

namespace PlutoNetCoreTemplate.Api
{
    using PlutoNetCoreTemplate.Api.SeedData;
    using PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore;

    public class Program
    {
        public static readonly string AppName = typeof(Program).Namespace;
        public static void Main(string[] args)
        {
            var baseConfig = GetLogConfig();
            Log.Logger = SerilogConfiguration.CreateSerilogLogger(baseConfig, AppName);
            try
            {
                Log.Information("准备启动{ApplicationContext}...", AppName);
                var host = BuildWebHost(args);
                Log.Information("{ApplicationContext} 已启动", AppName);
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "{ApplicationContext} 出现错误:{Messsage} !", AppName, ex.Message);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IHost BuildWebHost(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                           .UseContentRoot(Directory.GetCurrentDirectory())
                           .ConfigureWebHostDefaults(webhost =>
                           {
                               webhost.UseStartup<Startup>()
                                      .UseIISIntegration()
                                      .CaptureStartupErrors(false);
                           })
                           .ConfigureAppConfiguration((context, builder) =>
                           {
                               var env = context.HostingEnvironment;
                               var baseConfig = GetConfiguration(env);
                               builder.AddConfiguration(baseConfig);
                           })
                           .UseSerilog(dispose: true)
                           .Build();
#if DEBUG
            host.MigrateDbContext<DeviceCenterDbContext>();
            host.MigrateDbContext<SystemDbContext>();
#endif

            return host;
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        private static IConfiguration GetConfiguration(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            return builder.Build();

        }

        /// <summary>
        /// 日志配置
        /// </summary>
        /// <returns></returns>
        private static IConfiguration GetLogConfig()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("serilogsetting.json", optional: false, reloadOnChange: true);
            return builder.Build();

        }

    }

}
