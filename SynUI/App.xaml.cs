using System;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.WindowsAPICodePack.Dialogs;
using sxlib;
using SynUI.Properties;
using SynUI.Services;
using SynUI.ViewModels;
using SynUI.Views;

namespace SynUI;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private static readonly string[] RequiredFiles =
    {
        "auth/options.bin",
        "auth/sessiontoken.bin",
        "bin/SLAgent.dll"
    };

    public static IHost? AppHost { get; private set; }

    // i have no idea what im doing.
    public App()
    {
        while (
            string.IsNullOrWhiteSpace(Settings.Default.SynapseDirectory) ||
            !Directory.Exists(Settings.Default.SynapseDirectory) ||
            !_isDirectorySynapse(Settings.Default.SynapseDirectory))
        {
            var dialog = new CommonOpenFileDialog
            {
                Title = "Select your Synapse X's installation folder.",
                IsFolderPicker = true,
                Multiselect = false
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok && _isDirectorySynapse(dialog.FileName))
                Settings.Default.SynapseDirectory = dialog.FileName;
            else
                MessageBox.Show(
                    "Please choose your Synapse X Installation folder!\n" +
                    "Your folder is missing these files:\n" +
                    string.Join("\n", RequiredFiles),
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
        }

        Environment.CurrentDirectory = Settings.Default.SynapseDirectory;

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
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<Func<Type, ViewModelBase>>(provider =>
                    viewModelType => (ViewModelBase)provider.GetRequiredService(viewModelType));

                services.AddSingleton<ISynapseService, SynapseService>();
            })
            .Build();
    }

    private static bool _isDirectorySynapse(string path) =>
        RequiredFiles.All(p => File.Exists(Path.Combine(path, p)));

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

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