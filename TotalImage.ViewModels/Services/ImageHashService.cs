using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TotalImage.ViewModels.Services;

/// <summary>
/// Calculates hashes with <see cref="HashCalculator"/> on the thread pool.
/// </summary>
public sealed class ImageHashService : IImageHashService
{
    /// <inheritdoc/>
    public Task<string> ComputeAsync(Stream stream, ImageHashKind kind, CancellationToken cancellationToken)
        => Task.Run(() => kind switch
        {
            ImageHashKind.Crc32 => HashCalculator.CalculateCrc32HashAsync(stream, cancellationToken),
            ImageHashKind.Md5 => HashCalculator.CalculateMd5HashAsync(stream, cancellationToken),
            ImageHashKind.Sha1 => HashCalculator.CalculateSha1HashAsync(stream, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(kind))
        }, cancellationToken);
}
