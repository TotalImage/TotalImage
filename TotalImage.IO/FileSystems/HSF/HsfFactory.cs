using System;
using System.IO;
using System.Linq;

namespace TotalImage.FileSystems.HSF
{
    /// <summary>
    /// A factory class that can create an ISO 9660 file system
    /// </summary>
    public class HsfFactory : IFileSystemFactory
    {
        /// <inheritdoc />
        public FileSystem? TryLoadFileSystem(Stream stream)
        {
            long nextOffset = 0x8009;
            byte[] identifier = new byte[5];

            do
            {
                stream.Seek(nextOffset, SeekOrigin.Begin);
                stream.Read(identifier);

                if (identifier.SequenceEqual(HsfVolumeDescriptor.StandardIdentifier))
                {
                    // trim off any leading blocks in this image

                    var partialStreamStart = nextOffset - 0x8009;
                    return partialStreamStart == 0
                        ? new HighSierraFileSystem(stream)
                        : new HighSierraFileSystem(new PartialStream(stream, partialStreamStart, stream.Length - partialStreamStart));
                }

                nextOffset += 0x800;
            }
            while (stream.Length > nextOffset);

            return null;
        }
    }
}