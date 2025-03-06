using SportSphere.App.ViewModels;
using SportSphere.App.Extensions;

namespace SportSphere.App.Views;

public partial class VenuesPage : ContentPage
{
    private readonly VenuesViewModel _viewModel;
    
    public VenuesPage(VenuesViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.RefreshCommand.ExecuteAsync(null);
    }
} 