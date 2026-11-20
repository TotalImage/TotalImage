using System;
using System.Buffers.Binary;
using System.IO;
using System.Linq;
using System.Text;
using TotalImage.FileSystems;
using TotalImage.FileSystems.BPB;
using TotalImage.FileSystems.FAT;
using Xunit;

namespace TotalImage.IO.Tests.Fat;

public class FatClusterChainTests
{
    // Table lengths are ClusterCount + 2, so MAX (the highest valid cluster number) is length - 1.
    [Theory]
    [InlineData(0xFFFFu, 100u, 0x0000u, ClusterType.Free)]
    [InlineData(0xFFFFu, 100u, 0x0001u, ClusterType.NonFree)]
    [InlineData(0xFFFFu, 100u, 0x0002u, ClusterType.Data)]
    [InlineData(0xFFFFu, 100u, 99u, ClusterType.Data)]         // MAX
    [InlineData(0xFFFFu, 100u, 100u, ClusterType.Invalid)]    // MAX + 1
    [InlineData(0xFFFFu, 100u, 0xFFF0u, ClusterType.Invalid)]
    [InlineData(0xFFFFu, 100u, 0xFFF6u, ClusterType.Invalid)]
    [InlineData(0xFFFFu, 100u, 0xFFF7u, ClusterType.Bad)]
    [InlineData(0xFFFFu, 100u, 0xFFF8u, ClusterType.EndOfChain)]
    [InlineData(0xFFFFu, 100u, 0xFFFFu, ClusterType.EndOfChain)]
    [InlineData(0xFFFFu, 65524u + 2, 0xFFF0u, ClusterType.Data)] // largest FAT16 volume
    [InlineData(0xFFFFu, 65524u + 2, 0xFFF5u, ClusterType.Data)]
    [InlineData(0xFFFFu, 65524u + 2, 0xFFF6u, ClusterType.Invalid)]
    [InlineData(0xFFFu, 4084u + 2, 0xFF5u, ClusterType.Data)]    // largest FAT12 volume
    [InlineData(0xFFFu, 4084u + 2, 0xFF6u, ClusterType.Invalid)]
    [InlineData(0xFFFu, 4084u + 2, 0xFF7u, ClusterType.Bad)]
    [InlineData(0xFFFu, 4084u + 2, 0xFF8u, ClusterType.EndOfChain)]
    [InlineData(0xFFFFFFFu, 100u, 0xFFFFFF6u, ClusterType.Invalid)]
    [InlineData(0xFFFFFFFu, 100u, 0xFFFFFF7u, ClusterType.Bad)]
    [InlineData(0xFFFFFFFu, 100u, 0xFFFFFF8u, ClusterType.EndOfChain)]
    public void ClassifiesValuesPerSpec(uint mask, uint length, uint value, ClusterType expected)
    {
        var fat = new ArrayFileAllocationTable(length, mask);
        Assert.Equal(expected, fat.GetClusterType(value));
        Assert.Equal(expected == ClusterType.EndOfChain, fat.IsEndOfChainMarker(value));
    }

    [Fact]
    public void Fat16_ChainFollowsFFF0ToFFF5WhenInsideTable()
    {
        // A maximum-size FAT16 volume has 65524 clusters, numbered 2..0xFFF5.
        var fat = new ArrayFileAllocationTable(65524 + 2, 0xFFFF);
        fat[2] = 0xFFF0;
        for (uint c = 0xFFF0; c < 0xFFF5; c++)
            fat[c] = c + 1;
        fat[0xFFF5] = 0xFFF8;

        Assert.Equal(new uint[] { 2, 0xFFF0, 0xFFF1, 0xFFF2, 0xFFF3, 0xFFF4, 0xFFF5 }, fat.GetClusterChain(2));
    }

    [Theory]
    [InlineData(0x0000u)] // free
    [InlineData(0x0001u)] // reserved value 1
    [InlineData(0xFFF7u)] // bad
    [InlineData(100u)]    // first index past the table
    [InlineData(0xFFF0u)] // reserved value that is not a cluster of this volume
    public void ChainThrowsOnInvalidLink(uint value)
    {
        var fat = new ArrayFileAllocationTable(100, 0xFFFF);
        fat[2] = 3;
        fat[3] = value;

        Assert.Throws<InvalidDataException>(() => fat.GetClusterChain(2));
        Assert.Throws<InvalidDataException>(() => fat.GetNextCluster(3));
    }

    [Theory]
    [InlineData(0u)]
    [InlineData(1u)]
    [InlineData(100u)]
    public void ChainThrowsOnInvalidFirstCluster(uint first)
    {
        var fat = new ArrayFileAllocationTable(100, 0xFFFF);
        Assert.Throws<InvalidDataException>(() => fat.GetClusterChain(first));
    }

