using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using SynUI.Models;

namespace SynUI.Converters;

public class OutputTypeToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            OutputInfo => Application.Current.TryFindResource("InformationOutputBrush"),
            OutputWarning => Application.Current.TryFindResource("WarningOutputBrush"),
            OutputError => Application.Current.TryFindResource("ErrorOutputBrush"),
            _ => Application.Current.TryFindResource("ThemeForegroundLowBrush")
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class OutputTypeToCodiconGlyphConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            OutputInfo => "\xea74",
            OutputWarning => "\xea6c",
            OutputError => "\xea87",
            _ => ""
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}