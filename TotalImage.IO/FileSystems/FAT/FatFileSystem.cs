using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TotalImage.FileSystems.BPB;

namespace TotalImage.FileSystems.FAT
{
    public abstract class FatFileSystem : FileSystem
    {
        protected readonly BiosParameterBlock _bpb;
        public BiosParameterBlock BiosParameterBlock => _bpb;
        protected readonly bool _isSmall;
        public bool IsSmall => _isSmall;

        protected FatFileSystem(Stream stream, BiosParameterBlock bpb) : base(stream)
        {
            _bpb = bpb;
            _isSmall = bpb.IsSmall;
            RootDirectory = new FatDirectory(this);
        }

        /// <inheritdoc />
        public override Directory RootDirectory { get; }

        /// <inheritdoc />
        public override long TotalSize => TotalSectors * _bpb.BytesPerLogicalSector;

        /// <inheritdoc />
        public override long TotalFreeSpace => TotalFreeClusters * BytesPerCluster;

        /// <inheritdoc />
        public override uint AllocationUnitSize => BytesPerCluster;

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

        public uint DataAreaFirstSector
        {
            get
            {
                var fatOffset = (uint)_bpb.ReservedLogicalSectors;
                var fatSize = (uint)_bpb.NumberOfFATs * _bpb.LogicalSectorsPerFAT;
                var rootDirSize = (uint)_bpb.RootDirectoryEntries * (IsSmall ? 16u : 32u) / _bpb.BytesPerLogicalSector;
                return fatOffset + fatSize + RootDirectorySectors;
            }
        }

        public uint TotalSectors
            => _bpb.TotalLogicalSectors;

        public uint ReservedSectors
            => _bpb.ReservedLogicalSectors;

        public uint ClusterMapsSectors
            => (uint)_bpb.NumberOfFATs * _bpb.LogicalSectorsPerFAT;

        public uint RootDirectorySectors
            => (uint)_bpb.RootDirectoryEntries * (IsSmall ? 16u : 32u) / _bpb.BytesPerLogicalSector;

        public uint DataAreaSectors
            => TotalSectors - ReservedSectors - ClusterMapsSectors - RootDirectorySectors;

        public uint BytesPerCluster
            => (uint)_bpb.LogicalSectorsPerCluster * _bpb.BytesPerLogicalSector;

        public uint BytesPerClusterMap
            => (uint)_bpb.LogicalSectorsPerFAT * _bpb.BytesPerLogicalSector;

        public uint ClusterCount
            => DataAreaSectors / _bpb.LogicalSectorsPerCluster;

        public abstract FileAllocationTable[] Fats { get; }
        public FileAllocationTable MainFat => Fats[0];

        public override string VolumeLabel
        {
            get => RootDirectoryVolumeLabel is not null ? RootDirectoryVolumeLabel : BpbVolumeLabel is not null && BpbVolumeLabel.ToUpper() != "NO NAME    " ? BpbVolumeLabel : "<no label>";
            set => throw new NotImplementedException();
        }

        public string? BpbVolumeLabel
        {
            get => BiosParameterBlock.Version == BiosParameterBlockVersion.Dos40 ? ((ExtendedBiosParameterBlock)BiosParameterBlock).VolumeLabel : null;
            set => throw new NotImplementedException();
        }

        public string? RootDirectoryVolumeLabel
        {
            get
            {
                foreach(var (entry, _) in DirectoryEntry.EnumerateRootDirectory(this))
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
    }
}
