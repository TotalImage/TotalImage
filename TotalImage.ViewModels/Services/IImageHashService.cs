using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TotalImage.ViewModels.Services;

/// <summary>
/// The hashes shown for an image file.
/// </summary>
public enum ImageHashKind
{
    Crc32,
    Md5,
    Sha1
}

/// <summary>
/// Calculates hashes of whole streams on a background thread.
/// </summary>
public interface IImageHashService
{
    /// <summary>
    /// Calculates a hash of the whole stream, from its start, on a background thread.
    /// </summary>
    /// <returns>The hash as lower-case hexadecimal digits.</returns>
    /// <exception cref="System.OperationCanceledException">The calculation was cancelled.</exception>
    Task<string> ComputeAsync(Stream stream, ImageHashKind kind, CancellationToken cancellationToken);
}
