using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SynUI.Models;

public class ExplorerNode : ObservableObject
{
    private string? _fullPath;

    public string? FullPath
    {
        get => _fullPath;
        set => SetProperty(ref _fullPath, value);
    }
}

public class ExplorerDirectory : ExplorerNode
{
    public ObservableCollection<ExplorerNode> Nodes { get; } = new();
}

public class ExplorerFile : ExplorerNode {}