using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using TotalImage.Containers;
using TotalImage.FileSystems;
using TotalImage.FileSystems.ISO;

namespace TotalImage.Partitions
{
    /// <summary>
    /// A partition table class that represents an optical media type setup
    /// where partitions overlay each other rather than exist sequentially
    /// </summary>
    public class OpticalPartitionTable : PartitionTable
    {
        private static readonly ImmutableArray<ImmutableArray<byte>> _identifiers = (new ImmutableArray<byte>[]
        {
            IsoVolumeDescriptor.IsoStandardIdentifier
        }).ToImmutableArray();

        /// <summary>
        /// A list of valid optical media file system identifiers
        /// </summary>
        public static ImmutableArray<ImmutableArray<byte>> Identifiers => _identifiers;

        /// <inheritdoc />
        public OpticalPartitionTable(Container container) : base(container)
        {
        }

        /// <inheritdoc />
        public override string DisplayName => "Optical Media";

        /// <inheritdoc />
        protected override IEnumerable<PartitionEntry> LoadPartitions()
        {
            var builder = ImmutableList.CreateBuilder<PartitionEntry>();


            long nextOffset = 0x8000;
            byte[] recordBytes = new byte[2048];

            bool noValidIdentifier = false;
            do
            {
                _container.Content.Seek(nextOffset, SeekOrigin.Begin);
                _container.Content.Read(recordBytes);

                IsoVolumeDescriptor? record = IsoVolumeDescriptor.ReadVolumeDescriptor(recordBytes);
                if (record != null)
                {
                    if (record is IsoPrimaryVolumeDescriptor pvd && (pvd.IsJolietVolumeDescriptor || record is not IsoSecondaryVolumeDescriptor))
                    {
                        builder.Add(new OpticalFileSystemLayer(new Iso9660FileSystem(_container.Content, pvd), _container.Content));
                    }

                    nextOffset += 0x800;
                    continue;
                }

                // if neither the last block nor this block was valid, then stop searching
                if (noValidIdentifier)
                {
                    break;
                }

                noValidIdentifier = true;
                nextOffset += 0x800;
            }
            while (_container.Content.Length > nextOffset);

            return builder.ToImmutable();
        }
    }

    /// <summary>
    /// A file system layer on optical media
    /// </summary>
    public class OpticalFileSystemLayer : PartitionEntry
    {
        private readonly FileSystem _fileSystem;

        /// <summary>
        /// Initialises an optical media file system layer
        /// </summary>
        /// <param name="fs">The file system of the layer</param>
        /// <param name="stream">The stream containing the partition data</param>
        public OpticalFileSystemLayer(FileSystem fs, Stream stream) : base(0, stream.Length, stream)
        {
            _fileSystem = fs;
        }

        /// <inheritdoc />
        public override FileSystem FileSystem
        {
            get
            {
                return _fileSystem;
            }
        }
    }
}
