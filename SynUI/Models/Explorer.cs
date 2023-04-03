using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SynUI.Models;

public class ExplorerNode : ObservableObject
{
    private string? fullPath;

    public string? FullPath
    {
        get => fullPath;
        set => SetProperty(ref fullPath, value);
    }
}

public class ExplorerDirectory : ExplorerNode
{
    public ObservableCollection<ExplorerNode> Nodes { get; } = new();
}

public class ExplorerFile : ExplorerNode
{
}