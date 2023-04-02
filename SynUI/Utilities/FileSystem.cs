using System;
using System.IO;
using System.Linq;

namespace SynUI.Utilities;

internal static class FileSystem
{
    private static readonly string[] RequiredFiles =
    {
        "auth/options.bin",
        "auth/sessiontoken.bin",
        "bin/SLAgent.dll"
    };

    public static string NormalizeDirectoryPath(string folderPath)
    {
        if (folderPath.EndsWith(Path.DirectorySeparatorChar.ToString(),
                StringComparison.OrdinalIgnoreCase))
            return folderPath;

        return folderPath + Path.DirectorySeparatorChar;
    }

    public static bool IsPathEquals(bool isFolder, string? path, params string?[] paths)
    {
        if (path == null)
            return false;

        return paths.All(p =>
            p != null && (isFolder
                ? NormalizeDirectoryPath(Path.GetFullPath(p)) == NormalizeDirectoryPath(Path.GetFullPath(path))
                : Path.GetFullPath(p) == Path.GetFullPath(path)));
    }

    public static bool IsDirectorySynapse(string path)
    {
        return RequiredFiles.All(p => File.Exists(Path.Combine(path, p)));
    }
}