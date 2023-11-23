using System;

namespace TotalImage
{
    /// <summary>
    /// The size unit that is used to display all sizes in the UI.
    /// </summary>
    public enum SizeUnit
    {
        /// <summary>
        /// All sizes are always displayed in bytes, indicated with a "B" suffix.
        /// </summary>
        Bytes = 1,

        /// <summary>
        /// All sizes are displayed in the most appropriate decimal unit with its SI prefix.
        /// </summary>
        Decimal = 1000,

        /// <summary>
        /// All sizes are displayed in the most appropriate binary unit with its IEC prefix.
        /// </summary>
        Binary = 1024
    }

    /// <summary>
    /// A class for some handy size unit-related methods.
    /// </summary>
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

        /// <summary>
        /// Formats the provided size in the provided size unit.
        /// </summary>
        /// <param name="sizeUnit">The size unit to format the provided size with.</param>
        /// <param name="size">The size to format with the provided size unit.</param>
        /// <returns>Formatted size as a string.</returns>
        /// <exception cref="ArgumentException">The provided size unit is invalid.</exception>
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

        /// <summary>
        /// Formats the provided size in the provided size unit and optionally includes size formatted in bytes at the end.
        /// </summary>
        /// <param name="sizeUnit">The size unit to format the provided size with.</param>
        /// <param name="size">The size to format with the provided size unit.</param>
        /// <param name="includeBytes">Include the same size formatted in bytes at the end of the string.</param>
        /// <returns>Formatted size as a string.</returns>
        public static string FormatSize(this SizeUnit sizeUnit, ulong size, bool includeBytes)
            => includeBytes ? $"{sizeUnit.FormatSize(size)} ({SizeUnit.Bytes.FormatSize(size)})" : sizeUnit.FormatSize(size);
    }
}
