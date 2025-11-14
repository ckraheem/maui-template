namespace MauiTemplate.Services;

public interface ILoadingService
{
    void Show(string message = "Loading...");
    void Hide();
    Task ShowAsync(string message = "Loading...");
    Task HideAsync();
}
