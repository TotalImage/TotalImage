using System.Diagnostics;

namespace TotalImage.ViewModels.Services;

/// <summary>
/// Opens URLs and folders through the shell. <see cref="ProcessStartInfo.UseShellExecute"/>
/// works on Windows, Linux (xdg-open) and macOS (open).
/// </summary>
public sealed class ProcessLauncherService : ILauncherService
{
    /// <inheritdoc/>
    public void OpenUrl(string url) => Start(url);

    /// <inheritdoc/>
    public void OpenFolder(string path) => Start(path);

    private static void Start(string target)
    {
        using var _ = Process.Start(new ProcessStartInfo
        {
            FileName = target,
            UseShellExecute = true
        });
    }
}
