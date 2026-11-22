using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using TotalImage.Changes;
using Directory = TotalImage.FileSystems.Directory;

namespace TotalImage.ViewModels.Browsing;

/// <summary>
/// What every folder of a tree needs to know to list its subfolders.
/// </summary>
/// <param name="ShowHidden">Whether hidden folders are listed.</param>
/// <param name="Changes">The pending changes of the image, which add, delete and rename folders.</param>
internal sealed record FolderTreeOptions(bool ShowHidden, ChangeSet? Changes);

/// <summary>
/// A folder in the folder tree. Its subfolders are read when they are first asked for.
/// </summary>
public sealed partial class FolderViewModel : ObservableObject
{
    private readonly FolderTreeOptions options;
    private IReadOnlyList<FolderViewModel>? children;

    /// <summary>
    /// Creates the root folder of a tree.
    /// </summary>
    internal FolderViewModel(Directory root, FolderTreeOptions options) : this(root, null, PendingState.None, options)
    {
    }

    private FolderViewModel(Directory directory, FolderViewModel? parent, PendingState pendingState, FolderTreeOptions options)
    {
        Directory = directory;
        Parent = parent;
        PendingState = pendingState;
        this.options = options;
        Name = parent is null ? Path.DirectorySeparatorChar.ToString() : directory.Name;
        IsHidden = directory.Attributes.HasFlag(FileAttributes.Hidden);
    }

    /// <summary>
    /// The folder, or a <see cref="PendingDirectory"/> for one that only exists as a pending change.
    /// </summary>
    public Directory Directory { get; }

    /// <summary>
    /// The folder above, or <c>null</c> for the root folder.
    /// </summary>
    public FolderViewModel? Parent { get; }

    public bool IsRoot => Parent is null;

    /// <summary>
    /// The name shown in the tree: the folder's name, or "\" for the root folder.
    /// </summary>
    public string Name { get; }

    public string FullName => Directory.FullName;

    public bool IsHidden { get; }

    /// <summary>
    /// Whether the folder only exists as a pending change.
    /// </summary>
    public bool IsPending => Directory is PendingDirectory;

    public PendingState PendingState { get; }

    public IconKey IconKey => IconKey.Folder;

    /// <summary>
    /// Whether the folder's subfolders are shown.
    /// </summary>
    [ObservableProperty]
    public partial bool IsExpanded { get; set; }

    /// <summary>
    /// Whether <see cref="Children"/> has been read already.
    /// </summary>
    public bool AreChildrenLoaded => children is not null;

    /// <summary>
    /// The subfolders, sorted by name, including folders that pending changes create. Read on first use.
    /// </summary>
    public IReadOnlyList<FolderViewModel> Children => children ??= LoadChildren();

    public bool HasChildren => Children.Count > 0;

    /// <summary>
    /// The folders below this one that have been read so far, at any depth.
    /// </summary>
    public IEnumerable<FolderViewModel> LoadedDescendants
        => children is null ? [] : children.SelectMany(child => child.LoadedDescendants.Prepend(child));

    /// <summary>
    /// Finds a folder below this one by its full name, reading subfolders as needed.
    /// </summary>
    /// <returns>The folder, or <c>null</c> when it isn't in the tree.</returns>
    public FolderViewModel? Find(string fullName)
    {
        var path = fullName.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
        var current = this;
        foreach (var name in path)
        {
            current = current.Children.FirstOrDefault(child => child.Directory.Name == name);
            if (current is null)
                return null;
        }

        return current;
    }

    private List<FolderViewModel> LoadChildren()
    {
        var directory = (Directory)Directory.ResolveTarget();
        var path = PendingItems.PathOf(directory);

        var list = directory.EnumerateDirectories(options.ShowHidden)
            .Select(subfolder => new FolderViewModel(subfolder, this, PendingItems.GetState(options.Changes, subfolder.Name, path), options))
            .ToList();

        foreach (var addition in PendingItems.GetAdditions(options.Changes, path, directoriesOnly: true))
        {
            // A folder that exists already (for example one created again after saving) is shown once.
            if (list.Any(folder => PendingItems.NamesEqual(folder.Directory.Name, addition.Name)))
                continue;

            list.Add(new FolderViewModel(new PendingDirectory(addition.Name, directory.FullName), this, PendingState.Added, options));
        }

        list.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.CurrentCulture));
        return list;
    }
}
