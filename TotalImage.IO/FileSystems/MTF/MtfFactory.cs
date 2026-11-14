using System;
using System.IO;

namespace TotalImage.FileSystems.MTF;

/// <summary>
/// Recognizes Microsoft Tape Format archives by their leading TAPE descriptor.
/// </summary>
public sealed class MtfFactory : IFileSystemFactory
{
    /// <inheritdoc />
    /// <exception cref="InvalidDataException">A recognized MTF archive is malformed or incomplete.</exception>
    /// <exception cref="NotSupportedException">A recognized MTF archive uses an unsupported feature.</exception>
    public FileSystem? TryLoadFileSystem(Stream stream)
    {
        return HasSignature(stream) ? new MtfFileSystem(stream) : null;
    }

    /// <summary>
    /// Checks for a leading TAPE signature without changing the stream's current position.
    /// </summary>
    /// <param name="stream">The readable, seekable logical archive stream to probe.</param>
    /// <returns>True when the stream begins with the MTF TAPE signature; otherwise, false.</returns>
    internal static bool HasSignature(Stream stream)
    {
        if (stream.Length < 4)
        {
            return false;
        }
        long position = stream.Position;
        try
        {
            stream.Position = 0;
            Span<byte> signature = stackalloc byte[4];
            stream.ReadExactly(signature);
            return signature.SequenceEqual("TAPE"u8);
        }
        finally
        {
            stream.Position = position;
        }
    }
}
