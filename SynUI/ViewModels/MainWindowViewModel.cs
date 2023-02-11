using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SynUI.Services;

namespace SynUI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private INavigationService _navigation;
    private ISynapseService _synapse;
    private string _title;
    private WindowState _windowState;

    public MainWindowViewModel(INavigationService navigationService, ISynapseService synapseService)
    {
        Navigation = navigationService;
        Synapse = synapseService;

        StateCommand = new RelayCommand(_stateCommand);
        MinimizeCommand = new RelayCommand(_minimizeCommand);
        LoadedCommand = new RelayCommand(Synapse.Initialize);

        Navigation.NavigateTo<EditorViewModel>();
    }

    public ICommand StateCommand { get; }
    public ICommand MinimizeCommand { get; }
    public ICommand LoadedCommand { get; }

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