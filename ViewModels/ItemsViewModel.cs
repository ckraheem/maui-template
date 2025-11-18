using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiTemplate.Infrastructure.Database;
using MauiTemplate.Infrastructure.Navigation;
using MauiTemplate.Models;
using MauiTemplate.Services;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace MauiTemplate.ViewModels;

public partial class ItemsViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private readonly IDatabaseService<CachedListItem> _databaseService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private ObservableCollection<ListItem> _items = new();

    [ObservableProperty]
    private ListItem? _selectedItem;

    public ItemsViewModel(
        IApiService apiService,
        IDatabaseService<CachedListItem> databaseService,
        INavigationService navigationService,
        ILogger<ItemsViewModel> logger) : base(logger)
    {
        _apiService = apiService;
        _databaseService = databaseService;
        _navigationService = navigationService;
        Title = "Items";
    }

    public override async Task OnAppearingAsync(CancellationToken cancellationToken = default)
    {
        await base.OnAppearingAsync(cancellationToken);
        await LoadItemsAsync(cancellationToken);
    }

    [RelayCommand]
    private async Task LoadItemsAsync(CancellationToken cancellationToken)
    {
        await ExecuteAsync(async () =>
        {
            try
            {
                // Try to load from API
                var items = await _apiService.GetAsync<List<ListItem>>("items", cancellationToken);
                
                if (items != null && items.Any())
                {
                    Items.Clear();
                    foreach (var item in items)
                    {
                        Items.Add(item);
                    }

                    // Cache the items
                    await CacheItemsAsync(items);
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Failed to load from API, loading from cache");
                // Load from cache if API fails
                await LoadFromCacheAsync();
            }
        }, "Failed to load items");
    }

    [RelayCommand]
    private async Task RefreshAsync(CancellationToken cancellationToken)
    {
        IsRefreshing = true;
        await LoadItemsAsync(cancellationToken);
        IsRefreshing = false;
    }

    [RelayCommand]
    private async Task ItemSelectedAsync(CancellationToken cancellationToken)
    {
        if (SelectedItem == null)
            return;

        var parameters = new Dictionary<string, object>
        {
            { "ItemId", SelectedItem.Id }
        };

        await _navigationService.NavigateToAsync("itemdetail", parameters, cancellationToken);
        
        SelectedItem = null;
    }

    [RelayCommand]
    private async Task AddItemAsync(CancellationToken cancellationToken)
    {
        await _navigationService.NavigateToAsync("itemdetail", cancellationToken);
    }

    private async Task LoadFromCacheAsync()
    {
        var cachedItems = await _databaseService.GetAllAsync();
        Items.Clear();
        
        foreach (var cached in cachedItems)
        {
            Items.Add(new ListItem
            {
                Id = cached.Id,
                Title = cached.Title,
                Description = cached.Description,
                ImageUrl = cached.ImageUrl,
                CreatedAt = cached.CreatedAt,
                UpdatedAt = cached.UpdatedAt
            });
        }
    }

    private async Task CacheItemsAsync(List<ListItem> items)
    {
        await _databaseService.DeleteAllAsync();
        
        foreach (var item in items)
        {
            await _databaseService.InsertAsync(new CachedListItem
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                ImageUrl = item.ImageUrl,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt,
                CachedAt = DateTime.UtcNow
            });
        }
    }
}
