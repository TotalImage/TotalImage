using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;

namespace TotalImage.Avalonia.Dialogs;

/// <summary>
/// Wiring shared by the dialog windows.
/// </summary>
public static class DialogWindowExtensions
{
    /// <summary>
    /// Makes a button accept the dialog: it runs the view model's accept command and closes the
    /// window with <c>true</c>. Bind the button's <c>Command</c> to the same command in XAML so
    /// it is disabled while the input isn't valid; the click is handled here, because a
    /// button raises Click before it runs its command.
    /// </summary>
    public static void BindAccept(this Window window, Button button, IRelayCommand accept)
    {
        button.Click += (_, e) =>
        {
            e.Handled = true;
            if (!accept.CanExecute(null))
                return;

            accept.Execute(null);
            window.Close(true);
        };
    }

    /// <summary>
    /// Makes a button close the dialog without accepting it.
    /// </summary>
    public static void BindCancel(this Window window, Button button)
        => button.Click += (_, e) =>
        {
            e.Handled = true;
            window.Close(false);
        };

    /// <summary>
    /// Makes a button close a dialog that only shows information.
    /// </summary>
    public static void BindClose(this Window window, Button button)
        => button.Click += (_, e) =>
        {
            e.Handled = true;
            window.Close();
        };
}
