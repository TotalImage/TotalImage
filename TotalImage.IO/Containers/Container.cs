using System;
using System.IO;
using TotalImage.Partitions;

namespace TotalImage.Containers
{
    /// <summary>
    /// An abstract representation of container files
    /// </summary>
    public abstract class Container : IDisposable
    {
        /// <summary>
        /// The underlying stream containing the image
        /// </summary>
        protected readonly Stream containerStream;

        private PartitionTable? _partitionTable;

        /// <summary>
        /// Returns the partition table contained within the image
        /// </summary>
        /// <exception cref="InvalidDataException">Thrown if no partition table could be found within the image</exception>
        public PartitionTable PartitionTable
        {
            get
            {
                _partitionTable ??= LoadPartitionTable();
                if (_partitionTable == null)
                {
                    throw new InvalidDataException();
                }

                return _partitionTable;
            }
        }

        /// <summary>
        /// A stream exposing the content of the container file
        /// </summary>
        public abstract Stream Content { get; }

        /// <summary>
        /// The length of the container file
        /// </summary>
        public long Length => Content.Length;

        /// <summary>
        /// Create a container file from an existing file
        /// </summary>
        /// <param name="path">The location of the image file</param>
        protected Container(string path)
        {
            containerStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        /// <summary>
        /// Create a container file from a memory stream
        /// </summary>
        /// <param name="stream">The stream containing the image file</param>
        protected Container(Stream stream)
        {
            containerStream = stream;
            containerStream.Position = 0;
        }

        /// <summary>
        /// Load the file system from the container image
        /// </summary>
        /// <returns>The file system found on the image</returns>
        protected abstract PartitionTable LoadPartitionTable();

        /// <summary>
        /// Get raw bytes from the container image
        /// </summary>
        /// <param name="offset">The offset from the start of the image</param>
        /// <param name="length">The number of bytes to retrieve</param>
        /// <returns>An array of bytes from the container</returns>
        public abstract byte[] GetRawBytes(int offset, int length);

        /// <summary>
        /// Save out the container to a file
        /// </summary>
        /// <param name="path">The path to save out the image to</param>
        public void SaveImage(string path)
        {
            using var outStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            containerStream.Position = 0;
            containerStream.CopyTo(outStream);
            outStream.Flush();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">Method is being called by a Dispose method</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                containerStream.Dispose();
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
