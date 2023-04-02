using CommunityToolkit.Mvvm.ComponentModel;

namespace SynUI.Services;

public interface ISettingsService
{ 
    bool? Test { get; set; }
}

public partial class SettingsService : ObservableObject, ISettingsService
{
    private bool? test;

    public bool? Test
    {
        get => test;
        set => SetProperty(ref test, value);
    }
}
