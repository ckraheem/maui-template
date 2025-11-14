using MauiTemplate.ViewModels;

namespace MauiTemplate.Views.Pages;

public partial class ItemsPage : ContentPage
{
    public ItemsPage(ItemsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is BaseViewModel viewModel)
        {
            await viewModel.OnAppearingAsync();
        }
    }

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();
        if (BindingContext is BaseViewModel viewModel)
        {
            await viewModel.OnDisappearingAsync();
        }
    }
}
