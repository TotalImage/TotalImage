using System;
using System.IO;
using System.Text;

namespace TotalImage.FileSystems.UDF
{
    /// <summary>
    /// A factory class that can create a UDF file system.
    /// </summary>
    public class UdfFactory : IFileSystemFactory
    {
        private static readonly byte[] _beaIdentifier = Encoding.ASCII.GetBytes("BEA01");
        private static readonly byte[] _teaIdentifier = Encoding.ASCII.GetBytes("TEA01");
        private static readonly byte[] _nsr02Identifier = Encoding.ASCII.GetBytes("NSR02");
        private static readonly byte[] _nsr03Identifier = Encoding.ASCII.GetBytes("NSR03");

        /// <inheritdoc />
        public FileSystem? TryLoadFileSystem(Stream stream)
        {
            if (stream.Length < (257L * UdfUtilities.VolumeStructureDescriptorSize))
            {
                return null;
            }

            bool sawBeginningExtendedArea = false;
            bool sawNsrDescriptor = false;
            Span<byte> descriptor = stackalloc byte[UdfUtilities.VolumeStructureDescriptorSize];

            for (int sector = 16; sector < 64; sector++)
            {
                stream.Position = sector * UdfUtilities.VolumeStructureDescriptorSize;
                stream.ReadExactly(descriptor);

                ReadOnlySpan<byte> identifier = descriptor.Slice(1, 5);
                if (identifier.SequenceEqual(_beaIdentifier))
                {
                    sawBeginningExtendedArea = true;
                    continue;
                }

                if (identifier.SequenceEqual(_teaIdentifier))
                {
                    break;
                }

                if (identifier.SequenceEqual(_nsr02Identifier) || identifier.SequenceEqual(_nsr03Identifier))
                {
                    sawNsrDescriptor = true;
                }
            }

            if (!sawBeginningExtendedArea || !sawNsrDescriptor)
            {
                return null;
            }

            bool sawAnchor = false;
            foreach (uint anchorBlock in UdfUtilities.GetAnchorCandidateBlocks(stream.Length, UdfUtilities.VolumeStructureDescriptorSize))
            {
                stream.Position = anchorBlock * (long)UdfUtilities.VolumeStructureDescriptorSize;
                stream.ReadExactly(descriptor);

                ushort tagIdentifier = UdfUtilities.ReadTagIdentifier(descriptor);
                if (tagIdentifier == UdfUtilities.AnchorVolumeDescriptorPointer)
                {
                    sawAnchor = true;
                    break;
                }
            }

            if (!sawAnchor)
            {
                return null;
            }

            return new UdfFileSystem(stream);
        }
    }
}
