using System.IO;
using System.Windows;
using ICSharpCode.AvalonEdit.Document;
using SynUI.Utilities;

namespace SynUI.ViewModels.TabViewModels;

public class EditorTabViewModel : ViewModelBase
{
    private readonly FileSystemWatcher _watcher;
    private bool _isAnchored;

    public EditorTabViewModel()
    {
        _watcher = new FileSystemWatcher();

        _watcher.Renamed += (_, o) => Application.Current.Dispatcher.Invoke(() =>
        {
            if (FileSystem.IsPathEquals(false, o.OldFullPath, Document.FileName))
                Document.FileName = o.FullPath;
        });

        _watcher.NotifyFilter = NotifyFilters.FileName;
        _watcher.EnableRaisingEvents = IsAnchored;

        Document.FileNameChanged += (_, _) => OnPropertyChanged(nameof(Document));
        Document.TextChanged += (_, _) => OnPropertyChanged(nameof(Document));
    }

    public bool IsAnchored
    {
        get => _isAnchored;
        set
        {
            SetProperty(ref _isAnchored, value);
            _watcher.Path = Path.GetDirectoryName(Document.FileName);
            _watcher.EnableRaisingEvents = IsAnchored;
        }
    }

    public TextDocument Document { get; } = new();
}