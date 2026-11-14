using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;
using System.Text;
using TotalImage.Containers.TAP;
using TotalImage.FileSystems;
using TotalImage.FileSystems.MTF;
using Xunit;
using TiDirectory = TotalImage.FileSystems.Directory;
using TiFile = TotalImage.FileSystems.File;

namespace TotalImage.IO.Tests.Mtf;

/// <summary>
/// Exercises MTF indexing, tape-backed extraction, metadata, and validation using generated fixtures only.
/// </summary>
public class MtfFileSystemTests
{
    /// <summary>Verifies detection, grouping directories, timestamps, and hidden-file filtering.</summary>
    [Fact]
    public void Detection_LoadsMetadataAndNestedDirectories()
    {
        using var source = new MemoryStream(CreateArchive());
        var fs = Assert.IsType<MtfFileSystem>(FileSystem.AttemptDetection(source));
        Assert.Equal("Test tape", fs.VolumeLabel);
        Assert.True(fs.IsReadOnly);
        Assert.True(fs.SupportsSubdirectories);
        Assert.Equal(1024, fs.AllocationUnitSize);
        Assert.Equal(0, fs.TotalFreeSpace);
        var files = Files(fs.RootDirectory).ToArray();
        Assert.Equal(3, files.Length);
        var file = files.Single(f => f.Name == "hello.txt");
        Assert.Equal("\\Backup set 1\\Volume 1 (C)\\docs\\nested\\hello.txt", file.FullName);
        Assert.Equal(new DateTime(2026, 11, 14, 12, 34, 56), file.LastWriteTime);
        Assert.Equal(new DateTime(2020, 1, 2, 3, 4, 5), file.CreationTime);
        Assert.Equal(new DateTime(2026, 11, 15, 1, 2, 3), file.LastAccessTime);
        Assert.True((file.Attributes & FileAttributes.Hidden) != 0);
        Assert.DoesNotContain(file, file.Directory.EnumerateFiles(false));
        Assert.Equal(2, file.Directory.EnumerateFiles(false).Count());
        Assert.Null(files.Single(f => f.Name == "empty.txt").LastWriteTime);
    }

    /// <summary>Verifies that invalid or absent timestamps return null without throwing first-chance exceptions.</summary>
    /// <param name="year">The raw packed year.</param>
    /// <param name="month">The raw packed month.</param>
    /// <param name="day">The raw packed day.</param>
    /// <param name="hour">The raw packed hour.</param>
    /// <param name="minute">The raw packed minute.</param>
    /// <param name="second">The raw packed second.</param>
    [Theory]
    [InlineData(0, 0, 0, 0, 0, 0)]
    [InlineData(0, 1, 1, 0, 0, 0)]
    [InlineData(10000, 1, 1, 0, 0, 0)]
    [InlineData(16383, 12, 31, 23, 59, 59)]
    [InlineData(2026, 0, 1, 0, 0, 0)]
    [InlineData(2026, 13, 1, 0, 0, 0)]
    [InlineData(2026, 1, 0, 0, 0, 0)]
    [InlineData(2026, 4, 31, 0, 0, 0)]
    [InlineData(2023, 2, 29, 0, 0, 0)]
    [InlineData(1900, 2, 29, 0, 0, 0)]
    [InlineData(2024, 2, 30, 0, 0, 0)]
    [InlineData(2026, 1, 1, 24, 0, 0)]
    [InlineData(2026, 1, 1, 31, 0, 0)]
    [InlineData(2026, 1, 1, 0, 60, 0)]
    [InlineData(2026, 1, 1, 0, 63, 0)]
    [InlineData(2026, 1, 1, 0, 0, 60)]
    [InlineData(2026, 1, 1, 0, 0, 63)]
    public void InvalidTimestamps_ReturnNullWithoutThrowing(int year, int month, int day, int hour, int minute, int second)
    {
        byte[] archive = CreateArchive();
        int[] dateOffsets = [1024 + 88, 3072 + 56, 3072 + 61, 3072 + 71, 4096 + 56, 4096 + 61, 4096 + 71];
        foreach (int offset in dateOffsets)
        {
            PutDate(archive.AsSpan(offset), year, month, day, hour, minute, second);
        }

        int exceptionCount = 0;
        int threadId = Environment.CurrentManagedThreadId;
        EventHandler<FirstChanceExceptionEventArgs> handler = (_, args) =>
        {
            // Ignore exceptions from unrelated tests running on other threads.
            if (Environment.CurrentManagedThreadId == threadId && args.Exception is ArgumentOutOfRangeException)
            {
                exceptionCount++;
            }
        };

        using var source = new MemoryStream(archive);
        MtfFileSystem fs;
        AppDomain.CurrentDomain.FirstChanceException += handler;
        try
        {
            fs = new MtfFileSystem(source);
        }
        finally
        {
            AppDomain.CurrentDomain.FirstChanceException -= handler;
        }

        Assert.Equal(0, exceptionCount);
        Assert.Null(Assert.Single(fs.RootDirectory.EnumerateDirectories(true)).CreationTime);
        var file = Files(fs.RootDirectory).Single(f => f.Name == "hello.txt");
        Assert.Null(file.CreationTime);
        Assert.Null(file.LastWriteTime);
        Assert.Null(file.LastAccessTime);
        Assert.Null(file.Directory.CreationTime);
        Assert.Null(file.Directory.LastWriteTime);
        Assert.Null(file.Directory.LastAccessTime);
    }

