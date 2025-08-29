using System;

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
    }
}
