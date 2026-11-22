using System;
using System.IO;
using System.Threading.Tasks;
using TotalImage.Avalonia.Dialogs;
using TotalImage.ViewModels.Extraction;
using TotalImage.ViewModels.Services;

namespace TotalImage.Avalonia.Services;

/// <summary>
/// Shows every message and question in a <see cref="MessageWindow"/> owned by the scope's window.
/// The wording follows the WinForms UI.
/// </summary>
public sealed class AvaloniaMessageService(WindowOwner owner) : IMessageService
{
    private const string BugReportText = "If you think this is a bug, please submit a bug report on our GitHub repo.";
    private const string ImageBugReportText = "If you think this is a bug, please submit a bug report (with this image included) on our GitHub repo.";
    private const string DontAskAgainText = "Do not ask for confirmation again";

    private static readonly string Paragraph = Environment.NewLine + Environment.NewLine;

    private static readonly MessageButton Yes = new("_Yes", IsDefault: true);
    private static readonly MessageButton No = new("_No", IsCancel: true);
    private static readonly MessageButton YesNotDefault = new("_Yes");
    private static readonly MessageButton NoDefault = new("_No", IsDefault: true, IsCancel: true);

    public async Task<bool> ConfirmFormatAsync()
        => await AskYesNoAsync("WARNING: Formatting will erase all data in the current partition!",
            "Are you sure you want to continue?", "Warning");

    public Task ShowRecentImagesClearedAsync()
        => ShowAsync("Recent images cleared", "List of recently opened images has been successfully cleared.", "Success", MessageIcon.Information);

    public async Task<bool> ConfirmResetSettingsAsync()
        => await AskYesNoAsync("Are you sure you want to continue?", "All settings will be reset to their default values.", "Warning", defaultNo: true);

    public Task ShowSettingsResetAsync()
        => ShowAsync("Settings reset", "All settings were successfully reset to their default values.", "Success", MessageIcon.Information);

    public async Task<bool> ConfirmClearTempFolderWithOtherInstancesAsync()
        => await AskYesNoAsync("Multiple instances are running",
            "More than one instance of TotalImage is currently running. Attempting to clear the temporary folder while other instances are " +
            $"using it can cause unpredictable behavior.{Paragraph}Are you sure you want to continue?", "Warning", defaultNo: true);

    public Task ShowTempFolderClearedAsync()
        => ShowAsync("Temporary folder cleared", "Temporary folder has been successfully cleared.", "Success", MessageIcon.Information);

    public Task ShowTempFolderClearFailedAsync()
        => ShowAsync("Cannot clear temporary folder",
            "Temporary folder could not be cleared because it is in use by another process or inaccessible.", "Error", MessageIcon.Error);

    public Task ShowPartitionNotSupportedAsync()
        => ShowAsync("Unsupported partition type",
            "This partition cannot be loaded because it or the file system contained within is not supported. Select a supported partition to load.",
            "Information", MessageIcon.Information);

    public async Task<bool> ConfirmCreateWithoutBpbAsync()
        => await AskYesNoAsync("No BIOS Parameter Block selected",
            "You chose not to create a BIOS Parameter Block (BPB) for this image. Some programs and operating systems may not recognize the disk because of this." +
            $"{Paragraph}Are you sure you want to continue?", "Warning");

    public async Task<bool> ConfirmExportWithoutHashesAsync()
    {
        var continueButton = new MessageButton("C_ontinue", IsDefault: true);
        var result = await ShowAsync(new MessageOptions("Hash calculations in progress",
            "File hashes are still being calculated and won't be included in the exported file unless you wait for the calculations to complete first.",
            "Warning", MessageIcon.Warning, [new MessageButton("Cancel", IsCancel: true), continueButton]));
        return result.Button == continueButton;
    }

    public Task ShowExportSucceededAsync(string path)
        => ShowAsync("Export successful", $"The data was successfully exported to {path}", "Success", MessageIcon.Information);

