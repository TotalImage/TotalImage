using System.Collections.Immutable;

namespace TotalImage.FileSystems.HSF
{
    /// <summary>
    /// Represents a High Sierra volume descriptor set terminator
    /// </summary>
    public class HsfSetTerminatorVolumeDescriptor : HsfVolumeDescriptor
    {
        /// <summary>
        /// Create a High Sierra volume descriptor set terminator record
        /// </summary>
        /// <param name="type">The type of the volume descriptor</param>
        /// <param name="identifier">The volume descriptor identifier</param>
        /// <param name="version">The version of the volume descriptor</param>
        public HsfSetTerminatorVolumeDescriptor(in HsfVolumeDescriptorType type, in ImmutableArray<byte> identifier, in byte version)
            : base(type, identifier, version)
        {
        }
    }
}
