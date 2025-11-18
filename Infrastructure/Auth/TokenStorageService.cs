using MauiTemplate.Models;
using System.Text.Json;

namespace MauiTemplate.Infrastructure.Auth;

public class TokenStorageService : ITokenStorageService
{
    private const string TokenKey = "auth_token";
    private const string RefreshTokenKey = "refresh_token";
    private const string ExpiresAtKey = "token_expires_at";

    public async Task SaveTokenAsync(AuthToken token)
    {
        await SecureStorage.Default.SetAsync(TokenKey, token.AccessToken);
        await SecureStorage.Default.SetAsync(RefreshTokenKey, token.RefreshToken);
        await SecureStorage.Default.SetAsync(ExpiresAtKey, token.ExpiresAt.ToString("O"));
        
        // Also save to preferences for non-sensitive data
        Preferences.Default.Set("token_type", token.TokenType);
        Preferences.Default.Set("token_scopes", JsonSerializer.Serialize(token.Scopes));
    }

    public async Task<AuthToken?> GetTokenAsync()
    {
        try
        {
            var accessToken = await SecureStorage.Default.GetAsync(TokenKey);
            if (string.IsNullOrEmpty(accessToken))
                return null;

            var refreshToken = await SecureStorage.Default.GetAsync(RefreshTokenKey);
            var expiresAtStr = await SecureStorage.Default.GetAsync(ExpiresAtKey);

            if (string.IsNullOrEmpty(expiresAtStr))
                return null;

            return new AuthToken
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken ?? string.Empty,
                ExpiresAt = DateTime.Parse(expiresAtStr),
                TokenType = Preferences.Default.Get("token_type", "Bearer"),
                Scopes = JsonSerializer.Deserialize<string[]>(
                    Preferences.Default.Get("token_scopes", "[]")) ?? Array.Empty<string>()
            };
        }
        catch
        {
            return null;
        }
    }

    public async Task ClearTokenAsync()
    {
        SecureStorage.Default.Remove(TokenKey);
        SecureStorage.Default.Remove(RefreshTokenKey);
        SecureStorage.Default.Remove(ExpiresAtKey);
        Preferences.Default.Remove("token_type");
        Preferences.Default.Remove("token_scopes");
        await Task.CompletedTask;
    }

    public async Task<bool> HasValidTokenAsync()
    {
        var token = await GetTokenAsync();
        return token != null && !token.IsExpired;
    }
}
