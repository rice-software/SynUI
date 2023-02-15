using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using SynUI.Models;
using SynUI.Services;
using SynUI.Utilities;
using WatsonWebsocket;

namespace SynUI.ViewModels;

// TODO: Refactor this whole thing.
public class EditorViewModel : ViewModelBase
{
    private WatsonWsServer? _outputServer;
    private EditorItem? _selectedEditorItem;
    private Output? _selectedOutput;
    private ISynapseService? _synapse;

    private readonly FileSystemWatcher _fileWatcher;
    private readonly FileSystemWatcher _directoryWatcher;

    public EditorViewModel(ISynapseService? synapseService)
    {
        Synapse = synapseService;

        AddItemCommand = new RelayCommand(_addItemCommand);
        RemoveItemCommand = new RelayCommand<EditorItem>(_removeItemCommand);
        KillRobloxItemCommand = new RelayCommand(_killRobloxCommand);

        _fileWatcher = new FileSystemWatcher
        {
            Path = Path.Combine(Directory.GetCurrentDirectory(), "scripts"),
            NotifyFilter = NotifyFilters.FileName,
            IncludeSubdirectories = true,
            EnableRaisingEvents = true
        };

        _directoryWatcher = new FileSystemWatcher
        {
            Path = Path.Combine(Directory.GetCurrentDirectory(), "scripts"),
            NotifyFilter = NotifyFilters.DirectoryName,
            IncludeSubdirectories = true,
            EnableRaisingEvents = true
        };

        _initFolder(Path.Combine(Directory.GetCurrentDirectory(), "scripts"), ExplorerNodes);
        _initializeServer();
    }

    public ObservableCollection<ExplorerNode> ExplorerNodes { get; } = new();

    public ObservableCollection<EditorItem> EditorItems { get; } = new() { new EditorItem() };
    public ObservableCollection<Output> Outputs { get; } = new();

    public ICommand AddItemCommand { get; }
    public ICommand RemoveItemCommand { get; }
    public ICommand KillRobloxItemCommand { get; }

    public Output? SelectedOuput
    {
        get => _selectedOutput;
        set => SetProperty(ref _selectedOutput, value);
    }

    public ISynapseService? Synapse
    {
        get => _synapse;
        set => SetProperty(ref _synapse, value);
    }

    public EditorItem? SelectedEditorItem
    {
        get => _selectedEditorItem;
        set
        {
            SetProperty(ref _selectedEditorItem, value);
            OnPropertyChanged(nameof(IsTabSelected));
        }
    }

    public bool IsTabSelected => SelectedEditorItem != null;

    private void _addItemCommand()
    {
        EditorItems.Add(new EditorItem());
    }

    private void _removeItemCommand(EditorItem? parameter)
    {
        if (parameter != null)
            EditorItems.Remove(parameter);
    }

    private void _killRobloxCommand()
    {
        foreach (var process in Process.GetProcessesByName("RobloxPlayerBeta"))
            process.Kill();
    }

    private static bool _isPathEquals(bool isFolder, string path, params string[] paths) =>
        paths.All(p => isFolder ?
            FileSystem.NormalizeDirectoryPath(Path.GetFullPath(p)) == FileSystem.NormalizeDirectoryPath(Path.GetFullPath(path)) :
            Path.GetFullPath(p) == Path.GetFullPath(path));

    private void _initFile(string path, ICollection<ExplorerNode> nodes)
    {
        var item = new ExplorerFile { FullPath = path };

        _fileWatcher.Renamed += (_, e) => Application.Current.Dispatcher.Invoke(() =>
        {
            if (_isPathEquals(false, e.OldFullPath, item.FullPath))
                item.FullPath = e.FullPath;
        });

        _fileWatcher.Deleted += (_, e) => Application.Current.Dispatcher.Invoke(() =>
        {
            if (_isPathEquals(false, e.FullPath, item.FullPath))
                nodes.Remove(item);
        });

        nodes.Add(item);
    }

    private void _initFolder(string path, ICollection<ExplorerNode> nodes)
    {
        var item = new ExplorerDirectory { FullPath = path };

        _directoryWatcher.Renamed += (_, e) => Application.Current.Dispatcher.Invoke(() =>
        {
            if (_isPathEquals(true, e.OldFullPath, item.FullPath))
                item.FullPath = e.FullPath;
        });

        _directoryWatcher.Deleted += (_, e) => Application.Current.Dispatcher.Invoke(() =>
        {
            if (_isPathEquals(true, e.FullPath, item.FullPath))
                nodes.Remove(item);
        });

        _directoryWatcher.Created += (_, e) => Application.Current.Dispatcher.Invoke(() =>
        {
            if (_isPathEquals(true, Path.GetDirectoryName(e.FullPath)!, item.FullPath))
                _initFolder(e.FullPath, item.Nodes);
        });

        _fileWatcher.Created += (_, e) => Application.Current.Dispatcher.Invoke(() =>
        {
            if (_isPathEquals(true, Path.GetDirectoryName(e.FullPath)!, item.FullPath))
                _initFile(e.FullPath, item.Nodes);
        });

        foreach (var dir in Directory.GetDirectories(item.FullPath))
            _initFolder(dir, item.Nodes);

        foreach (var file in Directory.GetFiles(item.FullPath))
            _initFile(file, item.Nodes);

        nodes.Add(item);
    }

    private void _outputServer_MessageReceived(object sender, MessageReceivedEventArgs e)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            Debug.WriteLine($"received: \n{Encoding.UTF8.GetString(e.Data.Array!)}\ntype:{e.MessageType}");
            if (e.MessageType != WebSocketMessageType.Text)
                return;

            var json = Encoding.UTF8.GetString(e.Data.Array!);
            var deserialized = JsonConvert.DeserializeObject<OutputResponse>(json);

            var message = Outputs.FirstOrDefault(o => o.Name == deserialized!.Name);

            if (message == null)
            {
                message = new Output { Name = deserialized!.Name };
                Outputs.Add(message);
            }

            message.Outputs.Add(new OutputMessage { Content = deserialized!.Message, Type = deserialized.Type });
        });
    }

    private void _initializeServer()
    {
        _outputServer = new WatsonWsServer(port: 7500);
        _outputServer.ClientConnected += (_, e) => Debug.WriteLine("connected: " + e.Client.Name);
        _outputServer.ClientDisconnected += (_, e) => Debug.WriteLine("disconnected: " + e.Client.Name);
        _outputServer.MessageReceived += _outputServer_MessageReceived;
        _outputServer.Start();
    }
}