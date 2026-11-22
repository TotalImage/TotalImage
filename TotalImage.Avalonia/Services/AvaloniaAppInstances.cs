using TotalImage.ViewModels.Services;

namespace TotalImage.Avalonia.Services;

/// <summary>
/// Other instances share the temporary folder: other processes of the app, and the other main
/// windows of this one.
/// </summary>
public sealed class AvaloniaAppInstances(MainWindows windows) : IAppInstances
{
    private readonly ProcessAppInstances processes = new();

    /// <inheritdoc/>
    public bool AreOtherInstancesRunning() => windows.Count > 1 || processes.AreOtherInstancesRunning();
}
