# .NET MAUI App Template

A comprehensive, production-ready .NET MAUI application template with best practices, modern architecture, and enterprise-grade features.

## 🚀 Features

### Architecture & Patterns
- **MVVM Architecture** with CommunityToolkit.Mvvm
- **Dependency Injection** with Microsoft.Extensions.DependencyInjection
- **Repository Pattern** for data access
- **Service Layer** for business logic
- **Navigation Service** for ViewModel-based navigation

### Navigation
- **Shell Flyout Navigation** with customizable menu
- **Deep Linking** support with URI scheme handling
- **Modal & Popup** navigation
- **Bottom Sheet** support via CommunityToolkit.Maui

### Authentication & Security
- **Multi-Provider Authentication**
  - JWT Token authentication
  - Azure AD integration (MSAL ready)
  - Auth0 integration support
- **Secure Token Storage** with SecureStorage API
- **Automatic Token Refresh** mechanism
- **Role-Based UI** with visibility converters

### Data Management
- **HttpClient API Integration** with typed services
- **SQLite Offline Cache** with async operations
- **Network Connectivity** awareness
- **Automatic Cache Sync** when online

### UI/UX Features
- **x:DataType Bindings** for compile-time safety and performance
- **Global Loading Indicator** service
- **Pull-to-Refresh** on list views
- **Swipe Actions** on list items
- **Custom Converters** for data binding
- **Responsive Layouts** for different screen sizes

### Developer Experience
- **Serilog Logging** with file and debug output
- **Global Error Boundary** for unhandled exceptions
- **Lifecycle Hooks** for app state management
- **Async/Await** patterns throughout
- **Cancellation Token** support for operations
- **Performance Helpers** for optimization

### Performance Optimizations
- **Fast Startup** with lazy loading
- **Low Memory Footprint** with efficient caching
- **Connection Pooling** for HTTP requests
- **Image Caching** support
- **Compiled Bindings** with x:DataType

## 📋 Prerequisites

- .NET 10.0 SDK or later
- Visual Studio 2022 17.12+ or VS Code with C# Dev Kit
- Android SDK (for Android development)
- Xcode (for iOS/macOS development on Mac)
- Windows 11 SDK (for Windows development)

## 🏗️ Project Structure

```
MauiTemplate/
├── Behaviors/              # XAML behaviors
├── Converters/             # Value converters
├── Helpers/                # Utility classes
├── Infrastructure/         # Core infrastructure
│   ├── Auth/              # Authentication services
│   ├── Database/          # SQLite database services
│   ├── Extensions/        # Extension methods
│   ├── Logging/           # Logging configuration
│   └── Navigation/        # Navigation services
├── Models/                 # Data models
├── Resources/              # Images, fonts, styles
├── Services/               # Application services
├── ViewModels/            # MVVM ViewModels
└── Views/                 # XAML pages and views
    ├── Modals/
    ├── Pages/
    └── Popups/
```

## 🚦 Getting Started

### 1. Clone and Restore

```bash
git clone <repository-url>
cd maui-template
dotnet restore
```

### 2. Configure API Endpoint

Update the base URL in `MauiProgram.cs`:

```csharp
builder.Services.AddHttpClient<IApiService, ApiService>(client =>
{
    client.BaseAddress = new Uri("https://your-api.com/");
    client.Timeout = TimeSpan.FromSeconds(30);
});
```

### 3. Configure Authentication

Choose and configure your authentication provider in `Infrastructure/Auth/`:

**For JWT:**
- Update login endpoint in `AuthenticationService.cs`
- Configure token validation parameters

**For Azure AD:**
- Install `Microsoft.Identity.Client`
- Configure tenant ID and client ID
- Update `LoginWithAzureAdAsync` method

**For Auth0:**
- Install Auth0 SDK
- Configure domain and client ID
- Update `LoginWithAuth0Async` method

### 4. Build and Run

```bash
# For Android
dotnet build -t:Run -f net10.0-android

# For iOS
dotnet build -t:Run -f net10.0-ios

# For Windows
dotnet build -t:Run -f net10.0-windows10.0.19041.0
```

## 📱 Key Components

### Authentication Service

