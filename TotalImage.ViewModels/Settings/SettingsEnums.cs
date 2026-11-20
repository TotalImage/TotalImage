namespace TotalImage.ViewModels.Settings;

// The numeric values of these enums are stored in settings.json, so they must
// match the values of the WinForms enums they replace (View, SortOrder and
// SystemColorMode). Never renumber them.

/// <summary>
/// How the file list presents its entries.
/// </summary>
public enum ViewMode
{
    LargeIcon = 0,
    Details = 1,
    SmallIcon = 2,
    List = 3,
    Tile = 4
}

/// <summary>
/// The direction of a sort.
/// </summary>
public enum SortDirection
{
    None = 0,
    Ascending = 1,
    Descending = 2
}

/// <summary>
/// The application's color theme.
/// </summary>
public enum ColorTheme
{
    /// <summary>
    /// Always light.
    /// </summary>
    Classic = 0,
    /// <summary>
    /// Follows the system theme.
    /// </summary>
    System = 1,
    /// <summary>
    /// Always dark.
    /// </summary>
    Dark = 2
}
