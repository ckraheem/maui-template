using Serilog;
using Serilog.Events;

namespace MauiTemplate.Infrastructure.Extensions;

public static class LoggingExtensions
{
    public static MauiAppBuilder ConfigureLogging(this MauiAppBuilder builder)
    {
        var logPath = Path.Combine(FileSystem.AppDataDirectory, "logs", "app-.log");

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Debug()
            .WriteTo.File(
                logPath,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        builder.Logging.AddSerilog(Log.Logger);

        return builder;
    }
}
