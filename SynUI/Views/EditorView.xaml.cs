using System.Windows.Controls;
using System.Windows.Input;
using SynUI.Models;
using SynUI.ViewModels;

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

    // private void Explorer_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    // {
    //     if (((TreeView)sender).SelectedItem is ExplorerFile)
    //         EditorControl.Focus();
    // }

    // i hate this but i have no choice
    private void Explorer_TreeViewItem_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        var evm = (EditorViewModel)DataContext;
        var tv = (TreeViewItem)sender;

        if (tv.DataContext is ExplorerFile)
            evm.SelectedExplorerNodeOnDoubleClickCommand.Execute(tv.DataContext);
    }
}