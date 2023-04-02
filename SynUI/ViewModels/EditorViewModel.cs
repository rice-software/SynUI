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

        AddItemCommand = new RelayCommand<ViewModelBase?>(_addItemCommand);
        RemoveItemCommand = new RelayCommand<ViewModelBase>(_removeItemCommand);
        KillRobloxItemCommand = new RelayCommand(_killRobloxCommand);
        SelectedExplorerNodeOnDoubleClickCommand =
            new RelayCommand<ExplorerNode>(_selectedExplorerNodeOnDoubleClickCommand);
        SaveCommand = new RelayCommand(_saveCommand, () => SelectedEditorItem is EditorTabViewModel);
        LoadCommand = new RelayCommand(_loadCommand);
        SelectNextTabCommand = new RelayCommand(_selectNextTabCommand);
        SelectPreviousTabCommand = new RelayCommand(_selectPreviousTabCommand);

        EditorItems = new ObservableCollection<ViewModelBase>();

        EditorItems.CollectionChanged += (_, o) =>
        {
            if (o.NewItems is not null)
                foreach (var newItems in o.NewItems)
                {
                    if (newItems is EditorTabViewModel model)
                        model.PropertyChanged += (_, _) => _updateSettings();
                    SelectedEditorItem = (ViewModelBase)newItems;
                }

            if (o.OldItems is not null &&
                (SelectedEditorItem is null || o.OldItems.Contains(SelectedEditorItem)) &&
                EditorItems.Count > 0)
                SelectedEditorItem = EditorItems[o.OldStartingIndex - 1 >= 0 ? o.OldStartingIndex - 1 : 0];

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
            EditorItems.Add(new WelcomeTabViewModel(this));
    }

    public ICommand AddItemCommand { get; }
    public ICommand RemoveItemCommand { get; }
    public ICommand KillRobloxItemCommand { get; }
    public ICommand SelectedExplorerNodeOnDoubleClickCommand { get; }
    public RelayCommand SaveCommand { get; }
    public RelayCommand LoadCommand { get; }
    public RelayCommand SelectPreviousTabCommand { get; }
    public RelayCommand SelectNextTabCommand { get; }

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
            OnPropertyChanged(nameof(SelectedDocument));
            OnPropertyChanged(nameof(IsSelectedDocument));
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    public EditorTabViewModel? SelectedDocument => SelectedEditorItem as EditorTabViewModel;
    public bool IsSelectedDocument => SelectedEditorItem is EditorTabViewModel;

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

    private void _addItemCommand(ViewModelBase? item)
    {
        item ??= new EditorTabViewModel();

        if (EditorItems.Contains(item))
            item = EditorItems.FirstOrDefault(o => o.Equals(item));
        else
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

    private void _selectedExplorerNodeOnDoubleClickCommand(ExplorerNode? parameter)
    {
        if (parameter is not ExplorerFile || parameter.FullPath == null)
            return;

        var item = new EditorTabViewModel
        {
            Document = { FileName = parameter.FullPath, Text = File.ReadAllText(parameter.FullPath) },
            IsAnchored = true
        };

        _addItemCommand(item);
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

            model.Document.FileName = dialog.FileName;
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

    private void _selectNextTabCommand()
    {
        if (EditorItems.Count == 0)
            return;

        if (SelectedEditorItem is null)
        {
            SelectedEditorItem = EditorItems[0];
        }
        else
        {
            var index = EditorItems.IndexOf(SelectedEditorItem);
            SelectedEditorItem = EditorItems[index + 1 < EditorItems.Count ? index + 1 : 0];
        }
    }

    private void _selectPreviousTabCommand()
    {
        if (EditorItems.Count == 0)
            return;

        if (SelectedEditorItem is null)
        {
            SelectedEditorItem = EditorItems[EditorItems.Count - 1];
        }
        else
        {
            var index = EditorItems.IndexOf(SelectedEditorItem);
            SelectedEditorItem = EditorItems[index - 1 >= 0 ? index - 1 : EditorItems.Count - 1];
        }
    }
}