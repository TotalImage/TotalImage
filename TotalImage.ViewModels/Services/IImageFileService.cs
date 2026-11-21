using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TotalImage.Containers;

namespace TotalImage.ViewModels.Services;

/// <summary>
/// Reads and writes image files. The slow work runs on a background thread; the returned tasks
/// complete on the caller's context when it has one.
/// </summary>
public interface IImageFileService
{
    /// <summary>
    /// Opens an image file as the container its extension names, and detects its partition
    /// table and file systems.
    /// </summary>
    /// <exception cref="EmptyImageFileException">The file is empty.</exception>
    /// <exception cref="FileNotFoundException">The file doesn't exist.</exception>
    /// <exception cref="IOException">The file can't be read, for example because it is locked.</exception>
    /// <exception cref="UnauthorizedAccessException">Access to the file was denied.</exception>
    /// <exception cref="InvalidDataException">The container format is not recognized or the data is not valid.</exception>
    Task<Container> OpenAsync(string path, CancellationToken cancellationToken = default);

    /// <summary>
    /// Writes the image and its pending changes to a file, replacing it. The container is
    /// released (disposed) once the file has been written, so it must be opened again to be used.
    /// </summary>
    /// <exception cref="IOException">The file can't be written.</exception>
    /// <exception cref="UnauthorizedAccessException">Access to the file was denied.</exception>
    Task SaveAsync(Container image, string path, CancellationToken cancellationToken = default);
}

/// <summary>
/// Thrown when an image file to open is empty (zero bytes long).
/// </summary>
public sealed class EmptyImageFileException(string path)
    : Exception($"The file \"{path}\" is empty.")
{
    /// <summary>
    /// The empty file.
    /// </summary>
    public string Path { get; } = path;
}
