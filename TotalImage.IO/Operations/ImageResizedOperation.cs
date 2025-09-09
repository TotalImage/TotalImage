using System;
using System.IO;
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

        /// <summary>
        /// Apply this operation to resize the image.
        /// </summary>
        /// <param name="imageStream">The stream representing the disk image content.</param>
        public override void Apply(Stream imageStream)
        {
            // Resize the stream to the new size
            imageStream.SetLength((long)NewSize);
            imageStream.Flush();
        }
    }
}
