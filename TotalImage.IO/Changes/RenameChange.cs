using System;

namespace TotalImage.Changes
{
    /// <summary>
    /// Represents a pending rename of a file or directory in the image.
    /// </summary>
    public sealed class RenameChange : PendingChange
    {
        /// <summary>
        /// The path components identifying the entry to rename within the filesystem.
        /// Example: <c>["SUBDIR", "OLD.TXT"]</c> renames <c>SUBDIR\OLD.TXT</c>.
        /// </summary>
        public string[] OldPath { get; }

        /// <summary>
        /// The new name for the entry (not a full path — just the filename or directory name).
        /// </summary>
        public string NewName { get; }

        /// <summary>
        /// Creates a new <see cref="RenameChange"/>.
        /// </summary>
        /// <param name="oldPath">Path components identifying the entry to rename.</param>
        /// <param name="newName">The new name for the entry.</param>
        public RenameChange(string[] oldPath, string newName)
        {
            OldPath = oldPath ?? throw new ArgumentNullException(nameof(oldPath));
            NewName = newName ?? throw new ArgumentNullException(nameof(newName));
        }
    }
}
