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

        protected FatFileSystem(Stream stream, BiosParameterBlock bpb) : base(stream)
        { 
            _bpb = bpb;
            RootDirectory = new FatDirectory(this);
        }

        /// <inheritdoc />
        public override Directory RootDirectory { get; }

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

        public uint TotalSectors
            => (uint)_bpb.TotalLogicalSectors;

        public uint ReservedSectors
            => (uint)_bpb.ReservedLogicalSectors;

        public uint ClusterMapsSectors
            => (uint)_bpb.NumberOfFATs * _bpb.LogicalSectorsPerFAT;

        public uint RootDirectorySectors
            => (uint)_bpb.RootDirectoryEntries * 32 / _bpb.BytesPerLogicalSector;

        public uint DataAreaSectors
            => TotalSectors - ReservedSectors - ClusterMapsSectors - RootDirectorySectors;

        public uint BytesPerCluster
            => (uint)_bpb.LogicalSectorsPerCluster * _bpb.BytesPerLogicalSector;

        public uint BytesPerClusterMap
            => (uint)_bpb.LogicalSectorsPerFAT * _bpb.BytesPerLogicalSector;

        public uint ClusterCount
            => (uint)DataAreaSectors / _bpb.LogicalSectorsPerCluster;
        

        /// <summary>
        /// Retrieves the number of the next cluster in a cluster chain.
        /// </summary>
        /// <param name="index">Current cluster</param>
        /// <param name="fat">Specifies which copy of the FAT should be used</param>
        /// <returns>The next cluster number if any, otherwise <c>null</c>.</returns>
        public abstract uint? GetNextCluster(uint index, int fat = 0);

        public uint[] GetClusterChain(uint firstCluster)
        {
            var clusters = new List<uint>();
            var cluster = (uint?)firstCluster;

            while (cluster.HasValue)
            {
                clusters.Add(cluster.Value);
                cluster = GetNextCluster(cluster.Value);
            }

            return clusters.ToArray();
        }

        public abstract ClusterMap[] ClusterMaps { get; }
        public ClusterMap MainClusterMap => ClusterMaps[0];

        public abstract class ClusterMap : IEnumerable<uint>
        {
            public abstract uint this[uint index] { get; set; }

            public abstract uint Length { get; }

            /// <inheritdoc/>
            public IEnumerator<uint> GetEnumerator() => new ClusterMapEnumerator(this);

            /// <inheritdoc/>
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public class ClusterMapEnumerator : IEnumerator<uint>
            {
                uint _currentIndex = 0;
                ClusterMap _map;

                /// <inheritdoc/>
                public uint Current => _map[_currentIndex];

                /// <inheritdoc/>
                object IEnumerator.Current => Current;

                internal ClusterMapEnumerator(ClusterMap map)
                {
                    _map = map;
                }

                /// <inheritdoc/>
                public void Dispose() { }

                /// <inheritdoc/>
                public bool MoveNext() => ++_currentIndex < _map.Length ? false : true;

                /// <inheritdoc/>
                public void Reset() => _currentIndex = 0;
            }
        }

        public override string VolumeLabel { get => RootDirectoryVolumeLabel ?? BpbVolumeLabel ?? "NO NAME"; set => throw new NotImplementedException(); }

        public string? BpbVolumeLabel
        {
            get => BiosParameterBlock.Version == BiosParameterBlockVersion.Dos40 ? ((ExtendedBiosParameterBlock)BiosParameterBlock).VolumeLabel : null;
            set => throw new NotImplementedException();
        }

        public string? RootDirectoryVolumeLabel
        {
            get
            {
                foreach(var entry in DirectoryEntry.ReadRootDirectory(this))
                {
                    if (entry.attr.HasFlag(FatAttributes.VolumeId) && !entry.attr.HasFlag(FatAttributes.LongName))
                    {
                        return Helper.TrimFileName(Encoding.ASCII.GetString(entry.name));
                    }
                }
                return null;
            }
            set => throw new NotImplementedException();
        }
    }
}