using System;
using System.IO;
using System.Text;
using TotalImage.FileSystems.BPB;

namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// A representation of a FAT16 file system
    /// </summary>
    public class Fat16FileSystem : FatFileSystem
    {
        public Fat16FileSystem(Stream stream, BiosParameterBlock bpb) : base(stream, bpb)
        {
            ClusterMaps = new ClusterMap[bpb.NumberOfFATs];
            for(int i = 0; i < bpb.NumberOfFATs; i++)
                ClusterMaps[i] = new ClusterMap(this, i);
        }

        /// <inheritdoc />
        public override string DisplayName => "FAT16";

        /// <inheritdoc />
        public override long AvailableFreeSpace => throw new NotImplementedException();

        /// <inheritdoc />
        public override long TotalFreeSpace => throw new NotImplementedException();

        /// <inheritdoc />
        public override long TotalSize => throw new NotImplementedException();

        /// <inheritdoc />
        public override uint? GetNextCluster(uint index, int fat = 0)
            => index > 1 && ClusterMaps[fat][index] < 0xFFEF ? (uint?)ClusterMaps[fat][index] : null;

        /// <inheritdoc />
        public override FatFileSystem.ClusterMap[] ClusterMaps { get; }

        private new class ClusterMap : FatFileSystem.ClusterMap
        {
            Fat16FileSystem _fat16;
            int _fatIndex;
            internal ClusterMap(Fat16FileSystem fat16, int fatIndex)
            {
                if (fatIndex >= fat16._bpb.NumberOfFATs || fatIndex < 0) throw new ArgumentOutOfRangeException();

                _fat16 = fat16;
                _fatIndex = fatIndex;
            }

            public override uint Length
                => (uint)(_fat16.BytesPerClusterMap) / 2;

            public override uint this[uint index]
            {
                get
                {
                    if (index >= Length) throw new ArgumentOutOfRangeException();

                    var stream = _fat16.GetStream();
                    using var reader = new BinaryReader(stream, Encoding.ASCII, true);

                    // Seek to the beginning of the cluster map.
                    stream.Position = _fat16._bpb.ReservedLogicalSectors * _fat16._bpb.BytesPerLogicalSector;

                    if (_fatIndex > 0)
                    {
                        // Reading from a backup FAT, so seek to the beginning of that.
                        var fatOffset = _fatIndex * _fat16._bpb.LogicalSectorsPerFAT * _fat16._bpb.BytesPerLogicalSector;
                        reader.BaseStream.Seek(fatOffset, SeekOrigin.Current);
                    }

                    // 16-bit cluster map values. Nothing to see here
                    reader.BaseStream.Seek(index * 2, SeekOrigin.Current);
                    return reader.ReadUInt16();
                }

                set
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}