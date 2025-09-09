using System;
using TotalImage.Partitions;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for changing the volume label.
    /// </summary>
    public class VolumeLabelChangedOperation : Operation
    { 
        /// <summary>
        /// The volume label before this operation.
        /// </summary>
        public string OldLabel { get; }

        /// <summary>
        /// The volume label after this operation.
        /// </summary>
        public string NewLabel { get; }

        /// <summary>
        /// Indicates which volume label is to be changedd in a FAT file system.
        /// </summary>
        public VolumeLabel? LabelToChange { get; }

        /// <summary>
        /// Which volume label is to be changed in a FAT file system.
        /// </summary>
        public enum VolumeLabel
        {
            /// <summary>
            /// Only the volume label in the root directory is to be changed.
            /// </summary>
            RootDirectory,

            /// <summary>
            /// Only the volume label in the BIOS parameter block (BPB) is to be changed.
            /// </summary>
            BiosParameterBlock,

            /// <summary>
            /// Both the root directory and BPB volume labels are to be changed to the same value.
            /// </summary>
            Both
        }

        /// <summary>
        /// An operation for changing the volume label.
        /// </summary>
        /// <param name="targetObject">The partition entry of the partition whose volume label is to be changed.</param>
        /// <param name="oldLabel">Old volume label.</param>
        /// <param name="newLabel">New volume label.</param>
        /// <param name="labelToChange">Which volume label is to be changed for FAT file systems.</param>
        /// <param name="timestamp">The date and time when the volume label was changed.</param>
        public VolumeLabelChangedOperation(PartitionEntry targetObject, string oldLabel, string newLabel, VolumeLabelChangedOperation.VolumeLabel labelToChange, DateTime timestamp) : base(targetObject, timestamp)
        {
            if (string.IsNullOrEmpty(oldLabel))
            {
                throw new ArgumentNullException(nameof(oldLabel), "Old label cannot be null!");
            }

            if (string.IsNullOrEmpty(newLabel))
            {
                throw new ArgumentNullException(nameof(newLabel), "New label cannot be null!");
            }

            OldLabel = oldLabel;
            NewLabel = newLabel;   
            LabelToChange = labelToChange;
        }
    }
}
