using System;
using System.Buffers.Binary;
using System.IO;
using System.Text;

namespace TotalImage.Containers.VHD;

/// <summary>
/// A class representing the dynic disk header structure of the VHD. This structure follows a backup copy of the VHD footer at the start of the file
/// and is only present for dynamic and differencing disks
/// </summary>
public class VhdDynamicHeader
{
    private const string COOKIE_VALUE = "cxsparse";

    /// <summary>
    /// The magic value that marks the start of the dynamic disk header record.
    /// </summary>
    public string Cookie { get; }

    /// <summary>
    /// The offset of the next data record. Currently unused and should be set to 0xFFFFFFFFFFFFFFFF
    /// </summary>
    public ulong DataOffset { get; }

    /// <summary>
    /// The offset of the Block Allocation Table (BAT) structure
    /// </summary>
    public ulong TableOffset { get; }

    /// <summary>
    /// The major version of the dynamic disk header
    /// </summary>
    public ushort HeaderVersionMajor { get; }

    /// <summary>
    /// The minor version of the dynamic disk header
    /// </summary>
    public ushort HeaderVersionMinor { get; }

    /// <summary>
    /// Maximum number of entries present in the BAT. Should equal the number of blocks in the disk
    /// </summary>
    public uint MaxTableEntries { get; }

    /// <summary>
    /// The size of a block in this VHD in bytes. Sectors per block must be a power of two
    /// </summary>
    public uint BlockSize { get; }

    /// <summary>
    /// The checksum of the dynamic disk header fields
    /// </summary>
    public uint Checksum { get; }

    /// <summary>
    /// The GUID of the parent VHD image, used only for differencing disks
    /// </summary>
    public Guid ParentUniqueId { get; }

    /// <summary>
    /// The modification time of the parent VHD image, used only for differencing disks
    /// </summary>
    public DateTimeOffset ParentModificationTime { get; }

    /// <summary>
    /// A UTF-16 string containing the parent VHD image filename, used only for differencing disks
    /// </summary>
    public string ParentUnicodeName { get; }

    /// <summary>
    /// The first parent locator entry
    /// </summary>
    public VhdPlatformLocatorEntry ParentLocatorEntry1 { get; }

    /// <summary>
    /// The second parent locator entry
    /// </summary>
    public VhdPlatformLocatorEntry ParentLocatorEntry2 { get; }

    /// <summary>
    /// The third parent locator entry
    /// </summary>
    public VhdPlatformLocatorEntry ParentLocatorEntry3 { get; }

    /// <summary>
    /// The fourth parent locator entry
    /// </summary>
    public VhdPlatformLocatorEntry ParentLocatorEntry4 { get; }

    /// <summary>
    /// The fifth parent locator entry
    /// </summary>
    public VhdPlatformLocatorEntry ParentLocatorEntry5 { get; }

    /// <summary>
    /// The sixth parent locator entry
    /// </summary>
    public VhdPlatformLocatorEntry ParentLocatorEntry6 { get; }

    /// <summary>
    /// The seventh parent locator entry
    /// </summary>
    public VhdPlatformLocatorEntry ParentLocatorEntry7 { get; }

