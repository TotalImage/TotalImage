using System;
using System.Buffers.Binary;
using System.IO;
using System.Linq;
using System.Text;
using TotalImage.FileSystems;
using TotalImage.FileSystems.ISO;
using TotalImage.FileSystems.UDF;
using Xunit;

namespace TotalImage.IO.Tests.Udf;

public class UdfFileSystemTests
{
    [Fact]
    public void AttemptDetection_LoadsUdfFileSystemAndVolumeLabel()
    {
        using var stream = new MemoryStream(CreateTestImage());

        var fileSystem = FileSystem.AttemptDetection(stream);

        var udf = Assert.IsType<UdfFileSystem>(fileSystem);
        Assert.Equal("UDF", udf.DisplayName);
        Assert.Equal("TESTVOL", udf.VolumeLabel);
        Assert.True(udf.IsReadOnly);
        Assert.True(udf.SupportsSubdirectories);
        Assert.Equal(2048, udf.AllocationUnitSize);
    }

    [Fact]
    public void RootDirectory_EnumeratesFilesAndDirectories()
    {
        using var stream = new MemoryStream(CreateTestImage());

        var fileSystem = Assert.IsType<UdfFileSystem>(FileSystem.AttemptDetection(stream));
        var entries = fileSystem.RootDirectory.EnumerateFileSystemObjects(showHidden: true).OrderBy(entry => entry.Name).ToArray();

        Assert.Equal(2, entries.Length);
        Assert.Collection(
            entries,
            entry => Assert.IsType<UdfDirectory>(entry),
            entry => Assert.IsType<UdfFile>(entry));
        Assert.Equal("docs", entries[0].Name);
        Assert.Equal("hello.txt", entries[1].Name);
        Assert.Equal("\\hello.txt", entries[1].FullName);
    }

    [Fact]
    public void FileAndSubdirectoryStreams_ReturnExpectedContents()
    {
        using var stream = new MemoryStream(CreateTestImage());

        var fileSystem = Assert.IsType<UdfFileSystem>(FileSystem.AttemptDetection(stream));
        var rootEntries = fileSystem.RootDirectory.EnumerateFileSystemObjects(showHidden: true).ToArray();
        var helloFile = Assert.IsType<UdfFile>(rootEntries.Single(entry => entry.Name == "hello.txt"));
        var docsDirectory = Assert.IsType<UdfDirectory>(rootEntries.Single(entry => entry.Name == "docs"));
        var readme = Assert.IsType<UdfFile>(docsDirectory.EnumerateFileSystemObjects(showHidden: true).Single());

        using var helloStream = helloFile.GetStream();
        using var readmeStream = readme.GetStream();
        using var helloReader = new StreamReader(helloStream, Encoding.ASCII, false, leaveOpen: true);
        using var readmeReader = new StreamReader(readmeStream, Encoding.ASCII, false, leaveOpen: true);

        Assert.Equal("Hello UDF", helloReader.ReadToEnd());
        Assert.Equal("Nested readme", readmeReader.ReadToEnd());
        Assert.Equal("\\docs\\readme.txt", readme.FullName);
    }

    [Fact]
    public void FileStreams_ReadPastFirstLogicalBlock()
    {
        using var stream = new MemoryStream(CreateMultiBlockFileImage());

        var fileSystem = Assert.IsType<UdfFileSystem>(FileSystem.AttemptDetection(stream));
        var file = Assert.IsType<UdfFile>(fileSystem.RootDirectory.EnumerateFiles(showHidden: true).Single());

        using var reader = new StreamReader(file.GetStream(), Encoding.ASCII, false, leaveOpen: false);
        string contents = reader.ReadToEnd();

        Assert.Equal(3000, contents.Length);
        Assert.Equal(new string('A', 2048) + new string('B', 952), contents);
    }

    [Fact]
    public void GetStream_SupportsEntriesLargerThanInt32MaxValue()
    {
        using var stream = new MemoryStream(CreateLargeLengthFileImage());

        var fileSystem = Assert.IsType<UdfFileSystem>(FileSystem.AttemptDetection(stream));
        var file = Assert.IsType<UdfFile>(fileSystem.RootDirectory.EnumerateFiles(showHidden: true).Single());

        using var fileStream = file.GetStream();
        byte[] firstBytes = new byte[16];
        int bytesRead = fileStream.Read(firstBytes, 0, firstBytes.Length);

        Assert.Equal((long)int.MaxValue + 1024L, fileStream.Length);
        Assert.Equal(16, bytesRead);
        Assert.Equal(Encoding.ASCII.GetBytes(new string('L', 16)), firstBytes);
    }

    [Fact]
    public void IsoFactory_PrefersUdfWhenHybridMetadataIsPresent()
    {
        using var stream = new MemoryStream(CreateHybridIsoUdfImage());

        var fileSystem = new IsoFactory().TryLoadFileSystem(stream);

        Assert.IsType<UdfFileSystem>(fileSystem);
    }

    [Fact]
    public void AttemptDetection_LoadsUdfWhenLogicalVolumeDescriptorIsInChainedSequence()
    {
        using var stream = new MemoryStream(CreateChainedDescriptorSequenceImage());

        var fileSystem = FileSystem.AttemptDetection(stream);

        var udf = Assert.IsType<UdfFileSystem>(fileSystem);
        Assert.Equal("TESTVOL", udf.VolumeLabel);
        Assert.Equal(2048, udf.AllocationUnitSize);
        Assert.Single(udf.RootDirectory.EnumerateFiles(showHidden: true));
    }

    [Fact]
    public void AttemptDetection_LoadsUdfWhenOnlyTrailingAnchorIsPresent()
    {
        using var stream = new MemoryStream(CreateTrailingAnchorImage());

        var fileSystem = FileSystem.AttemptDetection(stream);

        var udf = Assert.IsType<UdfFileSystem>(fileSystem);
        Assert.Equal("TESTVOL", udf.VolumeLabel);
    }

    [Fact]
    public void AttemptDetection_ReadsMetadataPartitionFiles()
    {
        using var stream = new MemoryStream(CreateMetadataPartitionImage());

        var fileSystem = Assert.IsType<UdfFileSystem>(FileSystem.AttemptDetection(stream));
        var file = Assert.IsType<UdfFile>(fileSystem.RootDirectory.EnumerateFiles(showHidden: true).Single());

        using var reader = new StreamReader(file.GetStream(), Encoding.ASCII, false, leaveOpen: false);
        Assert.Equal("Hello UDF", reader.ReadToEnd());
    }

