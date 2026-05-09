using System;
using System.IO;

namespace TotalImage.Changes
{
    /// <summary>
    /// Provides access to the data content of a pending file injection, either held in memory or backed by a
    /// temporary file on disk, depending on the size of the data relative to the memory-mapping threshold.
    /// </summary>
    public abstract class FileDataSource : IDisposable
    {
        private bool _disposed;

        /// <summary>
        /// The length of the file data in bytes.
        /// </summary>
        public abstract long Length { get; }

        /// <summary>
        /// Opens a read-only stream over the file data.
        /// </summary>
        public abstract Stream OpenRead();

        /// <summary>
        /// Creates a <see cref="FileDataSource"/> from a stream, choosing in-memory or temp-file storage based on
        /// the provided threshold.
        /// </summary>
        /// <param name="source">The stream containing the file data. Its position should be at the beginning.</param>
        /// <param name="tempDir">The directory to use for temporary files when the data exceeds the threshold.</param>
        /// <param name="memoryThreshold">
        /// If the source length is below this value, data is stored in memory; otherwise it is written to a temp file.
        /// </param>
        public static FileDataSource Create(Stream source, string tempDir, long memoryThreshold)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (tempDir is null) throw new ArgumentNullException(nameof(tempDir));

            if (source.Length < memoryThreshold)
            {
                var data = new byte[source.Length];
                source.ReadExactly(data);
                return new InMemoryFileDataSource(data);
            }
            else
            {
                Directory.CreateDirectory(tempDir);
                string tempPath = Path.Combine(tempDir, Guid.NewGuid().ToString("N") + ".tmp");
                using (var tempFile = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    source.CopyTo(tempFile);
                }
                return new TempFileDataSource(tempPath, source.Length);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                DisposeCore();
                _disposed = true;
            }
        }

        /// <summary>
        /// Override in derived classes to release resources.
        /// </summary>
        protected virtual void DisposeCore() { }
    }

    /// <summary>
    /// A <see cref="FileDataSource"/> that holds file data in a byte array in memory.
    /// </summary>
    public sealed class InMemoryFileDataSource : FileDataSource
    {
        private readonly byte[] _data;

        /// <inheritdoc />
        public override long Length => _data.Length;

        /// <summary>
        /// Creates an in-memory data source from a byte array.
        /// </summary>
        public InMemoryFileDataSource(byte[] data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
        }

        /// <inheritdoc />
        public override Stream OpenRead() => new MemoryStream(_data, writable: false);
    }

    /// <summary>
    /// A <see cref="FileDataSource"/> backed by a temporary file on disk. The temp file is deleted when this
    /// instance is disposed.
    /// </summary>
    public sealed class TempFileDataSource : FileDataSource
    {
        private readonly string _tempPath;

        /// <inheritdoc />
        public override long Length { get; }

        /// <summary>
        /// Creates a temp-file data source referencing the given path.
        /// </summary>
        internal TempFileDataSource(string tempPath, long length)
        {
            _tempPath = tempPath ?? throw new ArgumentNullException(nameof(tempPath));
            Length = length;
        }

        /// <inheritdoc />
        public override Stream OpenRead()
            => new FileStream(_tempPath, FileMode.Open, FileAccess.Read, FileShare.Read);

        /// <inheritdoc />
        protected override void DisposeCore()
        {
            try { File.Delete(_tempPath); } catch (IOException) { /* best effort */ }
        }
    }
}
