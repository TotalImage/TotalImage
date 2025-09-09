using System;
using System.IO;

namespace TotalImage.Operations
{
    /// <summary>
    /// The base operation class for tracking changes made to an image.
    /// </summary>
    /// <param name="targetObject">The object on which this operation will be applied.</param>
    /// <param name="timestamp">The date and time when this operation was made.</param>
    public abstract class Operation(object targetObject, DateTime timestamp)
    {
        /// <summary>
        /// The object that was the target of the operation.
        /// </summary>
        public object TargetObject { get; } = targetObject;

        /// <summary>
        /// The date and time when this operation was made.
        /// </summary>
        public DateTime Timestamp { get; } = timestamp;

        /// <summary>
        /// Apply this operation to the disk image stream to write the byte-level changes.
        /// </summary>
        /// <param name="imageStream">The stream representing the disk image content.</param>
        /// <remarks>
        /// Default implementation performs no action. Override in derived classes to implement specific operation logic.
        /// </remarks>
        public virtual void Apply(Stream imageStream)
        {
            // Default implementation: no action
            // Override in derived classes to implement specific byte-level changes
        }
    }
}