    [Fact]
    public void AttemptDetection_ReadsMetadataPartitionFilesWhenMetadataEntryUsesChainedLongDescriptors()
    {
        using var stream = new MemoryStream(CreateMetadataPartitionImageWithChainedMetadataEntry());

        var fileSystem = Assert.IsType<UdfFileSystem>(FileSystem.AttemptDetection(stream));
        var file = Assert.IsType<UdfFile>(fileSystem.RootDirectory.EnumerateFiles(showHidden: true).Single());

        using var reader = new StreamReader(file.GetStream(), Encoding.ASCII, false, leaveOpen: false);
        Assert.Equal("Hello UDF", reader.ReadToEnd());
    }

    [Fact]
    public void AttemptDetection_ReadsVatBackedVirtualPartitionFiles()
    {
        using var stream = new MemoryStream(CreateVirtualPartitionImage());

        var fileSystem = Assert.IsType<UdfFileSystem>(FileSystem.AttemptDetection(stream));
        var file = Assert.IsType<UdfFile>(fileSystem.RootDirectory.EnumerateFiles(showHidden: true).Single());

        using var reader = new StreamReader(file.GetStream(), Encoding.ASCII, false, leaveOpen: false);
        Assert.Equal("Hello UDF", reader.ReadToEnd());
    }

    [Fact]
    public void GetStream_ReadsFragmentedFileAcrossManySeparateExtents()
    {
        using var stream = new MemoryStream(CreateFragmentedFileImage());

        var fileSystem = Assert.IsType<UdfFileSystem>(FileSystem.AttemptDetection(stream));
        var file = Assert.IsType<UdfFile>(fileSystem.RootDirectory.EnumerateFiles(showHidden: true).Single());

        using var reader = new StreamReader(file.GetStream(), Encoding.ASCII, false, leaveOpen: false);
        Assert.Equal("AAABBBCCC", reader.ReadToEnd());
    }

    [Fact]
    public void GetStream_ReadsFileFromAllocationExtentDescriptorChain()
    {
        using var stream = new MemoryStream(CreateAllocationExtentChainImage());

        var fileSystem = Assert.IsType<UdfFileSystem>(FileSystem.AttemptDetection(stream));
        var file = Assert.IsType<UdfFile>(fileSystem.RootDirectory.EnumerateFiles(showHidden: true).Single());

        using var reader = new StreamReader(file.GetStream(), Encoding.ASCII, false, leaveOpen: false);
        Assert.Equal("ABCD", reader.ReadToEnd());
    }

    [Fact]
    public void GetStream_ReadsFileFromExtendedAllocationDescriptors()
    {
        using var stream = new MemoryStream(CreateExtendedAllocationDescriptorImage());

        var fileSystem = Assert.IsType<UdfFileSystem>(FileSystem.AttemptDetection(stream));
        var file = Assert.IsType<UdfFile>(fileSystem.RootDirectory.EnumerateFiles(showHidden: true).Single());

        using var reader = new StreamReader(file.GetStream(), Encoding.ASCII, false, leaveOpen: false);
        Assert.Equal("WXYZ", reader.ReadToEnd());
    }

    [Fact]
    public void GetStream_ZeroFillsUnrecordedExtents()
    {
        using var stream = new MemoryStream(CreateSparseFileImage());

        var fileSystem = Assert.IsType<UdfFileSystem>(FileSystem.AttemptDetection(stream));
        var file = Assert.IsType<UdfFile>(fileSystem.RootDirectory.EnumerateFiles(showHidden: true).Single());

        using var fileStream = file.GetStream();
        byte[] contents = new byte[fileStream.Length];
        fileStream.ReadExactly(contents);

        byte[] expected = Encoding.ASCII.GetBytes("AB").Concat(new byte[4]).Concat(Encoding.ASCII.GetBytes("CD")).ToArray();
        Assert.Equal(expected, contents);
    }

    private static byte[] CreateTestImage()
    {
        const int blockSize = 2048;
        const int totalBlocks = 512;
        const uint partitionStart = 400;
        const uint partitionLength = 16;

        byte[] image = new byte[totalBlocks * blockSize];

        WriteVolumeStructureDescriptor(image, 16, "BEA01");
        WriteVolumeStructureDescriptor(image, 17, "NSR02");
        WriteVolumeStructureDescriptor(image, 18, "TEA01");

        WriteAnchorVolumeDescriptorPointer(image, 256, 300, 4);
        WritePrimaryVolumeDescriptor(image, 300, "TESTVOL");
        WritePartitionDescriptor(image, 301, partitionStart, partitionLength);
        WriteLogicalVolumeDescriptor(image, 302, "TESTVOL", 2048, 0, 8);
        WriteTerminatingDescriptor(image, 303);

        byte[] helloData = Encoding.ASCII.GetBytes("Hello UDF");
        byte[] readmeData = Encoding.ASCII.GetBytes("Nested readme");

        byte[] docsDirectoryEntries = BuildDirectoryEntries(
            BuildFileIdentifierDescriptor(string.Empty, 0, 0, UdfUtilities.FileCharacteristicParent | UdfUtilities.FileCharacteristicDirectory),
            BuildFileIdentifierDescriptor("readme.txt", 6, 0, 0));

        byte[] rootDirectoryEntries = BuildDirectoryEntries(
            BuildFileIdentifierDescriptor(string.Empty, 0, 0, UdfUtilities.FileCharacteristicParent | UdfUtilities.FileCharacteristicDirectory),
            BuildFileIdentifierDescriptor("hello.txt", 2, 0, 0),
            BuildFileIdentifierDescriptor("docs", 4, 0, UdfUtilities.FileCharacteristicDirectory));

        WriteFileEntry(image, partitionStart + 0, 0, UdfUtilities.IcbFileTypeDirectory, (ulong)rootDirectoryEntries.Length, 1, rootDirectoryEntries.Length);
        WriteBytes(image, partitionStart + 1, rootDirectoryEntries);

        WriteFileEntry(image, partitionStart + 2, 0, 0x05, (ulong)helloData.Length, 3, helloData.Length);
        WriteBytes(image, partitionStart + 3, helloData);

        WriteFileEntry(image, partitionStart + 4, 0, UdfUtilities.IcbFileTypeDirectory, (ulong)docsDirectoryEntries.Length, 5, docsDirectoryEntries.Length);
        WriteBytes(image, partitionStart + 5, docsDirectoryEntries);

        WriteFileEntry(image, partitionStart + 6, 0, 0x05, (ulong)readmeData.Length, 7, readmeData.Length);
        WriteBytes(image, partitionStart + 7, readmeData);

        WriteFileSetDescriptor(image, partitionStart + 8, 0, 0);

        return image;
    }

