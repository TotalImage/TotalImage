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
    private byte[] jump = new byte[3];
    private byte[] oem = new byte[8];
    #endregion

    #region Original BIOS Parameter Block fields (DOS 2.0+)
    private ushort bytesPerSector;
    private byte sectorsPerCluster;
    private ushort reservedSectors;
    private byte fats;
    private ushort rootEntries;
    private ushort sectors;
    private byte media;
    private ushort sectorsPerFat;
    private ushort sectorsPerTrack;
    private ushort heads;
    private uint hiddenSectors;
    private uint largeSectors;
    #endregion

    #region Extended BIOS Parameter Block fields (DOS 4.0+)
    private byte physicalDriveNumber;
    private byte reserved1;
    private ExtendedBootSignature signature;
    private uint id;
    private byte[] volumeLabel = new byte[11];
    private byte[] systemId = new byte[8];
    #endregion

    #region FAT32 BIOS Parameter Block fields
    private uint largeSectorsPerFat;
    private ushort extendedFlags;
    private ushort fsVersion;
    private uint rootDirFirstCluster;
    private ushort fsInfoSector;
    private ushort backupBootSector;
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
        signature is FAT.ExtendedBootSignature.Dos34 or FAT.ExtendedBootSignature.Dos40;

    /// <summary>
    /// <c>true</c> if this BIOS Parameter Block includes the volume label and
    /// filesystem type fields.
    /// </summary>
    public bool HasDos40Fields =>
        signature == FAT.ExtendedBootSignature.Dos40;

    /// <summary>
    /// <c>true</c> if this BIOS Parameter Block includes FAT32-specific fields.
    /// </summary>
    public bool HasFat32Fields => sectorsPerFat == 0;

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
    public byte[] BootJump => jump;

    /// <summary>
    /// The eight-character OEM ID from the boot sector
    /// </summary>
    public string OemId => Encoding.ASCII.GetString(oem);

    /// <summary>
    /// The number of bytes per logical sector
    /// </summary>
    public int BytesPerLogicalSector => bytesPerSector;

    /// <summary>
    /// The number of logical sectors per cluster
    /// </summary>
    public int LogicalSectorsPerCluster => sectorsPerCluster;

    /// <summary>
    /// The number of reserved logical sectors
    /// </summary>
    public int ReservedLogicalSectors => reservedSectors;

    /// <summary>
    /// The number of file allocation tables
    /// </summary>
    public int NumberOfFATs => fats;

    /// <summary>
    /// The number of root directory entries
    /// </summary>
    public int RootDirectoryEntries => rootEntries;

    /// <summary>
    /// The total number of logical sectors
    /// </summary>
    public long TotalLogicalSectors => sectors > 0 ? sectors : largeSectors;

    /// <summary>
    /// The media descriptor
    /// </summary>
    public byte MediaDescriptor => media;

    /// <summary>
    /// The size of the file allocation table in logical sectors
    /// </summary>
    public long LogicalSectorsPerFAT => sectorsPerFat > 0 ? sectorsPerFat : largeSectorsPerFat;

    /// <summary>
    /// The number of physical sectors per track
    /// </summary>
    public int PhysicalSectorsPerTrack => this switch
    {
        { IsApricot: true, MediaDescriptor: 0xFC } => 70, // 315k
        { IsApricot: true, MediaDescriptor: 0xFE } => 80, // 720k
        _ => sectorsPerTrack
    };

    /// <summary>
    /// The number of heads
    /// </summary>
    public int NumberOfHeads => this switch
    {
        { IsApricot: true, MediaDescriptor: 0xFC } => 1, // 315k
        { IsApricot: true, MediaDescriptor: 0xFE } => 2, // 720k
        _ => heads
    };

    /// <summary>
    /// The number of hidden sectors.
    /// </summary>
    public long HiddenSectors => hiddenSectors;

    /// <summary>
    /// Physical drive number per INT 13h, or <c>null</c> if not supported.
    /// </summary>
    public byte? PhysicalDriveNumber => HasDos34Fields ? physicalDriveNumber : null;

    /// <summary>
    /// Reserved field, or <c>null</c> if not supported.
    /// </summary>
    public byte? Flags => HasDos34Fields ? reserved1 : null;

    /// <summary>
    /// Extended boot signature, or <c>null</c> if not supported. Determines
    /// whether the EBPB includes volume label and filesystem type fields.
    /// </summary>
    public ExtendedBootSignature? ExtendedBootSignature => HasDos34Fields ? signature : null;

    /// <summary>
    /// Volume serial number, or <c>null</c> if not supported.
    /// </summary>
    public uint? VolumeSerialNumber => HasDos34Fields ? id : null;

    /// <summary>
    /// Partition volume label, or <c>null</c> if not supported.
    /// </summary>
    public string? VolumeLabel => HasDos40Fields ? Encoding.ASCII.GetString(volumeLabel) : null;

    /// <summary>
    /// Filesystem type for display purposes, or <c>null</c> if not supported.
    /// </summary>
    public string? FileSystemType => HasDos40Fields ? Encoding.ASCII.GetString(systemId) : null;

    /// <summary>
    /// FAT32 extended flags, or <c>null</c> if not supported.
    /// </summary>
    public ushort? ExtendedFlags => HasFat32Fields ? extendedFlags : null;

    /// <summary>
    /// FAT32 version number, or <c>null</c> if not supported.
    /// </summary>
    public int? FileSystemVersion => HasFat32Fields ? fsVersion : null;

    /// <summary>
    /// Cluster number of root directory start, or <c>null</c> if not supported.
    /// </summary>
    public uint? RootDirectoryCluster => HasFat32Fields ? rootDirFirstCluster : null;

    /// <summary>
    /// Logical sector number of the File System Information Sector, or
    /// <c>null</c> if not supported.
    /// </summary>
    public uint? FsInfoSector => HasFat32Fields ? fsInfoSector : null;

    /// <summary>
    /// First logical sector number of a copy of the FAT32 boot sectors, or
    /// <c>null</c> if not supported.
    /// </summary>
    public int? BackupBootSector => HasFat32Fields ? backupBootSector : null;

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
            bytesPerSector = BinaryPrimitives.ReadUInt16LittleEndian(bpbBytes[0..2]),
            sectorsPerCluster = bpbBytes[2],
            reservedSectors = BinaryPrimitives.ReadUInt16LittleEndian(bpbBytes[3..5]),
            fats = bpbBytes[5],
            rootEntries = BinaryPrimitives.ReadUInt16LittleEndian(bpbBytes[6..8]),
            sectors = BinaryPrimitives.ReadUInt16LittleEndian(bpbBytes[8..10]),
            media = bpbBytes[10],
            sectorsPerFat = BinaryPrimitives.ReadUInt16LittleEndian(bpbBytes[11..13]),
        };

        if (bpb is { bytesPerSector: 0 } or { sectorsPerCluster: 0 } or { reservedSectors: 0 } or { fats: 0 })
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

        bpb.sectorsPerTrack = BinaryPrimitives.ReadUInt16LittleEndian(sectorBytes[24..26]);
        bpb.heads = BinaryPrimitives.ReadUInt16LittleEndian(sectorBytes[26..28]);

        if (bpb is { sectorsPerTrack: 0 } or { heads: 0 })
        {
            throw new InvalidDataException("At least one of the required BPB parameters is zero.");
        }

        bpb.jump = sectorBytes[0..3].ToArray();
        bpb.oem = sectorBytes[3..11].ToArray();

        if (bpb.sectors > 0)
        {
            bpb.hiddenSectors = BinaryPrimitives.ReadUInt16LittleEndian(sectorBytes[28..30]);
        }
        else
        {
            bpb.hiddenSectors = BinaryPrimitives.ReadUInt32LittleEndian(sectorBytes[28..32]);
            bpb.largeSectors = BinaryPrimitives.ReadUInt32LittleEndian(sectorBytes[32..36]);
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
            bpb.physicalDriveNumber = ebpbBytes[0];
            bpb.reserved1 = ebpbBytes[1];
            bpb.signature = (ExtendedBootSignature)ebpbBytes[2];
            bpb.id = BinaryPrimitives.ReadUInt32LittleEndian(ebpbBytes[3..7]);

            if (bpb.signature == FAT.ExtendedBootSignature.Dos40)
            {
                bpb.volumeLabel = ebpbBytes[7..18].ToArray();
                bpb.systemId = ebpbBytes[18..26].ToArray();
            }
        }

        if (bpb.sectorsPerFat > 0)
        {
            ReadEbpb(sectorBytes[36..]);

            if (bpb.rootEntries == 0)
            {
                throw new InvalidDataException("Claimed root directory entry count is zero.");
            }
        }
        else
        {
            bpb.largeSectorsPerFat = BinaryPrimitives.ReadUInt32LittleEndian(sectorBytes[36..40]);
            bpb.extendedFlags = BinaryPrimitives.ReadUInt16LittleEndian(sectorBytes[40..42]);
            bpb.fsVersion = BinaryPrimitives.ReadUInt16LittleEndian(sectorBytes[42..44]);
            bpb.rootDirFirstCluster = BinaryPrimitives.ReadUInt32LittleEndian(sectorBytes[44..48]);
            bpb.fsInfoSector = BinaryPrimitives.ReadUInt16LittleEndian(sectorBytes[48..50]);
            bpb.backupBootSector = BinaryPrimitives.ReadUInt16LittleEndian(sectorBytes[50..52]);
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
            bytesPerSector = 512,
            sectorsPerCluster = 1,
            reservedSectors = 1,
            fats = 2,
            rootEntries = 64,
            sectors = 320,
            media = 0xFE,
            sectorsPerFat = 1,
            sectorsPerTrack = 8,
            heads = 1,
            hiddenSectors = 0
        },
        [184320] = new() //5.25" 180 KiB
        {
            bytesPerSector = 512,
            sectorsPerCluster = 1,
            reservedSectors = 1,
            fats = 2,
            rootEntries = 64,
            sectors = 360,
            media = 0xFC,
            sectorsPerFat = 1,
            sectorsPerTrack = 9,
            heads = 1,
            hiddenSectors = 0,
        },
        [327680] = new() //5.25" 320 KiB
        {
            bytesPerSector = 512,
            sectorsPerCluster = 2,
            reservedSectors = 1,
            fats = 2,
            rootEntries = 112,
            sectors = 640,
            media = 0xFF,
            sectorsPerFat = 1,
            sectorsPerTrack = 8,
            heads = 2,
            hiddenSectors = 0
        },
        [368640] = new() //5.25" 360 KiB
        {
            bytesPerSector = 512,
            sectorsPerCluster = 2,
            reservedSectors = 1,
            fats = 2,
            rootEntries = 112,
            sectors = 720,
            media = 0xFD,
            sectorsPerFat = 2,
            sectorsPerTrack = 9,
            heads = 2,
            hiddenSectors = 0
        },
        [256256] = new() //8" 250 KiB
        {
            bytesPerSector = 128,
            sectorsPerCluster = 4,
            reservedSectors = 1,
            fats = 2,
            rootEntries = 68,
            sectors = 2002,
            media = 0xFE,
            sectorsPerFat = 6,
            sectorsPerTrack = 26,
            heads = 1,
            hiddenSectors = 0
        },
        [1261568] = new() //8" 1232 KiB
        {
            bytesPerSector = 1024,
            sectorsPerCluster = 1,
            reservedSectors = 1,
            fats = 2,
            rootEntries = 192,
            sectors = 1232,
            media = 0xFE,
            sectorsPerFat = 2,
            sectorsPerTrack = 8,
            heads = 2,
            hiddenSectors = 0
        }
    }.ToImmutableDictionary();

    /// <summary>
    /// Default parameters for an Acorn 800k disk that doesn't include its own BPB.
    /// </summary>
    public static BiosParameterBlock DefaultAcornParameters => new()
    {
        bytesPerSector = 1024,
        sectorsPerCluster = 1,
        reservedSectors = 0,
        fats = 1,
        rootEntries = 2,
        sectors = 192,
        media = 5,
        sectorsPerFat = 2,
        sectorsPerTrack = 800,
        heads = 0xFD,
        hiddenSectors = 0
    };
}
