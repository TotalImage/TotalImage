using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace TotalImage.Avalonia.Converters;

/// <summary>
/// Small converters used by the views.
/// </summary>
public static class ValueConverters
{
    /// <summary>
    /// Whether the value equals the converter parameter, compared as text. For radio menu items
    /// such as the view mode or the sort column.
    /// </summary>
    public static IValueConverter EqualsParameter { get; } = new FuncValueConverter<object?, object?, bool>(
        (value, parameter) => string.Equals(value?.ToString(), parameter?.ToString(), StringComparison.Ordinal));

    /// <summary>
    /// Wraps a single item into a list, or gives an empty list for <c>null</c>. For the folder
    /// tree, whose only top-level item is the root folder.
    /// </summary>
    public static IValueConverter SingleItemList { get; } = new FuncValueConverter<object?, object?[]>(
        value => value is null ? [] : [value]);
}
