using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;

namespace PlutoNetCoreTemplate.API.Extensions
{
    public static class ILoggerBuilderExtension
    {
        public static ILoggingBuilder AddCustomerSerilog(this ILoggingBuilder builder,IConfiguration configuration)
        {
            Log.Logger = CreateSerilogLogger(configuration);
            builder.AddProvider((ILoggerProvider)new SerilogLoggerProvider(dispose:false));
            builder.AddFilter<SerilogLoggerProvider>((string)null, LogLevel.Trace);
            return builder;
        }


        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            const string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Console()
                .WriteTo.File(Path.Combine("logs", @"log.log"), rollingInterval: RollingInterval.Day,
                    outputTemplate: outputTemplate)
                .CreateLogger();
        }
    }
}
