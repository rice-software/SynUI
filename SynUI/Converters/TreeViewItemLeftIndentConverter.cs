using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace SynUI.Converters;

public class TreeViewItemLeftIndentConverter : IValueConverter
{
    public int Indent { get; set; } = 16;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not DependencyObject dependencyObject)
            return new Thickness(0, 0, 0, 0);

        var level = 0;
        var parent = VisualTreeHelper.GetParent(dependencyObject);

        while (parent is not TreeView && parent != null)
        {
            if (parent is TreeViewItem)
                level += 1;
            parent = VisualTreeHelper.GetParent(parent);
        }

        return new Thickness(Indent * level, 0, 0, 0);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}