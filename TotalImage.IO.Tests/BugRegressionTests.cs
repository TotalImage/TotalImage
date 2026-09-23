using System;
using System.Buffers.Binary;
using System.IO;
using System.Linq;
using System.Text;
using TotalImage.Containers.VHD;
using TotalImage.FileSystems.ExFAT;
using TotalImage.FileSystems.ISO;
using Xunit;

namespace TotalImage.IO.Tests;

public class BugRegressionTests
{
    [Fact]
    public void PartialStream_AllowsWriteEndingAtBoundary()
    {
        using var backing = new MemoryStream(new byte[10]);
        using var slice = new PartialStream(backing, 2, 4);
        slice.Position = 2;
        slice.Write(new byte[] { 7, 8 });
        Assert.Equal(new byte[] { 0, 0, 0, 0, 7, 8, 0, 0, 0, 0 }, backing.ToArray());
        Assert.Throws<ArgumentOutOfRangeException>(() => slice.WriteByte(9));
    }

    [Fact]
    public void VhdFooter_UsesActualDiskGeometry()
    {
        var footer = new VhdFooter(10UL * 1024 * 1024);
        Assert.True(footer.DiskCylinders < 1000);
        Assert.True(footer.DiskHeads <= 16);
    }

    [Fact]
    public void DynamicVhd_ReadsSparseAndAllocatedBlocksAcrossBoundaries()
    {
        const int blockSize = 4096;
        byte[] image = new byte[512 + 1024 + 512 + 2 * (512 + blockSize) + 512];
        byte[] footer = new VhdFooter(4 * blockSize).GetByteSpan().ToArray();
        BinaryPrimitives.WriteUInt64BigEndian(footer.AsSpan(16, 8), 512);
        BinaryPrimitives.WriteUInt32BigEndian(footer.AsSpan(60, 4), 3);
        UpdateChecksum(footer, 64);
        footer.CopyTo(image, 0);
        footer.CopyTo(image, image.Length - 512);
        Span<byte> header = image.AsSpan(512, 1024);
        Encoding.ASCII.GetBytes("cxsparse").CopyTo(header);
        header.Slice(8, 8).Fill(0xFF);
        BinaryPrimitives.WriteUInt64BigEndian(header[16..24], 1536);
        BinaryPrimitives.WriteUInt32BigEndian(header[24..28], 0x00010000);
        BinaryPrimitives.WriteUInt32BigEndian(header[28..32], 4);
        BinaryPrimitives.WriteUInt32BigEndian(header[32..36], blockSize);
        UpdateChecksum(header, 36);
        // BAT occupies one sector; blocks 1 and 3 are allocated, 0 and 2 are sparse.
        image.AsSpan(1536, 512).Fill(0xFF);
        BinaryPrimitives.WriteUInt32BigEndian(image.AsSpan(1540, 4), 4);
        BinaryPrimitives.WriteUInt32BigEndian(image.AsSpan(1548, 4), 13);
        image.AsSpan(5 * 512, blockSize).Fill((byte)'A');
        image.AsSpan(14 * 512, blockSize).Fill((byte)'B');
        string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".vhd");
        try
        {
            System.IO.File.WriteAllBytes(path, image);
            using var vhd = new VhdContainer(path, false);
            var buffer = Enumerable.Repeat((byte)0xCC, 4 * blockSize + 5).ToArray();
            Assert.Equal(4 * blockSize, vhd.Content.Read(buffer, 5, 4 * blockSize));
            Assert.All(buffer.AsSpan(5, blockSize).ToArray(), b => Assert.Equal((byte)0, b));
            Assert.All(buffer.AsSpan(5 + blockSize, blockSize).ToArray(), b => Assert.Equal((byte)'A', b));
            Assert.All(buffer.AsSpan(5 + 2 * blockSize, blockSize).ToArray(), b => Assert.Equal((byte)0, b));
            Assert.All(buffer.AsSpan(5 + 3 * blockSize, blockSize).ToArray(), b => Assert.Equal((byte)'B', b));
        }
        finally
        {
            System.IO.File.Delete(path);
        }
    }

    private static void UpdateChecksum(Span<byte> bytes, int checksumOffset)
    {
        bytes.Slice(checksumOffset, 4).Clear();
        uint sum = 0;
        foreach (byte b in bytes) sum += b;
        BinaryPrimitives.WriteUInt32BigEndian(bytes.Slice(checksumOffset, 4), ~sum);
    }

    [Fact]
    public void IsoVolumeTimestamp_AcceptsUtcOffsetZero()
    {
        char[] date = "2026092312000000".ToCharArray().Append('\0').ToArray();
        Assert.Equal(TimeSpan.Zero, IsoUtilities.FromIsoDateTime(date)!.Value.Offset);
    }

    [Fact]
    public void ExFat_StopsAtAnyEndOfChainMarker_AndDetectsCycles()
    {
        var image = new byte[4096];
        BinaryPrimitives.WriteUInt32LittleEndian(image.AsSpan(80, 4), 1);
        BinaryPrimitives.WriteUInt32LittleEndian(image.AsSpan(84, 4), 1);
        image[108] = 9;
        image[110] = 1;
        BinaryPrimitives.WriteUInt32LittleEndian(image.AsSpan(512 + 8, 4), 0xFFFFFFF8);
        var fat = new ExFatFileSystem(new MemoryStream(image)).ActiveFat;
        Assert.Equal(128u, fat.Length);
        Assert.Equal(new uint[] { 2 }, fat.GetClusterChain(2));

        BinaryPrimitives.WriteUInt32LittleEndian(image.AsSpan(512 + 8, 4), 2);
        Assert.Throws<InvalidDataException>(() => fat.GetClusterChain(2));
    }

    [Fact]
    public void ExFatFatChainedStream_ExposesOnlyValidData()
    {
        byte[] image = new byte[4096];
        BinaryPrimitives.WriteUInt32LittleEndian(image.AsSpan(80, 4), 1);
        BinaryPrimitives.WriteUInt32LittleEndian(image.AsSpan(84, 4), 1);
        BinaryPrimitives.WriteUInt32LittleEndian(image.AsSpan(88, 4), 2);
        image[108] = 9;
        image[110] = 1;
        BinaryPrimitives.WriteUInt32LittleEndian(image.AsSpan(512 + 8, 4), 3);
        BinaryPrimitives.WriteUInt32LittleEndian(image.AsSpan(512 + 12, 4), 0xFFFFFFFF);
        image.AsSpan(1024, 512).Fill((byte)'A');
        image.AsSpan(1536, 512).Fill((byte)'B');
        byte[] entry = new byte[32];
        entry[0] = (byte)EntryType.StreamExtension;
        BinaryPrimitives.WriteUInt64LittleEndian(entry.AsSpan(8, 8), 600);
        BinaryPrimitives.WriteUInt32LittleEndian(entry.AsSpan(20, 4), 2);
        BinaryPrimitives.WriteUInt64LittleEndian(entry.AsSpan(24, 8), 1024);
        var fs = new ExFatFileSystem(new MemoryStream(image));
        using var content = new StreamExtensionDirectoryEntry(entry).GetStream(fs);

        Assert.Equal(600, content.Length);
        byte[] bytes = new byte[1024];
        Assert.Equal(600, content.Read(bytes));
        Assert.All(bytes.AsSpan(0, 512).ToArray(), b => Assert.Equal((byte)'A', b));
        Assert.All(bytes.AsSpan(512, 88).ToArray(), b => Assert.Equal((byte)'B', b));
        Assert.Equal(0, content.Read(bytes));
        Assert.Equal(600, content.Seek(0, SeekOrigin.End));
        Assert.Equal(0, content.Read(bytes));
    }

    [Fact]
    public void ExFatDirectoryEnumeration_StopsWhenStreamEndsWithoutMarker()
    {
        using var stream = new MemoryStream(Enumerable.Repeat((byte)0x80, 32).ToArray());
        Assert.Empty(DirectoryEntry.EnumerateDirectory(stream));
    }
}
