using System;
using System.Buffers.Binary;
using System.IO;
using System.Text;

namespace TotalImage.Containers.VHD;

/// <summary>
/// Represents the features supported by a virtual disk
/// </summary>
[Flags]
public enum VhdFeatures : uint
{
    /// <summary>
    /// No optional features are supported by this virtual disk
    /// </summary>
    ///
    NoFeatures = 0x0,

    /// <summary>
    /// This virtual disk is temporary and may be deleted on shutdown
    /// </summary>
    Temporary = 0x1,

    /// <summary>
    /// This flag should always be set
    /// </summary>
    Reserved = 0x2
}

/// <summary>
/// A class representing the footer structure of the VHD
/// </summary>
public class VhdFooter
{
    private const string COOKIE_VALUE = "conectix";

    /// <summary>
    /// The magic value that marks the start of the footer record.
    /// </summary>
    public string Cookie { get; }

    /// <summary>
    /// Features supported by the VHD
    /// </summary>
    public VhdFeatures Features { get; }

    /// <summary>
    /// The major version of the VHD format contained within the file
    /// </summary>
    public ushort FormatVersionMajor { get; }

    /// <summary>
    /// The minor version of the VHD format contained within the file
    /// </summary>
    public ushort FormatVersionMinor { get; }

    /// <summary>
    /// The offset of the next data record
    /// </summary>
    public ulong DataOffset { get; }

    /// <summary>
    /// The time that the VHD was created
    /// </summary>
    public DateTimeOffset CreationTime { get; }

    /// <summary>
    /// The identifier of the application which created the VHD
    /// </summary>
    public string CreatorApplication { get; }

    /// <summary>
    /// The major version of the application which created the VHD
    /// </summary>
    public ushort CreatorVersionMajor { get; }

    /// <summary>
    /// The minor version of the application which created the VHD
    /// </summary>
    public ushort CreatorVersionMinor { get; }

    /// <summary>
    /// The host operating system that created the VHD
    /// </summary>
    public string CreatorHost { get; }

    /// <summary>
    /// The original size of the disk image, in bytes
    /// </summary>
    public ulong OriginalSize { get; }

    /// <summary>
    /// The current size of the disk image, in bytes
    /// </summary>
    public ulong CurrentSize { get; }

    /// <summary>
    /// The number of cylinders presented by the virtual disk
    /// </summary>
    public ushort DiskCylinders { get; }

    /// <summary>
    /// The number of heads presented by the virtual disk
    /// </summary>
    public byte DiskHeads { get; }

    /// <summary>
    /// The number of sectors per cylinder presented by the virtual disk
    /// </summary>
    public byte DiskSectorsPerCylinder { get; }

    /// <summary>
    /// The type of the VHD contained within the image
    /// </summary>
    public VhdType Type { get; }

    /// <summary>
    /// The checksum of the footer fields
    /// </summary>
    public uint Checksum { get; }

    /// <summary>
    /// The unique ID of the disk
    /// </summary>
    public Guid UniqueId { get; set; }

    /// <summary>
    /// Indicates whether the virtual disk is referenced by a saved virtual machine state
    /// </summary>
    public bool SavedState { get; }

