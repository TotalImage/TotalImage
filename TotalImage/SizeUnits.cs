using System;

namespace TotalImage
{
    public enum SizeUnits
    {
        Bytes = 1,
        Decimal = 1000,
        Binary = 1024
    }

    public static class SizeUnitsExtensions
    {
        static readonly char[] prefixes = { '☢', 'K', 'M', 'G', 'T', 'P', 'E', 'Z', 'Y' };

        private static int GetPrefixIndexForSize(this SizeUnits sizeUnit, ulong size)
        {
            var i = 0;
            while (size / (ulong)sizeUnit > 0)
                i++;

            return i;
        }

        public static string FormatSize(this SizeUnits sizeUnit, ulong size)
        {
            var prefix = sizeUnit.GetPrefixIndexForSize(size);

            var denominator = 1ul;
            for (var i = 0; i < prefix; i++)
                denominator *= (ulong)sizeUnit;

            var formattedSize = size / denominator;

            if (prefix == 0)
                sizeUnit = SizeUnits.Bytes;

            var prefixSign = sizeUnit switch
            {
                SizeUnits.Bytes => "B",
                SizeUnits.Binary => $"{prefixes[prefix]}iB",
                SizeUnits.Decimal => $"{prefixes[prefix]}B",
                _ => throw new ArgumentException()
            };

            return $"{formattedSize:#,0.##} {prefixSign}";
        }

        public static string FormatSize(this SizeUnits sizeUnit, ulong size, bool includeBytes)
            => includeBytes ? $"{sizeUnit.FormatSize(size)} ({SizeUnits.Bytes.FormatSize(size)})" : sizeUnit.FormatSize(size);

        public static string FormatSize(this Settings.SizeUnit sizeUnit, ulong size)
            => $"{(size / (float)sizeUnit):#,0.##} {Enum.GetName(typeof(Settings.SizeUnit), sizeUnit)}";

        public static string FormatSize(this Settings.SizeUnit sizeUnit, ulong size, bool includeBytes)
            => includeBytes ? $"{sizeUnit.FormatSize(size)} ({Settings.SizeUnit.B.FormatSize(size)})" : sizeUnit.FormatSize(size);
    }
}