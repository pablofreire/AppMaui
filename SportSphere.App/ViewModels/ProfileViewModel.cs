using System.Collections.ObjectModel;
using System.Windows.Input;
using SportSphere.App.Services;
using SportSphere.Shared.Models;

namespace SportSphere.App.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private readonly AuthService _authService;
        private readonly ApiService _apiService;
        private SportSphere.Shared.Models.UserModel _currentUser;
        private string _profileImageUrl;
        private string _fullName;
        private string _username;
        private string _email;
        private DateTime _dateOfBirth;
        private DateTime _registrationDate;
        private DateTime _lastLoginDate;
        private string _bio;
        private ObservableCollection<SportModel> _favoriteSports;

        public ProfileViewModel(AuthService authService, ApiService apiService)
        {
            _authService = authService;
            _apiService = apiService;
            _currentUser = new SportSphere.Shared.Models.UserModel();
            _favoriteSports = new ObservableCollection<SportModel>();
            
            Title = "Meu Perfil";
            
            LogoutCommand = new Command(async () => await LogoutAsync());
            EditProfileCommand = new Command(async () => await EditProfileAsync());
            LoadProfileCommand = new Command(async () => await LoadUserDataAsync());
            
            LoadUserDataAsync();
        }

        public string ProfileImageUrl
        {
            get => _profileImageUrl;
            set => SetProperty(ref _profileImageUrl, value);
        }

        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
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

        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set => SetProperty(ref _dateOfBirth, value);
        }

        public DateTime RegistrationDate
        {
            get => _registrationDate;
            set => SetProperty(ref _registrationDate, value);
        }

        public DateTime LastLoginDate
        {
            get => _lastLoginDate;
            set => SetProperty(ref _lastLoginDate, value);
        }

        public string Bio
        {
            get => _bio;
            set => SetProperty(ref _bio, value);
        }

        public ObservableCollection<SportModel> FavoriteSports
        {
            get => _favoriteSports;
            set => SetProperty(ref _favoriteSports, value);
        }

        public ICommand LogoutCommand { get; }
        public ICommand EditProfileCommand { get; }
        public ICommand LoadProfileCommand { get; }

        private async Task LoadUserDataAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var user = await _authService.GetCurrentUserAsync();
                if (user != null)
                {
                    _currentUser = user;
                    ProfileImageUrl = string.IsNullOrEmpty(user.ProfilePictureUrl) 
                        ? "dotnet_bot.png" 
                        : user.ProfilePictureUrl;
                    FullName = $"{user.FirstName} {user.LastName}";
                    Username = user.Username;
                    Email = user.Email;
                    DateOfBirth = user.DateOfBirth;
                    RegistrationDate = user.RegistrationDate;
                    LastLoginDate = user.LastLoginDate;
                    Bio = user.Bio;

                    FavoriteSports.Clear();
                    if (user.FavoriteSports != null)
                    {
                        foreach (var sport in user.FavoriteSports)
                        {
                            FavoriteSports.Add(sport);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await ShowAlertAsync("Erro", $"Não foi possível carregar os dados do usuário: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void LoadUserData()
        {
            await LoadUserDataAsync();
        }

        private async Task LogoutAsync()
        {
            bool answer = await ShowConfirmationAsync("Sair", "Tem certeza que deseja sair?", "Sim", "Não");
            if (answer)
            {
                await _authService.LogoutAsync();
                await Shell.Current.GoToAsync("//login");
            }
        }

        private async Task EditProfileAsync()
        {
            // Navegar para a página de edição de perfil (a ser implementada)
            await Shell.Current.DisplayAlert("Editar Perfil", "Funcionalidade a ser implementada", "OK");
        }
    }
} 