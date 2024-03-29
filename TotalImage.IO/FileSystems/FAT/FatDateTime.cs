using System;

namespace TotalImage.FileSystems.FAT
{
    internal static class FatDateTime
    {
        public static (int, int, int) UnpackFatDate(ushort date)
        {
            var year = (date & 0xFE00) >> 9;
            var month = (date & 0x1E0) >> 5;
            var day = date & 0x1F;

            return (year, month, day);
        }

        public static ushort PackFatDate(int year, int month, int day)
            => (ushort)(((year - 1980) << 9) | (month << 5) | day);

        public static (int, int, int) UnpackFatTime(ushort time)
        {
            var hour = (time & 0xF800) >> 11;
            var minute = (time & 0x7E0) >> 5;
            var second = (time & 0x1F) << 1;

            return (hour, minute, second);
        }

        public static ushort PackFatTime(int hour, int minute, int second)
            => (ushort)((hour << 11) | (minute << 5) | (second >> 1));

        static int[] dayCounts = { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        static bool IsDateValid(int year, int month, int day)
        {
            if (month < 1 || day < 1) return false;
            if (month > 12) return false;
            if (day <= dayCounts[month]) return true;
            if (month == 2 && day == dayCounts[month] + 1 && DateTime.IsLeapYear(year)) return true;
            return false;
        }

        static bool IsTimeValid(int hour, int minute, int second)
            => hour < 24 && minute < 60 && second < 60;

        public static DateTime? ToDateTime(ushort date)
        {
            var (year, month, day) = UnpackFatDate(date);

            year += 1980;

            if (IsDateValid(year, month, day))
                return new DateTime(year, month, day);
            else
                return null;
        }

        public static DateTime? ToDateTime(ushort date, ushort time)
        {
            var (year, month, day) = UnpackFatDate(date);
            var (hour, minute, second) = UnpackFatTime(time);

            year += 1980;

            if (IsDateValid(year, month, day) && IsTimeValid(hour, minute, second))
                return new DateTime(year, month, day, hour, minute, second, DateTimeKind.Unspecified);
            else
                return null;
        }

        public static DateTime? ToDateTime(ushort date, ushort time, byte csec)
            => csec < 200 ? ToDateTime(date, time)?.AddMilliseconds(csec * 10) : null;
    }

    internal static class DateTimeExtensions
    {
        public static (ushort, ushort, byte) ToFatDateTime(this DateTime dateTime)
        {
            if (dateTime.Year < 1980) throw new InvalidOperationException();

            var date = FatDateTime.PackFatDate(dateTime.Year, dateTime.Month, dateTime.Day);
            var time = FatDateTime.PackFatTime(dateTime.Hour, dateTime.Minute, dateTime.Second);
            var msec = (byte)((dateTime.Second & 1) * 100 + dateTime.Millisecond / 10);

            return (date, time, msec);
        }
    }
}