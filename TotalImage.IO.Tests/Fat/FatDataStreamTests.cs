using System;
using System.IO;
using System.Linq;
using System.Text;
using TotalImage.FileSystems;
using TotalImage.FileSystems.FAT;
using Xunit;

namespace TotalImage.IO.Tests.Fat;

public class FatDataStreamTests
{
    [Fact]
    public void Build_MergesConsecutiveClusters()
    {
        var extents = FileAllocationTableBase.BuildExtents([2, 3, 4], 1000, 512, 10, 1536);
        Assert.Equal([new StreamExtent(1000, 1536)], extents);
    }

    [Fact]
    public void Build_SplitsFragmentedChain()
    {
        var extents = FileAllocationTableBase.BuildExtents([2, 5, 6, 3], 1000, 512, 10, 2048);
        Assert.Equal([new StreamExtent(1000, 512), new StreamExtent(2536, 1024), new StreamExtent(1512, 512)], extents);
    }

    [Fact]
    public void Build_ClipsLastExtentAndIgnoresUnusedClusters()
    {
        var extents = FileAllocationTableBase.BuildExtents([2, 3, 7], 0, 512, 10, 600);
        Assert.Equal([new StreamExtent(0, 600)], extents);
    }

    [Theory]
    [InlineData(0u)]
    [InlineData(1u)]
    [InlineData(12u)]
    public void Build_RejectsInvalidCluster(uint cluster)
    {
        Assert.Throws<InvalidDataException>(() => FileAllocationTableBase.BuildExtents([2, cluster], 0, 512, 10, 1024));
    }

    [Fact]
    public void Build_RejectsLengthBeyondChain()
    {
        Assert.Throws<InvalidDataException>(() => FileAllocationTableBase.BuildExtents([2, 3], 0, 512, 10, 1025));
    }

    [Fact]
    public void Build_EmptyLengthReturnsNoExtents()
    {
        Assert.Empty(FileAllocationTableBase.BuildExtents([0], 0, 512, 10, 0));
    }

    [Fact]
    public void Build_UsesSixtyFourBitOffsets()
    {
        const long bytesPerCluster = 32768;
        const uint cluster = 200_000; // (200000 - 2) * 32 KiB ≈ 6.1 GiB
        var extents = FileAllocationTableBase.BuildExtents([cluster], 4096, bytesPerCluster, 300_000, bytesPerCluster);
        Assert.Equal(4096 + (cluster - 2L) * bytesPerCluster, Assert.Single(extents).Offset);
    }

    [Fact]
    public void Fat12_FragmentedChainReadsInChainOrder()
    {
        var (fs, image) = Create();
        SetChain(fs, 2, 5, 3);
        Fill(fs, image, 2, 'A');
        Fill(fs, image, 5, 'B');
        Fill(fs, image, 3, 'C');

        using var stream = new FatDataStream(fs, Entry(2, 1536));
        var data = new byte[1536];
        stream.ReadExactly(data);
        Assert.True(data.Take(512).All(b => b == 'A'));
        Assert.True(data.Skip(512).Take(512).All(b => b == 'B'));
        Assert.True(data.Skip(1024).All(b => b == 'C'));
    }

    [Fact]
    public void Fat12_ReadAcrossClusterBoundary()
    {
        var (fs, image) = Create();
        SetChain(fs, 2, 6);
        Fill(fs, image, 2, 'X');
        Fill(fs, image, 6, 'Y');

        using var stream = new FatDataStream(fs, Entry(2, 1024));
        stream.Position = 510;
        var data = new byte[4];
        stream.ReadExactly(data);
        Assert.Equal("XXYY", Encoding.ASCII.GetString(data));
    }

    [Fact]
    public void Fat12_NonAlignedSizeEndsAtFileSize()
    {
        var (fs, image) = Create();
        SetChain(fs, 2, 3);
        Fill(fs, image, 2, 'P');
        Fill(fs, image, 3, 'Q');

        using var stream = new FatDataStream(fs, Entry(2, 700));
        Assert.Equal(700, stream.Length);
        var data = new byte[1024];
        Assert.Equal(700, stream.ReadAtLeast(data, 1024, throwOnEndOfStream: false));
        Assert.Equal((byte)'Q', data[699]);
        Assert.Equal(0, stream.Read(new byte[1]));
    }

    [Fact]
    public void Fat12_InterleavedStreamsKeepOwnPositions()
    {
        var (fs, image) = Create();
        SetChain(fs, 2, 3);
        SetChain(fs, 4, 5);
        Fill(fs, image, 2, 'a');
        Fill(fs, image, 3, 'b');
        Fill(fs, image, 4, 'c');
        Fill(fs, image, 5, 'd');

        using var first = new FatDataStream(fs, Entry(2, 1024));
        using var second = new FatDataStream(fs, Entry(4, 1024));
        var one = new byte[1024];
        var two = new byte[1024];
        for (int i = 0; i < 1024; i += 100)
        {
            int n = Math.Min(100, 1024 - i);
            first.ReadExactly(one, i, n);
            second.ReadExactly(two, i, n);
        }
        Assert.True(one.Take(512).All(b => b == 'a') && one.Skip(512).All(b => b == 'b'));
        Assert.True(two.Take(512).All(b => b == 'c') && two.Skip(512).All(b => b == 'd'));
    }

    [Fact]
    public void Fat12_EmptyFileDoesNotWalkChain()
    {
        var (fs, _) = Create();
        using var stream = new FatDataStream(fs, Entry(0, 0));
        Assert.Equal(0, stream.Length);
        Assert.Equal(0, stream.Read(new byte[1]));
    }

    private static (Fat12FileSystem, byte[]) Create()
    {
        var image = new byte[2880 * 512];
        return (FatBugRegressionTests.CreateFat12(image), image);
    }

    private static void SetChain(FatFileSystem fs, params uint[] clusters)
    {
        for (int i = 0; i < clusters.Length; i++)
            fs.MainFat[clusters[i]] = i + 1 < clusters.Length ? clusters[i + 1] : 0xFFF;
    }

    private static void Fill(FatFileSystem fs, byte[] image, uint cluster, char value)
    {
        long offset = fs.DataAreaFirstSector * 512L + (cluster - 2L) * 512;
        image.AsSpan((int)offset, 512).Fill((byte)value);
    }

    private static DirectoryEntry Entry(uint firstCluster, uint size) =>
        new(Encoding.ASCII.GetBytes("FILE    BIN"), FatAttributes.Archive, firstCluster, size, null, null, null);
}
