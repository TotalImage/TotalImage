using System.Collections.Immutable;
using System.IO;
using TotalImage.FileSystems.FAT;
using TotalImage.FileSystems.ISO;
using TotalImage.FileSystems.RAW;

namespace TotalImage.FileSystems
{
    /// <summary>
    /// An abstract class representing a file system
    /// </summary>
    public abstract class FileSystem
    {
        private readonly static ImmutableArray<IFileSystemFactory> _knownFactories = ImmutableArray.Create<IFileSystemFactory>(
            new FatFactory(),
            new IsoFactory()
        );

        /// <summary>
        /// Attempt to detect the file system of a stream using known file system types
        /// </summary>
        /// <param name="stream">The stream containing a file system</param>
        /// <returns>A file system if detection was successful, null if not.</returns>
        public static FileSystem AttemptDetection(Stream stream)
        {
            foreach (var factory in _knownFactories)
            {
                var result = factory.TryLoadFileSystem(stream);
                if (result != null)
                {
                    return result;
                }
            }

            return new RawFileSystem(stream);
        }

        /// <summary>
        /// The stream containing the file system
        /// </summary>
        protected readonly Stream _stream;

        /// <summary>
        /// Create a representation of the file system
        /// </summary>
        /// <param name="containerStream">The stream containing the file system</param>
        protected FileSystem(Stream containerStream)
        {
            _stream = containerStream;
        }

        /// <summary>
        /// Retrieves a stream of the file system contents
        /// </summary>
        public Stream GetStream()
        {
            return _stream;
        }

        /// <summary>
        /// A display name for the format of the file system
        /// </summary>
        public abstract string DisplayName { get; }

        /// <summary>
        /// The volume label specified within the file system
        /// </summary>
        public abstract string VolumeLabel { get; set; }

        /// <summary>
        /// The root directory entry of the file system
        /// </summary>
        public abstract Directory RootDirectory { get; }

        /// <summary>
        /// The total free space within the file system
        /// </summary>
        public abstract long TotalFreeSpace { get; }

        /// <summary>
        /// The total size of the file system
        /// </summary>
        public abstract long TotalSize { get; }

        /// <summary>
        /// The minimum allocatable unit size on the file system
        /// </summary>
        public abstract uint AllocationUnitSize { get; }
    }
}