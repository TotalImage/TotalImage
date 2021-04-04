using System;
using System.Linq;

namespace TotalImage.FileSystems.FAT
{
    internal static class Helper
    {
        public static DateTime? FatToDateTime(ushort date)
        {
            if (date == 0) return null;

            var year = ((date & 0xFE00) >> 9) + 1980;
            var month = (date & 0x1E0) >> 5;
            var day = date & 0x1F;

            try
            {
                return new DateTime(year, month, day);
            }
            catch(ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        public static DateTime? FatToDateTime(ushort date, ushort time)
        {
            if (date == 0 && time == 0) return null;

            var year = ((date & 0xFE00) >> 9) + 1980;
            var month = (date & 0x1E0) >> 5;
            var day = date & 0x1F;
            var hour = (time & 0xF800) >> 11;
            var minute = (time & 0x7E0) >> 5;
            var second = (time & 0x1F) * 2;

            try
            {
                return new DateTime(year, month, day, hour, minute, second);
            }
            catch(ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        public static DateTime? FatToDateTime(ushort date, ushort time, byte tenths)
        {
            if (date == 0 && time == 0 && tenths == 0) return null;

            var year = ((date & 0xFE00) >> 9) + 1980;
            var month = (date & 0x1E0) >> 5;
            var day = date & 0x1F;
            var hour = (time & 0xF800) >> 11;
            var minute = (time & 0x7E0) >> 5;
            var second = (time & 0x1F) * 2 + (tenths / 100);
            var millisecond = (tenths % 100) * 10;

            try
            {
                return new DateTime(year, month, day, hour, minute, second, millisecond);
            }
            catch(ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        public static string TrimFileName(string filename)
            => filename.Substring(0, 8).Trim() + (string.IsNullOrWhiteSpace(filename.Substring(8, 3)) ? "" : $".{filename.Substring(8, 3).TrimEnd()}");

        public static byte LfnChecksum(byte[] filename)
        {
            if (filename.Length != 11) throw new ArgumentException();

            byte sum = 0;

            for (var i = 0; i < 11; i++)
            {
                sum = (byte)(((sum & 1) << 7) + (sum >> 1) + filename[i]);
            }

            return sum;
        }

        public static byte[] RetrieveLongNameBytes(DirectoryEntry entry)
        {
            var bytes = new byte[26];

            Buffer.BlockCopy(entry.name, 1, bytes, 0, 10);
            BitConverter.GetBytes(entry.crtTime).CopyTo(bytes, 10);
            BitConverter.GetBytes(entry.crtDate).CopyTo(bytes, 12);
            BitConverter.GetBytes(entry.lstAccDate).CopyTo(bytes, 14);
            BitConverter.GetBytes(entry.fstClusHI).CopyTo(bytes, 16);
            BitConverter.GetBytes(entry.wrtTime).CopyTo(bytes, 18);
            BitConverter.GetBytes(entry.wrtDate).CopyTo(bytes, 20);
            BitConverter.GetBytes(entry.fileSize).CopyTo(bytes, 22);

            return bytes;
        }

        public static byte[] RetrieveLongNameBytes(DirectoryEntry[] entries)
            => entries.SelectMany(x => RetrieveLongNameBytes(x)).ToArray();
    }
}