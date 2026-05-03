using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TotalImage.FileSystems;
using TotalImage.FileSystems.NTFS;
using Xunit;

namespace TotalImage.IO.Tests.Ntfs;

public class NtfsFileSystemTests
{
    [Fact]
    public void AttemptDetection_LoadsNtfsFileSystemAndVolumeMetadata()
    {
        using var stream = new MemoryStream(CreateTestImage());

        var fileSystem = FileSystem.AttemptDetection(stream);

        var ntfs = Assert.IsType<NtfsFileSystem>(fileSystem);
        Assert.Equal("NTFS", ntfs.DisplayName);
        Assert.Equal("TESTVOL", ntfs.VolumeLabel);
        Assert.True(ntfs.IsReadOnly);
        Assert.True(ntfs.SupportsSubdirectories);
        Assert.Equal(512, ntfs.AllocationUnitSize);
        Assert.Equal(64 * 512, ntfs.TotalSize);
        Assert.Equal(37 * 512, ntfs.TotalFreeSpace);
    }

    [Fact]
    public void RootDirectory_EnumeratesFilesAndDirectories()
    {
        using var stream = new MemoryStream(CreateTestImage());

        var fileSystem = Assert.IsType<NtfsFileSystem>(FileSystem.AttemptDetection(stream));
        var entries = fileSystem.RootDirectory.EnumerateFileSystemObjects(showHidden: true).OrderBy(entry => entry.Name).ToArray();

        Assert.Equal(4, entries.Length);
        Assert.Collection(
            entries,
            entry => Assert.IsType<NtfsFile>(entry),
            entry => Assert.IsType<NtfsDirectory>(entry),
            entry => Assert.IsType<NtfsFile>(entry),
            entry => Assert.IsType<NtfsFile>(entry));
        Assert.Equal("$Bitmap", entries[0].Name);
        Assert.Equal("docs", entries[1].Name);
        Assert.Equal("hello.txt", entries[2].Name);
        Assert.Equal("large.bin", entries[3].Name);
        Assert.Equal("\\hello.txt", entries[2].FullName);
    }

    [Fact]
    public void FileAndSubdirectoryStreams_ReturnExpectedContents()
    {
        using var stream = new MemoryStream(CreateTestImage());

        var fileSystem = Assert.IsType<NtfsFileSystem>(FileSystem.AttemptDetection(stream));
        var rootEntries = fileSystem.RootDirectory.EnumerateFileSystemObjects(showHidden: true).ToArray();
        var helloFile = Assert.IsType<NtfsFile>(rootEntries.Single(entry => entry.Name == "hello.txt"));
        var docsDirectory = Assert.IsType<NtfsDirectory>(rootEntries.Single(entry => entry.Name == "docs"));
        var readme = Assert.IsType<NtfsFile>(docsDirectory.EnumerateFileSystemObjects(showHidden: true).Single());

        using var helloReader = new StreamReader(helloFile.GetStream(), Encoding.ASCII, false, leaveOpen: false);
        using var readmeReader = new StreamReader(readme.GetStream(), Encoding.ASCII, false, leaveOpen: false);

        Assert.Equal("Hello NTFS", helloReader.ReadToEnd());
        Assert.Equal("Nested readme", readmeReader.ReadToEnd());
        Assert.Equal("\\docs\\readme.txt", readme.FullName);
    }

    [Fact]
    public void NonResidentFileStreams_ReadAcrossFragmentedRuns()
    {
        using var stream = new MemoryStream(CreateTestImage());

        var fileSystem = Assert.IsType<NtfsFileSystem>(FileSystem.AttemptDetection(stream));
        var file = Assert.IsType<NtfsFile>(fileSystem.RootDirectory.EnumerateFiles(showHidden: true).Single(entry => entry.Name == "large.bin"));

        using var reader = new StreamReader(file.GetStream(), Encoding.ASCII, false, leaveOpen: false);
        string contents = reader.ReadToEnd();

        Assert.Equal(700, contents.Length);
        Assert.Equal(new string('A', 512) + new string('B', 188), contents);
    }

