using System;
using System.Threading.Tasks;
using TotalImage.ViewModels.Extraction;

namespace TotalImage.ViewModels.Services;

/// <summary>
/// Simple questions and messages. There is one method per kind of question; the wording and
/// the buttons belong to each UI.
/// </summary>
public interface IMessageService
{
    /// <summary>
    /// Warns that formatting erases all data in the current partition and asks whether to continue.
    /// </summary>
    Task<bool> ConfirmFormatAsync();

    /// <summary>
    /// Tells the user that the list of recently opened images was cleared.
    /// </summary>
    Task ShowRecentImagesClearedAsync();

    /// <summary>
    /// Warns that all settings will be reset to their defaults and asks whether to continue.
    /// </summary>
    Task<bool> ConfirmResetSettingsAsync();

    /// <summary>
    /// Tells the user that all settings were reset to their defaults.
    /// </summary>
    Task ShowSettingsResetAsync();

    /// <summary>
    /// Warns that other instances are running and may be using the temporary folder, and asks
    /// whether to clear it anyway.
    /// </summary>
    Task<bool> ConfirmClearTempFolderWithOtherInstancesAsync();

    /// <summary>
    /// Tells the user that the temporary folder was cleared.
    /// </summary>
    Task ShowTempFolderClearedAsync();

    /// <summary>
    /// Tells the user that the temporary folder could not be cleared.
    /// </summary>
    Task ShowTempFolderClearFailedAsync();

    /// <summary>
    /// Explains that a partition can't be loaded because it or its file system is not supported.
    /// </summary>
    Task ShowPartitionNotSupportedAsync();

    /// <summary>
    /// Warns that the new image gets no BIOS Parameter Block (BPB), so some programs and operating
    /// systems may not recognize it, and asks whether to continue.
    /// </summary>
    Task<bool> ConfirmCreateWithoutBpbAsync();

    /// <summary>
    /// Warns that the image hashes are still being calculated and won't be exported, and asks
    /// whether to export anyway.
    /// </summary>
    Task<bool> ConfirmExportWithoutHashesAsync();

    /// <summary>
    /// Tells the user that something was exported to a file.
    /// </summary>
    Task ShowExportSucceededAsync(string path);

    /// <summary>
    /// Tells the user that an operation failed.
    /// </summary>
    /// <param name="operation">What was being done, as a short phrase such as
    /// <c>Exporting to "C:\boot.bin"</c>; the UI builds its message around it.</param>
    /// <param name="exception">The reason it failed.</param>
    Task ShowErrorAsync(string operation, Exception exception);

    /// <summary>
    /// Says that the folder chosen for extraction does not exist and asks whether to create it.
    /// </summary>
    Task<bool> ConfirmCreateExtractFolderAsync(string path);

    /// <summary>
    /// Says that the open image has unsaved changes and asks whether to save them before
    /// going on.
    /// </summary>
    /// <param name="reason">What is about to happen to the image.</param>
    Task<UnsavedChangesAnswer> AskUnsavedChangesAsync(UnsavedChangesReason reason);

    /// <summary>
    /// Explains why an image could not be opened.
    /// </summary>
    Task ShowCannotOpenImageAsync(ImageOpenFailure failure);

    /// <summary>
    /// Says that a file in the recent images list no longer exists and will be removed from
    /// the list.
    /// </summary>
    /// <param name="fileName">The file name, without its folder.</param>
    Task ShowRecentImageMissingAsync(string fileName);

    /// <summary>
    /// Says that the feature is only available for FAT file systems so far.
    /// </summary>
    Task ShowFatOnlyFeatureAsync();

    /// <summary>
    /// Says that a file being extracted exists already in the target folder and asks whether to
    /// overwrite it, skip it or stop the extraction, and whether to do the same for every other
    /// such file.
    /// </summary>
    /// <param name="fileName">The file name, without its folder.</param>
    Task<OverwriteAnswer> AskOverwriteAsync(string fileName);

