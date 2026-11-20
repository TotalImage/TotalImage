using System;
using System.Windows.Forms;
using TotalImage.ViewModels.Settings;

namespace TotalImage
{
    /// <summary>
    /// Converts between the shared settings enums and their WinForms counterparts.
    /// </summary>
    internal static class EnumMappings
    {
        public static View ToWinForms(this ViewMode mode) => mode switch
        {
            ViewMode.LargeIcon => View.LargeIcon,
            ViewMode.Details => View.Details,
            ViewMode.SmallIcon => View.SmallIcon,
            ViewMode.List => View.List,
            ViewMode.Tile => View.Tile,
            _ => throw new ArgumentOutOfRangeException(nameof(mode))
        };

        public static SortOrder ToWinForms(this SortDirection direction) => direction switch
        {
            SortDirection.None => SortOrder.None,
            SortDirection.Ascending => SortOrder.Ascending,
            SortDirection.Descending => SortOrder.Descending,
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };

        public static SortDirection ToSortDirection(this SortOrder order) => order switch
        {
            SortOrder.None => SortDirection.None,
            SortOrder.Ascending => SortDirection.Ascending,
            SortOrder.Descending => SortDirection.Descending,
            _ => throw new ArgumentOutOfRangeException(nameof(order))
        };

        public static SystemColorMode ToWinForms(this ColorTheme theme) => theme switch
        {
            ColorTheme.Classic => SystemColorMode.Classic,
            ColorTheme.System => SystemColorMode.System,
            ColorTheme.Dark => SystemColorMode.Dark,
            _ => throw new ArgumentOutOfRangeException(nameof(theme))
        };
    }
}
