using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text;

namespace TotalImage.FileSystems.FAT;

/// <summary>
/// This class represents the BIOS Parameter Block, a structure used by
/// FAT file systems to store basic information about the partition.
/// </summary>
public class BiosParameterBlock
{
    #region Boot Record
    private byte[] jmpBoot = new byte[3];
    private byte[] oemName = new byte[8];
    #endregion

    #region Original BIOS Parameter Block fields (DOS 2.0+)
    private ushort bytsPerSec;
    private byte secPerClus;
    private ushort rsvdSecCnt;
    private byte numFATs;
    private ushort rootEntCnt;
    private ushort totSec16;
    private byte media;
    private ushort fatSz16;
    private ushort secPerTrk;
    private ushort numHeads;
    private uint hiddSec;
    private uint totSec32;
    #endregion

    #region Extended BIOS Parameter Block fields (DOS 4.0+)
    private byte drvNum;
    private byte reserved1;
    private ExtendedBootSignature bootSig;
    private uint volId;
    private byte[] volLab = new byte[11];
    private byte[] filSysType = new byte[8];
    #endregion

    #region FAT32 BIOS Parameter Block fields
    private uint fatSz32;
    private ushort extFlags;
    private ushort fsVer;
    private uint rootClus;
    private ushort fsInfo;
    private ushort bkBootSec;
    private byte[] reserved = new byte[12];
    #endregion

    /// <summary>
    /// <c>true</c> if this BIOS Parameter Block is Apricot-style.
    /// </summary>
    public bool IsApricot { get; private set; }

    /// <summary>
    /// <c>true</c> if this BIOS Parameter Block includes the volume serial
    /// number field.
    /// </summary>
    public bool HasDos34Fields =>
        bootSig is FAT.ExtendedBootSignature.Dos34 or FAT.ExtendedBootSignature.Dos40;

    /// <summary>
    /// <c>true</c> if this BIOS Parameter Block includes the volume label and
    /// filesystem type fields.
    /// </summary>
    public bool HasDos40Fields =>
        bootSig == FAT.ExtendedBootSignature.Dos40;

    /// <summary>
    /// <c>true</c> if this BIOS Parameter Block includes FAT32-specific fields.
    /// </summary>
    public bool HasFat32Fields => fatSz16 == 0;

    /// <summary>
    /// The version of the BIOS Parameter Block
    /// </summary>
    public BiosParameterBlockVersion Version => this switch
    {
        { HasFat32Fields: true } => BiosParameterBlockVersion.Fat32,
        { HasDos40Fields: true } => BiosParameterBlockVersion.Dos40,
        { HasDos34Fields: true } => BiosParameterBlockVersion.Dos34,
        _ => BiosParameterBlockVersion.Dos20
    };

    /// <summary>
    /// The three-byte jump instruction in the beginning of the boot sector
    /// </summary>
    public byte[] BootJump => jmpBoot;

    /// <summary>
    /// The eight-character OEM ID from the boot sector
    /// </summary>
    public string OemId => Encoding.ASCII.GetString(oemName);

    /// <summary>
    /// The number of bytes per logical sector
    /// </summary>
    public int BytesPerLogicalSector => bytsPerSec;

    /// <summary>
    /// The number of logical sectors per cluster
    /// </summary>
    public int LogicalSectorsPerCluster => secPerClus;

    /// <summary>
    /// The number of reserved logical sectors
    /// </summary>
    public int ReservedLogicalSectors => rsvdSecCnt;

    /// <summary>
    /// The number of file allocation tables
    /// </summary>
    public int NumberOfFATs => numFATs;

    /// <summary>
    /// The number of root directory entries
    /// </summary>
    public int RootDirectoryEntries => rootEntCnt;

    /// <summary>
    /// The total number of logical sectors
    /// </summary>
    public long TotalLogicalSectors => totSec16 > 0 ? totSec16 : totSec32;

