using Microsoft.Maui.Controls;
using SportSphere.App.ViewModels;
using SportSphere.App.Extensions;

namespace SportSphere.App.Views
{
    public partial class MapPage : ContentPage
    {
        private readonly MapViewModel _viewModel;

        public MapPage(MapViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadMapCommand.ExecuteAsync(null);
        }
    }
} 