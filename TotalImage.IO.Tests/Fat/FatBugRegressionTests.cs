using System;
using System.Buffers.Binary;
using System.IO;
using System.Linq;
using TotalImage.FileSystems.BPB;
using TotalImage.FileSystems.FAT;
using Xunit;

namespace TotalImage.IO.Tests.Fat;

public class FatBugRegressionTests
{
    [Fact]
    public void Fat12_UpdatesOnlyRequestedEntryInBothCopies()
    {
        var image = new byte[2880 * 512];
        var fs = CreateFat12(image);
        Assert.Equal(fs.ClusterCount + 2, fs.MainFat.Length);
        fs.Fats[0][2] = 3;
        fs.Fats[1][2] = 3;
        fs.Fats[0][3] = 0xFFF;
        fs.Fats[1][3] = 0xFFF;

        Assert.Equal(3u, fs.Fats[0][2]);
        Assert.Equal(0xFFFu, fs.Fats[0][3]);
        Assert.Equal(fs.Fats[0][2], fs.Fats[1][2]);
        Assert.Equal((byte)3, image[512 + 3]);
        Assert.Equal((byte)3, image[512 + 9 * 512 + 3]);
    }

    [Fact]
    public void Fat12_ClusterStreamSeeksToEndAndRejectsCycles()
    {
        var fs = CreateFat12(new byte[2880 * 512]);
        fs.MainFat[2] = 0xFFF;
        using var data = new FatDataStream(fs, 2);
        Assert.Equal(512, data.Seek(0, SeekOrigin.End));
        Assert.Equal(0, data.Read(new byte[1]));

        fs.MainFat[2] = 2;
        Assert.Throws<InvalidDataException>(() => fs.MainFat.GetClusterChain(2));
    }

    [Fact]
    public void Fat12_AddRenameAndDeleteLongName_ReclaimsSlots()
    {
        var image = new byte[2880 * 512];
        var fs = CreateFat12(image);
        var root = (FatDirectory)fs.RootDirectory;
        using var data = new MemoryStream(new byte[] { 1, 2, 3 });
        root.WriteAddFile("A longer file name.txt", data, FatAttributes.Archive, null, null, null);
        var file = Assert.IsType<FatFile>(Assert.Single(root.EnumerateFileSystemObjects(true)));
        Assert.Equal("A longer file name.txt", file.Name);
        file.WriteRename("Renamed long file.txt");
        file = Assert.IsType<FatFile>(Assert.Single(root.EnumerateFileSystemObjects(true)));
        Assert.Equal("Renamed long file.txt", file.Name);
        file.WriteDelete();
        Assert.Empty(root.EnumerateFileSystemObjects(true));
        long rootOffset = (1 + 18) * 512;
        Assert.Equal((byte)0xE5, image[rootOffset]);

        using var empty = new MemoryStream();
        root.WriteAddFile("SMALL.TXT", empty, FatAttributes.Archive, null, null, null);
        var shortFile = Assert.IsType<FatFile>(Assert.Single(root.EnumerateFileSystemObjects(true)));
        shortFile.WriteRename("A much longer replacement filename.txt");
        Assert.Equal("A much longer replacement filename.txt", Assert.Single(root.EnumerateFileSystemObjects(true)).Name);
    }

    [Fact]
    public void Fat12_AddFile_HandlesShortSourceReads()
    {
        var fs = CreateFat12(new byte[2880 * 512]);
        using var source = new OneByteAtATimeStream(new byte[] { 1, 2, 3, 4 });
        ((FatDirectory)fs.RootDirectory).WriteAddFile("DATA.BIN", source, FatAttributes.Archive, null, null, null);
        var file = Assert.IsType<FatFile>(Assert.Single(fs.RootDirectory.EnumerateFileSystemObjects(true)));
        using var data = file.GetStream();
        var bytes = new byte[4];
        data.ReadExactly(bytes);
        Assert.Equal(new byte[] { 1, 2, 3, 4 }, bytes);
    }

    [Fact]
    public void Fat12_SizeOnDiskRoundsOnlyNonEmptyUnalignedFiles()
    {
        var fs = CreateFat12(new byte[2880 * 512]);
        var root = (FatDirectory)fs.RootDirectory;
        root.WriteAddFile("EMPTY.TXT", new MemoryStream(), FatAttributes.Archive, null, null, null);
        root.WriteAddFile("EXACT.TXT", new MemoryStream(new byte[512]), FatAttributes.Archive, null, null, null);
        root.WriteAddFile("ODD.TXT", new MemoryStream(new byte[513]), FatAttributes.Archive, null, null, null);
        var files = root.EnumerateFileSystemObjects(true).OfType<FatFile>().ToDictionary(f => f.Name);

        Assert.Equal(0UL, files["EMPTY.TXT"].LengthOnDisk);
        Assert.Equal(512UL, files["EXACT.TXT"].LengthOnDisk);
        Assert.Equal(1024UL, files["ODD.TXT"].LengthOnDisk);
        Assert.Equal(root.GetSize(false, false), root.GetStats(false).Size);
        Assert.Equal(root.GetSize(false, true), root.GetStats(false).SizeOnDisk);
    }

