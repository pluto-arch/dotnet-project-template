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

namespace PlutoNetCoreTemplate.Extensions
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
            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
    }
}
