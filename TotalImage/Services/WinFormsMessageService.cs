using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using TotalImage.ViewModels.Extraction;
using TotalImage.ViewModels.Services;

namespace TotalImage.Services
{
    public sealed class WinFormsMessageService(DialogOwner owner) : IMessageService
    {
        public Task<bool> ConfirmFormatAsync()
        {
            var result = ShowTaskDialog(new TaskDialogPage()
            {
                Text = "Are you sure you want to continue?",
                Heading = "WARNING: Formatting will erase all data in the current partition!",
                Caption = "Warning",
                Buttons =
                {
                    TaskDialogButton.Yes,
                    TaskDialogButton.No
                },
                Icon = TaskDialogIcon.Warning,
            });

            return Task.FromResult(result == TaskDialogButton.Yes);
        }

        public Task ShowRecentImagesClearedAsync()
        {
            ShowTaskDialog(new TaskDialogPage()
            {
                Text = "List of recently opened images has been successfully cleared.",
                Heading = "Recent images cleared",
                Caption = "Success",
                Buttons =
                {
                    TaskDialogButton.OK
                },
                Icon = TaskDialogIcon.Information,
                DefaultButton = TaskDialogButton.OK
            });

            return Task.CompletedTask;
        }

        public Task<bool> ConfirmResetSettingsAsync()
        {
            var result = ShowTaskDialog(new TaskDialogPage()
            {
                Text = "All settings will be reset to their default values.",
                Heading = "Are you sure you want to continue?",
                Caption = "Warning",
                Buttons =
                {
                    TaskDialogButton.Yes,
                    TaskDialogButton.No
                },
                Icon = TaskDialogIcon.Warning,
                DefaultButton = TaskDialogButton.No
            });

            return Task.FromResult(result == TaskDialogButton.Yes);
        }

        public Task ShowSettingsResetAsync()
        {
            ShowTaskDialog(new TaskDialogPage()
            {
                Text = "All settings were successfully reset to their default values.",
                Heading = "Settings reset",
                Caption = "Success",
                Buttons =
                {
                    TaskDialogButton.OK
                },
                Icon = TaskDialogIcon.Information,
                DefaultButton = TaskDialogButton.OK
            });

            return Task.CompletedTask;
        }

        public Task<bool> ConfirmClearTempFolderWithOtherInstancesAsync()
        {
            var result = ShowTaskDialog(new TaskDialogPage()
            {
                Text = $"More than one instance of TotalImage is currently running. Attempting to clear the temporary folder while other instances are " +
                $"using it can cause unpredictable behavior.{Environment.NewLine}{Environment.NewLine}Are you sure you want to continue?",
                Heading = "Multiple instances are running",
                Caption = "Warning",
                Buttons =
                {
                    TaskDialogButton.Yes,
                    TaskDialogButton.No,
                },
                Icon = TaskDialogIcon.Warning,
                DefaultButton = TaskDialogButton.No,
                SizeToContent = true
            });

            return Task.FromResult(result == TaskDialogButton.Yes);
        }

        public Task ShowTempFolderClearedAsync()
        {
            ShowTaskDialog(new TaskDialogPage()
            {
                Text = "Temporary folder has been successfully cleared.",
                Heading = "Temporary folder cleared",
                Caption = "Success",
                Buttons =
                {
                    TaskDialogButton.OK
                },
                Icon = TaskDialogIcon.Information,
                DefaultButton = TaskDialogButton.OK
            });

            return Task.CompletedTask;
        }

        public Task ShowTempFolderClearFailedAsync()
        {
            ShowTaskDialog(new TaskDialogPage()
            {
                Text = "Temporary folder could not be cleared because it is in use by another process or inaccessible.",
                Heading = "Cannot clear temporary folder",
                Caption = "Error",
                Buttons =
                {
                    TaskDialogButton.OK
                },
                Icon = TaskDialogIcon.Error,
                DefaultButton = TaskDialogButton.OK
            });

            return Task.CompletedTask;
        }

        public Task ShowPartitionNotSupportedAsync()
        {
            ShowTaskDialog(new TaskDialogPage()
            {
                Text = "This partition cannot be loaded because it or the file system contained within is not supported. Select a supported partition to load.",
                Heading = "Unsupported partition type",
                Caption = "Information",
                Buttons =
                {
                    TaskDialogButton.OK
                },
                Icon = TaskDialogIcon.Information,
                DefaultButton = TaskDialogButton.OK,
                SizeToContent = true
            });

            return Task.CompletedTask;
        }

