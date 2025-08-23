using System;
using System.IO;

namespace TotalImage.FileSystems.IMGFS
{
    public class ImgfsFactory : IFileSystemFactory
    {
        private readonly byte[] signature = [0xf8, 0xac, 0x2c, 0x9d, 0xe3, 0xd4, 0x2b, 0x4d, 0xbd, 0x30, 0x91, 0x6e, 0xd8, 0x4f, 0x31, 0xdc];

        public FileSystem? TryLoadFileSystem(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            Span<byte> buffer = new byte[16];
            stream.Read(buffer);

            if (buffer.SequenceEqual(signature))
            {
                return new CEImageFileSystem(stream);
            }

            return null;
        }
    }
}
