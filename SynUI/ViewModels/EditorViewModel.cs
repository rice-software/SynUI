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
    private EditorItem? _selectedEditorItem;
    private Output? _selectedOutput;
    private ISynapseService? _synapseService;
    private IDirectoryService? _directoryService;
    private ISocketService? _socketService;

    public EditorViewModel(ISynapseService? synapseServiceService, IDirectoryService directoryService, ISocketService socketService)
    {
        SynapseService = synapseServiceService;
        DirectoryService = directoryService;
        SocketService = socketService;

        AddItemCommand = new RelayCommand(_addItemCommand);
        RemoveItemCommand = new RelayCommand<EditorItem>(_removeItemCommand);
        KillRobloxItemCommand = new RelayCommand(_killRobloxCommand);
    }

    public ObservableCollection<EditorItem> EditorItems { get; } = new() { new EditorItem() };

    public ICommand AddItemCommand { get; }
    public ICommand RemoveItemCommand { get; }
    public ICommand KillRobloxItemCommand { get; }

    public Output? SelectedOuput
    {
        get => _selectedOutput;
        set => SetProperty(ref _selectedOutput, value);
    }

    public ISynapseService? SynapseService
    {
        get => _synapseService;
        private set => SetProperty(ref _synapseService, value);
    }

    public IDirectoryService? DirectoryService
    {
        get => _directoryService;
        private set => SetProperty(ref _directoryService, value);
    }

    public ISocketService? SocketService
    {
        get => _socketService;
        private set => SetProperty(ref _socketService, value);
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

    private static void _killRobloxCommand()
    {
        foreach (var process in Process.GetProcessesByName("RobloxPlayerBeta"))
            process.Kill();
    }
}