    [Fact]
    public void RootDirectory_EnumeratesEntriesStoredInIndexAllocation()
    {
        using var stream = new MemoryStream(CreateIndexAllocationDirectoryImage());

        var fileSystem = Assert.IsType<NtfsFileSystem>(FileSystem.AttemptDetection(stream));
        var entries = fileSystem.RootDirectory.EnumerateFileSystemObjects(showHidden: true).OrderBy(entry => entry.Name).ToArray();

        Assert.Equal(["$Bitmap", "docs", "hello.txt", "large.bin"], entries.Select(entry => entry.Name).ToArray());
    }

    [Fact]
    public void DirectoriesAndMetadataFiles_ReportExpectedAttributes()
    {
        using var stream = new MemoryStream(CreateTestImage());

        var fileSystem = Assert.IsType<NtfsFileSystem>(FileSystem.AttemptDetection(stream));
        var entries = fileSystem.RootDirectory.EnumerateFileSystemObjects(showHidden: true).ToArray();
        var docs = Assert.IsType<NtfsDirectory>(entries.Single(entry => entry.Name == "docs"));
        var bitmap = Assert.IsType<NtfsFile>(entries.Single(entry => entry.Name == "$Bitmap"));

        Assert.True(docs.Attributes.HasFlag(FileAttributes.Directory));
        Assert.True(bitmap.Attributes.HasFlag(FileAttributes.Hidden));
        Assert.True(bitmap.Attributes.HasFlag(FileAttributes.System));
    }

    private static byte[] CreateTestImage()
    {
        const int bytesPerSector = 512;
        const int sectorsPerCluster = 1;
        const int totalClusters = 64;
        const int mftStartCluster = 4;
        const int mftClusterCount = 24;
        const int fileRecordSize = 1024;

        byte[] image = new byte[totalClusters * bytesPerSector];

        WriteBootSector(image, bytesPerSector, sectorsPerCluster, totalClusters, mftStartCluster, fileRecordSize);

        byte[] helloData = Encoding.ASCII.GetBytes("Hello NTFS");
        byte[] readmeData = Encoding.ASCII.GetBytes("Nested readme");
        byte[] largeFirstCluster = Encoding.ASCII.GetBytes(new string('A', 512));
        byte[] largeSecondCluster = Encoding.ASCII.GetBytes(new string('B', 188));

        byte[] volumeRecord = BuildFileRecord(3, false, [
            BuildStandardInformationAttribute(FileAttributes.System | FileAttributes.Hidden),
            BuildResidentAttribute(0x60, Encoding.Unicode.GetBytes("TESTVOL"))
        ]);

        byte[] rootRecord = BuildFileRecord(5, true, [
            BuildStandardInformationAttribute(FileAttributes.Directory),
            BuildResidentAttribute(0x90, BuildIndexRoot([
                BuildIndexEntry(8, "docs", FileAttributes.Directory, 0),
                BuildIndexEntry(6, "$Bitmap", FileAttributes.Archive, 8),
                BuildIndexEntry(7, "hello.txt", FileAttributes.Archive, (ulong)helloData.Length),
                BuildIndexEntry(10, "large.bin", FileAttributes.Archive, 700)
            ]))
        ]);

        byte[] bitmapRecord = BuildFileRecord(6, false, [
            BuildStandardInformationAttribute(FileAttributes.Hidden | FileAttributes.System),
            BuildResidentAttribute(0x80, BuildBitmap(totalClusters, [0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 30, 32]))
        ]);

        byte[] helloRecord = BuildFileRecord(7, false, [
            BuildStandardInformationAttribute(FileAttributes.Archive),
            BuildResidentAttribute(0x80, helloData)
        ]);

        byte[] docsRecord = BuildFileRecord(8, true, [
            BuildStandardInformationAttribute(FileAttributes.Directory),
            BuildResidentAttribute(0x90, BuildIndexRoot([
                BuildIndexEntry(9, "readme.txt", FileAttributes.Archive, (ulong)readmeData.Length)
            ]))
        ]);

        byte[] readmeRecord = BuildFileRecord(9, false, [
            BuildStandardInformationAttribute(FileAttributes.Archive),
            BuildResidentAttribute(0x80, readmeData)
        ]);

        byte[] largeRecord = BuildFileRecord(10, false, [
            BuildStandardInformationAttribute(FileAttributes.Archive),
            BuildNonResidentDataAttribute(700, [
                (30, 1),
                (32, 1)
            ])
        ]);

        WriteFileRecord(image, mftStartCluster, 0, BuildFileRecord(0, false, [
            BuildStandardInformationAttribute(FileAttributes.Hidden | FileAttributes.System),
            BuildNonResidentDataAttribute((ulong)(mftClusterCount * bytesPerSector), [
                (mftStartCluster, mftClusterCount)
            ])
        ]));
        WriteFileRecord(image, mftStartCluster, 3, volumeRecord);
        WriteFileRecord(image, mftStartCluster, 5, rootRecord);
        WriteFileRecord(image, mftStartCluster, 6, bitmapRecord);
        WriteFileRecord(image, mftStartCluster, 7, helloRecord);
        WriteFileRecord(image, mftStartCluster, 8, docsRecord);
        WriteFileRecord(image, mftStartCluster, 9, readmeRecord);
        WriteFileRecord(image, mftStartCluster, 10, largeRecord);

        largeFirstCluster.CopyTo(image, 30 * bytesPerSector);
        largeSecondCluster.CopyTo(image, 32 * bytesPerSector);

        return image;
    }

