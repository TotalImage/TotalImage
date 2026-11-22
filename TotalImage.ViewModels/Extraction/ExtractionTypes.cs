namespace TotalImage.ViewModels.Extraction;

/// <summary>
/// Where and how to extract entries of the image.
/// </summary>
/// <param name="TargetPath">The folder to extract into. It is created when needed.</param>
/// <param name="DirectoryMode">What to do with the folders among the entries.</param>
/// <param name="IncludeHidden">Whether hidden entries inside extracted folders are extracted too.</param>
/// <param name="PreserveDates">Whether the extracted files get the dates they have in the image.</param>
/// <param name="PreserveAttributes">Whether the extracted files get the attributes they have in the image.</param>
public sealed record ExtractionOptions(
    string TargetPath,
    DirectoryExtractionMode DirectoryMode,
    bool IncludeHidden,
    bool PreserveDates,
    bool PreserveAttributes);

/// <summary>
/// How far an extraction is.
/// </summary>
/// <param name="Completed">The number of files done (extracted or skipped).</param>
/// <param name="Total">The number of files to extract.</param>
/// <param name="FileName">The file being extracted now, or <c>null</c> when the files have just been listed.</param>
public readonly record struct ExtractionProgress(int Completed, int Total, string? FileName);

/// <summary>
/// What to do with a file that exists already in the target folder.
/// </summary>
public enum OverwriteChoice
{
    Overwrite,
    Skip,

    /// <summary>
    /// Stop the extraction.
    /// </summary>
    Cancel
}

/// <summary>
/// The answer to <see cref="Services.IMessageService.AskOverwriteAsync"/>.
/// </summary>
/// <param name="Choice">What to do with the file.</param>
/// <param name="ApplyToAll">Whether to do the same with every other existing file of this extraction, without asking.</param>
public sealed record OverwriteAnswer(OverwriteChoice Choice, bool ApplyToAll = false);
