# Quick Start Guide

Get up and running with the MAUI Template in minutes!

## Prerequisites

Before you begin, ensure you have:
- [.NET 10 SDK](https://dotnet.microsoft.com/download) installed
- [Visual Studio 2022 (17.12+)](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- Android SDK for Android development
- Xcode for iOS/macOS (Mac only)

## Step 1: Clone the Template

```bash
git clone https://github.com/ckraheem/maui-template.git
cd maui-template
```

## Step 2: Restore Dependencies

```bash
dotnet restore
```

## Step 3: Configure Your API (Optional)

If you have a backend API, update the base URL in `MauiProgram.cs`:

```csharp
builder.Services.AddHttpClient<IApiService, ApiService>(client =>
{
    client.BaseAddress = new Uri("https://your-api-endpoint.com/");
    client.Timeout = TimeSpan.FromSeconds(30);
});
```

## Step 4: Choose Your Platform and Run

### Android
```bash
dotnet build -t:Run -f net10.0-android
```

### iOS (Mac only)
```bash
dotnet build -t:Run -f net10.0-ios
```

### Windows
```bash
dotnet build -t:Run -f net10.0-windows10.0.19041.0
```

### macOS (Mac only)
```bash
dotnet build -t:Run -f net10.0-maccatalyst
```

## Step 5: Explore the Template

### Login Page
The app starts with a login page featuring:
- Traditional username/password login
- Azure AD login button (configure in `AuthenticationService.cs`)
- Auth0 login button (configure in `AuthenticationService.cs`)

**Demo Credentials**: Any username/password will work in the demo mode.

### Items List
After login, you'll see:
- A list of sample items
- Pull-to-refresh functionality
- Swipe actions on items
- Add button to create new items

### Item Detail
Tap any item to view/edit:
- Title and description fields
- Save, Cancel, and Delete buttons
- Automatic navigation back on save

### Navigation Menu
Tap the hamburger menu (☰) to access:
- Home
- Items
- Settings
- Logout

## Common Customizations

### Change App Name
Update in `MauiTemplate.csproj`:
```xml
<ApplicationTitle>Your App Name</ApplicationTitle>
<ApplicationId>com.yourcompany.yourapp</ApplicationId>
```

### Change Theme Colors
Edit `Resources/Styles/Colors.xaml`:
```xml
<Color x:Key="Primary">#512BD4</Color>
<Color x:Key="Secondary">#DFD8F7</Color>
```

### Add New Page

1. **Create ViewModel**:
```csharp
// ViewModels/MyViewModel.cs
public partial class MyViewModel : BaseViewModel
{
    public MyViewModel(ILogger<MyViewModel> logger) : base(logger)
    {
        Title = "My Page";
    }
}
```

2. **Create View**:
```xml
<!-- Views/Pages/MyPage.xaml -->
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             x:DataType="vm:MyViewModel"
             Title="{Binding Title}">
    <Label Text="Hello from My Page!" />
</ContentPage>
```

3. **Register in DI** (`MauiProgram.cs`):
```csharp
builder.Services.AddTransient<MyViewModel>();
builder.Services.AddTransient<MyPage>();
```

4. **Add Route** (`AppShell.xaml.cs`):
```csharp
Routing.RegisterRoute("mypage", typeof(MyPage));
```

5. **Navigate to it**:
```csharp
await _navigationService.NavigateToAsync("mypage");
```

## Testing the Template

### Test Authentication
1. Launch the app
2. Enter any username/password
3. Tap "Sign In"
4. You should navigate to the Items page

### Test Offline Mode
1. Enable airplane mode on your device
2. Pull to refresh on Items page
3. App should load from SQLite cache

### Test Deep Linking
```bash
# Android
adb shell am start -W -a android.intent.action.VIEW -d "mauitemplate://items"

# iOS (simulator)
xcrun simctl openurl booted "mauitemplate://items"
```

## Troubleshooting

### Build Fails with Missing Workload
```bash
# Install MAUI workload
dotnet workload install maui-android
```

### "Unable to find package" Error
```bash
# Clear NuGet cache
dotnet nuget locals all --clear
dotnet restore
```

### Android Deployment Issues
1. Ensure Android SDK is installed
2. Check device is connected: `adb devices`
3. Try clean build: `dotnet clean && dotnet build`

### iOS/Mac Issues
1. Ensure Xcode is installed
2. Accept Xcode license: `sudo xcodebuild -license accept`
3. Install iOS simulators via Xcode

## Next Steps

- Read the [Architecture Guide](ARCHITECTURE.md)
- Explore the [Full Documentation](README.md)
- Check out the [Contributing Guide](CONTRIBUTING.md)
- Configure authentication providers
- Customize UI theme
- Add your business logic

## Getting Help

- 📖 Check the documentation files
- 🐛 Report issues on GitHub
- 💬 Ask questions in discussions
- 📧 Contact the maintainers

Happy coding! 🚀
