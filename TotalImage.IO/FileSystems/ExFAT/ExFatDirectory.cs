using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text;

namespace TotalImage.FileSystems.ExFAT;

public class ExFatDirectory : Directory
{
    public ExFatDirectory(ExFatFileSystem fileSystem) : base(fileSystem, null)
    {

    }

    public override string Name
    {
        get
        {
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
        get => null;
        set => throw new NotImplementedException();
    }
    public override DateTime? LastWriteTime
    {
        get => null;
        set => throw new NotImplementedException();
    }

    public override DateTime? CreationTime
    {
        get => null;
        set => throw new NotImplementedException();
    }

    public override ulong Length
    {
        get
        {
            var exFat = (ExFatFileSystem)FileSystem;
            var clusters = exFat.ActiveFat.GetClusterChain(exFat.BootSector.FirstClusterOfRootDirectory);
            return (ulong)(clusters.Length * exFat.BytesPerCluster);
        }
        set => throw new NotImplementedException();
    }


    public override Directory CreateSubdirectory(string path)
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
        var stream = new ExFatDataStream(fileSystem, fileSystem.BootSector.FirstClusterOfRootDirectory);

        var entry = new byte[32];

        ExFatFileDirectoryEntry fileDirEntry = null;
        ExFatStreamExtensionDirectoryEntry streamExtDirEntry = null;
        List<ExFatFileNameDirectoryEntry> fileNameDirEntries = new();

        do
        {
            stream.Read(entry);

            var position = stream.Position;

            if (entry[0] == 0x85) // File
            {
                if (fileDirEntry != null && streamExtDirEntry != null && fileNameDirEntries.Count != 0)
                {
                    yield return new ExFatFile(this, fileDirEntry, streamExtDirEntry, fileNameDirEntries);
                }

                fileDirEntry = new(entry);
                streamExtDirEntry = null;
                fileNameDirEntries.Clear();
            }
            else if (entry[0] == 0xC0) // Stream Extension
            {
                streamExtDirEntry = new(entry);
            }
            else if (entry[0] == 0xC1) // File Name
            {
                fileNameDirEntries.Add(new(entry));
            }

            stream.Position = position;
        }
        while (entry[0] != 0x00);

        if (fileDirEntry != null && streamExtDirEntry != null && fileNameDirEntries.Count != 0)
        {
            yield return new ExFatFile(this, fileDirEntry, streamExtDirEntry, fileNameDirEntries);
        }
    }

    public override void MoveTo(string path)
    {
        throw new NotImplementedException();
    }
}
