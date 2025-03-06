using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using SportSphere.App.Services;
using SportSphere.Shared.DTOs;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace SportSphere.App.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private readonly LocationService _locationService;
        private readonly AuthService _authService;
        private Map _map;
        private double _latitude;
        private double _longitude;
        private double _radius = 10; // Default radius in km

        public Map Map
        {
            get => _map;
            set => SetProperty(ref _map, value);
        }

        public double Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }

        public double Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        public double Radius
        {
            get => _radius;
            set => SetProperty(ref _radius, value);
        }

        public ObservableCollection<EventDto> NearbyEvents { get; } = new ObservableCollection<EventDto>();
        public ObservableCollection<VenueDto> NearbyVenues { get; } = new ObservableCollection<VenueDto>();

        public ICommand LoadMapCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand CreateEventCommand { get; }
        public ICommand ViewEventDetailsCommand { get; }
        public ICommand ViewVenueDetailsCommand { get; }

        public MapViewModel(ApiService apiService, LocationService locationService, AuthService authService)
        {
            _apiService = apiService;
            _locationService = locationService;
            _authService = authService;
            Title = "Map";

            LoadMapCommand = new Command(async () => await LoadMapAsync());
            RefreshCommand = new Command(async () => await RefreshAsync());
            CreateEventCommand = new Command(async () => await CreateEventAsync());
            ViewEventDetailsCommand = new Command<EventDto>(async (eventDto) => await ViewEventDetailsAsync(eventDto));
            ViewVenueDetailsCommand = new Command<VenueDto>(async (venueDto) => await ViewVenueDetailsAsync(venueDto));
        }

        private async Task LoadMapAsync()
        {
            await DisplayLoading("Loading map...");

            try
            {
                var hasPermission = await _locationService.CheckLocationPermissionAsync();
                if (!hasPermission)
                {
                    await DisplayAlert("Permission Denied", "Location permission is required to use the map.", "OK");
                    return;
                }

                var location = await _locationService.GetCurrentLocationAsync();
                Latitude = location.Latitude;
                Longitude = location.Longitude;

                Map = new Map
                {
                    IsShowingUser = true,
                    MapType = MapType.Street
                };

                var position = new Location(Latitude, Longitude);
                Map.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(Radius)));

                await LoadNearbyEventsAndVenuesAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
            finally
            {
                HideLoading();
            }
        }

        private async Task RefreshAsync()
        {
            await DisplayLoading("Refreshing...");

            try
            {
                await LoadNearbyEventsAndVenuesAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
            finally
            {
                HideLoading();
            }
        }

        private async Task LoadNearbyEventsAndVenuesAsync()
        {
            NearbyEvents.Clear();
            NearbyVenues.Clear();

            var events = await _apiService.GetNearbyEventsAsync(Latitude, Longitude, Radius);
            foreach (var eventDto in events)
            {
                NearbyEvents.Add(eventDto);
                AddEventPin(eventDto);
            }

            var venues = await _apiService.GetNearbyVenuesAsync(Latitude, Longitude, Radius);
            foreach (var venueDto in venues)
            {
                NearbyVenues.Add(venueDto);
                AddVenuePin(venueDto);
            }
        }

        private void AddEventPin(EventDto eventDto)
        {
            if (Map == null || eventDto?.Location == null)
                return;

            var pin = new Pin
            {
                Label = eventDto.Title,
                Address = eventDto.Location.Address,
                Location = new Location(eventDto.Location.Latitude, eventDto.Location.Longitude),
                Type = PinType.Place
            };

            pin.MarkerClicked += (s, e) =>
            {
                ViewEventDetailsCommand.Execute(eventDto);
            };

            Map.Pins.Add(pin);
        }

        private void AddVenuePin(VenueDto venueDto)
        {
            if (Map == null || venueDto?.Location == null)
                return;

            var pin = new Pin
            {
                Label = venueDto.Name,
                Address = venueDto.Address,
                Location = new Location(venueDto.Location.Latitude, venueDto.Location.Longitude),
                Type = PinType.Place
            };

            pin.MarkerClicked += (s, e) =>
            {
                ViewVenueDetailsCommand.Execute(venueDto);
            };

            Map.Pins.Add(pin);
        }

        private async Task CreateEventAsync()
        {
            if (!_authService.IsAuthenticated)
            {
                await DisplayAlert("Authentication Required", "Please login to create an event.", "OK");
                return;
            }

            await Shell.Current.GoToAsync("createevent");
        }

        private async Task ViewEventDetailsAsync(EventDto eventDto)
        {
            await Shell.Current.GoToAsync($"eventdetail?id={eventDto.Id}");
        }

        private async Task ViewVenueDetailsAsync(VenueDto venueDto)
        {
            await Shell.Current.GoToAsync($"venuedetail?id={venueDto.Id}");
        }
    }
} 