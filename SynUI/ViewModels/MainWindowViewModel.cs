using System;
using System.IO;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using sxlib;
using sxlib.Specialized;
using SynUI.Commands;

namespace SynUI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private string _title;
    private WindowState _windowState;
    private SxLibBase.SynLoadEvents _synLoadState;
    private SxLibBase.SynAttachEvents _synAttachState;

    private void _sxlib_OnLoadEvent(SxLibBase.SynLoadEvents e, object sender) =>
        SynLoadState = e;

    private void _sxlib_OnAttachEvent(SxLibBase.SynAttachEvents e, object sender) =>
        SynAttachState = e;

    private void _initializeSynapse(object o)
    {
        string dir = null;
        while (string.IsNullOrEmpty(dir))
        {
            var dialog = new CommonOpenFileDialog
            {
                Title = "Select your Synapse X's installation folder.",
                IsFolderPicker = true,
                Multiselect = false
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                dir = dialog.FileName;
        }

        Environment.CurrentDirectory = dir;
        Sxlib = SxLib.InitializeWPF(Application.Current.MainWindow, Directory.GetCurrentDirectory());
        Sxlib.LoadEvent += _sxlib_OnLoadEvent;
        Sxlib.AttachEvent += _sxlib_OnAttachEvent;
        Sxlib.Load();
    }

    public MainWindowViewModel()
    {
        CloseCommand = new DelegateCommand<object>(
            _ =>
            {
                Sxlib.ScriptHubMarkAsClosed();
                Sxlib.Close();
                Application.Current.MainWindow.Close();
            },
            _ => SynLoadState == SxLibBase.SynLoadEvents.READY);

        StateCommand = new DelegateCommand<object>(_ => WindowState = WindowState switch
        {
            WindowState.Maximized => WindowState.Normal,
            _ => WindowState.Maximized
        });

        MinimizeCommand = new DelegateCommand<object>(_ => WindowState = WindowState.Minimized);

        LoadedCommand = new DelegateCommand<object>(_initializeSynapse);
    }

    public DelegateCommand<object> CloseCommand { get; set; }
    public DelegateCommand<object> StateCommand { get; set; }
    public DelegateCommand<object> MinimizeCommand { get; set; }
    public DelegateCommand<object> LoadedCommand { get; set; }

    public SxLibWPF Sxlib { get; set; }

    public WindowState WindowState
    {
        get => _windowState;
        set
        {
            _windowState = value;
            OnPropertyChanged();
        }
    }

    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged();
        }
    }

    public SxLibBase.SynLoadEvents SynLoadState
    {
        get => _synLoadState;
        set
        {
            _synLoadState = value;
            OnPropertyChanged();
        }
    }

    public SxLibBase.SynAttachEvents SynAttachState
    {
        get => _synAttachState;
        set
        {
            _synAttachState = value;
            OnPropertyChanged();
        }
    }
}