```csharp
public interface IAuthenticationService
{
    Task<bool> LoginAsync(string username, string password, CancellationToken cancellationToken = default);
    Task<bool> LoginWithAzureAdAsync(CancellationToken cancellationToken = default);
    Task<bool> LoginWithAuth0Async(CancellationToken cancellationToken = default);
    Task<bool> RefreshTokenAsync(CancellationToken cancellationToken = default);
    Task LogoutAsync(CancellationToken cancellationToken = default);
    Task<User?> GetCurrentUserAsync(CancellationToken cancellationToken = default);
    bool IsAuthenticated { get; }
}
```

### API Service

```csharp
public interface IApiService
{
    Task<T?> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default);
    Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken = default);
    Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string endpoint, CancellationToken cancellationToken = default);
}
```

### Navigation Service

```csharp
public interface INavigationService
{
    Task NavigateToAsync(string route, CancellationToken cancellationToken = default);
    Task NavigateToAsync(string route, IDictionary<string, object> parameters, CancellationToken cancellationToken = default);
    Task GoBackAsync(CancellationToken cancellationToken = default);
    Task NavigateToModalAsync(string route, CancellationToken cancellationToken = default);
    Task CloseModalAsync(CancellationToken cancellationToken = default);
}
```

### Database Service

```csharp
public interface IDatabaseService<T> where T : class, new()
{
    Task InitializeAsync();
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(string id);
    Task<int> InsertAsync(T item);
    Task<int> UpdateAsync(T item);
    Task<int> DeleteAsync(T item);
}
```

## 🎨 Customization

### Adding a New Page

1. Create ViewModel in `ViewModels/`:
```csharp
public partial class MyViewModel : BaseViewModel
{
    public MyViewModel(ILogger<MyViewModel> logger) : base(logger)
    {
        Title = "My Page";
    }
}
```

2. Create View in `Views/Pages/`:
```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             x:Class="MauiTemplate.Views.Pages.MyPage"
             x:DataType="vm:MyViewModel"
             Title="{Binding Title}">
    <!-- Your XAML -->
</ContentPage>
```

3. Register in `MauiProgram.cs`:
```csharp
builder.Services.AddTransient<MyViewModel>();
builder.Services.AddTransient<MyPage>();
```

4. Add route in `AppShell.xaml.cs`:
```csharp
Routing.RegisterRoute("mypage", typeof(MyPage));
```

### Adding a New Service

1. Create interface in `Services/`:
```csharp
public interface IMyService
{
    Task DoSomethingAsync();
}
```

2. Implement service:
```csharp
public class MyService : IMyService
{
    public async Task DoSomethingAsync()
    {
        // Implementation
    }
}
```

3. Register in `MauiProgram.cs`:
```csharp
builder.Services.AddSingleton<IMyService, MyService>();
```

## 🔒 Security Best Practices

1. **Never commit secrets** - Use user secrets or environment variables
2. **Use SecureStorage** for sensitive data (tokens, keys)
3. **Implement certificate pinning** for production APIs
4. **Validate all user input** before processing
5. **Use parameterized queries** with SQLite
6. **Enable ProGuard/R8** for Android release builds
7. **Implement app transport security** for iOS

## 📊 Logging

Logs are written to:
- Debug output (during development)
- File system: `{AppDataDirectory}/logs/app-{date}.log`
- Retained for 7 days

Example usage:
```csharp
_logger.LogInformation("User logged in: {UserId}", userId);
_logger.LogWarning("API call failed, using cache");
_logger.LogError(ex, "Unhandled exception in service");
```

## 🧪 Testing

The template is designed to be testable:

- ViewModels are decoupled from Views
- Services use dependency injection
- Interfaces enable mocking
- Async operations support cancellation

## 📝 Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## 📄 License

This template is provided as-is for use in your projects.

## 🆘 Support

For issues and questions:
- Create an issue in the repository
- Check existing documentation
- Review the code examples

## 🎯 Roadmap

Future enhancements:
- [ ] Biometric authentication
- [ ] Push notifications
- [ ] Background sync
- [ ] Offline-first architecture
- [ ] Unit test examples
- [ ] UI test examples
- [ ] CI/CD pipeline templates

---

Built with ❤️ using .NET MAUI
