using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynUI.Commands;

public class DelegateCommand<T> : System.Windows.Input.ICommand
{
    private readonly Predicate<T> _canExecute;
    private readonly Action<T> _execute;

    public DelegateCommand(Action<T> execute)
        : this(execute, null)
    {
    }

    public DelegateCommand(Action<T> execute, Predicate<T> canExecute)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(object parameter)
    {
        return
            _canExecute == null ||
            _canExecute(parameter == null ?
                default :
                (T)Convert.ChangeType(parameter, typeof(T)));
    }

    public void Execute(object parameter)
    {
        _execute(parameter == null ? default : (T)Convert.ChangeType(parameter, typeof(T)));
    }

    public event EventHandler CanExecuteChanged;
    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}