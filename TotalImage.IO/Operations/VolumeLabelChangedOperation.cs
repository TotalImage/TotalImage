using TotalImage.Partitions;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for changing the volume label.
    /// </summary>
    /// <param name="targetObject">The partition whose volume label is to be changed.</param>
    /// <param name="oldLabel">Old volume label.</param>
    /// <param name="newLabel">New volume label.</param>
    /// <param name="labelToChange">Which volume label is to be changed for FAT file systems.</param>
    public class VolumeLabelChangedOperation(PartitionEntry targetObject, string oldLabel, string newLabel, VolumeLabelChangedOperation.VolumeLabel labelToChange) : Operation(targetObject) 
    { 
        /// <summary>
        /// The volume label before this operation.
        /// </summary>
        public string OldLabel { get; } = oldLabel;

        /// <summary>
        /// The volume label after this operation.
        /// </summary>
        public string NewLabel { get; } = newLabel;

        /// <summary>
        /// Indicates which volume label was changed in a FAT file system.
        /// </summary>
        public VolumeLabel VolumeLabelChanged { get; } = labelToChange;

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
    }
}
