using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using SportSphere.App.Services;
using SportSphere.Shared.DTOs;
using SportSphere.App.Views;

namespace SportSphere.App.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly AuthService _authService;
        private readonly ApiService _apiService;
        private string _email;
        private string _password;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand ForgotPasswordCommand { get; }

        public LoginViewModel(AuthService authService, ApiService apiService)
        {
            _authService = authService;
            _apiService = apiService;
            LoginCommand = new Command(async () => await LoginAsync());
            RegisterCommand = new Command(async () => await GoToRegisterAsync());
            ForgotPasswordCommand = new Command(async () => await GoToForgotPasswordAsync());
        }

        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await DisplayAlert("Erro de Validação", "Email e senha são obrigatórios", "OK");
                return;
            }

            await DisplayLoading("Fazendo login...");

            try
            {
                App.LogToFile($"Iniciando login para usuário: {Email}");
                
                var loginDto = new UserLoginDto { Email = Email, Password = Password };
                var result = await _apiService.LoginAsync(loginDto);

                App.LogToFile($"Resultado do login: Success={result.Success}, Message={result.Message}");

                if (result.Success)
                {
                    App.LogToFile("Login bem-sucedido, navegando para a página principal");
                    
                    // Verificar se o AuthService está inicializado
                    if (_authService == null)
                    {
                        App.LogToFile("ERRO: AuthService é nulo");
                        await DisplayAlert("Erro", "Erro interno da aplicação: AuthService não inicializado", "OK");
                        return;
                    }
                    
                    try
                    {
                        //var registerViewModel = new ProfileViewModel(_authService, _apiService);
                        Application.Current.MainPage = new AppShell(_authService);

                        // Navigate to main page
                        //Application.Current.MainPage = new AppShell(_authService);
                        
                        // Adicionar log para confirmar a navegação
                        App.LogToFile("AppShell definido como MainPage com sucesso");
                        
                        // Forçar a atualização da UI
                        await Task.Delay(100);
                        App.LogToFile("UI atualizada após navegação");
                    }
                    catch (Exception navEx)
                    {
                        App.LogToFile($"Erro ao navegar para AppShell: {navEx.Message}");
                        App.LogToFile($"Stack trace: {navEx.StackTrace}");
                        await DisplayAlert("Erro de Navegação", $"Não foi possível navegar para a página principal: {navEx.Message}", "OK");
                    }
                }
                else
                {
                    App.LogToFile($"Login falhou: {result.Message}");
                    await DisplayAlert("Falha no Login", result.Message, "OK");
                }
            }
            catch (Exception ex)
            {
                App.LogToFile($"Exceção durante login: {ex.Message}");
                App.LogToFile($"Stack trace: {ex.StackTrace}");
                await DisplayAlert("Erro", $"Ocorreu um erro: {ex.Message}", "OK");
            }
            finally
            {
                HideLoading();
            }
        }

        private async Task GoToRegisterAsync()
        {
            try
            {
                // Criar e exibir a página de registro diretamente
                var registerViewModel = new RegisterViewModel(_authService);
                Application.Current.MainPage = new RegisterPage(registerViewModel);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Não foi possível navegar para a página de registro: {ex.Message}", "OK");
            }
        }

        private async Task GoToForgotPasswordAsync()
        {
            try
            {
                // Criar e exibir a página de esqueceu a senha diretamente
                var forgotPasswordViewModel = new ForgotPasswordViewModel(_apiService);
                Application.Current.MainPage = new ForgotPasswordPage(forgotPasswordViewModel);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Não foi possível navegar para a página de recuperação de senha: {ex.Message}", "OK");
            }
        }
    }
} 