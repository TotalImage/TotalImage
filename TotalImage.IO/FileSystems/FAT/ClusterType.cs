namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// Enumeration of possible FAT and exFAT cluster map value types. Cluster chains follow
    /// <see cref="Data"/> values, end at <see cref="EndOfChain"/>, and treat any other type as corruption.
    /// </summary>
    public enum ClusterType
    {
        /// <summary>
        /// Free cluster (FAT value 0). Invalid inside a cluster chain.
        /// </summary>
        Free,

        /// <summary>
        /// FAT value 1. Not defined by the specification; DOS writes it as a temporary "in use" marker
        /// while building a chain, so it only remains on disk after an interrupted allocation.
        /// Invalid inside a cluster chain.
        /// </summary>
        NonFree,

        /// <summary>
        /// Pointer to a cluster described by the allocation table (2 to the table length - 1).
        /// </summary>
        Data,

        /// <summary>
        /// Bad cluster (xF7). Invalid inside a cluster chain.
        /// </summary>
        Bad,

        /// <summary>
        /// Last cluster in a file (FAT xF8–xFF, exFAT 0xFFFFFFFF).
        /// </summary>
        EndOfChain,

        /// <summary>
        /// A value the specification does not allow in a cluster map entry. On FAT this is MAX+1 to xF6
        /// (where MAX is the last cluster number), which the FAT specification reserves and says must not be used.
        /// On exFAT it is anything outside the valid range in section 4.1.3 of the exFAT specification:
        /// 0, 1, a pointer past the last cluster, or 0xFFFFFFF8–0xFFFFFFFE.
        /// </summary>
        Invalid
    }
}
