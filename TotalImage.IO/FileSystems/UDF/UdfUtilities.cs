using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Text;

namespace TotalImage.FileSystems.UDF
{
    internal static class UdfUtilities
    {
        internal const int VolumeStructureDescriptorSize = 2048;
        internal const int DescriptorTagSize = 16;
        internal const int FileIdentifierDescriptorHeaderSize = 38;
        internal const int FileEntrySize = 176;
        internal const int ExtendedFileEntrySize = 216;
        internal const uint ExtentLengthMask = 0x3FFFFFFF;
        internal const uint ExtentTypeMask = 0xC0000000;
        internal const uint ExtentRecordedAllocated = 0x00000000;
        internal const uint ExtentNotRecordedAllocated = 0x40000000;
        internal const uint ExtentNotRecordedNotAllocated = 0x80000000;
        internal const uint ExtentNextAllocationDescriptors = 0xC0000000;
        internal const ushort AnchorVolumeDescriptorPointer = 0x0002;
        internal const ushort VolumeDescriptorPointer = 0x0003;
        internal const ushort PrimaryVolumeDescriptor = 0x0001;
        internal const ushort PartitionDescriptor = 0x0005;
        internal const ushort LogicalVolumeDescriptor = 0x0006;
        internal const ushort TerminatingDescriptor = 0x0008;
        internal const ushort FileSetDescriptor = 0x0100;
        internal const ushort FileIdentifierDescriptor = 0x0101;
        internal const ushort AllocationExtentDescriptor = 0x0102;
        internal const ushort FileEntry = 0x0105;
        internal const ushort ExtendedFileEntry = 0x010A;
        internal const ushort IcbAllocationDescriptorShort = 0x0000;
        internal const ushort IcbAllocationDescriptorLong = 0x0001;
        internal const ushort IcbAllocationDescriptorExtended = 0x0002;
        internal const ushort IcbAllocationDescriptorInIcb = 0x0003;
        internal const byte FileCharacteristicHidden = 0x01;
        internal const byte FileCharacteristicDirectory = 0x02;
        internal const byte FileCharacteristicDeleted = 0x04;
        internal const byte FileCharacteristicParent = 0x08;
        internal const byte IcbFileTypeDirectory = 0x04;
        internal const byte IcbFileTypeVat = 0xF8;
        internal const string VirtualPartitionIdentifier = "*UDF Virtual Partition";
        internal const string MetadataPartitionIdentifier = "*UDF Metadata Partition";

        internal static ushort ReadUInt16(ReadOnlySpan<byte> bytes, int offset)
            => BinaryPrimitives.ReadUInt16LittleEndian(bytes.Slice(offset, sizeof(ushort)));

        internal static uint ReadUInt32(ReadOnlySpan<byte> bytes, int offset)
            => BinaryPrimitives.ReadUInt32LittleEndian(bytes.Slice(offset, sizeof(uint)));

        internal static ulong ReadUInt64(ReadOnlySpan<byte> bytes, int offset)
            => BinaryPrimitives.ReadUInt64LittleEndian(bytes.Slice(offset, sizeof(ulong)));

        internal static int Align(int value, int alignment)
            => ((value + alignment - 1) / alignment) * alignment;

        internal static ushort ReadTagIdentifier(ReadOnlySpan<byte> bytes)
            => ReadUInt16(bytes, 0);

        internal static string ReadEntityIdentifier(ReadOnlySpan<byte> bytes, int offset)
        {
            ReadOnlySpan<byte> identifier = bytes.Slice(offset + 1, 23);
            int terminator = identifier.IndexOf((byte)0);
            if (terminator >= 0)
            {
                identifier = identifier[..terminator];
            }

            return Encoding.ASCII.GetString(identifier);
        }

        internal static IEnumerable<uint> GetAnchorCandidateBlocks(long streamLength, int blockSize)
        {
            if (streamLength < blockSize)
            {
                yield break;
            }

            uint lastBlock = (uint)(streamLength / blockSize) - 1;
            uint[] candidates =
            [
                256,
                lastBlock,
                lastBlock >= 256 ? lastBlock - 256 : 0
            ];

            var seen = new HashSet<uint>();
            foreach (uint candidate in candidates)
            {
                if (candidate <= lastBlock && seen.Add(candidate))
                {
                    yield return candidate;
                }
            }
        }

        internal static string DecodeDString(ReadOnlySpan<byte> bytes)
        {
            if (bytes.Length == 0)
            {
                return string.Empty;
            }

            int usedLength = bytes[^1];
            if (usedLength <= 0)
            {
                return string.Empty;
            }

            if (usedLength >= bytes.Length)
            {
                usedLength = bytes.Length - 1;
            }

            return DecodeCompressedUnicode(bytes[..usedLength]);
        }

