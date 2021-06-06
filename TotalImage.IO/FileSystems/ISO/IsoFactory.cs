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
            stream.Seek(0x8001, SeekOrigin.Begin);
            byte[] identifier = new byte[5];
            stream.Read(identifier);
            if (identifier.SequenceEqual(IsoVolumeDescriptor.StandardIdentifier))
            {
                return new Iso9660FileSystem(stream);
            }

            return null;
        }
    }
}