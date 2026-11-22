using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using TotalImage.Changes;
using TotalImage.FileSystems.FAT;
using TotalImage.ViewModels.Browsing;
using TotalImage.ViewModels.Formatting;
using TotalImage.ViewModels.Services;
using Directory = TotalImage.FileSystems.Directory;
using FileSystemObject = TotalImage.FileSystems.FileSystemObject;

namespace TotalImage.ViewModels.Main;

// Editing: adding files, creating folders, deleting and renaming entries. Every edit is queued
// in the image's change set and written when the image is saved.
public sealed partial class MainViewModel
{
    /// <summary>
    /// Asks for files of the computer and adds them to the current folder.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanInjectFiles))]
    private async Task InjectFiles()
    {
        //TODO: Get the count and total size of seleted items to inject before showing the dialog
        var paths = await filePicker.PickFilesToAddAsync();
        if (paths.Count == 0)
            return;

        var current = settings.Current;
        if (current.ConfirmInjection)
        {
            ulong totalSize = 0;
            foreach (var path in paths)
            {
                try { totalSize += (ulong)new FileInfo(path).Length; }
                catch { /* best effort */ }
            }

            var answer = await messages.ConfirmInjectAsync(paths.Count, current.SizeUnit.FormatSize(totalSize));
            if (answer.DontAskAgain)
                current.ConfirmInjection = false;

            if (!answer.Confirmed)
                return;
        }

        var target = CurrentFolder?.Directory;
        if (target is FatDirectory fatDirectory)
        {
            foreach (var path in paths)
            {
                var source = ReadFile(path);
                var now = DateTime.Now;
                fatDirectory.EnqueueAddFile(Path.GetFileName(path), source, FatAttributes.Archive, now, now, now);
            }
        }
        else if (target is PendingDirectory pendingDirectory && Image is { } image)
        {
            var folderPath = PendingItems.PathOf(pendingDirectory);
            foreach (var path in paths)
            {
                var source = ReadFile(path);
                var now = DateTime.Now;
                image.PendingChanges.Add(new AddFileChange([.. folderPath, Path.GetFileName(path)], source, FatAttributes.Archive, now, now, now));
            }
        }

        FileDataSource ReadFile(string path)
        {
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            return FileDataSource.Create(stream, tempDirectory.Root, current.MemoryMappingThreshold);
        }
    }

    /// <summary>
    /// Asks for a name and creates a folder with it in the current folder.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanCreateFolder))]
    private async Task NewFolder()
    {
        var name = await entryDialogs.AskNewFolderNameAsync();
        if (name is null || Image is not { } image || CurrentFolder is not { } folder)
            return;

        if (folder.Directory is FatDirectory fatDirectory)
            fatDirectory.EnqueueCreateSubdirectory(name);
        else if (folder.Directory is PendingDirectory pendingDirectory)
            image.PendingChanges.Add(new CreateDirectoryChange([.. PendingItems.PathOf(pendingDirectory), name]));
    }

    /// <summary>
    /// Deletes the selected entries, or the folder selected in the tree, after asking when the
    /// settings say so.
    /// </summary>
    //TODO: Implement deletion here and in the FS/container
    [RelayCommand(CanExecute = nameof(CanDelete))]
    private async Task Delete()
    {
        var current = settings.Current;
        if (ActivePane == BrowsePane.Files && SelectedObjects is { Count: > 0 } selected)
        {
            if (current.ConfirmDeletion)
            {
                var size = 0ul;
                var directories = 0;
                foreach (var entry in selected)
                {
                    if (entry is Directory directory)
                    {
                        directories++;
                        size += directory.GetSize(true, false); //For directories, calculate the total size of everything inside
                    }
                    else
                    {
                        size += entry.Length;
                    }
                }

                var answer = await messages.ConfirmDeleteAsync(new DeletionSummary(selected.Count - directories, directories, current.SizeUnit.FormatSize(size)));
                if (!Confirmed(answer))
                    return;
            }

            foreach (var entry in selected)
                EnqueueDelete(entry);
        }
        else if (ActivePane == BrowsePane.Folders && CurrentFolder is { IsRoot: false } folder)
        {
            if (current.ConfirmDeletion && !Confirmed(await messages.ConfirmDeleteFolderAsync()))
                return;

            EnqueueDelete(folder.Directory);
        }

        bool Confirmed(Confirmation answer)
        {
            if (answer.DontAskAgain)
                current.ConfirmDeletion = false;

            return answer.Confirmed;
        }
    }

    // Only FAT entries can be changed so far; others (and pending entries) are left alone.
    private static void EnqueueDelete(FileSystemObject entry)
    {
        if (entry is FatFile fatFile)
            fatFile.EnqueueDelete();
        else if (entry is FatDirectory fatDirectory)
            fatDirectory.EnqueueDelete();
    }
}
