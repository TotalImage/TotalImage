using System;
using TotalImage.Partitions;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for replacing the boot sector (MBR or VBR) of a partition or disk.
    /// </summary>
    /// <param name="targetObject">The partition whose boot sector will be replaced.</param>
    /// <param name="oldBootSector">The current boot sector of the partition or disk.</param>
    /// <param name="newBootSector">The boot sector to be written to the partition or disk.</param>
    /// <param name="timestamp">The date and time when the boot sector was replaced.</param>
    public class BootSectorReplacedOperation(PartitionEntry targetObject, byte[] oldBootSector, byte[] newBootSector, DateTime timestamp) : Operation(targetObject, timestamp)
    {
        /// <summary>
        /// The current boot sector of the partition or disk.
        /// </summary>
        public byte[] OldBootSector { get; } = oldBootSector;

        /// <summary>
        /// The boot sector to be written to the partition or disk.
        /// </summary>
        public byte[] NewBootSector { get; } = newBootSector;
    }
}
