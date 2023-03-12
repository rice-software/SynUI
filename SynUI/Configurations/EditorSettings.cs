using System;

namespace SynUI.Configurations;

[Serializable]
public sealed class EditorTabSettingsItem
{
    public string? FileName { get; set; }
    public string? Content { get; set; }
}