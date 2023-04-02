using System;
using SynUI.Services;

namespace SynUI.ViewModels.TabViewModels;

public class SettingsTabViewModel : ViewModelBase, IEquatable<SettingsTabViewModel>
{
    public SettingsTabViewModel(ISettingsService settingsService)
    {
        SettingsService = settingsService;
    }
    
    public ISettingsService SettingsService { get; }

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