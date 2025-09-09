using System;
using TotalImage.Containers;

namespace TotalImage.Operations 
{
    /// <summary>
    /// An operation for changing the comment of an image, if the container type supports comments.
    /// </summary>

    public class ImageCommentChangedOperation : Operation
    {
        /// <summary>
        /// The image comment before this operation.
        /// </summary>
        public string OldComment { get; }

        /// <summary>
        /// The image comment after this operation.
        /// </summary>
        public string NewComment { get; }

        /// <summary>
        /// Creates a new ImageCommentChangedOperation, if the current container supports comments.
        /// </summary>
        /// <param name="targetObject">The image whose comment will be changed.</param>
        /// <param name="oldComment">Old comment of the image.</param>
        /// <param name="newComment">New comment of the image.</param>
        /// <param name="timestamp">The date and time when the comment was changed.</param>
        public ImageCommentChangedOperation(Container targetObject, string oldComment, string newComment, DateTime timestamp) : base(targetObject, timestamp)
        {
            //Container doesn't support comments, so this operation can't be performed...
            if(targetObject is not IContainerComment)
            {
                throw new InvalidOperationException("This container type does not support comments!");
            }

            //We allow NewComment to be null, empty or whitespace here, since this effectively means the comment was removed.
            OldComment = oldComment;
            NewComment = newComment;
        }
    }
}
