using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TotalImage.ViewModels.Dialogs;

/// <summary>
/// One row of the partition management list.
/// </summary>
/// <param name="Number">The partition number.</param>
/// <param name="VolumeLabel">The volume label.</param>
/// <param name="Type">The partition type.</param>
/// <param name="Start">Where the partition starts.</param>
/// <param name="End">Where the partition ends.</param>
/// <param name="Size">The partition size.</param>
/// <param name="Active">Whether the partition is active.</param>
public sealed record ManagedPartitionRow(string Number, string VolumeLabel, string Type, string Start, string End, string Size, string Active);

/// <summary>
/// The partition manager. Only the list and the selection-dependent buttons exist so far:
/// nothing fills the list yet, and none of the actions are implemented.
/// </summary>
public partial class ManagePartitionsViewModel : ObservableObject
{
    public ManagePartitionsViewModel(IReadOnlyList<ManagedPartitionRow> rows)
    {
        Rows = rows;
        SelectedIndex = -1;
    }

    /// <summary>
    /// The partitions shown in the list.
    /// </summary>
    public IReadOnlyList<ManagedPartitionRow> Rows { get; }

    /// <summary>
    /// The index of the selected row, or -1 when nothing is selected.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasSelection))]
    public partial int SelectedIndex { get; set; }

    /// <summary>
    /// Whether a partition is selected. Setting it active, deleting, formatting and resizing
    /// are only available then.
    /// </summary>
    public bool HasSelection => SelectedIndex >= 0 && SelectedIndex < Rows.Count;
}
