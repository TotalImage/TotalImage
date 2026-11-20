using System.Diagnostics;

namespace TotalImage.ViewModels.Services;

/// <summary>
/// Finds other instances by looking for processes with the same name as this one.
/// </summary>
public sealed class ProcessAppInstances : IAppInstances
{
    /// <inheritdoc/>
    public bool AreOtherInstancesRunning()
    {
        using var current = Process.GetCurrentProcess();
        var processes = Process.GetProcessesByName(current.ProcessName);
        foreach (var process in processes)
            process.Dispose();

        return processes.Length > 1;
    }
}
