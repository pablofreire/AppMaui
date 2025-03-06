using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SportSphere.Shared.DTOs;
using SportSphere.Shared.Models;

namespace SportSphere.App.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;
        private readonly string _baseUrl;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiService(AuthService authService)
        {
            try
            {
                App.LogToFile("Inicializando ApiService...");
                
                _authService = authService ?? throw new ArgumentNullException(nameof(authService));
                _httpClient = new HttpClient();
                
                // Determine the base URL based on platform
                if (DeviceInfo.Platform == DevicePlatform.Android && DeviceInfo.DeviceType == DeviceType.Virtual)
                {
                    // Android emulator uses 10.0.2.2 to access host machine
                    _baseUrl = "http://10.0.2.2:5290/api";
                    App.LogToFile("Configurado URL base para emulador Android: " + _baseUrl);
                }
                else
                {
                    // Windows or iOS
                    _baseUrl = "http://localhost:5290/api";
                    App.LogToFile("Configurado URL base para Windows/iOS: " + _baseUrl);
                }
                
                _jsonOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                
                App.LogToFile("ApiService inicializado com sucesso");
            }
            catch (Exception ex)
            {
                App.LogToFile($"ERRO ao inicializar ApiService: {ex.Message}");
                App.LogToFile($"StackTrace: {ex.StackTrace}");
                throw; // Rethrow to let the caller handle it
            }
        }

        private async Task SetAuthHeaderAsync()
        {
            var token = await _authService.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // Auth methods
        public async Task<(bool Success, string Token, string Message)> LoginAsync(UserLoginDto loginDto)
        {
            try
            {
                // Verificar se o loginDto é válido
                if (loginDto == null || string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
                {
                    return (false, string.Empty, "Email e senha são obrigatórios");
                }

                // Log para depuração
                App.LogToFile($"Tentando login com email: {loginDto.Email}");
                App.LogToFile($"URL da API: {_baseUrl}/auth/login");

                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/auth/login", loginDto);
                
                App.LogToFile($"Resposta recebida. Status: {response.StatusCode}");
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    App.LogToFile($"Conteúdo da resposta: {responseContent}");
                    
                    var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
                    
                    if (result == null)
                    {
                        App.LogToFile("Erro: Resposta da API não pôde ser desserializada");
                        return (false, string.Empty, "Erro ao processar resposta do servidor");
                    }
                    
                    if (string.IsNullOrEmpty(result.Token))
                    {
                        App.LogToFile("Erro: Token recebido é nulo ou vazio");
                        return (false, string.Empty, "Token de autenticação inválido");
                    }
                    
                    // Salvar token no AuthService
                    await _authService.SaveTokenAsync(result.Token);
                    
                    App.LogToFile("Login bem-sucedido");
                    return (true, result.Token, result.Message ?? "Login bem-sucedido");
                }
                
                var errorMessage = await response.Content.ReadAsStringAsync();
                App.LogToFile($"Erro no login: {errorMessage}");
                return (false, string.Empty, errorMessage);
            }
            catch (Exception ex)
            {
                App.LogToFile($"Exceção durante login: {ex.Message}");
                App.LogToFile($"Stack trace: {ex.StackTrace}");
                return (false, string.Empty, $"Erro durante login: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> RegisterAsync(UserRegistrationDto registrationDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/auth/register", registrationDto);
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<MessageResponseDto>();
                    return (true, result.Message);
                }
                
                var errorMessage = await response.Content.ReadAsStringAsync();
                return (false, errorMessage);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        // User methods
        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<UserDto>($"{_baseUrl}/users/{id}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<UserDto>>($"{_baseUrl}/users");
            }
            catch
            {
                return new List<UserDto>();
            }
        }

        // Sport methods
        public async Task<List<SportDto>> GetAllSportsAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<SportDto>>($"{_baseUrl}/sports");
            }
            catch
            {
                return new List<SportDto>();
            }
        }

        public async Task<SportDto> GetSportByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<SportDto>($"{_baseUrl}/sports/{id}");
            }
            catch
            {
                return null;
            }
        }

        // Venue methods
        public async Task<List<VenueDto>> GetAllVenuesAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<VenueDto>>($"{_baseUrl}/venues");
            }
            catch
            {
                return new List<VenueDto>();
            }
        }

        public async Task<List<VenueDto>> GetNearbyVenuesAsync(double latitude, double longitude, double radiusInKm = 10)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<VenueDto>>($"{_baseUrl}/venues/nearby?latitude={latitude}&longitude={longitude}&radiusInKm={radiusInKm}");
            }
            catch
            {
                return new List<VenueDto>();
            }
        }

        // Event methods
        public async Task<List<EventDto>> GetAllEventsAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<EventDto>>($"{_baseUrl}/events");
            }
            catch
            {
                return new List<EventDto>();
            }
        }

        public async Task<List<EventDto>> GetNearbyEventsAsync(double latitude, double longitude, double radiusInKm = 10)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<EventDto>>($"{_baseUrl}/events/nearby?latitude={latitude}&longitude={longitude}&radiusInKm={radiusInKm}");
            }
            catch
            {
                return new List<EventDto>();
            }
        }

        public async Task<EventDto> GetEventByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<EventDto>($"{_baseUrl}/events/{id}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<int> CreateEventAsync(EventCreateDto eventDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/events", eventDto);
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<IdResponseDto>();
                    return result.Id;
                }
                
                return -1;
            }
            catch
            {
                return -1;
            }
        }

        public async Task<bool> JoinEventAsync(int eventId)
        {
            try
            {
                var response = await _httpClient.PostAsync($"{_baseUrl}/events/{eventId}/join", null);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> LeaveEventAsync(int eventId)
        {
            try
            {
                var response = await _httpClient.PostAsync($"{_baseUrl}/events/{eventId}/leave", null);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // Método adicionado para o EventsViewModel
        public async Task<List<EventDto>> GetEventsAsync()
        {
            try
            {
                await SetAuthHeaderAsync();
                return await _httpClient.GetFromJsonAsync<List<EventDto>>($"{_baseUrl}/events");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting events: {ex.Message}");
                return new List<EventDto>();
            }
        }

        // Método adicionado para o MainViewModel
        public async Task<List<SportDto>> GetPopularSportsAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<SportDto>>($"{_baseUrl}/sports");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting popular sports: {ex.Message}");
                return new List<SportDto>();
            }
        }

        // Método adicionado para o MainViewModel
        public async Task<List<EventDto>> GetUpcomingEventsAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<EventDto>>($"{_baseUrl}/events");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting upcoming events: {ex.Message}");
                return new List<EventDto>();
            }
        }

        // Método adicionado para o MainViewModel
        public async Task<List<VenueDto>> GetPopularVenuesAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<VenueDto>>($"{_baseUrl}/venues");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting popular venues: {ex.Message}");
                return new List<VenueDto>();
            }
        }

        // Método adicionado para o VenuesViewModel
        public async Task<List<VenueDto>> GetVenuesAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<VenueDto>>($"{_baseUrl}/venues");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting venues: {ex.Message}");
                return new List<VenueDto>();
            }
        }

        // Método adicionado para o VenuesViewModel
        public async Task<List<VenueDto>> SearchVenuesAsync(string searchText)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<VenueDto>>($"{_baseUrl}/venues/search?searchTerm={Uri.EscapeDataString(searchText)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching venues: {ex.Message}");
                return new List<VenueDto>();
            }
        }

        // User events
        public async Task<List<EventDto>> GetUserEventsAsync(string userId)
        {
            try
            {
                await SetAuthHeaderAsync();
                return await _httpClient.GetFromJsonAsync<List<EventDto>>($"{_baseUrl}/users/{userId}/events/created");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting user events: {ex.Message}");
                return new List<EventDto>();
            }
        }

        public async Task<List<EventDto>> GetUserParticipatingEventsAsync(string userId)
        {
            try
            {
                await SetAuthHeaderAsync();
                return await _httpClient.GetFromJsonAsync<List<EventDto>>($"{_baseUrl}/users/{userId}/events/participating");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting user participating events: {ex.Message}");
                return new List<EventDto>();
            }
        }

        public async Task<bool> UpdateUserAsync(int userId, UserUpdateDto userDto)
        {
            try
            {
                await SetAuthHeaderAsync();
                var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/users/{userId}", userDto);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AddSportToFavoritesAsync(int userId, int sportId)
        {
            try
            {
                await SetAuthHeaderAsync();
                var response = await _httpClient.PostAsync($"{_baseUrl}/users/{userId}/favorites/{sportId}", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding sport to favorites: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RemoveSportFromFavoritesAsync(int userId, int sportId)
        {
            try
            {
                await SetAuthHeaderAsync();
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/users/{userId}/favorites/{sportId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing sport from favorites: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateEventAsync(int eventId, EventUpdateDto eventDto)
        {
            try
            {
                await SetAuthHeaderAsync();
                var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/events/{eventId}", eventDto);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating event: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteEventAsync(int eventId)
        {
            try
            {
                await SetAuthHeaderAsync();
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/events/{eventId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting event: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CancelEventAsync(int eventId)
        {
            try
            {
                await SetAuthHeaderAsync();
                var response = await _httpClient.PostAsync($"{_baseUrl}/events/{eventId}/cancel", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error canceling event: {ex.Message}");
                return false;
            }
        }

        public async Task<List<UserDto>> GetEventParticipantsAsync(int eventId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<UserDto>>($"{_baseUrl}/events/{eventId}/participants");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting event participants: {ex.Message}");
                return new List<UserDto>();
            }
        }
    }

    public class LoginResponseDto
    {
        public string Token { get; set; }
        public string Message { get; set; }
    }

    public class MessageResponseDto
    {
        public string Message { get; set; }
    }

    public class IdResponseDto
    {
        public int Id { get; set; }
    }
} 