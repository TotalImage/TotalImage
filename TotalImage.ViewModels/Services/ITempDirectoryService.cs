namespace TotalImage.ViewModels.Services;

/// <summary>
/// Manages the folder where the application keeps temporary files.
/// </summary>
public interface ITempDirectoryService
{
    /// <summary>
    /// The application's temporary folder.
    /// </summary>
    string Root { get; }

    /// <summary>
    /// Creates <see cref="Root"/> if it doesn't exist.
    /// </summary>
    void EnsureRootExists();

    /// <summary>
    /// Returns the path of a new, uniquely named subfolder of <see cref="Root"/>. The folder is
    /// not created.
    /// </summary>
    string GetUniqueSubdirectoryPath();

    /// <summary>
    /// Deletes everything in <see cref="Root"/>.
    /// </summary>
    /// <exception cref="System.IO.IOException">A file is in use.</exception>
    /// <exception cref="System.UnauthorizedAccessException">A file can't be deleted.</exception>
    void Clear();
}
