using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;

namespace TotalImage.Containers.TAP;

/// <summary>
/// Exposes SIMH tape-record payloads as a seekable stream without copying their contents.
/// </summary>
internal sealed class TapStream : ReadOnlyExtentStream
{
    /// <summary>
    /// Indexes the physical tape records and creates a non-owning view of their concatenated payloads.
    /// </summary>
    /// <param name="source">The readable, seekable stream containing the framed SIMH tape image.</param>
    /// <exception cref="ArgumentException">The source is not readable and seekable.</exception>
    /// <exception cref="InvalidDataException">The framing is malformed or contains an erroneous record.</exception>
    /// <exception cref="NotSupportedException">The image contains an unsupported marker, such as an erasure gap.</exception>
    internal TapStream(Stream source) : base(source, ReadRecords(source))
    {
    }

    /// <summary>
    /// Validates tape-record markers and returns payload ranges, omitting tape marks and odd-byte padding.
    /// </summary>
    /// <param name="source">The framed tape stream to index.</param>
    /// <returns>The physical payload ranges in tape order.</returns>
    private static IReadOnlyList<StreamExtent> ReadRecords(Stream source)
    {
        if (!source.CanRead || !source.CanSeek)
        {
            throw new ArgumentException("A readable, seekable tape image is required.", nameof(source));
        }
        var records = new List<StreamExtent>();
        Span<byte> word = stackalloc byte[4];
        long offset = 0;
        while (offset < source.Length)
        {
            if (source.Length - offset < 4)
            {
                throw new InvalidDataException("Truncated TAP record marker.");
            }
            source.Position = offset;
            source.ReadExactly(word);
            uint marker = BinaryPrimitives.ReadUInt32LittleEndian(word);
            offset += 4;
            if (marker == 0)
            {
                continue; // Tape mark: no trailing length word.
            }
            if (marker == uint.MaxValue)
            {
                if (offset != source.Length)
                {
                    throw new InvalidDataException("Data follows the TAP end-of-medium marker.");
                }
                break;
            }
            if ((marker & 0x7f000000) != 0)
            {
                throw new NotSupportedException("This TAP record marker is not supported (erasure gaps are not supported).");
            }
            if ((marker & 0x80000000) != 0)
            {
                throw new InvalidDataException("The TAP image contains a record marked as erroneous.");
            }
            long length = marker;
            long paddedLength = length + (length & 1);
            if (source.Length - offset < paddedLength + 4)
            {
                throw new InvalidDataException("Truncated TAP data record.");
            }
            source.Position = offset + paddedLength;
            source.ReadExactly(word);
            if (BinaryPrimitives.ReadUInt32LittleEndian(word) != marker)
            {
                throw new InvalidDataException("The leading and trailing TAP record lengths do not match.");
            }
            records.Add(new StreamExtent(offset, length));
            offset += paddedLength + 4;
        }
        if (records.Count == 0)
        {
            throw new InvalidDataException("The TAP image contains no data records.");
        }
        return records;
    }
}
