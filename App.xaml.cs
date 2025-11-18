using MauiTemplate.Infrastructure.Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MauiTemplate;

public partial class App : Application
{
    private readonly ILogger<App> _logger;
    private readonly IAuthenticationService _authService;

    public App(ILogger<App> logger, IAuthenticationService authService)
    {
        InitializeComponent();
        _logger = logger;
        _authService = authService;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var shell = Handler!.MauiContext!.Services.GetRequiredService<AppShell>();
        
        // Handle deep linking
        if (activationState != null && activationState.State.TryGetValue("uri", out var uri))
        {
            _logger.LogInformation("App launched with deep link: {Uri}", uri);
            HandleDeepLink(uri.ToString()!);
        }

        return new Window(shell);
    }

    protected override void OnStart()
    {
        base.OnStart();
        _logger.LogInformation("App started");
        
        // Check authentication and navigate accordingly
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            if (!_authService.IsAuthenticated)
            {
                await Shell.Current.GoToAsync("//login");
            }
        });
    }

    protected override void OnSleep()
    {
        base.OnSleep();
        _logger.LogInformation("App suspended");
    }

    protected override void OnResume()
    {
        base.OnResume();
        _logger.LogInformation("App resumed");
        
        // Refresh token if needed
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                await _authService.RefreshTokenAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to refresh token on resume");
            }
        });
    }

    private void HandleDeepLink(string uri)
    {
        _logger.LogInformation("Handling deep link: {Uri}", uri);
        
        // Parse and navigate based on deep link
        // Example: myapp://items/123
        if (Uri.TryCreate(uri, UriKind.Absolute, out var parsedUri))
        {
            var path = parsedUri.PathAndQuery;
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync(path);
            });
        }
    }
}