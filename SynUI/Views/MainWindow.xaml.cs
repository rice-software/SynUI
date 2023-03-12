using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using SynUI.Utilities;
using static SynUI.Utilities.WindowBackdrop.ParameterTypes;

namespace SynUI.Views;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_OnLoaded;
    }

    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        RefreshFrame();
        RefreshDarkMode();
        RefreshBackdropMode(2);
        // ThemeManager.Current.ActualApplicationThemeChanged += (_, _) => RefreshDarkMode();
    }

    private void RefreshFrame()
    {
        var mainWindowPtr = new WindowInteropHelper(this).Handle;
        var mainWindowSrc = HwndSource.FromHwnd(mainWindowPtr);

        if (mainWindowSrc?.CompositionTarget == null) return;
        mainWindowSrc.CompositionTarget.BackgroundColor = Color.FromArgb(0, 0, 0, 0);

        var margins = new MARGINS
        {
            cxLeftWidth = -1,
            cxRightWidth = -1,
            cyTopHeight = -1,
            cyBottomHeight = -1
        };

        WindowBackdrop.Methods.ExtendFrame(mainWindowSrc.Handle, margins);
    }

    private void RefreshDarkMode()
    {
        // always dark
        // var isDark = ThemeManager.Current.ActualApplicationTheme == ApplicationTheme.Dark;
        // var flag = isDark ? 1 : 0;

        WindowBackdrop.Methods.SetWindowAttribute(
            new WindowInteropHelper(this).Handle,
            DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE,
            1);
    }

    private void RefreshBackdropMode(int flag)
    {
        WindowBackdrop.Methods.SetWindowAttribute(
            new WindowInteropHelper(this).Handle,
            DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
            flag);
    }
}