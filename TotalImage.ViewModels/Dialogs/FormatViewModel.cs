using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TotalImage.ViewModels.Dialogs;

/// <summary>
/// How to format a partition.
/// </summary>
/// <param name="FileSystem">The file system to create, or <c>null</c> if none was chosen.</param>
/// <param name="ClusterSize">The cluster size, or <c>null</c> for the default.</param>
/// <param name="VolumeLabel">The volume label.</param>
/// <param name="QuickFormat">Skip clearing the data area.</param>
public sealed record FormatOptions(string? FileSystem, string? ClusterSize, string VolumeLabel, bool QuickFormat);

/// <summary>
/// Asks how to format the current partition.
/// </summary>
public partial class FormatViewModel : DialogViewModel<FormatOptions>
{
    /// <summary>
    /// The longest volume label a file system can take.
    /// </summary>
    public const int MaxVolumeLabelLength = 11;

    public FormatViewModel()
    {
        VolumeLabel = string.Empty;
        QuickFormat = true;
    }

    // TODO: Obtain the list of available file systems and cluster sizes for this particular image
    /// <summary>
    /// The file systems that can be created.
    /// </summary>
    public IReadOnlyList<string> FileSystems { get; } = [];

    /// <summary>
    /// The cluster sizes available for <see cref="FileSystem"/>.
    /// </summary>
    public IReadOnlyList<string> ClusterSizes { get; } = [];

    [ObservableProperty]
    public partial string? FileSystem { get; set; }

    [ObservableProperty]
    public partial string? ClusterSize { get; set; }

    [ObservableProperty]
    public partial string VolumeLabel { get; set; }

    [ObservableProperty]
    public partial bool QuickFormat { get; set; }

    protected override FormatOptions CreateAnswer() => new(FileSystem, ClusterSize, VolumeLabel, QuickFormat);
}
