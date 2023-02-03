using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
using sxlib;
using sxlib.Specialized;
using SynUI.Properties;

namespace SynUI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private string _title;
    private WindowState _windowState;
    private SxLibBase.SynLoadEvents _synLoadState;
    private SxLibBase.SynAttachEvents _synAttachState;

    private static bool _isDirectorySynapse(string path) =>
        new[]
        {
            "auth/options.bin",
            "auth/sessiontoken.bin",
            "bin/SLAgent.dll"
        }.All(p => File.Exists(Path.Combine(path, p)));

    private void _loadedCommand()
    {
        while (string.IsNullOrWhiteSpace(Settings.Default.SynapseDirectory))
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
                MessageBox.Show("Please choose your Synapse X Installation folder!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        Environment.CurrentDirectory = Settings.Default.SynapseDirectory;
        Sxlib = SxLib.InitializeWPF(Application.Current.MainWindow, Directory.GetCurrentDirectory());
        Sxlib.LoadEvent += _sxlib_OnLoadEvent;
        Sxlib.AttachEvent += _sxlib_OnAttachEvent;
        Sxlib.Load();
    }

    private void _sxlib_OnLoadEvent(SxLibBase.SynLoadEvents e, object sender) =>
        SynLoadState = e;

    private void _sxlib_OnAttachEvent(SxLibBase.SynAttachEvents e, object sender) =>
        SynAttachState = e;

    private void _closeCommand()
    {
        // goofy hack to forcefully close sxlib
        Sxlib.Close();
        Environment.Exit(0);
    }

    private void _stateCommand() =>
        WindowState = WindowState switch
        {
            WindowState.Maximized => WindowState.Normal,
            _ => WindowState.Maximized,
        };

    private void _minimizeCommand() =>
        WindowState = WindowState.Minimized;

    public MainWindowViewModel()
    {
        ClosedCommand = new RelayCommand(_closeCommand);
        StateCommand = new RelayCommand(_stateCommand);
        MinimizeCommand = new RelayCommand(_minimizeCommand);
        LoadedCommand = new RelayCommand(_loadedCommand);
    }

    public ICommand ClosedCommand { get; }
    public ICommand StateCommand { get; }
    public ICommand MinimizeCommand { get; }
    public ICommand LoadedCommand { get; }

    public SxLibWPF Sxlib { get; private set; }

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