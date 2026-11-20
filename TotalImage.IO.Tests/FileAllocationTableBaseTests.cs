using System;
using System.Buffers.Binary;
using System.IO;
using System.Linq;
using TotalImage.FileSystems;
using TotalImage.FileSystems.ExFAT;
using TotalImage.FileSystems.FAT;
using Xunit;

namespace TotalImage.IO.Tests;

public class FileAllocationTableBaseTests
{
    // 512-byte sectors, FAT at sector 1 (one sector = 128 entries), cluster heap at sector 2.
    private static (byte[] image, ExFatFileAllocationTable fat) CreateExFat()
    {
        var image = new byte[4096];
        BinaryPrimitives.WriteUInt32LittleEndian(image.AsSpan(80, 4), 1);  // FatOffset
        BinaryPrimitives.WriteUInt32LittleEndian(image.AsSpan(84, 4), 1);  // FatLength
        BinaryPrimitives.WriteUInt32LittleEndian(image.AsSpan(88, 4), 2);  // ClusterHeapOffset
        BinaryPrimitives.WriteUInt32LittleEndian(image.AsSpan(92, 4), 4);  // ClusterCount
        image[108] = 9;
        image[110] = 1;
        return (image, new ExFatFileSystem(new MemoryStream(image)).ActiveFat);
    }

    private static void SetExFatEntry(byte[] image, uint index, uint value)
        => BinaryPrimitives.WriteUInt32LittleEndian(image.AsSpan(512 + (int)index * 4, 4), value);

    [Fact]
    public void ExFat_LengthIsClampedToClusterCount()
    {
        var (_, fat) = CreateExFat();
        Assert.Equal(4u + 2, fat.Length);
    }

    [Theory]
    [InlineData(0u, ClusterType.Invalid)]
    [InlineData(1u, ClusterType.Invalid)]
    [InlineData(0xFFFFFFF7u, ClusterType.Bad)]
    [InlineData(0xFFFFFFF8u, ClusterType.Invalid)]
    [InlineData(0xFFFFFFFCu, ClusterType.Invalid)]
    [InlineData(0xFFFFFFFEu, ClusterType.Invalid)]
    [InlineData(0xFFFFFFFFu, ClusterType.EndOfChain)]
    [InlineData(2u, ClusterType.Data)]
    [InlineData(5u, ClusterType.Data)]            // last cluster (ClusterCount + 1)
    [InlineData(6u, ClusterType.Invalid)]         // past the last cluster
    [InlineData(0x0FFFFFF8u, ClusterType.Invalid)]
    [InlineData(0xFFFFFFF6u, ClusterType.Invalid)]
    public void ExFat_ClassifiesSpecialValues(uint value, ClusterType expected)
    {
        var (_, fat) = CreateExFat();
        Assert.Equal(expected, fat.GetClusterType(value));
        Assert.Equal(expected == ClusterType.EndOfChain, fat.IsEndOfChainMarker(value));
    }

    [Fact]
    public void ExFat_ChainStopsAtEndOfChain()
    {
        var (image, fat) = CreateExFat();
        SetExFatEntry(image, 2, 3);
        SetExFatEntry(image, 3, 5);
        SetExFatEntry(image, 5, 0xFFFFFFFF);

        Assert.Equal(new uint[] { 2, 3, 5 }, fat.GetClusterChain(2));
        Assert.Equal(3u, fat.GetNextCluster(2));
        Assert.Null(fat.GetNextCluster(5));
    }

