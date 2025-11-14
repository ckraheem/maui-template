using Microsoft.Extensions.Logging;

namespace MauiTemplate.Infrastructure.Extensions;

public static class ErrorBoundary
{
    private static ILogger? _logger;

    public static void Initialize(ILogger logger)
    {
        _logger = logger;

        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
    }

    private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception exception)
        {
            _logger?.LogCritical(exception, "Unhandled exception occurred");
            
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current?.MainPage?.DisplayAlert(
                    "Error",
                    "An unexpected error occurred. The application will now close.",
                    "OK")!;
            });
        }
    }

    private static void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        _logger?.LogError(e.Exception, "Unobserved task exception occurred");
        e.SetObserved();
    }

    public static async Task<T?> ExecuteSafelyAsync<T>(
        Func<Task<T>> operation,
        string errorMessage = "An error occurred",
        T? defaultValue = default)
    {
        try
        {
            return await operation();
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, errorMessage);
            
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Application.Current?.MainPage?.DisplayAlert(
                    "Error",
                    errorMessage,
                    "OK")!;
            });
            
            return defaultValue;
        }
    }
}
