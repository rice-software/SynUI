namespace SynUI.ViewModels.TabViewModels;

public class WelcomeTabViewModel : ViewModelBase
{
    public WelcomeTabViewModel(EditorViewModel editorViewModel)
    {
        EditorViewModel = editorViewModel;
    }

    public EditorViewModel? EditorViewModel { get; }
}