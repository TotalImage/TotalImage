using System;
using System.Globalization;
using Avalonia.Data.Converters;
using TotalImage.Avalonia.Services;
using TotalImage.ViewModels.Browsing;

namespace TotalImage.Avalonia.Converters;

/// <summary>
/// Converts an <see cref="IconKey"/> to its image. The app puts one for small and one for large
/// icons into its resources, under <see cref="SmallKey"/> and <see cref="LargeKey"/>.
/// </summary>
public sealed class FileIconConverter(FileIcons icons, bool large) : IValueConverter
{
    public const string SmallKey = "SmallFileIconConverter";
    public const string LargeKey = "LargeFileIconConverter";

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is IconKey key ? icons.Get(key, large) : null;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