    /// <summary>
    /// The media descriptor
    /// </summary>
    public byte MediaDescriptor => media;

    /// <summary>
    /// The size of the file allocation table in logical sectors
    /// </summary>
    public long LogicalSectorsPerFAT => fatSz16 > 0 ? fatSz16 : fatSz32;

    /// <summary>
    /// The number of physical sectors per track
    /// </summary>
    public int PhysicalSectorsPerTrack => this switch
    {
        { IsApricot: true, MediaDescriptor: 0xFC } => 70, // 315k
        { IsApricot: true, MediaDescriptor: 0xFE } => 80, // 720k
        _ => secPerTrk
    };

    /// <summary>
    /// The number of heads
    /// </summary>
    public int NumberOfHeads => this switch
    {
        { IsApricot: true, MediaDescriptor: 0xFC } => 1, // 315k
        { IsApricot: true, MediaDescriptor: 0xFE } => 2, // 720k
        _ => numHeads
    };

    /// <summary>
    /// The number of hidden sectors.
    /// </summary>
    public long HiddenSectors => hiddSec;

    /// <summary>
    /// Physical drive number per INT 13h, or <c>null</c> if not supported.
    /// </summary>
    public byte? PhysicalDriveNumber => HasDos34Fields ? drvNum : null;

    /// <summary>
    /// Reserved field, or <c>null</c> if not supported.
    /// </summary>
    public byte? Flags => HasDos34Fields ? reserved1 : null;

    /// <summary>
    /// Extended boot signature, or <c>null</c> if not supported. Determines
    /// whether the EBPB includes volume label and filesystem type fields.
    /// </summary>
    public ExtendedBootSignature? ExtendedBootSignature => HasDos34Fields ? bootSig : null;

    /// <summary>
    /// Volume serial number, or <c>null</c> if not supported.
    /// </summary>
    public uint? VolumeSerialNumber => HasDos34Fields ? volId : null;

    /// <summary>
    /// Partition volume label, or <c>null</c> if not supported.
    /// </summary>
    public string? VolumeLabel => HasDos40Fields ? Encoding.ASCII.GetString(volLab) : null;

    /// <summary>
    /// Filesystem type for display purposes, or <c>null</c> if not supported.
    /// </summary>
    public string? FileSystemType => HasDos40Fields ? Encoding.ASCII.GetString(filSysType) : null;

    /// <summary>
    /// FAT32 extended flags, or <c>null</c> if not supported.
    /// </summary>
    public ushort? ExtendedFlags => HasFat32Fields ? extFlags : null;

    /// <summary>
    /// FAT32 version number, or <c>null</c> if not supported.
    /// </summary>
    public int? FileSystemVersion => HasFat32Fields ? fsVer : null;

    /// <summary>
    /// Cluster number of root directory start, or <c>null</c> if not supported.
    /// </summary>
    public uint? RootDirectoryCluster => HasFat32Fields ? rootClus : null;

    /// <summary>
    /// Logical sector number of the File System Information Sector, or
    /// <c>null</c> if not supported.
    /// </summary>
    public uint? FsInfoSector => HasFat32Fields ? fsInfo : null;

    /// <summary>
    /// First logical sector number of a copy of the FAT32 boot sectors, or
    /// <c>null</c> if not supported.
    /// </summary>
    public int? BackupBootSector => HasFat32Fields ? bkBootSec : null;

