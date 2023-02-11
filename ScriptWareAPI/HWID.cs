using System;
using System.Management;
using System.Security.Principal;
using System.Text;

namespace ScriptWareAPI;

internal static class HwidGrabber
{
    public static string GetOwnerIdentity(bool returnDashes = false) => returnDashes
        ? WindowsIdentity.GetCurrent().Owner?.Value
        : WindowsIdentity.GetCurrent().Owner?.Value.Replace("-", "");

    public static string GetCpuid(string id = null) => GetWin32("Win32_Processor", id ?? "ProcessorId");
    public static string GetDiskId(string id = null) => GetWin32("Win32_DiskDrive", id ?? "SerialNumber");
    public static string GetUsbid(string id = null) => GetWin32("Win32_USBHub", id ?? "PNPDeviceID");
    public static string GetBaseboardId(string id = null) => GetWin32("Win32_BaseBoard", id ?? "SerialNumber");
    public static string GetMemoryId(string id = null) => GetWin32("Win32_PhysicalMemory", id ?? "PartNumber");
    public static string GetGpuid(string id = null) => GetWin32("Win32_VideoController", id ?? "PNPDeviceID");
    public static string GetMonitorId(string id = null) => GetWin32("Win32_DesktopMonitor", id ?? "PNPDeviceID");
    public static string GetWin32(string selection, string id)
    {
        using var managementObjectSearcher = new ManagementObjectSearcher($"SELECT * FROM {selection}");
        try
        {
            var enumerator = managementObjectSearcher.Get().GetEnumerator();
            return enumerator.MoveNext() ? enumerator.Current[id].ToString() : "0";
        }
        catch (ManagementException ex)
        {
            return ex.ErrorCode switch
            {
                ManagementStatus.NotFound => "ID Not Found",
                ManagementStatus.InvalidClass => "Invalid Class",
                _ => "0"
            };
        }
    }
    public static string GetHwid()
    {
        try
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(
                GetCpuid() +
                GetGpuid() +
                GetMemoryId() +
                GetBaseboardId()));
        }
        catch
        {
            try
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(
                    GetCpuid() + 
                    GetGpuid() +
                    GetOwnerIdentity() +
                    GetBaseboardId()));
            }
            catch
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(
                    GetGpuid() +
                    GetOwnerIdentity() +
                    GetBaseboardId()));
            }
        }
    }
    public static string GetOtherHwid()
    {
        try
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(
                GetOwnerIdentity() +
                GetMemoryId() +
                GetUsbid() +
                GetBaseboardId()));
        }
        catch
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(
                GetOwnerIdentity() +
                GetUsbid() + 
                GetBaseboardId()));
        }
    }
}