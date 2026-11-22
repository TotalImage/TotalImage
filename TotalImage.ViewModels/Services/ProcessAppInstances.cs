using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TotalImage.ViewModels.Services;

/// <summary>
/// Finds other instances by looking for processes of the application. The WinForms and the
/// Avalonia application share the temporary folder, so processes of either count.
/// </summary>
public sealed class ProcessAppInstances : IAppInstances
{
    /// <summary>
    /// The process names of the applications that share the temporary folder.
    /// </summary>
    public static IReadOnlyList<string> ProcessNames { get; } = ["TotalImage", "TotalImage.Avalonia"];

    private readonly Func<string, IReadOnlyList<int>> findProcessIds;
    private readonly int currentProcessId;

    public ProcessAppInstances() : this(FindProcessIds, Environment.ProcessId)
    {
    }

    /// <param name="findProcessIds">Gives the IDs of the running processes with a given name.</param>
    /// <param name="currentProcessId">The ID of this process, which doesn't count.</param>
    internal ProcessAppInstances(Func<string, IReadOnlyList<int>> findProcessIds, int currentProcessId)
    {
        this.findProcessIds = findProcessIds;
        this.currentProcessId = currentProcessId;
    }

    /// <inheritdoc/>
    public bool AreOtherInstancesRunning()
        => ProcessNames.Any(name => findProcessIds(name).Any(id => id != currentProcessId));

    private static IReadOnlyList<int> FindProcessIds(string name)
    {
        var processes = Process.GetProcessesByName(name);
        var ids = new int[processes.Length];
        for (var i = 0; i < processes.Length; i++)
        {
            ids[i] = processes[i].Id;
            processes[i].Dispose();
        }

        return ids;
    }
}
