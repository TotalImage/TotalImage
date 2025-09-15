using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace TotalImage
{
    /// <summary>
    /// A utility class for calculating CRC32, MD5 and SHA-1 hashes.
    /// </summary>
    public static class HashCalculator
    {
        private static SemaphoreSlim hashMutex = new SemaphoreSlim(1, 1);

        private static async Task<string> CalculateHashAsyncCore(Stream stream, HashAlgorithm algorithm, CancellationToken cancellationToken)
        {
            await hashMutex.WaitAsync(cancellationToken);
            byte[] hash;

            try
            {
                stream.Position = 0;
                hash = await algorithm.ComputeHashAsync(stream, cancellationToken);
            }
            finally
            {
                hashMutex.Release();
            }

            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        /// <summary>
        /// Asynchronously calculates the MD5 hash of the provided stream.
        /// </summary>
        /// <returns>A task that represents the asynchronous hash calculation operation and wraps the string containing the hexadecimal representation of the MD5 hash.</returns>
        public static async Task<string> CalculateMd5HashAsync(Stream stream, CancellationToken cancellationToken = default)
            => await CalculateHashAsyncCore(stream, MD5.Create(), cancellationToken);

        /// <summary>
        /// Asynchronously calculates the SHA-1 hash of the provided stream.
        /// </summary>
        /// <returns>A task that represents the asynchronous hash calculation operation and wraps the string containing the hexadecimal representation of the SHA-1 hash.</returns>
        public static async Task<string> CalculateSha1HashAsync(Stream stream, CancellationToken cancellationToken = default)
            => await CalculateHashAsyncCore(stream,SHA1.Create(), cancellationToken);
    }
}
