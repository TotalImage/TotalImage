using System;
using System.Collections.Generic;
using System.IO;
using TotalImage.Changes;
using Directory = TotalImage.FileSystems.Directory;

namespace TotalImage.ViewModels.Browsing;

/// <summary>
/// How the pending (not yet saved) changes affect an entry.
/// </summary>
public enum PendingState
{
    None,

    /// <summary>
    /// The entry is added (or added again) by a pending change.
    /// </summary>
    Added,

    /// <summary>
    /// The entry is deleted by a pending change.
    /// </summary>
    Deleted,

    /// <summary>
    /// The entry is renamed by a pending change.
    /// </summary>
    Renamed
}

/// <summary>
/// An entry that only exists as a pending addition in a folder.
/// </summary>
/// <param name="Name">The name of the entry.</param>
/// <param name="IsDirectory">Whether a folder is created, rather than a file added.</param>
/// <param name="Length">The size of an added file.</param>
/// <param name="LastWriteTime">The modification time of an added file.</param>
internal sealed record PendingAddition(string Name, bool IsDirectory, ulong Length, DateTime? LastWriteTime);

/// <summary>
/// Reads what the pending changes do to the entries of a folder. Paths are arrays of names, as in
/// <see cref="PendingChange"/>; the root folder is an empty array.
/// </summary>
internal static class PendingItems
{
    /// <summary>
    /// The path of a folder. A <see cref="PendingDirectory"/> has no parent chain, so its full name is split instead.
    /// </summary>
    public static string[] PathOf(Directory directory)
    {
        if (directory is PendingDirectory)
            return directory.FullName.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);

        var parts = new List<string>();
        for (var current = directory; current.Parent is not null; current = current.Parent)
            parts.Add(current.Name);

        parts.Reverse();
        return [.. parts];
    }

    /// <summary>
    /// The state of an entry in a folder. The last change that names it wins, so an entry that is
    /// renamed and then deleted is <see cref="PendingState.Deleted"/>.
    /// </summary>
    public static PendingState GetState(ChangeSet? changes, string name, string[] folderPath)
    {
        var state = PendingState.None;
        if (changes is null)
            return state;

        foreach (var change in changes.Changes)
        {
            var (path, changeState) = change switch
            {
                AddFileChange add => (add.DestinationPath, PendingState.Added),
                CreateDirectoryChange create => (create.Path, PendingState.Added),
                DeleteEntryChange delete => (delete.Path, PendingState.Deleted),
                RenameChange rename => (rename.OldPath, PendingState.Renamed),
                _ => (null, PendingState.None)
            };

            if (path is { Length: > 0 } && IsIn(path, folderPath) && NamesEqual(path[^1], name))
                state = changeState;
        }

        return state;
    }

    /// <summary>
    /// The files and folders that pending changes add to a folder, in the order they were added.
    /// </summary>
    public static IEnumerable<PendingAddition> GetAdditions(ChangeSet? changes, string[] folderPath, bool directoriesOnly = false)
    {
        if (changes is null)
            yield break;

        foreach (var change in changes.Changes)
        {
            if (change is AddFileChange add && !directoriesOnly && add.DestinationPath.Length > 0 && IsIn(add.DestinationPath, folderPath))
                yield return new(add.DestinationPath[^1], false, (ulong)add.Source.Length, add.LastWriteTime);
            else if (change is CreateDirectoryChange create && create.Path.Length > 0 && IsIn(create.Path, folderPath))
                yield return new(create.Path[^1], true, 0, null);
        }
    }

    public static bool NamesEqual(string a, string b) => string.Equals(a, b, StringComparison.OrdinalIgnoreCase);

    // Whether the entry at the path is directly inside the folder.
    private static bool IsIn(string[] entryPath, string[] folderPath)
    {
        if (entryPath.Length != folderPath.Length + 1)
            return false;

        for (var i = 0; i < folderPath.Length; i++)
        {
            if (!NamesEqual(entryPath[i], folderPath[i]))
                return false;
        }

        return true;
    }
}
