using Microsoft.Extensions.Logging;

namespace MauiTemplate.Infrastructure.Extensions;

/// <summary>
/// Extension methods for optimizing app startup
/// </summary>
public static class StartupExtensions
{
    /// <summary>
    /// Performs startup initialization tasks
    /// </summary>
    public static async Task InitializeAsync(this IServiceProvider services)
    {
        var logger = services.GetRequiredService<ILogger<App>>();
        
        try
        {
            logger.LogInformation("Starting app initialization...");

            // Warm up critical services in parallel
            var tasks = new List<Task>
            {
                Task.Run(async () => await InitializeDatabaseAsync(services, logger)),
                Task.Run(async () => await InitializeAuthenticationAsync(services, logger)),
                Task.Run(() => InitializeAppCenter(logger))
            };

            await Task.WhenAll(tasks);

            logger.LogInformation("App initialization completed");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "App initialization failed");
            throw;
        }
    }

    private static async Task InitializeDatabaseAsync(IServiceProvider services, ILogger logger)
    {
        try
        {
            logger.LogDebug("Initializing database...");
            
            // Initialize database tables
            // Add your database initialization here
            
            await Task.CompletedTask;
            logger.LogDebug("Database initialized");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Database initialization failed");
        }
    }

    private static async Task InitializeAuthenticationAsync(IServiceProvider services, ILogger logger)
    {
        try
        {
            logger.LogDebug("Checking authentication state...");
            
            // Check for existing auth token
            // Refresh if needed
            
            await Task.CompletedTask;
            logger.LogDebug("Authentication check completed");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Authentication initialization failed");
        }
    }

    private static void InitializeAppCenter(ILogger logger)
    {
        try
        {
            logger.LogDebug("Initializing analytics...");
            
            // Initialize AppCenter or other analytics
            // This is a placeholder - implement based on your needs
            
            logger.LogDebug("Analytics initialized");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Analytics initialization failed");
        }
    }

    /// <summary>
    /// Preloads resources to improve perceived performance
    /// </summary>
    public static void PreloadResources(this IServiceProvider services)
    {
        var logger = services.GetRequiredService<ILogger<App>>();
        
        try
        {
            logger.LogDebug("Preloading resources...");
            
            // Preload fonts
            // Preload images
            // Warm up ViewModels
            
            logger.LogDebug("Resources preloaded");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Resource preloading failed");
        }
    }
}
