using System;

namespace TotalImage
{
    public enum SizeUnit
    {
        Bytes = 1,
        Decimal = 1000,
        Binary = 1024
    }

    public static class SizeUnitExtensions
    {
        static readonly char[] prefixes = { '☢', 'K', 'M', 'G', 'T', 'P', 'E', 'Z', 'Y' };

        private static int DeterminePrefix(this SizeUnit sizeUnit, ulong size)
        {
            if (sizeUnit == SizeUnit.Bytes) return 0;

            var i = 0;
            while ((size /= (ulong)sizeUnit) > 0)
                i++;

            return i;
        }

        public static string FormatSize(this SizeUnit sizeUnit, ulong size)
        {
            var prefix = sizeUnit.DeterminePrefix(size);

            var denominator = 1.0;
            for (var i = 0; i < prefix; i++)
                denominator *= (ulong)sizeUnit;

            var formattedSize = size / denominator;

            if (prefix == 0)
                sizeUnit = SizeUnit.Bytes;

            if (formattedSize >= 1000 && sizeUnit == SizeUnit.Binary)
            {
                prefix++;
                formattedSize /= (double)sizeUnit;
            }

            var prefixSign = sizeUnit switch
            {
                SizeUnit.Bytes => "B",
                SizeUnit.Binary => $"{prefixes[prefix]}iB",
                SizeUnit.Decimal => $"{prefixes[prefix]}B",
                _ => throw new ArgumentException()
            };

            return $"{formattedSize:#,0.##} {prefixSign}";
        }

        public static string FormatSize(this SizeUnit sizeUnit, ulong size, bool includeBytes)
            => includeBytes ? $"{sizeUnit.FormatSize(size)} ({SizeUnit.Bytes.FormatSize(size)})" : sizeUnit.FormatSize(size);
    }
}