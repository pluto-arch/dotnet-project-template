using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;
using Serilog.Core;
using Serilog.Events;

using System;
using System.Diagnostics;

namespace PlutoNetCoreTemplate.Api.Extensions.Logger
{
    using Infrastructure.Providers;

    public static class SerilogConfiguration
    {
        public static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration, string applicationName)
        {
            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithProperty("ApplicationName", applicationName)
                .Enrich.With<ActivityEnricher>()
                .Enrich.FromLogContext()
                .CreateLogger();
        }
    }


    /// <summary>
    /// 服务间追踪id
    /// </summary>
    public class ServiceTraceIdEnricher : ILogEventEnricher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventIdProvider _eventIdProvider;
        private readonly Action<LogEvent, ILogEventPropertyFactory, HttpContext> _enrichAction;

        public ServiceTraceIdEnricher(IServiceProvider serviceProvider) : this(serviceProvider, null)
        {
            _eventIdProvider = serviceProvider.GetService<IEventIdProvider>() ?? new NullEventIdProvider();
        }

        public ServiceTraceIdEnricher(IServiceProvider serviceProvider, Action<LogEvent, ILogEventPropertyFactory, HttpContext> enrichAction)
        {
            _serviceProvider = serviceProvider;
            if (enrichAction == null)
            {
                _enrichAction = (logEvent, propertyFactory, httpContext) =>
                {
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("serviceTraceId", _eventIdProvider.EventId.ToString()));
                };
            }
            else
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

    public class ActivityEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var activity = Activity.Current;
            if (activity != null)
            {
                logEvent.AddPropertyIfAbsent(new LogEventProperty("spanId", new ScalarValue(activity.GetSpanId())));
                logEvent.AddPropertyIfAbsent(new LogEventProperty("traceId", new ScalarValue(activity.GetTraceId())));
                logEvent.AddPropertyIfAbsent(new LogEventProperty("parentId", new ScalarValue(activity.GetParentId())));
            }
        }
    }

    internal static class ActivityExtensions
    {
        public static string GetSpanId(this Activity activity)
        {
            return activity.IdFormat switch
            {
                ActivityIdFormat.Hierarchical => activity.Id,
                ActivityIdFormat.W3C => activity.SpanId.ToHexString(),
                _ => null,
            } ?? string.Empty;
        }

        public static string GetTraceId(this Activity activity)
        {
            return activity.IdFormat switch
            {
                ActivityIdFormat.Hierarchical => activity.RootId,
                ActivityIdFormat.W3C => activity.TraceId.ToHexString(),
                _ => null,
            } ?? string.Empty;
        }

        public static string GetParentId(this Activity activity)
        {
            return activity.IdFormat switch
            {
                ActivityIdFormat.Hierarchical => activity.ParentId,
                ActivityIdFormat.W3C => activity.ParentSpanId.ToHexString(),
                _ => null,
            } ?? string.Empty;
        }
    }
}
