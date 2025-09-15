using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TotalImage
{
    /// <summary>
    /// A utility class for calculating CRC32, MD5 and SHA-1 hashes.
    /// </summary>
    public static class HashCalculator
    {
        static SemaphoreSlim hashMutex = new SemaphoreSlim(1, 1);

        static async Task<string> CalculateHashAsyncCore(Stream stream, HashAlgorithm algorithm, CancellationToken cancellationToken)
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
        /// Calculates the MD5 hash of this file
        /// </summary>
        /// <returns>A string containing the hexadecimal representation of the MD5 hash</returns>
        public static string CalculateMd5Hash(Stream stream)
        {
            using MD5 md5 = MD5.Create();
            stream.Seek(0, SeekOrigin.Begin);
            byte[]? hash = md5.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        /// <summary>
        /// Asynchronously calculates the MD5 hash of this file
        /// </summary>
        /// <returns>A task that represents the asynchronous hash calculation operation and wraps the string containing the hexadecimal representation of the MD5 hash</returns>
        public static async Task<string> CalculateMd5HashAsync(Stream stream, CancellationToken cancellationToken = default)
            => await CalculateHashAsyncCore(stream, MD5.Create(), cancellationToken);

        /// <summary>
        /// Calculates the SHA-1 hash of this file
        /// </summary>
        /// <returns>A string containing the hexadecimal representation of the SHA-1 hash</returns>
        public static string CalculateSha1Hash(Stream stream)
        {
            using SHA1 sha1 = SHA1.Create();
            stream.Seek(0, SeekOrigin.Begin);
            byte[]? hash = sha1.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        /// <summary>
        /// Asynchronously calculates the SHA-1 hash of this file
        /// </summary>
        /// <returns>A task that represents the asynchronous hash calculation operation and wraps the string containing the hexadecimal representation of the SHA-1 hash</returns>
        public static async Task<string> CalculateSha1HashAsync(Stream stream, CancellationToken cancellationToken = default)
            => await CalculateHashAsyncCore(stream,SHA1.Create(), cancellationToken);
    }
}