    /// <summary>Verifies leap-year rules and the supported civil timestamp boundaries.</summary>
    /// <param name="year">The valid packed year.</param>
    /// <param name="month">The valid packed month.</param>
    /// <param name="day">The valid packed day.</param>
    /// <param name="hour">The valid packed hour.</param>
    /// <param name="minute">The valid packed minute.</param>
    /// <param name="second">The valid packed second.</param>
    [Theory]
    [InlineData(1, 1, 1, 0, 0, 0)]
    [InlineData(9999, 12, 31, 23, 59, 59)]
    [InlineData(1900, 2, 28, 23, 59, 59)]
    [InlineData(2000, 2, 29, 12, 34, 56)]
    [InlineData(2024, 2, 29, 0, 0, 0)]
    public void ValidTimestampBoundaries_ArePreserved(int year, int month, int day, int hour, int minute, int second)
    {
        byte[] archive = CreateArchive();
        PutDate(archive.AsSpan(4096 + 56), year, month, day, hour, minute, second);
        using var source = new MemoryStream(archive);
        var file = Files(new MtfFileSystem(source).RootDirectory).Single(f => f.Name == "hello.txt");

        Assert.Equal(new DateTime(year, month, day, hour, minute, second, DateTimeKind.Unspecified), file.LastWriteTime);
        Assert.Equal(DateTimeKind.Unspecified, file.LastWriteTime!.Value.Kind);
    }

    /// <summary>Verifies cross-record extraction and independent, seekable, read-only file views.</summary>
    /// <param name="recordSize">The generated physical tape-record payload size in bytes.</param>
    [Theory]
    [InlineData(512)]
    [InlineData(513)]
    [InlineData(17)]
    public void TapeContainer_ExtractsAcrossRecordsAndSupportsIndependentSeekableStreams(int recordSize)
    {
        byte[] mtf = CreateArchive();
        using var container = new TapContainer(new MemoryStream(WrapTape(mtf, recordSize)));
        Assert.False(container.SupportsWriting);
        Assert.False(container.Content.CanWrite);
        var partition = Assert.Single(container.PartitionTable.Partitions);
        Assert.False(partition.SupportsWriting);
        Assert.Same(container, partition.FileSystem.OwningContainer);
        Assert.Equal(mtf.LongLength, container.Length);
        Assert.True(container.ContainerFileStream.Length > container.Content.Length);
        var file = Files(partition.FileSystem.RootDirectory).Single(f => f.Name == "hello.txt");
        using var first = file.GetStream();
        using var second = file.GetStream();
        byte[] expected = Payload();
        Assert.Equal(expected[0], first.ReadByte());
        second.Position = 700;
        Assert.Equal(expected[700], second.ReadByte());
        Assert.Equal(expected[1], first.ReadByte());
        first.Seek(-30, SeekOrigin.End);
        Assert.Equal(expected[^30], first.ReadByte());
        first.Seek(0, SeekOrigin.Begin);
        using var extracted = new MemoryStream();
        first.CopyTo(extracted);
        Assert.Equal(expected, extracted.ToArray());
        Assert.Equal(-1, first.ReadByte());
        Assert.Throws<IOException>(() => first.Seek(-1, SeekOrigin.Begin));
        Assert.Throws<NotSupportedException>(() => first.WriteByte(0));
        second.Dispose();
        first.Position = 100;
        Assert.Equal(expected[100], first.ReadByte());
        var empty = Files(partition.FileSystem.RootDirectory).Single(f => f.Name == "empty.txt");
        using var emptyStream = empty.GetStream();
        Assert.Equal(0, emptyStream.Length);
        Assert.Equal(-1, emptyStream.ReadByte());
    }

    /// <summary>Verifies that identically named entries from different backup sets and volumes remain separate.</summary>
    [Fact]
    public void MultipleSetsAndVolumes_AreKeptSeparate()
    {
        using var stream = new MemoryStream(CreateArchive(sets: 2, volumes: 2));
        var fs = new MtfFileSystem(stream);
        var sets = fs.RootDirectory.EnumerateDirectories(true).ToArray();
        Assert.Equal(2, sets.Length);
        Assert.All(sets, set => Assert.Equal(2, set.EnumerateDirectories(true).Count()));
        Assert.Equal(12ul, fs.RootDirectory.CountFiles(true));
    }

    /// <summary>Verifies single-byte names and skipping metadata streams without changing file content.</summary>
    [Fact]
    public void AsciiNamesAndMetadataStreams_AreSupported()
    {
        using var stream = new MemoryStream(CreateArchive(ascii: true));
        var fs = new MtfFileSystem(stream);
        Assert.Equal("Test tape", fs.VolumeLabel);
        Assert.Equal(3ul, fs.RootDirectory.CountFiles(true));
        using var data = Files(fs.RootDirectory).Single(f => f.Name == "hello.txt").GetStream();
        Assert.Equal(SHA256.HashData(Payload()), SHA256.HashData(data));
    }

