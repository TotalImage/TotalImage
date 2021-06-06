namespace TotalImage.FileSystems.ISO
{
    /// <summary>
    /// The type of volume descriptor record
    /// </summary>
    public enum IsoVolumeDescriptorType : byte
    {
        /// <summary>
        /// This record describes a boot record
        /// </summary>
        BootRecord = 0,

        /// <summary>
        /// This is the primary record describing a volume
        /// </summary>
        PrimaryVolumeDescriptor = 1,

        /// <summary>
        /// This is a supplementary or enhanced record describing a volume
        /// </summary>
        SupplementaryVolumeDescriptor = 2,

        /// <summary>
        /// This record describes a volume partition
        /// </summary>
        VolumePartitionDescriptor = 3,

        /// <summary>
        /// This terminates the set of volume descriptors
        /// </summary>
        VolumeDescriptorSetTerminator = 255
    }
}
