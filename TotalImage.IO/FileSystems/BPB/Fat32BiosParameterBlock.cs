using System.IO;

namespace TotalImage.FileSystems.BPB
{
    /// <summary>
    /// A FAT32-specific version of the Extended BIOS Parameter Block.
    /// </summary>
    public class Fat32BiosParameterBlock : ExtendedBiosParameterBlock
    {
        /// <inheritdoc />
        public override BiosParameterBlockVersion Version => BiosParameterBlockVersion.Fat32;

        /// <summary>
        /// FAT32 extended flags.
        /// </summary>
        public ushort ExtFlags { get; private set; }

        /// <summary>
        /// FAT32 file system revision.
        /// </summary>
        public ushort FileSystemVersion { get; private set; }

        /// <summary>
        /// The first cluster of the root directory.
        /// </summary>
        public uint RootDirectoryCluster { get; private set; }

        /// <summary>
        /// Sector number of the FSINFO structure.
        /// </summary>
        public ushort FsInfo { get; private set; }

        /// <summary>
        /// Sector number of the backup boot sector.
        /// </summary>
        public ushort BackupBootSector { get; private set; }

        /// <summary>
        /// Reserved FAT32 boot sector bytes.
        /// </summary>
        public byte[] Reserved { get; private set; } = new byte[11];

        /// <summary>
        /// Creates a FAT32 BIOS parameter block from a base BIOS parameter block.
        /// </summary>
        /// <param name="bpb">The base BIOS parameter block.</param>
        public Fat32BiosParameterBlock(BiosParameterBlock bpb) : base(bpb) { }

        /// <summary>
        /// Continues parsing a FAT32 BIOS parameter block from a stream.
        /// </summary>
        /// <param name="bpb">The base BIOS parameter block that was already parsed.</param>
        /// <param name="reader">The reader positioned at the FAT32-specific fields.</param>
        /// <returns>The parsed FAT32 BIOS parameter block, or <see langword="null"/> if the extended fields are invalid.</returns>
        public new static Fat32BiosParameterBlock? ContinueParsing(BiosParameterBlock bpb, BinaryReader reader)
        {
            var fat32bpb = new Fat32BiosParameterBlock(bpb);
            fat32bpb.ReadFat32BpbFields(reader);
            return fat32bpb.ReadEbpbFields(reader) ? fat32bpb : null;
        }

        private void ReadFat32BpbFields(BinaryReader reader)
        {
            LogicalSectorsPerFAT = reader.ReadUInt32();
            ExtFlags = reader.ReadUInt16();
            FileSystemVersion = reader.ReadUInt16();
            RootDirectoryCluster = reader.ReadUInt32();
            FsInfo = reader.ReadUInt16();
            BackupBootSector = reader.ReadUInt16();
            Reserved = reader.ReadBytes(12);
        }
    }
}
