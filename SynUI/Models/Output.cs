using System.Collections.ObjectModel;

namespace SynUI.Models;

public class OutputPlayer
{
    public string Name { get; set; }
    public ObservableCollection<Output> Outputs { get; } = new();
}

public class Output
{
    public string Content { get; set; }
    public OutputType Type { get; set; }
}

public enum OutputType
{
    Output,
    Info,
    Warning,
    Error
}