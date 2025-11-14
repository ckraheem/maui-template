namespace MauiTemplate.Models;

public class User
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public string[] Roles { get; set; } = Array.Empty<string>();
    public Dictionary<string, string> Claims { get; set; } = new();

    public bool HasRole(string role) => 
        Roles?.Any(r => r.Equals(role, StringComparison.OrdinalIgnoreCase)) ?? false;
}