        internal static string DecodeCompressedUnicode(ReadOnlySpan<byte> bytes)
        {
            if (bytes.Length == 0)
            {
                return string.Empty;
            }

            int compressionId = bytes[0];
            ReadOnlySpan<byte> data = bytes[1..];

            if (compressionId == 8)
            {
                var chars = new char[data.Length];
                for (int i = 0; i < data.Length; i++)
                {
                    chars[i] = (char)data[i];
                }
                return new string(chars);
            }

            if (compressionId == 16)
            {
                if (data.Length % 2 == 1)
                {
                    data = data[..^1];
                }
                return Encoding.BigEndianUnicode.GetString(data);
            }

            return string.Empty;
        }

        internal static DateTime? ReadTimestamp(ReadOnlySpan<byte> bytes, int offset)
        {
            if (bytes.Length < offset + 12)
            {
                return null;
            }

            ushort typeAndTimezone = ReadUInt16(bytes, offset);
            ushort year = ReadUInt16(bytes, offset + 2);
            byte month = bytes[offset + 4];
            byte day = bytes[offset + 5];
            byte hour = bytes[offset + 6];
            byte minute = bytes[offset + 7];
            byte second = bytes[offset + 8];
            byte centiseconds = bytes[offset + 9];
            byte hundredsOfMicroseconds = bytes[offset + 10];
            byte microseconds = bytes[offset + 11];

            if (year == 0 || month == 0 || day == 0)
            {
                return null;
            }

            try
            {
                int timezoneField = typeAndTimezone & 0x0FFF;
                if ((timezoneField & 0x0800) != 0)
                {
                    timezoneField -= 0x1000;
                }

                TimeSpan offsetSpan = (typeAndTimezone & 0xF000) == 0x1000
                    ? TimeSpan.FromMinutes(timezoneField)
                    : TimeSpan.Zero;

                var value = new DateTimeOffset(
                    year,
                    month,
                    day,
                    hour,
                    minute,
                    second,
                    offsetSpan).AddMilliseconds((centiseconds * 10) + hundredsOfMicroseconds + (microseconds / 10.0));

                return value.LocalDateTime;
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }
    }

    internal readonly record struct UdfLongAllocationDescriptor(uint Length, uint LogicalBlockNumber, ushort PartitionReferenceNumber);

    internal readonly record struct UdfShortAllocationDescriptor(uint Length, uint LogicalBlockNumber);

    internal readonly record struct UdfExtendedAllocationDescriptor(uint Length, uint RecordedLength, uint InformationLength, uint LogicalBlockNumber, ushort PartitionReferenceNumber);

    internal readonly record struct UdfContentExtent(uint RawLength, uint LogicalBlockNumber, ushort PartitionReferenceNumber);

    internal readonly record struct UdfPartitionDescriptorInfo(ushort PartitionNumber, uint StartingBlock, uint LengthInBlocks);

    internal enum UdfPartitionMapKind
    {
        Physical,
        Virtual,
        Metadata
    }

    internal sealed class UdfPartitionMapInfo
    {
        public required ushort ReferenceNumber { get; set; }

        public required ushort PartitionNumber { get; set; }

        public required UdfPartitionMapKind Kind { get; set; }

        public uint StartingBlock { get; set; }

        public uint LengthInBlocks { get; set; }

        public ushort ParentReferenceNumber { get; set; }

        public uint MetadataFileLocation { get; set; }

        public uint MetadataMirrorFileLocation { get; set; }

        public UdfFileEntryInfo? MetadataFileEntry { get; set; }

        public UdfFileEntryInfo? MetadataMirrorFileEntry { get; set; }

        public uint[]? VatEntries { get; set; }
    }

    internal readonly record struct UdfFileEntryInfo(
        byte FileType,
        ushort AllocationDescriptorType,
        ushort PartitionReferenceNumber,
        ulong InformationLength,
        uint LengthOfAllocationDescriptors,
        byte[] AllocationDescriptors,
        DateTime? CreationTime,
        DateTime? LastAccessTime,
        DateTime? LastWriteTime);

    internal readonly record struct UdfFileIdentifierDescriptorInfo(
        string Name,
        byte FileCharacteristics,
        UdfLongAllocationDescriptor Icb)
    {
        internal bool IsDirectory => (FileCharacteristics & UdfUtilities.FileCharacteristicDirectory) != 0;

        internal bool IsHidden => (FileCharacteristics & UdfUtilities.FileCharacteristicHidden) != 0;

        internal bool IsDeleted => (FileCharacteristics & UdfUtilities.FileCharacteristicDeleted) != 0;

        internal bool IsParent => (FileCharacteristics & UdfUtilities.FileCharacteristicParent) != 0;
    }
}