    /// <summary>
    /// Parse a BIOS Parameter Block.
    /// </summary>
    /// <param name="sectorBytes">A span containing the first sector of a partition.</param>
    /// <param name="offset">The BIOS Parameter Block offset</param>
    /// <returns>A BIOS Parameter Block object</returns>
    public static BiosParameterBlock Parse(in ReadOnlySpan<byte> sectorBytes, int offset)
    {
        var bpbBytes = sectorBytes[offset..];

        var bpb = new BiosParameterBlock
        {
            bytsPerSec = BinaryPrimitives.ReadUInt16LittleEndian(bpbBytes[0..2]),
            secPerClus = bpbBytes[2],
            rsvdSecCnt = BinaryPrimitives.ReadUInt16LittleEndian(bpbBytes[3..5]),
            numFATs = bpbBytes[5],
            rootEntCnt = BinaryPrimitives.ReadUInt16LittleEndian(bpbBytes[6..8]),
            totSec16 = BinaryPrimitives.ReadUInt16LittleEndian(bpbBytes[8..10]),
            media = bpbBytes[10],
            fatSz16 = BinaryPrimitives.ReadUInt16LittleEndian(bpbBytes[11..13]),
        };

        if (bpb is { bytsPerSec: 0 } or { secPerClus: 0 } or { rsvdSecCnt: 0 } or { numFATs: 0 })
        {
            throw new InvalidDataException("At least one of the required BPB parameters is zero.");
        }

        if (offset == 0x50)
        {
            if (bpb.MediaDescriptor is 0xFC or 0xFE)
            {
                bpb.IsApricot = true;
                return bpb;
            }
            else
            {
                throw new InvalidDataException("Unrecognized Media Descriptor value for an Apricot-style FAT.");
            }
        }

        bpb.secPerTrk = BinaryPrimitives.ReadUInt16LittleEndian(sectorBytes[24..26]);
        bpb.numHeads = BinaryPrimitives.ReadUInt16LittleEndian(sectorBytes[26..28]);

        if (bpb is { secPerTrk: 0 } or { numHeads: 0 })
        {
            throw new InvalidDataException("At least one of the required BPB parameters is zero.");
        }

        bpb.jmpBoot = sectorBytes[0..3].ToArray();
        bpb.oemName = sectorBytes[3..11].ToArray();

        if (bpb.totSec16 > 0)
        {
            bpb.hiddSec = BinaryPrimitives.ReadUInt16LittleEndian(sectorBytes[28..30]);
        }
        else
        {
            bpb.hiddSec = BinaryPrimitives.ReadUInt32LittleEndian(sectorBytes[28..32]);
            bpb.totSec32 = BinaryPrimitives.ReadUInt32LittleEndian(sectorBytes[32..36]);
        }

        if (bpb.TotalLogicalSectors / bpb.NumberOfHeads / bpb.PhysicalSectorsPerTrack == 0)
        {
            throw new InvalidDataException("Claimed BPB parameters don't match actual image size.");
        }

        if (bpb.TotalLogicalSectors == 0)
        {
            throw new InvalidDataException("Claimed total number of logical sectors is zero.");
        }

        void ReadEbpb(ReadOnlySpan<byte> ebpbBytes)
        {
            bpb.drvNum = ebpbBytes[0];
            bpb.reserved1 = ebpbBytes[1];
            bpb.bootSig = (ExtendedBootSignature)ebpbBytes[2];
            bpb.volId = BinaryPrimitives.ReadUInt32LittleEndian(ebpbBytes[3..7]);

            if (bpb.bootSig == FAT.ExtendedBootSignature.Dos40)
            {
                bpb.volLab = ebpbBytes[7..18].ToArray();
                bpb.filSysType = ebpbBytes[18..26].ToArray();
            }
        }

        if (bpb.fatSz16 > 0)
        {
            ReadEbpb(sectorBytes[36..]);

            if (bpb.rootEntCnt == 0)
            {
                throw new InvalidDataException("Claimed root directory entry count is zero.");
            }
        }
        else
        {
            bpb.fatSz32 = BinaryPrimitives.ReadUInt32LittleEndian(sectorBytes[36..40]);
            bpb.extFlags = BinaryPrimitives.ReadUInt16LittleEndian(sectorBytes[40..42]);
            bpb.fsVer = BinaryPrimitives.ReadUInt16LittleEndian(sectorBytes[42..44]);
            bpb.rootClus = BinaryPrimitives.ReadUInt32LittleEndian(sectorBytes[44..48]);
            bpb.fsInfo = BinaryPrimitives.ReadUInt16LittleEndian(sectorBytes[48..50]);
            bpb.bkBootSec = BinaryPrimitives.ReadUInt16LittleEndian(sectorBytes[50..52]);
            bpb.reserved = sectorBytes[54..64].ToArray();

            ReadEbpb(sectorBytes[64..]);
        }

        if (bpb.LogicalSectorsPerFAT == 0)
        {
            throw new InvalidDataException("Claimed size of the File Allocation Table is zero.");
        }

        return bpb;
    }

