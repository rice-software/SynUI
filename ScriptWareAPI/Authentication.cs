using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace ScriptWareAPI;

internal static class Authentication
{
    public static volatile string Token = string.Empty;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [DllImport("bin\\ScriptWareAuth.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern void doAuthentication(
        bool resetHwid,
        string HardwareID,
        byte[] buf,
        string Username = "",
        string Password = "",
        string dxid = "",
        string dxid_other = "",
        string emailCode = "");

    public static Dictionary<string, string> AuthResponses { get; } = new()
    {
        { "1001", "Unknown." }
    };
}