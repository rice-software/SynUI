using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace SynUI.Models;

public class OutputResponse
{
    [JsonProperty("name")]
    public string? Name { get; set; }
    [JsonProperty("message")]
    public string? Message { get; set; }
    [JsonProperty("messageType")]
    public OutputType Type { get; set; }
}

public class Output
{
    public string? Name { get; set; }
    public ObservableCollection<OutputMessage> Outputs { get; } = new();
}

public class OutputMessage
{
    public string? Content { get; set; }
    public OutputType? Type { get; set; }
}

public enum OutputType
{
    [EnumMember(Value = "MessageOutput")]
    Output,
    [EnumMember(Value = "MessageInfo")]
    Info,
    [EnumMember(Value = "MessageWarning")]
    Warning,
    [EnumMember(Value = "MessageError")]
    Error
}