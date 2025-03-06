using System;
using System.Threading.Tasks;
using SportSphere.Shared.DTOs;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using Microsoft.Maui.Storage;

namespace SportSphere.App.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly ISecureStorage _secureStorage;
        private const string TokenKey = "auth_token";
        private const string UserIdKey = "user_id";

        public AuthService(ISecureStorage secureStorage)
        {
            try
            {
                App.LogToFile("Inicializando AuthService...");
                _secureStorage = secureStorage ?? throw new ArgumentNullException(nameof(secureStorage));
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
                
                App.LogToFile("AuthService inicializado com sucesso");
            }
            catch (Exception ex)
            {
                App.LogToFile($"ERRO ao inicializar AuthService: {ex.Message}");
                App.LogToFile($"StackTrace: {ex.StackTrace}");
                throw; // Rethrow to let the caller handle it
            }
        }

        public async Task<(bool Success, string Token, string Message)> LoginAsync(string username, string password)
        {
            try
            {
                App.LogToFile($"Tentando login para usuário: {username}");
                
                var loginData = new
                {
                    Username = username,
                    Password = password
                };

                var json = JsonSerializer.Serialize(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                App.LogToFile($"Enviando requisição para {_baseUrl}/auth/login");
                var response = await _httpClient.PostAsync($"{_baseUrl}/auth/login", content);
                
                App.LogToFile($"Resposta recebida. Status: {response.StatusCode}");
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    App.LogToFile($"Conteúdo da resposta: {responseContent}");
                    
                    var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
                    {
                        App.LogToFile("Login bem-sucedido. Salvando token e ID do usuário.");
                        await _secureStorage.SetAsync(TokenKey, loginResponse.Token);
                        await _secureStorage.SetAsync(UserIdKey, loginResponse.UserId.ToString());
                        return (true, loginResponse.Token, "Login bem-sucedido");
                    }
                    else
                    {
                        App.LogToFile("Login falhou: Token inválido ou nulo na resposta");
                        return (false, string.Empty, "Token inválido ou nulo");
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    App.LogToFile($"Login falhou. Status: {response.StatusCode}, Erro: {errorContent}");
                    return (false, string.Empty, $"Falha no login: {response.StatusCode} - {errorContent}");
                }
            }
            catch (Exception ex)
            {
                App.LogToFile($"Exceção durante login: {ex.Message}");
                App.LogToFile($"Tipo de exceção: {ex.GetType().FullName}");
                App.LogToFile($"Stack trace: {ex.StackTrace}");
                return (false, string.Empty, $"Erro durante login: {ex.Message}");
            }
        }

        public async Task<string> GetTokenAsync()
        {
            try
            {
                App.LogToFile("Obtendo token de autenticação do armazenamento seguro");
                var token = await _secureStorage.GetAsync(TokenKey);
                App.LogToFile($"Token obtido: {(string.IsNullOrEmpty(token) ? "Nulo ou vazio" : "Token válido encontrado")}");
                return token;
            }
            catch (Exception ex)
            {
                App.LogToFile($"Erro ao obter token: {ex.Message}");
                App.LogToFile($"Tipo de exceção: {ex.GetType().FullName}");
                App.LogToFile($"Stack trace: {ex.StackTrace}");
                
                // If there's an issue with SecureStorage, try to recover by clearing it
                try
                {
                    _secureStorage.Remove(TokenKey);
                    App.LogToFile("Removed problematic token key");
                }
                catch (Exception clearEx)
                {
                    App.LogToFile($"Error clearing token key: {clearEx.Message}");
                }
                
                return null;
            }
        }

        public async Task SaveTokenAsync(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    App.LogToFile("Tentativa de salvar token nulo ou vazio");
                    return;
                }
                
                App.LogToFile("Salvando token de autenticação no armazenamento seguro");
                await _secureStorage.SetAsync(TokenKey, token);
                App.LogToFile("Token salvo com sucesso");
            }
            catch (Exception ex)
            {
                App.LogToFile($"Erro ao salvar token: {ex.Message}");
                App.LogToFile($"Tipo de exceção: {ex.GetType().FullName}");
                App.LogToFile($"Stack trace: {ex.StackTrace}");
                
                // Try to clear and retry
                try
                {
                    _secureStorage.RemoveAll();
                    App.LogToFile("Cleared all secure storage");
                    await _secureStorage.SetAsync(TokenKey, token);
                    App.LogToFile("Token saved successfully after clearing storage");
                }
                catch (Exception retryEx)
                {
                    App.LogToFile($"Error saving token after retry: {retryEx.Message}");
                }
            }
        }

        public async Task<string> GetUserIdAsync()
        {
            try
            {
                App.LogToFile("Obtendo ID do usuário do armazenamento seguro");
                var userId = await _secureStorage.GetAsync(UserIdKey);
                App.LogToFile($"ID do usuário obtido: {(string.IsNullOrEmpty(userId) ? "Nulo ou vazio" : userId)}");
                return userId;
            }
            catch (Exception ex)
            {
                App.LogToFile($"Erro ao obter ID do usuário: {ex.Message}");
                App.LogToFile($"Tipo de exceção: {ex.GetType().FullName}");
                App.LogToFile($"Stack trace: {ex.StackTrace}");
                
                // If there's an issue with SecureStorage, try to recover by clearing it
                try
                {
                    _secureStorage.Remove(UserIdKey);
                    App.LogToFile("Removed problematic user ID key");
                }
                catch (Exception clearEx)
                {
                    App.LogToFile($"Error clearing user ID key: {clearEx.Message}");
                }
                
                return null;
            }
        }

        public async Task SaveUserIdAsync(string userId)
        {
            try
            {
                await _secureStorage.SetAsync(UserIdKey, userId);
                App.LogToFile("User ID saved successfully");
            }
            catch (Exception ex)
            {
                App.LogToFile($"Error saving user ID: {ex.Message}");
                App.LogToFile($"Tipo de exceção: {ex.GetType().FullName}");
                App.LogToFile($"Stack trace: {ex.StackTrace}");
                
                // Try to clear and retry
                try
                {
                    _secureStorage.RemoveAll();
                    App.LogToFile("Cleared all secure storage");
                    await _secureStorage.SetAsync(UserIdKey, userId);
                    App.LogToFile("User ID saved successfully after clearing storage");
                }
                catch (Exception retryEx)
                {
                    App.LogToFile($"Error saving user ID after retry: {retryEx.Message}");
                }
            }
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            try
            {
                App.LogToFile("Verificando se o usuário está autenticado");
                var token = await GetTokenAsync();
                var isAuthenticated = !string.IsNullOrEmpty(token);
                App.LogToFile($"Usuário está autenticado: {isAuthenticated}");
                return isAuthenticated;
            }
            catch (Exception ex)
            {
                App.LogToFile($"Erro ao verificar autenticação: {ex.Message}");
                App.LogToFile($"Tipo de exceção: {ex.GetType().FullName}");
                App.LogToFile($"Stack trace: {ex.StackTrace}");
                return false;
            }
        }

        public bool IsAuthenticated 
        { 
            get
            {
                try
                {
                    App.LogToFile("Acessando propriedade IsAuthenticated");
                    var token = GetTokenAsync().Result;
                    var isAuthenticated = !string.IsNullOrEmpty(token);
                    App.LogToFile($"Propriedade IsAuthenticated retornando: {isAuthenticated}");
                    return isAuthenticated;
                }
                catch (Exception ex)
                {
                    App.LogToFile($"Erro ao acessar propriedade IsAuthenticated: {ex.Message}");
                    App.LogToFile($"Tipo de exceção: {ex.GetType().FullName}");
                    App.LogToFile($"Stack trace: {ex.StackTrace}");
                    return false;
                }
            }
        }

        public async Task LogoutAsync()
        {
            try
            {
                App.LogToFile("Realizando logout do usuário");
                _secureStorage.Remove(TokenKey);
                _secureStorage.Remove(UserIdKey);
                App.LogToFile("Logout concluído com sucesso");
            }
            catch (Exception ex)
            {
                App.LogToFile($"Erro ao realizar logout: {ex.Message}");
                App.LogToFile($"Tipo de exceção: {ex.GetType().FullName}");
                App.LogToFile($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<SportSphere.Shared.Models.UserModel> GetCurrentUserAsync()
        {
            try
            {
                App.LogToFile("Obtendo informações do usuário atual");
                var userId = await GetUserIdAsync();
                var token = await GetTokenAsync();

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                {
                    App.LogToFile("Não foi possível obter usuário atual: ID do usuário ou token não encontrados");
                    return null;
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                
                App.LogToFile($"Enviando requisição para {_baseUrl}/users/{userId}");
                var response = await _httpClient.GetAsync($"{_baseUrl}/users/{userId}");
                
                App.LogToFile($"Resposta recebida. Status: {response.StatusCode}");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    App.LogToFile($"Conteúdo da resposta: {content}");
                    
                    var user = JsonSerializer.Deserialize<SportSphere.Shared.Models.UserModel>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    
                    App.LogToFile($"Usuário obtido com sucesso: {user?.Username}");
                    return user;
                }
                
                return null;
            }
            catch (Exception ex)
            {
                App.LogToFile($"Exceção ao obter usuário atual: {ex.Message}");
                App.LogToFile($"Tipo de exceção: {ex.GetType().FullName}");
                App.LogToFile($"Stack trace: {ex.StackTrace}");
                return null;
            }
        }

        public async Task<(bool Success, string Message)> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                App.LogToFile($"Tentando registrar novo usuário: {registerDto.Username}");
                
                var json = JsonSerializer.Serialize(registerDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                App.LogToFile($"Enviando requisição para {_baseUrl}/auth/register");
                var response = await _httpClient.PostAsync($"{_baseUrl}/auth/register", content);
                
                App.LogToFile($"Resposta recebida. Status: {response.StatusCode}");
                
                var responseContent = await response.Content.ReadAsStringAsync();
                App.LogToFile($"Conteúdo da resposta: {responseContent}");
                
                if (response.IsSuccessStatusCode)
                {
                    App.LogToFile("Registro bem-sucedido");
                    return (true, "Registro bem-sucedido");
                }
                else
                {
                    App.LogToFile($"Falha no registro. Status: {response.StatusCode}, Erro: {responseContent}");
                    return (false, $"Falha no registro: {responseContent}");
                }
            }
            catch (Exception ex)
            {
                App.LogToFile($"Exceção durante registro: {ex.Message}");
                App.LogToFile($"Tipo de exceção: {ex.GetType().FullName}");
                App.LogToFile($"Stack trace: {ex.StackTrace}");
                return (false, $"Erro durante registro: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> ChangePasswordAsync(string currentPassword, string newPassword)
        {
            try
            {
                App.LogToFile("Tentando alterar senha do usuário");
                
                var token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    App.LogToFile("Não foi possível alterar a senha: usuário não autenticado");
                    return (false, "Usuário não autenticado");
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                
                var changePasswordData = new
                {
                    CurrentPassword = currentPassword,
                    NewPassword = newPassword
                };

                var json = JsonSerializer.Serialize(changePasswordData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                App.LogToFile($"Enviando requisição para {_baseUrl}/auth/change-password");
                var response = await _httpClient.PostAsync($"{_baseUrl}/auth/change-password", content);
                
                App.LogToFile($"Resposta recebida. Status: {response.StatusCode}");
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    App.LogToFile($"Conteúdo da resposta: {responseContent}");
                    
                    var messageResponse = JsonSerializer.Deserialize<MessageResponseDto>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    App.LogToFile("Senha alterada com sucesso");
                    return (true, messageResponse?.Message ?? "Senha alterada com sucesso");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    App.LogToFile($"Falha ao alterar senha. Status: {response.StatusCode}, Erro: {errorContent}");
                    return (false, $"Falha ao alterar senha: {response.StatusCode} - {errorContent}");
                }
            }
            catch (Exception ex)
            {
                App.LogToFile($"Exceção durante alteração de senha: {ex.Message}");
                App.LogToFile($"Tipo de exceção: {ex.GetType().FullName}");
                App.LogToFile($"Stack trace: {ex.StackTrace}");
                return (false, $"Erro durante alteração de senha: {ex.Message}");
            }
        }

        private class LoginResponse
        {
            public string Token { get; set; }
            public int UserId { get; set; }
        }
    }

    public class RegisterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Bio { get; set; }
    }
} 
 