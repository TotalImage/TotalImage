using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TotalImage.FileSystems;
using TotalImage.ViewModels.Browsing;
using TotalImage.ViewModels.Formatting;
using TotalImage.ViewModels.Settings;
using TotalImage.ViewModels.Sorting;
using Directory = TotalImage.FileSystems.Directory;

namespace TotalImage.ViewModels.Main;

/// <summary>
/// The pane of the main window that has (or last had) the keyboard focus. The entry commands act
/// on what is selected in it.
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

// Browsing: the folder tree, the current folder and its entries, the selection, and sorting.
public sealed partial class MainViewModel
{
    /// <summary>
    /// The root of the folder tree of the loaded partition, or <c>null</c> when no image is open.
    /// Replaced when the partition's contents change.
    /// </summary>
    [ObservableProperty]
    public partial FolderViewModel? RootFolder { get; private set; }

    /// <summary>
    /// The folder selected in the tree, whose entries the file list shows.
    /// </summary>
    [ObservableProperty]
    public partial FolderViewModel? CurrentFolder { get; set; }

    /// <summary>
    /// The entries of the current folder, sorted, with the ".." entry first in a subfolder.
    /// Replaced as a whole when the folder, its contents or the sorting change.
    /// </summary>
    [ObservableProperty]
    public partial IReadOnlyList<EntryViewModel> Entries { get; private set; } = [];

    /// <summary>
    /// The entries selected in the file list. The ".." entry is only ever selected on its own.
    /// </summary>
    [ObservableProperty]
    public partial IReadOnlyList<EntryViewModel> SelectedEntries { get; private set; } = [];

    /// <summary>
    /// The pane the entry commands act on.
    /// </summary>
    [ObservableProperty]
    public partial BrowsePane ActivePane { get; set; }

    /// <summary>
    /// The selected entries without the ".." entry: what the entry commands act on.
    /// </summary>
    public IReadOnlyList<FileSystemObject> SelectedObjects { get; private set; } = [];

    /// <summary>
    /// Whether the ".." entry is selected (on its own).
    /// </summary>
    public bool IsParentEntrySelected => SelectedEntries is [{ IsParentEntry: true }];

    /// <summary>
    /// Selects entries of the file list. When the ".." entry is selected with other entries, it
    /// is left out.
    /// </summary>
    public void SelectEntries(IReadOnlyList<EntryViewModel> entries)
    {
        if (entries.Count > 1 && entries.Any(entry => entry.IsParentEntry))
            entries = entries.Where(entry => !entry.IsParentEntry).ToList();

        if (entries.SequenceEqual(SelectedEntries))
            return;

        SetSelection(entries);
    }

    /// <summary>
    /// Opens an entry of the file list if it is a folder (or the ".." entry): it becomes the
    /// current folder.
    /// </summary>
    /// <returns>Whether the entry is a folder; files are opened by the view.</returns>
    public bool TryOpenFolder(EntryViewModel entry)
    {
        if (entry.IsParentEntry)
        {
            GoToParentFolder();
            return true;
        }

        if (entry.Entry is not Directory folder || RootFolder is null)
            return false;

        var target = (Directory)folder.ResolveTarget();
        if ((RootFolder.Find(target.FullName) ?? RootFolder.Find(folder.FullName)) is { } found)
            CurrentFolder = found;

        return true;
    }

    [RelayCommand(CanExecute = nameof(CanUseImage))]
    private void SelectAll()
    {
        ActivePane = BrowsePane.Files;
        SelectEntries(Entries);
    }

    [RelayCommand(CanExecute = nameof(CanGoToParentFolder))]
    private void GoToParentFolder()
    {
        if (CurrentFolder?.Parent is { } parent)
            CurrentFolder = parent;
    }

    /// <summary>
    /// Expands every folder of the tree, reading the whole tree.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExpandOrCollapse))]
    private void ExpandFolderTree()
    {
        if (RootFolder is not null)
            Expand(RootFolder);

        static void Expand(FolderViewModel folder)
        {
            if (!folder.HasChildren)
                return;

            folder.IsExpanded = true;
            foreach (var child in folder.Children)
                Expand(child);
        }
    }

    /// <summary>
    /// Collapses every folder of the tree and goes to the root folder.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExpandOrCollapse))]
    private void CollapseFolderTree()
    {
        if (RootFolder is not { } root)
            return;

        foreach (var folder in root.LoadedDescendants.Reverse())
            folder.IsExpanded = false;
        root.IsExpanded = false;

        CurrentFolder = root;
    }