    /// <summary>Verifies that filesystem, directory, and file mutations are rejected.</summary>
    [Fact]
    public void Mutations_AreRejected()
    {
        using var stream = new MemoryStream(CreateArchive());
        var fs = new MtfFileSystem(stream);
        var file = Files(fs.RootDirectory).First();
        Assert.Throws<NotSupportedException>(() => fs.VolumeLabel = "changed");
        Assert.Throws<NotSupportedException>(() => fs.RootDirectory.CreateSubdirectory("new"));
        Assert.Throws<NotSupportedException>(() => fs.RootDirectory.Delete());
        Assert.Throws<NotSupportedException>(() => file.Name = "changed");
        Assert.Throws<NotSupportedException>(() => file.Length = 1);
        Assert.Throws<NotSupportedException>(() => file.Attributes = FileAttributes.Normal);
        Assert.Throws<NotSupportedException>(() => file.Delete());
        Assert.Throws<NotSupportedException>(() => file.MoveTo("elsewhere"));
    }

    /// <summary>Verifies that unsafe entry names cannot become extraction path components.</summary>
    /// <param name="name">The unsafe filename to place in the generated archive.</param>
    [Theory]
    [InlineData("../escape.txt")]
    [InlineData("C:\\escape.txt")]
    [InlineData("..")]
    [InlineData("NUL.txt")]
    [InlineData("trailing.")]
    [InlineData("stream:ads")]
    public void UnsafeFileNames_AreRejected(string name)
    {
        using var stream = new MemoryStream(CreateArchive(fileName: name));
        Assert.Throws<InvalidDataException>(() => new MtfFileSystem(stream));
    }

    /// <summary>Verifies rejection of parent-directory traversal within a recorded directory path.</summary>
    [Fact]
    public void UnsafeDirectoryPaths_AreRejected()
    {
        using var stream = new MemoryStream(CreateArchive(directoryPath: "docs\0..\0escape\0"));
        Assert.Throws<InvalidDataException>(() => new MtfFileSystem(stream));
    }

    /// <summary>Verifies that checksum failures propagate instead of causing a RAW fallback.</summary>
    /// <param name="offset">The logical byte offset of the header byte to corrupt.</param>
    /// <param name="change">The bit mask to XOR into the selected byte.</param>
    [Theory]
    [InlineData(50, 0x10)] // Bad descriptor checksum.
    [InlineData(4096 + 8, 0x10)] // Bad stream offset (header checksum invalid).
    [InlineData(4096 + 192, 0x10)] // Bad stream checksum.
    public void DamagedHeaders_AreNotSilentlyDetectedAsRaw(int offset, byte change)
    {
        byte[] archive = CreateArchive();
        archive[offset] ^= change;
        using var stream = new MemoryStream(archive);
        Assert.Throws<InvalidDataException>(() => FileSystem.AttemptDetection(stream));
    }

