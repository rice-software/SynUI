using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SynUI.Services;

namespace SynUI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private INavigationService? _navigationService;
    private ISynapseService? _synapseService;
    private string _title = "SynUI";
    private WindowState _windowState;

    public MainWindowViewModel(INavigationService? navigationServiceService, ISynapseService? synapseServiceService)
    {
        NavigationService = navigationServiceService;
        SynapseService = synapseServiceService;

        StateCommand = new RelayCommand(_stateCommand);
        MinimizeCommand = new RelayCommand(_minimizeCommand);
        LoadedCommand = new RelayCommand(SynapseService!.Initialize);
        ClosingCommand = new RelayCommand(() => Environment.Exit(0));

        NavigateToEditorCommand = new RelayCommand(() => NavigationService!.NavigateTo<EditorViewModel>());
        NavigateToSettingsCommand = new RelayCommand(() => NavigationService!.NavigateTo<SettingsViewModel>());

        NavigationService!.NavigateTo<EditorViewModel>();
    }

    public ICommand StateCommand { get; }
    public ICommand MinimizeCommand { get; }
    public ICommand LoadedCommand { get; }
    public ICommand ClosingCommand { get; }

    public ICommand NavigateToEditorCommand { get; }
    public ICommand NavigateToSettingsCommand { get; }

    public string Version => Assembly
        .GetExecutingAssembly()
        .GetName()
        .Version.ToString();

    public INavigationService? NavigationService
    {
        get => _navigationService;
        set => SetProperty(ref _navigationService, value);
    }

    public ISynapseService? SynapseService
    {
        get => _synapseService;
        set => SetProperty(ref _synapseService, value);
    }

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public WindowState WindowState
    {
        get => _windowState;
        set => SetProperty(ref _windowState, value);
    }

    private void _stateCommand()
    {
        WindowState = WindowState switch
        {
            WindowState.Maximized => WindowState.Normal,
            _ => WindowState.Maximized
        };
    }

    private void _minimizeCommand()
    {
        WindowState = WindowState.Minimized;
    }
}