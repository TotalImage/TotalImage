using System;
using System.IO;
using System.Text;
using TotalImage.FileSystems;
using Directory = TotalImage.FileSystems.Directory;

namespace TotalImage.ViewModels.Browsing;

/// <summary>
/// A row of the file list: an entry of the current folder, an entry that only exists as a
/// pending change, or the ".." entry that goes to the parent folder.
/// </summary>
public sealed class EntryViewModel
{
    /// <summary>
    /// The name of the entry that goes to the parent folder.
    /// </summary>
    public const string ParentEntryName = "..";

    private EntryViewModel(FileSystemObject entry, string name, string typeName, string sizeText, string modifiedText,
        string attributesText, IconKey iconKey, bool isHidden, bool isPending, bool isParentEntry, PendingState pendingState)
    {
        Entry = entry;
        Name = name;
        TypeName = typeName;
        SizeText = sizeText;
        ModifiedText = modifiedText;
        AttributesText = attributesText;
        IconKey = iconKey;
        IsHidden = isHidden;
        IsPending = isPending;
        IsParentEntry = isParentEntry;
        PendingState = pendingState;
    }

    /// <summary>
    /// An entry of a folder, as it is on the disk.
    /// </summary>
    internal static EntryViewModel ForEntry(FileSystemObject entry, string typeName, string sizeText, PendingState pendingState)
        => new(entry, entry.Name, typeName, sizeText, entry.LastWriteTime?.ToString() ?? string.Empty, FormatAttributes(entry.Attributes),
            IconKey.ForEntry(entry.Name, entry.Attributes), entry.Attributes.HasFlag(FileAttributes.Hidden), false, false, pendingState);

    /// <summary>
    /// An entry that only exists as a pending addition.
    /// </summary>
    /// <param name="entry">A <see cref="PendingFile"/> or a <see cref="PendingDirectory"/>.</param>
    /// <param name="typeName">The Type column.</param>
    /// <param name="sizeText">The Size column.</param>
    /// <param name="modified">The modification time shown.</param>
    internal static EntryViewModel ForPendingEntry(FileSystemObject entry, string typeName, string sizeText, DateTime modified)
    {
        var isDirectory = entry is Directory;
        return new(entry, entry.Name, typeName, sizeText, modified.ToString(), isDirectory ? "D----" : "-A---",
            isDirectory ? IconKey.Folder : IconKey.ForFile(entry.Name), false, true, false, PendingState.Added);
    }

    /// <summary>
    /// The ".." entry.
    /// </summary>
    /// <param name="parent">The parent folder.</param>
    internal static EntryViewModel ForParent(Directory parent)
        => new(parent, ParentEntryName, string.Empty, string.Empty, string.Empty, string.Empty, IconKey.ParentFolder, false, false, true, PendingState.None);

    /// <summary>
    /// The entry; for the ".." entry, the parent folder.
    /// </summary>
    public FileSystemObject Entry { get; }

    public string Name { get; }

    /// <summary>
    /// The Type column.
    /// </summary>
    public string TypeName { get; }

    /// <summary>
    /// The Size column. Empty for folders, unless the settings ask for folder sizes.
    /// </summary>
    public string SizeText { get; }

    /// <summary>
    /// The Modified column.
    /// </summary>
    public string ModifiedText { get; }

    /// <summary>
    /// The Attributes column: D, A, S, H, R and L (reparse point), or "-" for each one not set.
    /// </summary>
    public string AttributesText { get; }

    public IconKey IconKey { get; }

    public bool IsHidden { get; }

    public bool IsDirectory => !IsParentEntry && Entry is Directory;

    /// <summary>
    /// Whether the entry only exists as a pending addition, so it can't be opened or changed yet.
    /// </summary>
    public bool IsPending { get; }

    /// <summary>
    /// Whether this is the ".." entry.
    /// </summary>
    public bool IsParentEntry { get; }

    public PendingState PendingState { get; }

    public string ToolTip
    {
        get
        {
            if (IsParentEntry)
                return "Parent directory";

            var text = $"Type: {TypeName}{Environment.NewLine}Size: {SizeText}";
            if (!string.IsNullOrWhiteSpace(ModifiedText))
                text += $"{Environment.NewLine}Modified: {ModifiedText}";
            return text;
        }
    }

    internal static string FormatAttributes(FileAttributes attributes)
    {
        var text = new StringBuilder(6);
        text.Append(attributes.HasFlag(FileAttributes.Directory) ? 'D' : '-');
        text.Append(attributes.HasFlag(FileAttributes.Archive) ? 'A' : '-');
        text.Append(attributes.HasFlag(FileAttributes.System) ? 'S' : '-');
        text.Append(attributes.HasFlag(FileAttributes.Hidden) ? 'H' : '-');
        text.Append(attributes.HasFlag(FileAttributes.ReadOnly) ? 'R' : '-');
        text.Append(attributes.HasFlag(FileAttributes.ReparsePoint) ? 'L' : '-');
        return text.ToString();
    }
}
