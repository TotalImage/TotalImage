using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace TotalImage.UI.Converters;

public class SizeConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return null;
        }

        decimal adjustedSize = System.Convert.ToDecimal(value);
        FileSizeUnit unit = FileSizeUnit.Bytes;

        while ((adjustedSize >= 1024) && (unit < FileSizeUnit.EiB))
        {
            adjustedSize /= 1024;
            unit++;
        }

        return $"{adjustedSize:0.##} {unit}";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// The unit for a file size value
    /// </summary>
    public enum FileSizeUnit : byte
    {
        /// <summary>
        /// The value is in bytes
        /// </summary>
        Bytes = 0,

        /// <summary>
        /// The value is in kibibytes
        /// </summary>
        KiB,

        /// <summary>
        /// The value is in mebibytes
        /// </summary>
        MiB,

        /// <summary>
        /// The value is in gibibytes
        /// </summary>
        GiB,

        /// <summary>
        /// The value is in tebibytes
        /// </summary>
        TiB,

        /// <summary>
        /// The value is in pebibytes
        /// </summary>
        PiB,

        /// <summary>
        /// The value is in exbibytes
        /// </summary>
        EiB
    }
}