        public Task<bool> ConfirmCreateWithoutBpbAsync()
        {
            var result = ShowTaskDialog(new TaskDialogPage()
            {
                Text = "You chose not to create a BIOS Parameter Block (BPB) for this image. Some programs and operating systems may not recognize the disk because of this.\n\nAre you sure you want to continue?",
                Heading = "No BIOS Parameter Block selected",
                Caption = "Warning",
                Buttons =
                {
                    TaskDialogButton.Yes,
                    TaskDialogButton.No
                },
                Icon = TaskDialogIcon.Warning,
            });

            return Task.FromResult(result == TaskDialogButton.Yes);
        }

        public Task<bool> ConfirmExportWithoutHashesAsync()
        {
            var result = ShowTaskDialog(new TaskDialogPage()
            {
                Text = $"File hashes are still being calculated and won't be included in the exported file{Environment.NewLine}unless you wait for the calculations to complete first.",
                Heading = "Hash calculations in progress",
                Caption = "Warning",
                Buttons =
                {
                    TaskDialogButton.Cancel,
                    TaskDialogButton.Continue
                },
                Icon = TaskDialogIcon.Warning,
                DefaultButton = TaskDialogButton.Continue,
                SizeToContent = true
            });

            return Task.FromResult(result == TaskDialogButton.Continue);
        }

        public Task ShowExportSucceededAsync(string path)
        {
            ShowTaskDialog(new TaskDialogPage()
            {
                Text = $"The data was successfully exported to {path}",
                Heading = "Export successful",
                Caption = "Success",
                Buttons =
                {
                    TaskDialogButton.OK
                },
                Icon = TaskDialogIcon.Information,
                DefaultButton = TaskDialogButton.OK
            });

            return Task.CompletedTask;
        }

        public Task ShowErrorAsync(string operation, Exception exception)
        {
            var (heading, reason) = exception switch
            {
                UnauthorizedAccessException => ("Access denied",
                    "because you do not have the necessary permissions. Make sure you have the necessary permissions and try again."),
                IOException when (exception.HResult & 0x0000FFFF) == 32 => ("Access denied",
                    "because the file is locked by another process. Close all processes locking this file and try again."),
                _ => ("Operation failed",
                    $"with the following error: {exception.Message}")
            };

            ShowTaskDialog(new TaskDialogPage()
            {
                Text = $"{operation} failed {reason}{Environment.NewLine}{Environment.NewLine}" +
                    "If you think this is a bug, please submit a bug report on our GitHub repo.",
                Heading = heading,
                Caption = "Error",
                Buttons =
                {
                    TaskDialogButton.OK
                },
                Icon = TaskDialogIcon.Error,
                DefaultButton = TaskDialogButton.OK
            });

            return Task.CompletedTask;
        }

        public Task<bool> ConfirmCreateExtractFolderAsync(string path)
        {
            var result = ShowTaskDialog(new TaskDialogPage()
            {
                Text = $"The target directory you selected does not exist.{Environment.NewLine}Would you like to create it and continue the extraction?",
                Heading = "Directory does not exist",
                Caption = "Extraction",
                Buttons =
                {
                    TaskDialogButton.Yes,
                    TaskDialogButton.No
                },
                Icon = TaskDialogIcon.Warning,
                SizeToContent = true
            });

            return Task.FromResult(result == TaskDialogButton.Yes);
        }

        public Task<UnsavedChangesAnswer> AskUnsavedChangesAsync(UnsavedChangesReason reason)
        {
            var save = new TaskDialogCommandLinkButton("&Save");
            var discard = new TaskDialogCommandLinkButton("&Discard");
            var action = reason switch
            {
                UnsavedChangesReason.NewImage => "creating a new image",
                UnsavedChangesReason.OpenImage => "opening another image",
                UnsavedChangesReason.CloseImage => "closing the current image",
                UnsavedChangesReason.ReloadImage => "reloading the image",
                _ => "closing TotalImage"
            };

            var result = ShowTaskDialog(new TaskDialogPage()
            {
                Text = $"Would you like to save them before {action}?",
                Heading = "You have unsaved changes",
                Caption = "Warning",
                Buttons =
                {
                    save,
                    discard,
                    TaskDialogButton.Cancel
                },
                Icon = TaskDialogIcon.Warning,
            });

            return Task.FromResult(result == save ? UnsavedChangesAnswer.Save
                : result == discard ? UnsavedChangesAnswer.Discard
                : UnsavedChangesAnswer.Cancel);
        }

