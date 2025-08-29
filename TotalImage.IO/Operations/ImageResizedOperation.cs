using System;
using TotalImage.Containers;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for resizing an image.
    /// </summary>
    /// <param name="targetObject">The image which will be resized.</param>
    /// <param name="oldSize">Old size of the image in bytes.</param>
    /// <param name="newSize">New size of the image in bytes.</param>
    public class ImageResizedOperation(Container targetObject, ulong oldSize, ulong newSize) : Operation(targetObject, DateTime.Now)
    {
        /// <summary>
        /// The size of the image before this operation.
        /// </summary>
        public ulong OldSize { get; } = oldSize;

        /// <summary>
        /// The size of the image after this operation.
        /// </summary>
        public ulong NewSize { get; } = newSize;
    }
}
