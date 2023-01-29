using System.Collections.ObjectModel;
using SynUI.Models;

namespace SynUI.ViewModels;

public class EditorViewModel : ViewModelBase
{
    private OutputPlayer _selectedOutput;

    public EditorViewModel()
    {
        Outputs = new ObservableCollection<OutputPlayer>();

        var one = new OutputPlayer { Name = "retard 1" };
        one.Outputs.Add(new Output { Content = "this is output\nnew line!", Type = OutputType.Output });
        one.Outputs.Add(new Output { Content = "this is info", Type = OutputType.Info });
        one.Outputs.Add(new Output { Content = "this is warning", Type = OutputType.Warning });
        one.Outputs.Add(new Output { Content = "this is error", Type = OutputType.Error });

        var two = new OutputPlayer { Name = "retard 1" };
        two.Outputs.Add(new Output { Content = "this is output", Type = OutputType.Output });
        two.Outputs.Add(new Output { Content = "this is info", Type = OutputType.Info });
        two.Outputs.Add(new Output { Content = "this is warning", Type = OutputType.Warning });
        two.Outputs.Add(new Output { Content = "this is error", Type = OutputType.Error });

        Outputs.Add(one);
        Outputs.Add(two);
    }

    public ObservableCollection<OutputPlayer> Outputs { get; }

    public OutputPlayer SelectedOuput
    {
        get => _selectedOutput;
        set
        {
            _selectedOutput = value;
            OnPropertyChanged();
        }
    }
}