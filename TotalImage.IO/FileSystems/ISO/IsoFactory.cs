using System;
using System.IO;
using System.Linq;

namespace TotalImage.FileSystems.ISO
{
    /// <summary>
    /// A factory class that can create an ISO 9660 file system
    /// </summary>
    public class IsoFactory : IFileSystemFactory
    {
        /// <inheritdoc />
        public FileSystem? TryLoadFileSystem(Stream stream)
        {
            long nextOffset = 0x8001;
            byte[] identifier = new byte[5];

            do
            {
                stream.Seek(nextOffset, SeekOrigin.Begin);
                stream.Read(identifier);

                if (identifier.SequenceEqual(IsoVolumeDescriptor.StandardIdentifier))
                {
                    // trim off any leading blocks in this image

                    var partialStreamStart = nextOffset - 0x8001;
                    return partialStreamStart == 0
                        ? new Iso9660FileSystem(stream)
                        : new Iso9660FileSystem(new PartialStream(stream, partialStreamStart, stream.Length - partialStreamStart));
                }

                nextOffset += 0x800;
            }
            while (stream.Length > nextOffset);

            return null;
        }
    }
}