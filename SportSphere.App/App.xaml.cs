using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using SportSphere.App.Services;
using SportSphere.App.Views;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using SportSphere.App.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace SportSphere.App;

public partial class App : Application
{
	private readonly AuthService _authService;
	private readonly ApiService _apiService;
	private static string _logFilePath;

    public App()
    {
        InitializeComponent();

        // Setup logging
        try
        {
            Console.WriteLine("Setting up logging...");
            Debug.WriteLine("Setting up logging...");

            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Console.WriteLine($"App data path: {appDataPath}");
            Debug.WriteLine($"App data path: {appDataPath}");

            string logDirectory = Path.Combine(appDataPath, "SportSphere", "Logs");
            Console.WriteLine($"Log directory path: {logDirectory}");
            Debug.WriteLine($"Log directory path: {logDirectory}");

            // Ensure directory exists with detailed error handling
            try
            {
                if (!Directory.Exists(logDirectory))
                {
                    Console.WriteLine($"Creating directory: {logDirectory}");
                    Debug.WriteLine($"Creating directory: {logDirectory}");
                    Directory.CreateDirectory(logDirectory);
                    Console.WriteLine("Directory created successfully");
                    Debug.WriteLine("Directory created successfully");
                }
                else
                {
                    Console.WriteLine("Directory already exists");
                    Debug.WriteLine("Directory already exists");
                }

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                _logFilePath = Path.Combine(logDirectory, $"SportSphere_Log_{timestamp}.log");
                Console.WriteLine($"Log file path: {_logFilePath}");
                Debug.WriteLine($"Log file path: {_logFilePath}");

                // Test write to log file
                File.WriteAllText(_logFilePath, $"Log initialized at {DateTime.Now}\r\n");
                Console.WriteLine("Initial log entry written successfully");
                Debug.WriteLine("Initial log entry written successfully");

                LogToFile($"Application started on {DateTime.Now}");
                LogToFile($"Platform: {DeviceInfo.Platform}, Idiom: {DeviceInfo.Idiom}");
                LogToFile($"Device Model: {DeviceInfo.Model}, Manufacturer: {DeviceInfo.Manufacturer}");
                LogToFile($"OS Version: {DeviceInfo.VersionString}");
            }
            catch (Exception dirEx)
            {
                Console.WriteLine($"Error creating directory: {dirEx.Message}");
                Debug.WriteLine($"Error creating directory: {dirEx.Message}");

                // Try fallback to temp directory
                try
                {
                    string tempPath = Path.GetTempPath();
                    Console.WriteLine($"Using temp path as fallback: {tempPath}");
                    Debug.WriteLine($"Using temp path as fallback: {tempPath}");

                    string fallbackLogDir = Path.Combine(tempPath, "SportSphere", "Logs");
                    Console.WriteLine($"Fallback log directory: {fallbackLogDir}");
                    Debug.WriteLine($"Fallback log directory: {fallbackLogDir}");

                    Directory.CreateDirectory(fallbackLogDir);
                    Console.WriteLine("Fallback directory created successfully");
                    Debug.WriteLine("Fallback directory created successfully");

                    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    _logFilePath = Path.Combine(fallbackLogDir, $"SportSphere_Log_{timestamp}.log");
                    Console.WriteLine($"Fallback log file path: {_logFilePath}");
                    Debug.WriteLine($"Fallback log file path: {_logFilePath}");

                    // Test write to fallback log file
                    File.WriteAllText(_logFilePath, $"Fallback log initialized at {DateTime.Now}\r\n");
                    Console.WriteLine("Initial fallback log entry written successfully");
                    Debug.WriteLine("Initial fallback log entry written successfully");
                }
                catch (Exception fallbackEx)
                {
                    Console.WriteLine($"Error creating fallback directory: {fallbackEx.Message}");
                    Debug.WriteLine($"Error creating fallback directory: {fallbackEx.Message}");
                    // If fallback fails, use a static string to indicate logging is disabled
                    _logFilePath = string.Empty;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Critical error during logging setup: {ex.Message}");
            Debug.WriteLine($"Critical error during logging setup: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            Debug.WriteLine($"Stack trace: {ex.StackTrace}");

            // Ensure we have a fallback page even if logging fails
            MainPage = new ContentPage
            {
                Content = new VerticalStackLayout
                {
                    Children =
                                {
                                    new Label { Text = "An error occurred during startup", FontSize = 20, HorizontalOptions = LayoutOptions.Center },
                                    new Label { Text = ex.Message, FontSize = 16, HorizontalOptions = LayoutOptions.Center }
                                },
                    VerticalOptions = LayoutOptions.Center
                }
            };
            return;
        }

        // Inicializar serviços manualmente
        try
        {
            LogToFile("Inicializando serviços...");
            
            // Criar instância do ISecureStorage
            ISecureStorage secureStorage;
            try
            {
                secureStorage = SecureStorage.Default;
                LogToFile("SecureStorage inicializado com sucesso");
            }
            catch (Exception secEx)
            {
                LogToFile($"Erro ao inicializar SecureStorage: {secEx.Message}");
                if (secEx.InnerException != null)
                {
                    LogToFile($"Inner exception: {secEx.InnerException.Message}");
                }
                LogToFile($"Stack trace: {secEx.StackTrace}");
                
                // Fallback para uma implementação alternativa ou continuar sem SecureStorage
                secureStorage = SecureStorage.Default; // Tentar novamente
                LogToFile("Tentando inicializar SecureStorage novamente");
            }
            
            // Criar instâncias dos serviços
            _authService = new AuthService(secureStorage);
            _apiService = new ApiService(_authService);
            
            LogToFile("Serviços inicializados com sucesso");
            
            // Definir a página inicial de forma síncrona
            // Inicialmente, definimos uma página de login como padrão
            var loginViewModel = new LoginViewModel(_authService, _apiService);
            MainPage = new LoginPage(loginViewModel);
            
            // Iniciar verificação de autenticação em segundo plano após a página inicial ser definida
            MainThread.BeginInvokeOnMainThread(async () => {
                await CheckAuthenticationAsync();
            });
        }
        catch (Exception ex)
        {
            LogToFile($"Erro ao inicializar serviços: {ex.Message}");
            LogToFile($"Stack trace: {ex.StackTrace}");
            
            // Exibir página de erro
            MainPage = new ContentPage
            {
                Content = new VerticalStackLayout
                {
                    Children =
                    {
                        new Label { Text = "Erro ao inicializar a aplicação", FontSize = 20, HorizontalOptions = LayoutOptions.Center },
                        new Label { Text = ex.Message, FontSize = 16, HorizontalOptions = LayoutOptions.Center }
                    },
                    VerticalOptions = LayoutOptions.Center
                }
            };
        }
    }

	public static void LogToFile(string message)
	{
		try
		{
			if (string.IsNullOrEmpty(_logFilePath))
			{
				Console.WriteLine($"Cannot log to file, path is empty: {message}");
				Debug.WriteLine($"Cannot log to file, path is empty: {message}");
				return;
			}

			string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}";
			Console.WriteLine($"Logging: {logEntry}");
			Debug.WriteLine($"Logging: {logEntry}");
			
			File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
			Console.WriteLine("Log entry written successfully");
			Debug.WriteLine("Log entry written successfully");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error writing to log file: {ex.Message}");
			Debug.WriteLine($"Error writing to log file: {ex.Message}");
			Debug.WriteLine($"Stack trace: {ex.StackTrace}");
		}
	}

	private async Task CheckAuthenticationAsync()
	{
		try
		{
			LogToFile("Checking authentication status...");
			bool isAuthenticated = await _authService.IsAuthenticatedAsync();
			LogToFile($"User is authenticated: {isAuthenticated}");

			// Atualizar a UI na thread principal
			if (isAuthenticated)
			{
				LogToFile("Navigating to MainPage");
				MainPage = new AppShell(_authService);
			}
		}
		catch (Exception ex)
		{
			LogToFile($"Error during authentication check: {ex.Message}");
			if (ex.InnerException != null)
			{
				LogToFile($"Inner exception: {ex.InnerException.Message}");
			}
			LogToFile($"Stack trace: {ex.StackTrace}");
			
			// Já estamos na página de login, então não precisamos fazer nada aqui
		}
	}
}