using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace TotalImage.FileSystems.ISO
{
    /*
     * The ISO 9660 implementation here is based on the ECMA-119 specification, 4th edition
     * https://www.ecma-international.org/wp-content/uploads/ECMA-119_4th_edition_june_2019.pdf
     * A copy has also been included in the TotalImage repository, under the docs folder.
     *
     * High Sierra implementation is based on the differences mentioned here:
     * http://preserve.mactech.com/articles/develop/issue_03/high_sierra.html
     * and the structures described here:
     * https://386bsd.org/releases/inside-the-iso9660-filesystem-format-untangling-cdrom-standards-article
     */

    /// <summary>
    /// Representation of an ISO 9660 or High Sierra file system
    /// </summary>
    public class Iso9660FileSystem : FileSystem
    {
        /// <summary>
        /// The volume descriptors for the ISO 9660 or High Sierra file system
        /// </summary>
        public ImmutableArray<IsoVolumeDescriptor> VolumeDescriptors { get; }

        /// <summary>
        /// The first primary volume descriptor
        /// </summary>
        public IsoPrimaryVolumeDescriptor PrimaryVolumeDescriptor
            => VolumeDescriptors
                .OfType<IsoPrimaryVolumeDescriptor>()
                .OrderByDescending(e => e.IsJolietVolumeDescriptor)
                .ThenByDescending(e => e.Type == IsoVolumeDescriptorType.PrimaryVolumeDescriptor)
                .First();

        /// <summary>
        /// Create an ISO 9660 or High Sierra file system
        /// </summary>
        /// <param name="containerStream">The underlying stream</param>
        public Iso9660FileSystem(Stream containerStream) : base(containerStream)
        {
            containerStream.Seek(0x8000, SeekOrigin.Begin);

            var volumeDescriptors = ImmutableArray.CreateBuilder<IsoVolumeDescriptor>();

            byte[] recordBytes = new byte[2048];
            do
            {
                containerStream.Read(recordBytes);

                var record = IsoVolumeDescriptor.ReadVolumeDescriptor(recordBytes, this);
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
            while (volumeDescriptors[^1].Type != IsoVolumeDescriptorType.VolumeDescriptorSetTerminator);

            VolumeDescriptors = volumeDescriptors.ToImmutable();

            IsoPrimaryVolumeDescriptor? primaryDescriptor = VolumeDescriptors
                .OfType<IsoPrimaryVolumeDescriptor>()
                .OrderByDescending(e => e.IsJolietVolumeDescriptor)
                .ThenByDescending(e => e.Type == IsoVolumeDescriptorType.PrimaryVolumeDescriptor)
                .FirstOrDefault();
            if (primaryDescriptor == null)
            {
                throw new InvalidDataException("No primary volume descriptor");
            }

            VolumeLabel = !string.IsNullOrEmpty(primaryDescriptor.VolumeIdentifier)
                ? primaryDescriptor.VolumeIdentifier
                : primaryDescriptor.VolumeSetIdentifier;

            RootDirectory = new IsoDirectory(primaryDescriptor.RootDirectory, this);
            TotalSize = primaryDescriptor.LogicalBlockSize * primaryDescriptor.VolumeSpace;
        }

        /// <inheritdoc />
        public override string DisplayName => PrimaryVolumeDescriptor.Identifier.SequenceEqual(IsoVolumeDescriptor.HsfStandardIdentifier) ? "High Sierra" : PrimaryVolumeDescriptor.IsJolietVolumeDescriptor ? "ISO 9660 + Joliet" : "ISO 9660";

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