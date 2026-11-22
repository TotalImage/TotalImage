using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using TotalImage.ViewModels.Services;

namespace TotalImage.Avalonia.Services;

/// <summary>
/// Shows dialog windows on top of their owner.
/// </summary>
public static class DialogHost
{
    /// <summary>
    /// Shows a dialog modally on top of <paramref name="owner"/> (or the active window), and waits
    /// until it closes.
    /// </summary>
    /// <returns>Whether the dialog was accepted: closed with <c>Close(true)</c>.</returns>
    public static async Task<bool> ShowDialogAsync(Window dialog, Window? owner)
    {
        owner ??= ActiveWindow;
        if (owner is not null && owner != dialog)
            return await dialog.ShowDialog<bool?>(owner) == true;

        // No window to be modal to: show it on its own and wait until it closes.
        var closed = new TaskCompletionSource<bool>();
        dialog.Closed += (_, _) => closed.TrySetResult(true);
        dialog.Show();
        await closed.Task;
        return false;
    }

    /// <summary>
    /// The pickers and messages of a dialog, owned by the dialog's window once it is set on
    /// <paramref name="dialogOwner"/>. For dialog view models that ask further questions.
    /// </summary>
    public static DialogServices ServicesFor(WindowOwner dialogOwner)
        => new(new AvaloniaMessageService(dialogOwner), new AvaloniaFilePickerService(dialogOwner));

    /// <summary>
    /// The window to show pickers on: the given one or, without one, the active window.
    /// </summary>
    public static TopLevel? TopLevelFor(Window? window) => window ?? ActiveWindow;

    private static Window? ActiveWindow
        => Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.Windows.FirstOrDefault(window => window.IsActive) ?? desktop.Windows.FirstOrDefault()
            : null;
}
