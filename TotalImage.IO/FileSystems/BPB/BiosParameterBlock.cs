using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using TotalImage.DiskGeometries;
using TotalImage.FileSystems.FAT;

namespace TotalImage.FileSystems.BPB
{

    /// <summary>
    /// BIOS Parameter Block for FAT12 and FAT16 file systems versions 2.0-3.31.
    /// </summary>
    /// <remarks>
    /// For versions 3.4, 4.0, or HPFS, use BiosParameterBlock40 instead.
    /// </remarks>
    public class BiosParameterBlock
    {
        /// <summary>
        /// The version of the BIOS Parameter Block
        /// </summary>
        public virtual BiosParameterBlockVersion Version => BiosParameterBlockVersion.Dos20;

        /// <summary>
        /// The three-byte boot jump from the boot sector
        /// </summary>
        public byte[] BootJump { get; private set; }

        /// <summary>
        /// The eight-character OEM ID from the boot sector
        /// </summary>
        public string OemId { get; private set; }

        /// <summary>
        /// The number of bytes per logical sector
        /// </summary>
        public ushort BytesPerLogicalSector { get; private set; }

        /// <summary>
        /// The number of logical sectors per cluster
        /// </summary>
        public byte LogicalSectorsPerCluster { get; private set; }

        /// <summary>
        /// The number of reserved logical sectors
        /// </summary>
        public ushort ReservedLogicalSectors { get; private set; }

        /// <summary>
        /// The number of file allocation tables
        /// </summary>
        public byte NumberOfFATs { get; private set; }

        /// <summary>
        /// The number of root directory entries
        /// </summary>
        public ushort RootDirectoryEntries { get; private set; }

        /// <summary>
        /// The total number of logical sectors
        /// </summary>
        public uint TotalLogicalSectors { get; private set; }

        /// <summary>
        /// The media descriptor
        /// </summary>
        public byte MediaDescriptor { get; private set; }

        /// <summary>
        /// The size of the file allocation table in logical sectors
        /// </summary>
        public uint LogicalSectorsPerFAT { get; protected set; }

        /// <summary>
        /// The number of physical sectors per track
        /// </summary>
        public ushort PhysicalSectorsPerTrack { get; private set; }

        /// <summary>
        /// The number of heads
        /// </summary>
        public ushort NumberOfHeads { get; private set; }

        /// <summary>
        /// The number of hidden sectors
        /// </summary>
        public uint HiddenSectors { get; private set; }

        /// <summary>
        /// Create a new, empty BIOS Parameter Block
        /// </summary>
        protected BiosParameterBlock()
        {
            BootJump = new byte[] { 0xEB, 0x58, 0x90 };
            OemId = "";
        }

        /// <summary>
        /// Create a new BIOS Parameter Block using data from another BIOS Parameter Block
        /// </summary>
        /// <remarks>Typically used to move between BPB types</remarks>
        /// <param name="bpb">The BPB to use data from</param>
        protected BiosParameterBlock(BiosParameterBlock bpb)
        {
            BootJump = bpb.BootJump;
            OemId = bpb.OemId;
            BytesPerLogicalSector = bpb.BytesPerLogicalSector;
            LogicalSectorsPerCluster = bpb.LogicalSectorsPerCluster;
            ReservedLogicalSectors = bpb.ReservedLogicalSectors;
            NumberOfFATs = bpb.NumberOfFATs;
            RootDirectoryEntries = bpb.RootDirectoryEntries;
            TotalLogicalSectors = bpb.TotalLogicalSectors;
            MediaDescriptor = bpb.MediaDescriptor;
            LogicalSectorsPerFAT = bpb.LogicalSectorsPerFAT;
            PhysicalSectorsPerTrack = bpb.PhysicalSectorsPerTrack;
            NumberOfHeads = bpb.NumberOfHeads;
            HiddenSectors = bpb.HiddenSectors;
        }

        /// <summary>
        /// Checks whether the BIOS Parameter Block is valid
        /// </summary>
        /// <returns>True if all data within the BPB is valid, otherwise false</returns>
        public virtual bool Validate()
        {
            // TODO: These are just some very simple checks to see if the BPB is valid, this should probably be improved upon

            return NumberOfHeads != 0
                && PhysicalSectorsPerTrack != 0
                && BytesPerLogicalSector != 0
                && NumberOfFATs != 0
                && TotalLogicalSectors != 0
                && ReservedLogicalSectors != 0
                && LogicalSectorsPerCluster != 0;
                //&& LogicalSectorsPerFAT != 0 <-- This is a valid value for FAT32 volumes at this point.
                //&& RootDirectoryEntries != 0; <-- This is a valid value for FAT32 volumes at this point.
        }