    /// <summary>
    /// Asks whether to delete entries selected in the file list, and whether to stop asking.
    /// </summary>
    Task<Confirmation> ConfirmDeleteAsync(DeletionSummary summary);

    /// <summary>
    /// Asks whether to delete the folder selected in the folder tree and everything in it, and
    /// whether to stop asking.
    /// </summary>
    Task<Confirmation> ConfirmDeleteFolderAsync();

    /// <summary>
    /// Asks whether to add files of the computer to the image, and whether to stop asking.
    /// </summary>
    /// <param name="fileCount">The number of files.</param>
    /// <param name="sizeText">Their total size, formatted.</param>
    Task<Confirmation> ConfirmInjectAsync(int fileCount, string sizeText);
}

/// <summary>
/// The answer to a question that asks for confirmation and offers not to ask again.
/// </summary>
/// <param name="Confirmed">Whether to go on.</param>
/// <param name="DontAskAgain">Whether the user asked not to be asked again (whatever the answer).</param>
public sealed record Confirmation(bool Confirmed, bool DontAskAgain = false);

/// <summary>
/// What is about to be deleted, for <see cref="IMessageService.ConfirmDeleteAsync"/>.
/// </summary>
/// <param name="FileCount">The number of files.</param>
/// <param name="DirectoryCount">The number of folders.</param>
/// <param name="SizeText">The total size, including everything in the folders, formatted.</param>
public sealed record DeletionSummary(int FileCount, int DirectoryCount, string SizeText);

/// <summary>
/// The answer to <see cref="IMessageService.AskUnsavedChangesAsync"/>.
/// </summary>
public enum UnsavedChangesAnswer
{
    /// <summary>
    /// Save the changes, then go on.
    /// </summary>
    Save,

    /// <summary>
    /// Go on without saving; the changes are lost.
    /// </summary>
    Discard,

    /// <summary>
    /// Don't go on.
    /// </summary>
    Cancel
}

/// <summary>
/// What is about to happen to an image with unsaved changes.
/// </summary>
public enum UnsavedChangesReason
{
    NewImage,
    OpenImage,
    CloseImage,
    ReloadImage,
    ExitApplication
}

/// <summary>
/// Why an image could not be opened.
/// </summary>
public enum ImageOpenProblem
{
    /// <summary>
    /// The path is empty or white space.
    /// </summary>
    InvalidPath,

    /// <summary>
    /// The file is zero bytes long.
    /// </summary>
    EmptyFile,

    FileNotFound,

    /// <summary>
    /// Another process has locked the file.
    /// </summary>
    FileLocked,

    AccessDenied,

    /// <summary>
    /// Reading the file failed; <see cref="ImageOpenFailure.ErrorMessage"/> says why.
    /// </summary>
    ReadFailed,

    /// <summary>
    /// The image has no partition table and no supported file system.
    /// </summary>
    NoSupportedFileSystem,

    /// <summary>
    /// The partition table has no partitions.
    /// </summary>
    NoPartitions,

    /// <summary>
    /// The partition table has several partitions, but none with a supported file system.
    /// </summary>
    NoSupportedPartitions,

    /// <summary>
    /// The partition table has one partition, and its file system is not supported.
    /// </summary>
    UnsupportedPartition
}

/// <summary>
/// Describes why an image could not be opened.
/// </summary>
/// <param name="Problem">What went wrong.</param>
/// <param name="FileName">The image's file name, without its folder.</param>
/// <param name="ErrorMessage">The error, for <see cref="ImageOpenProblem.ReadFailed"/>.</param>
/// <param name="ContainerName">The container format the file was opened as, for <see cref="ImageOpenProblem.NoSupportedFileSystem"/>.</param>
/// <param name="PartitionTableName">The partition table found, for the partition problems.</param>
public sealed record ImageOpenFailure(
    ImageOpenProblem Problem,
    string FileName,
    string? ErrorMessage = null,
    string? ContainerName = null,
    string? PartitionTableName = null);