    /// <summary>Verifies rejection of continuation, variable-length, encrypted, and compressed stream flags.</summary>
    /// <param name="flags">The unsupported media-format stream flags to record.</param>
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(8)]
    [InlineData(16)]
    public void UnsupportedStreamFormats_AreRejected(ushort flags)
    {
        byte[] archive = CreateArchive();
        int header = 4096 + 192;
        Put16(archive, header + 6, flags);
        Checksum(archive.AsSpan(header, 22));
        using var stream = new MemoryStream(archive);
        Assert.Throws<NotSupportedException>(() => new MtfFileSystem(stream));
    }

    /// <summary>Verifies that container construction reports unsupported compression and releases its stream.</summary>
    [Fact]
    public void UnsupportedSetCompression_IsReportedByContainer()
    {
        byte[] archive = CreateArchive();
        Put16(archive, 1024 + 58, 0xabe);
        using var stream = new MemoryStream(WrapTape(archive));
        var exception = Assert.Throws<NotSupportedException>(() => new TapContainer(stream));
        Assert.Contains("compressed", exception.Message);
        Assert.False(stream.CanRead); // Failed constructors release owned resources.
    }

    /// <summary>Verifies rejection of a backup beginning on a later tape in a media family.</summary>
    [Fact]
    public void ContinuationTape_IsRejected()
    {
        byte[] archive = CreateArchive();
        Put16(archive, 60, 2);
        using var stream = new MemoryStream(archive);
        Assert.Throws<NotSupportedException>(() => new MtfFileSystem(stream));
    }

    /// <summary>Verifies that a data set without its closing descriptor is considered incomplete.</summary>
    [Fact]
    public void MissingEndOfSet_IsRejected()
    {
        byte[] archive = CreateArchive();
        using var stream = new MemoryStream(archive[..^1024]);
        Assert.Throws<InvalidDataException>(() => new MtfFileSystem(stream));
    }

    /// <summary>Verifies bounds checks for stream lengths and descriptor string offsets.</summary>
    [Fact]
    public void TruncatedDataAndInvalidStringOffsets_AreRejected()
    {
        byte[] archive = CreateArchive();
        Put64(archive, 4096 + 192 + 8, ulong.MaxValue);
        Checksum(archive.AsSpan(4096 + 192, 22));
        using var truncated = new MemoryStream(archive);
        Assert.Throws<InvalidDataException>(() => new MtfFileSystem(truncated));
        archive = CreateArchive();
        Put16(archive, 4096 + 86, 65535);
        using var badName = new MemoryStream(archive);
        Assert.Throws<InvalidDataException>(() => new MtfFileSystem(badName));
    }

    /// <summary>Verifies validation of directory references and consistency between declared and stored file sizes.</summary>
    [Fact]
    public void UnknownDirectoryAndMissingFileData_AreRejected()
    {
        byte[] archive = CreateArchive();
        Put32(archive, 4096 + 76, 999);
        using var missingDirectory = new MemoryStream(archive);
        Assert.Throws<InvalidDataException>(() => new MtfFileSystem(missingDirectory));
        archive = CreateArchive();
        Put64(archive, 4096 + 12, 999);
        Checksum(archive.AsSpan(4096, 52));
        using var missingData = new MemoryStream(archive);
        Assert.Throws<InvalidDataException>(() => new MtfFileSystem(missingData));
    }

    /// <summary>Verifies NT attribute precedence and metadata streams both before and after standard file data.</summary>
    [Fact]
    public void NtAttributesAndStreamsBeforeStandardData_AreSupported()
    {
        byte[] archive = CreateArchive();
        // Add an NT OS-specific area inside the FILE descriptor's metadata region.
        Put16(archive, 4096 + 44, 12);
        Put16(archive, 4096 + 46, 176);
        archive[4096 + 11] = 1;
        Put32(archive, 4096 + 176, (uint)(FileAttributes.Archive | FileAttributes.System));
        Checksum(archive.AsSpan(4096, 52));
        // Build a sample-shaped FILE object with NACL before STAN and another metadata stream after it.
        using var output = new MemoryStream();
        output.Write(archive.AsSpan(0, 4096));
        WriteObject(output, archive.AsSpan(4096, 192).ToArray(),
            [("NACL", new byte[44]), ("STAN", Payload()), ("NTEA", new byte[7])]);
        // The modified file still occupies four logical blocks.
        output.Write(archive.AsSpan(8192));
        using var source = new MemoryStream(output.ToArray());
        var fs = new MtfFileSystem(source);
        var file = Files(fs.RootDirectory).Single(f => f.Name == "hello.txt");
        Assert.Equal(FileAttributes.Archive | FileAttributes.System, file.Attributes);
        using var content = file.GetStream();
        Assert.Equal(SHA256.HashData(Payload()), SHA256.HashData(content));
    }

    /// <summary>Verifies preservation of UTF-16 filenames.</summary>
    /// <param name="name">The Unicode filename to record and recover.</param>
    [Theory]
    [InlineData("résumé.txt")]
    [InlineData("日本語.txt")]
    public void UnicodeFileNames_ArePreserved(string name)
    {
        using var stream = new MemoryStream(CreateArchive(fileName: name));
        Assert.Contains(Files(new MtfFileSystem(stream).RootDirectory), file => file.Name == name);
    }

    /// <summary>Verifies support for the second ESET descriptor terminating a media catalog.</summary>
    [Fact]
    public void CatalogTerminatorWithoutStreams_IsAccepted()
    {
        byte[] archive = CreateArchive();
        using var stream = new MemoryStream();
        stream.Write(archive);
        stream.Write(Descriptor("ESET", 1024, ascii: false));
        var fs = new MtfFileSystem(stream);
        Assert.Equal(3ul, fs.RootDirectory.CountFiles(true));
    }

    /// <summary>Verifies Windows 2000 stream checksums and padding before catalog-terminator streams.</summary>
    /// <param name="metadataLength">The catalog terminator's metadata length, excluding alignment padding.</param>
    [Theory]
    [InlineData(85)]
    [InlineData(86)]
    [InlineData(87)]
    [InlineData(88)]
    public void Windows2000Backups_AlignStreamsAndExtractAcrossRecords(int metadataLength)
    {
        byte[] archive = CreateArchive(sets: 2, catalogTerminatorLength: metadataLength, streamChecksums: true);
        using var container = new TapContainer(new MemoryStream(WrapTape(archive, recordSize: 513)));
        var fs = Assert.IsType<MtfFileSystem>(Assert.Single(container.PartitionTable.Partitions).FileSystem);
        Assert.Equal(2, fs.RootDirectory.EnumerateDirectories(true).Count());
        Assert.Equal(6ul, fs.RootDirectory.CountFiles(true));
        Assert.True(fs.IsReadOnly);
        foreach (var file in Files(fs.RootDirectory))
        {
            using var content = file.GetStream();
            using var extracted = new MemoryStream();
            content.CopyTo(extracted);
            byte[] expected = file.Name switch
            {
                "hello.txt" => Payload(),
                "other.txt" => "abc"u8.ToArray(),
                _ => Array.Empty<byte>()
            };
            Assert.Equal(expected, extracted.ToArray());
            Assert.Equal(expected.LongLength, content.Length);
            if (file.Name == "hello.txt")
            {
                Assert.Equal(new DateTime(2026, 11, 14, 12, 34, 56), file.LastWriteTime);
                Assert.True((file.Attributes & FileAttributes.Hidden) != 0);
            }
        }
    }

    /// <summary>Verifies that aligning Windows 2000 streams does not bypass header checksum validation.</summary>
    /// <param name="offset">The byte to corrupt relative to the final catalog terminator.</param>
    /// <param name="kind">The header type whose checksum must fail.</param>
    [Theory]
    [InlineData(50, "descriptor")]
    [InlineData(88, "stream")]
    [InlineData(108, "stream")]
    public void Windows2000DamagedHeaders_AreRejected(int offset, string kind)
    {
        byte[] archive = CreateArchive(catalogTerminatorLength: 85, streamChecksums: true);
        archive[archive.Length - 1024 + offset] ^= 0x10;
        using var source = new MemoryStream(archive);
        var exception = Assert.Throws<InvalidDataException>(() =>
        {
            FileSystem.AttemptDetection(source);
        });
        Assert.Equal($"Invalid MTF {kind} header checksum.", exception.Message);
    }

    /// <summary>Verifies bounds checks for padding and stream headers after unaligned descriptor metadata.</summary>
    /// <param name="remainingLength">The bytes retained from the final catalog terminator.</param>
    [Theory]
    [InlineData(85)]
    [InlineData(86)]
    [InlineData(87)]
    [InlineData(88)]
    [InlineData(109)]
    public void Windows2000TruncatedCatalogTerminator_IsRejected(int remainingLength)
    {
        byte[] archive = CreateArchive(catalogTerminatorLength: 85, streamChecksums: true);
        using var source = new MemoryStream(archive[..(archive.Length - 1024 + remainingLength)]);
        Assert.Throws<InvalidDataException>(() =>
        {
            new MtfFileSystem(source);
        });
    }

    /// <summary>Verifies that stream lengths remain bounded after aligning descriptor metadata.</summary>
    [Fact]
    public void Windows2000TruncatedStream_IsRejected()
    {
        byte[] archive = CreateArchive(catalogTerminatorLength: 85, streamChecksums: true);
        int header = archive.Length - 1024 + 88;
        Put64(archive, header + 8, ulong.MaxValue);
        Checksum(archive.AsSpan(header, 22));
        using var source = new MemoryStream(archive);
        var exception = Assert.Throws<InvalidDataException>(() =>
        {
            new MtfFileSystem(source);
        });
        Assert.Equal("Truncated MTF stream.", exception.Message);
    }

    /// <summary>Verifies that System State source drives become safe, relative directory components.</summary>
    /// <param name="drive">The source-drive component recorded in the archive.</param>
    /// <param name="directoryName">The safe directory name exposed for that drive.</param>
    [Theory]
    [InlineData("C:", "Drive C")]
    [InlineData("D:", "Drive D")]
    [InlineData("z:", "Drive Z")]
    public void SystemStateDrivePaths_AreExposedAsRelativeDirectories(string drive, string directoryName)
    {
        using var source = new MemoryStream(CreateArchive(directoryPath: $"Boot Files\0{drive}\0WINNT\0",
            catalogTerminatorLength: 85, streamChecksums: true));
        var fs = new MtfFileSystem(source);
        var files = Files(fs.RootDirectory).ToArray();
        Assert.Equal(3, files.Length);
        foreach (var file in files)
        {
            Assert.Equal($"\\Backup set 1\\Volume 1 (C)\\Boot Files\\{directoryName}\\WINNT\\{file.Name}", file.FullName);
            Assert.DoesNotContain(':', file.FullName);
        }
    }

    /// <summary>Verifies that drive-label normalization does not permit traversal, alternate streams, or arbitrary colons.</summary>
    /// <param name="path">The unsafe directory path recorded in the archive.</param>
    [Theory]
    [InlineData("Boot Files\0C:\0..\0escape\0")]
    [InlineData("Boot Files\0C:escape\0")]
    [InlineData("Boot Files\0CC:\0")]
    [InlineData("Boot Files\01:\0")]
    [InlineData("Boot Files\0C:\0stream:ads\0")]
    public void UnsafeSystemStatePaths_AreRejected(string path)
    {
        using var source = new MemoryStream(CreateArchive(directoryPath: path));
        Assert.Throws<InvalidDataException>(() =>
        {
            new MtfFileSystem(source);
        });
    }

    /// <summary>Verifies that a literal directory cannot silently merge with a normalized source-drive directory.</summary>
    [Fact]
    public void SystemStateDriveNameCollisions_AreRejected()
    {
        byte[] archive = CreateArchive(directoryPath: "Boot Files\0C:\0WINNT\0");
        using var output = new MemoryStream();
        output.Write(archive.AsSpan(0, archive.Length - 1024));
        byte[] directory = Descriptor("DIRB", 192, ascii: false);
        Put32(directory, 76, 2);
        PutString(directory, 80, 96, "Boot Files\0Drive C\0WINNT\0", ascii: false);
        WriteObject(output, directory, []);
        output.Write(archive.AsSpan(archive.Length - 1024));
        using var source = new MemoryStream(output.ToArray());
        var exception = Assert.Throws<InvalidDataException>(() =>
        {
            new MtfFileSystem(source);
        });
        Assert.Equal("Conflicting MTF source directory names.", exception.Message);
    }

    /// <summary>Verifies that repeated System State files retain independent payloads and collision-free extraction names.</summary>
    [Fact]
    public void RepeatedFiles_ArePreservedWithUniqueNames()
    {
        byte[] archive = CreateArchive(directoryPath: "Boot Files\0C:\0WINNT\0");
        using var output = new MemoryStream();
        output.Write(archive.AsSpan(0, archive.Length - 1024));

        void WriteFile(string name, byte[] data)
        {
            byte[] descriptor = archive.AsSpan(4096, 192).ToArray();
            PutString(descriptor, 84, 96, name, ascii: false);
            Put64(descriptor, 12, (ulong)data.Length);
            Checksum(descriptor.AsSpan(0, 52));
            WriteObject(output, descriptor, [("STAN", data)], streamChecksums: true);
        }

        WriteFile("hello (2).txt", "reserved name"u8.ToArray());
        WriteFile("HELLO.TXT", "second copy"u8.ToArray());
        WriteFile("hello.txt", "third copy"u8.ToArray());
        output.Write(archive.AsSpan(archive.Length - 1024));
        using var source = new MemoryStream(output.ToArray());
        var fs = new MtfFileSystem(source);
        var files = Files(fs.RootDirectory).ToDictionary(file =>
        {
            return file.Name;
        });
        Assert.Equal(6, files.Count);
        var expected = new Dictionary<string, byte[]>
        {
            ["hello.txt"] = Payload(),
            ["hello (2).txt"] = "reserved name"u8.ToArray(),
            ["HELLO (3).TXT"] = "second copy"u8.ToArray(),
            ["hello (4).txt"] = "third copy"u8.ToArray()
        };
        foreach (var pair in expected)
        {
            using var content = files[pair.Key].GetStream();
            using var extracted = new MemoryStream();
            content.CopyTo(extracted);
            Assert.Equal(pair.Value, extracted.ToArray());
        }
    }

    /// <summary>Verifies explicit rejection of sparse, link, reparse, and streamed-name formats.</summary>
    /// <param name="type">The unsupported four-character stream type.</param>
    [Theory]
    [InlineData("SPAR")]
    [InlineData("LINK")]
    [InlineData("NTRP")]
    [InlineData("FNAM")]
    [InlineData("PNAM")]
    public void UnsupportedObjectStreams_AreRejected(string type)
    {
        byte[] archive = CreateArchive();
        Encoding.ASCII.GetBytes(type).CopyTo(archive, 4096 + 192);
        Checksum(archive.AsSpan(4096 + 192, 22));
        using var stream = new MemoryStream(archive);
        Assert.Throws<NotSupportedException>(() => new MtfFileSystem(stream));
    }

    /// <summary>Verifies that unsupported NT link metadata is rejected before file data is exposed.</summary>
    /// <param name="attributes">The NT filesystem attributes to record.</param>
    /// <param name="flags">The NT file-specific flags to record.</param>
    [Theory]
    [InlineData(0x400u, 0u)] // NT reparse-point attribute.
    [InlineData(0u, 1u)] // NT hard-link flag (FILE, OS version 1).
    public void NtLinkFlags_AreRejectedBeforeOpeningFileData(uint attributes, uint flags)
    {
        byte[] archive = CreateArchive();
        Put16(archive, 4096 + 44, 12);
        Put16(archive, 4096 + 46, 176);
        archive[4096 + 11] = 1;
        Put32(archive, 4096 + 176, attributes);
        Put32(archive, 4096 + 184, flags);
        Checksum(archive.AsSpan(4096, 52));
        using var stream = new MemoryStream(archive);
        Assert.Throws<NotSupportedException>(() => new MtfFileSystem(stream));
    }

    /// <summary>Enumerates all files below a directory, including hidden entries.</summary>
    /// <param name="directory">The directory at which to begin recursive enumeration.</param>
    /// <returns>The files in this directory and its descendants.</returns>
    private static IEnumerable<TiFile> Files(TiDirectory directory)
    {
        return directory.EnumerateFiles(true).Concat(directory.EnumerateDirectories(true).SelectMany(Files));
    }

    /// <summary>Wraps a logical payload in generated SIMH records, tape marks, and an end-of-medium marker.</summary>
    /// <param name="data">The logical tape payload.</param>
    /// <param name="recordSize">The positive maximum payload size of each physical record in bytes.</param>
    /// <returns>The framed tape image, including padding after odd-sized records.</returns>
    internal static byte[] WrapTape(byte[] data, int recordSize = 512)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        writer.Write(0u);
        for (int offset = 0; offset < data.Length; offset += recordSize)
        {
            int count = Math.Min(recordSize, data.Length - offset);
            writer.Write(count);
            writer.Write(data, offset, count);
            if (count % 2 != 0)
            {
                writer.Write((byte)0);
            }
            writer.Write(count);
            if (offset == 0)
            {
                writer.Write(0u);
            }
        }
        writer.Write(0u);
        writer.Write(uint.MaxValue);
        return stream.ToArray();
    }

    /// <summary>Creates deterministic file data containing an embedded descriptor-looking tag.</summary>
    /// <returns>The standard-data payload shared by extraction fixtures.</returns>
    private static byte[] Payload()
    {
        byte[] result = Enumerable.Range(0, 2801).Select(i => (byte)(i % 251)).ToArray();
        // Embedded descriptor-looking data must never be treated as metadata.
        "FILE"u8.CopyTo(result.AsSpan(810));
        return result;
    }

    /// <summary>Builds a complete in-memory MTF archive without external sample files.</summary>
    /// <param name="sets">The number of independently terminated data sets.</param>
    /// <param name="volumes">The number of source volumes within each data set.</param>
    /// <param name="ascii">Whether to use single-byte strings rather than UTF-16LE.</param>
    /// <param name="fileName">The primary file's name, including deliberately unsafe names for validation tests.</param>
    /// <param name="directoryPath">The null-separated directory path recorded in each volume.</param>
    /// <param name="catalogTerminatorLength">The optional second ESET's metadata length, excluding alignment padding.</param>
    /// <param name="streamChecksums">Whether to emit Windows 2000-style checksum flags and opaque CSUM streams.</param>
    /// <returns>A logical archive with metadata, standard data, an empty file, and catalog streams.</returns>
    private static byte[] CreateArchive(int sets = 1, int volumes = 1, bool ascii = false,
        string fileName = "hello.txt", string directoryPath = "docs\0nested\0",
        int catalogTerminatorLength = 0, bool streamChecksums = false)
    {
        using var result = new MemoryStream();
        byte[] tape = Descriptor("TAPE", 1024, ascii);
        Put16(tape, 60, 1);
        Put16(tape, 84, 1024);
        tape[93] = 1;
        PutString(tape, 68, 128, "Test tape", ascii);
        result.Write(tape);
        for (int s = 0; s < sets; s++)
        {
            var set = Descriptor("SSET", 1024, ascii);
            Put16(set, 62, (ushort)(s + 1));
            result.Write(set);
            for (int v = 0; v < volumes; v++)
            {
                var volume = Descriptor("VOLB", 1024, ascii);
                PutString(volume, 56, 128, "C:", ascii);
                result.Write(volume);
                var directory = Descriptor("DIRB", 192, ascii);
                Put32(directory, 76, 1);
                PutString(directory, 80, 96, directoryPath, ascii);
                WriteObject(result, directory, []);
                var file = Descriptor("FILE", 192, ascii);
                Put32(file, 76, 1);
                Put32(file, 80, 1);
                Put32(file, 52, 0x200);
                PutString(file, 84, 96, fileName, ascii);
                PutDate(file.AsSpan(56), new DateTime(2026, 11, 14, 12, 34, 56));
                PutDate(file.AsSpan(61), new DateTime(2020, 1, 2, 3, 4, 5));
                PutDate(file.AsSpan(66), new DateTime(2026, 11, 16, 0, 0, 0));
                PutDate(file.AsSpan(71), new DateTime(2026, 11, 15, 1, 2, 3));
                Put64(file, 12, (ulong)Payload().Length);
                Checksum(file.AsSpan(0, 52));
                if (streamChecksums)
                {
                    WriteObject(result, file, [("NACL", new byte[44]), ("STAN", Payload())], streamChecksums: true);
                }
                else
                {
                    WriteObject(result, file, [("STAN", Payload()), ("NACL", new byte[44])]);
                }
                var empty = Descriptor("FILE", 192, ascii);
                Put32(empty, 76, 1);
                PutString(empty, 84, 96, "empty.txt", ascii);
                WriteObject(result, empty, []);
                var other = Descriptor("FILE", 192, ascii);
                Put32(other, 76, 1);
                PutString(other, 84, 96, "other.txt", ascii);
                Put64(other, 12, 3);
                Checksum(other.AsSpan(0, 52));
                WriteObject(result, other, [("STAN", "abc"u8.ToArray())]);
            }
            // End-of-set catalogs are streams, not directory/file descriptors.
            var end = Descriptor("ESET", 192, ascii);
            WriteObject(result, end, [("TFDD", "FILE SSET DIRB"u8.ToArray()), ("TSMP", new byte[30])]);
            if (catalogTerminatorLength != 0)
            {
                WriteObject(result, Descriptor("ESET", catalogTerminatorLength, ascii), []);
            }
        }
        return result.ToArray();
    }

    /// <summary>Creates a zero-initialized descriptor with a valid common-header checksum.</summary>
    /// <param name="type">The four-character descriptor type.</param>
    /// <param name="streamOffset">The descriptor metadata length and offset of its first stream.</param>
    /// <param name="ascii">Whether the descriptor identifies single-byte rather than UTF-16LE strings.</param>
    /// <returns>The descriptor buffer, ready for type-specific fields.</returns>
    private static byte[] Descriptor(string type, int streamOffset, bool ascii)
    {
        byte[] data = new byte[streamOffset];
        Encoding.ASCII.GetBytes(type).CopyTo(data, 0);
        Put16(data, 8, (ushort)streamOffset);
        data[10] = 14;
        data[48] = ascii ? (byte)1 : (byte)2;
        Checksum(data.AsSpan(0, 52));
        return data;
    }

    /// <summary>Writes an object's descriptor and streams, ending with padding to a 1024-byte logical boundary.</summary>
    /// <param name="output">The destination logical archive stream.</param>
    /// <param name="descriptor">The complete descriptor metadata buffer.</param>
    /// <param name="streams">The stream types and payloads to append in order.</param>
    /// <param name="streamChecksums">Whether each stream has the checksum-present flag and a following opaque CSUM stream.</param>
    private static void WriteObject(MemoryStream output, byte[] descriptor, (string Type, byte[] Data)[] streams,
        bool streamChecksums = false)
    {
        output.Write(descriptor);
        // The metadata length need not include padding to the first stream's four-byte boundary.
        while (output.Position % 4 != 0)
        {
            output.WriteByte(0);
        }
        foreach (var (type, data) in streams)
        {
            WriteStreamHeader(output, type, (ulong)data.Length, mediaFlags: streamChecksums ? (ushort)0x20 : (ushort)0);
            output.Write(data);
            while (output.Position % 4 != 0)
            {
                output.WriteByte(0);
            }
            if (streamChecksums)
            {
                // CSUM payloads are skipped, not interpreted as headers or appended to standard file data.
                WriteStreamHeader(output, "CSUM", 4);
                output.Write("FILE"u8);
                output.Write(new byte[2]);
            }
        }
        // SPAD can consume another logical block if fewer than 22 bytes remain.
        int padding = (int)((1024 - (output.Position + 22) % 1024) % 1024);
        WriteStreamHeader(output, "SPAD", (ulong)padding);
        output.Write(new byte[padding]);
    }

    /// <summary>Writes a 22-byte stream header with a valid word-XOR checksum.</summary>
    /// <param name="output">The destination logical archive stream.</param>
    /// <param name="type">The four-character stream type.</param>
    /// <param name="length">The payload length in bytes, excluding the header and alignment padding.</param>
    /// <param name="mediaFlags">The media-format stream flags, including the optional checksum-present bit.</param>
    private static void WriteStreamHeader(Stream output, string type, ulong length, ushort mediaFlags = 0)
    {
        byte[] header = new byte[22];
        Encoding.ASCII.GetBytes(type).CopyTo(header, 0);
        Put16(header, 6, mediaFlags);
        Put64(header, 8, length);
        Checksum(header);
        output.Write(header);
    }

    /// <summary>Encodes string data and writes its MTF size/offset field.</summary>
    /// <param name="data">The descriptor buffer to update.</param>
    /// <param name="field">The byte offset of the four-byte string reference.</param>
    /// <param name="offset">The byte offset of the string storage within the descriptor.</param>
    /// <param name="text">The string to encode, including any embedded path separators.</param>
    /// <param name="ascii">Whether to use single-byte rather than UTF-16LE encoding.</param>
    private static void PutString(byte[] data, int field, int offset, string text, bool ascii)
    {
        byte[] encoded = (ascii ? Encoding.Latin1 : Encoding.Unicode).GetBytes(text);
        Put16(data, field, (ushort)encoded.Length);
        Put16(data, field + 2, (ushort)offset);
        encoded.CopyTo(data, offset);
    }

    /// <summary>Encodes a civil timestamp in the five-byte packed MTF representation.</summary>
    /// <param name="b">The destination region, which must contain at least five bytes.</param>
    /// <param name="date">The timestamp whose date and time components are encoded.</param>
    private static void PutDate(Span<byte> b, DateTime date)
    {
        PutDate(b, date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
    }

    /// <summary>Encodes raw timestamp components, allowing invalid values within the MTF fields' bit widths.</summary>
    /// <param name="b">The destination region, which must contain at least five bytes.</param>
    /// <param name="year">The 14-bit year value.</param>
    /// <param name="month">The four-bit month value.</param>
    /// <param name="day">The five-bit day value.</param>
    /// <param name="hour">The five-bit hour value.</param>
    /// <param name="minute">The six-bit minute value.</param>
    /// <param name="second">The six-bit second value.</param>
    private static void PutDate(Span<byte> b, int year, int month, int day, int hour, int minute, int second)
    {
        b[0] = (byte)(year >> 6);
        b[1] = (byte)((year << 2) | (month >> 2));
        b[2] = (byte)((month << 6) | (day << 1) | (hour >> 4));
        b[3] = (byte)((hour << 4) | (minute >> 2));
        b[4] = (byte)((minute << 6) | second);
    }

    /// <summary>Sets the final word of an even-sized header to its word-wise XOR checksum.</summary>
    /// <param name="bytes">The complete header, including the checksum word to overwrite.</param>
    private static void Checksum(Span<byte> bytes)
    {
        ushort checksum = 0;
        for (int i = 0; i < bytes.Length - 2; i += 2)
        {
            checksum ^= BinaryPrimitives.ReadUInt16LittleEndian(bytes[i..]);
        }
        BinaryPrimitives.WriteUInt16LittleEndian(bytes[^2..], checksum);
    }

    /// <summary>Writes a little-endian 16-bit fixture field.</summary>
    /// <param name="b">The destination buffer.</param>
    /// <param name="offset">The byte offset of the field.</param>
    /// <param name="value">The value to encode.</param>
    private static void Put16(byte[] b, int offset, ushort value)
    {
        BinaryPrimitives.WriteUInt16LittleEndian(b.AsSpan(offset), value);
    }

    /// <summary>Writes a little-endian 32-bit fixture field.</summary>
    /// <param name="b">The destination buffer.</param>
    /// <param name="offset">The byte offset of the field.</param>
    /// <param name="value">The value to encode.</param>
    private static void Put32(byte[] b, int offset, uint value)
    {
        BinaryPrimitives.WriteUInt32LittleEndian(b.AsSpan(offset), value);
    }

    /// <summary>Writes a little-endian 64-bit fixture field.</summary>
    /// <param name="b">The destination buffer.</param>
    /// <param name="offset">The byte offset of the field.</param>
    /// <param name="value">The value to encode.</param>
    private static void Put64(byte[] b, int offset, ulong value)
    {
        BinaryPrimitives.WriteUInt64LittleEndian(b.AsSpan(offset), value);
    }
}
