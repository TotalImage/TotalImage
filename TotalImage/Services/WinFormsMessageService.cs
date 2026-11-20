using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        private TaskDialogButton ShowTaskDialog(TaskDialogPage page)
            => owner.Window is null ? TaskDialog.ShowDialog(page) : TaskDialog.ShowDialog(owner.Window, page);
    }
}
