using System;

namespace TotalImage.FileSystems.FAT
{
    internal static class Helper
    {
        public static DateTime FatToDateTime(ushort date)
        {
            if(date == 0) return DateTime.MinValue;
            var year = ((date & 0xFE00) >> 9) + 1980;
            var month = (date & 0x1E0) >> 5;
            var day = date & 0x1F;

            return new DateTime(year, month, day);
        }

        public static DateTime FatToDateTime(ushort date, ushort time)
        {
            if(date == 0 && time == 0) return DateTime.MinValue;
            var year = ((date & 0xFE00) >> 9) + 1980;
            var month = (date & 0x1E0) >> 5;
            var day = date & 0x1F;
            var hour = (time & 0xF800) >> 11;
            var minute = (time & 0x7E0) >> 5;
            var second = (time & 0x1F) * 2;

            return new DateTime(year, month, day, hour, minute, second);
        }

        public static DateTime FatToDateTime(ushort date, ushort time, byte tenths)
        {
            if(date == 0 && time == 0 && tenths == 0) return DateTime.MinValue;
            var year = ((date & 0xFE00) >> 9) + 1980;
            var month = (date & 0x1E0) >> 5;
            var day = date & 0x1F;
            var hour = (time & 0xF800) >> 11;
            var minute = (time & 0x7E0) >> 5;
            var second = (time & 0x1F) * 2 + (tenths / 100);
            var millisecond = (tenths % 100) * 100;

            return new DateTime(year, month, day, hour, minute, second, millisecond);
        }

        public static string TrimFileName(string filename)
            => filename.Substring(0, 8).Trim() + "." + filename.Substring(8, 3);
    }
}