using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TotalImage.FileSystems;
using TotalImage.ViewModels.Extraction;
using Directory = TotalImage.FileSystems.Directory;
using File = TotalImage.FileSystems.File;

namespace TotalImage.ViewModels.Services;

/// <summary>
/// Copies files and folders out of the image into a folder of the computer.
/// </summary>
public interface IFileExtractionService
{
    /// <summary>
    /// Extracts entries of the image. The image is read on a background thread, so nothing else may
    /// read it until the returned task completes (the UI shows a modal progress window meanwhile).
    /// Progress is reported and questions are asked on the caller's context.
    /// </summary>
    /// <param name="entries">The files and folders to extract.</param>
    /// <param name="options">Where and how to extract them.</param>
    /// <param name="askOverwrite">Asks what to do with a file (named without its folder) that exists
    /// already, or <c>null</c> to overwrite every such file without asking.</param>
    /// <param name="progress">Receives the progress, or <c>null</c>.</param>
    /// <param name="cancellationToken">Stops the extraction before the next file.</param>
    /// <returns><c>true</c> when everything was extracted (or skipped), <c>false</c> when the
    /// extraction was cancelled, through the token or an overwrite answer.</returns>
    /// <exception cref="IOException">A file could not be read or written.</exception>
    /// <exception cref="UnauthorizedAccessException">A file could not be written.</exception>
    Task<bool> ExtractAsync(IEnumerable<FileSystemObject> entries, ExtractionOptions options,
        Func<string, Task<OverwriteAnswer>>? askOverwrite, IProgress<ExtractionProgress>? progress = null,
        CancellationToken cancellationToken = default);
}

/// <inheritdoc cref="IFileExtractionService"/>
public sealed class FileExtractionService : IFileExtractionService
{
    /// <inheritdoc/>
    public async Task<bool> ExtractAsync(IEnumerable<FileSystemObject> entries, ExtractionOptions options,
        Func<string, Task<OverwriteAnswer>>? askOverwrite, IProgress<ExtractionProgress>? progress = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var files = await Task.Run(() => ListFiles(entries, options.TargetPath, options, cancellationToken).ToList(), cancellationToken);
            progress?.Report(new(0, files.Count, null));

            // The answer that applies to every other existing file, once the user said so.
            OverwriteChoice? choiceForAll = askOverwrite is null ? OverwriteChoice.Overwrite : null;

            for (var i = 0; i < files.Count; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var (file, path) = files[i];
                progress?.Report(new(i, files.Count, file.Name));

                var exists = await Task.Run(() =>
                {
                    System.IO.Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                    return System.IO.File.Exists(path);
                }, cancellationToken);

                if (exists)
                {
                    var choice = choiceForAll;
                    if (choice is null)
                    {
                        var answer = await askOverwrite!(file.Name);
                        choice = answer.Choice;
                        if (answer.ApplyToAll && answer.Choice != OverwriteChoice.Cancel)
                            choiceForAll = answer.Choice;
                    }

                    if (choice == OverwriteChoice.Cancel)
                        return false;
                    if (choice == OverwriteChoice.Skip)
                        continue;
                }

                await Task.Run(() => CopyAsync(file, path, options, cancellationToken), cancellationToken);
            }

            return true;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            return false;
        }
    }

    // The files to extract, with their target paths: the files among the entries first, then
    // the files of each folder, depth first.
    private static IEnumerable<(File File, string Path)> ListFiles(IEnumerable<FileSystemObject> entries, string targetPath,
        ExtractionOptions options, CancellationToken cancellationToken)
    {
        var list = entries.ToList();
        foreach (var file in list.OfType<File>())
        {
            // Deleted entries start with a question mark; they aren't extracted.
            if (file.Name.StartsWith('?'))
                continue;

            cancellationToken.ThrowIfCancellationRequested();
            yield return ((File)file.ResolveTarget(), Path.Combine(targetPath, file.Name));
        }

        if (options.DirectoryMode == DirectoryExtractionMode.Skip)
            yield break;

        foreach (var directory in list.OfType<Directory>())
        {
            var target = (Directory)directory.ResolveTarget();
            var childPath = options.DirectoryMode == DirectoryExtractionMode.Preserve ? Path.Combine(targetPath, directory.Name) : targetPath;
            foreach (var child in ListFiles(target.EnumerateFileSystemObjects(options.IncludeHidden), childPath, options, cancellationToken))
                yield return child;
        }
    }

    private static async Task CopyAsync(File file, string path, ExtractionOptions options, CancellationToken cancellationToken)
    {
        using (var target = new FileStream(path, FileMode.Create))
        using (var source = file.GetStream())
        {
            source.Position = 0; // CopyTo only copies from the current position
            await source.CopyToAsync(target, cancellationToken);
        }

        if (options.PreserveDates)
        {
            if (file.CreationTime.HasValue)
                System.IO.File.SetCreationTime(path, file.CreationTime.Value);

            if (file.LastAccessTime.HasValue)
                System.IO.File.SetLastAccessTime(path, file.LastAccessTime.Value);

            if (file.LastWriteTime.HasValue)
                System.IO.File.SetLastWriteTime(path, file.LastWriteTime.Value);
        }

        if (options.PreserveAttributes)
        {
            /* NOTE: Windows automatically sets the Archive attribute on all newly created files, so even if the attribute is cleared in the
             * image, Windows will still automatically set it anyway. Should we perhaps try to work around this by manually clearing it after? */
            System.IO.File.SetAttributes(path, file.Attributes);
        }
    }
}
