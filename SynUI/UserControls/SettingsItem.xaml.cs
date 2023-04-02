using System.Windows;
using System.Windows.Controls;

namespace SynUI.UserControls;

public partial class SettingsItem : UserControl
{
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title), typeof(string), typeof(SettingsItem), new PropertyMetadata(default(string)));

    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        nameof(Description), typeof(string), typeof(SettingsItem), new PropertyMetadata(default(string)));

    public static readonly DependencyProperty PropertyProperty = DependencyProperty.Register(
        nameof(Property), typeof(string), typeof(SettingsItem), new PropertyMetadata(default(string)));

    public SettingsItem()
    {
        InitializeComponent();
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public string Property
    {
        get => (string)GetValue(PropertyProperty);
        set => SetValue(PropertyProperty, value);
    }
}