        public Task ShowCannotOpenImageAsync(ImageOpenFailure failure)
        {
            const string BugReport = "If you think this is a bug, please submit a bug report (with this image included) on our GitHub repo.";
            var paragraph = Environment.NewLine + Environment.NewLine;
            var (heading, text) = failure.Problem switch
            {
                ImageOpenProblem.InvalidPath => ("Invalid path",
                    "The provided path is empty or whitespace and cannot be opened."),
                ImageOpenProblem.EmptyFile => ("File is empty",
                    $"The file \"{failure.FileName}\" appears to be empty (ie. zero bytes in size). If you downloaded or copied this file from some other location, make sure the source file is not damaged and that the transfer completed successfully.{paragraph}{BugReport}"),
                ImageOpenProblem.FileNotFound => ("File not found",
                    $"The file \"{failure.FileName}\" could not be opened because it no longer exists. It may have been moved or deleted in the mean time. Make sure the file exists and is accessible and try again.{paragraph}{BugReport}"),
                ImageOpenProblem.FileLocked => ("File is in use",
                    $"The file \"{failure.FileName}\" could not be opened because it's currently locked by another process. Close all processes using this file and try again.{paragraph}{BugReport}"),
                ImageOpenProblem.AccessDenied => ("Access denied",
                    $"The file \"{failure.FileName}\" could not be opened because access was denied. Make sure you have the required permissions and that the file is not marked as read-only.{paragraph}{BugReport}"),
                ImageOpenProblem.NoSupportedFileSystem => ("Cannot open file",
                    $"We attempted to open this file as the {failure.ContainerName} container, but found no supported partition table or any supported file systems inside, so this image cannot be loaded. " +
                    $"It's also possible this container format might not be correct for this file.{paragraph}{BugReport}"),
                ImageOpenProblem.NoPartitions => ("No partitions found",
                    $"We found a supported partition table in this image ({failure.PartitionTableName}), but no partitions, so this image cannot be loaded.{paragraph}{BugReport}"),
                ImageOpenProblem.NoSupportedPartitions => ("No supported file systems",
                    $"We found a supported partition table ({failure.PartitionTableName}) and several partitions in this image, but they all contain unsupported file systems, so this image cannot be loaded.{paragraph}{BugReport}"),
                ImageOpenProblem.UnsupportedPartition => ("Unsupported file system",
                    $"We found a supported partition table ({failure.PartitionTableName}) and one partition in this image, but it contains an unsupported file system, so this image cannot be loaded.{paragraph}{BugReport}"),
                _ => ("Cannot open file",
                    $"The file \"{failure.FileName}\" could not be opened due to the following exception:{paragraph}{failure.ErrorMessage}{paragraph}{BugReport}")
            };

            ShowTaskDialog(new TaskDialogPage()
            {
                Text = text,
                Heading = heading,
                Caption = "Error",
                Buttons =
                {
                    TaskDialogButton.OK
                },
                Icon = TaskDialogIcon.Error,
                SizeToContent = true
            });

            return Task.CompletedTask;
        }

        public Task ShowRecentImageMissingAsync(string fileName)
        {
            ShowTaskDialog(new TaskDialogPage()
            {
                Text = $"The file \"{fileName}\" could not be opened because it no longer exists. It may have been renamed, moved or deleted since it was last opened in TotalImage. It will now be removed from your recent images list.{Environment.NewLine}{Environment.NewLine}" +
                    $"If you think this is a bug, please submit a bug report (with this image included) on our GitHub repo.",
                Heading = "File not found",
                Caption = "Error",
                Buttons =
                {
                    TaskDialogButton.OK
                },
                Icon = TaskDialogIcon.Error,
                SizeToContent = true
            });

            return Task.CompletedTask;
        }

        public Task ShowFatOnlyFeatureAsync()
        {
            ShowTaskDialog(new TaskDialogPage()
            {
                Text = "This feature is currently only available for FAT file systems.",
                Heading = "Feature not available",
                Caption = "Information",
                Buttons =
                {
                    TaskDialogButton.OK
                },
                Icon = TaskDialogIcon.Information,
                DefaultButton = TaskDialogButton.OK
            });

            return Task.CompletedTask;
        }

