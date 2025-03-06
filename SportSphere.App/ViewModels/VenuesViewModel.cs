using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using SportSphere.App.Services;
using SportSphere.Shared.Models;
using SportSphere.App.Extensions;
using System.Linq;

namespace SportSphere.App.ViewModels
{
    public class VenuesViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private ObservableCollection<VenueModel> _venues;
        private bool _isRefreshing;
        private string _searchText;

        public VenuesViewModel(ApiService apiService)
        {
            _apiService = apiService;
            _venues = new ObservableCollection<VenueModel>();
            
            Title = "Locais";
            
            RefreshCommand = new Command(async () => await RefreshVenuesAsync());
            VenueSelectedCommand = new Command<VenueModel>(async (venue) => await OnVenueSelectedAsync(venue));
            SearchCommand = new Command(async () => await SearchVenuesAsync());
            
            LoadVenuesAsync();
        }

        public ObservableCollection<VenueModel> Venues
        {
            get => _venues;
            set => SetProperty(ref _venues, value);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public ICommand RefreshCommand { get; }
        public ICommand VenueSelectedCommand { get; }
        public ICommand SearchCommand { get; }

        private async Task LoadVenuesAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var venuesDto = await _apiService.GetVenuesAsync();
                Venues.Clear();
                
                foreach (var venueDto in venuesDto)
                {
                    Venues.Add(venueDto.ToModel());
                }
            }
            catch (Exception ex)
            {
                await ShowAlertAsync("Erro", $"Não foi possível carregar os locais: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }

        private async Task RefreshVenuesAsync()
        {
            IsRefreshing = true;
            await LoadVenuesAsync();
        }

        private async Task OnVenueSelectedAsync(VenueModel selectedVenue)
        {
            if (selectedVenue == null)
                return;

            // Navegar para a página de detalhes do local
            var parameters = new Dictionary<string, object>
            {
                { "Venue", selectedVenue }
            };

            await Shell.Current.GoToAsync($"venuedetails?id={selectedVenue.Id}");
        }

        private async Task SearchVenuesAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var venuesDto = string.IsNullOrWhiteSpace(SearchText)
                    ? await _apiService.GetVenuesAsync()
                    : await _apiService.SearchVenuesAsync(SearchText);

                Venues.Clear();
                
                foreach (var venueDto in venuesDto)
                {
                    Venues.Add(venueDto.ToModel());
                }
            }
            catch (Exception ex)
            {
                await ShowAlertAsync("Erro", $"Não foi possível pesquisar os locais: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
} 