using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Xaml;
using System.Xml;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace SynUI.Views;

/// <summary>
///     Interaction logic for EditorView.xaml
/// </summary>
public partial class EditorView : UserControl
{
    public EditorView()
    {
        InitializeComponent();
    }
}