using Microsoft.Extensions.Logging;
using SportSphere.App.Services;
using SportSphere.App.ViewModels;
using SportSphere.App.Views;

namespace SportSphere.App;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		// Registrar serviços
		builder.Services.AddSingleton<ISecureStorage>(SecureStorage.Default);
		builder.Services.AddSingleton<AuthService>();
		builder.Services.AddSingleton<ApiService>();
		builder.Services.AddSingleton<LocationService>();

		// Registrar ViewModels
		builder.Services.AddTransient<LoginViewModel>();
		builder.Services.AddTransient<RegisterViewModel>();
		builder.Services.AddTransient<MainViewModel>();
		builder.Services.AddTransient<ProfileViewModel>();
		builder.Services.AddTransient<EventsViewModel>();
		builder.Services.AddTransient<VenuesViewModel>();
		builder.Services.AddTransient<ForgotPasswordViewModel>();
		builder.Services.AddTransient<MapViewModel>();

		// Registrar Views
		builder.Services.AddTransient<LoginPage>();
		builder.Services.AddTransient<RegisterPage>();
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<ProfilePage>();
		builder.Services.AddTransient<EventsPage>();
		builder.Services.AddTransient<VenuesPage>();
		builder.Services.AddTransient<ForgotPasswordPage>();
		builder.Services.AddTransient<MapPage>();

		return builder.Build();
	}
}
