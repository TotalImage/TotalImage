using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TotalImage.FileSystems;

namespace TotalImage.ViewModels.Entries;

/// <summary>
/// Sizes and counts of a selection of entries.
/// </summary>
/// <param name="SelectedFiles">The number of selected entries that are not directories.</param>
/// <param name="SelectedDirectories">The number of selected directories.</param>
/// <param name="ContainedFiles">The number of files inside the selected directories, at any depth.</param>
/// <param name="ContainedDirectories">The number of directories inside the selected directories, at any depth.</param>
/// <param name="Size">The total size of the selected files and of all files in the selected directories.</param>
/// <param name="SizeOnDisk">The same total, counting whole allocation units.</param>
public sealed record EntryMeasurements(
    ulong SelectedFiles, ulong SelectedDirectories,
    ulong ContainedFiles, ulong ContainedDirectories,
    ulong Size, ulong SizeOnDisk)
{
    /// <summary>
    /// Measures the entries on a background thread, since directories are scanned recursively.
    /// </summary>
    public static Task<EntryMeasurements> MeasureAsync(IReadOnlyList<FileSystemObject> entries, CancellationToken cancellationToken = default)
        => Task.Run(() => Measure(entries, cancellationToken), cancellationToken);

    /// <summary>
    /// Measures the entries on the calling thread.
    /// </summary>
    public static EntryMeasurements Measure(IReadOnlyList<FileSystemObject> entries, CancellationToken cancellationToken = default)
    {
        ulong selectedFiles = 0, selectedDirectories = 0, containedFiles = 0, containedDirectories = 0, size = 0, sizeOnDisk = 0;

        foreach (var entry in entries)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (entry is Directory directory)
            {
                selectedDirectories++;
                var stats = directory.GetStats(true);
                containedFiles += stats.FileCount;
                containedDirectories += stats.DirectoryCount;
                size += stats.Size;
                sizeOnDisk += stats.SizeOnDisk;
            }
            else
            {
                selectedFiles++;
                size += entry.Length;
                sizeOnDisk += entry.LengthOnDisk;
            }
        }

        return new(selectedFiles, selectedDirectories, containedFiles, containedDirectories, size, sizeOnDisk);
    }
}
