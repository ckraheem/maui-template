using CommunityToolkit.Maui;
using MauiTemplate.Infrastructure.Auth;
using MauiTemplate.Infrastructure.Database;
using MauiTemplate.Infrastructure.Extensions;
using MauiTemplate.Infrastructure.Navigation;
using MauiTemplate.Services;
using MauiTemplate.ViewModels;
using MauiTemplate.Views.Pages;
using Microsoft.Extensions.Logging;


namespace MauiTemplate;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureLogging()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Configure HttpClient
        builder.Services.AddHttpClient<IApiService, ApiService>(client =>
        {
            client.BaseAddress = new Uri("https://api.example.com/");
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        // Register Infrastructure Services
        builder.Services.AddSingleton<ITokenStorageService, TokenStorageService>();
        builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<ILoadingService, LoadingService>();
        
        // Register Database Services
        builder.Services.AddSingleton(typeof(IDatabaseService<>), typeof(DatabaseService<>));
        
        // Register Application Services
        builder.Services.AddTransient<IApiService, ApiService>();

        // Register ViewModels
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<ItemsViewModel>();
        builder.Services.AddTransient<ItemDetailViewModel>();

        // Register Pages
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<ItemsPage>();
        builder.Services.AddTransient<ItemDetailPage>();
        
        // Register Shell
        builder.Services.AddSingleton<AppShell>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        // Initialize error boundary
        var logger = app.Services.GetRequiredService<ILogger<App>>();
        ErrorBoundary.Initialize(logger);

        return app;
    }
}
