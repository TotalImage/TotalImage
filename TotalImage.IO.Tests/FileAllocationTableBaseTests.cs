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

    [Theory]
    [InlineData(0u, ClusterType.Free)]
    [InlineData(1u, ClusterType.NonFree)]
    [InlineData(0xFFFFFFF7u, ClusterType.Bad)]
    [InlineData(0xFFFFFFF8u, ClusterType.EndOfChain)]
    [InlineData(0xFFFFFFFCu, ClusterType.EndOfChain)]
    [InlineData(0xFFFFFFFFu, ClusterType.EndOfChain)]
    [InlineData(2u, ClusterType.Data)]
    [InlineData(0x0FFFFFF8u, ClusterType.Data)]
    [InlineData(0xFFFFFFF6u, ClusterType.Data)]
    public void ExFat_ClassifiesSpecialValues(uint value, ClusterType expected)
    {
        var (_, fat) = CreateExFat();
        Assert.Equal(expected, fat.GetClusterType(value));
        Assert.Equal(expected != ClusterType.Data, fat.IsEndOfChainMarker(value));
    }

    [Theory]
    [InlineData(0u)]
    [InlineData(1u)]
    [InlineData(0xFFFFFFF7u)]
    [InlineData(0xFFFFFFF8u)]
    [InlineData(0xFFFFFFFFu)]
    public void ExFat_ChainStopsAtTerminator(uint terminator)
    {
        var (image, fat) = CreateExFat();
        SetExFatEntry(image, 2, 3);
        SetExFatEntry(image, 3, 5);
        SetExFatEntry(image, 5, terminator);

        Assert.Equal(new uint[] { 2, 3, 5 }, fat.GetClusterChain(2));
        Assert.Equal(3u, fat.GetNextCluster(2));
        Assert.Null(fat.GetNextCluster(5));
    }

    [Fact]
    public void ExFat_ChainFollowsLargeNonMarkerValue()
    {
        var (image, fat) = CreateExFat();
        SetExFatEntry(image, 2, 0xFFFFFFF6);
        Assert.Equal(0xFFFFFFF6u, fat.GetNextCluster(2));
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
        Assert.Equal(127, values.Length);
        Assert.Equal(0xAAu, values[0]);
        Assert.Equal(0xBBu, values[1]);
    }
}
