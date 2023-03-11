using System;
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
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Win32;
using Newtonsoft.Json;
using Serilog;
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

    public ICommand AddItemCommand { get; }
    public ICommand RemoveItemCommand { get; }
    public ICommand KillRobloxItemCommand { get; }
    public ICommand SelectedExplorerNodeChangedCommand { get; }
    public RelayCommand SaveCommand { get; }
    public RelayCommand LoadCommand { get; }
    public RelayCommand ToggleOutputVisibility { get; }

    public ObservableCollection<EditorItem> EditorItems { get; } = new() { new EditorItem() };

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
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    public EditorViewModel(
        ISynapseService? synapseServiceService,
        IDirectoryService directoryService,
        ISocketService socketService)
    {
        SynapseService = synapseServiceService;
        DirectoryService = directoryService;
        SocketService = socketService;

        AddItemCommand = new RelayCommand(_addItemCommand);
        RemoveItemCommand = new RelayCommand<EditorItem>(_removeItemCommand);
        KillRobloxItemCommand = new RelayCommand(_killRobloxCommand);
        SelectedExplorerNodeChangedCommand = new RelayCommand<ExplorerNode>(_selectedExplorerNodeChangedCommand);
        SaveCommand = new RelayCommand(_saveCommand, () => SelectedEditorItem != null);
        LoadCommand = new RelayCommand(_loadCommand);
    }

    private void _addItemCommand()
    {
        var item = new EditorItem();
        EditorItems.Add(item);
        SelectedEditorItem = item;
    }

    private void _removeItemCommand(EditorItem? parameter)
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
        } catch { /* ignore */ }
    }

    private void _selectedExplorerNodeChangedCommand(ExplorerNode? parameter)
    {
        if (parameter is not ExplorerFile || parameter.FullPath == null)
            return;

        var item = EditorItems.FirstOrDefault(o =>
            o.Document.FileName != null && FileSystem.IsPathEquals(false, o.Document.FileName, parameter.FullPath));

        if (item == null)
        {
            item = new EditorItem
            {
                Document = { FileName = parameter.FullPath },
                IsAnchored = true
            };

            EditorItems.Add(item);
        }
        
        SelectedEditorItem = item;
    }

    private void _saveCommand()
    {
        if (string.IsNullOrWhiteSpace(SelectedEditorItem!.Document.FileName))
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

            SelectedEditorItem!.Document.FileName = dialog.FileName;
        }

        File.WriteAllText(SelectedEditorItem!.Document.FileName, SelectedEditorItem.Document.Text);
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
            EditorItems.Add(new EditorItem
            {
                Document = { FileName = dialog.FileName, Text = File.ReadAllText(dialog.FileName) },
                IsAnchored = true,
            });
    }
}