using ICSharpCode.AvalonEdit.Document;

namespace SynUI.Models;

public class EditorItem
{
    public string Name { get; set; } = "Untitled";
    public TextDocument Document { get; set; } = new("-- print(\"Hello World\")");
}