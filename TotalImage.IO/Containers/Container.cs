using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using TotalImage.FileSystems.ISO;
using TotalImage.Partitions;

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
                containerStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
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
            // TODO: introduce a factory system that tries and then fails as required, like file systems have

            Content.Seek(0x1FE, SeekOrigin.Begin);

            byte[] signatureBytes = new byte[2];
            Content.Read(signatureBytes);
            ushort signature = BinaryPrimitives.ReadUInt16LittleEndian(signatureBytes);

            if (signature != 0xaa55)
            {
                if (Content.Length >= 0x8800)
                {
                    Content.Seek(0x8001, SeekOrigin.Begin);

                    byte[] identifier = new byte[5];
                    Content.Read(identifier);

                    if (OpticalPartitionTable.Identifiers.Any(e => e.SequenceEqual(identifier)))
                    {
                        return new OpticalPartitionTable(this);
                    }

                    Content.Seek(0x8009, SeekOrigin.Begin);
                    Content.Read(identifier);

                    if (identifier.SequenceEqual(IsoVolumeDescriptor.HsfStandardIdentifier))
                    {
                        return new OpticalPartitionTable(this);
                    }
                }

                return new NoPartitionTable(this);
            }

            MbrPartitionTable mbrPartition = new MbrPartitionTable(this);
            if (mbrPartition.Partitions.Count >= 1)
            {
                if (mbrPartition.Partitions[0] is MbrPartitionTable.MbrPartitionEntry entry
                    && (entry.Offset + entry.Length) > uint.MaxValue
                    && entry.Type == MbrPartitionTable.MbrPartitionType.GptProtectivePartition)
                {
                    return new GptPartitionTable(this);
                }

                // check partitions seem fine (ie, no overlapping)
                bool sanity = true;
                long lastOffset = 512;
                foreach (var partition in mbrPartition.Partitions)
                {
                    sanity &= (partition.Offset >= lastOffset);
                    sanity &= (partition.Length > 0);
                    lastOffset = partition.Offset + partition.Length;
                    sanity &= (lastOffset <= Content.Length);
                }

                if (sanity)
                {
                    return mbrPartition;
                }
            }

            return new NoPartitionTable(this);
        }

        /// <summary>
        /// Save out the container to a file
        /// </summary>
        /// <param name="path">The path to save out the image to</param>
        public void SaveImage(string path)
        {
            string? tempPath = Path.ChangeExtension(path, ".tmp");
            using FileStream outStream = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None);
            containerStream.Position = 0;
            containerStream.CopyTo(outStream);
            outStream.Flush();
            outStream.Dispose();
            Dispose();
            File.Move(tempPath, path, true);
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
