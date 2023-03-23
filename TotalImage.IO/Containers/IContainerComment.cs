namespace TotalImage.Containers;

/// <summary>
/// Interface for container formats that support specifying an image comment.
/// </summary>
public interface IContainerComment
{
    /// <summary>
    /// The comment for this image.
    /// </summary>
    string? Comment { get; }
}
