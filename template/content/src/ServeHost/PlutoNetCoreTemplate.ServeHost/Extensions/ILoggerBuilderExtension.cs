namespace PlutoNetCoreTemplate.ServeHost.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Primitives;
    using Microsoft.Extensions.DependencyInjection;

    using Serilog;
    using Serilog.Core;
    using Serilog.Events;

    public static class ILoggerBuilderExtension
    {
        public static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration, string applicationName)
        {
            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithProperty("ApplicationName", applicationName)
                .Enrich.FromLogContext()
                .CreateLogger();
        }

        

    }
}
