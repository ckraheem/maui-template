using MauiTemplate.Models;

namespace MauiTemplate.Infrastructure.Auth;

public interface ITokenStorageService
{
    Task SaveTokenAsync(AuthToken token);
    Task<AuthToken?> GetTokenAsync();
    Task ClearTokenAsync();
    Task<bool> HasValidTokenAsync();
}
