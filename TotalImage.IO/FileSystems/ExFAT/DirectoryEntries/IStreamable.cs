using System.IO;

namespace TotalImage.FileSystems.ExFAT;

/// <summary>
/// Exposes a stream for an exFAT directory entry payload.
/// </summary>
public interface IStreamable
{
    /// <summary>
    /// Opens a stream for the entry data.
    /// </summary>
    /// <param name="fileSystem">The file system that owns the entry.</param>
    /// <returns>A readable stream for the entry data.</returns>
    Stream GetStream(ExFatFileSystem fileSystem);
}
