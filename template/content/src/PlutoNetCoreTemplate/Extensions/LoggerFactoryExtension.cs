using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Extensions.Logging;

namespace PlutoNetCoreTemplate.Extensions
{
    public static class LoggerFactoryExtension
    {
        public static ILoggerFactory AddCustomerSerilog(this ILoggerFactory factory, IConfiguration configuration, IServiceProvider services)
        {
            Log.Logger = CreateSerilogLogger(configuration, services);
            factory.AddProvider((ILoggerProvider)new SerilogLoggerProvider(dispose: false));
            return factory;
        }

        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration, IServiceProvider services)
        {
            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithHttpContextInfo(services, (logEvent, propertyFactory, httpContext) =>
                {
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RequestIP", httpContext.Connection.RemoteIpAddress.ToString()));
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RequestPath", httpContext.Request.Path));
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RequestMethod", httpContext.Request.Method));
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Referer", httpContext.Request.Headers["Referer"].ToString()));
                    if (httpContext.Response.HasStarted)
                    {
                        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ResponseStatus", httpContext.Response.StatusCode));
                    }
                })
                .CreateLogger();
        }

    }

    public class HttpContextEnricher : ILogEventEnricher
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly Action<LogEvent, ILogEventPropertyFactory, HttpContext> _enrichAction;

        public HttpContextEnricher(IServiceProvider serviceProvider) : this(serviceProvider, null)
        {
        }

        public HttpContextEnricher(IServiceProvider serviceProvider, Action<LogEvent, ILogEventPropertyFactory, HttpContext> enrichAction)
        {
            _serviceProvider = serviceProvider;
            if (enrichAction == null)
            {
                _enrichAction = (logEvent, propertyFactory, httpContext) =>
                {
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RequestIP", httpContext.Connection.RemoteIpAddress.ToString()));
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RequestPath", httpContext.Request.Path));
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RequestMethod", httpContext.Request.Method));
                };
            } else
            {
                _enrichAction = enrichAction;
            }
        }


        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var httpContext = _serviceProvider.GetService<IHttpContextAccessor>()?.HttpContext;
            if (null != httpContext)
            {
                _enrichAction.Invoke(logEvent, propertyFactory, httpContext);
            }
        }
    }

    public static class EnricherExtensions
    {
        public static LoggerConfiguration WithHttpContextInfo(this LoggerEnrichmentConfiguration enrich, IServiceProvider serviceProvider)
        {
            if (enrich == null)
                throw new ArgumentNullException(nameof(enrich));

            return enrich.With(new HttpContextEnricher(serviceProvider));
        }

        public static LoggerConfiguration WithHttpContextInfo(this LoggerEnrichmentConfiguration enrich, IServiceProvider serviceProvider, Action<LogEvent, ILogEventPropertyFactory, HttpContext> enrichAction)
        {
            if (enrich == null)
                throw new ArgumentNullException(nameof(enrich));

            return enrich.With(new HttpContextEnricher(serviceProvider, enrichAction));
        }
    }
}
