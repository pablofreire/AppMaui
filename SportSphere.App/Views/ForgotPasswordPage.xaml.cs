using SportSphere.App.ViewModels;

namespace SportSphere.App.Views;

public partial class ForgotPasswordPage : ContentPage
{
    private readonly ForgotPasswordViewModel _viewModel;
    
    public ForgotPasswordPage(ForgotPasswordViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.Email = string.Empty;
        _viewModel.ErrorMessage = string.Empty;
    }
} 