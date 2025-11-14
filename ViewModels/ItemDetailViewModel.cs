using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiTemplate.Infrastructure.Navigation;
using MauiTemplate.Models;
using MauiTemplate.Services;
using Microsoft.Extensions.Logging;

namespace MauiTemplate.ViewModels;

public partial class ItemDetailViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IApiService _apiService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private ListItem _item = new();

    [ObservableProperty]
    private bool _isEditMode;

    public ItemDetailViewModel(
        IApiService apiService,
        INavigationService navigationService,
        ILogger<ItemDetailViewModel> logger) : base(logger)
    {
        _apiService = apiService;
        _navigationService = navigationService;
        Title = "Item Details";
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("ItemId"))
        {
            var itemId = query["ItemId"].ToString();
            IsEditMode = true;
            _ = LoadItemAsync(itemId!);
        }
        else
        {
            IsEditMode = false;
            Item = new ListItem();
        }
    }

    private async Task LoadItemAsync(string itemId)
    {
        await ExecuteAsync(async () =>
        {
            var item = await _apiService.GetAsync<ListItem>($"items/{itemId}");
            if (item != null)
            {
                Item = item;
                Title = $"Edit {item.Title}";
            }
        }, "Failed to load item");
    }

    [RelayCommand]
    private async Task SaveAsync(CancellationToken cancellationToken)
    {
        await ExecuteAsync(async () =>
        {
            if (string.IsNullOrWhiteSpace(Item.Title))
            {
                await Application.Current!.MainPage!.DisplayAlert(
                    "Validation Error", 
                    "Title is required", 
                    "OK");
                return;
            }

            if (IsEditMode)
            {
                await _apiService.PutAsync<ListItem, ListItem>($"items/{Item.Id}", Item, cancellationToken);
            }
            else
            {
                await _apiService.PostAsync<ListItem, ListItem>("items", Item, cancellationToken);
            }

            await _navigationService.GoBackAsync(cancellationToken);
        }, "Failed to save item");
    }

    [RelayCommand]
    private async Task DeleteAsync(CancellationToken cancellationToken)
    {
        if (!IsEditMode)
            return;

        var confirm = await Application.Current!.MainPage!.DisplayAlert(
            "Confirm Delete", 
            $"Are you sure you want to delete {Item.Title}?", 
            "Yes", 
            "No");

        if (!confirm)
            return;

        await ExecuteAsync(async () =>
        {
            await _apiService.DeleteAsync($"items/{Item.Id}", cancellationToken);
            await _navigationService.GoBackAsync(cancellationToken);
        }, "Failed to delete item");
    }

    [RelayCommand]
    private async Task CancelAsync(CancellationToken cancellationToken)
    {
        await _navigationService.GoBackAsync(cancellationToken);
    }
}
