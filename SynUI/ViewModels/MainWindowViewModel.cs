using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SynUI.Services;

namespace SynUI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private string _title = "SynUI";
    private WindowState _windowState;

    public ICommand StateCommand { get; }
    public ICommand MinimizeCommand { get; }
    public ICommand LoadedCommand { get; }
    public ICommand ClosingCommand { get; }

    public ICommand NavigateToEditorCommand { get; }

    public string Version =>
        FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

    public INavigationService? NavigationService { get; }
    public ISynapseService? SynapseService { get; }

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

    public MainWindowViewModel(INavigationService navigationServiceService, ISynapseService synapseServiceService)
    {
        NavigationService = navigationServiceService;
        SynapseService = synapseServiceService;

        StateCommand = new RelayCommand(_stateCommand);
        MinimizeCommand = new RelayCommand(_minimizeCommand);
        LoadedCommand = new RelayCommand(SynapseService!.Initialize);
        ClosingCommand = new RelayCommand(() => Environment.Exit(0));

        NavigateToEditorCommand = new RelayCommand(() => NavigationService!.NavigateTo<EditorViewModel>());

        NavigationService!.NavigateTo<EditorViewModel>();
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