    /// <summary>
    /// Get the bytes of the VHD footer record for writing
    /// </summary>
    public Span<byte> GetByteSpan()
    {
        //Mixing byte array and Span<byte> usage here because binary primitives just wouldn't work directly with the byte array...
        byte[] bytes = new byte[512];
        var span = new Span<byte>(bytes);

        Array.Copy(Encoding.ASCII.GetBytes(COOKIE_VALUE), 0, bytes, 0, 8);
        BinaryPrimitives.WriteUInt32BigEndian(span[8..12], (uint)Features);
        BinaryPrimitives.WriteUInt16BigEndian(span[12..14], FormatVersionMajor);
        BinaryPrimitives.WriteUInt16BigEndian(span[14..16], FormatVersionMinor);
        BinaryPrimitives.WriteUInt64BigEndian(span[16..24], DataOffset);
        BinaryPrimitives.WriteUInt32BigEndian(span[24..28], Convert.ToUInt32((CreationTime - new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds));
        Array.Copy(Encoding.ASCII.GetBytes(CreatorApplication), 0, bytes, 28, 4);
        BinaryPrimitives.WriteUInt16BigEndian(span[32..34], CreatorVersionMajor);
        BinaryPrimitives.WriteUInt16BigEndian(span[34..36], CreatorVersionMinor);
        Array.Copy(Encoding.ASCII.GetBytes(CreatorHost), 0, bytes, 36, 4);
        BinaryPrimitives.WriteUInt64BigEndian(span[40..48], OriginalSize);
        BinaryPrimitives.WriteUInt64BigEndian(span[48..56], CurrentSize);
        BinaryPrimitives.WriteUInt16BigEndian(span[56..58], DiskCylinders);
        span[58] = DiskHeads;
        span[59] = DiskSectorsPerCylinder;
        BinaryPrimitives.WriteUInt32BigEndian(span[60..64], (uint)Type);
        BinaryPrimitives.WriteUInt32BigEndian(span[64..68], Checksum);
        byte[] guid = UniqueId.ToByteArray();
        Array.Reverse(guid, 0, 4);
        Array.Reverse(guid, 4, 2);
        Array.Reverse(guid, 6, 2);
        Array.Copy(guid, 0, bytes, 68, 16);
        span[84] = (byte)(SavedState ? 1 : 0);

        return span;
    }

    /// <summary>
    /// Calculates the VHD footer checksum field from the bytes of a footer record
    /// </summary>
    /// <param name="record">The bytes containing the VHD footer record</param>
    private static uint CalculateChecksum(ReadOnlySpan<byte> record)
    {
        // Taken from Checksum Calculation in VHD Spec, App. A
        uint checksum = 0;
        for (int i = 0; i < record.Length; i++)
        {
            // skip checksum field
            if (i >= 64 && i < 68)
            {
                continue;
            }

            unchecked
            {
                checksum += record[i];
            }
        }

        return ~checksum;
    }

    /// <summary>
    /// Verifies that the stored checksum value in the footer of this VHD matches a freshly calculated value
    /// </summary>
    /// <returns>True when the values match, otherwise false</returns>
    public bool VerifyChecksum()
    {
        return Checksum == CalculateChecksum(GetByteSpan());
    }

    /// <summary>
    /// Returns canonical CHS values for a given disk size according to the VHD specification
    /// </summary>
    /// <param name="size">The size of the disk in bytes</param>
    /// <returns>A tuple of CHS values</returns>
    private static (ushort, byte, byte) GetGeometryFromSize(ulong size)
    {
        // Taken from CHS Calculation in VHD Spec, App. A
        ushort cylinders = 0;
        byte heads = 0;
        byte sectorsPerTrack = 0;
        uint cylinderTimesHeads = 0;

        uint sectors = (uint)Math.Max(size / 512, 65535 * 16 * 255);

        if (sectors >= 65535 * 16 * 63)
        {
            sectorsPerTrack = 255;
            heads = 16;
            cylinderTimesHeads = sectors / sectorsPerTrack;
        }
        else
        {
            sectorsPerTrack = 17;
            cylinderTimesHeads = sectors / sectorsPerTrack;

            heads = (byte)((cylinderTimesHeads + 1023) / 1024);

            if (heads < 4)
            {
                heads = 4;
            }
            if (cylinderTimesHeads >= (heads * 1024) || heads > 16)
            {
                sectorsPerTrack = 31;
                heads = 16;
                cylinderTimesHeads = sectors / sectorsPerTrack;
            }
            if (cylinderTimesHeads >= (heads * 1024))
            {
                sectorsPerTrack = 63;
                heads = 16;
                cylinderTimesHeads = sectors / sectorsPerTrack;
            }
        }

        cylinders = (ushort)(cylinderTimesHeads / heads);

        return (cylinders, heads, sectorsPerTrack);

    }

    /// <summary>
    /// Create an empty new VHD footer for a specified disk size
    /// </summary>
    /// <param name="size">Size of disk in bytes</param>
    public VhdFooter(ulong size)
    {
        Cookie = COOKIE_VALUE;
        Features = VhdFeatures.Reserved;
        FormatVersionMajor = 1;
        FormatVersionMinor = 0;
        DataOffset = ulong.MaxValue;
        CreationTime = DateTimeOffset.UtcNow;
        CreatorApplication = "timg";
        CreatorVersionMajor = 1;
        CreatorVersionMinor = 0;
        CreatorHost = "Wi2k";
        OriginalSize = size;
        CurrentSize = size;
        (DiskCylinders, DiskHeads, DiskSectorsPerCylinder) = GetGeometryFromSize(size);
        Type = VhdType.FixedHardDisk;
        Checksum = 0;
        UniqueId = Guid.NewGuid();
        SavedState = false;
    }

    /// <summary>
    /// Read a VHD footer record from a span of bytes
    /// </summary>
    /// <param name="bytes">The span of bytes containing the footer record</param>
    public VhdFooter(ReadOnlySpan<byte> bytes)
    {
        Cookie = Encoding.ASCII.GetString(bytes[0..8]);
        if (!string.Equals(COOKIE_VALUE, Cookie, StringComparison.InvariantCulture))
        {
            // Is it the older 511 byte header? Skip forward one to test
            //TODO: Actually get a hold of an image with this old header (or pre-MS VHD spec) and see how it differs besides the offset...
            bytes = bytes[1..];
            Cookie = Encoding.ASCII.GetString(bytes[0..8]);
            if (!string.Equals(COOKIE_VALUE, Cookie, StringComparison.InvariantCulture))
            {
                throw new InvalidDataException("Could not find a valid VHD footer");
            }
        }

        Features = (VhdFeatures)BinaryPrimitives.ReadUInt32BigEndian(bytes[8..12]);
        FormatVersionMajor = BinaryPrimitives.ReadUInt16BigEndian(bytes[12..14]);
        FormatVersionMinor = BinaryPrimitives.ReadUInt16BigEndian(bytes[14..16]);
        DataOffset = BinaryPrimitives.ReadUInt64BigEndian(bytes[16..24]);

        CreationTime = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero).AddSeconds(BinaryPrimitives.ReadUInt32BigEndian(bytes[24..28]));
        CreatorApplication = Encoding.ASCII.GetString(bytes[28..32]);
        CreatorVersionMajor = BinaryPrimitives.ReadUInt16BigEndian(bytes[32..34]);
        CreatorVersionMinor = BinaryPrimitives.ReadUInt16BigEndian(bytes[34..36]);
        CreatorHost = Encoding.ASCII.GetString(bytes[36..40]);
        OriginalSize = BinaryPrimitives.ReadUInt64BigEndian(bytes[40..48]);
        CurrentSize = BinaryPrimitives.ReadUInt64BigEndian(bytes[48..56]);
        DiskCylinders = BinaryPrimitives.ReadUInt16BigEndian(bytes[56..58]);
        DiskHeads = bytes[58];
        DiskSectorsPerCylinder = bytes[59];
        Type = (VhdType)BinaryPrimitives.ReadUInt32BigEndian(bytes[60..64]);
        if (Type == VhdType.DifferencingHardDisk)
        {
            throw new NotImplementedException("Differencing disks are not currently supported");
        }

        Checksum = BinaryPrimitives.ReadUInt32BigEndian(bytes[64..68]);
        UniqueId = new Guid(
            BinaryPrimitives.ReadInt32BigEndian(bytes[68..72]),
            BinaryPrimitives.ReadInt16BigEndian(bytes[72..74]),
            BinaryPrimitives.ReadInt16BigEndian(bytes[74..76]),
            bytes[76..84].ToArray());
        SavedState = bytes[84] != 0;
    }


}
