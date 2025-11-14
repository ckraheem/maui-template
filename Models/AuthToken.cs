namespace MauiTemplate.Models;

public class AuthToken
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public string TokenType { get; set; } = "Bearer";
    public string[] Scopes { get; set; } = Array.Empty<string>();

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt.AddMinutes(-5);
}
