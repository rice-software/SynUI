using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
using sxlib;
using sxlib.Specialized;
using SynUI.Properties;
using SynUI.Services;
using SynUI.Views;

namespace SynUI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private string? _title;
    private WindowState _windowState;
    private SxLibBase.SynLoadEvents _synLoadState;
    private SxLibBase.SynAttachEvents _synAttachState;
    private INavigationService _navigation;
    private ISynapseService _synapse;

    private void _stateCommand() =>
        WindowState = WindowState switch
        {
            WindowState.Maximized => WindowState.Normal,
            _ => WindowState.Maximized,
        };

    private void _minimizeCommand() =>
        WindowState = WindowState.Minimized;

    public MainWindowViewModel(INavigationService navigationService, ISynapseService synapseService)
    {
        Navigation = navigationService;
        Synapse = synapseService;
        
        StateCommand = new RelayCommand(_stateCommand);
        MinimizeCommand = new RelayCommand(_minimizeCommand);
        
        Navigation.NavigateTo<EditorViewModel>();
    }

    public ICommand StateCommand { get; }
    public ICommand MinimizeCommand { get; }

    public INavigationService Navigation
    {
        get => _navigation;
        set => SetProperty(ref _navigation, value);
    }

    public ISynapseService Synapse
    {
        get => _synapse;
        set => SetProperty(ref _synapse, value);
    }

    public string? Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public WindowState WindowState
    {
        get => _windowState;
        set => SetProperty(ref _windowState, value);
    }

    public SxLibBase.SynLoadEvents SynLoadState
    {
        get => _synLoadState;
        private set => SetProperty(ref _synLoadState, value);
    }

    public SxLibBase.SynAttachEvents SynAttachState
    {
        get => _synAttachState;
        private set => SetProperty(ref _synAttachState, value);
    }
}