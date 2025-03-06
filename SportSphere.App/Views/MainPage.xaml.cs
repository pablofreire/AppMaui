using SportSphere.App.ViewModels;
using SportSphere.App.Extensions;

namespace SportSphere.App.Views;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;
    
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadDataCommand.ExecuteAsync(null);
    }
} 