        public Task<OverwriteAnswer> AskOverwriteAsync(string fileName)
        {
            var overwriteButton = new TaskDialogButton("&Overwrite");
            var skipButton = new TaskDialogButton("&Skip");
            var page = new TaskDialogPage()
            {
                Text = $"The file \"{fileName}\" already exists in the target directory. Do you want to overwrite it?",
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

            var result = ShowTaskDialog(page);
            var choice = result == overwriteButton ? OverwriteChoice.Overwrite
                : result == skipButton ? OverwriteChoice.Skip
                : OverwriteChoice.Cancel;

            return Task.FromResult(new OverwriteAnswer(choice, choice != OverwriteChoice.Cancel && page.Verification.Checked));
        }

        public Task<Confirmation> ConfirmDeleteAsync(DeletionSummary summary)
        {
            //Build a fancy string for showing the item count
            var (fileCount, dirCount) = (summary.FileCount, summary.DirectoryCount);
            string itemCount = "";
            if (dirCount > 0)
            {
                itemCount += $"{dirCount} director{(dirCount == 1 ? "y" : "ies")}";
                if (fileCount > 0)
                    itemCount += $" and {fileCount} file{(fileCount == 1 ? "" : "s")}";
            }
            else
            {
                itemCount += $"{fileCount} file{(fileCount == 1 ? "" : "s")}";
            }

            var total = fileCount + dirCount;
            return Task.FromResult(ShowConfirmation(new TaskDialogPage()
            {
                Text = $"Are you sure you want to delete {itemCount} occupying {summary.SizeText}?{Environment.NewLine}" +
                $"You might still be able to undo this operation later.",
                Heading = $"{total} item{(total > 1 ? "s" : "")} will be deleted",
                Caption = "Deletion",
                Buttons =
                {
                    TaskDialogButton.Yes,
                    TaskDialogButton.No
                },
                DefaultButton = TaskDialogButton.Yes,
                SizeToContent = true,
                Icon = TaskDialogIcon.Warning,
                Verification = new TaskDialogVerificationCheckBox()
                {
                    Text = "Do not ask for confirmation again"
                }
            }, clearFocus: true));
        }

        public Task<Confirmation> ConfirmDeleteFolderAsync()
        {
            return Task.FromResult(ShowConfirmation(new TaskDialogPage()
            {
                Text = $"Are you sure you want to delete this directory and all its contents?{Environment.NewLine}" +
                $"You might still be able to undo this operation later.",
                Heading = $"Directory will be deleted",
                Caption = "Warning",
                Buttons =
                {
                    TaskDialogButton.Yes,
                    TaskDialogButton.No
                },
                Icon = TaskDialogIcon.Warning,
                Verification = new TaskDialogVerificationCheckBox()
                {
                    Text = "Do not ask for confirmation again"
                }
            }, clearFocus: true));
        }

        public Task<Confirmation> ConfirmInjectAsync(int fileCount, string sizeText)
        {
            return Task.FromResult(ShowConfirmation(new TaskDialogPage()
            {
                Text = $"Are you sure you want to inject {fileCount} file{(fileCount == 1 ? "" : "s")} occupying {sizeText} into the image?",
                Heading = $"{fileCount} file{(fileCount == 1 ? "" : "s")} will be injected",
                Caption = "Injection",
                Buttons =
                {
                    TaskDialogButton.Yes,
                    TaskDialogButton.No
                },
                Icon = TaskDialogIcon.Warning,
                Verification = new TaskDialogVerificationCheckBox()
                {
                    Text = "Do not ask for confirmation again"
                }
            }, clearFocus: false));
        }

        // A Yes/No question with a "do not ask again" check box. Clearing the focus keeps the
        // Enter key that answers it from reaching the file list or the folder tree.
        private Confirmation ShowConfirmation(TaskDialogPage page, bool clearFocus)
        {
            if (clearFocus && owner.Window is ContainerControl window)
                window.ActiveControl = null;

            var result = ShowTaskDialog(page);
            return new Confirmation(result == TaskDialogButton.Yes, page.Verification!.Checked);
        }

        private TaskDialogButton ShowTaskDialog(TaskDialogPage page)
            => owner.Window is null ? TaskDialog.ShowDialog(page) : TaskDialog.ShowDialog(owner.Window, page);
    }
}
