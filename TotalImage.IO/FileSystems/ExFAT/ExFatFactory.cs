using System;
using System.Buffers.Binary;
using System.IO;

namespace TotalImage.FileSystems.ExFAT;

public class ExFatFactory : IFileSystemFactory
{
    public FileSystem? TryLoadFileSystem(Stream stream)
    {
        stream.Position = 108; // Bytes Per Sector Shift

        var bytesPerSector = 1 << stream.ReadByte();
        var bootRegion = new byte[bytesPerSector * 11];

        stream.Position = 0;
        stream.Read(bootRegion);

        var checksum = new byte[4];
        stream.Position = bootRegion.Length;
        stream.Read(checksum);

        if (CalculateBootChecksum(bootRegion) == BinaryPrimitives.ReadUInt32LittleEndian(checksum))
        {
            return new ExFatFileSystem(stream);
        }

        return null;
    }

    private uint CalculateBootChecksum(in ReadOnlySpan<byte> sectors)
    {
        var checksum = 0u;

        for (var i = 0; i < sectors.Length; i++)
        {
            if ((i == 106) || (i == 107) || (i == 112))
            {
                continue;
            }
            checksum = ((checksum & 1) != 0 ? 0x80000000 : 0) + (checksum >> 1) + (uint)sectors[i];
        }

        return checksum;
    }
}
