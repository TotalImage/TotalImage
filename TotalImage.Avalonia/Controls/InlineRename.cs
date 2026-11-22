using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace TotalImage.Avalonia.Controls;

/// <summary>
/// Edits a name in place: a text box below the item, which Enter (or clicking elsewhere) accepts
/// and Escape cancels, like label editing in Explorer.
/// </summary>
internal static class InlineRename
{
    /// <summary>
    /// Lets the user edit a name next to a control.
    /// </summary>
    /// <returns>The new name, or <c>null</c> when cancelled or unchanged.</returns>
    public static Task<string?> EditAsync(Control target, string name)
    {
        var result = new TaskCompletionSource<string?>();
        var cancelled = false;

        var textBox = new TextBox { Text = name, MinWidth = Math.Max(200, target.Bounds.Width / 2) };
        var flyout = new Flyout
        {
            Content = textBox,
            Placement = PlacementMode.BottomEdgeAlignedLeft,
            ShowMode = FlyoutShowMode.Standard
        };

        textBox.AddHandler(InputElement.KeyDownEvent, (_, e) =>
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                flyout.Hide();
            }
            else if (e.Key == Key.Escape)
            {
                e.Handled = true;
                cancelled = true;
                flyout.Hide();
            }
        }, RoutingStrategies.Tunnel);

        flyout.Opened += (_, _) =>
        {
            textBox.Focus();

            // Select the name without its extension, like Explorer.
            var dot = name.LastIndexOf('.');
            textBox.SelectionStart = 0;
            textBox.SelectionEnd = dot > 0 ? dot : name.Length;
        };
        flyout.Closed += (_, _) =>
        {
            var text = textBox.Text;
            result.TrySetResult(cancelled || string.IsNullOrEmpty(text) || text == name ? null : text);
            target.Focus();
        };

        flyout.ShowAt(target);
        return result.Task;
    }
}
