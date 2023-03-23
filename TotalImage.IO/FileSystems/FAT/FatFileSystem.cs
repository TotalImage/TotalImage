using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TotalImage.FileSystems.FAT
{
    public abstract class FatFileSystem : FileSystem
    {
        protected readonly BiosParameterBlock _bpb;
        public BiosParameterBlock BiosParameterBlock => _bpb;

        protected FatFileSystem(Stream stream, BiosParameterBlock bpb) : base(stream)
        {
            _bpb = bpb;
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
                var rootDirSize = (uint)_bpb.RootDirectoryEntries * 32 / _bpb.BytesPerLogicalSector;
                return (uint)(fatOffset + fatSize + RootDirectorySectors);
            }
        }

        public uint TotalSectors
            => (uint)_bpb.TotalLogicalSectors;

        public uint ReservedSectors
            => (uint)_bpb.ReservedLogicalSectors;

        public uint ClusterMapsSectors
            => (uint)_bpb.NumberOfFATs * (uint)_bpb.LogicalSectorsPerFAT;

        public uint RootDirectorySectors
            => (uint)_bpb.RootDirectoryEntries * 32 / (uint)_bpb.BytesPerLogicalSector;

        public uint DataAreaSectors
            => TotalSectors - ReservedSectors - ClusterMapsSectors - RootDirectorySectors;

        public uint BytesPerCluster
            => (uint)_bpb.LogicalSectorsPerCluster * (uint)_bpb.BytesPerLogicalSector;

        public uint BytesPerClusterMap
            => (uint)_bpb.LogicalSectorsPerFAT * (uint)_bpb.BytesPerLogicalSector;

        public uint ClusterCount
            => DataAreaSectors / (uint)_bpb.LogicalSectorsPerCluster;

        public abstract FileAllocationTable[] Fats { get; }
        public FileAllocationTable MainFat => Fats[0];

        public override string VolumeLabel
        {
            get => RootDirectoryVolumeLabel ?? BpbVolumeLabel ?? "NO NAME";
            set => throw new NotImplementedException();
        }

        public string? BpbVolumeLabel
        {
            get => BiosParameterBlock.VolumeLabel;
            set => throw new NotImplementedException();
        }

        public string? RootDirectoryVolumeLabel
        {
            get
            {
                IEnumerable<DirectoryEntry> entries;

                if (_bpb.RootDirectoryEntries == 0 && _bpb.RootDirectoryCluster is not null)
                    entries = DirectoryEntry.ReadSubdirectory(this, _bpb.RootDirectoryCluster.Value, false);
                else
                    entries = DirectoryEntry.ReadRootDirectory(this);

                foreach(var entry in entries)
                {
                    if (entry.attr.HasFlag(FatAttributes.VolumeId) && !entry.attr.HasFlag(FatAttributes.LongName))
                    {
                        return Encoding.ASCII.GetString(entry.name).TrimEnd();
                    }
                }
                return null;
            }
            set => throw new NotImplementedException();
        }
    }
}
