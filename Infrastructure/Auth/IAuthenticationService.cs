using MauiTemplate.Models;

namespace MauiTemplate.Infrastructure.Auth;

public interface IAuthenticationService
{
    Task<bool> LoginAsync(string username, string password, CancellationToken cancellationToken = default);
    Task<bool> LoginWithAzureAdAsync(CancellationToken cancellationToken = default);
    Task<bool> LoginWithAuth0Async(CancellationToken cancellationToken = default);
    Task<bool> RefreshTokenAsync(CancellationToken cancellationToken = default);
    Task LogoutAsync(CancellationToken cancellationToken = default);
    Task<User?> GetCurrentUserAsync(CancellationToken cancellationToken = default);
    bool IsAuthenticated { get; }
}
