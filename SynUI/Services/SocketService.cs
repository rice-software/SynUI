using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using SynUI.Models;
using WatsonWebsocket;

namespace SynUI.Services;

public interface ISocketService
{
    ObservableCollection<Output> Outputs { get; }
}

public class SocketService : ISocketService
{
    public ObservableCollection<Output> Outputs { get; } = new();

    public SocketService()
    {
        var outputServer = new WatsonWsServer(port: 7500);
        outputServer.MessageReceived += _outputServer_MessageReceived;
        outputServer.Start();
    }

    private void _outputServer_MessageReceived(object sender, MessageReceivedEventArgs e) => Application.Current.Dispatcher.Invoke(() =>
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