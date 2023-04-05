using System;
using System.IO;
using System.Reflection;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.WindowsAPICodePack.Dialogs;
using SynUI.Properties;
using SynUI.Services;
using SynUI.Utilities;
using SynUI.ViewModels;
using SynUI.ViewModels.TabViewModels;
using SynUI.Views;

namespace SynUI;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        // forcefully shutdown (fuck you sxlib)
        Exit += (_, _) => Environment.Exit(0);

        // Auto save settings
        Settings.Default.PropertyChanged += (_, _) =>
            Settings.Default.Save();

        // logging
        var logFolder = Directory.CreateDirectory(Path.Combine(Location, "logs"));
        var latestLog = Path.Combine(logFolder.FullName, "latest.log");
        var instanceLog = Path.Combine(logFolder.FullName, DateTime.Now.ToString("HH-mm-ss yy-MM-dd") + ".log");
        File.WriteAllText(latestLog, string.Empty);

        AppDomain.CurrentDomain.FirstChanceException += (_, e) =>
        {
            File.AppendAllText(instanceLog, e.Exception.ToString());
            File.AppendAllText(latestLog, e.Exception.ToString());
        };
    }

    public static IHost? AppHost { get; private set; }

    public string Location => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

    private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        services.AddSingleton(s => new MainWindow
        {
            DataContext = s.GetRequiredService<MainWindowViewModel>()
        });

        // Add views
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<EditorViewModel>();
        
        // settings view
        services.AddSingleton<SettingsTabViewModel>();

        // Add services
        services.AddSingleton<ISynapseService, SynapseService>();
        services.AddSingleton<IDirectoryService, DirectoryService>();
        services.AddSingleton<ISocketService, SocketService>();
        services.AddSingleton<INavigationService, NavigationService>();
        
        services.AddSingleton<Func<Type, ViewModelBase>>(provider =>
            viewModelType => (ViewModelBase)provider.GetRequiredService(viewModelType));

        // Add settings
        // var configurationRoot = context.Configuration;
        //
        // services.Configure<EditorSettings>(configurationRoot.GetSection(nameof(EditorSettings)));
    }

    // private void ConfigureAppConfiugration(HostBuilderContext context, IConfigurationBuilder configuration)
    // {
    //     configuration.SetBasePath(Location);
    //     configuration.AddJsonFile("appsettings.json", false, true);
    // }

    private static void _initializeEnvironment()
    {
        while (
            string.IsNullOrWhiteSpace(Settings.Default.SynapseDirectory) ||
            !Directory.Exists(Settings.Default.SynapseDirectory) ||
            !FileSystem.IsDirectorySynapse(Settings.Default.SynapseDirectory))
        {
            var dialog = new CommonOpenFileDialog
            {
                Title = "Select your Synapse X's installation folder.",
                IsFolderPicker = true,
                Multiselect = false
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok && FileSystem.IsDirectorySynapse(dialog.FileName))
                Settings.Default.SynapseDirectory = dialog.FileName;
            else
                MessageBox.Show(
                    "Please choose your Synapse X Installation folder!",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
        }

        Environment.CurrentDirectory = Settings.Default.SynapseDirectory;
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        _initializeEnvironment();

        // Initialize hosting
        AppHost = Host.CreateDefaultBuilder()
            // .ConfigureAppConfiguration(ConfigureAppConfiugration)
            .ConfigureServices(ConfigureServices)
            .Build();

        await AppHost.StartAsync();

        var startupView = AppHost.Services.GetRequiredService<MainWindow>();
        startupView.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        AppHost!.Services.GetRequiredService<EditorViewModel>().UpdateSettings();

        await AppHost.StopAsync();

        base.OnExit(e);
    }
}