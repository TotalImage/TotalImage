using System.Collections.Generic;
using TotalImage.FileSystems;

namespace TotalImage.ViewModels.Main;

// TEMPORARY (stage 5): the folder tree and the file list have no view models yet, so the view
// reports what they show and what is selected in them. MainViewModel uses it for the enabled
// rules of the entry commands and for the status bar. Removed when browsing moves into view models.

/// <summary>
/// The pane of the main window that has (or last had) the keyboard focus.
/// </summary>
public enum BrowsePane
{
    /// <summary>
    /// The file list.
    /// </summary>
    Files,

    /// <summary>
    /// The folder tree.
    /// </summary>
    Folders
}

/// <summary>
/// What is selected in the folder tree.
/// </summary>
public enum FolderSelection
{
    None,

    /// <summary>
    /// The root folder of the partition.
    /// </summary>
    Root,

    /// <summary>
    /// A folder below the root.
    /// </summary>
    Subfolder
}

/// <summary>
/// What the folder tree and the file list show and what is selected in them. Reported by the
/// view until browsing has its own view models (stage 5).
/// </summary>
/// <param name="FocusedPane">The pane that has, or last had, the focus.</param>
/// <param name="Folder">What is selected in the folder tree.</param>
/// <param name="CurrentDirectory">The folder selected in the tree, whose contents the file list shows.</param>
/// <param name="SelectedEntries">The entries selected in the file list, not counting the ".." entry.</param>
/// <param name="IsParentEntrySelected">The ".." entry (parent folder) is selected on its own.</param>
/// <param name="IsSelectedEntryPending">The one selected entry exists only as a pending change.</param>
/// <param name="HasSubfolders">The root folder has subfolders in the tree.</param>
public sealed record BrowseState(
    BrowsePane FocusedPane,
    FolderSelection Folder,
    Directory? CurrentDirectory,
    IReadOnlyList<FileSystemObject> SelectedEntries,
    bool IsParentEntrySelected,
    bool IsSelectedEntryPending,
    bool HasSubfolders)
{
    /// <summary>
    /// Nothing shown, nothing selected.
    /// </summary>
    public static BrowseState Empty { get; } = new(BrowsePane.Files, FolderSelection.None, null, [], false, false, false);
}
