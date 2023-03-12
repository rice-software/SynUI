using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace SynUI.Converters;

public class FullPathConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.IsNullOrWhiteSpace((string)value) ? "Untitled" : Path.GetFileName((string)value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}