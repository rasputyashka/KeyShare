using KeyShare.Activation;
using KeyShare.Contracts.Services;
using KeyShare.Models;
using KeyShare.Services;
using KeyShare.ViewModels;
using KeyShare.Views;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Microsoft.Data.Sqlite;
using Windows.Storage;
using SQLitePCL;
using KeyShare.Core.Contracts.Services.Domain;
using KeyShare.Core.Services.Domain;
using KeyShare.Core.Contracts.Services.Application;
using Windows.Security.Cryptography.Core;
using KeyShare.Core.Services.Application;
using KeyShare.Core.Contracts.Services.Db;
using KeyShare.Core.Services.Db;
using Microsoft.UI.Dispatching;

namespace KeyShare;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public static UIElement? AppTitlebar { get; set; }

    public App()
    {
        InitializeComponent();
        Batteries.Init();
        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {

            var connection = InitializeAndOpenDatabase();
            services.AddSingleton(connection);

            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers

            // Services
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            services.AddSingleton<ICipherTextRepository, CipherTextRepository>();
            services.AddSingleton<IKeyRepository, KeyRepository>();
            services.AddSingleton<ISecretPieceRepository, SecretPieceRepository>();

            services.AddSingleton<ICryptoService, AESCryptoService>();
            services.AddSingleton<IEncryptionKeyProvider, AESEncryptionKeyProvider>();
            services.AddSingleton<ISecretService, SecretService>();



            // commands
            services.AddSingleton<IEncryptAndSaveCommand, EncryptAndSaveCommand>();
            services.AddSingleton<IRetrieveAllCipherTextsCommand, RetrieveAllCipherTexts>();
            services.AddSingleton<IRetrieveAllPiecesCommand, RetrieveAllPiecesCommand>();
            services.AddSingleton<IGetKeyAndDecryptCommand, GetKeyAndDecryptCommand>();
            services.AddSingleton<IDeleteCipherTextCommand, DeleteCipherTextCommand>();


            // Core Services
            services.AddSingleton<IFileService, FileService>();

            // Persistence



            // Views and ViewModels
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();

            services.AddTransient<SharesViewModel>();
            services.AddTransient<SharesPage>();

            services.AddTransient<DecryptViewModel>();
            services.AddTransient<DecryptPage>();

            services.AddTransient<EncryptViewModel>();
            services.AddTransient<EncryptPage>();

            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        }).
        Build();
           
        
        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    private SqliteConnection InitializeAndOpenDatabase()
    {
        StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        var dbName = "KeyShare.db";
        var dbPath = Path.Combine(localFolder.Path, dbName);
        var dbExists = File.Exists(dbPath);

        var connection = new SqliteConnection($"Data Source={dbPath}");
        connection.Open();

        using (var pragmaCommand = connection.CreateCommand())
        {
            pragmaCommand.CommandText = "PRAGMA foreign_keys = ON;";
            pragmaCommand.ExecuteNonQuery();
        }

        if (!dbExists)
        {
            using (var createCmd = connection.CreateCommand())
            {
                createCmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS key (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    size INTEGER NOT NULL,
                    threshold INTEGER NOT NULL,
                    created_at TEXT NOT NULL,
                    updated_at TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS cipher_text (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    title TEXT NOT NULL,
                    algorithm TEXT NOT NULL,
                    description TEXT,
                    key_id INTEGER NOT NULL,
                    content BLOB NOT NULL,
                    iv BLOB NOT NULL,
                    created_at TEXT NOT NULL,
                    updated_at TEXT NOT NULL,
                    FOREIGN KEY (key_id) REFERENCES Key(id) ON DELETE CASCADE
                );

                CREATE TABLE IF NOT EXISTS secret_piece (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    x TEXT NOT NULL,
                    y TEXT NOT NULL,
                    key_id INTEGER NOT NULL,
                    created_at TEXT NOT NULL,
                    updated_at TEXT NOT NULL,
                    FOREIGN KEY (key_id) REFERENCES Key(id) ON DELETE CASCADE
                );
            ";
                createCmd.ExecuteNonQuery();
            }
        }

        return connection;
    }


    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);
        await App.GetService<IActivationService>().ActivateAsync(args);
    }
}