    private static byte[] CreateIndexAllocationDirectoryImage()
    {
        const int bytesPerSector = 512;
        const int sectorsPerCluster = 1;
        const int totalClusters = 64;
        const int mftStartCluster = 4;
        const int mftClusterCount = 24;
        const int fileRecordSize = 1024;
        const int indexRecordClusters = 8;
        const int bytesPerIndexRecord = indexRecordClusters * bytesPerSector;

        byte[] image = new byte[totalClusters * bytesPerSector];

        WriteBootSector(image, bytesPerSector, sectorsPerCluster, totalClusters, mftStartCluster, fileRecordSize);

        byte[] helloData = Encoding.ASCII.GetBytes("Hello NTFS");
        byte[] readmeData = Encoding.ASCII.GetBytes("Nested readme");
        byte[] largeFirstCluster = Encoding.ASCII.GetBytes(new string('A', 512));
        byte[] largeSecondCluster = Encoding.ASCII.GetBytes(new string('B', 188));

        byte[] volumeRecord = BuildFileRecord(3, false, [
            BuildStandardInformationAttribute(FileAttributes.System | FileAttributes.Hidden),
            BuildResidentAttribute(0x60, Encoding.Unicode.GetBytes("TESTVOL"))
        ]);

        byte[] rootRecord = BuildFileRecord(5, true, [
            BuildStandardInformationAttribute(FileAttributes.Directory),
            BuildResidentAttribute(0x90, BuildIndexRoot([], bytesPerIndexRecord, hasChildren: true)),
            BuildNonResidentNamedAttribute(0xA0, "$I30", (ulong)bytesPerIndexRecord, [
                (40, indexRecordClusters)
            ])
        ]);

        byte[] bitmapRecord = BuildFileRecord(6, false, [
            BuildStandardInformationAttribute(FileAttributes.Hidden | FileAttributes.System),
            BuildResidentAttribute(0x80, BuildBitmap(totalClusters, [0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 30, 32, 40, 41, 42, 43, 44, 45, 46, 47]))
        ]);

        byte[] helloRecord = BuildFileRecord(7, false, [
            BuildStandardInformationAttribute(FileAttributes.Archive),
            BuildResidentAttribute(0x80, helloData)
        ]);

        byte[] docsRecord = BuildFileRecord(8, true, [
            BuildStandardInformationAttribute(FileAttributes.Directory),
            BuildResidentAttribute(0x90, BuildIndexRoot([
                BuildIndexEntry(9, "readme.txt", FileAttributes.Archive, (ulong)readmeData.Length)
            ]))
        ]);

        byte[] readmeRecord = BuildFileRecord(9, false, [
            BuildStandardInformationAttribute(FileAttributes.Archive),
            BuildResidentAttribute(0x80, readmeData)
        ]);

        byte[] largeRecord = BuildFileRecord(10, false, [
            BuildStandardInformationAttribute(FileAttributes.Archive),
            BuildNonResidentDataAttribute(700, [
                (30, 1),
                (32, 1)
            ])
        ]);

        WriteFileRecord(image, mftStartCluster, 0, BuildFileRecord(0, false, [
            BuildStandardInformationAttribute(FileAttributes.Hidden | FileAttributes.System),
            BuildNonResidentDataAttribute((ulong)(mftClusterCount * bytesPerSector), [
                (mftStartCluster, mftClusterCount)
            ])
        ]));
        WriteFileRecord(image, mftStartCluster, 3, volumeRecord);
        WriteFileRecord(image, mftStartCluster, 5, rootRecord);
        WriteFileRecord(image, mftStartCluster, 6, bitmapRecord);
        WriteFileRecord(image, mftStartCluster, 7, helloRecord);
        WriteFileRecord(image, mftStartCluster, 8, docsRecord);
        WriteFileRecord(image, mftStartCluster, 9, readmeRecord);
        WriteFileRecord(image, mftStartCluster, 10, largeRecord);

        byte[] rootIndexRecord = BuildIndexRecord(0, bytesPerIndexRecord, [
            BuildIndexEntry(8, "docs", FileAttributes.Directory, 0),
            BuildIndexEntry(6, "$Bitmap", FileAttributes.Archive, 8),
            BuildIndexEntry(7, "hello.txt", FileAttributes.Archive, (ulong)helloData.Length),
            BuildIndexEntry(10, "large.bin", FileAttributes.Archive, 700)
        ]);

        rootIndexRecord.CopyTo(image, 40 * bytesPerSector);
        largeFirstCluster.CopyTo(image, 30 * bytesPerSector);
        largeSecondCluster.CopyTo(image, 32 * bytesPerSector);

        return image;
    }

