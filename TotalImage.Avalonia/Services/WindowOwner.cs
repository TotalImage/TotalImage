using Avalonia.Controls;

namespace TotalImage.Avalonia.Services;

/// <summary>
/// The window that owns the dialogs, pickers and messages shown in the current scope. The main
/// window sets itself here when it is created; a dialog that asks further questions gets its own
/// owner (see <see cref="DialogHost.ServicesFor"/>).
/// </summary>
public sealed class WindowOwner
{
    public Window? Window { get; set; }
}
