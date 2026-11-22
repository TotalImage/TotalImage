using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media;
using TotalImage.Avalonia.Services;

namespace TotalImage.Avalonia.Dialogs;

/// <summary>
/// The kind of a message, shown by its icon.
/// </summary>
public enum MessageIcon
{
    Information,
    Warning,
    Error
}

/// <summary>
/// A button of a message window.
/// </summary>
/// <param name="Text">The button text; an underscore marks the access key.</param>
/// <param name="IsDefault">Whether Enter clicks it.</param>
/// <param name="IsCancel">Whether Escape (and closing the window) clicks it.</param>
public sealed record MessageButton(string Text, bool IsDefault = false, bool IsCancel = false)
{
    public static MessageButton Ok { get; } = new("OK", IsDefault: true, IsCancel: true);
}

/// <summary>
/// What a message window shows.
/// </summary>
/// <param name="Heading">The main instruction, in larger text.</param>
/// <param name="Text">The explanation below it.</param>
/// <param name="Caption">The window title.</param>
/// <param name="Icon">The kind of message.</param>
/// <param name="Buttons">The buttons, left to right.</param>
/// <param name="CheckBoxText">The text of a check box below the message (for example "Do not ask again"), or <c>null</c> for none.</param>
public sealed record MessageOptions(
    string Heading,
    string Text,
    string Caption,
    MessageIcon Icon,
    IReadOnlyList<MessageButton> Buttons,
    string? CheckBoxText = null);

/// <summary>
/// The answer of a message window.
/// </summary>
/// <param name="Button">The button clicked, or the cancel button (or <c>null</c> when there is
/// none) when the window was closed otherwise.</param>
/// <param name="IsChecked">Whether the check box was checked.</param>
public sealed record MessageResult(MessageButton? Button, bool IsChecked);

/// <summary>
/// A small reusable message or question window: heading, text, icon, buttons and an optional
/// check box. <see cref="AvaloniaMessageService"/> builds every message from it.
/// </summary>
public partial class MessageWindow : Window
{
    private MessageButton? clicked;
    private readonly MessageOptions? options;

    public MessageWindow()
    {
        InitializeComponent();
    }

    public MessageWindow(MessageOptions options) : this()
    {
        this.options = options;
        Title = options.Caption;
        headingText.Text = options.Heading;
        messageText.Text = options.Text;
        messageText.IsVisible = options.Text.Length > 0;

        (iconText.Text, iconBorder.Background) = options.Icon switch
        {
            MessageIcon.Error => ("✕", new SolidColorBrush(Color.FromRgb(0xC4, 0x2B, 0x1C))),
            MessageIcon.Warning => ("!", new SolidColorBrush(Color.FromRgb(0xD8, 0x8C, 0x00))),
            _ => ("i", (IBrush)new SolidColorBrush(Color.FromRgb(0x00, 0x67, 0xC0)))
        };

        if (options.CheckBoxText is { } checkBoxText)
        {
            verificationCheckBox.Content = checkBoxText;
            verificationCheckBox.IsVisible = true;
        }

        foreach (var option in options.Buttons)
        {
            var button = new Button
            {
                Content = option.Text,
                IsDefault = option.IsDefault,
                IsCancel = option.IsCancel,
                MinWidth = 80,
                HorizontalContentAlignment = global::Avalonia.Layout.HorizontalAlignment.Center
            };
            if (option.IsDefault)
                button.Classes.Add("accent");
            button.Click += (_, _) =>
            {
                clicked = option;
                Close();
            };
            buttonPanel.Children.Add(button);
        }

        Opened += (_, _) =>
        {
            // Focus the default button, so Enter and Space answer with it.
            foreach (var child in buttonPanel.Children)
            {
                if (child is Button { IsDefault: true } button)
                    button.Focus();
            }
        };
    }

    /// <summary>
    /// The answer, once the window has closed.
    /// </summary>
    public MessageResult Result => new(clicked ?? CancelButton, verificationCheckBox.IsChecked == true);

    private MessageButton? CancelButton
    {
        get
        {
            if (options is null)
                return null;

            foreach (var button in options.Buttons)
            {
                if (button.IsCancel)
                    return button;
            }

            return null;
        }
    }

    /// <summary>
    /// Shows a message on top of a window (or the active window) and waits for the answer.
    /// </summary>
    public static async Task<MessageResult> ShowAsync(Window? owner, MessageOptions options)
    {
        var window = new MessageWindow(options);
        await DialogHost.ShowDialogAsync(window, owner);
        return window.Result;
    }
}
