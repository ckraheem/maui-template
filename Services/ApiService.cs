using MauiTemplate.Infrastructure.Auth;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MauiTemplate.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenStorageService _tokenStorage;
    private readonly ILogger<ApiService> _logger;

    public ApiService(
        HttpClient httpClient,
        ITokenStorageService tokenStorage,
        ILogger<ApiService> logger)
    {
        _httpClient = httpClient;
        _tokenStorage = tokenStorage;
        _logger = logger;
    }

    private async Task PrepareRequestAsync()
    {
        var token = await _tokenStorage.GetTokenAsync();
        if (token != null && !string.IsNullOrEmpty(token.AccessToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue(token.TokenType, token.AccessToken);
        }
    }

    public async Task<T?> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default)
    {
        try
        {
            await PrepareRequestAsync();
            _logger.LogInformation("GET request to: {Endpoint}", endpoint);

            var response = await _httpClient.GetAsync(endpoint, cancellationToken);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GET request failed: {Endpoint}", endpoint);
            throw;
        }
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(
        string endpoint, 
        TRequest data, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            await PrepareRequestAsync();
            _logger.LogInformation("POST request to: {Endpoint}", endpoint);

            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content, cancellationToken);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<TResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "POST request failed: {Endpoint}", endpoint);
            throw;
        }
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(
        string endpoint, 
        TRequest data, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            await PrepareRequestAsync();
            _logger.LogInformation("PUT request to: {Endpoint}", endpoint);

            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(endpoint, content, cancellationToken);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<TResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PUT request failed: {Endpoint}", endpoint);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(string endpoint, CancellationToken cancellationToken = default)
    {
        try
        {
            await PrepareRequestAsync();
            _logger.LogInformation("DELETE request to: {Endpoint}", endpoint);

            var response = await _httpClient.DeleteAsync(endpoint, cancellationToken);
            response.EnsureSuccessStatusCode();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DELETE request failed: {Endpoint}", endpoint);
            return false;
        }
    }
}
