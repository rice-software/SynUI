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
            OutputType.Info => Application.Current.TryFindResource("InformationOutputBrush"),
            OutputType.Warning => Application.Current.TryFindResource("WarningOutputBrush"),
            OutputType.Error => Application.Current.TryFindResource("ErrorOutputBrush"),
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
            OutputType.Info => "\xea74",
            OutputType.Warning => "\xea6c",
            OutputType.Error => "\xea87",
            _ => ""
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}