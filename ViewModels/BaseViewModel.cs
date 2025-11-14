using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace MauiTemplate.ViewModels;

public abstract partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private bool _isRefreshing;

    protected ILogger Logger { get; }

    protected BaseViewModel(ILogger logger)
    {
        Logger = logger;
    }

    public virtual Task OnAppearingAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public virtual Task OnDisappearingAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    protected async Task ExecuteAsync(Func<Task> operation, string? errorMessage = null)
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            await operation();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, errorMessage ?? "Operation failed");
            await HandleErrorAsync(ex);
        }
        finally
        {
            IsBusy = false;
        }
    }

    protected virtual Task HandleErrorAsync(Exception exception)
    {
        // Override in derived classes for custom error handling
        return Task.CompletedTask;
    }
}