    private static byte[] CreateHybridIsoUdfImage()
    {
        byte[] image = CreateTestImage();

        Span<byte> descriptor = image.AsSpan(19 * 2048, 2048);
        descriptor.Clear();
        descriptor[0] = 0x01;
        Encoding.ASCII.GetBytes("CD001", descriptor[1..6]);
        descriptor[6] = 0x01;

        return image;
    }

    private static byte[] CreateChainedDescriptorSequenceImage()
    {
        const int totalBlocks = 512;
        const uint partitionStart = 400;
        const uint partitionLength = 16;

        byte[] image = new byte[totalBlocks * 2048];

        WriteVolumeStructureDescriptor(image, 16, "BEA01");
        WriteVolumeStructureDescriptor(image, 17, "NSR02");
        WriteVolumeStructureDescriptor(image, 18, "TEA01");

        WriteAnchorVolumeDescriptorPointer(image, 256, 300, 3);
        WritePrimaryVolumeDescriptor(image, 300, "TESTVOL");
        WriteVolumeDescriptorPointer(image, 301, 320, 3);
        WriteTerminatingDescriptor(image, 302);
        WritePartitionDescriptor(image, 320, partitionStart, partitionLength);
        WriteLogicalVolumeDescriptor(image, 321, "TESTVOL", 2048, 0, 8);
        WriteTerminatingDescriptor(image, 322);

        byte[] helloData = Encoding.ASCII.GetBytes("Hello UDF");
        byte[] rootDirectoryEntries = BuildDirectoryEntries(
            BuildFileIdentifierDescriptor(string.Empty, 0, 0, UdfUtilities.FileCharacteristicParent | UdfUtilities.FileCharacteristicDirectory),
            BuildFileIdentifierDescriptor("hello.txt", 2, 0, 0));

        WriteFileEntry(image, partitionStart + 0, 0, UdfUtilities.IcbFileTypeDirectory, (ulong)rootDirectoryEntries.Length, 1, rootDirectoryEntries.Length);
        WriteBytes(image, partitionStart + 1, rootDirectoryEntries);
        WriteFileEntry(image, partitionStart + 2, 0, 0x05, (ulong)helloData.Length, 3, helloData.Length);
        WriteBytes(image, partitionStart + 3, helloData);
        WriteFileSetDescriptor(image, partitionStart + 8, 0, 0);

        return image;
    }

    private static byte[] CreateTrailingAnchorImage()
    {
        byte[] image = CreateTestImage();
        Array.Clear(image, 256 * 2048, 2048);
        WriteAnchorVolumeDescriptorPointer(image, 511, 300, 4);
        return image;
    }

    private static byte[] CreateMultiBlockFileImage()
    {
        byte[] image = new byte[512 * 2048];
        const uint partitionStart = 400;
        const uint partitionLength = 16;

        WriteVolumeStructureDescriptor(image, 16, "BEA01");
        WriteVolumeStructureDescriptor(image, 17, "NSR02");
        WriteVolumeStructureDescriptor(image, 18, "TEA01");

        WriteAnchorVolumeDescriptorPointer(image, 256, 300, 4);
        WritePrimaryVolumeDescriptor(image, 300, "TESTVOL");
        WritePartitionDescriptor(image, 301, partitionStart, partitionLength);
        WriteLogicalVolumeDescriptor(image, 302, "TESTVOL", 2048, 0, 5);
        WriteTerminatingDescriptor(image, 303);

        byte[] rootDirectoryEntries = BuildDirectoryEntries(
            BuildFileIdentifierDescriptor(string.Empty, 0, 0, UdfUtilities.FileCharacteristicParent | UdfUtilities.FileCharacteristicDirectory),
            BuildFileIdentifierDescriptor("large.bin", 2, 0, 0));

        byte[] firstBlock = Encoding.ASCII.GetBytes(new string('A', 2048));
        byte[] secondBlock = Encoding.ASCII.GetBytes(new string('B', 952));

        WriteFileEntry(image, partitionStart + 0, 0, UdfUtilities.IcbFileTypeDirectory, (ulong)rootDirectoryEntries.Length, 1, rootDirectoryEntries.Length);
        WriteBytes(image, partitionStart + 1, rootDirectoryEntries);
        WriteFileEntry(image, partitionStart + 2, 0, 0x05, 3000UL, 3, 3000);
        WriteBytes(image, partitionStart + 3, firstBlock);
        WriteBytes(image, partitionStart + 4, secondBlock);
        WriteFileSetDescriptor(image, partitionStart + 5, 0, 0);

        return image;
    }

    private static byte[] CreateLargeLengthFileImage()
    {
        byte[] image = new byte[512 * 2048];
        const uint partitionStart = 400;
        const uint partitionLength = 16;

        WriteVolumeStructureDescriptor(image, 16, "BEA01");
        WriteVolumeStructureDescriptor(image, 17, "NSR02");
        WriteVolumeStructureDescriptor(image, 18, "TEA01");

        WriteAnchorVolumeDescriptorPointer(image, 256, 300, 4);
        WritePrimaryVolumeDescriptor(image, 300, "TESTVOL");
        WritePartitionDescriptor(image, 301, partitionStart, partitionLength);
        WriteLogicalVolumeDescriptor(image, 302, "TESTVOL", 2048, 0, 4);
        WriteTerminatingDescriptor(image, 303);

        byte[] rootDirectoryEntries = BuildDirectoryEntries(
            BuildFileIdentifierDescriptor(string.Empty, 0, 0, UdfUtilities.FileCharacteristicParent | UdfUtilities.FileCharacteristicDirectory),
            BuildFileIdentifierDescriptor("huge.bin", 2, 0, 0));

        byte[] firstBlock = Encoding.ASCII.GetBytes(new string('L', 2048));

        WriteFileEntry(image, partitionStart + 0, 0, UdfUtilities.IcbFileTypeDirectory, (ulong)rootDirectoryEntries.Length, 1, rootDirectoryEntries.Length);
        WriteBytes(image, partitionStart + 1, rootDirectoryEntries);
        WriteFileEntry(image, partitionStart + 2, 0, 0x05, (ulong)int.MaxValue + 1024UL, 3, firstBlock.Length);
        WriteBytes(image, partitionStart + 3, firstBlock);
        WriteFileSetDescriptor(image, partitionStart + 4, 0, 0);

        return image;
    }

