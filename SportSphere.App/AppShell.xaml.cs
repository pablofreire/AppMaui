using SportSphere.App.Services;
using SportSphere.App.Views;
using System.Threading.Tasks;

namespace SportSphere.App
{
	public partial class AppShell : Shell
	{
		private readonly AuthService _authService;

		public Command LogoutCommand { get; }

		public AppShell(AuthService authService)
		{
			InitializeComponent();

			_authService = authService ?? throw new ArgumentNullException(nameof(authService));
			LogoutCommand = new Command(async () => await LogoutAsync());

			// Register routes for navigation
			Routing.RegisterRoute("register", typeof(RegisterPage));
			Routing.RegisterRoute("forgotpassword", typeof(ForgotPasswordPage));
			Routing.RegisterRoute("profile", typeof(ProfilePage));
			Routing.RegisterRoute("events", typeof(EventsPage));
			Routing.RegisterRoute("venues", typeof(VenuesPage));

            
			BindingContext = this;
		}

		private async Task LogoutAsync()
		{
			if (_authService == null)
				return;
				
			bool confirm = await DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
			if (confirm)
			{
				await _authService.LogoutAsync();
				await Shell.Current.GoToAsync("//login");
			}
		}
	}
}
