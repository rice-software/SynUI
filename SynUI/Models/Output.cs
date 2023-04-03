using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace SynUI.Models;

public class OutputResponse : ObservableObject
{
    private string? message;
    private string? name;
    private string? type;

    [JsonProperty("name")]
    public string? Name
    {
        get => name;
        set => SetProperty(ref name, value);
    }

    [JsonProperty("message")]
    public string? Message
    {
        get => message;
        set => SetProperty(ref message, value);
    }

    [JsonProperty("messageType")]
    public string? Type
    {
        get => type;
        set => SetProperty(ref type, value);
    }
}

public class Output : ObservableObject
{
    private string? name;

    public string? Name
    {
        get => name;
        set => SetProperty(ref name, value);
    }

    public ObservableCollection<OutputMessage> Outputs { get; } = new();
}

public class OutputMessage : ObservableObject
{
    private string? content;

    public string? Content
    {
        get => content;
        set => SetProperty(ref content, value);
    }
}

public class OutputInfo : OutputMessage
{
}

public class OutputWarning : OutputMessage
{
}

public class OutputError : OutputMessage
{
}