    /// <summary>
    /// Default parameters for a given size. This is typically used for DOS 1.0 images that don't include a BPB.
    /// </summary>
    public static IReadOnlyDictionary<long, BiosParameterBlock> DefaultParametersForSize => new Dictionary<long, BiosParameterBlock>
    {
        [163840] = new() //5.25" 160 KiB
        {
            bytsPerSec = 512,
            secPerClus = 1,
            rsvdSecCnt = 1,
            numFATs = 2,
            rootEntCnt = 64,
            totSec16 = 320,
            media = 0xFE,
            fatSz16 = 1,
            secPerTrk = 8,
            numHeads = 1,
            hiddSec = 0
        },
        [184320] = new() //5.25" 180 KiB
        {
            bytsPerSec = 512,
            secPerClus = 1,
            rsvdSecCnt = 1,
            numFATs = 2,
            rootEntCnt = 64,
            totSec16 = 360,
            media = 0xFC,
            fatSz16 = 1,
            secPerTrk = 9,
            numHeads = 1,
            hiddSec = 0,
        },
        [327680] = new() //5.25" 320 KiB
        {
            bytsPerSec = 512,
            secPerClus = 2,
            rsvdSecCnt = 1,
            numFATs = 2,
            rootEntCnt = 112,
            totSec16 = 640,
            media = 0xFF,
            fatSz16 = 1,
            secPerTrk = 8,
            numHeads = 2,
            hiddSec = 0
        },
        [368640] = new() //5.25" 360 KiB
        {
            bytsPerSec = 512,
            secPerClus = 2,
            rsvdSecCnt = 1,
            numFATs = 2,
            rootEntCnt = 112,
            totSec16 = 720,
            media = 0xFD,
            fatSz16 = 2,
            secPerTrk = 9,
            numHeads = 2,
            hiddSec = 0
        },
        [256256] = new() //8" 250 KiB
        {
            bytsPerSec = 128,
            secPerClus = 4,
            rsvdSecCnt = 1,
            numFATs = 2,
            rootEntCnt = 68,
            totSec16 = 2002,
            media = 0xFE,
            fatSz16 = 6,
            secPerTrk = 26,
            numHeads = 1,
            hiddSec = 0
        },
        [1261568] = new() //8" 1232 KiB
        {
            bytsPerSec = 1024,
            secPerClus = 1,
            rsvdSecCnt = 1,
            numFATs = 2,
            rootEntCnt = 192,
            totSec16 = 1232,
            media = 0xFE,
            fatSz16 = 2,
            secPerTrk = 8,
            numHeads = 2,
            hiddSec = 0
        }
    }.ToImmutableDictionary();

    /// <summary>
    /// Default parameters for an Acorn 800k disk that doesn't include its own BPB.
    /// </summary>
    public static BiosParameterBlock DefaultAcornParameters => new()
    {
        bytsPerSec = 1024,
        secPerClus = 1,
        rsvdSecCnt = 0,
        numFATs = 1,
        rootEntCnt = 2,
        totSec16 = 192,
        media = 5,
        fatSz16 = 2,
        secPerTrk = 800,
        numHeads = 0xFD,
        hiddSec = 0
    };
}
