using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TotalImage.Containers;
using TotalImage.Containers.Anex86;
using TotalImage.Containers.NHD;
using TotalImage.Containers.TAP;
using TotalImage.Containers.VHD;

namespace TotalImage.ViewModels.Services;

/// <summary>
/// Opens and saves image files on the thread pool.
/// </summary>
public sealed class ImageFileService : IImageFileService
{
    /// <inheritdoc/>
    public Task<Container> OpenAsync(string path, CancellationToken cancellationToken = default)
        => Task.Run(() => Open(path), cancellationToken);

    /// <inheritdoc/>
    public Task SaveAsync(Container image, string path, CancellationToken cancellationToken = default)
        => Task.Run(async () =>
        {
            if (image.PendingChanges.IsDirty)
                await image.CommitChanges(path);
            else
                image.SaveImage(path);
        }, cancellationToken);

    private static Container Open(string path)
    {
        if (new FileInfo(path).Length == 0)
            throw new EmptyImageFileException(path);

        //Disable this for now until it's properly implemented
        bool memoryMapping = false; //fileinfo.Length > settings.Current.MemoryMappingThreshold;

        //We're hardcoded for supported file extensions currently. This should probably be revisited at some point to see if
        //there's a better way to handle weird extensions for supported container types etc.
        Container container = Path.GetExtension(path).ToLowerInvariant() switch
        {
            ".vhd" => new VhdContainer(path, memoryMapping),
            ".nhd" => new NhdContainer(path, memoryMapping),
            ".imz" or ".zip" => new ImzContainer(path, memoryMapping),
            ".hdi" or ".fdi" => new Anex86Container(path, memoryMapping),
            ".img" or ".ima" or ".iso" or ".vfd" or ".flp" or ".360" or
            ".720" or ".12" or ".144" or ".288" or ".dsk" or ".hdm" => new RawContainer(path, memoryMapping),
            ".json" => new PCjsContainer(path, memoryMapping),
            ".tap" => new TapContainer(path, memoryMapping),
            _ => throw new InvalidDataException("This container format is not recognized and cannot be opened."),
        };

        try
        {
            // Detect the partition table and the file systems here, off the UI thread.
            foreach (var partition in container.PartitionTable.Partitions)
            {
                try
                {
                    _ = partition.FileSystem;
                }
                catch (InvalidDataException)
                {
                    // An unsupported file system; the caller decides what to do with it.
                }
            }
        }
        catch
        {
            container.Dispose();
            throw;
        }

        return container;
    }
}
