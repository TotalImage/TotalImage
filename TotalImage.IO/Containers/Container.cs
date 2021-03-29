using System;
using System.IO;
using System.Text;
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
        protected virtual PartitionTable LoadPartitionTable()
        {
            // TODO: introduce a factory system that tries and then fails as required, like file systems have

            using BinaryReader br = new BinaryReader(Content, Encoding.ASCII, true);
            br.BaseStream.Seek(0x1FE, SeekOrigin.Begin);
            var signature = br.ReadUInt16();

            if (signature != 0xaa55)
            {
                return new NoPartitionTable(this);
            }

            var mbrPartition = new MbrPartitionTable(this);
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
                    sanity &= (lastOffset <= br.BaseStream.Length);
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

        /// <summary>
        /// The display name of the container.
        /// </summary>
        public abstract string DisplayName { get; }
    }
}
