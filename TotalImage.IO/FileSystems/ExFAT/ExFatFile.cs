using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text;

namespace TotalImage.FileSystems.ExFAT;

/// <summary>
/// Represents a file in an exFAT file system.
/// </summary>
public class ExFatFile : File
{
    /// <summary>
    /// Gets the file directory entry backing this file.
    /// </summary>
    protected FileDirectoryEntry FileDirectoryEntry { get; }
    /// <summary>
    /// Gets the stream extension entry backing this file.
    /// </summary>
    protected StreamExtensionDirectoryEntry StreamExtensionDirectoryEntry { get; }
    /// <summary>
    /// Gets the file name entries that compose the file name.
    /// </summary>
    protected ImmutableArray<FileNameDirectoryEntry> FileNameDirectoryEntries { get; }

    /// <summary>
    /// Creates an exFAT file from parsed directory entry components.
    /// </summary>
    /// <param name="directory">The parent directory.</param>
    /// <param name="fileEntry">The file directory entry.</param>
    /// <param name="streamExtensionEntry">The stream extension entry.</param>
    /// <param name="fileNameEntries">The file name entries.</param>
    public ExFatFile(Directory directory, FileDirectoryEntry fileEntry, StreamExtensionDirectoryEntry streamExtensionEntry, IEnumerable<FileNameDirectoryEntry> fileNameEntries) : base(directory.FileSystem, directory)
    {
        FileDirectoryEntry = fileEntry;
        StreamExtensionDirectoryEntry = streamExtensionEntry;
        FileNameDirectoryEntries = fileNameEntries.ToImmutableArray();
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public override FileAttributes Attributes
    {
        get => (FileAttributes)FileDirectoryEntry.FileAttributes;
        set => throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override DateTime? LastAccessTime
    {
        get => FileDirectoryEntry.LastAccessedTime;
        set => throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override DateTime? LastWriteTime
    {
        get => FileDirectoryEntry.LastModifiedTime;
        set => throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override DateTime? CreationTime
    {
        get => FileDirectoryEntry.CreateTime;
        set => throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override ulong Length
    {
        get => StreamExtensionDirectoryEntry.ValidDataLength;
        set => throw new NotImplementedException();
    }
    public override void Erase()
    {
        Delete();

        //Here we zero out every cluster of the file
    }

    /// <inheritdoc />
    public override void Delete()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override Stream GetStream() =>
        StreamExtensionDirectoryEntry.GetStream((ExFatFileSystem)FileSystem);

    /// <inheritdoc />
    public override void MoveTo(string path)
    {
        throw new NotImplementedException();
    }
}
