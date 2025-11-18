using MauiTemplate.Models;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MauiTemplate.Infrastructure.Auth;

public class AuthenticationService : IAuthenticationService
{
    private readonly ITokenStorageService _tokenStorage;
    private readonly HttpClient _httpClient;
    private readonly ILogger<AuthenticationService> _logger;
    private User? _currentUser;

    public AuthenticationService(
        ITokenStorageService tokenStorage,
        HttpClient httpClient,
        ILogger<AuthenticationService> logger)
    {
        _tokenStorage = tokenStorage;
        _httpClient = httpClient;
        _logger = logger;
    }

    public bool IsAuthenticated => _currentUser != null;

    public async Task<bool> LoginAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Attempting login for user: {Username}", username);

            // This is a mock implementation - replace with actual API call
            var loginRequest = new { username, password };
            var content = new StringContent(
                JsonSerializer.Serialize(loginRequest),
                System.Text.Encoding.UTF8,
                "application/json");

            // Mock response for demo purposes
            // In production, call your actual login API:
            // var response = await _httpClient.PostAsync("/auth/login", content, cancellationToken);
            
            // For now, create a mock token
            var token = new AuthToken
            {
                AccessToken = GenerateMockJwtToken(username),
                RefreshToken = Guid.NewGuid().ToString(),
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                TokenType = "Bearer"
            };

            await _tokenStorage.SaveTokenAsync(token);
            _currentUser = await ExtractUserFromTokenAsync(token.AccessToken);
            
            _logger.LogInformation("Login successful for user: {Username}", username);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login failed for user: {Username}", username);
            return false;
        }
    }

    public async Task<bool> LoginWithAzureAdAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Attempting Azure AD login");
            
            // This would use Microsoft.Identity.Client (MSAL)
            // For now, mock implementation
            await Task.Delay(500, cancellationToken);
            
            var token = new AuthToken
            {
                AccessToken = GenerateMockJwtToken("azuread.user@domain.com"),
                RefreshToken = Guid.NewGuid().ToString(),
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                TokenType = "Bearer"
            };

            await _tokenStorage.SaveTokenAsync(token);
            _currentUser = await ExtractUserFromTokenAsync(token.AccessToken);
            
            _logger.LogInformation("Azure AD login successful");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Azure AD login failed");
            return false;
        }
    }

    public async Task<bool> LoginWithAuth0Async(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Attempting Auth0 login");
            
            // This would use Auth0 SDK
            // For now, mock implementation
            await Task.Delay(500, cancellationToken);
            
            var token = new AuthToken
            {
                AccessToken = GenerateMockJwtToken("auth0.user@domain.com"),
                RefreshToken = Guid.NewGuid().ToString(),
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                TokenType = "Bearer"
            };

            await _tokenStorage.SaveTokenAsync(token);
            _currentUser = await ExtractUserFromTokenAsync(token.AccessToken);
            
            _logger.LogInformation("Auth0 login successful");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Auth0 login failed");
            return false;
        }
    }

    public async Task<bool> RefreshTokenAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var currentToken = await _tokenStorage.GetTokenAsync();
            if (currentToken == null || string.IsNullOrEmpty(currentToken.RefreshToken))
            {
                _logger.LogWarning("No refresh token available");
                return false;
            }

            _logger.LogInformation("Refreshing access token");

            // Mock refresh - in production, call refresh endpoint
            var newToken = new AuthToken
            {
                AccessToken = GenerateMockJwtToken("refreshed.user@domain.com"),
                RefreshToken = currentToken.RefreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                TokenType = "Bearer"
            };

            await _tokenStorage.SaveTokenAsync(newToken);
            _currentUser = await ExtractUserFromTokenAsync(newToken.AccessToken);
            
            _logger.LogInformation("Token refreshed successfully");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Token refresh failed");
            return false;
        }
    }

    public async Task LogoutAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Logging out user");
            await _tokenStorage.ClearTokenAsync();
            _currentUser = null;
            _logger.LogInformation("Logout successful");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Logout failed");
        }
    }

    public async Task<User?> GetCurrentUserAsync(CancellationToken cancellationToken = default)
    {
        if (_currentUser != null)
            return _currentUser;

        var token = await _tokenStorage.GetTokenAsync();
        if (token == null)
            return null;

        _currentUser = await ExtractUserFromTokenAsync(token.AccessToken);
        return _currentUser;
    }

    private async Task<User> ExtractUserFromTokenAsync(string token)
    {
        await Task.CompletedTask;
        
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            return new User
            {
                Id = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value ?? Guid.NewGuid().ToString(),
                Email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value ?? "user@example.com",
                Name = jwtToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? "Demo User",
                Roles = jwtToken.Claims.Where(c => c.Type == "role").Select(c => c.Value).ToArray(),
                Claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value)
            };
        }
        catch
        {
            return new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = "user@example.com",
                Name = "Demo User",
                Roles = new[] { "User" }
            };
        }
    }

    private string GenerateMockJwtToken(string email)
    {
        // This is a mock token for demo purposes
        // In production, this would come from your auth server
        var header = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("{\"alg\":\"HS256\",\"typ\":\"JWT\"}"));
        var payload = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(
            $"{{\"sub\":\"123\",\"email\":\"{ email}\",\"name\":\"Demo User\",\"role\":\"User\",\"exp\":{DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds()}}}"));
        var signature = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("mock_signature"));
        
        return $"{header}.{payload}.{signature}";
    }
}
