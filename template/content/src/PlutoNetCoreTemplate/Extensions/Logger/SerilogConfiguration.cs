﻿using System;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace PlutoNetCoreTemplate.Extensions.Logger
{
    public static class SerilogConfiguration
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


    /// <summary>
    /// 记录http上下文的enricher
    /// </summary>
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
                    var x_forwarded_for = new StringValues();
                    if (httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                    {
                        x_forwarded_for = httpContext.Request.Headers["X-Forwarded-For"];
                    }
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("x_forwarded_for", JsonConvert.SerializeObject(x_forwarded_for)));
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("request_path", httpContext.Request.Path));
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("request_method", httpContext.Request.Method));
                    if (httpContext.Response.HasStarted)
                    {
                        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("response_status", httpContext.Response.StatusCode));
                    }
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
}