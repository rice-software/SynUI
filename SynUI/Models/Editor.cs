using CommunityToolkit.Mvvm.ComponentModel;
using ICSharpCode.AvalonEdit.Document;

namespace SynUI.Models;

public class EditorItem : ObservableObject
{
    private string _name = "Untitled";
    private TextDocument _document = new("-- print(\"Hello World\")");

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public TextDocument Document
    {
        get => _document;
        set => SetProperty(ref _document, value);
    }
}