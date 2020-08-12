using System;
using System.IO;
using TotalImage.FileSystems;

namespace TotalImage.ImageContainers
{
    /// <summary>
    /// An abstract representation of container files
    /// </summary>
    public abstract class ImageContainer : IDisposable
    {
        /// <summary>
        /// The underlying stream containing the image
        /// </summary>
        protected readonly MemoryStream containerStream;
        private FileSystem? _fileSystem;

        /// <summary>
        /// Returns the file system contained within the image
        /// </summary>
        /// <exception cref="InvalidDataException">Thrown if no file system could be found within the image</exception>
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
        /// Create a container file from an existing file
        /// </summary>
        /// <param name="path">The location of the image file</param>
        protected ImageContainer(string path)
        {
            using var inStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            containerStream = new MemoryStream();
            inStream.CopyTo(containerStream);
            containerStream.Flush();
        }

        /// <summary>
        /// Create a container file from a memory stream
        /// </summary>
        /// <param name="stream">The stream containing the image file</param>
        protected ImageContainer(MemoryStream stream)
        {
            containerStream = stream;
            containerStream.Position = 0;
        }

        /// <summary>
        /// Load the file system from the container image
        /// </summary>
        /// <returns>The file system found on the image</returns>
        protected abstract FileSystem LoadFileSystem();

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
