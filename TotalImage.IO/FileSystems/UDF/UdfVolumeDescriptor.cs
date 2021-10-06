using System;

namespace TotalImage.FileSystems.UDF
{
    /// <summary>
    /// Represents a UDF Volume Descriptor structure
    /// </summary>
    public abstract class UdfVolumeDescriptor
    {
        /// <summary>
        /// The tag describing the descriptor
        /// </summary>
        public UdfDescriptorTag Tag { get; }

        /// <summary>
        /// Create a UDF volume descriptor
        /// </summary>
        /// <param name="tag">The tag describing the descriptor</param>
        protected UdfVolumeDescriptor(UdfDescriptorTag tag)
        {
            Tag = tag;
        }

        /// <summary>
        /// Read a UDF volume descriptor
        /// </summary>
        /// <param name="record">A span containing the volume descriptor record</param>
        /// <returns>The volume descriptor record</returns>
        public static UdfVolumeDescriptor? ReadVolumeDescriptor(in ReadOnlySpan<byte> record)
        {
            UdfDescriptorTag tag = new UdfDescriptorTag(record[0..16]);
            if (!tag.IsValid())
            {
                return null;
            }

            return tag.Identifier switch
            {
                _ => new UdfUnknownVolumeDescriptor(record, tag),
            };
        }
    }
}
