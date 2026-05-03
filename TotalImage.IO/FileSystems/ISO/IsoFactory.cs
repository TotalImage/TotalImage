using System;
using System.IO;
using System.Linq;
using TotalImage.FileSystems.UDF;

namespace TotalImage.FileSystems.ISO
{
    /// <summary>
    /// A factory class that can create an ISO 9660 file system
    /// </summary>
    public class IsoFactory : IFileSystemFactory
    {
        private readonly UdfFactory _udfFactory = new();

        /// <inheritdoc />
        public FileSystem? TryLoadFileSystem(Stream stream)
        {
            long nextOffset = 0x8001;
            byte[] identifierBytes = new byte[13];

            //Although the primary volume descriptor must be located at offset 0x8000 (sector 17, counting from 1), there are images out there (bad dumps?)
            //where it's further into the file, so we try to catch such cases with this loop by looking for the descriptor's identifier for a bit before
            //giving up at 512 KiB. We might also want to eventually check the volume descriptor ID at this step already - should be 0x01 in this case.
            do
            {
                stream.Seek(nextOffset, SeekOrigin.Begin);
                stream.Read(identifierBytes);

                //First check if this might be ISO 9660 or High Sierra
                if (identifierBytes[0..5].SequenceEqual(IsoVolumeDescriptor.IsoStandardIdentifier) || identifierBytes[8..13].SequenceEqual(IsoVolumeDescriptor.HsfStandardIdentifier))
                {
                    // Hybrid optical media commonly contains both ISO 9660 and UDF metadata.
                    // Prefer UDF when present because it exposes the richer file system view.
                    FileSystem? udfFileSystem = _udfFactory.TryLoadFileSystem(stream);
                    if (udfFileSystem != null)
                    {
                        return udfFileSystem;
                    }

                    // trim off any leading blocks in this image - e.g. if there's more than 32 KiB of system area before the primary volume descriptor,
                    // ditch them because that's out of spec and probably the result of a bad dump
                    long partialStreamStart = nextOffset - 0x8001;
                    Stream candidateStream = partialStreamStart == 0
                        ? stream
                        : new PartialStream(stream, partialStreamStart, stream.Length - partialStreamStart);

                    return new Iso9660FileSystem(candidateStream);
                }

                nextOffset += 0x800; //If it's neither, just seek to the next sector and try again
            }
            while (nextOffset < stream.Length && nextOffset < 0x80000); //Finally, stop at 512 KiB because it's effectively pointless to look further

            return null;
        }
    }
}
