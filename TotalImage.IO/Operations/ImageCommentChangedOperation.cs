using TotalImage.Containers;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for changing the comment of an image.
    /// </summary>
    /// <param name="targetObject">The image whose comment will be changed.</param>
    /// <param name="oldComment">Old comment of the image.</param>
    /// <param name="newComment">New comment of the image.</param>
    public class ImageCommentChangedOperation(Container targetObject, string oldComment, string newComment) : Operation(targetObject)
    {
        /// <summary>
        /// The image comment before this operation.
        /// </summary>
        public string OldComment { get; } = oldComment;

        /// <summary>
        /// The image comment after this operation.
        /// </summary>
        public string NewComment { get; } = newComment;
    }
}
