using System;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SynUI.Services;

namespace SynUI.ViewModels.TabViewModels;

public class SettingsTabViewModel : ViewModelBase, IEquatable<SettingsTabViewModel>
{
    private ViewModelBase? selectedSettingsView;

    public ViewModelBase? SelectedSettingsView
    {
        get => selectedSettingsView;
        set => SetProperty(ref selectedSettingsView, value);
    }

    public bool Equals(SettingsTabViewModel? other)
    {
        return other != null && other.GetType() == GetType();
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType();
    }

    public override int GetHashCode()
    {
        return 0;
    }
}