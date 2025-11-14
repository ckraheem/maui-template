using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiTemplate.Infrastructure.Auth;
using MauiTemplate.Infrastructure.Navigation;
using Microsoft.Extensions.Logging;

namespace MauiTemplate.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    private readonly IAuthenticationService _authService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private bool _rememberMe;

    public LoginViewModel(
        IAuthenticationService authService,
        INavigationService navigationService,
        ILogger<LoginViewModel> logger) : base(logger)
    {
        _authService = authService;
        _navigationService = navigationService;
        Title = "Login";
    }

    [RelayCommand]
    private async Task LoginAsync(CancellationToken cancellationToken)
    {
        await ExecuteAsync(async () =>
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current!.MainPage!.DisplayAlert(
                    "Validation Error", 
                    "Please enter username and password", 
                    "OK");
                return;
            }

            var success = await _authService.LoginAsync(Username, Password, cancellationToken);
            
            if (success)
            {
                await _navigationService.NavigateToAsync("//main/items");
            }
            else
            {
                await Application.Current!.MainPage!.DisplayAlert(
                    "Login Failed", 
                    "Invalid username or password", 
                    "OK");
            }
        }, "Login failed");
    }

    [RelayCommand]
    private async Task LoginWithAzureAdAsync(CancellationToken cancellationToken)
    {
        await ExecuteAsync(async () =>
        {
            var success = await _authService.LoginWithAzureAdAsync(cancellationToken);
            
            if (success)
            {
                await _navigationService.NavigateToAsync("//main/items");
            }
            else
            {
                await Application.Current!.MainPage!.DisplayAlert(
                    "Login Failed", 
                    "Azure AD login failed", 
                    "OK");
            }
        }, "Azure AD login failed");
    }

    [RelayCommand]
    private async Task LoginWithAuth0Async(CancellationToken cancellationToken)
    {
        await ExecuteAsync(async () =>
        {
            var success = await _authService.LoginWithAuth0Async(cancellationToken);
            
            if (success)
            {
                await _navigationService.NavigateToAsync("//main/items");
            }
            else
            {
                await Application.Current!.MainPage!.DisplayAlert(
                    "Login Failed", 
                    "Auth0 login failed", 
                    "OK");
            }
        }, "Auth0 login failed");
    }
}
