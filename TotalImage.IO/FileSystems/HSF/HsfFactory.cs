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
            stream.Seek(0x8009, SeekOrigin.Begin);
            byte[] identifier = new byte[5];
            stream.Read(identifier);
            if (identifier.SequenceEqual(HsfVolumeDescriptor.StandardIdentifier))
            {
                return new HighSierraFileSystem(stream);
            }

            return null;
        }
    }
}