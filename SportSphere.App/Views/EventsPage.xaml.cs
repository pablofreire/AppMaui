using SportSphere.App.ViewModels;
using SportSphere.App.Extensions;

namespace SportSphere.App.Views;

public partial class EventsPage : ContentPage
{
    private readonly EventsViewModel _viewModel;
    
    public EventsPage(EventsViewModel viewModel)
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