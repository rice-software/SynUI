using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.Document;

namespace SynUI.Models;

public class EditorItem
{
    public string Name { get; set; } = "Untitled";
    public TextDocument Document { get; set; } = new("-- print(\"Hello World\")");
}