    private static byte[] CreateFragmentedFileImage()
    {
        byte[] image = new byte[512 * 2048];
        const uint partitionStart = 400;

        WriteBasicVolume(image, partitionStart, 16, 8);

        byte[] rootDirectoryEntries = BuildDirectoryEntries(
            BuildFileIdentifierDescriptor(string.Empty, 0, 0, UdfUtilities.FileCharacteristicParent | UdfUtilities.FileCharacteristicDirectory),
            BuildFileIdentifierDescriptor("frag.bin", 2, 0, 0));

        WriteFileEntry(image, partitionStart + 0, 0, UdfUtilities.IcbFileTypeDirectory, (ulong)rootDirectoryEntries.Length, 1, rootDirectoryEntries.Length);
        WriteBytes(image, partitionStart + 1, rootDirectoryEntries);
        WriteFileEntryWithShortDescriptors(image, partitionStart + 2, 0, 0x05, 9, [
            (3u, Encoding.ASCII.GetBytes("AAA")),
            (5u, Encoding.ASCII.GetBytes("BBB")),
            (7u, Encoding.ASCII.GetBytes("CCC"))
        ]);
        WriteFileSetDescriptor(image, partitionStart + 8, 0, 0);

        return image;
    }

    private static byte[] CreateAllocationExtentChainImage()
    {
        byte[] image = new byte[512 * 2048];
        const uint partitionStart = 400;

        WriteBasicVolume(image, partitionStart, 16, 8);

        byte[] rootDirectoryEntries = BuildDirectoryEntries(
            BuildFileIdentifierDescriptor(string.Empty, 0, 0, UdfUtilities.FileCharacteristicParent | UdfUtilities.FileCharacteristicDirectory),
            BuildFileIdentifierDescriptor("chain.bin", 2, 0, 0));

        WriteFileEntry(image, partitionStart + 0, 0, UdfUtilities.IcbFileTypeDirectory, (ulong)rootDirectoryEntries.Length, 1, rootDirectoryEntries.Length);
        WriteBytes(image, partitionStart + 1, rootDirectoryEntries);
        WriteFileEntryWithLongDescriptors(image, partitionStart + 2, 0, 0x05, 4, [
            BuildLongAllocationDescriptor(0xC0000000 | 2048u, 6, 0)
        ]);
        WriteAllocationExtentDescriptor(image, partitionStart + 6, [
            BuildLongAllocationDescriptor(2u, 3, 0),
            BuildLongAllocationDescriptor(2u, 4, 0)
        ]);
        WriteBytes(image, partitionStart + 3, Encoding.ASCII.GetBytes("AB"));
        WriteBytes(image, partitionStart + 4, Encoding.ASCII.GetBytes("CD"));
        WriteFileSetDescriptor(image, partitionStart + 8, 0, 0);

        return image;
    }

    private static byte[] CreateExtendedAllocationDescriptorImage()
    {
        byte[] image = new byte[512 * 2048];
        const uint partitionStart = 400;

        WriteBasicVolume(image, partitionStart, 16, 8);

        byte[] rootDirectoryEntries = BuildDirectoryEntries(
            BuildFileIdentifierDescriptor(string.Empty, 0, 0, UdfUtilities.FileCharacteristicParent | UdfUtilities.FileCharacteristicDirectory),
            BuildFileIdentifierDescriptor("extd.bin", 2, 0, 0));

        WriteFileEntry(image, partitionStart + 0, 0, UdfUtilities.IcbFileTypeDirectory, (ulong)rootDirectoryEntries.Length, 1, rootDirectoryEntries.Length);
        WriteBytes(image, partitionStart + 1, rootDirectoryEntries);
        WriteFileEntryWithExtendedDescriptors(image, partitionStart + 2, 0, 0x05, 4, [
            BuildExtendedAllocationDescriptor(4u, 4u, 4u, 3u, 0)
        ]);
        WriteBytes(image, partitionStart + 3, Encoding.ASCII.GetBytes("WXYZ"));
        WriteFileSetDescriptor(image, partitionStart + 8, 0, 0);

        return image;
    }

    private static byte[] CreateSparseFileImage()
    {
        byte[] image = new byte[512 * 2048];
        const uint partitionStart = 400;

        WriteBasicVolume(image, partitionStart, 16, 8);

        byte[] rootDirectoryEntries = BuildDirectoryEntries(
            BuildFileIdentifierDescriptor(string.Empty, 0, 0, UdfUtilities.FileCharacteristicParent | UdfUtilities.FileCharacteristicDirectory),
            BuildFileIdentifierDescriptor("sparse.bin", 2, 0, 0));

        WriteFileEntry(image, partitionStart + 0, 0, UdfUtilities.IcbFileTypeDirectory, (ulong)rootDirectoryEntries.Length, 1, rootDirectoryEntries.Length);
        WriteBytes(image, partitionStart + 1, rootDirectoryEntries);
        WriteFileEntryWithShortDescriptorBytes(image, partitionStart + 2, 0, 0x05, 8, [
            BuildShortAllocationDescriptor(2u, 3u),
            BuildShortAllocationDescriptor(UdfUtilities.ExtentNotRecordedAllocated | 4u, 0u),
            BuildShortAllocationDescriptor(2u, 4u)
        ]);
        WriteBytes(image, partitionStart + 3, Encoding.ASCII.GetBytes("AB"));
        WriteBytes(image, partitionStart + 4, Encoding.ASCII.GetBytes("CD"));
        WriteFileSetDescriptor(image, partitionStart + 8, 0, 0);

        return image;
    }

    private static byte[] CreateMetadataPartitionImage()
    {
        byte[] image = new byte[512 * 2048];
        const uint physicalPartitionStart = 400;
        const uint physicalPartitionLength = 24;

        WriteVolumeStructureDescriptor(image, 16, "BEA01");
        WriteVolumeStructureDescriptor(image, 17, "NSR02");
        WriteVolumeStructureDescriptor(image, 18, "TEA01");

        WriteAnchorVolumeDescriptorPointer(image, 256, 300, 4);
        WritePrimaryVolumeDescriptor(image, 300, "TESTVOL");
        WritePartitionDescriptor(image, 301, physicalPartitionStart, physicalPartitionLength);
        WriteLogicalVolumeDescriptorWithMetadataPartition(image, 302, "TESTVOL", 2048, 0, 8, 1);
        WriteTerminatingDescriptor(image, 303);

        byte[] helloData = Encoding.ASCII.GetBytes("Hello UDF");
        byte[] rootDirectoryEntries = BuildDirectoryEntries(
            BuildFileIdentifierDescriptor(string.Empty, 0, 1, UdfUtilities.FileCharacteristicParent | UdfUtilities.FileCharacteristicDirectory),
            BuildFileIdentifierDescriptor("hello.txt", 2, 1, 0));

        WriteMetadataFileEntry(image, physicalPartitionStart + 8, 0, 9, 5);
        WriteFileEntry(image, physicalPartitionStart + 9, 1, UdfUtilities.IcbFileTypeDirectory, (ulong)rootDirectoryEntries.Length, 3, rootDirectoryEntries.Length);
        WriteFileSetDescriptor(image, physicalPartitionStart + 10, 0, 1);
        WriteFileEntry(image, physicalPartitionStart + 11, 1, 0x05, (ulong)helloData.Length, 4, helloData.Length);
        WriteBytes(image, physicalPartitionStart + 12, rootDirectoryEntries);
        WriteBytes(image, physicalPartitionStart + 13, helloData);

        return image;
    }

