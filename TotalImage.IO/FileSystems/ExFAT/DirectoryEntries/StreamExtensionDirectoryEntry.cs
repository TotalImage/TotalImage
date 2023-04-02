using System;
using System.Buffers.Binary;
using System.IO;

namespace TotalImage.FileSystems.ExFAT;

public class StreamExtensionDirectoryEntry : SecondaryDirectoryEntry, IStreamable
{
    public byte NameLength { get; }
    public ushort NameHash { get; }
    public ulong ValidDataLength { get; }

    public StreamExtensionDirectoryEntry(in ReadOnlySpan<byte> entry) : base(entry)
    {
        NameLength = entry[3];
        NameHash = BinaryPrimitives.ReadUInt16LittleEndian(entry[4..6]);
        ValidDataLength = BinaryPrimitives.ReadUInt64LittleEndian(entry[8..16]);
    }

    public Stream GetStream(ExFatFileSystem fileSystem) =>
        GetStreamInternal(fileSystem, (GeneralSecondaryFlags & 0x02) != 0, ValidDataLength);
}
