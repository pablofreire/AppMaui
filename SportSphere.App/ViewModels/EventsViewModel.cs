using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Graphics;
using SportSphere.App.Services;
using SportSphere.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using SportSphere.App.Extensions;
using System.Linq;

namespace SportSphere.App.ViewModels
{
    public class EventsViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private readonly AuthService _authService;
        private ObservableCollection<EventModel> _events;
        private bool _isRefreshing;
        private string _currentFilter = "all";
        private Color _allFilterColor = Colors.DarkBlue;
        private Color _mineFilterColor = Colors.Gray;
        private Color _participatingFilterColor = Colors.Gray;

        public EventsViewModel(ApiService apiService, AuthService authService)
        {
            _apiService = apiService;
            _authService = authService;
            _events = new ObservableCollection<EventModel>();
            
            Title = "Eventos";
            
            RefreshCommand = new Command(async () => await RefreshEventsAsync());
            EventSelectedCommand = new Command<EventModel>(async (eventItem) => await OnEventSelectedAsync(eventItem));
            CreateEventCommand = new Command(async () => await OnCreateEventAsync());
            FilterEventsCommand = new Command<string>(async (filter) => await FilterEventsAsync(filter));
            
            LoadEventsAsync();
        }

        public ObservableCollection<EventModel> Events
        {
            get => _events;
            set => SetProperty(ref _events, value);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public Color AllFilterColor
        {
            get => _allFilterColor;
            set => SetProperty(ref _allFilterColor, value);
        }

        public Color MineFilterColor
        {
            get => _mineFilterColor;
            set => SetProperty(ref _mineFilterColor, value);
        }

        public Color ParticipatingFilterColor
        {
            get => _participatingFilterColor;
            set => SetProperty(ref _participatingFilterColor, value);
        }

        public ICommand RefreshCommand { get; }
        public ICommand EventSelectedCommand { get; }
        public ICommand CreateEventCommand { get; }
        public ICommand FilterEventsCommand { get; }

        private async Task LoadEventsAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Events.Clear();
                
                List<EventModel> events;
                
                if (_currentFilter == "mine" && await _authService.IsAuthenticatedAsync())
                {
                    var userId = await _authService.GetUserIdAsync();
                    var userEvents = await _apiService.GetUserEventsAsync(userId);
                    events = userEvents.Select(e => e.ToModel()).ToList();
                }
                else
                {
                    var allEvents = await _apiService.GetAllEventsAsync();
                    events = allEvents.Select(e => e.ToModel()).ToList();
                }
                
                foreach (var evt in events)
                {
                    Events.Add(evt);
                }
            }
            catch (Exception ex)
            {
                await ShowAlertAsync("Erro", $"Não foi possível carregar os eventos: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }

        private async Task RefreshEventsAsync()
        {
            IsRefreshing = true;
            await LoadEventsAsync();
            IsRefreshing = false;
        }

        private async Task OnEventSelectedAsync(EventModel selectedEvent)
        {
            if (selectedEvent == null)
                return;

            // Navegar para a página de detalhes do evento
            var parameters = new Dictionary<string, object>
            {
                { "EventId", selectedEvent.Id }
            };
            
            await Shell.Current.GoToAsync($"eventdetail", parameters);
        }

        private async Task OnCreateEventAsync()
        {
            // Navegar para a página de criação de evento
            await Shell.Current.GoToAsync("createevent");
        }

        private async Task FilterEventsAsync(string filter)
        {
            if (filter == _currentFilter)
                return;

            _currentFilter = filter;
            
            // Atualizar cores dos botões
            AllFilterColor = filter == "all" ? Colors.DarkBlue : Colors.Gray;
            MineFilterColor = filter == "mine" ? Colors.DarkBlue : Colors.Gray;
            ParticipatingFilterColor = filter == "participating" ? Colors.DarkBlue : Colors.Gray;
            
            // Recarregar eventos com o filtro selecionado
            await RefreshEventsAsync();
        }
    }
} 