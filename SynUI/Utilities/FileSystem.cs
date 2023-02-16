using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public static bool IsPathEquals(bool isFolder, string path, params string[] paths) =>
        paths.All(p => isFolder ?
            NormalizeDirectoryPath(Path.GetFullPath(p)) == NormalizeDirectoryPath(Path.GetFullPath(path)) :
            Path.GetFullPath(p) == Path.GetFullPath(path));

    public static bool IsDirectorySynapse(string path) =>
        RequiredFiles.All(p => File.Exists(Path.Combine(path, p)));
}