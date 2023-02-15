using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace SynUI.Models;

public class OutputResponse : ObservableObject
{
    private string? _name;
    private string? _message;
    private OutputType _type;

    [JsonProperty("name")]
    public string? Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    [JsonProperty("message")]
    public string? Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    [JsonProperty("messageType")]
    public OutputType Type
    {
        get => _type;
        set => SetProperty(ref _type, value);
    }
}

public class Output : ObservableObject
{
    private string? _name;

    public string? Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public ObservableCollection<OutputMessage> Outputs { get; } = new();
}

public class OutputMessage : ObservableObject
{
    private string? _content;
    private OutputType? _type;

    public string? Content
    {
        get => _content;
        set => SetProperty(ref _content, value);
    }

    public OutputType? Type
    {
        get => _type;
        set => SetProperty(ref _type, value);
    }
}

public enum OutputType
{
    [EnumMember(Value = "MessageOutput")] Output,
    [EnumMember(Value = "MessageInfo")] Info,
    [EnumMember(Value = "MessageWarning")] Warning,
    [EnumMember(Value = "MessageError")] Error
}