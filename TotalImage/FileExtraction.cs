using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TotalImage;

using TiFile = TotalImage.FileSystems.File;
using TiDirectory = TotalImage.FileSystems.Directory;
using TiFileSystemObject = TotalImage.FileSystems.FileSystemObject;

public enum DirectoryExtractionMode
{
    Skip,
    Preserve,
    Merge
}

public static class FileExtraction
{
    enum ConfirmOverwriteDialogResult
    {
        Cancel,
        Overwrite,
        OverwriteAll,
        Skip,
        SkipAll
    }

    static ConfirmOverwriteDialogResult ShowConfirmOverwriteDialog(IWin32Window parentDialog, string filename)
    {
        var overwriteButton = new TaskDialogButton("&Overwrite");
        var skipButton = new TaskDialogButton("&Skip");
        var overwritePage = new TaskDialogPage()
        {
            Text = $"The file \"{filename}\" already exists in the target directory. Do you want to overwrite it?",
            Heading = "File already exists",
            Caption = "Warning",
            Buttons =
            {
                overwriteButton,
                skipButton,
                TaskDialogButton.Cancel
            },
            AllowCancel = true,
            Verification = new TaskDialogVerificationCheckBox()
            {
                Checked = false,
                Text = "Do this for all currently extracted files"
            },
            Icon = TaskDialogIcon.Warning,
            DefaultButton = TaskDialogButton.Cancel
        };

        var result = TaskDialog.ShowDialog(parentDialog, overwritePage);

        if (result == TaskDialogButton.Cancel)
        {
            return ConfirmOverwriteDialogResult.Cancel;
        }
        else
        {
            var overwrite = result == overwriteButton;

            if (overwritePage.Verification.Checked)
                return overwrite ? ConfirmOverwriteDialogResult.OverwriteAll : ConfirmOverwriteDialogResult.SkipAll;
            else
                return overwrite ? ConfirmOverwriteDialogResult.Overwrite : ConfirmOverwriteDialogResult.Skip;
        }
    }

    static async IAsyncEnumerable<(TiFile, string)> EnumerateFilesForExtractionAsync(IEnumerable<TiFileSystemObject> items, string path, DirectoryExtractionMode mode, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var files = from x in items where x is TiFile select x as TiFile;

        foreach (var file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return (file, Path.Combine(path, file.Name));
        }

        if (mode != DirectoryExtractionMode.Skip)
        {
            var dirs = from x in items where x is TiDirectory select x as TiDirectory;

            foreach (var dir in dirs)
            {
                var children = EnumerateFilesForExtractionAsync(
                    dir.EnumerateFileSystemObjects(Settings.CurrentSettings.ShowHiddenItems, false),
                    mode switch
                    {
                        DirectoryExtractionMode.Merge => path,
                        DirectoryExtractionMode.Preserve => Path.Combine(path, dir.Name),
                        _ => throw new ArgumentException()
                    }, mode, cancellationToken);

                await foreach (var child in children)
                {
                    yield return child;
                }
            }
        }
    }

    static async Task ExtractFilesAsync(IEnumerable<(TiFile, string)> items, IProgress<(int, string)>? progress = default, CancellationToken cancellationToken = default, Func<string, bool>? confirmOverwriteCallback = default)
    {
        var i = 0;
        var mutex = new SemaphoreSlim(1, 1);

        foreach (var (file, path) in items)
        {
            cancellationToken.ThrowIfCancellationRequested();
            progress?.Report((i++, file.Name));

            var fileExists = await Task.Run<bool>(() =>
            {
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                    Directory.CreateDirectory(Path.GetDirectoryName(path)!);

                return File.Exists(path);
            });

            if (fileExists && confirmOverwriteCallback?.Invoke(file.Name) == false)
                continue;

            await Task.Run(async () =>
            {
                await mutex.WaitAsync();

                using (var destStream = new FileStream(path, FileMode.Create))
                using (var fileStream = file.GetStream())
                {
                    fileStream.Position = 0; // reset position to zero because CopyTo will only go from current position
                    await fileStream.CopyToAsync(destStream, cancellationToken);
                }

                mutex.Release();

                if (Settings.CurrentSettings.ExtractPreserveDates)
                {
                    if (file.CreationTime.HasValue)
                        File.SetCreationTime(path, file.CreationTime.Value);

                    if (file.LastAccessTime.HasValue)
                        File.SetLastAccessTime(path, file.LastAccessTime.Value);

                    if (file.LastWriteTime.HasValue)
                        File.SetLastWriteTime(path, file.LastWriteTime.Value);
                }

                if (Settings.CurrentSettings.ExtractPreserveAttributes)
                {
                    /* NOTE: Windows automatically sets the Archive attribute on all newly created files, so even if the attribute is cleared in the
                    * image, Windows will still automatically set it anyway. Should we perhaps try to work around this by manually clearing it after? */
                    File.SetAttributes(path, file.Attributes);
                }
            });
        }
    }

