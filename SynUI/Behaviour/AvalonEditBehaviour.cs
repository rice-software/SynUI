using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ICSharpCode.AvalonEdit;
using Microsoft.Xaml.Behaviors;

namespace SynUI.Behaviour;

// code's source
// https://stackoverflow.com/questions/18964176/two-way-binding-to-avalonedit-document-text-using-mvvm
public sealed class AvalonEditBehaviour : Behavior<TextEditor>
{
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(string), typeof(AvalonEditBehaviour),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, PropertyChangedCallback));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        if (AssociatedObject != null)
            AssociatedObject.TextChanged += AssociatedObjectOnTextChanged;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        if (AssociatedObject != null)
            AssociatedObject.TextChanged -= AssociatedObjectOnTextChanged;
    }

    private void AssociatedObjectOnTextChanged(object sender, EventArgs eventArgs)
    {
        if (sender is not TextEditor textEditor) return;
        if (textEditor.Document != null)
            Text = textEditor.Document.Text;
    }

    private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
        var behavior = dependencyObject as AvalonEditBehaviour;
        var editor = behavior?.AssociatedObject;
        
        if (editor?.Document == null) return;
        
        var caretOffset = editor.CaretOffset;
        
        editor.Document.Text = dependencyPropertyChangedEventArgs.NewValue.ToString();
        editor.CaretOffset = editor.Document.TextLength < caretOffset ? editor.Document.TextLength : caretOffset;
    }
}