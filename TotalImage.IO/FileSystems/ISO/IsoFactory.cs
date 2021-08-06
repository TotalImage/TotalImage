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

                //First check if this might be ISO 9660
                if (identifier.SequenceEqual(IsoVolumeDescriptor.IsoStandardIdentifier))
                {
                    // trim off any leading blocks in this image

                    long partialStreamStart = nextOffset - 0x8001;
                    return partialStreamStart == 0
                        ? new Iso9660FileSystem(stream)
                        : new Iso9660FileSystem(new PartialStream(stream, partialStreamStart, stream.Length - partialStreamStart));
                }
                else
                {
                    //If not, check for High Sierra instead
                    nextOffset += 8;
                    stream.Seek(nextOffset, SeekOrigin.Begin);
                    stream.Read(identifier);

                    if (identifier.SequenceEqual(IsoVolumeDescriptor.HsfStandardIdentifier))
                    {
                        long partialStreamStart = nextOffset - 0x8009;
                        return partialStreamStart == 0
                            ? new Iso9660FileSystem(stream)
                            : new Iso9660FileSystem(new PartialStream(stream, partialStreamStart, stream.Length - partialStreamStart));
                    }
                    else
                    {
                        nextOffset -= 8;
                    }
                }

                nextOffset += 0x800;
            }
            while (stream.Length > nextOffset);

            return null;
        }
    }
}
