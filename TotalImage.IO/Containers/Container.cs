using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using TotalImage.Partitions;
using TotalImage.Operations;

namespace TotalImage.Containers
{
    /// <summary>
    /// An abstract representation of container files
    /// </summary>
    public abstract class Container : IDisposable
    {
        private PartitionTable? _partitionTable;

        /// <summary>
        /// The backing file containing the image, opened as a memory-mapped file
        /// </summary>
        protected readonly MemoryMappedFile? backingFile;

        /// <summary>
        /// The underlying stream containing the image
        /// </summary>
        protected Stream containerStream;

        /// <summary>
        /// A stream exposing the content of the container file
        /// </summary>
        public abstract Stream Content { get; }

        /// <summary>
        /// The display name of the container.
        /// </summary>
        public abstract string DisplayName { get; }

        /// <summary>
        /// The length of the container file
        /// </summary>
        public long Length => Content.Length;

        /// <summary>
        /// The stack of changes made to this image that are still pending and may be either applied or discarded.
        /// </summary>
        public OperationsStack PendingChanges = new();

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
        /// Create a container file from an existing file
        /// </summary>
        /// <param name="path">The location of the image file</param>
        /// <param name="memoryMapping">Should the file be mapped into memory</param>
        protected Container(string path, bool memoryMapping)
        {
            if (memoryMapping)
            {
                backingFile = MemoryMappedFile.CreateFromFile(path, FileMode.Open);

                /* Using 0 for ViewStream size can cause problems when the view ends up being larger than the file (e.g. with VHDs, as the metadata is in
                 * the footer at the end of the file...), so for now let's just set it to file length instead. */
                FileInfo fileInfo = new FileInfo(path);
                containerStream = backingFile.CreateViewStream(0, fileInfo.Length, MemoryMappedFileAccess.Read);
            }
            else
            {
                containerStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            }
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
        /// Load the partition table from the container image
        /// </summary>
        /// <returns>The partition table found in the image</returns>
        protected virtual PartitionTable LoadPartitionTable()
        {
            return PartitionTable.AttemptDetection(this);
        }

        /// <summary>
        /// Save out the container to a file
        /// </summary>
        /// <param name="path">The path to save out the image to</param>
        public void SaveImage(string path)
        {
            // First, check if we can write to the current stream
            if (!containerStream.CanWrite)
            {
                throw new InvalidOperationException("Cannot save changes: the container stream is not writable. " +
                    "This may be because the container was opened with memory mapping enabled. " +
                    "Try opening the container without memory mapping to save changes.");
            }

            try
            {
                // Apply all pending operations to the container stream from bottom to top (oldest to newest)
                for (int i = 0; i < PendingChanges.Count(); i++)
                {
                    var operation = PendingChanges.PeekAt(i);
                    if (operation != null)
                    {
                        operation.Apply(containerStream);
                    }
                }

                // Create the output file by copying the updated container stream
                string? tempPath = Path.ChangeExtension(path, ".tmp");
                using FileStream outStream = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None);
                
                containerStream.Position = 0;
                containerStream.CopyTo(outStream);
                outStream.Flush();
                outStream.Close();
                
                // Replace the target file with the temporary file
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                File.Move(tempPath, path);

                // Clear the pending changes since they have been applied
                PendingChanges.Discard();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save image to '{path}': {ex.Message}", ex);
            }
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
                backingFile?.Dispose();
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        SemaphoreSlim hashMutex = new SemaphoreSlim(1, 1);

        async Task<string> CalculateHashAsyncCore(HashAlgorithm algorithm, CancellationToken cancellationToken)
        {
            await hashMutex.WaitAsync(cancellationToken);
            byte[] hash;

            try
            {
                containerStream.Position = 0;
                hash = await algorithm.ComputeHashAsync(containerStream, cancellationToken);
            }
            finally
            {
                hashMutex.Release();
            }

            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        /// <summary>
        /// Calculates the MD5 hash of this file
        /// </summary>
        /// <returns>A string containing the hexadecimal representation of the MD5 hash</returns>
        public string CalculateMd5Hash()
        {
            using MD5 md5 = MD5.Create();
            containerStream.Seek(0, SeekOrigin.Begin);
            byte[]? hash = md5.ComputeHash(containerStream);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        /// <summary>
        /// Asynchronously calculates the MD5 hash of this file
        /// </summary>
        /// <returns>A task that represents the asynchronous hash calculation operation and wraps the string containing the hexadecimal representation of the MD5 hash</returns>
        public async Task<string> CalculateMd5HashAsync(CancellationToken cancellationToken = default)
            => await CalculateHashAsyncCore(MD5.Create(), cancellationToken);

        /// <summary>
        /// Calculates the SHA-1 hash of this file
        /// </summary>
        /// <returns>A string containing the hexadecimal representation of the SHA-1 hash</returns>
        public string CalculateSha1Hash()
        {
            using SHA1 sha1 = SHA1.Create();
            containerStream.Seek(0, SeekOrigin.Begin);
            byte[]? hash = sha1.ComputeHash(containerStream);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        /// <summary>
        /// Asynchronously calculates the SHA-1 hash of this file
        /// </summary>
        /// <returns>A task that represents the asynchronous hash calculation operation and wraps the string containing the hexadecimal representation of the SHA-1 hash</returns>
        public async Task<string> CalculateSha1HashAsync(CancellationToken cancellationToken = default)
            => await CalculateHashAsyncCore(SHA1.Create(), cancellationToken);
    }
}