    [Theory]
    [InlineData(0u)]          // free
    [InlineData(1u)]          // invalid
    [InlineData(0xFFFFFFF7u)] // bad
    [InlineData(0xFFFFFFF8u)] // invalid
    [InlineData(0xFFFFFFFEu)] // invalid
    [InlineData(6u)]          // first index past ClusterCount + 2
    [InlineData(100u)]        // inside the FAT sector's slack, past the volume's clusters
    [InlineData(0xFFFFFFF6u)] // far out of range
    public void ExFat_ChainThrowsOnInvalidLink(uint value)
    {
        var (image, fat) = CreateExFat();
        SetExFatEntry(image, 2, 3);
        SetExFatEntry(image, 3, value);

        Assert.Throws<InvalidDataException>(() => fat.GetClusterChain(2));
        Assert.Throws<InvalidDataException>(() => fat.GetNextCluster(3));
    }

    [Theory]
    [InlineData(0u)]
    [InlineData(1u)]
    [InlineData(6u)]
    public void ExFat_ChainThrowsOnInvalidFirstCluster(uint first)
    {
        var (_, fat) = CreateExFat();
        Assert.Throws<InvalidDataException>(() => fat.GetClusterChain(first));
    }

    [Fact]
    public void ExFat_CyclicChainThrows()
    {
        var (image, fat) = CreateExFat();
        SetExFatEntry(image, 2, 3);
        SetExFatEntry(image, 3, 2);
        Assert.Throws<InvalidDataException>(() => fat.GetClusterChain(2));
    }

    [Fact]
    public void ExFat_EnumeratorAndIndexersReadThroughBase()
    {
        var (image, fat) = CreateExFat();
        SetExFatEntry(image, 1, 0xAA);
        SetExFatEntry(image, 2, 0xBB);
        FileAllocationTableBase table = fat;

        Assert.Equal(0xBBu, table[2]);
        Assert.Equal(0xBBu, table[2u]);
        var values = table.ToArray();
        // Existing enumerator behaviour: first MoveNext skips index 0.
        Assert.Equal(5, values.Length);
        Assert.Equal(0xAAu, values[0]);
        Assert.Equal(0xBBu, values[1]);
    }

    /// <summary>
    /// Builds an exFAT volume whose root directory (cluster 2) holds only an allocation bitmap
    /// (cluster 3, contiguous) marking clusters 2 and 3 as used.
    /// </summary>
    private static ExFatFileSystem CreateExFatWithBitmap(uint clusterCount)
    {
        var image = new byte[(2 + clusterCount) * 512];
        BinaryPrimitives.WriteUInt32LittleEndian(image.AsSpan(80, 4), 1);  // FatOffset
        BinaryPrimitives.WriteUInt32LittleEndian(image.AsSpan(84, 4), 1);  // FatLength
        BinaryPrimitives.WriteUInt32LittleEndian(image.AsSpan(88, 4), 2);  // ClusterHeapOffset
        BinaryPrimitives.WriteUInt32LittleEndian(image.AsSpan(92, 4), clusterCount);
        BinaryPrimitives.WriteUInt32LittleEndian(image.AsSpan(96, 4), 2);  // FirstClusterOfRootDirectory
        image[108] = 9;
        image[110] = 1;
        SetExFatEntry(image, 2, 0xFFFFFFFF);

        uint bitmapLength = (clusterCount + 7) / 8;
        var root = image.AsSpan(1024, 32);
        root[0] = 0x81; // Allocation bitmap, first bitmap
        BinaryPrimitives.WriteUInt32LittleEndian(root[20..24], 3);
        BinaryPrimitives.WriteUInt64LittleEndian(root[24..32], bitmapLength);

        // Clusters 2 and 3 are allocated; every other bit, including any padding bits past
        // ClusterCount in the last byte, is clear.
        image[1536] = 0b0000_0011;
        return new ExFatFileSystem(new MemoryStream(image));
    }

    [Theory]
    [InlineData(16u)] // multiple of 8
    [InlineData(13u)] // not a multiple of 8
    [InlineData(9u)]
    public void ExFat_TotalFreeSpaceCountsExactlyClusterCountBits(uint clusterCount)
    {
        var fs = CreateExFatWithBitmap(clusterCount);
        Assert.Equal((clusterCount - 2) * 512L, fs.TotalFreeSpace);
    }
}
