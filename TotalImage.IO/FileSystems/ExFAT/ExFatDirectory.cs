using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text;
using TotalImage.FileSystems.FAT;

namespace TotalImage.FileSystems.ExFAT;

public class ExFatDirectory : Directory
{
    protected FileDirectoryEntry? FileDirectoryEntry { get; }
    protected StreamExtensionDirectoryEntry? StreamExtensionDirectoryEntry { get; }
    protected ImmutableArray<FileNameDirectoryEntry>? FileNameDirectoryEntries { get; }

    public ExFatDirectory(ExFatFileSystem fileSystem) : base(fileSystem, null)
    {

    }

    public ExFatDirectory(Directory directory, FileDirectoryEntry fileEntry, StreamExtensionDirectoryEntry streamExtensionEntry, IEnumerable<FileNameDirectoryEntry> fileNameEntries) : base(directory.FileSystem, directory)
    {
        FileDirectoryEntry = fileEntry;
        StreamExtensionDirectoryEntry = streamExtensionEntry;
        FileNameDirectoryEntries = fileNameEntries.ToImmutableArray();
    }

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

    public override FileAttributes Attributes
    {
        get => (FileAttributes?)FileDirectoryEntry?.FileAttributes ?? FileAttributes.Directory;
        set => throw new NotImplementedException();
    }

    public override DateTime? LastAccessTime
    {
        get => FileDirectoryEntry?.LastAccessedTime ?? null;
        set => throw new NotImplementedException();
    }
    public override DateTime? LastWriteTime
    {
        get => FileDirectoryEntry?.LastModifiedTime ?? null;
        set => throw new NotImplementedException();
    }

    public override DateTime? CreationTime
    {
        get => FileDirectoryEntry?.CreateTime;
        set => throw new NotImplementedException();
    }

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


    public override Directory CreateSubdirectory(string path)
    {
        throw new NotImplementedException();
    }

    public override void Erase()
    {
        throw new NotImplementedException();
    }

    public override void Delete()
    {
        throw new NotImplementedException();
    }

    public override IEnumerable<FileSystemObject> EnumerateFileSystemObjects(bool showHidden, bool showDeleted)
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
                    if (((FileAttributes)fileDirEntry.FileAttributes).HasFlag(FileAttributes.Directory))
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
            if (((FileAttributes)fileDirEntry.FileAttributes).HasFlag(FileAttributes.Directory))
            {
                yield return new ExFatDirectory(this, fileDirEntry, streamExtDirEntry, fileNameDirEntries);
            }
            else
            {
                yield return new ExFatFile(this, fileDirEntry, streamExtDirEntry, fileNameDirEntries);
            }
        }
    }

    public override void MoveTo(string path)
    {
        throw new NotImplementedException();
    }
}
