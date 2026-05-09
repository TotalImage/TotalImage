using System;

namespace TotalImage.Changes
{
    /// <summary>
    /// Represents a pending deletion of a file or directory (and all its contents) from the image.
    /// </summary>
    public sealed class DeleteEntryChange : PendingChange
    {
        /// <summary>
        /// The path components identifying the entry to delete within the filesystem.
        /// Example: <c>["SUBDIR", "README.TXT"]</c> deletes <c>SUBDIR\README.TXT</c>.
        /// </summary>
        public string[] Path { get; }

        /// <summary>
        /// Creates a new <see cref="DeleteEntryChange"/>.
        /// </summary>
        /// <param name="path">Path components identifying the entry to delete.</param>
        public DeleteEntryChange(string[] path)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
        }
    }
}
