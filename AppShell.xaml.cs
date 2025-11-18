using MauiTemplate.Infrastructure.Auth;
using MauiTemplate.Views.Pages;

namespace MauiTemplate;

public partial class AppShell : Shell
{
    private readonly IAuthenticationService _authService;

    public AppShell(IAuthenticationService authService)
    {
        InitializeComponent();
        _authService = authService;
        
        // Register routes for navigation
        Routing.RegisterRoute("itemdetail", typeof(ItemDetailPage));
        Routing.RegisterRoute("login", typeof(LoginPage));
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Settings", "Settings page coming soon!", "OK");
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        var confirm = await DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
        
        if (confirm)
        {
            await _authService.LogoutAsync();
            await Shell.Current.GoToAsync("//login");
        }
    }
}
