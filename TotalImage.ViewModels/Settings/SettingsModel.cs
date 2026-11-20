using System;
using System.Collections.Generic;
using TotalImage.ViewModels.Extraction;
using TotalImage.ViewModels.Formatting;

namespace TotalImage.ViewModels.Settings;

/// <summary>
/// The settings model for storing the program's various settings and options.
/// It is serialized to settings.json, so property names must not change.
/// </summary>
public class SettingsModel
{
    /// <summary>
    /// The view type of the file list in the main window.
    /// </summary>
    public ViewMode FilesView { get; set; } = ViewMode.Details;
    /// <summary>
    /// The sorting column of the file list in the main window.
    /// </summary>
    public int FilesSortingColumn { get; set; } = 0;
    /// <summary>
    /// Sort order of the file list in the main window.
    /// </summary>
    public SortDirection FilesSortOrder { get; set; } = SortDirection.Ascending;
    /// <summary>
    /// List of recently opened images in the File menu.
    /// </summary>
    public List<string> RecentImages { get; set; } = new List<string>();
    /// <summary>
    /// Parse hidden file system objects in all directories and show them in the directory tree and file list.
    /// </summary>
    /// <remarks>May not apply to all supported file systems.</remarks>
    public bool ShowHiddenItems { get; set; } = true;
    /// <summary>
    /// Show the command bar in the main window.
    /// </summary>
    public bool ShowCommandBar { get; set; } = true;
    /// <summary>
    /// Show the directory tree in the main window.
    /// </summary>
    public bool ShowDirectoryTree { get; set; } = true;
    /// <summary>
    /// Show the status bar in the main window.
    /// </summary>
    public bool ShowStatusBar { get; set; } = true;
    /// <summary>
    /// The size unit that will be used to display all sizes in the UI.
    /// </summary>
    public SizeUnit SizeUnit { get; set; } = SizeUnit.Bytes;
    /// <summary>
    /// The default path for extraction that is used unless the user manually selects a different path.
    /// </summary>
    public string DefaultExtractPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    /// <summary>
    /// The default directory extraction mode that is used unless the user manually selects a different mode.
    /// </summary>
    public DirectoryExtractionMode DefaultDirectoryExtractionMode { get; set; } = DirectoryExtractionMode.Preserve;
    /// <summary>
    /// Always ask for extraction options before extracting.
    /// </summary>
    public bool ExtractAlwaysAsk { get; set; } = true;
    /// <summary>
    /// Open the target folder after extraction.
    /// </summary>
    public bool OpenFolderAfterExtract { get; set; } = true;
    /// <summary>
    /// Preserve original file system object timestamps during extraction.
    /// </summary>
    /// <remarks>May not apply to all supported file systems.</remarks>
    public bool ExtractPreserveDates { get; set; } = true;
    /// <summary>
    /// Preserve original file system object attributes during extraction.
    /// </summary>
    /// <remarks>May not apply to all supported file systems.</remarks>
    public bool ExtractPreserveAttributes { get; set; } = false;
    /// <summary>
    /// Obtain file type information such as icon and name from the operating system instead of using generic defaults.
    /// </summary>
    /// <remarks>May slightly increase the loading time for large images on slower systems.</remarks>
    public bool QueryShellForFileTypeInfo { get; set; } = true;
    /// <summary>
    /// Confirm injecting objects into the image.
    /// </summary>
    public bool ConfirmInjection { get; set; } = true;
    /// <summary>
    /// Confirm deleting objects from the image.
    /// </summary>
    public bool ConfirmDeletion { get; set; } = true;
    /// <summary>
    /// Confirm overwriting existing objects in the target folder during extraction.
    /// </summary>
    public bool ConfirmOverwriteExtraction { get; set; } = true;
    /// <summary>
    /// Automatically increment the last used file name when saving images if the filename ends with a numeric character.
    /// </summary>
    public bool AutoIncrementFilename { get; set; } = true;
    /// <summary>
    /// The threshold image size for using memory-mapping when opening images.
    /// </summary>
    /// <remarks>Default is 100 MiB (104.86 MB).</remarks>
    public long MemoryMappingThreshold { get; set; } = 104857600; //100 MiB
    /// <summary>
    /// Calculate and display the total size of every directory shown in the file list.
    /// </summary>
    public bool FileListShowDirSize { get; set; } = false;
    /// <summary>
    /// Color mode for the application. Choosing System will follow the system theme, Classic and Dark force the respective theme regardless of the system theme.
    /// </summary>
    public ColorTheme ColorMode { get; set; } = ColorTheme.System;
}
