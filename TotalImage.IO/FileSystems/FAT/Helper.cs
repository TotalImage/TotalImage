using System;
using System.Linq;
using System.Text;

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
                return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Unspecified);
            }
            catch (ArgumentOutOfRangeException)
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
                return new DateTime(year, month, day, hour, minute, second, DateTimeKind.Unspecified);
            }
            catch (ArgumentOutOfRangeException)
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
                return new DateTime(year, month, day, hour, minute, second, millisecond, DateTimeKind.Unspecified);
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        public static string UseAsLabel(string label, int length = 8)
        {
            if (label.Length > length)
            {
                label = label.Substring(0, length);
            }
            else if (label.Length < length)
            {
                label = label.PadRight(length);
            }

            var chars = label.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] >= 128)
                {
                    chars[i] = '_';
                }
                else if (char.IsLower(chars[i]))
                {
                    chars[i] = char.ToUpper(chars[i]);
                }
            }

            return new string(chars);
        }

        public static string TrimFileName(string filename)
            => filename.Substring(0, 8).Trim() + (string.IsNullOrWhiteSpace(filename.Substring(8, 3)) ? "" : $".{filename.Substring(8, 3).TrimEnd()}");
    }
}