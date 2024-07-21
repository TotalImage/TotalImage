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
        /// <inheritdoc />
        public override string DisplayName => "FAT16";

        /// <inheritdoc />
        public override FAT.FileAllocationTable[] Fats { get; }

        /// <inheritdoc />
        public override bool SupportsSubdirectories => true;

        public Fat16FileSystem(Stream stream, BiosParameterBlock bpb) : base(stream, bpb)
        {
            Fats = new FAT.FileAllocationTable[bpb.NumberOfFATs];
            for(int i = 0; i < bpb.NumberOfFATs; i++)
                Fats[i] = new FileAllocationTable(this, i);
        }

        private class FileAllocationTable : FAT.FileAllocationTable
        {
            Fat16FileSystem _fat16;
            int _fatIndex;

            internal FileAllocationTable(Fat16FileSystem fat16, int fatIndex)
            {
                if (fatIndex >= fat16._bpb.NumberOfFATs || fatIndex < 0) throw new ArgumentOutOfRangeException();

                _fat16 = fat16;
                _fatIndex = fatIndex;
            }

            protected override uint Mask => 0xFFFF;

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
                    if (index >= Length) throw new ArgumentOutOfRangeException();

                    var stream = _fat16.GetStream();
                    using var writer = new BinaryWriter(stream, Encoding.ASCII, true);

                    // Seek to the beginning of the cluster map.
                    stream.Position = _fat16._bpb.ReservedLogicalSectors * _fat16._bpb.BytesPerLogicalSector;

                    if (_fatIndex > 0)
                    {
                        // Reading from a backup FAT, so seek to the beginning of that.
                        var fatOffset = _fatIndex * _fat16._bpb.LogicalSectorsPerFAT * _fat16._bpb.BytesPerLogicalSector;
                        writer.BaseStream.Seek(fatOffset, SeekOrigin.Current);
                    }

                    //Write the new value to the cluster map
                    writer.BaseStream.Seek(index * 2, SeekOrigin.Current);
                    writer.Write((ushort)(value & Mask));
                }
            }
        }
    }
}
