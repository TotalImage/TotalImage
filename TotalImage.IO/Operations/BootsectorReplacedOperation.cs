using System;
using System.IO;
using TotalImage.Partitions;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for replacing the bootsector of a partition.
    /// </summary>
    /// <param name="targetObject">The partition whose bootsector will be replaced.</param>
    /// <param name="bootsector">The bootsector to be added to the partition.</param>
    /// <param name="timestamp">The date and time when the bootsector was replaced.</param>
    public class BootsectorReplacedOperation(PartitionEntry targetObject, byte[] bootsector, DateTime timestamp) : Operation(targetObject, timestamp)
    {
        /// <summary>
        /// The bootsector to be added to the partition.
        /// </summary>
        public byte[] Bootsector { get; } = bootsector;

        /// <summary>
        /// Apply this operation to write the bootsector to the partition.
        /// </summary>
        /// <param name="imageStream">The stream representing the disk image content.</param>
        public override void Apply(Stream imageStream)
        {
            var partition = (PartitionEntry)TargetObject;
            
            // Seek to the start of the partition
            imageStream.Seek(partition.Offset, SeekOrigin.Begin);
            
            // Write the bootsector (typically 512 bytes)
            imageStream.Write(Bootsector, 0, Bootsector.Length);
            imageStream.Flush();
        }
    }
}
