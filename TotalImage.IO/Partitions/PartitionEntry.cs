using System.IO;
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
        /// Load the file system from the partition
        /// </summary>
        /// <returns>The file system from the partition</returns>
        private FileSystem? LoadFileSystem()
        {
            return FileSystem.AttemptDetection(_stream);
        }

        /// <summary>
        /// Create a partition entry record from a stream and its position within a parent container
        /// </summary>
        /// <param name="offset">The offset of the partition in it's container file</param>
        /// <param name="length">The length of the partition</param>
        /// <param name="stream">The stream containing the partition data</param>
        public PartitionEntry(long offset, long length, Stream stream)
        {
            Offset = offset;
            Length = length;
            _stream = stream;
        }
    }
}