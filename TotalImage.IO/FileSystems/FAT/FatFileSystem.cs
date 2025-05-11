using System;
using System.IO;
using System.Text;
using TotalImage.FileSystems.BPB;

namespace TotalImage.FileSystems.FAT
{
    public abstract class FatFileSystem : FileSystem
    {
        protected readonly BiosParameterBlock _bpb;

        /// <inheritdoc />
        public override Directory RootDirectory { get; }

        /// <inheritdoc />
        public override long TotalSize => TotalSectors * _bpb.BytesPerLogicalSector;

        /// <inheritdoc />
        public override long TotalFreeSpace => TotalFreeClusters * BytesPerCluster;

        /// <inheritdoc />
        public override uint AllocationUnitSize => BytesPerCluster;

        /// <summary>
        /// The BIOS Parameter Block for this FAT file system.
        /// </summary>
        public BiosParameterBlock BiosParameterBlock => _bpb;

        /// <summary>
        /// The number of free clusters in this volume.
        /// </summary>
        public virtual uint TotalFreeClusters
        {
            get
            {
                var clusters = 0u;

                for (var i = 0u; i < ClusterCount; i++)
                {
                    if (MainFat[i + 2] == 0) clusters++;
                }

                return clusters;
            }
        }

        /// <summary>
        /// The address of the first sector of the data area.
        /// </summary>
        public uint DataAreaFirstSector
        {
            get
            {
                var fatOffset = (uint)_bpb.ReservedLogicalSectors;
                var fatSize = (uint)_bpb.NumberOfFATs * _bpb.LogicalSectorsPerFAT;
                var rootDirSize = (uint)_bpb.RootDirectoryEntries * 32 / _bpb.BytesPerLogicalSector;
                return fatOffset + fatSize + RootDirectorySectors;
            }
        }

        /// <summary>
        /// The total number of logical sectors in this volume.
        /// </summary>
        public uint TotalSectors
            => _bpb.TotalLogicalSectors;

        /// <summary>
        /// The number of reserved logical sectors in this volume.
        /// </summary>
        public uint ReservedSectors
            => _bpb.ReservedLogicalSectors;

        /// <summary>
        /// The number of sectors used for all File allocation tables.
        /// </summary>
        public uint ClusterMapsSectors
            => (uint)_bpb.NumberOfFATs * _bpb.LogicalSectorsPerFAT;

        /// <summary>
        /// The number of sectors used for the root directory.
        /// </summary>
        public uint RootDirectorySectors
            => (uint)_bpb.RootDirectoryEntries * 32 / _bpb.BytesPerLogicalSector;

        /// <summary>
        /// The number of sectors used for the data area.
        /// </summary>
        public uint DataAreaSectors
            => TotalSectors - ReservedSectors - ClusterMapsSectors - RootDirectorySectors;

        /// <summary>
        /// The size of a cluster in bytes.
        /// </summary>
        public uint BytesPerCluster
            => (uint)_bpb.LogicalSectorsPerCluster * _bpb.BytesPerLogicalSector;

        /// <summary>
        /// The size of a File allocation table in bytes.
        /// </summary>
        public uint BytesPerClusterMap
            => (uint)_bpb.LogicalSectorsPerFAT * _bpb.BytesPerLogicalSector;

        /// <summary>
        /// The number of clusters in this volume.
        /// </summary>
        public uint ClusterCount
            => DataAreaSectors / _bpb.LogicalSectorsPerCluster;

        /// <summary>
        /// File allocation tables. Usually 2.
        /// </summary>
        public abstract FileAllocationTable[] Fats { get; }

        /// <summary>
        /// The "primary" FAT (usually the first one).
        /// </summary>
        public FileAllocationTable MainFat => Fats[0];

        /// <summary>
        /// The volume label. Returns the root directory volume label if it exists, otherwise the BPB volume label if it exists, otherwise a place holder.
        /// </summary>
        public override string VolumeLabel
        {
            get => RootDirectoryVolumeLabel is not null ? RootDirectoryVolumeLabel : BpbVolumeLabel is not null && BpbVolumeLabel.ToUpper() != "NO NAME    " ? BpbVolumeLabel : "";
            set => throw new NotImplementedException();
        }

        /// <summary>
        /// The volume label stored in the BIOS Parameter Block. This field is rarely used in place of the root directory volume label.
        /// </summary>
        public string? BpbVolumeLabel
        {
            get => BiosParameterBlock.Version == BiosParameterBlockVersion.Dos40 ? ((ExtendedBiosParameterBlock)BiosParameterBlock).VolumeLabel : null;
            set => throw new NotImplementedException();
        }

        /// <summary>
        /// The volume label stored as a file in the root directory. This field is usually used by most software.
        /// </summary>
        public string? RootDirectoryVolumeLabel
        {
            get
            {
                foreach (var (entry, _) in DirectoryEntry.EnumerateRootDirectory(this))
                {
                    if (entry.Attributes.HasFlag(FatAttributes.VolumeId) && !entry.Attributes.HasFlag(FatAttributes.LongName))
                    {
                        return Encoding.ASCII.GetString(entry.FileNameBytes).TrimEnd();
                    }
                }
                return null;
            }
            set => throw new NotImplementedException();
        }

        protected FatFileSystem(Stream stream, BiosParameterBlock bpb) : base(stream)
        {
            _bpb = bpb;
            RootDirectory = new FatDirectory(this);
        }        
    }
}
