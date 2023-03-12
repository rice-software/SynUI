using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace SynUI.Views.TabViews;

/// <summary>
///     Interaction logic for EditorTabView.xaml
/// </summary>
public partial class EditorTabView : UserControl
{
    public EditorTabView()
    {
        InitializeComponent();
    }

    private void TextEditor_OnLoad(object sender, RoutedEventArgs e)
    {
        var editor = (TextEditor)sender;
        using var reader = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream("SynUI.Resources.syntax-dark.xshd");
        using var xml = new XmlTextReader(reader!);
        var xshd = HighlightingLoader.LoadXshd(xml);

        editor.SyntaxHighlighting = HighlightingLoader.Load(xshd, HighlightingManager.Instance);
    }
}