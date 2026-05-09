using System;
using System.Collections.Immutable;
using System.IO;
using TotalImage.Containers;
using TotalImage.FileSystems.ExFAT;
using TotalImage.FileSystems.FAT;
using TotalImage.FileSystems.IMGFS;
using TotalImage.FileSystems.ISO;
using TotalImage.FileSystems.NTFS;
using TotalImage.FileSystems.RAW;
using TotalImage.FileSystems.UDF;

namespace TotalImage.FileSystems
{
    /// <summary>
    /// An abstract class representing a file system
    /// </summary>
    public abstract class FileSystem
    {
        private static readonly ImmutableArray<IFileSystemFactory> _knownFactories =
        [
            new NtfsFactory(),
            new FatFactory(),
            new IsoFactory(),
            new UdfFactory(),
            new ExFatFactory(),
            new ImgfsFactory()
        ];

        /// <summary>
        /// The stream containing the file system
        /// </summary>
        protected readonly Stream _stream;

        /// <summary>
        /// The container that owns this file system. Set by <see cref="Partitions.PartitionEntry"/> after
        /// detection so that mutation entry points can reach the container's <see cref="Container.PendingChanges"/>.
        /// May be <see langword="null"/> for file systems not associated with a container
        /// (e.g. when opened directly from a stream in tests).
        /// </summary>
        public Container? OwningContainer { get; internal set; }

        /// <summary>
        /// The zero-based index of this file system's partition within <see cref="OwningContainer"/>.
        /// Set alongside <see cref="OwningContainer"/> by <see cref="Partitions.PartitionEntry"/>.
        /// </summary>
        public int PartitionIndex { get; internal set; }

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
        public abstract long AllocationUnitSize { get; }

        /// <summary>
        /// Does the file system support subdirectories
        /// </summary>
        public abstract bool SupportsSubdirectories { get; }

        /// <summary>
        /// Is this file system read-only
        /// </summary>
        public abstract bool IsReadOnly { get; }

        /// <summary>
        /// Create a representation of the file system
        /// </summary>
        /// <param name="containerStream">The stream containing the file system</param>
        protected FileSystem(Stream containerStream)
        {
            _stream = containerStream;
        }

        /// <summary>
        /// Attempt to detect the file system of a stream using known file system types
        /// </summary>
        /// <param name="stream">The stream containing a file system</param>
        /// <returns>A file system if detection was successful, null if not.</returns>
        public static FileSystem AttemptDetection(Stream stream)
        {
            foreach (var factory in _knownFactories)
            {
                try
                {
                    var result = factory.TryLoadFileSystem(stream);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"File system detection failed for {factory.GetType().Name}: {ex.Message}");
                }
            }

            return new RawFileSystem(stream);
        }

        /// <summary>
        /// Retrieves a stream of the file system contents
        /// </summary>
        public Stream GetStream()
        {
            return _stream;
        }
    }
}
