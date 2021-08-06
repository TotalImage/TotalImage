using System;
using System.Buffers.Binary;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Security.Cryptography;
using TotalImage.Partitions;

namespace TotalImage.Containers
{
    /// <summary>
    /// An abstract representation of container files
    /// </summary>
    public abstract class Container : IDisposable
    {
        /// <summary>
        /// The backing file containing the image, opened as a memory-mapped file
        /// </summary>
        protected readonly MemoryMappedFile? backingFile;

        /// <summary>
        /// The underlying stream containing the image
        /// </summary>
        protected Stream containerStream;

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
        /// <param name="memoryMapping">Should the file be mapped into memory</param>
        protected Container(string path, bool memoryMapping)
        {
            if (memoryMapping)
            {
                backingFile = MemoryMappedFile.CreateFromFile(path, FileMode.Open);
                containerStream = backingFile.CreateViewStream(0, 0, MemoryMappedFileAccess.Read);
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
        /// Load the file system from the container image
        /// </summary>
        /// <returns>The file system found on the image</returns>
        protected virtual PartitionTable LoadPartitionTable()
        {
            // TODO: introduce a factory system that tries and then fails as required, like file systems have

            Content.Seek(0x1FE, SeekOrigin.Begin);

            byte[] signatureBytes = new byte[2];
            Content.Read(signatureBytes);
            ushort signature = BinaryPrimitives.ReadUInt16LittleEndian(signatureBytes);

            if (signature != 0xaa55)
            {
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

        /// <summary>
        /// The display name of the container.
        /// </summary>
        public abstract string DisplayName { get; }

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
    }
}
