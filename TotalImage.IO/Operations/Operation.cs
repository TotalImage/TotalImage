namespace TotalImage.Operations
{
    /// <summary>
    /// The base operation class for tracking changes made to an image.
    /// </summary>
    /// <param name="targetObject">The object on which this operation will be applied.</param>
    public abstract class Operation(object targetObject)
    {
        /// <summary>
        /// The object that was the target of the operation.
        /// </summary>
        public object TargetObject { get; } = targetObject;
    }
}
