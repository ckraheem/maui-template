using CommunityToolkit.Maui.Core;

namespace MauiTemplate.Services;

public class LoadingService : ILoadingService
{
    private IPopup? _currentPopup;

    public void Show(string message = "Loading...")
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            // Using CommunityToolkit.Maui for popup
            // This is a simplified implementation
            Application.Current?.MainPage?.Dispatcher.Dispatch(() =>
            {
                // Show activity indicator or custom loading popup
            });
        });
    }

    public void Hide()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            _currentPopup = null;
        });
    }

    public Task ShowAsync(string message = "Loading...")
    {
        Show(message);
        return Task.CompletedTask;
    }

    public Task HideAsync()
    {
        Hide();
        return Task.CompletedTask;
    }
}
