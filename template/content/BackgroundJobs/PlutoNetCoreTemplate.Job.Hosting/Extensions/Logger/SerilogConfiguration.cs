namespace PlutoNetCoreTemplate.Job.Hosting.Extensions.Logger
{
    using Microsoft.Extensions.Configuration;

    using Serilog;

    public class SerilogConfiguration
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
