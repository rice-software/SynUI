using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace SynUI.Setup
{
    internal class Program
    {
        public const string InnoSetupCompilerPath = @"C:\Program Files (x86)\Inno Setup 6\ISCC.exe";
        // public const string AppName = "SynUI";
        // public static string AppVersion;

        public static string SolutionDirectory => Path.GetFullPath(@"..\..\..\");

        public static string AppDirectory => Path.GetFullPath(Path.Combine(SolutionDirectory, "SynUI"));
        public static string AppReleaseDirectory => Path.GetFullPath(Path.Combine(AppDirectory, @"bin\Release"));
        public static string AppExecPath => Path.GetFullPath(Path.Combine(AppReleaseDirectory, "SynUI.exe"));

        public static string SetupDirectory => Path.GetFullPath(Path.Combine(SolutionDirectory, "SynUI.Setup"));
        public static string InnoSetupDirectory => Path.GetFullPath(Path.Combine(SetupDirectory, "Inno"));
        public static string InnoSetupPath => Path.GetFullPath(Path.Combine(InnoSetupDirectory, "setup.iss"));

        public static string OutputDirectory => Path.GetFullPath(Path.Combine(SolutionDirectory, "Output"));
        public static string OutputPortableZipPath => Path.GetFullPath(Path.Combine(OutputDirectory, "portable.zip"));


        public static void Main(string[] args)
        {
            // Create output folder
            Directory.CreateDirectory(OutputDirectory);
            
            Console.WriteLine(SolutionDirectory);
            Console.WriteLine(AppDirectory);
            Console.WriteLine(AppReleaseDirectory);
            Console.WriteLine(AppExecPath);
            Console.WriteLine(SetupDirectory);
            Console.WriteLine(InnoSetupDirectory);
            Console.WriteLine(InnoSetupPath);
            Console.WriteLine(OutputDirectory);
            Console.WriteLine(OutputPortableZipPath);

            CompileInno();
            CompilePortableZip();
        }

        public static void CompileInno()
        {
            if (!File.Exists(InnoSetupCompilerPath))
            {
                Console.WriteLine("InnoSetup isn't installed!");
                return;
            }

            Process.Start(new ProcessStartInfo
            {
                FileName = InnoSetupCompilerPath,
                Arguments = $"/Q \"{InnoSetupPath}\"",
                WorkingDirectory = InnoSetupDirectory,
                UseShellExecute = false
            });
        }

        public static void CompilePortableZip()
        {
            if (File.Exists(OutputPortableZipPath))
                File.Delete(OutputPortableZipPath);

            ZipFile.CreateFromDirectory(AppReleaseDirectory, OutputPortableZipPath);
        }
    }
}