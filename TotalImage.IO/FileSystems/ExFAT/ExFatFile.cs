using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text;
using TotalImage.FileSystems.FAT;

namespace TotalImage.FileSystems.ExFAT;

public class ExFatFile : File
{
    protected ExFatFileDirectoryEntry FileDirectoryEntry { get; }
    protected ExFatStreamExtensionDirectoryEntry StreamExtensionDirectoryEntry { get; }
    protected ImmutableArray<ExFatFileNameDirectoryEntry> FileNameDirectoryEntries { get; }

    public ExFatFile(Directory directory, ExFatFileDirectoryEntry fileEntry, ExFatStreamExtensionDirectoryEntry streamExtensionEntry, IEnumerable<ExFatFileNameDirectoryEntry> fileNameEntries) : base(directory.FileSystem, directory)
    {
        FileDirectoryEntry = fileEntry;
        StreamExtensionDirectoryEntry = streamExtensionEntry;
        FileNameDirectoryEntries = fileNameEntries.ToImmutableArray();
    }

    public override string Name
    {
        get
        {
            var sb = new StringBuilder(FileNameDirectoryEntries.Length * 15);

            foreach (var entry in FileNameDirectoryEntries)
            {
                sb.Append(entry.FileName);
            }

            return sb.ToString().TrimEnd('\0');
        }
        set => throw new NotImplementedException();
    }

    public override FileAttributes Attributes
    {
        get => (FileAttributes)FileDirectoryEntry.FileAttributes;
        set => throw new NotImplementedException();
    }

    public override DateTime? LastAccessTime
    {
        get => FileDirectoryEntry.LastAccessedTime;
        set => throw new NotImplementedException();
    }

    public override DateTime? LastWriteTime
    {
        get => FileDirectoryEntry.LastModifiedTime;
        set => throw new NotImplementedException();
    }

    public override DateTime? CreationTime
    {
        get => FileDirectoryEntry.CreateTime;
        set => throw new NotImplementedException();
    }

    public override ulong Length
    {
        get => StreamExtensionDirectoryEntry.ValidDataLength;
        set => throw new NotImplementedException();
    }

    public override void Delete()
    {
        throw new NotImplementedException();
    }

    public override Stream GetStream() =>
        StreamExtensionDirectoryEntry.GetStream((ExFatFileSystem)FileSystem);

    public override void MoveTo(string path)
    {
        throw new NotImplementedException();
    }
}
