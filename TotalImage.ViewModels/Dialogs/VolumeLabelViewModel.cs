using CommunityToolkit.Mvvm.ComponentModel;

namespace TotalImage.ViewModels.Dialogs;

/// <summary>
/// The new volume labels of a FAT volume.
/// </summary>
/// <param name="RootDirectoryLabel">The label for the root directory entry, upper case and padded to 11 characters.</param>
/// <param name="WriteBpbLabel">Also write <paramref name="BpbLabel"/> to the BPB.</param>
/// <param name="BpbLabel">The label for the BPB, upper case and padded to 11 characters.</param>
public sealed record VolumeLabelChange(string RootDirectoryLabel, bool WriteBpbLabel, string BpbLabel);

/// <summary>
/// Asks for new volume labels for a FAT volume.
/// </summary>
public partial class VolumeLabelViewModel : DialogViewModel<VolumeLabelChange>
{
    /// <summary>
    /// The longest label FAT can store.
    /// </summary>
    public const int MaxLength = 11;

    public VolumeLabelViewModel(string rootDirectoryLabel, string bpbLabel)
    {
        RootDirectoryLabel = rootDirectoryLabel;
        BpbLabel = bpbLabel;
        // Both options start enabled, so the BPB label starts out matching the root directory label.
        WriteBpbLabel = true;
        SyncLabels = true;
    }

    /// <summary>
    /// The label stored in the root directory.
    /// </summary>
    [ObservableProperty]
    public partial string RootDirectoryLabel { get; set; }

    /// <summary>
    /// Whether the label is also written to the BPB.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanEditBpbLabel), nameof(IsBpbLabelReadOnly))]
    public partial bool WriteBpbLabel { get; set; }

    /// <summary>
    /// The label stored in the BPB.
    /// </summary>
    [ObservableProperty]
    public partial string BpbLabel { get; set; }

    /// <summary>
    /// Whether the BPB label follows the root directory label.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsBpbLabelReadOnly))]
    public partial bool SyncLabels { get; set; }

    /// <summary>
    /// The BPB label and the sync option can only be edited while the BPB label is written.
    /// </summary>
    public bool CanEditBpbLabel => WriteBpbLabel;

    /// <summary>
    /// The BPB label is read-only while it follows the root directory label.
    /// </summary>
    public bool IsBpbLabelReadOnly => SyncLabels;

    partial void OnRootDirectoryLabelChanged(string value)
    {
        if (WriteBpbLabel && SyncLabels)
            BpbLabel = value;
    }

    partial void OnWriteBpbLabelChanged(bool value)
    {
        if (value)
        {
            BpbLabel = RootDirectoryLabel;
        }
        else
        {
            BpbLabel = string.Empty;
            SyncLabels = false;
        }
    }

    partial void OnSyncLabelsChanged(bool value)
    {
        if (value)
            BpbLabel = RootDirectoryLabel;
    }

    protected override VolumeLabelChange CreateAnswer()
        => new(Normalize(RootDirectoryLabel), WriteBpbLabel, Normalize(BpbLabel));

    private static string Normalize(string label) => label.ToUpperInvariant().PadRight(MaxLength, ' ');
}