    private static byte[] CreateMetadataPartitionImageWithChainedMetadataEntry()
    {
        byte[] image = new byte[512 * 2048];
        const uint physicalPartitionStart = 400;
        const uint physicalPartitionLength = 24;

        WriteVolumeStructureDescriptor(image, 16, "BEA01");
        WriteVolumeStructureDescriptor(image, 17, "NSR02");
        WriteVolumeStructureDescriptor(image, 18, "TEA01");

        WriteAnchorVolumeDescriptorPointer(image, 256, 300, 4);
        WritePrimaryVolumeDescriptor(image, 300, "TESTVOL");
        WritePartitionDescriptor(image, 301, physicalPartitionStart, physicalPartitionLength);
        WriteLogicalVolumeDescriptorWithMetadataPartition(image, 302, "TESTVOL", 2048, 0, 8, 1);
        WriteTerminatingDescriptor(image, 303);

        byte[] helloData = Encoding.ASCII.GetBytes("Hello UDF");
        byte[] rootDirectoryEntries = BuildDirectoryEntries(
            BuildFileIdentifierDescriptor(string.Empty, 0, 1, UdfUtilities.FileCharacteristicParent | UdfUtilities.FileCharacteristicDirectory),
            BuildFileIdentifierDescriptor("hello.txt", 2, 1, 0));

        WriteMetadataFileEntryWithLongDescriptors(image, physicalPartitionStart + 8, 0, [
            BuildLongAllocationDescriptor(UdfUtilities.ExtentNextAllocationDescriptors | 2048u, 14u, 0)
        ]);
        WriteAllocationExtentDescriptor(image, physicalPartitionStart + 14, [
            BuildLongAllocationDescriptor(2048u, 9u, 0),
            BuildLongAllocationDescriptor(2048u, 10u, 0),
            BuildLongAllocationDescriptor(2048u, 11u, 0),
            BuildLongAllocationDescriptor(2048u, 12u, 0),
            BuildLongAllocationDescriptor(2048u, 13u, 0)
        ]);
        WriteFileEntry(image, physicalPartitionStart + 9, 1, UdfUtilities.IcbFileTypeDirectory, (ulong)rootDirectoryEntries.Length, 3, rootDirectoryEntries.Length);
        WriteFileSetDescriptor(image, physicalPartitionStart + 10, 0, 1);
        WriteFileEntry(image, physicalPartitionStart + 11, 1, 0x05, (ulong)helloData.Length, 4, helloData.Length);
        WriteBytes(image, physicalPartitionStart + 12, rootDirectoryEntries);
        WriteBytes(image, physicalPartitionStart + 13, helloData);

        return image;
    }

    private static byte[] CreateVirtualPartitionImage()
    {
        byte[] image = new byte[512 * 2048];
        const uint physicalPartitionStart = 400;
        const uint physicalPartitionLength = 24;

        WriteVolumeStructureDescriptor(image, 16, "BEA01");
        WriteVolumeStructureDescriptor(image, 17, "NSR02");
        WriteVolumeStructureDescriptor(image, 18, "TEA01");

        WriteAnchorVolumeDescriptorPointer(image, 256, 300, 4);
        WritePrimaryVolumeDescriptor(image, 300, "TESTVOL");
        WritePartitionDescriptor(image, 301, physicalPartitionStart, physicalPartitionLength);
        WriteLogicalVolumeDescriptorWithVirtualPartition(image, 302, "TESTVOL", 2048, 0, 1);
        WriteTerminatingDescriptor(image, 303);

        byte[] helloData = Encoding.ASCII.GetBytes("Hello UDF");
        byte[] rootDirectoryEntries = BuildDirectoryEntries(
            BuildFileIdentifierDescriptor(string.Empty, 0, 1, UdfUtilities.FileCharacteristicParent | UdfUtilities.FileCharacteristicDirectory),
            BuildFileIdentifierDescriptor("hello.txt", 2, 1, 0));

        byte[] vatContents = BuildVatContents(152, [8u, 12u, 9u, 10u, 11u]);
        WriteFileEntry(image, physicalPartitionStart + 20, 0, UdfUtilities.IcbFileTypeVat, (ulong)vatContents.Length, 21, vatContents.Length);
        WriteBytes(image, physicalPartitionStart + 21, vatContents);

        WriteFileEntry(image, physicalPartitionStart + 8, 1, UdfUtilities.IcbFileTypeDirectory, (ulong)rootDirectoryEntries.Length, 3, rootDirectoryEntries.Length);
        WriteFileSetDescriptor(image, physicalPartitionStart + 12, 0, 1);
        WriteFileEntry(image, physicalPartitionStart + 9, 1, 0x05, (ulong)helloData.Length, 4, helloData.Length);
        WriteBytes(image, physicalPartitionStart + 10, rootDirectoryEntries);
        WriteBytes(image, physicalPartitionStart + 11, helloData);

        return image;
    }

    private static void WriteVolumeStructureDescriptor(byte[] image, int block, string identifier)
    {
        Span<byte> descriptor = image.AsSpan(block * 2048, 2048);
        descriptor.Clear();
        descriptor[0] = 0;
        Encoding.ASCII.GetBytes(identifier, descriptor[1..6]);
        descriptor[6] = 1;
    }

