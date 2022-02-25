using System;
using Microsoft.Extensions.Logging;

namespace csharpi_api_tests
{
    public static class TestLogging
    {
        public static ILoggerFactory LogFactory { get; } = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            // Clear Microsoft's default providers (like eventlogs and others)
            builder.AddSimpleConsole(options =>
            {
                options.IncludeScopes = true;
                options.SingleLine = true;
                options.TimestampFormat = "hh:mm:ss ";
            }).SetMinimumLevel(LogLevel.Warning);
        });

        public static ILogger<T> CreateLogger<T>() => LogFactory.CreateLogger<T>();
    }
}
