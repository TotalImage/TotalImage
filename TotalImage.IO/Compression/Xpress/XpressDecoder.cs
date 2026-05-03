using System;
using System.Buffers.Binary;
using System.IO;

namespace TotalImage.Compression.Xpress
{
    /// <summary>
    /// Pure managed decoder for Microsoft Xpress compressed blocks.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This implementation targets the publicly documented MS-XCA "Plain LZ77" Xpress variant.
    /// Windows CE IMGFS identifies this codec as <c>XPR</c> and stores each file chunk as an
    /// independently decompressible block.
    /// </para>
    /// <para>
    /// The block format uses 32-bit flag groups, literal bytes, and 16-bit back-reference tokens
    /// with optional extended length encodings. The decoder consumes tokens until the requested
    /// uncompressed size is produced.
    /// </para>
    /// </remarks>
    public static class XpressDecoder
    {
        private const int MatchLengthBits = 3;
        private const int MatchLengthMask = (1 << MatchLengthBits) - 1;
        private const int MatchOffsetShift = MatchLengthBits;
        private const int MaximumMatchOffset = 8192;
        private const int MinimumMatchLength = 3;
        private const int InlineLengthLimit = 7;
        private const int NibbleLengthLimit = 15;
        private const int ByteLengthLimit = 255;

        /// <summary>
        /// Decompresses a single Xpress block.
        /// </summary>
        /// <param name="compressedData">The compressed input block.</param>
        /// <param name="uncompressedSize">Expected decompressed size.</param>
        /// <param name="kind">The Xpress variant to decode.</param>
        /// <returns>The decompressed bytes.</returns>
        /// <exception cref="InvalidDataException">Thrown when the block is malformed.</exception>
        /// <exception cref="NotSupportedException">Thrown when the requested variant is unsupported.</exception>
        public static byte[] Decompress(ReadOnlySpan<byte> compressedData, int uncompressedSize, XpressKind kind = XpressKind.PlainLz77)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(uncompressedSize);

            return kind switch
            {
                XpressKind.PlainLz77 => DecompressPlainLz77(compressedData, uncompressedSize),
                _ => throw new NotSupportedException($"Unsupported Xpress variant: {kind}.")
            };
        }

        /// <summary>
        /// Decompresses a single Xpress block into a caller-provided buffer.
        /// </summary>
        /// <param name="compressedData">The compressed input block.</param>
        /// <param name="destination">Destination buffer for the decompressed bytes.</param>
        /// <param name="kind">The Xpress variant to decode.</param>
        /// <returns>The number of bytes written to <paramref name="destination"/>.</returns>
        public static int Decompress(ReadOnlySpan<byte> compressedData, Span<byte> destination, XpressKind kind = XpressKind.PlainLz77)
        {
            byte[] result = Decompress(compressedData, destination.Length, kind);
            result.CopyTo(destination);
            return result.Length;
        }

