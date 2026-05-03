using System;
using System.Buffers.Binary;
using System.IO;
using System.Text;

namespace TotalImage.FileSystems.NTFS;

/// <summary>
/// Creates NTFS file system instances from streams.
/// </summary>
public class NtfsFactory : IFileSystemFactory
{
    private static readonly byte[] OemId = Encoding.ASCII.GetBytes("NTFS    ");

    /// <inheritdoc />
    public FileSystem? TryLoadFileSystem(Stream stream)
    {
        if (stream.Length < 512)
        {
            return null;
        }

        byte[] bootSector = new byte[512];
        stream.Position = 0;
        stream.ReadExactly(bootSector);

        if (!bootSector.AsSpan(3, OemId.Length).SequenceEqual(OemId))
        {
            return null;
        }

        if (BinaryPrimitives.ReadUInt16LittleEndian(bootSector.AsSpan(510, 2)) != 0xAA55)
        {
            return null;
        }

        return new NtfsFileSystem(stream);
    }
}
