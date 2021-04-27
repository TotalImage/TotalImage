namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// Enumeration of possible FAT cluster value types.
    /// </summary>
    public enum ClusterType
    {
        /// <summary>
        /// Free cluster. Treated as an end-of-chain marker.
        /// </summary>
        Free,

        /// <summary>
        /// Reserved cluster. Treated as an end-of-chain marker.
        /// </summary>
        NonFree,

        /// <summary>
        /// Data cluster.
        /// </summary>
        Data,

        /// <summary>
        /// Reserved cluster. Can be a data cluster in non-standard implementations.
        /// </summary>
        Reserved,

        /// <summary>
        /// Bad cluster. Can be a data cluster in non-standard implementations.
        /// </summary>
        Bad,

        /// <summary>
        /// Last cluster in a file.
        /// </summary>
        EndOfChain
    }
}