        private static byte[] DecompressPlainLz77(ReadOnlySpan<byte> compressedData, int uncompressedSize)
        {
            byte[] output = new byte[uncompressedSize];
            int inputIndex = 0;
            int outputIndex = 0;
            uint flags = 0;
            int flagBitsRemaining = 0;
            byte pendingLengthByte = 0;
            bool useHighLengthNibble = false;

            while (outputIndex < output.Length)
            {
                if (flagBitsRemaining == 0)
                {
                    if (inputIndex + sizeof(uint) > compressedData.Length)
                    {
                        throw new InvalidDataException("Unexpected end of XPRESS input while reading flag word.");
                    }

                    flags = BinaryPrimitives.ReadUInt32LittleEndian(compressedData[inputIndex..(inputIndex + sizeof(uint))]);
                    inputIndex += sizeof(uint);
                    flagBitsRemaining = 32;
                }

                bool isMatch = (flags & 0x80000000u) != 0;
                flags <<= 1;
                flagBitsRemaining--;

                if (!isMatch)
                {
                    if (inputIndex >= compressedData.Length)
                    {
                        throw new InvalidDataException("Unexpected end of XPRESS input while reading literal byte.");
                    }

                    output[outputIndex++] = compressedData[inputIndex++];
                    continue;
                }

                if (inputIndex + sizeof(ushort) > compressedData.Length)
                {
                    throw new InvalidDataException("Unexpected end of XPRESS input while reading match token.");
                }

                ushort token = BinaryPrimitives.ReadUInt16LittleEndian(compressedData[inputIndex..(inputIndex + sizeof(ushort))]);
                inputIndex += sizeof(ushort);

                int matchOffset = (token >> MatchOffsetShift) + 1;
                if (matchOffset <= 0 || matchOffset > MaximumMatchOffset || matchOffset > outputIndex)
                {
                    throw new InvalidDataException($"Invalid XPRESS match offset {matchOffset} at output position {outputIndex}.");
                }

                int encodedLength = token & MatchLengthMask;
                int matchLength = DecodeMatchLength(encodedLength, compressedData, ref inputIndex, ref pendingLengthByte, ref useHighLengthNibble);

                if (matchLength <= 0 || outputIndex + matchLength > output.Length)
                {
                    throw new InvalidDataException("XPRESS match length would overflow the destination buffer.");
                }

                int copySource = outputIndex - matchOffset;
                for (int i = 0; i < matchLength; i++)
                {
                    output[outputIndex++] = output[copySource + i];
                }
            }

            return output;
        }

        private static int DecodeMatchLength(int encodedLength, ReadOnlySpan<byte> compressedData, ref int inputIndex, ref byte pendingLengthByte, ref bool useHighLengthNibble)
        {
            if (encodedLength < InlineLengthLimit)
            {
                return encodedLength + MinimumMatchLength;
            }

            int matchLength = InlineLengthLimit;
            int nibbleLength = ReadPackedNibble(compressedData, ref inputIndex, ref pendingLengthByte, ref useHighLengthNibble);
            matchLength += nibbleLength;
            if (nibbleLength < NibbleLengthLimit)
            {
                return matchLength + MinimumMatchLength;
            }

            int byteLength = ReadByte(compressedData, ref inputIndex);
            matchLength += byteLength;
            if (byteLength < ByteLengthLimit)
            {
                return matchLength + MinimumMatchLength;
            }

            if (inputIndex + sizeof(ushort) > compressedData.Length)
            {
                throw new InvalidDataException("Unexpected end of XPRESS input while reading extended match length.");
            }

            ushort shortLength = BinaryPrimitives.ReadUInt16LittleEndian(compressedData[inputIndex..(inputIndex + sizeof(ushort))]);
            inputIndex += sizeof(ushort);
            if (shortLength != 0)
            {
                return shortLength + MinimumMatchLength;
            }

            if (inputIndex + sizeof(uint) > compressedData.Length)
            {
                throw new InvalidDataException("Unexpected end of XPRESS input while reading very long match length.");
            }

            uint longLength = BinaryPrimitives.ReadUInt32LittleEndian(compressedData[inputIndex..(inputIndex + sizeof(uint))]);
            inputIndex += sizeof(uint);
            if (longLength > int.MaxValue - MinimumMatchLength)
            {
                throw new InvalidDataException("XPRESS match length exceeds supported managed buffer limits.");
            }

            return checked((int)longLength + MinimumMatchLength);
        }

        private static int ReadPackedNibble(ReadOnlySpan<byte> compressedData, ref int inputIndex, ref byte pendingLengthByte, ref bool useHighLengthNibble)
        {
            if (!useHighLengthNibble)
            {
                pendingLengthByte = ReadByte(compressedData, ref inputIndex);
                useHighLengthNibble = true;
                return pendingLengthByte & 0x0F;
            }

            useHighLengthNibble = false;
            return (pendingLengthByte >> 4) & 0x0F;
        }

        private static byte ReadByte(ReadOnlySpan<byte> compressedData, ref int inputIndex)
        {
            if (inputIndex >= compressedData.Length)
            {
                throw new InvalidDataException("Unexpected end of XPRESS input.");
            }

            return compressedData[inputIndex++];
        }
    }
}
