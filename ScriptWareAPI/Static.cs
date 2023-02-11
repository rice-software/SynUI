using System;
using System.IO;

namespace ScriptWareAPI;

internal static class Links
{
    public static Uri Injector => new("https://script-ware.com/api/serve/beta/ScriptWareInjector.dll");
    public static Uri Auth => new("https://script-ware.com/api/serve/beta/ScriptWareAuth.dll");
    public static Uri Loader => new("https://script-ware.com/api/serve/beta/ScriptWareLoader.dll");
}

internal static class FileNames
{
    public static string BaseDirectory => "bin";
    
    public static string Injector => Path.Combine(BaseDirectory, "ScriptWareInjector.dll");
    public static string Auth => Path.Combine(BaseDirectory, "ScriptWareAuth.dll");
    public static string Loader => Path.Combine(BaseDirectory, "ScriptWareLoader.dll");
}