        /// <summary>
        /// Parse a BIOS Parameter Block from a stream
        /// </summary>
        /// <param name="reader">A binary reader around the stream</param>
        /// <param name="offset">The offset in the stream of the BPB</param>
        /// <returns>A BIOS Parameter block</returns>
        public static BiosParameterBlock Parse(BinaryReader reader, long offset)
        {
            reader.BaseStream.Seek(offset, SeekOrigin.Begin); //BPB offset

            var bpb = new BiosParameterBlock
            {
                BytesPerLogicalSector = reader.ReadUInt16(),
                LogicalSectorsPerCluster = reader.ReadByte(),
                ReservedLogicalSectors = reader.ReadUInt16(),
                NumberOfFATs = reader.ReadByte(),
                RootDirectoryEntries = reader.ReadUInt16(),
                TotalLogicalSectors = reader.ReadUInt16(),
                MediaDescriptor = reader.ReadByte(),
                LogicalSectorsPerFAT = reader.ReadUInt16()
            };

            //Parsing a standard BPB
            if (offset == 0x0B)
            {
                bpb.PhysicalSectorsPerTrack = reader.ReadUInt16();
                bpb.NumberOfHeads = reader.ReadUInt16();

                if (bpb.TotalLogicalSectors != 0)
                {
                    bpb.HiddenSectors = reader.ReadUInt16();
                    reader.BaseStream.Seek(6, SeekOrigin.Current);
                }
                else
                {
                    bpb.HiddenSectors = reader.ReadUInt32();
                    bpb.TotalLogicalSectors = reader.ReadUInt32();
                }
            }

            //Parsing an Apricot BPB, which doesn't have the number of heads and SPT, so we have to make some manual adjustments
            else if (offset == 0x50)
            {
                if (bpb.MediaDescriptor == 0xFC) //315k
                {
                    bpb.NumberOfHeads = 1;
                    bpb.PhysicalSectorsPerTrack = 70;
                }
                else if (bpb.MediaDescriptor == 0xFE) //720k
                {
                    bpb.NumberOfHeads = 2;
                    bpb.PhysicalSectorsPerTrack = 80;
                }
            }

            if (!bpb.Validate())
            {
                throw new InvalidDataException("At least one of BPB parameters is 0");
            }

            uint tracks = (uint)(bpb.TotalLogicalSectors / bpb.NumberOfHeads / bpb.PhysicalSectorsPerTrack);

            // TODO: This probably needs reviewing to better check BPB params match the correct size
            if (tracks == 0)
            {
                throw new InvalidDataException("BPB paramaters don't match image size");
            }

            // Parsing a standard BPB, read boot sector
            if (offset == 0x0B)
            {
                long endOffset = reader.BaseStream.Position;

                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                bpb.BootJump = reader.ReadBytes(3);
                bpb.OemId = new string(reader.ReadChars(8)).TrimEnd(' ').ToUpper();

                reader.BaseStream.Seek(endOffset, SeekOrigin.Begin);
            }

            if (bpb.LogicalSectorsPerFAT == 0)
            {
                // This could be a FAT32 BPB.
                return Fat32BiosParameterBlock.ContinueParsing(bpb, reader) ?? throw new InvalidDataException("FAT size is 0");
            }
            else
            {
                //So far, the BPB seems to be OK, so try to read it further as a DOS 4.0 BPB.
                return ExtendedBiosParameterBlock.ContinueParsing(bpb, reader) ?? bpb;
            }
        }

        /// <summary>
        /// Create a BIOS Parameter Block from a floppy geometry object
        /// </summary>
        /// <param name="geometry">The floppy geometry</param>
        /// <param name="version">The version of the BPB to create</param>
        /// <param name="oemId">The OEM ID to include in the boot sector</param>
        /// <returns>A BIOS Parameter Block suitable for the floppy geometry provided</returns>
        public static BiosParameterBlock FromGeometry(FloppyGeometry geometry, BiosParameterBlockVersion version, string oemId)
            => new BiosParameterBlock
            {
                OemId = Helper.UseAsLabel(oemId),
                BytesPerLogicalSector = (ushort)(128 << geometry.BPS),
                HiddenSectors = 0,
                LogicalSectorsPerCluster = geometry.SPC,
                LogicalSectorsPerFAT = geometry.SPF,
                MediaDescriptor = geometry.MediaDescriptor,
                NumberOfFATs = geometry.NoOfFATs,
                NumberOfHeads = geometry.Sides,
                PhysicalSectorsPerTrack = geometry.SPT,
                ReservedLogicalSectors = geometry.ReservedSectors,
                RootDirectoryEntries = geometry.RootDirectoryEntries,
                TotalLogicalSectors = (uint)(geometry.Tracks * geometry.SPT * geometry.Sides)
            };


