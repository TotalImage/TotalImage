using System.IO;
using TotalImage.Containers;
using TotalImage.FileSystems;

namespace TotalImage.Partitions
{
    /// <summary>
    /// A partition entry record identifying a partition within a container
    /// and exposing the file system within that partition
    /// </summary>
    public class PartitionEntry
    {
        /// <summary>
        /// The stream containing the partition's data
        /// </summary>
        private readonly Stream _stream;

        /// <summary>
        /// The container that owns this partition entry. Used to wire up
        /// <see cref="FileSystem.OwningContainer"/> after file system detection.
        /// </summary>
        private readonly Container? _owningContainer;

        /// <summary>
        /// The filesystem contained by the partition, validated and exposed by <see cref="FileSystem" />
        /// </summary>
        private FileSystem? _fileSystem = null;

        /// <summary>
        /// The offset of the partition in it's container file
        /// </summary>
        public long Offset { get; }

        /// <summary>
        /// The length of the partition in bytes
        /// </summary>
        public long Length { get; }

        /// <summary>
        /// Whether this partition supports writing — i.e. the owning container supports writing
        /// and the partition's file system is not read-only.
        /// </summary>
        public bool SupportsWriting
            => (_owningContainer?.SupportsWriting ?? false) && !FileSystem.IsReadOnly;

        /// <summary>
        /// The filesystem contained by the partition
        /// </summary>
        public FileSystem FileSystem
        {
            get
            {
                _fileSystem ??= LoadFileSystem();
                if (_fileSystem == null)
                {
                    throw new InvalidDataException();
                }

                return _fileSystem;
            }
        }

        /// <summary>
        /// Create a partition entry record from a stream and its position within a parent container.
        /// </summary>
        /// <param name="offset">The offset of the partition in it's container file</param>
        /// <param name="length">The length of the partition</param>
        /// <param name="stream">The stream containing the partition data</param>
        /// <param name="owningContainer">
        /// The container that owns this partition. When provided, it is wired to
        /// <see cref="FileSystem.OwningContainer"/> after detection so that file system
        /// mutation entry points can reach the container's pending change set.
        /// </param>
        public PartitionEntry(long offset, long length, Stream stream, Container? owningContainer = null)
        {
            Offset = offset;
            Length = length;
            _stream = stream;
            _owningContainer = owningContainer;
        }

        /// <summary>
        /// Load the file system from the partition
        /// </summary>
        /// <returns>The file system from the partition</returns>
        private FileSystem? LoadFileSystem()
        {
            var fs = FileSystem.AttemptDetection(_stream);
            if (fs is not null)
                fs.OwningContainer = _owningContainer;
            return fs;
        }
    }
}
