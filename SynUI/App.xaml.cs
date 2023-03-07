using System;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.WindowsAPICodePack.Dialogs;
using ModernWpf;
using sxlib;
using SynUI.Properties;
using SynUI.Services;
using SynUI.Utilities;
using SynUI.ViewModels;
using SynUI.Views;

namespace SynUI;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IHost? AppHost { get; private set; }

    public App()
    {
        // Auto save settings
        Settings.Default.PropertyChanged += (_, _) => Settings.Default.Save();
    }

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

    // i have no idea what im doing.
    protected override async void OnStartup(StartupEventArgs e)
    {
        _initializeEnvironment();

        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton(s => new MainWindow
                {
                    DataContext = s.GetRequiredService<MainWindowViewModel>()
                });

                // Add views
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<EditorViewModel>();
                services.AddSingleton<SettingsViewModel>();

                // Add services
                services.AddSingleton<ISynapseService, SynapseService>();
                services.AddSingleton<IDirectoryService, DirectoryService>();
                services.AddSingleton<ISocketService, SocketService>();
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<Func<Type, ViewModelBase>>(provider =>
                    viewModelType => (ViewModelBase)provider.GetRequiredService(viewModelType));
            })
            .Build();

        await AppHost.StartAsync();

        var startupView = AppHost.Services.GetRequiredService<MainWindow>();
        startupView.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();

        base.OnExit(e);
    }
}