    private sealed class OneByteAtATimeStream(byte[] bytes) : MemoryStream(bytes)
    {
        public override int Read(byte[] buffer, int offset, int count) => base.Read(buffer, offset, Math.Min(count, 1));
        public override int Read(Span<byte> buffer) => base.Read(buffer[..Math.Min(buffer.Length, 1)]);
    }

    [Fact]
    public void Fat12_WritesLongNameAcrossNoncontiguousDirectoryClusters()
    {
        var image = new byte[2880 * 512];
        var fs = CreateFat12(image);
        fs.MainFat[2] = 4;
        fs.MainFat[3] = 0xFFF;
        fs.MainFat[4] = 0xFFF;
        long dataStart = fs.DataAreaFirstSector * 512L;
        for (int i = 0; i < 15; i++)
            image[dataStart + i * 32] = 0xE5;
        image[dataStart + 15 * 32] = 0;
        image[dataStart + 512] = 0x7B; // cluster 3 is unrelated data
        var entry = new DirectoryEntry(new byte[] { (byte)'D', (byte)'I', (byte)'R', (byte)' ', (byte)' ', (byte)' ', (byte)' ', (byte)' ', (byte)' ', (byte)' ', (byte)' ' }, FatAttributes.Subdirectory, 2, 0, null, null, null);
        var directory = new FatDirectory(fs, entry, null, (FatDirectory)fs.RootDirectory);

        directory.WriteAddFile("a long filename.txt", new MemoryStream(), FatAttributes.Archive, null, null, null);
        Assert.Equal((byte)0x7B, image[dataStart + 512]);
        Assert.Equal("a long filename.txt", Assert.Single(directory.EnumerateFileSystemObjects(true)).Name);
    }

    [Fact]
    public void Fat32_UpdatesFsInfoOnAllocationAndFree()
    {
        var image = new byte[100 * 512];
        Span<byte> boot = image.AsSpan(0, 512);
        BinaryPrimitives.WriteUInt16LittleEndian(boot[11..13], 512);
        boot[13] = 1;
        BinaryPrimitives.WriteUInt16LittleEndian(boot[14..16], 2);
        boot[16] = 1;
        BinaryPrimitives.WriteUInt32LittleEndian(boot[32..36], 100);
        BinaryPrimitives.WriteUInt16LittleEndian(boot[24..26], 1);
        BinaryPrimitives.WriteUInt16LittleEndian(boot[26..28], 1);
        BinaryPrimitives.WriteUInt32LittleEndian(boot[36..40], 1);
        BinaryPrimitives.WriteUInt32LittleEndian(boot[44..48], 2);
        BinaryPrimitives.WriteUInt16LittleEndian(boot[48..50], 1);
        image[66] = 0x29;
        image[0x1FE] = 0x55;
        image[0x1FF] = 0xAA;
        Span<byte> info = image.AsSpan(512, 512);
        BinaryPrimitives.WriteUInt32LittleEndian(info[0..4], 0x41615252);
        BinaryPrimitives.WriteUInt32LittleEndian(info[484..488], 0x61417272);
        BinaryPrimitives.WriteUInt32LittleEndian(info[488..492], 90);
        BinaryPrimitives.WriteUInt32LittleEndian(info[492..496], 3);
        BinaryPrimitives.WriteUInt32LittleEndian(info[508..512], 0xAA550000);
        using var reader = new BinaryReader(new MemoryStream(image));
        var fs = new Fat32FileSystem(new MemoryStream(image), BiosParameterBlock.Parse(reader, 11));

        Assert.Equal(90u, fs.TotalFreeClusters);
        uint cluster = fs.AllocateCluster();
        Assert.Equal(89u, fs.TotalFreeClusters);
        Assert.Equal(89u, BinaryPrimitives.ReadUInt32LittleEndian(info[488..492]));
        fs.FreeClusterChain(cluster);
        Assert.Equal(90u, fs.TotalFreeClusters);
        Assert.Equal(90u, BinaryPrimitives.ReadUInt32LittleEndian(info[488..492]));
    }

    private static Fat12FileSystem CreateFat12(byte[] image)
    {
        Span<byte> boot = image.AsSpan(0, 512);
        BinaryPrimitives.WriteUInt16LittleEndian(boot[11..13], 512);
        boot[13] = 1;
        BinaryPrimitives.WriteUInt16LittleEndian(boot[14..16], 1);
        boot[16] = 2;
        BinaryPrimitives.WriteUInt16LittleEndian(boot[17..19], 224);
        BinaryPrimitives.WriteUInt16LittleEndian(boot[19..21], 2880);
        boot[21] = 0xF0;
        BinaryPrimitives.WriteUInt16LittleEndian(boot[22..24], 9);
        BinaryPrimitives.WriteUInt16LittleEndian(boot[24..26], 18);
        BinaryPrimitives.WriteUInt16LittleEndian(boot[26..28], 2);
        using var reader = new BinaryReader(new MemoryStream(image));
        var bpb = BiosParameterBlock.Parse(reader, 11);
        return new Fat12FileSystem(new MemoryStream(image), bpb);
    }
}
