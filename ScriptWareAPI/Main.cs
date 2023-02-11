using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ScriptWareAPI;

public class ScriptWareAPI : IDisposable
{
    private readonly WebClient _client = new();

    public bool IsReady { get; private set; }
    
    public async Task InitializeAsync()
    {
        Directory.CreateDirectory(FileNames.BaseDirectory);
        
        await _client.DownloadFileTaskAsync(Links.Injector, FileNames.Injector);
        await _client.DownloadFileTaskAsync(Links.Auth, FileNames.Auth);
        await _client.DownloadFileTaskAsync(Links.Loader, FileNames.Loader);

        IsReady = true;
    }

    public bool Authenticate(string username, string password)
    {
        if (!IsReady)
            return false;
        
        string authResponse;
        
        try
        {
            var numArray = new byte[300];

            Authentication.doAuthentication(
                false, HwidGrabber.GetHwid(),
                numArray, username, password,
                string.Empty, string.Empty);
            
            authResponse = Encoding.ASCII.GetString(TrimArray(numArray));
        }
        catch { return false; }
        
        
    }

    public void Dispose()
    {
        _client?.Dispose();
    }
    
    // lifted straight from the decompiled sw ui src lmao
    public static byte[] TrimArray(byte[] targetArray)
    {
        var enumerator = targetArray.GetEnumerator();
        var numArray1 = new byte[targetArray.Length];
        var length = 0;
        var num = 0;
        while (enumerator.MoveNext())
        {
            if (enumerator.Current != null && enumerator.Current.ToString().Equals("0"))
                ++num;
            else
            {
                ++length;
                numArray1[length - 1] = targetArray[length + num - 1];
            }
        }
        var numArray2 = new byte[length];
        for (var index = 0; index < length; ++index)
            numArray2[index] = numArray1[index];
        return numArray2;
    }
}