using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using SynUI.Configurations;
using SynUI.Models;
using SynUI.Properties;
using SynUI.Services;
using SynUI.Utilities;
using SynUI.ViewModels.TabViewModels;

namespace SynUI.ViewModels;

// TODO: Refactor this whole thing.
public class EditorViewModel : ViewModelBase
{
    private ViewModelBase? _selectedEditorItem;
    private Output? _selectedOutput;


    public EditorViewModel(
        ISynapseService synapseServiceService,
        IDirectoryService directoryService,
        ISocketService socketService)
    {
        SynapseService = synapseServiceService;
        DirectoryService = directoryService;
        SocketService = socketService;

        AddItemCommand = new RelayCommand(_addItemCommand);
        RemoveItemCommand = new RelayCommand<ViewModelBase>(_removeItemCommand);
        KillRobloxItemCommand = new RelayCommand(_killRobloxCommand);
        SelectedExplorerNodeChangedCommand = new RelayCommand<ExplorerNode>(_selectedExplorerNodeChangedCommand);
        SaveCommand = new RelayCommand(_saveCommand, () => SelectedEditorItem is EditorTabViewModel);
        LoadCommand = new RelayCommand(_loadCommand);

        EditorItems = new ObservableCollection<ViewModelBase>();

        EditorItems.CollectionChanged += (_, o) =>
        {
            if (o.NewItems is not null)
                foreach (var newItems in o.NewItems)
                    if (newItems is EditorTabViewModel model)
                        model.PropertyChanged += (_, _) => _updateSettings();

            _updateSettings();
        };

        Settings.Default.EditorItems?.ForEach(o => EditorItems.Add(new EditorTabViewModel
        {
            Document =
            {
                FileName = o.FileName,
                Text = o.Content
            },
            IsAnchored = !string.IsNullOrWhiteSpace(o.FileName)
        }));

        if (EditorItems.Count == 0)
        {
            var welcome = new WelcomeTabViewModel(this);
            EditorItems.Add(welcome);
            SelectedEditorItem = welcome;
        }
    }

    public ICommand AddItemCommand { get; }
    public ICommand RemoveItemCommand { get; }
    public ICommand KillRobloxItemCommand { get; }
    public ICommand SelectedExplorerNodeChangedCommand { get; }
    public RelayCommand SaveCommand { get; }
    public RelayCommand LoadCommand { get; }

    public ObservableCollection<ViewModelBase> EditorItems { get; }

    public Output? SelectedOuput
    {
        get => _selectedOutput;
        set => SetProperty(ref _selectedOutput, value);
    }

    public ISynapseService? SynapseService { get; }

    public IDirectoryService? DirectoryService { get; }

    public ISocketService? SocketService { get; }

    public ViewModelBase? SelectedEditorItem
    {
        get => _selectedEditorItem;
        set
        {
            SetProperty(ref _selectedEditorItem, value);
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    private void _updateSettings()
    {
        Settings.Default.EditorItems = (
            from vm in EditorItems.ToList()
            where vm is EditorTabViewModel
            select new EditorTabSettingsItem
            {
                FileName = ((EditorTabViewModel)vm).Document.FileName,
                Content = ((EditorTabViewModel)vm).Document.Text
            }).ToList();
    }

    private void _addItemCommand()
    {
        var item = new EditorTabViewModel();
        EditorItems.Add(item);
        SelectedEditorItem = item;
    }

    private void _removeItemCommand(ViewModelBase? parameter)
    {
        if (parameter != null)
            EditorItems.Remove(parameter);
    }

    private void _killRobloxCommand()
    {
        try
        {
            foreach (var process in Process.GetProcessesByName("RobloxPlayerBeta"))
                process.Kill();
        }
        catch
        {
            /* ignore */
        }
    }

    private void _selectedExplorerNodeChangedCommand(ExplorerNode? parameter)
    {
        if (parameter is not ExplorerFile || parameter.FullPath == null)
            return;

        var item = EditorItems.FirstOrDefault(o =>
            o is EditorTabViewModel { Document.FileName: { } } model &&
            FileSystem.IsPathEquals(false, model.Document.FileName, parameter.FullPath));

        if (item == null)
        {
            item = new EditorTabViewModel
            {
                Document = { FileName = parameter.FullPath, Text = File.ReadAllText(parameter.FullPath) },
                IsAnchored = true
            };

            EditorItems.Add(item);
        }

        SelectedEditorItem = item;
    }

    private void _saveCommand()
    {
        if (SelectedEditorItem is not EditorTabViewModel model) return;

        if (string.IsNullOrWhiteSpace(model.Document.FileName))
        {
            var dialog = new SaveFileDialog
            {
                Title = "Save untitled file",
                Filter = "LUA file (*.lua)|*.lua|Text document (*.txt)|*.txt",
                InitialDirectory = Path.Combine(Directory.GetCurrentDirectory(), "scripts"),
                RestoreDirectory = true
            };

            if (dialog.ShowDialog() != true)
                return;

            model!.Document.FileName = dialog.FileName;
        }

        File.WriteAllText(model.Document.FileName, model.Document.Text);
    }

    private void _loadCommand()
    {
        var dialog = new OpenFileDialog
        {
            Title = "Open script file",
            Filter = "LUA file (*.lua)|*.lua|Text document (*.txt)|*.txt",
            InitialDirectory = Path.Combine(Directory.GetCurrentDirectory(), "scripts"),
            RestoreDirectory = true
        };

        if (dialog.ShowDialog() == true)
            EditorItems.Add(new EditorTabViewModel
            {
                Document = { FileName = dialog.FileName, Text = File.ReadAllText(dialog.FileName) },
                IsAnchored = true
            });
    }
}