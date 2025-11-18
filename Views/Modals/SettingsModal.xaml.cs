namespace MauiTemplate.Views.Modals;

public partial class SettingsModal : ContentPage
{
    public SettingsModal()
    {
        InitializeComponent();
    }

    private async void OnCloseClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
