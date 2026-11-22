using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using Avalonia.Media;
using TotalImage.ViewModels.Browsing;

namespace TotalImage.Avalonia.Controls;

/// <summary>
/// The colors of entries and folders, as in the WinForms UI: pending changes are highlighted
/// (green = added, red = pending delete, orange = renamed) and hidden ones are gray.
/// </summary>
public static class EntryColors
{
    private static readonly IBrush AddedBack = new SolidColorBrush(Color.FromRgb(198, 239, 206));
    private static readonly IBrush AddedFore = new SolidColorBrush(Color.FromRgb(0, 97, 0));
    private static readonly IBrush DeletedBack = new SolidColorBrush(Color.FromRgb(255, 199, 206));
    private static readonly IBrush DeletedFore = new SolidColorBrush(Color.FromRgb(156, 0, 6));
    private static readonly IBrush RenamedBack = new SolidColorBrush(Color.FromRgb(255, 235, 156));
    private static readonly IBrush RenamedFore = new SolidColorBrush(Color.FromRgb(156, 87, 0));
    private static readonly IBrush HiddenFore = Brushes.Gray;

    public static IBrush? Background(PendingState state) => state switch
    {
        PendingState.Added => AddedBack,
        PendingState.Deleted => DeletedBack,
        PendingState.Renamed => RenamedBack,
        _ => null
    };

    public static IBrush? Foreground(PendingState state, bool isHidden) => state switch
    {
        PendingState.Added => AddedFore,
        PendingState.Deleted => DeletedFore,
        PendingState.Renamed => RenamedFore,
        _ => isHidden ? HiddenFore : null
    };

    /// <summary>
    /// Colors a row or item of the file list. Rows are reused, so colors are cleared when not needed.
    /// </summary>
    public static void Apply(TemplatedControl control, EntryViewModel? entry)
    {
        if (entry is not null && Background(entry.PendingState) is { } back)
            control.Background = back;
        else
            control.ClearValue(TemplatedControl.BackgroundProperty);

        if (entry is not null && Foreground(entry.PendingState, entry.IsHidden) is { } fore)
            control.Foreground = fore;
        else
            control.ClearValue(TemplatedControl.ForegroundProperty);
    }

    /// <summary>
    /// The background of a folder of the tree, or unset.
    /// </summary>
    public static IValueConverter FolderBackground { get; } = new Converter(value =>
        value is FolderViewModel folder ? Background(folder.PendingState) : null);

    /// <summary>
    /// The text color of a folder of the tree, or unset.
    /// </summary>
    public static IValueConverter FolderForeground { get; } = new Converter(value =>
        value is FolderViewModel folder ? Foreground(folder.PendingState, folder.IsHidden) : null);

    // Gives AvaloniaProperty.UnsetValue rather than null, so the default color applies.
    private sealed class Converter(Func<object?, IBrush?> convert) : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => convert(value) ?? AvaloniaProperty.UnsetValue;

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
