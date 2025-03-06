using SportSphere.App.ViewModels;

namespace SportSphere.App.Views;

public partial class RegisterPage : ContentPage
{
    private readonly RegisterViewModel _viewModel;
    
    public RegisterPage(RegisterViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.FirstName = string.Empty;
        _viewModel.LastName = string.Empty;
        _viewModel.Username = string.Empty;
        _viewModel.Email = string.Empty;
        _viewModel.Password = string.Empty;
        _viewModel.ConfirmPassword = string.Empty;
        _viewModel.ErrorMessage = string.Empty;
    }
} 