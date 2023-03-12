using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace SynUI.Views.TabViews
{
    /// <summary>
    /// Interaction logic for EditorTabView.xaml
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
            using var reader = Assembly.GetExecutingAssembly().GetManifestResourceStream("SynUI.Resources.syntax-dark.xshd");
            using var xml = new XmlTextReader(reader!);
            var xshd = HighlightingLoader.LoadXshd(xml);

            editor.SyntaxHighlighting = HighlightingLoader.Load(xshd, HighlightingManager.Instance);
        }
    }
}
