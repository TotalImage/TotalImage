﻿using System;
using System.Buffers.Binary;
using System.IO;

namespace TotalImage.FileSystems.ISO
{
    /// <summary>
    /// Utilities used in reading the ISO 9660 file system
    /// </summary>
    public static class IsoUtilities
    {
        /// <summary>
        /// Converts an ISO-9660 date format to a DateTimeOffset
        /// </summary>
        /// <param name="date">the ISO-9660 date format</param>
        /// <returns>a DateTimeOffset containing the time</returns>
        public static DateTimeOffset? FromIsoDateTime(ReadOnlySpan<char> date)
        {
            if (date.Length != 17)
            {
                throw new ArgumentOutOfRangeException(nameof(date));
            }

            bool success = int.TryParse(date[0..4], out int year);
            success &= int.TryParse(date[4..6], out int month);
            success &= int.TryParse(date[6..8], out int day);
            success &= int.TryParse(date[8..10], out int hour);
            success &= int.TryParse(date[10..12], out int minute);
            success &= int.TryParse(date[12..14], out int second);
            success &= int.TryParse(date[14..16], out int hundredths);

            if (!success)
            {
                throw new ArgumentOutOfRangeException(nameof(date));
            }

            sbyte offsetByte = (sbyte)date[16];

            if (year == 0 && month == 0 && day == 0 && hour == 0 && minute == 0 && second == 0 && hundredths == 0 && offsetByte == 0)
            {
                return null;
            }

            TimeSpan offset = TimeSpan.FromMinutes((sbyte)date[6] * 15);
            return new DateTimeOffset(year, month, day, hour, minute, second, hundredths * 10, offset);
        }

        /// <summary>
        /// Converts an ISO-9660 recording date format to a DateTimeOffset
        /// </summary>
        /// <param name="date">the ISO-9660 recording date format</param>
        /// <returns>a DateTimeOffset containing the time</returns>
        public static DateTimeOffset? FromIsoRecordingDateTime(ReadOnlySpan<byte> date)
        {
            if(date.Length != 7)
            {
                throw new ArgumentOutOfRangeException(nameof(date));
            }

            bool zero = true;
            foreach (var t in date)
            {
                zero &= (t == 0);
            }

            if (zero)
            {
                return null;
            }

            TimeSpan offset = TimeSpan.FromMinutes((sbyte)date[6] * 15);
            return new DateTimeOffset(1900 + date[0], date[1], date[2], date[3], date[4], date[5], offset);
        }

        /// <summary>
        /// Reads both Little-Endian and Big-Endian values in a Multi-Endian 16-bit unsigned integer and ensures they match
        /// </summary>
        /// <param name="bytes">A span containing the multi-endian value</param>
        /// <returns>The 16-bit unsigned integer</returns>
        /// <exception cref="ArgumentOutOfRangeException">The span is the wrong size to contain the data</exception>
        /// <exception cref="InvalidDataException">The Little-Endian and Big-Endian values do not represent the same data</exception>
        public static ushort ReadUInt16MultiEndian(in ReadOnlySpan<byte> bytes)
        {
            if (bytes.Length != 4)
            {
                throw new ArgumentOutOfRangeException(nameof(bytes));
            }

            ushort leValue = BinaryPrimitives.ReadUInt16LittleEndian(bytes[0..2]);
            ushort beValue = BinaryPrimitives.ReadUInt16BigEndian(bytes[2..4]);

            if (leValue != beValue)
            {
                throw new InvalidDataException();
            }

            return leValue;
        }

        /// <summary>
        /// Reads both Little-Endian and Big-Endian values in a Multi-Endian 32-bit unsigned integer and ensures they match
        /// </summary>
        /// <param name="bytes">A span containing the multi-endian value</param>
        /// <returns>The 32-bit unsigned integer</returns>
        /// <exception cref="ArgumentOutOfRangeException">The span is the wrong size to contain the data</exception>
        /// <exception cref="InvalidDataException">The Little-Endian and Big-Endian values do not represent the same data</exception>
        public static uint ReadUInt32MultiEndian(in ReadOnlySpan<byte> bytes)
        {
            if (bytes.Length != 8)
            {
                throw new ArgumentOutOfRangeException(nameof(bytes));
            }

            uint leValue = BinaryPrimitives.ReadUInt32LittleEndian(bytes[0..4]);
            uint beValue = BinaryPrimitives.ReadUInt32BigEndian(bytes[4..8]);

            if (leValue != beValue)
            {
                throw new InvalidDataException();
            }

            return leValue;
        }
    }
}
