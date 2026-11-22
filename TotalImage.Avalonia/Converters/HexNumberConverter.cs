using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace TotalImage.Avalonia.Converters;

/// <summary>
/// Shows a <see cref="Avalonia.Controls.NumericUpDown"/> value as hexadecimal text and reads it
/// back, for its <c>TextConverter</c> (decimal values can't be formatted as hexadecimal).
/// </summary>
/// <remarks>
/// NumericUpDown converts text to a value with <see cref="Convert"/> and a value to text with
/// <see cref="ConvertBack"/>; both directions go by the type of the input, so the converter
/// doesn't depend on that.
/// </remarks>
public sealed class HexNumberConverter : IValueConverter
{
    public static HexNumberConverter Instance { get; } = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => Translate(value);

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => Translate(value);

    private static object? Translate(object? value) => value switch
    {
        decimal number => ((long)number).ToString("X2", CultureInfo.InvariantCulture),
        string text when long.TryParse(text.Trim(), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var number) => (decimal)number,
        _ => null,
    };
}
