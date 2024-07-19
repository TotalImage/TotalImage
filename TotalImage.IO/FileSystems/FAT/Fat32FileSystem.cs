using System;
using System.IO;
using System.Text;
using TotalImage.FileSystems.BPB;

namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// A representation of a FAT32 file system
    /// </summary>
    public class Fat32FileSystem : FatFileSystem
    {
        private FsInfo _fsInfo;

        /// <inheritdoc />
        public override string DisplayName => "FAT32";

        /// <inheritdoc />
        public override uint TotalFreeClusters
            => _fsInfo.IsValid && _fsInfo.freeCount <= ClusterCount ? _fsInfo.freeCount : base.TotalFreeClusters;

        /// <inheritdoc />
        public override FAT.FileAllocationTable[] Fats { get; }

        /// <inheritdoc />
        public override bool SupportsSubdirectories => true;

        /// <inheritdoc />
        public override bool IsReadOnly => false;

        public Fat32FileSystem(Stream stream, BiosParameterBlock bpb) : base(stream, bpb)
        {
            Fats = new FAT.FileAllocationTable[bpb.NumberOfFATs];
            for(int i = 0; i < bpb.NumberOfFATs; i++)
                Fats[i] = new FileAllocationTable(this, i);

            if (bpb is Fat32BiosParameterBlock fat32bpb)
            {
                using var reader = new BinaryReader(stream, Encoding.ASCII, true);
                stream.Position = fat32bpb.BytesPerLogicalSector * fat32bpb.FsInfo;
                _fsInfo = FsInfo.Parse(reader);
            }
        }

        private class FileAllocationTable : FAT.FileAllocationTable
        {
            Fat32FileSystem _fat32;
            int _fatIndex;

            internal FileAllocationTable(Fat32FileSystem fat32, int fatIndex)
            {
                if (fatIndex >= fat32._bpb.NumberOfFATs || fatIndex < 0) throw new ArgumentOutOfRangeException();

                _fat32 = fat32;
                _fatIndex = fatIndex;
            }

            protected override uint Mask => 0xFFFFFFF; // The most significant 4 bits are reserved

            public override uint Length
                => (uint)(_fat32.BytesPerClusterMap) / 4;

            public override uint this[uint index]
            {
                get
                {
                    index &= Mask;

                    if (index >= Length) throw new ArgumentOutOfRangeException();

                    var stream = _fat32.GetStream();
                    using var reader = new BinaryReader(stream, Encoding.ASCII, true);

                    // Seek to the beginning of the cluster map.
                    stream.Position = _fat32._bpb.ReservedLogicalSectors * _fat32._bpb.BytesPerLogicalSector;

                    if (_fatIndex > 0)
                    {
                        // Reading from a backup FAT, so seek to the beginning of that.
                        var fatOffset = _fatIndex * _fat32._bpb.LogicalSectorsPerFAT * _fat32._bpb.BytesPerLogicalSector;
                        reader.BaseStream.Seek(fatOffset, SeekOrigin.Current);
                    }

                    // 32-bit cluster map values. Nothing to see here
                    reader.BaseStream.Seek(index * 4, SeekOrigin.Current);
                    return reader.ReadUInt32() & Mask;
                }

                set
                {
                    index &= Mask;

                    if (index >= Length) throw new ArgumentOutOfRangeException();

                    var stream = _fat32.GetStream();
                    using var writer = new BinaryWriter(stream, Encoding.ASCII, true);

                    // Seek to the beginning of the cluster map.
                    stream.Position = _fat32._bpb.ReservedLogicalSectors * _fat32._bpb.BytesPerLogicalSector;

                    if (_fatIndex > 0)
                    {
                        // Reading from a backup FAT, so seek to the beginning of that.
                        var fatOffset = _fatIndex * _fat32._bpb.LogicalSectorsPerFAT * _fat32._bpb.BytesPerLogicalSector;
                        writer.BaseStream.Seek(fatOffset, SeekOrigin.Current);
                    }

                    // 32-bit cluster map values. Nothing to see here
                    writer.BaseStream.Seek(index * 4, SeekOrigin.Current);
                    writer.Write(value & Mask);
                }
            }
        }
    }
}
