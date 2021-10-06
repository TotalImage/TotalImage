using System;
using System.Collections.Immutable;

namespace TotalImage.FileSystems.UDF
{
    /// <summary>
    /// Represents an unknown type of UDF volume descriptor
    /// </summary>
    public class UdfUnknownVolumeDescriptor : UdfVolumeDescriptor
    {
        /// <summary>
        /// The raw binary data contained within the record
        /// </summary>
        public ImmutableArray<byte> Content { get; }

        /// <summary>
        /// Create a UDF volume descriptor of an unknown type
        /// </summary>
        /// <param name="record">A span containing the volume descriptor record</param>
        /// <param name="tag">The volume descriptor tag</param>
        public UdfUnknownVolumeDescriptor(in ReadOnlySpan<byte> record, UdfDescriptorTag tag) : base(tag)
        {
            Content = record[16..2048].ToArray().ToImmutableArray();
        }
    }
}