    private static void WriteBootSector(byte[] image, ushort bytesPerSector, byte sectorsPerCluster, long totalClusters, long mftLogicalClusterNumber, int bytesPerFileRecord)
    {
        Span<byte> bootSector = image.AsSpan(0, 512);
        bootSector.Clear();
        bootSector[0] = 0xEB;
        bootSector[1] = 0x52;
        bootSector[2] = 0x90;
        Encoding.ASCII.GetBytes("NTFS    ", bootSector[3..11]);
        BinaryPrimitives.WriteUInt16LittleEndian(bootSector[0x0B..0x0D], bytesPerSector);
        bootSector[0x0D] = sectorsPerCluster;
        BinaryPrimitives.WriteInt64LittleEndian(bootSector[0x28..0x30], totalClusters * sectorsPerCluster);
        BinaryPrimitives.WriteInt64LittleEndian(bootSector[0x30..0x38], mftLogicalClusterNumber);
        bootSector[0x40] = unchecked((byte)-10); // 2^10 = 1024 bytes per file record.
        BinaryPrimitives.WriteUInt16LittleEndian(bootSector[510..512], 0xAA55);
    }

    private static void WriteFileRecord(byte[] image, int mftStartCluster, int recordNumber, byte[] record)
    {
        int offset = (mftStartCluster * 512) + (recordNumber * 1024);
        record.CopyTo(image, offset);
    }

    private static byte[] BuildFileRecord(int recordNumber, bool isDirectory, IEnumerable<byte[]> attributes)
    {
        byte[] record = new byte[1024];
        Span<byte> span = record;
        span.Clear();
        "FILE"u8.CopyTo(span);

        BinaryPrimitives.WriteUInt16LittleEndian(span[0x10..0x12], 1);
        BinaryPrimitives.WriteUInt16LittleEndian(span[0x12..0x14], 1);
        BinaryPrimitives.WriteUInt16LittleEndian(span[0x14..0x16], 0x38);
        BinaryPrimitives.WriteUInt16LittleEndian(span[0x16..0x18], (ushort)(0x01 | (isDirectory ? 0x02 : 0x00)));
        BinaryPrimitives.WriteUInt32LittleEndian(span[0x1C..0x20], 1024);
        BinaryPrimitives.WriteUInt32LittleEndian(span[0x2C..0x30], (uint)recordNumber);

        int offset = 0x38;
        ushort attributeId = 0;
        foreach (byte[] attribute in attributes)
        {
            BinaryPrimitives.WriteUInt16LittleEndian(attribute.AsSpan(0x0E, 2), attributeId++);
            attribute.CopyTo(record, offset);
            offset += attribute.Length;
        }

        BinaryPrimitives.WriteUInt32LittleEndian(span[offset..(offset + 4)], 0xFFFFFFFF);
        offset += 4;
        BinaryPrimitives.WriteUInt32LittleEndian(span[0x18..0x1C], (uint)offset);

        ApplyFixup(record, 0x30, 0x30);
        return record;
    }

