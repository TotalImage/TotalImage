namespace TotalImage.Containers;

/// <summary>
/// Interface for container formats that support specifying an image comment.
/// </summary>
public interface IContainerComment
{
    /// <summary>
    /// The comment for this image.
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// Writes the provided comment to the image.
    /// </summary>
    /// <param name="comment">The comment to write</param>
    public abstract void WriteComment(string comment);
}