    public Task ShowErrorAsync(string operation, Exception exception)
    {
        var (heading, reason) = exception switch
        {
            UnauthorizedAccessException => ("Access denied",
                "because you do not have the necessary permissions. Make sure you have the necessary permissions and try again."),
            IOException when (exception.HResult & 0x0000FFFF) == 32 => ("Access denied",
                "because the file is locked by another process. Close all processes locking this file and try again."),
            _ => ("Operation failed", $"with the following error: {exception.Message}")
        };

        return ShowAsync(heading, $"{operation} failed {reason}{Paragraph}{BugReportText}", "Error", MessageIcon.Error);
    }

    public async Task<bool> ConfirmCreateExtractFolderAsync(string path)
        => await AskYesNoAsync("Directory does not exist",
            $"The target directory you selected does not exist.{Environment.NewLine}Would you like to create it and continue the extraction?", "Extraction");

    public async Task<UnsavedChangesAnswer> AskUnsavedChangesAsync(UnsavedChangesReason reason)
    {
        var save = new MessageButton("_Save", IsDefault: true);
        var discard = new MessageButton("_Discard");
        var action = reason switch
        {
            UnsavedChangesReason.NewImage => "creating a new image",
            UnsavedChangesReason.OpenImage => "opening another image",
            UnsavedChangesReason.CloseImage => "closing the current image",
            UnsavedChangesReason.ReloadImage => "reloading the image",
            _ => "closing TotalImage"
        };

        var result = await ShowAsync(new MessageOptions("You have unsaved changes", $"Would you like to save them before {action}?",
            "Warning", MessageIcon.Warning, [save, discard, new MessageButton("Cancel", IsCancel: true)]));
        return result.Button == save ? UnsavedChangesAnswer.Save
            : result.Button == discard ? UnsavedChangesAnswer.Discard
            : UnsavedChangesAnswer.Cancel;
    }

    public Task ShowCannotOpenImageAsync(ImageOpenFailure failure)
    {
        var (heading, text) = failure.Problem switch
        {
            ImageOpenProblem.InvalidPath => ("Invalid path",
                "The provided path is empty or whitespace and cannot be opened."),
            ImageOpenProblem.EmptyFile => ("File is empty",
                $"The file \"{failure.FileName}\" appears to be empty (ie. zero bytes in size). If you downloaded or copied this file from some other location, make sure the source file is not damaged and that the transfer completed successfully.{Paragraph}{ImageBugReportText}"),
            ImageOpenProblem.FileNotFound => ("File not found",
                $"The file \"{failure.FileName}\" could not be opened because it no longer exists. It may have been moved or deleted in the mean time. Make sure the file exists and is accessible and try again.{Paragraph}{ImageBugReportText}"),
            ImageOpenProblem.FileLocked => ("File is in use",
                $"The file \"{failure.FileName}\" could not be opened because it's currently locked by another process. Close all processes using this file and try again.{Paragraph}{ImageBugReportText}"),
            ImageOpenProblem.AccessDenied => ("Access denied",
                $"The file \"{failure.FileName}\" could not be opened because access was denied. Make sure you have the required permissions and that the file is not marked as read-only.{Paragraph}{ImageBugReportText}"),
            ImageOpenProblem.NoSupportedFileSystem => ("Cannot open file",
                $"We attempted to open this file as the {failure.ContainerName} container, but found no supported partition table or any supported file systems inside, so this image cannot be loaded. " +
                $"It's also possible this container format might not be correct for this file.{Paragraph}{ImageBugReportText}"),
            ImageOpenProblem.NoPartitions => ("No partitions found",
                $"We found a supported partition table in this image ({failure.PartitionTableName}), but no partitions, so this image cannot be loaded.{Paragraph}{ImageBugReportText}"),
            ImageOpenProblem.NoSupportedPartitions => ("No supported file systems",
                $"We found a supported partition table ({failure.PartitionTableName}) and several partitions in this image, but they all contain unsupported file systems, so this image cannot be loaded.{Paragraph}{ImageBugReportText}"),
            ImageOpenProblem.UnsupportedPartition => ("Unsupported file system",
                $"We found a supported partition table ({failure.PartitionTableName}) and one partition in this image, but it contains an unsupported file system, so this image cannot be loaded.{Paragraph}{ImageBugReportText}"),
            _ => ("Cannot open file",
                $"The file \"{failure.FileName}\" could not be opened due to the following exception:{Paragraph}{failure.ErrorMessage}{Paragraph}{ImageBugReportText}")
        };

        return ShowAsync(heading, text, "Error", MessageIcon.Error);
    }

