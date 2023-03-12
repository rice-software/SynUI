using System;
using System.Globalization;
using System.Windows.Data;
using sxlib.Specialized;

namespace SynUI.Converters;

public class SynapseLoadStateConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (SxLibBase.SynLoadEvents)value switch
        {
            SxLibBase.SynLoadEvents.CHECKING_DATA => "Checking data",
            SxLibBase.SynLoadEvents.CHECKING_WL => "Checking whitelist",
            SxLibBase.SynLoadEvents.DOWNLOADING_DATA or SxLibBase.SynLoadEvents.DOWNLOADING_DLLS => "Updating",
            SxLibBase.SynLoadEvents.FAILED_TO_DOWNLOAD => "Failed to download, are you connected to the internet?",
            SxLibBase.SynLoadEvents.CHANGING_WL => "Changing whitelist",
            SxLibBase.SynLoadEvents.FAILED_TO_VERIFY => "Failed to verify Synapse X's dependencies, please redownload",
            SxLibBase.SynLoadEvents.NOT_LOGGED_IN => "You're not logged in, please open Synapse X and login",
            SxLibBase.SynLoadEvents.NOT_UPDATED => "Synapse X isn't updated",
            SxLibBase.SynLoadEvents.READY => "Ready",
            SxLibBase.SynLoadEvents.UNAUTHORIZED_HWID => "HWID isn't authorized, please wait 24h before switching PC",
            SxLibBase.SynLoadEvents.UNKNOWN => "Unknown error",

            _ => value.ToString()
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class SynapseAttachStateConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (SxLibBase.SynAttachEvents)value switch
        {
            // Generic states
            SxLibBase.SynAttachEvents.ALREADY_INJECTED or SxLibBase.SynAttachEvents.READY => "Injected",
            SxLibBase.SynAttachEvents.INJECTING or SxLibBase.SynAttachEvents.REINJECTING => "Injecting",
            SxLibBase.SynAttachEvents.NOT_INJECTED or SxLibBase.SynAttachEvents.PROC_CREATION
                or SxLibBase.SynAttachEvents.PROC_DELETION => "Not injected",

            // Injecting
            SxLibBase.SynAttachEvents.CHECKING_WHITELIST => "Injecting (Checking whitelist)",
            SxLibBase.SynAttachEvents.SCANNING => "Injecting (Scanning)",
            SxLibBase.SynAttachEvents.UPDATING_DLLS or SxLibBase.SynAttachEvents.NOT_RUNNING_LATEST_VER_UPDATING =>
                "Injecting (Updating)",

            // Not injected
            SxLibBase.SynAttachEvents.FAILED_TO_ATTACH => "Not injected (Failed to attach to Roblox)",
            SxLibBase.SynAttachEvents.FAILED_TO_FIND => "Not injected (Please open Roblox)",
            SxLibBase.SynAttachEvents.FAILED_TO_UPDATE => "Not injected (Failed to update Synapse X)",
            SxLibBase.SynAttachEvents.NOT_UPDATED => "Not injected (Please update Synapse X)",
            SxLibBase.SynAttachEvents.CHECKING => "Not injected (Checking data)",

            _ => value.ToString()
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}