    private static void WriteAnchorVolumeDescriptorPointer(byte[] image, uint block, uint mainSequenceBlock, uint mainSequenceLengthInBlocks)
    {
        Span<byte> descriptor = image.AsSpan((int)(block * 2048), 2048);
        descriptor.Clear();
        WriteDescriptorTag(descriptor, UdfUtilities.AnchorVolumeDescriptorPointer, block);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[16..20], mainSequenceLengthInBlocks * 2048u);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[20..24], mainSequenceBlock);
    }

    private static void WritePrimaryVolumeDescriptor(byte[] image, uint block, string volumeLabel)
    {
        Span<byte> descriptor = image.AsSpan((int)(block * 2048), 2048);
        descriptor.Clear();
        WriteDescriptorTag(descriptor, UdfUtilities.PrimaryVolumeDescriptor, block);
        WriteDString(descriptor[24..56], volumeLabel);
    }

    private static void WritePartitionDescriptor(byte[] image, uint block, uint partitionStart, uint partitionLength)
    {
        Span<byte> descriptor = image.AsSpan((int)(block * 2048), 2048);
        descriptor.Clear();
        WriteDescriptorTag(descriptor, UdfUtilities.PartitionDescriptor, block);
        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[22..24], 0);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[188..192], partitionStart);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[192..196], partitionLength);
    }

    private static void WriteVolumeDescriptorPointer(byte[] image, uint block, uint nextSequenceBlock, uint nextSequenceLengthInBlocks)
    {
        Span<byte> descriptor = image.AsSpan((int)(block * 2048), 2048);
        descriptor.Clear();
        WriteDescriptorTag(descriptor, UdfUtilities.VolumeDescriptorPointer, block);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[20..24], nextSequenceLengthInBlocks * 2048u);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[24..28], nextSequenceBlock);
    }

    private static void WriteLogicalVolumeDescriptor(byte[] image, uint block, string volumeLabel, uint logicalBlockSize, ushort partitionNumber, uint fileSetDescriptorBlock)
    {
        Span<byte> descriptor = image.AsSpan((int)(block * 2048), 2048);
        descriptor.Clear();
        WriteDescriptorTag(descriptor, UdfUtilities.LogicalVolumeDescriptor, block);
        WriteDString(descriptor[84..212], volumeLabel);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[212..216], logicalBlockSize);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[264..268], 6);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[268..272], 1);
        WriteLongAllocationDescriptor(descriptor[248..264], 2048, fileSetDescriptorBlock, 0);
        descriptor[440] = 1;
        descriptor[441] = 6;
        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[442..444], 1);
        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[444..446], partitionNumber);
    }

    private static void WriteBasicVolume(byte[] image, uint partitionStart, uint partitionLength, uint fileSetDescriptorBlock)
    {
        WriteVolumeStructureDescriptor(image, 16, "BEA01");
        WriteVolumeStructureDescriptor(image, 17, "NSR02");
        WriteVolumeStructureDescriptor(image, 18, "TEA01");

        WriteAnchorVolumeDescriptorPointer(image, 256, 300, 4);
        WritePrimaryVolumeDescriptor(image, 300, "TESTVOL");
        WritePartitionDescriptor(image, 301, partitionStart, partitionLength);
        WriteLogicalVolumeDescriptor(image, 302, "TESTVOL", 2048, 0, fileSetDescriptorBlock);
        WriteTerminatingDescriptor(image, 303);
    }

    private static void WriteLogicalVolumeDescriptorWithMetadataPartition(byte[] image, uint block, string volumeLabel, uint logicalBlockSize, ushort physicalPartitionNumber, uint metadataFileBlock, uint fileSetDescriptorBlock)
    {
        Span<byte> descriptor = image.AsSpan((int)(block * 2048), 2048);
        descriptor.Clear();
        WriteDescriptorTag(descriptor, UdfUtilities.LogicalVolumeDescriptor, block);
        WriteDString(descriptor[84..212], volumeLabel);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[212..216], logicalBlockSize);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[264..268], 70);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[268..272], 2);
        WriteLongAllocationDescriptor(descriptor[248..264], 2048, fileSetDescriptorBlock, 1);

        descriptor[440] = 1;
        descriptor[441] = 6;
        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[442..444], 1);
        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[444..446], physicalPartitionNumber);

        descriptor[446] = 2;
        descriptor[447] = 64;
        WriteEntityIdentifier(descriptor[450..482], UdfUtilities.MetadataPartitionIdentifier);
        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[482..484], 1);
        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[484..486], physicalPartitionNumber);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[486..490], metadataFileBlock);
    }

    private static void WriteLogicalVolumeDescriptorWithVirtualPartition(byte[] image, uint block, string volumeLabel, uint logicalBlockSize, ushort physicalPartitionNumber, uint fileSetDescriptorBlock)
    {
        Span<byte> descriptor = image.AsSpan((int)(block * 2048), 2048);
        descriptor.Clear();
        WriteDescriptorTag(descriptor, UdfUtilities.LogicalVolumeDescriptor, block);
        WriteDString(descriptor[84..212], volumeLabel);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[212..216], logicalBlockSize);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[264..268], 70);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[268..272], 2);
        WriteLongAllocationDescriptor(descriptor[248..264], 2048, fileSetDescriptorBlock, 1);

        descriptor[440] = 1;
        descriptor[441] = 6;
        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[442..444], 1);
        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[444..446], physicalPartitionNumber);

        descriptor[446] = 2;
        descriptor[447] = 64;
        WriteEntityIdentifier(descriptor[450..482], UdfUtilities.VirtualPartitionIdentifier);
        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[482..484], 1);
        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[484..486], physicalPartitionNumber);
    }

    private static void WriteTerminatingDescriptor(byte[] image, uint block)
    {
        Span<byte> descriptor = image.AsSpan((int)(block * 2048), 2048);
        descriptor.Clear();
        WriteDescriptorTag(descriptor, UdfUtilities.TerminatingDescriptor, block);
    }

    private static void WriteFileSetDescriptor(byte[] image, uint block, uint rootDirectoryBlock, ushort partitionReferenceNumber)
    {
        Span<byte> descriptor = image.AsSpan((int)(block * 2048), 2048);
        descriptor.Clear();
        WriteDescriptorTag(descriptor, UdfUtilities.FileSetDescriptor, block);
        WriteLongAllocationDescriptor(descriptor[400..416], 2048, rootDirectoryBlock, partitionReferenceNumber);
    }

    private static void WriteFileEntry(byte[] image, uint block, ushort partitionReferenceNumber, byte fileType, ulong informationLength, uint dataBlock, int dataLength)
    {
        Span<byte> descriptor = image.AsSpan((int)(block * 2048), 2048);
        descriptor.Clear();
        WriteDescriptorTag(descriptor, UdfUtilities.FileEntry, block);
        descriptor[27] = fileType;
        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[34..36], UdfUtilities.IcbAllocationDescriptorShort);
        BinaryPrimitives.WriteUInt64LittleEndian(descriptor[56..64], informationLength);
        BinaryPrimitives.WriteUInt64LittleEndian(descriptor[64..72], 1);
        WriteTimestamp(descriptor[84..96], new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Utc));
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[168..172], 0);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[172..176], 8);
        WriteShortAllocationDescriptor(descriptor[176..184], (uint)dataLength, dataBlock);
        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[30..32], partitionReferenceNumber);
    }

    private static void WriteFileEntryWithShortDescriptors(byte[] image, uint block, ushort partitionReferenceNumber, byte fileType, ulong informationLength, params (uint LogicalBlock, byte[] Data)[] extents)
    {
        Span<byte> descriptor = image.AsSpan((int)(block * 2048), 2048);
        descriptor.Clear();
        WriteDescriptorTag(descriptor, UdfUtilities.FileEntry, block);
        descriptor[27] = fileType;
        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[34..36], UdfUtilities.IcbAllocationDescriptorShort);
        BinaryPrimitives.WriteUInt64LittleEndian(descriptor[56..64], informationLength);
        BinaryPrimitives.WriteUInt64LittleEndian(descriptor[64..72], (ulong)extents.Length);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[168..172], 0);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[172..176], (uint)(extents.Length * 8));

        for (int i = 0; i < extents.Length; i++)
        {
            WriteShortAllocationDescriptor(descriptor.Slice(176 + (i * 8), 8), (uint)extents[i].Data.Length, extents[i].LogicalBlock);
            WriteBytes(image, 400 + extents[i].LogicalBlock, extents[i].Data);
        }

        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[30..32], partitionReferenceNumber);
    }

    private static void WriteFileEntryWithShortDescriptorBytes(byte[] image, uint block, ushort partitionReferenceNumber, byte fileType, ulong informationLength, params byte[][] descriptors)
    {
        Span<byte> descriptor = image.AsSpan((int)(block * 2048), 2048);
        descriptor.Clear();
        WriteDescriptorTag(descriptor, UdfUtilities.FileEntry, block);
        descriptor[27] = fileType;
        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[34..36], UdfUtilities.IcbAllocationDescriptorShort);
        BinaryPrimitives.WriteUInt64LittleEndian(descriptor[56..64], informationLength);
        BinaryPrimitives.WriteUInt64LittleEndian(descriptor[64..72], (ulong)descriptors.Length);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[168..172], 0);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[172..176], (uint)(descriptors.Length * 8));

        for (int i = 0; i < descriptors.Length; i++)
        {
            descriptors[i].CopyTo(descriptor.Slice(176 + (i * 8), 8));
        }

        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[30..32], partitionReferenceNumber);
    }

    private static void WriteFileEntryWithLongDescriptors(byte[] image, uint block, ushort partitionReferenceNumber, byte fileType, ulong informationLength, params byte[][] descriptors)
    {
        Span<byte> descriptor = image.AsSpan((int)(block * 2048), 2048);
        descriptor.Clear();
        WriteDescriptorTag(descriptor, UdfUtilities.FileEntry, block);
        descriptor[27] = fileType;
        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[34..36], UdfUtilities.IcbAllocationDescriptorLong);
        BinaryPrimitives.WriteUInt64LittleEndian(descriptor[56..64], informationLength);
        BinaryPrimitives.WriteUInt64LittleEndian(descriptor[64..72], 1);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[168..172], 0);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[172..176], (uint)(descriptors.Length * 16));

        for (int i = 0; i < descriptors.Length; i++)
        {
            descriptors[i].CopyTo(descriptor.Slice(176 + (i * 16), 16));
        }

        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[30..32], partitionReferenceNumber);
    }

    private static void WriteFileEntryWithExtendedDescriptors(byte[] image, uint block, ushort partitionReferenceNumber, byte fileType, ulong informationLength, params byte[][] descriptors)
    {
        Span<byte> descriptor = image.AsSpan((int)(block * 2048), 2048);
        descriptor.Clear();
        WriteDescriptorTag(descriptor, UdfUtilities.FileEntry, block);
        descriptor[27] = fileType;
        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[34..36], UdfUtilities.IcbAllocationDescriptorExtended);
        BinaryPrimitives.WriteUInt64LittleEndian(descriptor[56..64], informationLength);
        BinaryPrimitives.WriteUInt64LittleEndian(descriptor[64..72], 1);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[168..172], 0);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[172..176], (uint)(descriptors.Length * 20));

        for (int i = 0; i < descriptors.Length; i++)
        {
            descriptors[i].CopyTo(descriptor.Slice(176 + (i * 20), 20));
        }

        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[30..32], partitionReferenceNumber);
    }

    private static void WriteAllocationExtentDescriptor(byte[] image, uint block, params byte[][] descriptors)
    {
        Span<byte> descriptor = image.AsSpan((int)(block * 2048), 2048);
        descriptor.Clear();
        WriteDescriptorTag(descriptor, UdfUtilities.AllocationExtentDescriptor, block);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[16..20], 0);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[20..24], (uint)(descriptors.Sum(entry => entry.Length)));

        int offset = 24;
        foreach (byte[] entry in descriptors)
        {
            entry.CopyTo(descriptor.Slice(offset, entry.Length));
            offset += entry.Length;
        }
    }

    private static byte[] BuildDirectoryEntries(params byte[][] entries)
    {
        byte[] contents = new byte[2048];
        int offset = 0;
        foreach (byte[] entry in entries)
        {
            entry.CopyTo(contents, offset);
            offset += entry.Length;
        }

        return contents[..offset];
    }

    private static byte[] BuildFileIdentifierDescriptor(string name, uint targetBlock, ushort partitionReferenceNumber, byte fileCharacteristics)
    {
        byte[] encodedName = EncodeCompressedUnicode(name);
        int descriptorLength = Align(38 + encodedName.Length, 4);
        byte[] descriptor = new byte[descriptorLength];
        WriteDescriptorTag(descriptor, UdfUtilities.FileIdentifierDescriptor, 0);
        descriptor[18] = fileCharacteristics;
        descriptor[19] = (byte)encodedName.Length;
        WriteLongAllocationDescriptor(descriptor.AsSpan(20, 16), 2048, targetBlock, partitionReferenceNumber);
        encodedName.CopyTo(descriptor.AsSpan(38, encodedName.Length));
        return descriptor;
    }

    private static byte[] EncodeCompressedUnicode(string value)
    {
        byte[] textBytes = Encoding.ASCII.GetBytes(value);
        byte[] encoded = new byte[textBytes.Length + 1];
        encoded[0] = 8;
        textBytes.CopyTo(encoded, 1);
        return encoded;
    }

    private static void WriteDescriptorTag(Span<byte> descriptor, ushort tagIdentifier, uint tagLocation)
    {
        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[0..2], tagIdentifier);
        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[2..4], 3);
        descriptor[4] = 0;
        descriptor[5] = 0;
        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[6..8], 1);
        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[8..10], 0);
        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[10..12], 0);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[12..16], tagLocation);

        byte checksum = 0;
        for (int i = 0; i < 16; i++)
        {
            if (i != 4)
            {
                checksum += descriptor[i];
            }
        }

        descriptor[4] = checksum;
    }

    private static void WriteLongAllocationDescriptor(Span<byte> destination, uint length, uint logicalBlockNumber, ushort partitionReferenceNumber)
    {
        BinaryPrimitives.WriteUInt32LittleEndian(destination[0..4], length);
        BinaryPrimitives.WriteUInt32LittleEndian(destination[4..8], logicalBlockNumber);
        BinaryPrimitives.WriteUInt16LittleEndian(destination[8..10], partitionReferenceNumber);
    }

    private static byte[] BuildLongAllocationDescriptor(uint length, uint logicalBlockNumber, ushort partitionReferenceNumber)
    {
        byte[] descriptor = new byte[16];
        WriteLongAllocationDescriptor(descriptor, length, logicalBlockNumber, partitionReferenceNumber);
        return descriptor;
    }

    private static byte[] BuildShortAllocationDescriptor(uint length, uint logicalBlockNumber)
    {
        byte[] descriptor = new byte[8];
        WriteShortAllocationDescriptor(descriptor, length, logicalBlockNumber);
        return descriptor;
    }

    private static byte[] BuildExtendedAllocationDescriptor(uint length, uint recordedLength, uint informationLength, uint logicalBlockNumber, ushort partitionReferenceNumber)
    {
        byte[] descriptor = new byte[20];
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor.AsSpan(0, 4), length);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor.AsSpan(4, 4), recordedLength);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor.AsSpan(8, 4), informationLength);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor.AsSpan(12, 4), logicalBlockNumber);
        BinaryPrimitives.WriteUInt16LittleEndian(descriptor.AsSpan(16, 2), partitionReferenceNumber);
        return descriptor;
    }

    private static void WriteShortAllocationDescriptor(Span<byte> destination, uint length, uint logicalBlockNumber)
    {
        BinaryPrimitives.WriteUInt32LittleEndian(destination[0..4], length);
        BinaryPrimitives.WriteUInt32LittleEndian(destination[4..8], logicalBlockNumber);
    }

    private static void WriteEntityIdentifier(Span<byte> destination, string identifier)
    {
        destination.Clear();
        byte[] bytes = Encoding.ASCII.GetBytes(identifier);
        destination[0] = 0;
        bytes.CopyTo(destination[1..]);
    }

    private static void WriteDString(Span<byte> destination, string value)
    {
        destination.Clear();
        byte[] bytes = Encoding.ASCII.GetBytes(value);
        destination[0] = 8;
        bytes.CopyTo(destination[1..]);
        destination[^1] = (byte)(bytes.Length + 1);
    }

    private static void WriteTimestamp(Span<byte> destination, DateTime value)
    {
        DateTime utc = value.ToUniversalTime();
        BinaryPrimitives.WriteUInt16LittleEndian(destination[0..2], 0x1000);
        BinaryPrimitives.WriteUInt16LittleEndian(destination[2..4], (ushort)utc.Year);
        destination[4] = (byte)utc.Month;
        destination[5] = (byte)utc.Day;
        destination[6] = (byte)utc.Hour;
        destination[7] = (byte)utc.Minute;
        destination[8] = (byte)utc.Second;
        destination[9] = 0;
        destination[10] = 0;
        destination[11] = 0;
    }

    private static void WriteBytes(byte[] image, uint block, byte[] bytes)
    {
        bytes.CopyTo(image, block * 2048);
    }

    private static void WriteMetadataFileEntry(byte[] image, uint block, ushort partitionReferenceNumber, uint firstDataBlock, uint blockCount)
    {
        Span<byte> descriptor = image.AsSpan((int)(block * 2048), 2048);
        descriptor.Clear();
        WriteDescriptorTag(descriptor, UdfUtilities.FileEntry, block);
        descriptor[27] = 0xFA;
        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[34..36], UdfUtilities.IcbAllocationDescriptorShort);
        BinaryPrimitives.WriteUInt64LittleEndian(descriptor[56..64], (ulong)(blockCount * 2048));
        BinaryPrimitives.WriteUInt64LittleEndian(descriptor[64..72], blockCount);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[168..172], 0);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[172..176], 8);
        WriteShortAllocationDescriptor(descriptor[176..184], blockCount * 2048, firstDataBlock);
        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[30..32], partitionReferenceNumber);
    }

    private static void WriteMetadataFileEntryWithLongDescriptors(byte[] image, uint block, ushort partitionReferenceNumber, params byte[][] descriptors)
    {
        Span<byte> descriptor = image.AsSpan((int)(block * 2048), 2048);
        descriptor.Clear();
        WriteDescriptorTag(descriptor, UdfUtilities.FileEntry, block);
        descriptor[27] = 0xFA;
        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[34..36], UdfUtilities.IcbAllocationDescriptorLong);
        BinaryPrimitives.WriteUInt64LittleEndian(descriptor[56..64], 5UL * 2048UL);
        BinaryPrimitives.WriteUInt64LittleEndian(descriptor[64..72], 1);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[168..172], 0);
        BinaryPrimitives.WriteUInt32LittleEndian(descriptor[172..176], (uint)(descriptors.Length * 16));

        for (int i = 0; i < descriptors.Length; i++)
        {
            descriptors[i].CopyTo(descriptor.Slice(176 + (i * 16), 16));
        }

        BinaryPrimitives.WriteUInt16LittleEndian(descriptor[30..32], partitionReferenceNumber);
    }

    private static byte[] BuildVatContents(ushort headerLength, uint[] entries)
    {
        byte[] data = new byte[headerLength + (entries.Length * 4)];
        BinaryPrimitives.WriteUInt16LittleEndian(data.AsSpan(0, 2), headerLength);
        for (int i = 0; i < entries.Length; i++)
        {
            BinaryPrimitives.WriteUInt32LittleEndian(data.AsSpan(headerLength + (i * 4), 4), entries[i]);
        }

        return data;
    }

    private static int Align(int value, int alignment) => ((value + alignment - 1) / alignment) * alignment;
}
