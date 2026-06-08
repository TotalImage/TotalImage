using System;
using System.Buffers.Binary;
using System.IO;
using System.Linq;
using System.Text;
using TotalImage.Containers;
using TotalImage.Partitions;
using Xunit;

namespace TotalImage.IO.Tests.Partitions;

public class PartitionTableTests
{
    [Fact]
    public void AttemptDetection_ReturnsNoPartitionTableForBlankImage()
    {
        using var container = new RawContainer(new MemoryStream(new byte[1024]));

        var table = container.PartitionTable;

        var noPartitionTable = Assert.IsType<NoPartitionTable>(table);
        Assert.Equal("Unpartitioned", noPartitionTable.DisplayName);
        Assert.True(noPartitionTable.SupportsWriting);

        var partition = Assert.Single(noPartitionTable.Partitions);
        Assert.Equal(0, partition.Offset);
        Assert.Equal(1024, partition.Length);
    }

    [Fact]
    public void AttemptDetection_ReturnsMbrPartitionTableForBasicMbrImage()
    {
        using var container = new RawContainer(new MemoryStream(CreateMbrImage()));

        var table = container.PartitionTable;

        var mbr = Assert.IsType<MbrPartitionTable>(table);
        Assert.Equal("Master Boot Record", mbr.DisplayName);
        Assert.True(mbr.SupportsWriting);

        var partition = Assert.Single(mbr.Partitions);
        var entry = Assert.IsType<MbrPartitionTable.MbrPartitionEntry>(partition);
        Assert.True(entry.Active);
        Assert.Equal(MbrPartitionTable.MbrPartitionType.Fat32, entry.Type);
        Assert.Equal(1u, entry.LbaStart);
        Assert.Equal(2u, entry.LbaLength);
        Assert.Equal(512L, entry.Offset);
        Assert.Equal(1024L, entry.Length);
    }

    [Fact]
    public void AttemptDetection_ReturnsGptPartitionTableForProtectiveMbr()
    {
        using var container = new RawContainer(new MemoryStream(CreateGptImage()));

        var table = new GptPartitionTable(container);

        var gpt = Assert.IsType<GptPartitionTable>(table);
        Assert.Equal("GUID Partition Table", gpt.DisplayName);
        Assert.True(gpt.SupportsWriting);

        _ = gpt.Partitions;

        Assert.NotNull(gpt.Header);
        Assert.Equal("EFI PART", gpt.Header!.Signature);

        var partition = Assert.Single(gpt.Partitions);
        var entry = Assert.IsType<GptPartitionTable.GptPartitionEntry>(partition);
        Assert.Equal(Guid.Parse("C12A7328-F81F-11D2-BA4B-00A0C93EC93B"), entry.TypeId);
        Assert.Equal(3UL, entry.FirstLBA);
        Assert.Equal(4UL, entry.LastLBA);
        Assert.Equal(1536L, entry.Offset);
        Assert.Equal(1024L, entry.Length);
        Assert.Equal("ESP", entry.Name.TrimEnd('\0'));
    }

    private static byte[] CreateMbrImage()
    {
        byte[] image = new byte[1024];
        Span<byte> bootSector = image.AsSpan(0, 512);
        bootSector[0x1BE] = 0x80;
        bootSector[0x1BE + 4] = (byte)MbrPartitionTable.MbrPartitionType.Fat32;
        BinaryPrimitives.WriteUInt32LittleEndian(bootSector[(0x1BE + 8)..(0x1BE + 12)], 1);
        BinaryPrimitives.WriteUInt32LittleEndian(bootSector[(0x1BE + 12)..(0x1BE + 16)], 2);
        BinaryPrimitives.WriteUInt16LittleEndian(bootSector[510..512], 0xAA55);
        return image;
    }

    private static byte[] CreateGptImage()
    {
        byte[] image = new byte[2560];

        Span<byte> mbr = image.AsSpan(0, 512);
        mbr[0x1BE] = 0x00;
        mbr[0x1BE + 4] = (byte)MbrPartitionTable.MbrPartitionType.GptProtectivePartition;
        BinaryPrimitives.WriteUInt32LittleEndian(mbr[(0x1BE + 8)..(0x1BE + 12)], 1);
        BinaryPrimitives.WriteUInt32LittleEndian(mbr[(0x1BE + 12)..(0x1BE + 16)], 0xFFFFFFFF);
        BinaryPrimitives.WriteUInt16LittleEndian(mbr[510..512], 0xAA55);

        Span<byte> header = image.AsSpan(512, 92);
        Encoding.ASCII.GetBytes("EFI PART").CopyTo(header);
        BinaryPrimitives.WriteUInt32LittleEndian(header[8..12], 0x00010000);
        BinaryPrimitives.WriteUInt32LittleEndian(header[12..16], 92);
        BinaryPrimitives.WriteUInt64LittleEndian(header[24..32], 1);
        BinaryPrimitives.WriteUInt64LittleEndian(header[32..40], 4);
        BinaryPrimitives.WriteUInt64LittleEndian(header[40..48], 34);
        BinaryPrimitives.WriteUInt64LittleEndian(header[48..56], 200);
        header[56..72].Fill(0x11);
        BinaryPrimitives.WriteUInt64LittleEndian(header[72..80], 2);
        BinaryPrimitives.WriteUInt32LittleEndian(header[80..84], 1);
        BinaryPrimitives.WriteUInt32LittleEndian(header[84..88], 128);

        Span<byte> entry = image.AsSpan(1024, 128);
        Guid.Parse("C12A7328-F81F-11D2-BA4B-00A0C93EC93B").TryWriteBytes(entry[0..16]);
        Guid.Parse("01234567-89AB-CDEF-0123-456789ABCDEF").TryWriteBytes(entry[16..32]);
        BinaryPrimitives.WriteUInt64LittleEndian(entry[32..40], 3);
        BinaryPrimitives.WriteUInt64LittleEndian(entry[40..48], 4);
        Encoding.Unicode.GetBytes("ESP").CopyTo(entry[56..]);

        return image;
    }
}