    public static bool ExtractFiles(IWin32Window parentWindow, IEnumerable<TiFileSystemObject> items, string path, DirectoryExtractionMode mode, bool openFolder, bool overwrite = false)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var cancelButton = TaskDialogButton.Cancel;
        cancelButton.Click += (_, _) => cancellationTokenSource.Cancel();

        var page = new TaskDialogPage()
        {
            Caption = "Extracting",
            Heading = "Extracting files, please wait...",
            Text = "This might take a while, so sit back and relax while we do all the work.",
            Buttons = { cancelButton },
            ProgressBar = new TaskDialogProgressBar()
            {
                State = TaskDialogProgressBarState.Marquee
            },
            Footnote = new TaskDialogFootnote()
            {
                Text = "Enumerating files..."
            }
        };

        page.Created += async (_, _) =>
        {
            var progress = new Progress<(int, string)>();
            progress.ProgressChanged += (_, e) =>
            {
                page.ProgressBar.Value = e.Item1;
                page.Footnote.Text = $"Extracting {e.Item2}";
            };

            try
            {
                var files = await EnumerateFilesForExtractionAsync(items, path, mode, cancellationTokenSource.Token)
                    .ToListAsync(cancellationTokenSource.Token);

                page.ProgressBar.Maximum = files.Count;
                page.ProgressBar.State = TaskDialogProgressBarState.Normal;

                bool? overwriteAll = overwrite || !Settings.CurrentSettings.ConfirmOverwriteExtraction ? true : null;

                await ExtractFilesAsync(files, progress, cancellationTokenSource.Token, (filename) =>
                {
                    if (overwriteAll is null)
                        switch (ShowConfirmOverwriteDialog(page.BoundDialog!, filename))
                        {
                            case ConfirmOverwriteDialogResult.Overwrite:
                                return true;
                            case ConfirmOverwriteDialogResult.OverwriteAll:
                                overwriteAll = true;
                                return true;
                            case ConfirmOverwriteDialogResult.Skip:
                                return false;
                            case ConfirmOverwriteDialogResult.SkipAll:
                                overwriteAll = false;
                                return false;
                            case ConfirmOverwriteDialogResult.Cancel:
                                cancellationTokenSource.Cancel();
                                return false;
                            default:
                                throw new ArgumentException();
                        }
                    else return overwriteAll.Value;
                });
            }
            catch (OperationCanceledException)
            {
                // task canceled
            }
            finally
            {
                page.BoundDialog?.Close();
            }
        };

        TaskDialog.ShowDialog(parentWindow, page);

        if (openFolder && !cancellationTokenSource.IsCancellationRequested)
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = path,
                UseShellExecute = true,
                ErrorDialog = true,
                ErrorDialogParentHandle = parentWindow.Handle
            });
        }

        return !cancellationTokenSource.IsCancellationRequested;
    }

    public static bool ExtractFilesToTemporaryDirectory(IWin32Window parentWindow, IEnumerable<TiFileSystemObject> items, DirectoryExtractionMode mode)
        => ExtractFiles(parentWindow, items, ((frmMain)Application.OpenForms["frmMain"]).tempDir, mode, false, true);

    public static bool ExtractFiles(IWin32Window parentWindow, IEnumerable<TiFileSystemObject> items)
        => ExtractFiles(parentWindow, items,
            Settings.CurrentSettings.DefaultExtractPath,
            Settings.CurrentSettings.DefaultDirectoryExtractionMode,
            Settings.CurrentSettings.OpenFolderAfterExtract);
}
