using Microsoft.Extensions.Configuration;

using Serilog;

namespace PlutoNetCoreTemplate.Api
{
    public static class SerilogConfiguration
    {
        public static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration, string applicationName)
        {
            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithProperty("AppName", applicationName)
                .CreateLogger();
        }
    }
}
