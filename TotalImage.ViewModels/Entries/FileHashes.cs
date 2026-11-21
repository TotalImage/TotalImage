using System.Threading;
using System.Threading.Tasks;
using TotalImage.FileSystems;

namespace TotalImage.ViewModels.Entries;

/// <summary>
/// The hashes of a file's contents, as lower-case hexadecimal strings.
/// </summary>
public sealed record FileHashes(string Crc32, string Md5, string Sha1)
{
    /// <summary>
    /// Reads the file and calculates its hashes on a background thread.
    /// </summary>
    public static Task<FileHashes> CalculateAsync(File file, CancellationToken cancellationToken = default)
        => Task.Run(async () =>
        {
            using var stream = file.GetStream();
            var crc32 = await HashCalculator.CalculateCrc32HashAsync(stream, cancellationToken);
            var md5 = await HashCalculator.CalculateMd5HashAsync(stream, cancellationToken);
            var sha1 = await HashCalculator.CalculateSha1HashAsync(stream, cancellationToken);
            return new FileHashes(crc32, md5, sha1);
        }, cancellationToken);
}
