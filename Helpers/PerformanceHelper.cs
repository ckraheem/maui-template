using System.Diagnostics;

namespace MauiTemplate.Helpers;

/// <summary>
/// Helper class for performance optimization and monitoring
/// </summary>
public static class PerformanceHelper
{
    /// <summary>
    /// Measures execution time of an operation
    /// </summary>
    public static async Task<(T Result, TimeSpan Duration)> MeasureAsync<T>(Func<Task<T>> operation)
    {
        var stopwatch = Stopwatch.StartNew();
        var result = await operation();
        stopwatch.Stop();
        return (result, stopwatch.Elapsed);
    }

    /// <summary>
    /// Throttles an action to prevent excessive calls
    /// </summary>
    public static Action<T> Throttle<T>(Action<T> action, TimeSpan interval)
    {
        var lastRun = DateTime.MinValue;
        return arg =>
        {
            var now = DateTime.UtcNow;
            if (now - lastRun >= interval)
            {
                action(arg);
                lastRun = now;
            }
        };
    }

    /// <summary>
    /// Debounces an async action
    /// </summary>
    public static Func<Task> Debounce(Func<Task> action, TimeSpan delay)
    {
        CancellationTokenSource? cts = null;
        
        return async () =>
        {
            cts?.Cancel();
            cts = new CancellationTokenSource();
            
            try
            {
                await Task.Delay(delay, cts.Token);
                await action();
            }
            catch (TaskCanceledException)
            {
                // Expected when debouncing
            }
        };
    }
}
