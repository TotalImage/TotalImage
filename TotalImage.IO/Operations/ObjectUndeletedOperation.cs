using TiFileSystemObject = TotalImage.FileSystems.FileSystemObject;

namespace TotalImage.Operations
{
    /// <summary>
    /// An operation for undeleting a file system object.
    /// </summary>
    /// <param name="targetObject">The file system object to be undeleted.</param>
    /// <param name="firstChar">The first character of the object's name after undeletion in FAT file systems.</param>
    public class ObjectUndeletedOperation(TiFileSystemObject targetObject, char firstChar) : Operation(targetObject)
    {
        /// <summary>
        /// The first character of the object's name after undeletion in FAT file systems.
        /// </summary>
        public char? FirstCharacter { get; } = firstChar;
    }
}
