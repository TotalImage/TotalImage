using System;

namespace TotalImage.Changes
{
    /// <summary>
    /// Represents a pending volume label change on a partition's filesystem.
    /// </summary>
    public sealed class VolumeLabelChange : PendingChange
    {
        /// <summary>
        /// The new volume label to apply. May be null or empty to clear the label.
        /// </summary>
        public string NewLabel { get; }

        /// <summary>
        /// The zero-based index of the partition whose filesystem volume label should be changed.
        /// </summary>
        public int PartitionIndex { get; }

        /// <summary>
        /// Creates a new <see cref="VolumeLabelChange"/>.
        /// </summary>
        /// <param name="partitionIndex">Zero-based index of the target partition.</param>
        /// <param name="newLabel">The new volume label.</param>
        public VolumeLabelChange(int partitionIndex, string newLabel)
        {
            if (partitionIndex < 0) throw new ArgumentOutOfRangeException(nameof(partitionIndex));
            PartitionIndex = partitionIndex;
            NewLabel = newLabel ?? string.Empty;
        }
    }
}