    public Task ShowRecentImageMissingAsync(string fileName)
        => ShowAsync("File not found",
            $"The file \"{fileName}\" could not be opened because it no longer exists. It may have been renamed, moved or deleted since it was last opened in TotalImage. " +
            $"It will now be removed from your recent images list.{Paragraph}{ImageBugReportText}", "Error", MessageIcon.Error);

    public Task ShowFatOnlyFeatureAsync()
        => ShowAsync("Feature not available", "This feature is currently only available for FAT file systems.", "Information", MessageIcon.Information);

    public async Task<OverwriteAnswer> AskOverwriteAsync(string fileName)
    {
        var overwrite = new MessageButton("_Overwrite");
        var skip = new MessageButton("_Skip");
        var result = await ShowAsync(new MessageOptions("File already exists",
            $"The file \"{fileName}\" already exists in the target directory. Do you want to overwrite it?",
            "Warning", MessageIcon.Warning, [overwrite, skip, new MessageButton("Cancel", IsDefault: true, IsCancel: true)],
            "Do this for all currently extracted files"));

        var choice = result.Button == overwrite ? OverwriteChoice.Overwrite
            : result.Button == skip ? OverwriteChoice.Skip
            : OverwriteChoice.Cancel;
        return new OverwriteAnswer(choice, choice != OverwriteChoice.Cancel && result.IsChecked);
    }

    public Task<Confirmation> ConfirmDeleteAsync(DeletionSummary summary)
    {
        //Build a fancy string for showing the item count
        var (fileCount, dirCount) = (summary.FileCount, summary.DirectoryCount);
        string itemCount;
        if (dirCount > 0)
        {
            itemCount = $"{dirCount} director{(dirCount == 1 ? "y" : "ies")}";
            if (fileCount > 0)
                itemCount += $" and {fileCount} file{(fileCount == 1 ? "" : "s")}";
        }
        else
        {
            itemCount = $"{fileCount} file{(fileCount == 1 ? "" : "s")}";
        }

        var total = fileCount + dirCount;
        return ConfirmAsync($"{total} item{(total > 1 ? "s" : "")} will be deleted",
            $"Are you sure you want to delete {itemCount} occupying {summary.SizeText}?{Environment.NewLine}You might still be able to undo this operation later.",
            "Deletion");
    }

    public Task<Confirmation> ConfirmDeleteFolderAsync()
        => ConfirmAsync("Directory will be deleted",
            $"Are you sure you want to delete this directory and all its contents?{Environment.NewLine}You might still be able to undo this operation later.",
            "Warning");

    public Task<Confirmation> ConfirmInjectAsync(int fileCount, string sizeText)
    {
        var files = $"{fileCount} file{(fileCount == 1 ? "" : "s")}";
        return ConfirmAsync($"{files} will be injected", $"Are you sure you want to inject {files} occupying {sizeText} into the image?", "Injection");
    }

    // A Yes/No question with a "do not ask again" check box.
    private async Task<Confirmation> ConfirmAsync(string heading, string text, string caption)
    {
        var result = await ShowAsync(new MessageOptions(heading, text, caption, MessageIcon.Warning, [Yes, No], DontAskAgainText));
        return new Confirmation(result.Button == Yes, result.IsChecked);
    }

    private async Task<bool> AskYesNoAsync(string heading, string text, string caption, bool defaultNo = false)
    {
        var yes = defaultNo ? YesNotDefault : Yes;
        var result = await ShowAsync(new MessageOptions(heading, text, caption, MessageIcon.Warning, [yes, defaultNo ? NoDefault : No]));
        return result.Button == yes;
    }

    private Task ShowAsync(string heading, string text, string caption, MessageIcon icon)
        => ShowAsync(new MessageOptions(heading, text, caption, icon, [MessageButton.Ok]));

    private Task<MessageResult> ShowAsync(MessageOptions options) => MessageWindow.ShowAsync(owner.Window, options);
}
