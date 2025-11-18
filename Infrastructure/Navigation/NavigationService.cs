using Microsoft.Extensions.Logging;

namespace MauiTemplate.Infrastructure.Navigation;

public class NavigationService : INavigationService
{
    private readonly ILogger<NavigationService> _logger;

    public NavigationService(ILogger<NavigationService> logger)
    {
        _logger = logger;
    }

    public async Task NavigateToAsync(string route, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Navigating to: {Route}", route);
            await Shell.Current.GoToAsync(route);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Navigation failed to: {Route}", route);
            throw;
        }
    }

    public async Task NavigateToAsync(string route, IDictionary<string, object> parameters, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Navigating to: {Route} with parameters", route);
            await Shell.Current.GoToAsync(route, parameters);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Navigation with parameters failed to: {Route}", route);
            throw;
        }
    }

    public async Task GoBackAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Navigating back");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Back navigation failed");
            throw;
        }
    }

    public async Task NavigateToModalAsync(string route, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Navigating to modal: {Route}", route);
            await Shell.Current.Navigation.PushModalAsync(
                (Page)Activator.CreateInstance(Type.GetType(route)!)!);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Modal navigation failed to: {Route}", route);
            throw;
        }
    }

    public async Task NavigateToModalAsync(string route, IDictionary<string, object> parameters, CancellationToken cancellationToken = default)
    {
        await NavigateToModalAsync(route, cancellationToken);
    }

    public async Task CloseModalAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Closing modal");
            await Shell.Current.Navigation.PopModalAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to close modal");
            throw;
        }
    }
}
