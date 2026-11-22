using System;
using System.Collections.Generic;
using TotalImage.ViewModels.Browsing;
using TotalImage.ViewModels.Settings;
using Directory = TotalImage.FileSystems.Directory;

namespace TotalImage.ViewModels.Sorting;

/// <summary>
/// The columns of the file list, in the order of <see cref="Main.MainViewModel.SortColumn"/>.
/// </summary>
public enum EntryColumn
{
    Name = 0,
    Type,
    Size,
    Modified,
    Attributes
}

/// <summary>
/// Sorts the entries of the file list by a column. Folders come first (last when descending),
/// and the ".." entry always comes first.
/// </summary>
public sealed class EntryComparer : IComparer<EntryViewModel>
{
    private readonly IComparer<EntryViewModel> columnComparer;

    private EntryComparer(IComparer<EntryViewModel> columnComparer) => this.columnComparer = columnComparer;

    /// <summary>
    /// Gets the comparer for a column (one that doesn't exist sorts by name) and direction
    /// (<see cref="SortDirection.None"/> sorts ascending).
    /// </summary>
    public static EntryComparer For(int column, SortDirection direction)
    {
        IComparer<EntryViewModel> comparer = (EntryColumn)column switch
        {
            EntryColumn.Type => ByText(entry => entry.TypeName),
            EntryColumn.Size => ByEntry(FileSystemObjectComparer.LengthDirectoriesFirst),
            EntryColumn.Modified => ByEntry(FileSystemObjectComparer.LastWriteDateDirectoriesFirst),
            EntryColumn.Attributes => ByText(entry => entry.AttributesText),
            _ => ByEntry(FileSystemObjectComparer.NameDirectoriesFirst)
        };

        if (direction == SortDirection.Descending)
            comparer = new InvertedComparer<EntryViewModel>(comparer);

        return new(comparer);
    }

    public int Compare(EntryViewModel? x, EntryViewModel? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (x is null) return 1;
        if (y is null) return -1;

        if (x.IsParentEntry) return -1;
        if (y.IsParentEntry) return 1;

        return columnComparer.Compare(x, y);
    }

    private static Comparer<EntryViewModel> ByEntry(FileSystemObjectComparer comparer)
        => Comparer<EntryViewModel>.Create((x, y) => comparer.Compare(x.Entry, y.Entry));

    // Folders first, then the column's text.
    private static Comparer<EntryViewModel> ByText(Func<EntryViewModel, string> text)
        => Comparer<EntryViewModel>.Create((x, y) =>
        {
            if (x.Entry is Directory && y.Entry is not Directory)
                return -1;
            if (y.Entry is Directory && x.Entry is not Directory)
                return 1;
            return string.Compare(text(x), text(y));
        });
}
