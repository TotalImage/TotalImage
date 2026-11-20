using System.IO;

namespace TotalImage.ViewModels.Services;

/// <summary>
/// Keeps temporary files in a folder under the system temporary folder.
/// </summary>
public sealed class TempDirectoryService : ITempDirectoryService
{
    /// <summary>
    /// Uses TotalImage in the system temporary folder.
    /// </summary>
    public TempDirectoryService() : this(Path.Combine(Path.GetTempPath(), "TotalImage")) { }

    /// <summary>
    /// Uses the given folder.
    /// </summary>
    public TempDirectoryService(string root)
    {
        Root = root;
    }

    /// <inheritdoc/>
    public string Root { get; }

    /// <inheritdoc/>
    public void EnsureRootExists() => Directory.CreateDirectory(Root);

    /// <inheritdoc/>
    public string GetUniqueSubdirectoryPath()
        => Path.Combine(Root, $"~{Path.GetFileNameWithoutExtension(Path.GetRandomFileName()).ToUpperInvariant()}");

    /// <inheritdoc/>
    public void Clear()
    {
        if (Directory.Exists(Root))
            Directory.Delete(Root, true);

        Directory.CreateDirectory(Root);
    }
}
