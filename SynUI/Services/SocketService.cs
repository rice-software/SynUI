using System.Collections.ObjectModel;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
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
    public SocketService()
    {
        var outputServer = new WatsonWsServer(port: 7500);
        outputServer.MessageReceived += _outputServer_MessageReceived;
        outputServer.Start();
    }

    public ObservableCollection<Output> Outputs { get; } = new();

    private void _outputServer_MessageReceived(object sender, MessageReceivedEventArgs e)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
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

            message.Outputs.Add(deserialized?.Type switch
            {
                "MessageInfo" => new OutputInfo { Content = deserialized.Message },
                "MessageWarning" => new OutputWarning { Content = deserialized.Message },
                "MessageError" => new OutputError { Content = deserialized.Message },
                _ => new OutputMessage { Content = deserialized?.Message }
            });
        });
    }
}