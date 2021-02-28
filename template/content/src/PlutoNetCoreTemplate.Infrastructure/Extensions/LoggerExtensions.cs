namespace PlutoNetCoreTemplate.Infrastructure.Extensions
{
    using System;

    using Microsoft.Extensions.Logging;

    public static class LoggerExtensions
    {
        public static void LogInfo(this ILogger logger, string message)=> _logInfo(logger, message, null);


        private static readonly Action<ILogger, string, Exception> _logInfo =
            LoggerMessage.Define<string>(LogLevel.Information, new EventId(1, nameof(LogInfo)), "{message}");
    }
}