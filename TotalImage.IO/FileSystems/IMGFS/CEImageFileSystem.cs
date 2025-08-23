using System;
using System.IO;

namespace TotalImage.FileSystems.IMGFS
{
    /// <summary>
    /// Representation of a Windows CE IMGFS file system
    /// </summary>
    public class CEImageFileSystem : FileSystem
    {
        /// <inheritdoc />
        public override string DisplayName => "CE Image File System";

        /// <inheritdoc />
        public override string VolumeLabel { get; set; } = "";

        /// <inheritdoc />
        public override Directory RootDirectory => throw new NotImplementedException();

        /// <inheritdoc />
        public override long TotalFreeSpace => 0;

        /// <inheritdoc />
        public override long TotalSize { get; }

        /// <inheritdoc />
        public override uint AllocationUnitSize => 1;

        /// <inheritdoc />
        public override bool SupportsSubdirectories => true;

        /// <summary>
        /// Create a Windows CE IMGFS file system
        /// </summary>
        /// <param name="containerStream">The underlying stream</param>
        public CEImageFileSystem(Stream containerStream) : base(containerStream)
        {
        }
    }
}
