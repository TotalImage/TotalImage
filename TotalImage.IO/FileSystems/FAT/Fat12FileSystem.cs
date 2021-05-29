﻿using System;
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
        public static Fat12FileSystem Create(Stream stream, BiosParameterBlock bpb)
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


        /* Changes the volume label
         * TODO: Rewrite this so the two labels can be changed separately
         *
         * If BPB version <= 3.31, only the root dir label is changed
         * If BPB version >= 3.40, both the root dir and BPB label are changed
         */
        public void ChangeVolLabel(string label)
        {
            if (string.IsNullOrEmpty(label))
                throw new ArgumentNullException(nameof(label), "label cannot be null!");

            uint rootDirOffset = (uint)(_bpb.BytesPerLogicalSector + (_bpb.BytesPerLogicalSector * _bpb.LogicalSectorsPerFAT * 2));
            bool rootDirFull = false;

            using (var reader = new BinaryReader(_stream, Encoding.ASCII, true))
            using (var writer = new BinaryWriter(_stream, Encoding.ASCII, true))
            {
                _stream.Seek(rootDirOffset, SeekOrigin.Begin);

                for (int i = 0; i < _bpb.RootDirectoryEntries; i++)
                {
                    _stream.Seek(rootDirOffset + i * 0x20, SeekOrigin.Begin);
                    byte firstChar = reader.ReadByte();

                    /* 0x00      = no more entries after this one, write the volume label
                     * 0xE5/0x05 = deleted entry, only overwrite it if the directory is full */
                    if (firstChar == 0x00)
                    {
                        //Root dir is empty, so let's add the first entry
                        _stream.Seek(-0x01, SeekOrigin.Current);
                        writer.Write(label.ToCharArray());
                        writer.Write((byte)0x08); //Volume label attribute
                        break;
                    }
                    else if (firstChar == 0xE5 || firstChar == 0x05)
                    {
                        if (rootDirFull)
                        {
                            _stream.Seek(-0x01, SeekOrigin.Current);
                            writer.Write(label.ToCharArray());
                            writer.Write((byte)0x08); //Volume label attribute
                            rootDirFull = false;
                            break;
                        }
                        else continue;
                    }

                    /* Root directory is not empty, we need to find the volume label, as it may not be the first entry or it may not exist at all */
                    _stream.Seek(-0x01, SeekOrigin.Current);
                    DirectoryEntry entry = new DirectoryEntry
                    {
                        name = reader.ReadBytes(11),
                        attr = (FatAttributes)reader.ReadByte(),
                        ntRes = reader.ReadByte(),
                        crtTimeTenth = reader.ReadByte(),
                        crtTime = reader.ReadUInt16(),
                        crtDate = reader.ReadUInt16(),
                        lstAccDate = reader.ReadUInt16(),
                        fstClusHI = reader.ReadUInt16(),
                        wrtTime = reader.ReadUInt16(),
                        wrtDate = reader.ReadUInt16(),
                        fstClusLO = reader.ReadUInt16(),
                        fileSize = reader.ReadUInt32()
                    };

                    if (entry.attr == FatAttributes.VolumeId)
                    {
                        _stream.Seek(-0x20, SeekOrigin.Current);
                        writer.Write(label.ToCharArray());
                        writer.Write((byte)0x08);
                        break;
                    }

                    /* All entries have been checked and there are no free ones left
                     * Time to check again, this time overwriting the first deleted entry that's found */
                    if (i == _bpb.RootDirectoryEntries - 1 && !rootDirFull)
                    {
                        rootDirFull = true;
                        i = 0;
                    }
                }

                //Writes the volume label to the BPB as well if BPBP is for DOS 4.0+
                if (_bpb is ExtendedBiosParameterBlock && _bpb.Version == BiosParameterBlockVersion.Dos40)
                {
                    _stream.Seek(0x2B, SeekOrigin.Begin);
                    writer.Write(label.ToCharArray());
                }

                if (rootDirFull)
                {
                    throw new Exception("Root directory is full, volume label cannot be written");
                }
            }
        }

        //Reads the specified cluster in the data area and returns its bytes
        public byte[] ReadCluster(uint cluster)
        {
            if (cluster < 2 || cluster > 0xFEF)
                return Array.Empty<byte>();

            uint fat1Offset = (uint)(_bpb.BytesPerLogicalSector * _bpb.ReservedLogicalSectors);
            uint fatSize = (uint)_bpb.BytesPerLogicalSector * _bpb.LogicalSectorsPerFAT;
            uint dataAreaOffset = (uint)(fat1Offset + fatSize * 2 + (_bpb.RootDirectoryEntries << 5));

            using (var reader = new BinaryReader(_stream, Encoding.ASCII, true))
            {
                uint clusterOffset = (cluster - 2) * _bpb.LogicalSectorsPerCluster * _bpb.BytesPerLogicalSector;
                _stream.Seek(dataAreaOffset + clusterOffset, SeekOrigin.Begin);
                byte[] bytes = reader.ReadBytes(_bpb.BytesPerLogicalSector * _bpb.LogicalSectorsPerCluster);

                return bytes;
            }
        }

        //Writes data to the specified cluster in the data area
        public void WriteCluster(uint cluster, byte[] data)
        {
            if (cluster < 2 || cluster > 0xFEF)
                return;

            uint fat1Offset = (uint)(_bpb.BytesPerLogicalSector * _bpb.ReservedLogicalSectors);
            uint fatSize = (uint)_bpb.BytesPerLogicalSector * _bpb.LogicalSectorsPerFAT;
            uint dataAreaOffset = (uint)(fat1Offset + fatSize * 2 + (_bpb.RootDirectoryEntries << 5));

            using (var writer = new BinaryWriter(_stream, Encoding.ASCII, true))
            {
                uint clusterOffset = (cluster - 2) * _bpb.LogicalSectorsPerCluster * _bpb.BytesPerLogicalSector;
                _stream.Seek(dataAreaOffset + clusterOffset, SeekOrigin.Begin);
                writer.Write(data);
            }
        }

        //Marks a cluster in the FATs as free (0x00)
        public void FatFreeCluster(uint cluster)
        {
            uint fat1Offset = (uint)(_bpb.BytesPerLogicalSector * _bpb.ReservedLogicalSectors);
            uint fatSize = (uint)_bpb.BytesPerLogicalSector * _bpb.LogicalSectorsPerFAT;
            uint fat2Offset = fat1Offset + fatSize;

            using (var writer = new BinaryWriter(_stream, Encoding.ASCII, true))
            using (var reader = new BinaryReader(_stream, Encoding.ASCII, true))
            {
                if (cluster % 2 == 0)
                {
                    _stream.Seek(fat1Offset + (uint)(cluster * 1.5), SeekOrigin.Begin);
                    writer.Write((byte)0x00);
                    byte upper4 = (byte)(reader.ReadByte() & 0xF0); //Zero out the bottom 4 bits only - the upper 4 are for the next cluster!
                    _stream.Seek(-8, SeekOrigin.Current);
                    writer.Write(upper4);

                    //Repeat the process for the backup FAT
                    _stream.Seek(fat2Offset + (uint)(cluster * 1.5), SeekOrigin.Begin);
                    writer.Write((byte)0x00);
                    writer.Write(upper4);
                }
                else
                {
                    _stream.Seek(fat1Offset + (uint)Math.Floor(cluster * 1.5), SeekOrigin.Begin);
                    byte lower4 = (byte)(reader.ReadByte() & 0x0F); //Zero out the top 4 bits only - the bottom 4 are for the previous cluster!
                    _stream.Seek(-8, SeekOrigin.Current);
                    writer.Write(lower4);
                    writer.Write(0x00);

                    _stream.Seek(fat2Offset + (uint)Math.Floor(cluster * 1.5), SeekOrigin.Begin);
                    writer.Write(lower4);
                    writer.Write(0x00);
                }
            }
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