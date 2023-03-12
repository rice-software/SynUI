using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using SynUI.Models;
using SynUI.Utilities;

namespace SynUI.Services;

public interface IDirectoryService
{
    ObservableCollection<ExplorerNode> Items { get; }
}

public class DirectoryService : IDirectoryService
{
    private readonly FileSystemWatcher _directoryWatcher;
    private readonly FileSystemWatcher _fileWatcher;

    public DirectoryService()
    {
        _fileWatcher = new FileSystemWatcher
        {
            Path = Path.Combine(Directory.GetCurrentDirectory(), "scripts"),
            NotifyFilter = NotifyFilters.FileName,
            IncludeSubdirectories = true,
            EnableRaisingEvents = true
        };

        _directoryWatcher = new FileSystemWatcher
        {
            Path = Path.Combine(Directory.GetCurrentDirectory(), "scripts"),
            NotifyFilter = NotifyFilters.DirectoryName,
            IncludeSubdirectories = true,
            EnableRaisingEvents = true
        };

        _initFolder(Path.Combine(Directory.GetCurrentDirectory(), "scripts"), new[] { ".txt", ".lua" }, Items);
    }

    public ObservableCollection<ExplorerNode> Items { get; } = new();

    private void _initFile(string path, ICollection<ExplorerNode> nodes)
    {
        var item = new ExplorerFile { FullPath = path };

        _fileWatcher.Renamed += (_, e) => Application.Current.Dispatcher.Invoke(() =>
        {
            if (FileSystem.IsPathEquals(false, e.OldFullPath, item.FullPath))
                item.FullPath = e.FullPath;
        });

        _fileWatcher.Deleted += (_, e) => Application.Current.Dispatcher.Invoke(() =>
        {
            if (FileSystem.IsPathEquals(false, e.FullPath, item.FullPath))
                nodes.Remove(item);
        });

        nodes.Add(item);
    }

    private void _initFolder(string path, string[] extensions, ICollection<ExplorerNode> nodes)
    {
        var item = new ExplorerDirectory { FullPath = path };

        _directoryWatcher.Renamed += (_, e) => Application.Current.Dispatcher.Invoke(() =>
        {
            if (FileSystem.IsPathEquals(true, e.OldFullPath, item.FullPath))
                item.FullPath = e.FullPath;
        });

        _directoryWatcher.Deleted += (_, e) => Application.Current.Dispatcher.Invoke(() =>
        {
            if (FileSystem.IsPathEquals(true, e.FullPath, item.FullPath))
                nodes.Remove(item);
        });

        _directoryWatcher.Created += (_, e) => Application.Current.Dispatcher.Invoke(() =>
        {
            if (FileSystem.IsPathEquals(true, Path.GetDirectoryName(e.FullPath)!, item.FullPath))
                _initFolder(e.FullPath, extensions, item.Nodes);
        });

        _fileWatcher.Created += (_, e) => Application.Current.Dispatcher.Invoke(() =>
        {
            if (FileSystem.IsPathEquals(true, Path.GetDirectoryName(e.FullPath)!, item.FullPath) &&
                extensions.Contains(Path.GetExtension(e.FullPath)))
                _initFile(e.FullPath, item.Nodes);
        });

        foreach (var dir in Directory.GetDirectories(item.FullPath))
            _initFolder(dir, extensions, item.Nodes);

        foreach (var file in Directory.GetFiles(item.FullPath))
            _initFile(file, item.Nodes);

        nodes.Add(item);
    }
}