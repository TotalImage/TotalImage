using System;
using System.IO;
using System.Text;
using TotalImage.FileSystems.BPB;

namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// A representation of a FAT12 file system
    /// </summary>
    public class Fat12FileSystem : FatFileSystem
    {
        /// <inheritdoc />
        public override string DisplayName => "FAT12";

        public Fat12FileSystem(Stream stream, BiosParameterBlock bpb) : base(stream, bpb)
        {
            ClusterMaps = new ClusterMap[bpb.NumberOfFATs];
            for(int i = 0; i < bpb.NumberOfFATs; i++)
                ClusterMaps[i] = new ClusterMap(this, i);
        }

        //Formats a volume with FAT12 file system - currently assumes it's a floppy disk...
        public static Fat12FileSystem Create(Stream stream, BiosParameterBlock bpb, bool writeBPB)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream), "stream cannot be null!");
            if (bpb == null)
                throw new ArgumentNullException(nameof(bpb), "bpb cannot be null!");
            if (!bpb.Validate())
                throw new InvalidDataException("bpb is invalid!");

            var fat = new Fat12FileSystem(stream, bpb);

            uint totalSize = (uint)stream.Length;
            uint rootDirSize = (uint)(bpb.RootDirectoryEntries << 5);
            uint fatSize = (uint)(bpb.LogicalSectorsPerFAT * bpb.BytesPerLogicalSector);
            uint fat1Offset = (uint)(bpb.ReservedLogicalSectors * bpb.BytesPerLogicalSector);
            uint fat2Offset = fat1Offset + fatSize;
            uint dataAreaOffset = fat2Offset + fatSize + rootDirSize;

            using (var writer = new BinaryWriter(stream, Encoding.ASCII, true))
            {
                stream.Seek(0, SeekOrigin.Begin);
                for (uint i = 0; i < totalSize; i++)
                {
                    writer.Write((byte)0);
                }

                //Fille the data area with 0xF6
                stream.Seek(dataAreaOffset, SeekOrigin.Begin);
                for (uint i = 0; i < totalSize - dataAreaOffset; i++)
                {
                    writer.Write((byte)0xF6);
                }

                stream.Seek(0, SeekOrigin.Begin);
                writer.Write(bpb.BootJump, 0, 3);
                writer.Write(bpb.OemId.PadRight(8, ' ').ToCharArray());

                if (writeBPB)
                {
                    writer.Write(bpb.BytesPerLogicalSector);
                    writer.Write(bpb.LogicalSectorsPerCluster);
                    writer.Write(bpb.ReservedLogicalSectors);
                    writer.Write(bpb.NumberOfFATs);
                    writer.Write(bpb.RootDirectoryEntries);
                    writer.Write(bpb.TotalLogicalSectors <= ushort.MaxValue ? (ushort)bpb.TotalLogicalSectors : (ushort)0);
                    writer.Write(bpb.MediaDescriptor);
                    writer.Write(bpb.Version != BiosParameterBlockVersion.Fat32 ? (ushort)bpb.LogicalSectorsPerFAT : (ushort)0);
                    writer.Write(bpb.PhysicalSectorsPerTrack);
                    writer.Write(bpb.NumberOfHeads);
                    writer.Write(bpb.HiddenSectors);
                    writer.Write(bpb.TotalLogicalSectors > ushort.MaxValue ? bpb.TotalLogicalSectors : 0);

                    //DOS 3.4+ specific values
                    {
                        if (bpb.Version == BiosParameterBlockVersion.Dos40)
                        {
                            var ebpb = (ExtendedBiosParameterBlock)bpb;
                            writer.Write(ebpb.PhysicalDriveNumber);
                            writer.Write(ebpb.Flags);
                            writer.Write((byte)ebpb.ExtendedBootSignature);

                            //DOS 4.0 adds volume label and FS type as well
                            if (ebpb.ExtendedBootSignature == ExtendedBootSignature.Dos40)
                            {
                                writer.Write(ebpb.VolumeSerialNumber.Value);
                                if (string.IsNullOrEmpty(ebpb.VolumeLabel))
                                    writer.Write("NO NAME    ".ToCharArray());
                                else
                                    writer.Write(ebpb.VolumeLabel.PadRight(11, ' ').ToCharArray());
                                writer.Write(ebpb.FileSystemType.PadRight(8, ' ').ToCharArray());
                            }
                        }
                    }
                }

                //Boot signature
                stream.Seek(0x1FE, SeekOrigin.Begin);
                writer.Write((byte)0x55);
                writer.Write((byte)0xAA);

                /* Media descriptor needs to be written to each FAT as well, upper 4 bits must all be set.
                 * It takes up the first cluster entry (0), and the second entry (1) is also reserved */
                stream.Seek(fat1Offset, SeekOrigin.Begin);
                writer.Write(bpb.MediaDescriptor);
                writer.Write((byte)0xFF);
                writer.Write((byte)0xFF);
                stream.Seek(fat2Offset, SeekOrigin.Begin);
                writer.Write(bpb.MediaDescriptor);
                writer.Write((byte)0xFF);
                writer.Write((byte)0xFF);

                //Volume label needs to be written to the root directory as well
                stream.Seek(fat2Offset + fatSize, SeekOrigin.Begin);

                //First 11 bytes (8.3 space-padded filename without the period) are the label itself
                {
                    if (bpb is ExtendedBiosParameterBlock bpb40 && !string.IsNullOrEmpty(bpb40.VolumeLabel))
                    {
                        writer.Write(bpb40.VolumeLabel.PadRight(11, ' ').ToCharArray());
                        writer.Write((byte)0x08); //Volume label attribute
                    }
                }
            }

            return fat;
        }

        /// <inheritdoc />
        protected override uint ClusterMask => 0xFFF;

        /// <inheritdoc />
        public override FatFileSystem.ClusterMap[] ClusterMaps { get; }

        private new class ClusterMap : FatFileSystem.ClusterMap
        {
            Fat12FileSystem _fat12;
            int _fatIndex;
            internal ClusterMap(Fat12FileSystem fat12, int fatIndex)
            {
                if (fatIndex >= fat12._bpb.NumberOfFATs || fatIndex < 0) throw new ArgumentOutOfRangeException();

                _fat12 = fat12;
                _fatIndex = fatIndex;
            }

            public override uint Length
                => (uint)(_fat12.BytesPerClusterMap) / 2 * 3;

            public override uint this[uint index]
            {
                get
                {
                    if (index >= Length) throw new ArgumentOutOfRangeException();

                    var stream = _fat12.GetStream();
                    using var reader = new BinaryReader(stream, Encoding.ASCII, true);

                    // Seek to the beginning of the cluster map.
                    stream.Position = _fat12._bpb.ReservedLogicalSectors * _fat12._bpb.BytesPerLogicalSector;

                    if (_fatIndex > 0)
                    {
                        // Reading from a backup FAT, so seek to the beginning of that.
                        var fatOffset = _fatIndex * _fat12._bpb.LogicalSectorsPerFAT * _fat12._bpb.BytesPerLogicalSector;
                        reader.BaseStream.Seek(fatOffset, SeekOrigin.Current);
                    }

                    // FAT12 uses 12-bit cluster indices, therefore it's time for some
                    // crazy maths! Let's first seek further to the nearest even index.
                    reader.BaseStream.Seek(index / 2 * 3, SeekOrigin.Current);
        
                    // Now we want to read two values. Considering there is no 24-bit
                    // integer type, we have to read 32 bits, which means we're going
                    // to read more than we need, so we have to discard the most
                    // significant byte.
                    var pair = reader.ReadUInt32() & 0xFFFFFF;

                    // Right now, `pair` has the value of 0x00123ABC, bits 0-11 contain
                    // the value of the even index and bits 12-23 contain the value of
                    // the odd index. All we need to do is return the relevant part.
                    if (index % 2 == 0)
                        return pair & 0xFFF;
                    else
                        return pair >> 12;
                }

                set
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}