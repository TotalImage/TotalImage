using System;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using TotalImage.Helpers;
using TotalImage.Partitions;

namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// A representation of a FAT12 file system
    /// </summary>
    public class Fat12 : FileSystem
    {
        private readonly BootSector _bs;
        private readonly IBiosParameterBlock? _bpb;
        private Directory _rootDirectory;

        public BootSector? BootSector => _bs;

        /// <summary>
        /// The BIOS Parameter Block structure for the file system
        /// </summary>
        public IBiosParameterBlock? BiosParameterBlock => _bpb;

        /// <inheritdoc />
        public override string Format => "FAT12";

        /// <inheritdoc />
        public override string VolumeLabel
        {
            get => _bpb is BiosParameterBlock40 bpb40 && bpb40.BpbVersion == BiosParameterBlockVersion.Dos40 ? bpb40.VolumeLabel : "UNSUPPORTED";
            set => ChangeVolLabel(value);
        }

        /// <inheritdoc />
        public override Directory RootDirectory => _rootDirectory;

        /// <inheritdoc />
        public override long AvailableFreeSpace => throw new NotImplementedException();

        /// <inheritdoc />
        public override long TotalFreeSpace => throw new NotImplementedException();

        /// <inheritdoc />
        public override long TotalSize => throw new NotImplementedException();

        //TODO: Should the detection code be moved elsewhere, e.g. to the container or main form?
        public Fat12(Stream stream, in IBiosParameterBlock? bpb, in BootSector? bs = null) : base(stream)
        {
            _bpb = bpb;
            _bs = bs ?? new BootSector();
            _rootDirectory = new FatRootDirectory(this);
        }

        public Stream GetStream()
        {
            return _stream;
        }

        //Formats a volume with FAT12 file system - currently assumes it's a floppy disk...
        public static Fat12 Create<T>(Stream? stream, in T? bpb, in BootSector bs)
            where T : struct, IBiosParameterBlock
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (!bpb.HasValue)
            {
                throw new ArgumentNullException(nameof(bpb));
            }

            var bpbObj = bpb.Value;

            var fat = new Fat12(stream, bpbObj, bs);

            uint totalSize = (uint)stream.Length;
            uint rootDirSize = (uint)(bpbObj.RootDirectoryEntries << 5);
            uint fatSize = (uint)(bpbObj.LogicalSectorsPerFAT * bpbObj.BytesPerLogicalSector);
            uint fat1Offset = (uint)(bpbObj.ReservedLogicalSectors * bpbObj.BytesPerLogicalSector);
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
                stream.WriteStruct(bs);
                stream.WriteStruct(bpbObj);

                //Boot signature
                stream.Seek(0x1FE, SeekOrigin.Begin);
                writer.Write((byte)0x55);
                writer.Write((byte)0xAA);

                /* Media descriptor needs to be written to each FAT as well, upper 4 bits must all be set.
                 * It takes up the first cluster entry (0), and the second entry (1) is also reserved */
                stream.Seek(fat1Offset, SeekOrigin.Begin);
                writer.Write(bpbObj.MediaDescriptor);
                writer.Write((byte)0xFF);
                writer.Write((byte)0xFF);
                stream.Seek(fat2Offset, SeekOrigin.Begin);
                writer.Write(bpbObj.MediaDescriptor);
                writer.Write((byte)0xFF);
                writer.Write((byte)0xFF);

                //Volume label needs to be written to the root directory as well
                stream.Seek(fat2Offset + fatSize, SeekOrigin.Begin);

                //First 11 bytes (8.3 space-padded filename without the period) are the label itself
                {
                    if (bpbObj is BiosParameterBlock40 bpb40 && !string.IsNullOrEmpty(bpb40.VolumeLabel))
                    {
                        writer.Write(bpb40.VolumeLabel.PadRight(11, ' ').ToCharArray());
                        writer.Write((byte)0x08); //Volume label attribute
                    }
                }
            }

            fat._rootDirectory = new FatRootDirectory(fat);
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
                        name = Encoding.ASCII.GetString(reader.ReadBytes(8)).TrimEnd(' ').ToUpper() + "." +
                               Encoding.ASCII.GetString(reader.ReadBytes(3)).TrimEnd(' ').ToUpper(),
                        attr = reader.ReadByte(),
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

                    if (entry.attr == 0x08)
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
                if (_bpb is BiosParameterBlock40 && _bpb.BpbVersion == BiosParameterBlockVersion.Dos40)
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

        //Returns the current volume label in the root directory, if it exists
        public string GetRDVolLabel()
        {
            uint rootDirOffset = (uint)(_bpb.BytesPerLogicalSector * _bpb.ReservedLogicalSectors + (_bpb.BytesPerLogicalSector * _bpb.LogicalSectorsPerFAT * 2));

            using (var reader = new BinaryReader(_stream, Encoding.ASCII, true))
            {
                _stream.Seek(rootDirOffset, SeekOrigin.Begin);

                for (int i = 0; i < _bpb.RootDirectoryEntries; i++)
                {
                    _stream.Seek(rootDirOffset + i * 0x20, SeekOrigin.Begin);
                    byte firstChar = reader.ReadByte();

                    /* 0x00      = no more entries after this one, stop
                     * 0xE5/0x05 = deleted entry, skip for now */
                    if (firstChar == 0x00) break;
                    else if (firstChar == 0xE5 || firstChar == 0x05) continue;

                    //Root directory is not empty, we need to find the volume label, as it may not be the first entry or it may not exist at all
                    _stream.Seek(-0x01, SeekOrigin.Current);
                    DirectoryEntry entry = new DirectoryEntry
                    {
                        name = Encoding.ASCII.GetString(reader.ReadBytes(8)).TrimEnd(' ').ToUpper() +
                               Encoding.ASCII.GetString(reader.ReadBytes(3)).TrimEnd(' ').ToUpper(),
                        attr = reader.ReadByte(),
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

                    if (entry.attr == 0x08)
                    {
                        return entry.name;
                    }
                }

                return null;
            }
        }

        //Returns the current volume label in the BPB, if BPB is for DOS 4.0+
        public string GetBPBVolLabel()
        {
            if (_bpb is BiosParameterBlock40 && _bpb.BpbVersion == BiosParameterBlockVersion.Dos40)
            {
                return ((BiosParameterBlock40)_bpb).VolumeLabel;
            }

            return null;
        }

        //Returns the number of the next cluster in the chain from either primary or backup FAT
        public uint FatGetNextCluster(uint cluster, bool useBackupFat)
        {
            uint fat1Offset = (uint)(_bpb.BytesPerLogicalSector * _bpb.ReservedLogicalSectors);
            uint fatSize = (uint)_bpb.BytesPerLogicalSector * _bpb.LogicalSectorsPerFAT;
            uint fat2Offset = fat1Offset + fatSize;

            using (var reader = new BinaryReader(_stream, Encoding.ASCII, true))
            {
                if (cluster % 2 == 0)
                {
                    if (useBackupFat)
                        _stream.Seek(fat2Offset + (uint)(cluster * 1.5), SeekOrigin.Begin);
                    else
                        _stream.Seek(fat1Offset + (uint)(cluster * 1.5), SeekOrigin.Begin);

                    ushort lower8 = reader.ReadByte();
                    ushort upper4 = (ushort)((reader.ReadByte() & 0x0F) << 8);

                    return (uint)(upper4 | lower8);
                }
                else
                {
                    if (useBackupFat)
                        _stream.Seek(fat2Offset + (uint)Math.Floor(cluster * 1.5), SeekOrigin.Begin);
                    else
                        _stream.Seek(fat1Offset + (uint)Math.Floor(cluster * 1.5), SeekOrigin.Begin);

                    ushort lower4 = (ushort)(reader.ReadByte() >> 4);
                    ushort upper8 = (ushort)(reader.ReadByte() << 4);

                    return (uint)(upper8 | lower4);
                }
            }
        }

        //Reads the specified cluster in the data area and returns its bytes
        public byte[] ReadCluster(uint cluster)
        {
            if (cluster < 2 || cluster > 0xFEF)
                return null;

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
    }
}