    private static void ApplyFixup(byte[] record, ushort sequenceNumber, ushort usaOffset)
    {
        Span<byte> span = record;
        BinaryPrimitives.WriteUInt16LittleEndian(span[4..6], usaOffset);
        BinaryPrimitives.WriteUInt16LittleEndian(span[6..8], 3);
        BinaryPrimitives.WriteUInt16LittleEndian(span.Slice(usaOffset, 2), sequenceNumber);

        ushort sector0 = BinaryPrimitives.ReadUInt16LittleEndian(span[510..512]);
        ushort sector1 = BinaryPrimitives.ReadUInt16LittleEndian(span[1022..1024]);
        BinaryPrimitives.WriteUInt16LittleEndian(span.Slice(usaOffset + 2, 2), sector0);
        BinaryPrimitives.WriteUInt16LittleEndian(span.Slice(usaOffset + 4, 2), sector1);
        BinaryPrimitives.WriteUInt16LittleEndian(span[510..512], sequenceNumber);
        BinaryPrimitives.WriteUInt16LittleEndian(span[1022..1024], sequenceNumber);
    }

    private static byte[] BuildStandardInformationAttribute(FileAttributes attributes)
    {
        byte[] value = new byte[48];
        long fileTime = new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Utc).ToFileTimeUtc();
        BinaryPrimitives.WriteInt64LittleEndian(value.AsSpan(0, 8), fileTime);
        BinaryPrimitives.WriteInt64LittleEndian(value.AsSpan(8, 8), fileTime);
        BinaryPrimitives.WriteInt64LittleEndian(value.AsSpan(16, 8), fileTime);
        BinaryPrimitives.WriteInt64LittleEndian(value.AsSpan(24, 8), fileTime);
        BinaryPrimitives.WriteUInt32LittleEndian(value.AsSpan(32, 4), (uint)attributes);
        return BuildResidentAttribute(0x10, value);
    }

    private static byte[] BuildResidentAttribute(uint type, byte[] value)
    {
        int valueOffset = 0x18;
        int length = Align(valueOffset + value.Length, 8);
        byte[] attribute = new byte[length];

        BinaryPrimitives.WriteUInt32LittleEndian(attribute.AsSpan(0, 4), type);
        BinaryPrimitives.WriteUInt32LittleEndian(attribute.AsSpan(4, 4), (uint)length);
        attribute[8] = 0;
        attribute[9] = 0;
        BinaryPrimitives.WriteUInt16LittleEndian(attribute.AsSpan(10, 2), 0);
        BinaryPrimitives.WriteUInt16LittleEndian(attribute.AsSpan(12, 2), 0);
        BinaryPrimitives.WriteUInt32LittleEndian(attribute.AsSpan(16, 4), (uint)value.Length);
        BinaryPrimitives.WriteUInt16LittleEndian(attribute.AsSpan(20, 2), (ushort)valueOffset);
        value.CopyTo(attribute, valueOffset);

        return attribute;
    }

    private static byte[] BuildNonResidentDataAttribute(ulong dataSize, IReadOnlyList<(int Lcn, int Length)> runs)
        => BuildNonResidentNamedAttribute(0x80, null, dataSize, runs);

    private static byte[] BuildNonResidentNamedAttribute(uint type, string? name, ulong dataSize, IReadOnlyList<(int Lcn, int Length)> runs)
    {
        byte[] runList = BuildRunList(runs);
        byte[] nameBytes = string.IsNullOrEmpty(name) ? [] : Encoding.Unicode.GetBytes(name);
        int nameOffset = 0x40;
        int valueOffset = Align(nameOffset + nameBytes.Length, 8);
        int length = Align(valueOffset + runList.Length, 8);
        byte[] attribute = new byte[length];

        BinaryPrimitives.WriteUInt32LittleEndian(attribute.AsSpan(0, 4), type);
        BinaryPrimitives.WriteUInt32LittleEndian(attribute.AsSpan(4, 4), (uint)length);
        attribute[8] = 1;
        attribute[9] = (byte)(nameBytes.Length / 2);
        BinaryPrimitives.WriteUInt16LittleEndian(attribute.AsSpan(10, 2), (ushort)(nameBytes.Length == 0 ? 0 : nameOffset));
        BinaryPrimitives.WriteUInt16LittleEndian(attribute.AsSpan(12, 2), 0);
        BinaryPrimitives.WriteUInt64LittleEndian(attribute.AsSpan(16, 8), 0);
        BinaryPrimitives.WriteUInt64LittleEndian(attribute.AsSpan(24, 8), (ulong)(runs.Sum(run => run.Length) - 1));
        BinaryPrimitives.WriteUInt16LittleEndian(attribute.AsSpan(32, 2), (ushort)valueOffset);
        BinaryPrimitives.WriteUInt64LittleEndian(attribute.AsSpan(40, 8), (ulong)(runs.Sum(run => run.Length) * 512));
        BinaryPrimitives.WriteUInt64LittleEndian(attribute.AsSpan(48, 8), dataSize);
        BinaryPrimitives.WriteUInt64LittleEndian(attribute.AsSpan(56, 8), dataSize);
        nameBytes.CopyTo(attribute, nameOffset);
        runList.CopyTo(attribute, valueOffset);

        return attribute;
    }

    private static byte[] BuildRunList(IReadOnlyList<(int Lcn, int Length)> runs)
    {
        List<byte> bytes = new();
        int previousLcn = 0;

        foreach ((int lcn, int length) in runs)
        {
            int delta = lcn - previousLcn;
            previousLcn = lcn;

            byte[] lengthBytes = EncodeUnsigned(length);
            byte[] offsetBytes = EncodeSigned(delta);
            bytes.Add((byte)(lengthBytes.Length | (offsetBytes.Length << 4)));
            bytes.AddRange(lengthBytes);
            bytes.AddRange(offsetBytes);
        }

        bytes.Add(0);
        return bytes.ToArray();
    }

    private static byte[] BuildBitmap(int totalClusters, IEnumerable<int> usedClusters)
    {
        byte[] bitmap = new byte[(totalClusters + 7) / 8];
        foreach (int cluster in usedClusters)
        {
            bitmap[cluster / 8] |= (byte)(1 << (cluster % 8));
        }

        return bitmap;
    }

    private static byte[] BuildIndexRoot(IEnumerable<byte[]> entries, int bytesPerIndexRecord = 4096, bool hasChildren = false)
    {
        List<byte[]> entryList = entries.ToList();
        byte[] terminator = new byte[16];
        BinaryPrimitives.WriteUInt16LittleEndian(terminator.AsSpan(8, 2), 16);
        BinaryPrimitives.WriteUInt16LittleEndian(terminator.AsSpan(12, 2), 0x02);

        byte[] indexEntries = entryList.SelectMany(entry => entry).Concat(terminator).ToArray();
        byte[] value = new byte[0x20 + indexEntries.Length];
        BinaryPrimitives.WriteUInt32LittleEndian(value.AsSpan(0, 4), 0x30);
        BinaryPrimitives.WriteUInt32LittleEndian(value.AsSpan(4, 4), 1);
        BinaryPrimitives.WriteUInt32LittleEndian(value.AsSpan(8, 4), (uint)bytesPerIndexRecord);
        value[12] = 1;
        BinaryPrimitives.WriteUInt32LittleEndian(value.AsSpan(0x10, 4), 0x10);
        BinaryPrimitives.WriteUInt32LittleEndian(value.AsSpan(0x14, 4), (uint)(0x10 + indexEntries.Length));
        BinaryPrimitives.WriteUInt32LittleEndian(value.AsSpan(0x18, 4), (uint)(0x10 + indexEntries.Length));
        value[0x1C] = hasChildren ? (byte)0x01 : (byte)0x00;
        indexEntries.CopyTo(value, 0x20);

        return value;
    }

    private static byte[] BuildIndexRecord(long vcn, int bytesPerIndexRecord, IEnumerable<byte[]> entries)
    {
        List<byte[]> entryList = entries.ToList();
        byte[] terminator = new byte[16];
        BinaryPrimitives.WriteUInt16LittleEndian(terminator.AsSpan(8, 2), 16);
        BinaryPrimitives.WriteUInt16LittleEndian(terminator.AsSpan(12, 2), 0x02);

        byte[] indexEntries = entryList.SelectMany(entry => entry).Concat(terminator).ToArray();
        byte[] record = new byte[bytesPerIndexRecord];
        Span<byte> span = record;
        span.Clear();
        "INDX"u8.CopyTo(span);
        BinaryPrimitives.WriteUInt16LittleEndian(span[0x04..0x06], 0x30);
        BinaryPrimitives.WriteUInt16LittleEndian(span[0x06..0x08], 3);
        BinaryPrimitives.WriteInt64LittleEndian(span[0x10..0x18], vcn);
        BinaryPrimitives.WriteUInt32LittleEndian(span[0x18..0x1C], 0x18);
        BinaryPrimitives.WriteUInt32LittleEndian(span[0x1C..0x20], (uint)(0x18 + indexEntries.Length));
        BinaryPrimitives.WriteUInt32LittleEndian(span[0x20..0x24], (uint)(0x18 + indexEntries.Length));
        indexEntries.CopyTo(record, 0x30);
        ApplyFixup(record, 0x31, 0x28);

        return record;
    }

    private static byte[] BuildIndexEntry(ulong fileReference, string name, FileAttributes attributes, ulong realSize)
    {
        byte[] key = BuildFileNameKey(name, attributes, realSize);
        int length = Align(0x10 + key.Length, 8);
        byte[] entry = new byte[length];
        BinaryPrimitives.WriteUInt64LittleEndian(entry.AsSpan(0, 8), fileReference);
        BinaryPrimitives.WriteUInt16LittleEndian(entry.AsSpan(8, 2), (ushort)length);
        BinaryPrimitives.WriteUInt16LittleEndian(entry.AsSpan(10, 2), (ushort)key.Length);
        key.CopyTo(entry, 0x10);
        return entry;
    }

    private static byte[] BuildFileNameKey(string name, FileAttributes attributes, ulong realSize)
    {
        byte[] nameBytes = Encoding.Unicode.GetBytes(name);
        byte[] value = new byte[0x42 + nameBytes.Length];
        BinaryPrimitives.WriteUInt64LittleEndian(value.AsSpan(0x28, 8), realSize);
        BinaryPrimitives.WriteUInt64LittleEndian(value.AsSpan(0x30, 8), realSize);
        BinaryPrimitives.WriteUInt32LittleEndian(value.AsSpan(0x38, 4), (uint)attributes);
        value[0x40] = (byte)name.Length;
        value[0x41] = 0x01;
        nameBytes.CopyTo(value, 0x42);
        return value;
    }

    private static byte[] EncodeUnsigned(int value)
    {
        List<byte> bytes = new();
        int remaining = value;
        do
        {
            bytes.Add((byte)(remaining & 0xFF));
            remaining >>= 8;
        }
        while (remaining != 0);

        return bytes.ToArray();
    }

    private static byte[] EncodeSigned(int value)
    {
        List<byte> bytes = new();
        int remaining = value;

        while (true)
        {
            byte next = (byte)(remaining & 0xFF);
            bytes.Add(next);
            remaining >>= 8;

            bool done = (remaining == 0 && (next & 0x80) == 0) || (remaining == -1 && (next & 0x80) != 0);
            if (done)
            {
                break;
            }
        }

        return bytes.ToArray();
    }

    private static int Align(int value, int alignment) => ((value + alignment - 1) / alignment) * alignment;
}