    private bool CanGoToParentFolder() => CanUseImage() && CurrentFolder?.Parent is not null;

    private bool CanExpandOrCollapse() => CanUseImage() && (RootFolder?.HasChildren ?? false);

    /// <summary>
    /// Reads the folder tree of the loaded partition again, staying in the current folder when it
    /// still exists (or going to the root folder).
    /// </summary>
    private void ReloadFolders()
    {
        var path = CurrentFolder?.FullName;

        if (CurrentFileSystem is not { } fileSystem || Image is not { } image)
        {
            RootFolder = null;
            CurrentFolder = null;
            return;
        }

        var root = new FolderViewModel(fileSystem.RootDirectory, new FolderTreeOptions(ShowHiddenItems, image.PendingChanges));
        RootFolder = root;
        CurrentFolder = (path is null ? null : root.Find(path)) ?? root;
    }

    partial void OnCurrentFolderChanged(FolderViewModel? value) => LoadEntries();

    partial void OnActivePaneChanged(BrowsePane value) => RefreshEntryCommands();

    partial void OnSortColumnChanged(int value) => SortEntries();

    partial void OnSortDirectionChanged(SortDirection value) => SortEntries();

    partial void OnShowHiddenItemsChanged(bool value) => ReloadFolders();

    partial void OnCurrentFileSystemChanged(FileSystem? value) => ReloadFolders();

    /// <summary>
    /// Creates the entries of the current folder.
    /// </summary>
    private void LoadEntries()
    {
        SetSelection([]);

        if (CurrentFolder is not { } folder || Image is not { } image)
        {
            Entries = [];
            RefreshEntryCommands();
            return;
        }

        var entries = CreateEntries(folder, image.PendingChanges);
        entries.Sort(EntryComparer.For(SortColumn, SortDirection));
        Entries = entries;
        UpdateSelectionStatus();
        RefreshEntryCommands();
    }

    private List<EntryViewModel> CreateEntries(FolderViewModel folder, Changes.ChangeSet changes)
    {
        var directory = (Directory)folder.Directory.ResolveTarget();
        var path = PendingItems.PathOf(directory);
        var unit = settings.Current.SizeUnit;
        var entries = new List<EntryViewModel>();

        if (folder.Parent is { } parent)
            entries.Add(EntryViewModel.ForParent(parent.Directory));

        foreach (var entry in directory.EnumerateFileSystemObjects(ShowHiddenItems))
        {
            // Measuring a folder reads everything in it. It stays on this thread: the image can't
            // be read from two threads at once, and the window keeps reading it while it is used.
            var sizeText = entry is not Directory subfolder ? unit.FormatSize(entry.Length)
                : settings.Current.FileListShowDirSize ? unit.FormatSize(subfolder.GetSize(true, false))
                : string.Empty;
            entries.Add(EntryViewModel.ForEntry(entry, fileTypes.GetTypeName(entry.Name, entry.Attributes), sizeText,
                PendingItems.GetState(changes, entry.Name, path)));
        }

        foreach (var addition in PendingItems.GetAdditions(changes, path))
        {
            // Skip names shown already, for example a file added again after saving.
            if (entries.Any(entry => !entry.IsParentEntry && PendingItems.NamesEqual(entry.Name, addition.Name)))
                continue;

            FileSystemObject pending = addition.IsDirectory
                ? new PendingDirectory(addition.Name, directory.FullName)
                : new PendingFile(addition.Name, addition.Length, addition.LastWriteTime!.Value);
            var attributes = addition.IsDirectory ? System.IO.FileAttributes.Directory : System.IO.FileAttributes.Normal;
            entries.Add(EntryViewModel.ForPendingEntry(pending, fileTypes.GetTypeName(addition.Name, attributes),
                addition.IsDirectory ? string.Empty : unit.FormatSize(addition.Length), addition.LastWriteTime ?? DateTime.Now));
        }

        return entries;
    }

    // Sorts the entries again, keeping the selection.
    private void SortEntries()
    {
        if (Entries.Count == 0)
            return;

        var entries = Entries.ToList();
        entries.Sort(EntryComparer.For(SortColumn, SortDirection));
        Entries = entries;
    }

    private void SetSelection(IReadOnlyList<EntryViewModel> entries)
    {
        SelectedObjects = entries.Where(entry => !entry.IsParentEntry).Select(entry => entry.Entry).ToList();
        SelectedEntries = entries;
        UpdateSelectionStatus();
        RefreshEntryCommands();
    }

    private void RefreshEntryCommands()
    {
        foreach (var command in EntryCommands)
            command.NotifyCanExecuteChanged();
    }
}
