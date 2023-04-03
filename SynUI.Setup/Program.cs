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

        public static string SolutionDirectory;
        
        public static string SolutionPath => Path.Combine(SolutionDirectory, "SynUI.sln");

        public static string AppDirectory => Path.Combine(SolutionDirectory, @"SynUI");
        public static string AppReleaseDirectory => Path.Combine(AppDirectory, @"bin\Release");
        public static string AppExecPath => Path.Combine(AppReleaseDirectory, "SynUI.exe");

        public static string SetupDirectory => Path.Combine(SolutionDirectory, @"SynUI.Setup");
        public static string InnoSetupDirectory => Path.Combine(SetupDirectory, @"Inno");
        public static string InnoSetupPath => Path.Combine(InnoSetupDirectory, "setup.iss");

        public static string OutputDirectory => Path.Combine(SolutionDirectory, @"Output");
        public static string OutputPortableZipPath => Path.Combine(OutputDirectory, "portable.zip");


        public static void Main(string[] args)
        {
            InitializePath();
            CompileInno();
            CompilePortableZip();
        }

        public static void InitializePath()
        {
            SolutionDirectory = Directory.GetCurrentDirectory();

            if (!File.Exists(SolutionPath))
            {
                Console.WriteLine("Invalid parent directory: " + SolutionDirectory);
                SolutionDirectory = Path.GetFullPath(@"..\..\..\");

                if (!File.Exists(SolutionPath))
                {
                    Console.WriteLine("Invalid parent directory: " + SolutionDirectory);
                    Environment.Exit(0);
                }
            }
            
            // Cleaning up output
            if (Directory.Exists(OutputDirectory))
                Directory.Delete(OutputDirectory, true);
            
            // Create output folder
            Directory.CreateDirectory(OutputDirectory);
        }

        public static void CompileInno()
        {
            if (!File.Exists(InnoSetupCompilerPath))
            {
                Console.WriteLine("InnoSetup isn't installed!");
                return;
            }

            var proc = Process.Start(new ProcessStartInfo
            {
                FileName = InnoSetupCompilerPath,
                Arguments = $"/Q \"{InnoSetupPath}\"",
                UseShellExecute = false
            });

            proc?.WaitForExit();
        }

        public static void CompilePortableZip()
        {
            if (File.Exists(OutputPortableZipPath))
                File.Delete(OutputPortableZipPath);
            
            if (Directory.Exists(AppReleaseDirectory))
                ZipFile.CreateFromDirectory(AppReleaseDirectory, OutputPortableZipPath);
            else
                Console.WriteLine("Build directory doesn't exist!");
        }
    }
}