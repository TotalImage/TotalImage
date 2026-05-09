using System;
using System.IO;
using System.Linq;
using System.Text;
using TotalImage.Changes;
using TotalImage.Containers;
using TotalImage.FileSystems;
using TotalImage.FileSystems.FAT;
using TotalImage.Partitions;

namespace TotalImage.Changes
{
    /// <summary>
    /// Applies <see cref="PendingChange"/> records to a writable container during commit.
    /// All methods in this class operate on the <em>destination</em> container (a temporary copy),
    /// never on the original read-only source.
    /// </summary>
    internal static class ChangeApplicator
    {
        /// <summary>
        /// Dispatches a single pending change to the appropriate apply method.
        /// </summary>
        internal static void Apply(Container container, PendingChange change)
        {
            switch (change)
            {
                case AddFileChange add:
                    ApplyAddFile(container, add);
                    break;
                case DeleteEntryChange del:
                    ApplyDelete(container, del);
                    break;
                case CreateDirectoryChange mkdir:
                    ApplyCreateDirectory(container, mkdir);
                    break;
                case RenameChange ren:
                    ApplyRename(container, ren);
                    break;
                case VolumeLabelChange vl:
                    ApplyVolumeLabel(container, vl);
                    break;
                default:
                    throw new NotSupportedException($"Unknown change type: {change.GetType().Name}");
            }
        }

        // -----------------------------------------------------------------------
        // Internal helpers
        // -----------------------------------------------------------------------

        /// <summary>
        /// Navigates to the FAT directory identified by the directory components of a path array.
        /// Returns the parent directory and the final name component.
        /// </summary>
        private static (FatDirectory directory, string name) ResolvePath(FatFileSystem fat, string[] pathComponents)
        {
            if (pathComponents is null || pathComponents.Length == 0)
                throw new ArgumentException("Path must have at least one component.", nameof(pathComponents));

            var directory = (FatDirectory)fat.RootDirectory;

            // Navigate down to the parent directory
            for (int i = 0; i < pathComponents.Length - 1; i++)
            {
                string segment = pathComponents[i];
                var sub = directory.EnumerateFileSystemObjects(showHidden: true)
                    .OfType<FatDirectory>()
                    .FirstOrDefault(d => string.Equals(d.Name, segment, StringComparison.OrdinalIgnoreCase))
                    ?? throw new DirectoryNotFoundException($"Directory '{segment}' not found.");
                directory = sub;
            }

            return (directory, pathComponents[^1]);
        }

        /// <summary>
        /// Returns the FAT file system for the given partition index within a container.
        /// </summary>
        private static FatFileSystem GetFatFileSystem(Container container, int partitionIndex = 0)
        {
            var partitions = container.PartitionTable.Partitions;
            if (partitionIndex < 0 || partitionIndex >= partitions.Count)
                throw new ArgumentOutOfRangeException(nameof(partitionIndex));

            if (partitions[partitionIndex].FileSystem is not FatFileSystem fat)
                throw new InvalidOperationException($"Partition {partitionIndex} does not contain a FAT file system.");

            return fat;
        }

        // -----------------------------------------------------------------------
        // Apply methods
        // -----------------------------------------------------------------------

        private static void ApplyAddFile(Container container, AddFileChange change)
        {
            var fat = GetFatFileSystem(container);
            var (parentDir, name) = ResolvePath(fat, change.DestinationPath);

            using var sourceStream = change.Source.OpenRead();
            parentDir.WriteAddFile(name, sourceStream, change.Attributes, change.CreationTime, change.LastWriteTime, change.LastAccessTime);
        }

        private static void ApplyDelete(Container container, DeleteEntryChange change)
        {
            var fat = GetFatFileSystem(container);
            var (parentDir, name) = ResolvePath(fat, change.Path);

            var entry = parentDir.EnumerateFileSystemObjects(showHidden: true)
                .FirstOrDefault(e => string.Equals(e.Name, name, StringComparison.OrdinalIgnoreCase))
                ?? throw new FileNotFoundException($"Entry '{name}' not found.");

            switch (entry)
            {
                case FatFile file: file.WriteDelete(); break;
                case FatDirectory dir: dir.WriteDelete(); break;
                default: throw new NotSupportedException($"Cannot delete entry of type {entry.GetType().Name}");
            }
        }

        private static void ApplyCreateDirectory(Container container, CreateDirectoryChange change)
        {
            var fat = GetFatFileSystem(container);
            var (parentDir, name) = ResolvePath(fat, change.Path);
            parentDir.WriteCreateSubdirectory(name);
        }

        private static void ApplyRename(Container container, RenameChange change)
        {
            var fat = GetFatFileSystem(container);
            var (parentDir, oldName) = ResolvePath(fat, change.OldPath);

            var entry = parentDir.EnumerateFileSystemObjects(showHidden: true)
                .FirstOrDefault(e => string.Equals(e.Name, oldName, StringComparison.OrdinalIgnoreCase))
                ?? throw new FileNotFoundException($"Entry '{oldName}' not found.");

            switch (entry)
            {
                case FatFile file: file.WriteRename(change.NewName); break;
                case FatDirectory dir: dir.WriteRename(change.NewName); break;
                default: throw new NotSupportedException($"Cannot rename entry of type {entry.GetType().Name}");
            }
        }

        private static void ApplyVolumeLabel(Container container, VolumeLabelChange change)
        {
            var fat = GetFatFileSystem(container, change.PartitionIndex);
            fat.WriteVolumeLabel(change.NewLabel);
        }
    }
}
