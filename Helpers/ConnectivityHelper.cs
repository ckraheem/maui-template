namespace MauiTemplate.Helpers;

/// <summary>
/// Helper class for checking network connectivity
/// </summary>
public static class ConnectivityHelper
{
    /// <summary>
    /// Checks if the device has internet connectivity
    /// </summary>
    public static bool IsConnected => Connectivity.Current.NetworkAccess == NetworkAccess.Internet;

    /// <summary>
    /// Gets the current connection profiles
    /// </summary>
    public static IEnumerable<ConnectionProfile> ConnectionProfiles => Connectivity.Current.ConnectionProfiles;

    /// <summary>
    /// Executes an action only if connected to the internet
    /// </summary>
    public static async Task<T?> ExecuteIfConnectedAsync<T>(Func<Task<T>> onlineAction, Func<Task<T>>? offlineAction = null)
    {
        if (IsConnected)
        {
            return await onlineAction();
        }
        else if (offlineAction != null)
        {
            return await offlineAction();
        }
        
        return default;
    }
}