    [Fact]
    public void Fat12_PointerIntoTableSlackThrows()
    {
        // 1.44 MB floppy: the 9-sector FAT has room for 3072 entries but the volume has only 2847 clusters.
        var image = new byte[2880 * 512];
        var fs = FatBugRegressionTests.CreateFat12(image);
        Assert.Equal(2847u + 2, fs.MainFat.Length);
        fs.MainFat[2] = 3000;

        Assert.Throws<InvalidDataException>(() => fs.MainFat.GetClusterChain(2));
        Assert.Throws<InvalidDataException>(() => new ClusterChainStream(fs, 2));
    }

    [Fact]
    public void Fat12_MaxSizeVolumeReadsThroughReservedClusterNumbers()
    {
        // 4084 clusters (2..0xFF5): 1 reserved + 12 FAT + 32 root dir sectors.
        var (fs, image) = CreateVolume(clusters: 4084, fatSectors: 12, large: false, (b, s) => new Fat12FileSystem(s, b));
        Assert.Equal(4084u, fs.ClusterCount);
        Assert.Equal(4084u + 2, fs.MainFat.Length);
        AssertReadsChainThroughTop(fs, image, 0xFF0, 0xFF5, 0xFFF);
    }

    [Fact]
    public void Fat16_MaxSizeVolumeReadsThroughReservedClusterNumbers()
    {
        // 65524 clusters (2..0xFFF5): 1 reserved + 256 FAT + 32 root dir sectors.
        var (fs, image) = CreateVolume(clusters: 65524, fatSectors: 256, large: true, (b, s) => new Fat16FileSystem(s, b));
        Assert.Equal(65524u, fs.ClusterCount);
        Assert.Equal(65524u + 2, fs.MainFat.Length);
        AssertReadsChainThroughTop(fs, image, 0xFFF0, 0xFFF5, 0xFFFF);
    }

    [Fact]
    public void Fat12_AllocateAndFreeChainKeepsChainWalkable()
    {
        var fs = FatBugRegressionTests.CreateFat12(new byte[2880 * 512]);
        var first = fs.AllocateCluster();
        var second = fs.AllocateCluster(first);
        var third = fs.AllocateCluster(second);
        Assert.Equal(new[] { first, second, third }, fs.MainFat.GetClusterChain(first));

        fs.FreeClusterChain(first);
        Assert.Equal(0u, fs.MainFat[first]);
        Assert.Equal(0u, fs.MainFat[third]);
    }

    private static void AssertReadsChainThroughTop(FatFileSystem fs, byte[] image, uint low, uint high, uint eoc)
    {
        var chain = new[] { 2u }.Concat(Enumerable.Range((int)low, (int)(high - low + 1)).Select(c => (uint)c)).ToArray();
        for (int i = 0; i < chain.Length; i++)
            fs.MainFat[chain[i]] = i + 1 < chain.Length ? chain[i + 1] : eoc;
        for (int i = 0; i < chain.Length; i++)
        {
            long offset = fs.DataAreaFirstSector * 512L + (chain[i] - 2L) * 512;
            image.AsSpan((int)offset, 512).Fill((byte)('A' + i));
        }

        Assert.Equal(chain, fs.MainFat.GetClusterChain(2));
        var entry = new DirectoryEntry(Encoding.ASCII.GetBytes("FILE    BIN"), FatAttributes.Archive, 2, (uint)chain.Length * 512, null, null, null);
        using var stream = new ClusterChainStream(fs, entry.FirstClusterOfFile, entry.FileSize);
        var data = new byte[chain.Length * 512];
        stream.ReadExactly(data);
        for (int i = 0; i < chain.Length; i++)
            Assert.True(data.AsSpan(i * 512, 512).ToArray().All(b => b == 'A' + i));
    }

    private static (T fs, byte[] image) CreateVolume<T>(uint clusters, ushort fatSectors, bool large, Func<BiosParameterBlock, Stream, T> create)
    {
        const ushort rootEntries = 512;
        uint totalSectors = 1u + fatSectors + rootEntries * 32u / 512 + clusters;
        var image = new byte[totalSectors * 512];
        Span<byte> boot = image.AsSpan(0, 512);
        BinaryPrimitives.WriteUInt16LittleEndian(boot[11..13], 512);
        boot[13] = 1;                                                  // sectors per cluster
        BinaryPrimitives.WriteUInt16LittleEndian(boot[14..16], 1);     // reserved sectors
        boot[16] = 1;                                                  // number of FATs
        BinaryPrimitives.WriteUInt16LittleEndian(boot[17..19], rootEntries);
        if (large)
            BinaryPrimitives.WriteUInt32LittleEndian(boot[32..36], totalSectors);
        else
            BinaryPrimitives.WriteUInt16LittleEndian(boot[19..21], (ushort)totalSectors);
        boot[21] = 0xF8;
        BinaryPrimitives.WriteUInt16LittleEndian(boot[22..24], fatSectors);
        BinaryPrimitives.WriteUInt16LittleEndian(boot[24..26], 63);
        BinaryPrimitives.WriteUInt16LittleEndian(boot[26..28], 16);
        using var reader = new BinaryReader(new MemoryStream(image));
        var bpb = BiosParameterBlock.Parse(reader, 11);
        return (create(bpb, new MemoryStream(image)), image);
    }
}
