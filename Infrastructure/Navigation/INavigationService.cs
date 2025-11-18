namespace MauiTemplate.Infrastructure.Navigation;

public interface INavigationService
{
    Task NavigateToAsync(string route, CancellationToken cancellationToken = default);
    Task NavigateToAsync(string route, IDictionary<string, object> parameters, CancellationToken cancellationToken = default);
    Task GoBackAsync(CancellationToken cancellationToken = default);
    Task NavigateToModalAsync(string route, CancellationToken cancellationToken = default);
    Task NavigateToModalAsync(string route, IDictionary<string, object> parameters, CancellationToken cancellationToken = default);
    Task CloseModalAsync(CancellationToken cancellationToken = default);
}
