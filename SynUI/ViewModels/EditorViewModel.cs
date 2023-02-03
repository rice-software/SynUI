using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SynUI.Models;

namespace SynUI.ViewModels;

public class EditorViewModel : ViewModelBase
{
    private OutputPlayer _selectedOutput;

    private void _addItemCommand() =>
        EditorItems.Add(new EditorItem { Name = "new tab", Content = "hello world" });

    public EditorViewModel()
    {
        EditorItems = new ObservableCollection<EditorItem>();
        Outputs = new ObservableCollection<OutputPlayer>();

        AddItemCommand = new RelayCommand(_addItemCommand);

        EditorItems.Add(new EditorItem { Name = "tab 1", Content = "your mom 1" });
        EditorItems.Add(new EditorItem { Name = "tab 2", Content = "your mom 2" });
        EditorItems.Add(new EditorItem { Name = "tab 3", Content = "your mom 3" });
        EditorItems.Add(new EditorItem { Name = "tab 4", Content = "your mom 4" });

        var one = new OutputPlayer { Name = "retard 1" };
        one.Outputs.Add(new Output { Content = "this is output\nnew line!", Type = OutputType.Output });
        one.Outputs.Add(new Output { Content = "this is info", Type = OutputType.Info });
        one.Outputs.Add(new Output { Content = "this is warning", Type = OutputType.Warning });
        one.Outputs.Add(new Output { Content = "this is error", Type = OutputType.Error });

        var two = new OutputPlayer { Name = "retard 2" };
        two.Outputs.Add(new Output { Content = "this is output", Type = OutputType.Output });
        two.Outputs.Add(new Output { Content = "this is info", Type = OutputType.Info });
        two.Outputs.Add(new Output { Content = "this is warning", Type = OutputType.Warning });
        two.Outputs.Add(new Output { Content = "this is error", Type = OutputType.Error });

        Outputs.Add(one);
        Outputs.Add(two);
    }

    public ObservableCollection<EditorItem> EditorItems { get; }
    public ObservableCollection<OutputPlayer> Outputs { get; }

    public ICommand AddItemCommand { get; }

    public OutputPlayer SelectedOuput
    {
        get => _selectedOutput;
        set => SetProperty(ref _selectedOutput, value);
    }
}