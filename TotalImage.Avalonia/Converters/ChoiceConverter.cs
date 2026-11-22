using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace TotalImage.Avalonia.Converters;

/// <summary>
/// Binds a radio button's <c>IsChecked</c> to a property with one of several values: the button
/// is checked while the property equals the converter parameter, and checking it sets the
/// property to the parameter.
/// </summary>
public sealed class ChoiceConverter : IValueConverter
{
    public static ChoiceConverter Instance { get; } = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => Equals(value, parameter);

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is true ? parameter : BindingOperations.DoNothing;
}
