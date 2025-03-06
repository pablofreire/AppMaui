using System.Collections.ObjectModel;
using System.Windows.Input;
using SportSphere.App.Services;
using SportSphere.Shared.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using SportSphere.App.Extensions;
using System.Linq;

namespace SportSphere.App.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private readonly AuthService _authService;
        private ObservableCollection<SportModel> _popularSports;
        private ObservableCollection<EventModel> _upcomingEvents;
        private ObservableCollection<VenueModel> _popularVenues;

        public MainViewModel(ApiService apiService, AuthService authService)
        {
            _apiService = apiService;
            _authService = authService;
            
            _popularSports = new ObservableCollection<SportModel>();
            _upcomingEvents = new ObservableCollection<EventModel>();
            _popularVenues = new ObservableCollection<VenueModel>();
            
            Title = "Início";
            
            // Comandos
            GoToProfileCommand = new Command(async () => await GoToProfileAsync());
            GoToEventsCommand = new Command(async () => await GoToEventsAsync());
            GoToVenuesCommand = new Command(async () => await GoToVenuesAsync());
            
            SportSelectedCommand = new Command<SportModel>(async (sport) => await OnSportSelectedAsync(sport));
            EventSelectedCommand = new Command<EventModel>(async (evt) => await OnEventSelectedAsync(evt));
            VenueSelectedCommand = new Command<VenueModel>(async (venue) => await OnVenueSelectedAsync(venue));
            LoadDataCommand = new Command(async () => await LoadDataAsync());
            
            // Carregar dados
            LoadDataAsync();
        }

        public ObservableCollection<SportModel> PopularSports
        {
            get => _popularSports;
            set => SetProperty(ref _popularSports, value);
        }

        public ObservableCollection<EventModel> UpcomingEvents
        {
            get => _upcomingEvents;
            set => SetProperty(ref _upcomingEvents, value);
        }

        public ObservableCollection<VenueModel> PopularVenues
        {
            get => _popularVenues;
            set => SetProperty(ref _popularVenues, value);
        }

        public ICommand GoToProfileCommand { get; }
        public ICommand GoToEventsCommand { get; }
        public ICommand GoToVenuesCommand { get; }
        public ICommand SportSelectedCommand { get; }
        public ICommand EventSelectedCommand { get; }
        public ICommand VenueSelectedCommand { get; }
        public ICommand LoadDataCommand { get; }

        private async Task LoadDataAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                await Task.WhenAll(
                    LoadPopularSportsAsync(),
                    LoadUpcomingEventsAsync(),
                    LoadPopularVenuesAsync()
                );
            }
            catch (Exception ex)
            {
                await ShowAlertAsync("Erro", $"Não foi possível carregar os dados: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LoadPopularSportsAsync()
        {
            try
            {
                var sportsDto = await _apiService.GetPopularSportsAsync();
                PopularSports.Clear();
                
                foreach (var sportDto in sportsDto)
                {
                    PopularSports.Add(sportDto.ToModel());
                }
            }
            catch (Exception ex)
            {
                await ShowAlertAsync("Erro", $"Não foi possível carregar os esportes populares: {ex.Message}", "OK");
            }
        }

        private async Task LoadUpcomingEventsAsync()
        {
            try
            {
                var eventsDto = await _apiService.GetUpcomingEventsAsync();
                UpcomingEvents.Clear();
                
                foreach (var eventDto in eventsDto)
                {
                    UpcomingEvents.Add(eventDto.ToModel());
                }
            }
            catch (Exception ex)
            {
                await ShowAlertAsync("Erro", $"Não foi possível carregar os eventos próximos: {ex.Message}", "OK");
            }
        }

        private async Task LoadPopularVenuesAsync()
        {
            try
            {
                var venuesDto = await _apiService.GetPopularVenuesAsync();
                PopularVenues.Clear();
                
                foreach (var venueDto in venuesDto)
                {
                    PopularVenues.Add(venueDto.ToModel());
                }
            }
            catch (Exception ex)
            {
                await ShowAlertAsync("Erro", $"Não foi possível carregar os locais populares: {ex.Message}", "OK");
            }
        }

        private async Task GoToProfileAsync()
        {
            await Shell.Current.GoToAsync("profile");
        }

        private async Task GoToEventsAsync()
        {
            await Shell.Current.GoToAsync("events");
        }

        private async Task GoToVenuesAsync()
        {
            await Shell.Current.GoToAsync("venues");
        }

        private async Task OnSportSelectedAsync(SportModel selectedSport)
        {
            if (selectedSport == null)
                return;

            var parameters = new Dictionary<string, object>
            {
                { "SportId", selectedSport.Id }
            };
            
            await Shell.Current.GoToAsync($"sportdetail", parameters);
        }

        private async Task OnEventSelectedAsync(EventModel selectedEvent)
        {
            if (selectedEvent == null)
                return;

            var parameters = new Dictionary<string, object>
            {
                { "EventId", selectedEvent.Id }
            };
            
            await Shell.Current.GoToAsync($"eventdetail", parameters);
        }

        private async Task OnVenueSelectedAsync(VenueModel selectedVenue)
        {
            if (selectedVenue == null)
                return;

            var parameters = new Dictionary<string, object>
            {
                { "VenueId", selectedVenue.Id }
            };
            
            await Shell.Current.GoToAsync($"venuedetail", parameters);
        }
    }
} 