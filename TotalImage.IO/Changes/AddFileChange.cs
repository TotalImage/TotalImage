using System;
using TotalImage.FileSystems.FAT;

namespace TotalImage.Changes
{
    /// <summary>
    /// Represents a pending file injection into the image. The file content is held by a
    /// <see cref="FileDataSource"/> which may store data in memory or in a temporary file,
    /// depending on the size relative to the memory-mapping threshold.
    /// </summary>
    public sealed class AddFileChange : PendingChange, IDisposable
    {
        private bool _disposed;

        /// <summary>
        /// The path components identifying the destination within the filesystem.
        /// The last component is the filename; preceding components are directory names.
        /// Example: <c>["SUBDIR", "README.TXT"]</c> injects into <c>SUBDIR\README.TXT</c>.
        /// An empty array or single element places the file in the root directory.
        /// </summary>
        public string[] DestinationPath { get; }

        /// <summary>
        /// The source data to inject. May be in-memory or backed by a temporary file.
        /// </summary>
        public FileDataSource Source { get; }

        /// <summary>
        /// The FAT attributes to assign to the new file entry.
        /// </summary>
        public FatAttributes Attributes { get; }

        /// <summary>
        /// The creation timestamp for the new file entry.
        /// </summary>
        public DateTime CreationTime { get; }

        /// <summary>
        /// The last write timestamp for the new file entry.
        /// </summary>
        public DateTime LastWriteTime { get; }

        /// <summary>
        /// The last access date for the new file entry.
        /// </summary>
        public DateTime LastAccessTime { get; }

        /// <summary>
        /// Creates a new <see cref="AddFileChange"/>.
        /// </summary>
        /// <param name="destinationPath">Path components within the filesystem.</param>
        /// <param name="source">The file data source.</param>
        /// <param name="attributes">FAT attributes for the new entry.</param>
        /// <param name="creationTime">Creation timestamp.</param>
        /// <param name="lastWriteTime">Last write timestamp.</param>
        /// <param name="lastAccessTime">Last access timestamp.</param>
        public AddFileChange(
            string[] destinationPath,
            FileDataSource source,
            FatAttributes attributes,
            DateTime creationTime,
            DateTime lastWriteTime,
            DateTime lastAccessTime)
        {
            DestinationPath = destinationPath ?? throw new ArgumentNullException(nameof(destinationPath));
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Attributes = attributes;
            CreationTime = creationTime;
            LastWriteTime = lastWriteTime;
            LastAccessTime = lastAccessTime;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (!_disposed)
            {
                Source.Dispose();
                _disposed = true;
            }
        }
    }
}
