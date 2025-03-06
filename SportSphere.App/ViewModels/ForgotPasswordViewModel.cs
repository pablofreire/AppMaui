using System.Windows.Input;
using SportSphere.App.Services;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.Threading.Tasks;
using System;
using SportSphere.App.Views;

namespace SportSphere.App.ViewModels
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private string _email = string.Empty;
        private string _message = string.Empty;
        private bool _hasMessage;
        private Color _messageColor = Colors.Red;
        private string _errorMessage = string.Empty;
        private bool _hasError;

        public ForgotPasswordViewModel(ApiService apiService)
        {
            _apiService = apiService;
            
            Title = "Recuperar Senha";
            
            SendResetEmailCommand = new Command(async () => await SendResetEmailAsync());
            GoToLoginCommand = new Command(async () => await GoToLoginAsync());
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public bool HasMessage
        {
            get => _hasMessage;
            set => SetProperty(ref _hasMessage, value);
        }

        public Color MessageColor
        {
            get => _messageColor;
            set => SetProperty(ref _messageColor, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                SetProperty(ref _errorMessage, value);
                HasError = !string.IsNullOrEmpty(value);
            }
        }

        public bool HasError
        {
            get => _hasError;
            set => SetProperty(ref _hasError, value);
        }

        public ICommand SendResetEmailCommand { get; }
        public ICommand GoToLoginCommand { get; }

        private async Task SendResetEmailAsync()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                Message = "Por favor, informe seu email";
                HasMessage = true;
                MessageColor = Colors.Red;
                return;
            }

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                // Aqui seria implementada a chamada para a API para enviar o email de recuperação
                // Por enquanto, vamos simular um sucesso
                await Task.Delay(2000); // Simular chamada de rede
                
                Message = "Email de recuperação enviado com sucesso. Verifique sua caixa de entrada.";
                HasMessage = true;
                MessageColor = Colors.Green;
                
                // Limpar o campo de email
                Email = string.Empty;
            }
            catch (Exception ex)
            {
                Message = $"Erro ao enviar email: {ex.Message}";
                HasMessage = true;
                MessageColor = Colors.Red;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task GoToLoginAsync()
        {
            try
            {
                // Criar e exibir a página de login diretamente
                var authService = new AuthService(SecureStorage.Default);
                var loginViewModel = new LoginViewModel(authService, _apiService);
                Application.Current.MainPage = new LoginPage(loginViewModel);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erro ao navegar para a página de login: {ex.Message}";
            }
        }
    }
} 