        #region Static Fields
        /// <summary>
        /// Get default BPB for a given size. This is typically used for DOS 1.0 images that don't include a BPB.
        /// </summary>
        public static readonly IReadOnlyDictionary<long, BiosParameterBlock> DefaultParametersForSize;

        /// <summary>
        /// Get the default BPB for an Acorn 800k disk that doesn't include its own BPB.
        /// </summary>
        public static readonly BiosParameterBlock DefaultAcornParameters = new BiosParameterBlock
        {
            BytesPerLogicalSector = 1024,
            LogicalSectorsPerCluster = 1,
            ReservedLogicalSectors = 0,
            NumberOfFATs = 1,
            LogicalSectorsPerFAT = 2,
            RootDirectoryEntries = 192,
            PhysicalSectorsPerTrack = 5,
            NumberOfHeads = 2,
            TotalLogicalSectors = 800,
            MediaDescriptor = 0xFD,
            HiddenSectors = 0
        };

        static BiosParameterBlock()
        {
            Dictionary<long, BiosParameterBlock> defaultParams = new Dictionary<long, BiosParameterBlock>
            {
                {
                    163840, //5.25" 160 KiB
                    new BiosParameterBlock
                    {
                        BytesPerLogicalSector = 512,
                        LogicalSectorsPerCluster = 1,
                        ReservedLogicalSectors = 1,
                        NumberOfFATs = 2,
                        LogicalSectorsPerFAT = 1,
                        RootDirectoryEntries = 64,
                        PhysicalSectorsPerTrack = 8,
                        NumberOfHeads = 1,
                        TotalLogicalSectors = 320,
                        MediaDescriptor = 0xFE,
                        HiddenSectors = 0
                    }
                },
                {
                    184320, //5.25" 180 KiB
                    new BiosParameterBlock
                    {
                        BytesPerLogicalSector = 512,
                        LogicalSectorsPerCluster = 1,
                        ReservedLogicalSectors = 1,
                        NumberOfFATs = 2,
                        LogicalSectorsPerFAT = 1,
                        RootDirectoryEntries = 64,
                        PhysicalSectorsPerTrack = 9,
                        NumberOfHeads = 1,
                        TotalLogicalSectors = 360,
                        MediaDescriptor = 0xFC,
                        HiddenSectors = 0
                    }
                },
                {
                    327680, //5.25" 320 KiB
                    new BiosParameterBlock
                    {
                        BytesPerLogicalSector = 512,
                        LogicalSectorsPerCluster = 2,
                        ReservedLogicalSectors = 1,
                        NumberOfFATs = 2,
                        LogicalSectorsPerFAT = 1,
                        RootDirectoryEntries = 112,
                        PhysicalSectorsPerTrack = 8,
                        NumberOfHeads = 2,
                        TotalLogicalSectors = 640,
                        MediaDescriptor = 0xFF,
                        HiddenSectors = 0
                    }
                },
                {
                    368640, //5.25" 360 KiB
                    new BiosParameterBlock
                    {
                        BytesPerLogicalSector = 512,
                        LogicalSectorsPerCluster = 2,
                        ReservedLogicalSectors = 1,
                        NumberOfFATs = 2,
                        LogicalSectorsPerFAT = 2,
                        RootDirectoryEntries = 112,
                        PhysicalSectorsPerTrack = 9,
                        NumberOfHeads = 2,
                        TotalLogicalSectors = 720,
                        MediaDescriptor = 0xFD,
                        HiddenSectors = 0
                    }
                },
                {
                    256256, //8" 250 KiB
                    new BiosParameterBlock
                    {
                        BytesPerLogicalSector = 128,
                        LogicalSectorsPerCluster = 4,
                        ReservedLogicalSectors = 1,
                        NumberOfFATs = 2,
                        LogicalSectorsPerFAT = 6,
                        RootDirectoryEntries = 68,
                        PhysicalSectorsPerTrack = 26,
                        NumberOfHeads = 1,
                        TotalLogicalSectors = 2002,
                        MediaDescriptor = 0xFE,
                        HiddenSectors = 0
                    }
                },
                {
                    1261568, //8" 1232 KiB
                    new BiosParameterBlock
                    {
                        BytesPerLogicalSector = 1024,
                        LogicalSectorsPerCluster = 1,
                        ReservedLogicalSectors = 1,
                        NumberOfFATs = 2,
                        LogicalSectorsPerFAT = 2,
                        RootDirectoryEntries = 192,
                        PhysicalSectorsPerTrack = 8,
                        NumberOfHeads = 2,
                        TotalLogicalSectors = 1232,
                        MediaDescriptor = 0xFE,
                        HiddenSectors = 0
                    }
                }
            };

            DefaultParametersForSize = defaultParams.ToImmutableDictionary();
        }
        #endregion
    }
}
