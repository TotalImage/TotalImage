namespace TotalImage.ViewModels.Extraction;

/// <summary>
/// The directory extraction mode that is used for extracting directories and their contents.
/// </summary>
public enum DirectoryExtractionMode
{
    /// <summary>
    /// Skips all directories in the selected objects list.
    /// </summary>
    Skip,
    /// <summary>
    /// Extracts all directories in the selected objects list and preserves their original structure.
    /// </summary>
    Preserve,
    /// <summary>
    /// Extracts the contents of all directories in the selected objects list into the same target directory.
    /// </summary>
    Merge
}
