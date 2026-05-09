namespace TotalImage.Changes
{
    /// <summary>
    /// Base class for all pending mutations against a disk image. Changes are accumulated in a
    /// <see cref="ChangeSet"/> and applied atomically to a temporary copy of the image on commit.
    /// </summary>
    public abstract class PendingChange { }
}
