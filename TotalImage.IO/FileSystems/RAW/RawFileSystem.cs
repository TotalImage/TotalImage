using System.IO;

namespace TotalImage.FileSystems.RAW
{
    /// <summary>
    /// Represents an uninitialised partition containing no file system
    /// </summary>
    public class RawFileSystem : FileSystem
    {
        /// <summary>
        /// Create an empty RAW file system
        /// </summary>
        /// <param name="containerStream">The underlying stream</param>
        public RawFileSystem(Stream containerStream) : base(containerStream)
        {
        }

        /// <inheritdoc />
        public override string DisplayName => "RAW";

        /// <inheritdoc />
        public override string VolumeLabel
        {
            get => "";
            set { return; }
        }

        /// <inheritdoc />
        public override Directory RootDirectory => new RawRootDirectory(this);

        /// <inheritdoc />
        public override long TotalFreeSpace => 0;

        /// <inheritdoc />
        public override long TotalSize => _stream.Length;

        /// <inheritdoc />
        public override long AllocationUnitSize => 512;
    }
}
