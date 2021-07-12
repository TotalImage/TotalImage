using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace TotalImage.FileSystems.HSF
{
    /*
     * Prototype High Sierra implementation, based on our existing ISO 9600 implementation, as per the differences from ISO 9660 described here:
     * http://preserve.mactech.com/articles/develop/issue_03/high_sierra.html
     * and the structures described here:
     * https://386bsd.org/releases/inside-the-iso9660-filesystem-format-untangling-cdrom-standards-article
     */

    /// <summary>
    /// Representation of an ISO 9660 file system
    /// </summary>
    public class HighSierraFileSystem : FileSystem
    {
        /// <summary>
        /// The volume descriptors for the ISO 9660 file system
        /// </summary>
        public ImmutableArray<HsfVolumeDescriptor> VolumeDescriptors { get; }

        /// <summary>
        /// The first primary volume descriptor
        /// </summary>
        public HsfPrimaryVolumeDescriptor PrimaryVolumeDescriptor
            => VolumeDescriptors
                .OfType<HsfPrimaryVolumeDescriptor>()
                .OrderByDescending(e => e.Type == HsfVolumeDescriptorType.PrimaryVolumeDescriptor)
                .First();

        /// <summary>
        /// Create a High Sierra file system
        /// </summary>
        /// <param name="containerStream">The underlying stream</param>
        public HighSierraFileSystem(Stream containerStream) : base(containerStream)
        {
            containerStream.Seek(0x8000, SeekOrigin.Begin);

            var volumeDescriptors = ImmutableArray.CreateBuilder<HsfVolumeDescriptor>();

            byte[] recordBytes = new byte[2048];
            do
            {
                containerStream.Read(recordBytes);

                var record = HsfVolumeDescriptor.ReadVolumeDescriptor(recordBytes, this);
                if (record == null)
                {
                    break;
                }

                if (!record.IsValid())
                {
                    throw new InvalidDataException();
                }

                volumeDescriptors.Add(record);
            }
            while (volumeDescriptors[^1].Type != HsfVolumeDescriptorType.VolumeDescriptorSetTerminator);

            VolumeDescriptors = volumeDescriptors.ToImmutable();

            HsfPrimaryVolumeDescriptor? primaryDescriptor = VolumeDescriptors
                .OfType<HsfPrimaryVolumeDescriptor>()
                .OrderByDescending(e => e.Type == HsfVolumeDescriptorType.PrimaryVolumeDescriptor)
                .FirstOrDefault();
            if (primaryDescriptor == null)
            {
                throw new InvalidDataException("No primary volume descriptor");
            }

            VolumeLabel = !string.IsNullOrEmpty(primaryDescriptor.VolumeIdentifier)
                ? primaryDescriptor.VolumeIdentifier
                : primaryDescriptor.VolumeSetIdentifier;

            RootDirectory = new HsfDirectory(primaryDescriptor.RootDirectory, this);
            TotalSize = primaryDescriptor.LogicalBlockSize * primaryDescriptor.VolumeSpace;
        }

        /// <inheritdoc />
        public override string DisplayName => "High Sierra";

        /// <inheritdoc />
        public override string VolumeLabel { get; set; }

        /// <inheritdoc />
        public override Directory RootDirectory { get; }

        /// <inheritdoc />
        public override long TotalFreeSpace => 0;

        /// <inheritdoc />
        public override long TotalSize { get; }

        /// <inheritdoc />
        public override uint AllocationUnitSize => PrimaryVolumeDescriptor.LogicalBlockSize;
    }
}