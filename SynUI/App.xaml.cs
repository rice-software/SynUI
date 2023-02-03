using System.Windows;
using SynUI.Properties;

namespace SynUI;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        Settings.Default.PropertyChanged += (_, _) =>
            Settings.Default.Save();
    }
}