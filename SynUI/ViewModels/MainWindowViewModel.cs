using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SynUI.Services;
using SynUI.ViewModels.TabViewModels;

namespace SynUI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private string _title = "SynUI";
    private WindowState _windowState;

    public MainWindowViewModel(
        INavigationService navigationServiceService,
        ISettingsService settingsService,
        ISynapseService synapseService,
        EditorViewModel editorViewModel)
    {
        NavigationService = navigationServiceService;
        SettingsService = settingsService;
        SynapseService = synapseService;
        EditorViewModel = editorViewModel;

        StateCommand = new RelayCommand(_stateCommand);
        MinimizeCommand = new RelayCommand(_minimizeCommand);
        LoadedCommand = new RelayCommand(SynapseService!.Initialize);
        ClosingCommand = new RelayCommand(() => Environment.Exit(0));

        NavigateToEditorCommand = new RelayCommand(() => NavigationService.NavigateTo<EditorViewModel>());
        NavigateToSettingsCommand =
            new RelayCommand(() => EditorViewModel.AddItemCommand.Execute(new SettingsTabViewModel(SettingsService)));

        NavigationService!.NavigateTo<EditorViewModel>();
    }

    public ICommand StateCommand { get; }
    public ICommand MinimizeCommand { get; }
    public ICommand LoadedCommand { get; }
    public ICommand ClosingCommand { get; }

    public ICommand NavigateToEditorCommand { get; }
    public ICommand NavigateToSettingsCommand { get; }

    public string Version =>
        FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

    public INavigationService? NavigationService { get; }
    public ISettingsService? SettingsService { get; }
    public ISynapseService? SynapseService { get; }
    public EditorViewModel? EditorViewModel { get; }

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