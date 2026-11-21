using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

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

    /// <inheritdoc/>
    public void RevealFile(string path)
    {
        if (OperatingSystem.IsWindows())
        {
            try
            {
                using var _ = Process.Start(new ProcessStartInfo
                {
                    FileName = "explorer.exe",
                    Arguments = $"/select,\"{path}\"",
                    UseShellExecute = true,
                    ErrorDialog = true
                });
            }
            catch (Win32Exception ex) when (ex.NativeErrorCode == ErrorCancelled)
            {
                // The user dismissed the error dialog.
            }
        }
        else if (Path.GetDirectoryName(path) is { Length: > 0 } folder)
        {
            Start(folder);
        }
    }

    private const int ErrorCancelled = 0x000004C7;

    private static void Start(string target)
    {
        using var _ = Process.Start(new ProcessStartInfo
        {
            FileName = target,
            UseShellExecute = true
        });
    }
}
