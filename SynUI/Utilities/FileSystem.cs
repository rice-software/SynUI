using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynUI.Utilities;

internal static class FileSystem
{
    public static string NormalizeDirectoryPath(string folderPath)
    {
        if (folderPath.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString(),
                StringComparison.OrdinalIgnoreCase))
            return folderPath;

        return folderPath + System.IO.Path.DirectorySeparatorChar;
    }
}