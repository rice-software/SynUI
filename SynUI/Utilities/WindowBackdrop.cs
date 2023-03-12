using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace SynUI.Utilities;

// https://tvc-16.science/mica-wpf.html
internal class WindowBackdrop
{
    public class ParameterTypes
    {
        /*
        [Flags]
        enum DWM_SYSTEMBACKDROP_TYPE
        {
            DWMSBT_MAINWINDOW = 2, // Mica
            DWMSBT_TRANSIENTWINDOW = 3, // Acrylic
            DWMSBT_TABBEDWINDOW = 4 // Tabbed
        }
        */

        [Flags]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum DWMWINDOWATTRIBUTE
        {
            DWMWA_USE_IMMERSIVE_DARK_MODE = 20,
            DWMWA_SYSTEMBACKDROP_TYPE = 38
        }

        [StructLayout(LayoutKind.Sequential)]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public struct MARGINS
        {
            public int cxLeftWidth; // width of left border that retains its size
            public int cxRightWidth; // width of right border that retains its size
            public int cyTopHeight; // height of top border that retains its size
            public int cyBottomHeight; // height of bottom border that retains its size
        }
    }

    public static class Methods
    {
        [DllImport("DwmApi.dll")]
        private static extern int DwmExtendFrameIntoClientArea(
            IntPtr hwnd,
            ref ParameterTypes.MARGINS pMarInset);

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, ParameterTypes.DWMWINDOWATTRIBUTE dwAttribute,
            ref int pvAttribute, int cbAttribute);

        public static int ExtendFrame(IntPtr hwnd, ParameterTypes.MARGINS margins)
        {
            return DwmExtendFrameIntoClientArea(hwnd, ref margins);
        }

        public static int SetWindowAttribute(IntPtr hwnd, ParameterTypes.DWMWINDOWATTRIBUTE attribute, int parameter)
        {
            return DwmSetWindowAttribute(hwnd, attribute, ref parameter, Marshal.SizeOf<int>());
        }
    }
}