    /// <summary>
    /// The eighth parent locator entry
    /// </summary>
    public VhdPlatformLocatorEntry ParentLocatorEntry8 { get; }

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
            if (i >= 36 && i < 40)
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
    /// Get the bytes of the VHD dynamic disk header record for writing
    /// </summary>
    public Span<byte> GetByteSpan()
    {
        //Mixing byte array and Span<byte> usage here because binary primitives just wouldn't work directly with the byte array...
        byte[] bytes = new byte[1024];
        var span = new Span<byte>(bytes);

        Array.Copy(Encoding.ASCII.GetBytes(COOKIE_VALUE), 0, bytes, 0, 8);
        BinaryPrimitives.WriteUInt64BigEndian(span[8..16], DataOffset);
        BinaryPrimitives.WriteUInt64BigEndian(span[16..24], TableOffset);
        BinaryPrimitives.WriteUInt16BigEndian(span[24..26], HeaderVersionMajor);
        BinaryPrimitives.WriteUInt16BigEndian(span[26..28], HeaderVersionMinor);
        BinaryPrimitives.WriteUInt32BigEndian(span[28..32], MaxTableEntries);
        BinaryPrimitives.WriteUInt32BigEndian(span[32..36], BlockSize);
        BinaryPrimitives.WriteUInt32BigEndian(span[36..40], Checksum);
        byte[] guid = ParentUniqueId.ToByteArray();
        Array.Reverse(guid, 0, 4);
        Array.Reverse(guid, 4, 2);
        Array.Reverse(guid, 6, 2);
        Array.Copy(guid, 0, bytes, 40, 16);
        BinaryPrimitives.WriteUInt32BigEndian(span[56..60], Convert.ToUInt32((ParentModificationTime - new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds));
        Array.Copy(Encoding.Unicode.GetBytes(ParentUnicodeName), 0, bytes, 64, 512);

        //Now for this bunch of Platform Locator entries...
        BinaryPrimitives.WriteUInt32BigEndian(span[576..580], (uint)ParentLocatorEntry1.PlatformCode);
        BinaryPrimitives.WriteUInt32BigEndian(span[580..584], ParentLocatorEntry1.PlatformDataSpace);
        BinaryPrimitives.WriteUInt32BigEndian(span[584..588], ParentLocatorEntry1.PlatformDataLength);
        BinaryPrimitives.WriteUInt64BigEndian(span[588..596], ParentLocatorEntry1.PlatformDataOffset);

        BinaryPrimitives.WriteUInt32BigEndian(span[596..600], (uint)ParentLocatorEntry2.PlatformCode);
        BinaryPrimitives.WriteUInt32BigEndian(span[600..604], ParentLocatorEntry2.PlatformDataSpace);
        BinaryPrimitives.WriteUInt32BigEndian(span[604..608], ParentLocatorEntry2.PlatformDataLength);
        BinaryPrimitives.WriteUInt64BigEndian(span[608..616], ParentLocatorEntry2.PlatformDataOffset);

        BinaryPrimitives.WriteUInt32BigEndian(span[616..620], (uint)ParentLocatorEntry3.PlatformCode);
        BinaryPrimitives.WriteUInt32BigEndian(span[620..624], ParentLocatorEntry3.PlatformDataSpace);
        BinaryPrimitives.WriteUInt32BigEndian(span[624..628], ParentLocatorEntry3.PlatformDataLength);
        BinaryPrimitives.WriteUInt64BigEndian(span[628..636], ParentLocatorEntry3.PlatformDataOffset);

        BinaryPrimitives.WriteUInt32BigEndian(span[636..640], (uint)ParentLocatorEntry4.PlatformCode);
        BinaryPrimitives.WriteUInt32BigEndian(span[640..644], ParentLocatorEntry4.PlatformDataSpace);
        BinaryPrimitives.WriteUInt32BigEndian(span[644..648], ParentLocatorEntry4.PlatformDataLength);
        BinaryPrimitives.WriteUInt64BigEndian(span[648..656], ParentLocatorEntry4.PlatformDataOffset);

        BinaryPrimitives.WriteUInt32BigEndian(span[656..660], (uint)ParentLocatorEntry5.PlatformCode);
        BinaryPrimitives.WriteUInt32BigEndian(span[660..664], ParentLocatorEntry5.PlatformDataSpace);
        BinaryPrimitives.WriteUInt32BigEndian(span[664..668], ParentLocatorEntry5.PlatformDataLength);
        BinaryPrimitives.WriteUInt64BigEndian(span[668..676], ParentLocatorEntry5.PlatformDataOffset);

        BinaryPrimitives.WriteUInt32BigEndian(span[676..680], (uint)ParentLocatorEntry6.PlatformCode);
        BinaryPrimitives.WriteUInt32BigEndian(span[680..684], ParentLocatorEntry6.PlatformDataSpace);
        BinaryPrimitives.WriteUInt32BigEndian(span[684..688], ParentLocatorEntry6.PlatformDataLength);
        BinaryPrimitives.WriteUInt64BigEndian(span[688..696], ParentLocatorEntry6.PlatformDataOffset);

        BinaryPrimitives.WriteUInt32BigEndian(span[696..700], (uint)ParentLocatorEntry7.PlatformCode);
        BinaryPrimitives.WriteUInt32BigEndian(span[700..704], ParentLocatorEntry7.PlatformDataSpace);
        BinaryPrimitives.WriteUInt32BigEndian(span[704..708], ParentLocatorEntry7.PlatformDataLength);
        BinaryPrimitives.WriteUInt64BigEndian(span[708..716], ParentLocatorEntry7.PlatformDataOffset);

        BinaryPrimitives.WriteUInt32BigEndian(span[716..720], (uint)ParentLocatorEntry8.PlatformCode);
        BinaryPrimitives.WriteUInt32BigEndian(span[720..724], ParentLocatorEntry8.PlatformDataSpace);
        BinaryPrimitives.WriteUInt32BigEndian(span[724..728], ParentLocatorEntry8.PlatformDataLength);
        BinaryPrimitives.WriteUInt64BigEndian(span[728..736], ParentLocatorEntry8.PlatformDataOffset);

        return span;
    }

    /// <summary>
    /// Read a VHD dynamic disk header record from a span of bytes
    /// </summary>
    /// <param name="bytes">The span of bytes containing the dynamic header record</param>
    internal VhdDynamicHeader(ReadOnlySpan<byte> bytes)
    {
        Cookie = Encoding.ASCII.GetString(bytes[0..8]);
        if (!string.Equals(COOKIE_VALUE, Cookie, StringComparison.InvariantCulture))
        {
            throw new InvalidDataException("Could not find a valid VHD dynamic disk header");
        }

        DataOffset = BinaryPrimitives.ReadUInt64BigEndian(bytes[8..16]);
        if (DataOffset != ulong.MaxValue)
        {
            throw new NotImplementedException("Found invalid data while parsing the VHD dynamic disk header");
        }

        TableOffset = BinaryPrimitives.ReadUInt64BigEndian(bytes[16..24]);
        HeaderVersionMajor = BinaryPrimitives.ReadUInt16BigEndian(bytes[24..26]);
        HeaderVersionMinor = BinaryPrimitives.ReadUInt16BigEndian(bytes[26..28]);
        MaxTableEntries = BinaryPrimitives.ReadUInt32BigEndian(bytes[28..32]);
        BlockSize = BinaryPrimitives.ReadUInt32BigEndian(bytes[32..36]);
        Checksum = BinaryPrimitives.ReadUInt32BigEndian(bytes[36..40]);

        ParentUniqueId = new Guid(
            BinaryPrimitives.ReadInt32BigEndian(bytes[40..44]),
            BinaryPrimitives.ReadInt16BigEndian(bytes[44..46]),
            BinaryPrimitives.ReadInt16BigEndian(bytes[46..48]),
            bytes[48..56].ToArray());

        ParentModificationTime = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero).AddSeconds(BinaryPrimitives.ReadUInt32BigEndian(bytes[56..60]));
        ParentUnicodeName = Encoding.Unicode.GetString(bytes[64..576]).TrimEnd();

        ParentLocatorEntry1 = new VhdPlatformLocatorEntry(bytes[576..600]);
        ParentLocatorEntry2 = new VhdPlatformLocatorEntry(bytes[600..624]);
        ParentLocatorEntry3 = new VhdPlatformLocatorEntry(bytes[624..648]);
        ParentLocatorEntry4 = new VhdPlatformLocatorEntry(bytes[648..672]);
        ParentLocatorEntry5 = new VhdPlatformLocatorEntry(bytes[672..696]);
        ParentLocatorEntry6 = new VhdPlatformLocatorEntry(bytes[696..720]);
        ParentLocatorEntry7 = new VhdPlatformLocatorEntry(bytes[720..744]);
        ParentLocatorEntry8 = new VhdPlatformLocatorEntry(bytes[744..768]);
    }

