using System.Windows.Input;
using SportSphere.App.Services;
using SportSphere.Shared.DTOs;
using SportSphere.App.Views;

namespace SportSphere.App.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private readonly AuthService _authService;
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _username = string.Empty;
        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _confirmPassword = string.Empty;
        private string _errorMessage = string.Empty;
        private bool _hasError;

        public RegisterViewModel(AuthService authService)
        {
            _authService = authService;
            Title = "Registro";
            RegisterCommand = new Command(async () => await RegisterAsync());
            GoToLoginCommand = new Command(async () => await GoToLoginAsync());
        }

        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

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

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
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

        public ICommand RegisterCommand { get; }
        public ICommand GoToLoginCommand { get; }

        private async Task RegisterAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                if (string.IsNullOrWhiteSpace(FirstName) || 
                    string.IsNullOrWhiteSpace(LastName) || 
                    string.IsNullOrWhiteSpace(Username) || 
                    string.IsNullOrWhiteSpace(Email) || 
                    string.IsNullOrWhiteSpace(Password))
                {
                    ErrorMessage = "Todos os campos são obrigatórios";
                    return;
                }

                if (Password != ConfirmPassword)
                {
                    ErrorMessage = "As senhas não coincidem";
                    return;
                }

                var registerDto = new RegisterDto
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Username = Username,
                    Email = Email,
                    Password = Password
                };

                var result = await _authService.RegisterAsync(registerDto);

                if (result.Success)
                {
                    await GoToLoginAsync();
                }
                else
                {
                    ErrorMessage = result.Message;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erro ao registrar: {ex.Message}";
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
                var loginViewModel = new LoginViewModel(_authService, new ApiService(_authService));
                Application.Current.MainPage = new LoginPage(loginViewModel);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erro ao navegar para a página de login: {ex.Message}";
            }
        }
    }
} 