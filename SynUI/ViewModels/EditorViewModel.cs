using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using SynUI.Models;
using SynUI.Services;
using WatsonWebsocket;

namespace SynUI.ViewModels;

// TODO: Refactor this whole thing.
public class EditorViewModel : ViewModelBase
{
    private WatsonWsServer _outputServer;
    private EditorItem _selectedEditorItem;
    private Output _selectedOutput;
    private ISynapseService _synapse;

    public EditorViewModel(ISynapseService synapseService)
    {
        Synapse = synapseService;

        AddItemCommand = new RelayCommand(_addItemCommand);
        RemoveItemCommand = new RelayCommand<EditorItem>(_removeItemCommand);

        _initializeServer();
    }

    public ObservableCollection<EditorItem> EditorItems { get; } = new() { new EditorItem() };
    public ObservableCollection<Output> Outputs { get; } = new();

    public ICommand AddItemCommand { get; }
    public ICommand RemoveItemCommand { get; }

    public Output SelectedOuput
    {
        get => _selectedOutput;
        set => SetProperty(ref _selectedOutput, value);
    }

    public ISynapseService Synapse
    {
        get => _synapse;
        set => SetProperty(ref _synapse, value);
    }

    public EditorItem SelectedEditorItem
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

    private void _removeItemCommand(EditorItem parameter)
    {
        if (parameter != null)
            EditorItems.Remove(parameter);
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