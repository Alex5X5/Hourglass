namespace DatabaseUtil.Logic;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

public static class LoggingShortcuts {

    public static Serilog.Core.Logger CreateSimpleLogger() =>
        new Serilog.LoggerConfiguration()
            .WriteTo.Console()
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Hour)
                    .Enrich.FromLogContext()
                        .CreateLogger();

    public static Serilog.Core.Logger ReplaceLoggingWithSerilogSingleton(this WebApplicationBuilder builder) {
        var logger = CreateSimpleLogger();
        builder.Logging.ClearProviders()
            .Services.AddSingleton(logger);
        return logger;
    }
}
