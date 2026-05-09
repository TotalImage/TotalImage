using System;

namespace TotalImage.Changes
{
    /// <summary>
    /// Represents a pending creation of a new directory in the image.
    /// </summary>
    public sealed class CreateDirectoryChange : PendingChange
    {
        /// <summary>
        /// The path components identifying the new directory to create within the filesystem.
        /// The last component is the name of the new directory; preceding components are its ancestors.
        /// Example: <c>["DOCS", "NOTES"]</c> creates <c>DOCS\NOTES</c>.
        /// </summary>
        public string[] Path { get; }

        /// <summary>
        /// Creates a new <see cref="CreateDirectoryChange"/>.
        /// </summary>
        /// <param name="path">Path components identifying the new directory.</param>
        public CreateDirectoryChange(string[] path)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
        }
    }
}
