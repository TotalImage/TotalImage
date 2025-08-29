using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text;

namespace TotalImage.FileSystems.ExFAT;

/// <summary>
/// Represents a directory in an exFAT file system.
/// </summary>
public class ExFatDirectory : Directory
{
    /// <summary>
    /// Gets the file directory entry backing this directory, when available.
    /// </summary>
    protected FileDirectoryEntry? FileDirectoryEntry { get; }
    /// <summary>
    /// Gets the stream extension entry backing this directory, when available.
    /// </summary>
    protected StreamExtensionDirectoryEntry? StreamExtensionDirectoryEntry { get; }
    /// <summary>
    /// Gets the file name entries that compose the directory name, when available.
    /// </summary>
    protected ImmutableArray<FileNameDirectoryEntry>? FileNameDirectoryEntries { get; }

    /// <summary>
    /// Creates the root exFAT directory.
    /// </summary>
    /// <param name="fileSystem">The file system that owns the directory.</param>
    public ExFatDirectory(ExFatFileSystem fileSystem) : base(fileSystem, null)
    {

    }

    /// <summary>
    /// Creates an exFAT directory from parsed directory entry components.
    /// </summary>
    /// <param name="directory">The parent directory.</param>
    /// <param name="fileEntry">The file directory entry.</param>
    /// <param name="streamExtensionEntry">The stream extension entry.</param>
    /// <param name="fileNameEntries">The file name entries.</param>
    public ExFatDirectory(Directory directory, FileDirectoryEntry fileEntry, StreamExtensionDirectoryEntry streamExtensionEntry, IEnumerable<FileNameDirectoryEntry> fileNameEntries) : base(directory.FileSystem, directory)
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
            if (FileNameDirectoryEntries is not null)
            {
                var sb = new StringBuilder(FileNameDirectoryEntries.Value.Length * 15);

                foreach (var entry in FileNameDirectoryEntries)
                {
                    sb.Append(entry.FileName);
                }

                return sb.ToString().TrimEnd('\0');
            }

            return string.Empty;
        }
        set => throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override FileAttributes Attributes
    {
        get => (FileAttributes?)FileDirectoryEntry?.FileAttributes ?? FileAttributes.Directory;
        set => throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override DateTime? LastAccessTime
    {
        get => FileDirectoryEntry?.LastAccessedTime ?? null;
        set => throw new NotImplementedException();
    }
    /// <inheritdoc />
    public override DateTime? LastWriteTime
    {
        get => FileDirectoryEntry?.LastModifiedTime ?? null;
        set => throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override DateTime? CreationTime
    {
        get => FileDirectoryEntry?.CreateTime;
        set => throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override ulong Length
    {
        get
        {
            if (StreamExtensionDirectoryEntry is not null)
            {
                return StreamExtensionDirectoryEntry.ValidDataLength;
            }
            else
            {
                var exFat = (ExFatFileSystem)FileSystem;
                var clusters = exFat.ActiveFat.GetClusterChain(exFat.BootSector.FirstClusterOfRootDirectory);
                return (ulong)(clusters.Length * exFat.BytesPerCluster);
            }
        }
        set => throw new NotImplementedException();
    }


    /// <inheritdoc />
    public override Directory CreateSubdirectory(string path)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override void Erase()
    {
        throw new NotImplementedException();
    }

	/// <inheritdoc />
    public override void Delete()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override IEnumerable<FileSystemObject> EnumerateFileSystemObjects(bool showHidden, bool showDeleted = false)
    {
        var fileSystem = (ExFatFileSystem)FileSystem;
        var stream = StreamExtensionDirectoryEntry?.GetStream(fileSystem) ??
            new ExFatDataStream(fileSystem, fileSystem.BootSector.FirstClusterOfRootDirectory);

        FileDirectoryEntry? fileDirEntry = null;
        StreamExtensionDirectoryEntry? streamExtDirEntry = null;
        List<FileNameDirectoryEntry> fileNameDirEntries = new();

        foreach(var entry in DirectoryEntry.EnumerateDirectory(stream))
        {
            if (entry is FileDirectoryEntry)
            {
                if (fileDirEntry != null && streamExtDirEntry != null && fileNameDirEntries.Count != 0)
                {
                    if (!showHidden && ((FileAttributes)fileDirEntry.FileAttributes).HasFlag(FileAttributes.Hidden))
                    {
                        // Skip hidden entries
                    }
                    else if (((FileAttributes)fileDirEntry.FileAttributes).HasFlag(FileAttributes.Directory))
                    {
                        yield return new ExFatDirectory(this, fileDirEntry, streamExtDirEntry, fileNameDirEntries);
                    }
                    else
                    {
                        yield return new ExFatFile(this, fileDirEntry, streamExtDirEntry, fileNameDirEntries);
                    }
                }

                fileDirEntry = (FileDirectoryEntry)entry;
                streamExtDirEntry = null;
                fileNameDirEntries.Clear();
            }
            else if (entry is StreamExtensionDirectoryEntry)
            {
                streamExtDirEntry = (StreamExtensionDirectoryEntry)entry;
            }
            else if (entry is FileNameDirectoryEntry)
            {
                fileNameDirEntries.Add((FileNameDirectoryEntry)entry);
            }
        }

        if (fileDirEntry != null && streamExtDirEntry != null && fileNameDirEntries.Count != 0)
        {
            if (!showHidden && ((FileAttributes)fileDirEntry.FileAttributes).HasFlag(FileAttributes.Hidden))
            {
                // Skip hidden entries
            }
            else if (((FileAttributes)fileDirEntry.FileAttributes).HasFlag(FileAttributes.Directory))
            {
                yield return new ExFatDirectory(this, fileDirEntry, streamExtDirEntry, fileNameDirEntries);
            }
            else
            {
                yield return new ExFatFile(this, fileDirEntry, streamExtDirEntry, fileNameDirEntries);
            }
        }
    }

    /// <inheritdoc />
    public override void MoveTo(string path)
    {
        throw new NotImplementedException();
    }
}
