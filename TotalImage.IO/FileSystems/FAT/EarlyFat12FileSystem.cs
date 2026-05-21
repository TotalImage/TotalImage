using System.Collections.Generic;
using System.IO;
using TotalImage.FileSystems.BPB;

namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// Represents a FAT12 file system using the early 86-DOS (pre-0.42) 16-byte directory entry format.
    /// These volumes have no BPB, no timestamps, no attributes, and no subdirectory support.
    /// </summary>
    public class EarlyFat12FileSystem : Fat12FileSystem
    {
        /// <inheritdoc />
        public override string DisplayName => "FAT12 (early)";

        /// <inheritdoc />
        public override bool SupportsSubdirectories => false;

        /// <inheritdoc />
        public override bool IsReadOnly => true;

        /// <summary>
        /// The size of a single directory entry on this volume (16 bytes).
        /// </summary>
        public override int DirectoryEntrySize => 16;

        /// <summary>
        /// Opens an early FAT12 file system from an existing stream and BIOS parameter block.
        /// </summary>
        /// <param name="stream">The stream containing the file system.</param>
        /// <param name="bpb">The parsed BIOS parameter block.</param>
        public EarlyFat12FileSystem(Stream stream, BiosParameterBlock bpb) : base(stream, bpb) { }

        /// <inheritdoc />
        public override IEnumerable<(DirectoryEntry entry, LongDirectoryEntry[] lfnEntries)> EnumerateRootDirectoryEntries()
        {
            foreach (var small in SmallDirectoryEntry.EnumerateRootDirectory(this))
            {
                //Represent the small entry as a standard DirectoryEntry for use by the rest of the system.
                var nameBytes = new byte[11];
                small.FileNameBytes.CopyTo(nameBytes);

                var entry = new DirectoryEntry(
                    shortName: nameBytes,
                    attributes: 0,
                    firstCluster: small.FirstClusterOfFile,
                    fileSize: small.FileSize,
                    creationTime: null,
                    lastWriteTime: null,
                    lastAccessTime: null);

                yield return (entry, []);
            }
        }
    }
}