    /// <summary>
    /// An entry in the dynamic disk header that stores an absolute byte offset in the file where the parent locator for a differencing hard disk 
    /// is stored
    /// </summary>
    public class VhdPlatformLocatorEntry
    {
        /// <summary>
        /// The platform code for this parent locator
        /// </summary>
        public VhdPlatformCode PlatformCode { get; }

        /// <summary>
        /// The number of 512-byte sectors needed to store the parent hard disk locator
        /// </summary>
        public uint PlatformDataSpace { get; }

        /// <summary>
        /// This field stores the actual length of the parent hard disk locator in bytes
        /// </summary>
        public uint PlatformDataLength { get; }

        /// <summary>
        /// This field stores the absolute file offset in bytes where the platform specific file locator data is stored
        /// </summary>
        public ulong PlatformDataOffset { get; }

        /// <summary>
        /// Read a new VHD Parent locator entry from a span of bytes
        /// </summary>
        public VhdPlatformLocatorEntry(ReadOnlySpan<byte> bytes)
        {
            PlatformCode = (VhdPlatformCode)BinaryPrimitives.ReadUInt32BigEndian(bytes[0..4]);
            PlatformDataSpace = BinaryPrimitives.ReadUInt32BigEndian(bytes[4..8]);
            PlatformDataLength = BinaryPrimitives.ReadUInt32BigEndian(bytes[8..12]);
            PlatformDataOffset = BinaryPrimitives.ReadUInt64BigEndian(bytes[16..24]);
        }

        /// <summary>
        /// Describes which platform-specific format is used for the parent file locator
        /// </summary>
        public enum VhdPlatformCode : uint
        {
            /// <summary>
            /// No platform code
            /// </summary>
            None = 0,

            /// <summary>
            /// Non-unicode relative Windows file path (deprecated)
            /// </summary>
            Wi2r = 0x57693272,

            /// <summary>
            /// Non-unicode absolute Windows file path (deprecated)
            /// </summary>
            Wi2k = 0x5769326B,

            /// <summary>
            /// UTF-16 relative Windows file path
            /// </summary>
            Wi2ru = 0x57327275,

            /// <summary>
            /// UTF-16 absolute Windows file path
            /// </summary>
            Wi2ku = 0x57326B75,

            /// <summary>
            /// macOS alias stored as a blob
            /// </summary>
            Mac = 0x4D616320,

            /// <summary>
            /// A file URL with UTF-8 encoding conforming to RFC 2396
            /// </summary>
            MacX = 0x4D616358
        }
    }
}
