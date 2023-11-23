using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.Json;
using System.IO;
using System.Drawing;
using System.Linq;

namespace TotalImage
{
    /// <summary>
    /// Singleton thread-safe with a double check lock class for managing the program's settings and UI state.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// The UI state model for storing values related to the state of the main window and its child controls.
        /// </summary>
        public class UIStateModel
        {
            /// <summary>
            /// Location of the splitter between the directory tree and the file list. Only relevant if the directory tree is enabled.
            /// </summary>
            public int SplitterDistance { get; set; } = 280;
            /// <summary>
            /// Size of the main window.
            /// </summary>
            public Size WindowSize { get; set; } = new Size(1000, 700);
            /// <summary>
            /// Position of the main window on the screen.
            /// </summary>
            public Point WindowPosition { get; set; } = new Point((Screen.PrimaryScreen.Bounds.Width - 1000) / 2, (Screen.PrimaryScreen.Bounds.Height - 700) / 2);
            /// <summary>
            /// State of the main window.
            /// </summary>
            public FormWindowState WindowState { get; set; } = FormWindowState.Normal;
            /// <summary>
            /// Order of the columns in the file list in the main window.
            /// </summary>
            public List<int> MWColumnOrder { get; set; } = new List<int>(new[] { 0, 1, 2, 3, 4 });
            /// <summary>
            /// Width of each column in the file list in the main window.
            /// </summary>
            public List<int> MWColumnWidth { get; set; } = new List<int>(new[] { 150, 150, 150, 150, 85 });
        }

        /// <summary>
        /// The settings model for storing the program's various settings and options.
        /// </summary>
        public class SettingsModel
        {
            /// <summary>
            /// The view type of the file list in the main window.
            /// </summary>
            public View FilesView { get; set; } = View.Details;
            /// <summary>
            /// The sorting column of the file list in the main window.
            /// </summary>
            public int FilesSortingColumn { get; set; } = 0;
            /// <summary>
            /// Sort order of the file list in the main window.
            /// </summary>
            public SortOrder FilesSortOrder { get; set; } = SortOrder.Ascending;
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
            /// Parse deleted file system objects in all directories and show them in the directory tree and file list.
            /// </summary>
            /// <remarks>May not apply to all supported file systems.</remarks>
            public bool ShowDeletedItems { get; set; } = false;
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
            /// Obtain file type information such as icon and name from Windows instead of using generic defaults.
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
        }

        /// <summary>
        /// The currently used settings.
        /// </summary>
        public static SettingsModel CurrentSettings { get; private set; }

        /// <summary>
        /// The currently used UI state.
        /// </summary>
        public static UIStateModel CurrentUIState { get; private set; }

        /// <summary>
        /// Path of the folder where temporary files are stored.
        /// </summary>
        public static readonly string TempDir = Path.Combine(Path.GetTempPath(), "TotalImage");

        private static readonly string SettingsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TotalImage");
        private static readonly string SettingsFile = Path.Combine(SettingsDir, "settings.json");
        private static readonly string UIStateFile = Path.Combine(SettingsDir, "uistate.json");
        private static FileSystemWatcher settingsWatcher;

        //Loads all settings from permanent storage (settings.json)
        static Settings()
        {
            //If the file doesn't exist (yet), load default values and create the file
            if (!File.Exists(SettingsFile))
            {
                //Also create the directory if even that doesn't exist (yet)
                if (!Directory.Exists(SettingsDir))
                {
                    Directory.CreateDirectory(SettingsDir);
                }

                CurrentSettings = new SettingsModel();
                Save();
            }
            else
            {
                var json = File.ReadAllText(SettingsFile);
                var settings = JsonSerializer.Deserialize<SettingsModel>(json);

                if (settings is not null)
                    CurrentSettings = settings;
                else
                {
                    CurrentSettings = new SettingsModel();
                    Save();
                }
            }

            settingsWatcher = new(SettingsDir, "settings.json");
            settingsWatcher.EnableRaisingEvents = true;
            settingsWatcher.NotifyFilter = NotifyFilters.LastWrite;
            settingsWatcher.Changed += settingsWatcher_Changed;

            if (!File.Exists(UIStateFile))
            {
                CurrentUIState = new UIStateModel();
                SaveUIState();
            }
            else
            {
                var json = File.ReadAllText(UIStateFile);
                var state = JsonSerializer.Deserialize<UIStateModel>(json);

                if (state is not null)
                    CurrentUIState = state;
                else
                {
                    CurrentUIState = new UIStateModel();
                    SaveUIState();
                }
            }
        }

        // Handles the settings file being changed behind our back
        private static void settingsWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            try
            {
                var json = File.ReadAllText(SettingsFile);
                var settings = JsonSerializer.Deserialize<SettingsModel>(json);

                if (settings is not null)
                {
                    // We could just reload the entire settings, however:
                    // 1. The loaded settings could include changes to UI options.
                    // 2. It is not nice to change UI options out of the blue.
                    // 3. We are not on the UI thread anyway.

                    CurrentSettings.OpenFolderAfterExtract = settings.OpenFolderAfterExtract;
                    CurrentSettings.DefaultDirectoryExtractionMode = settings.DefaultDirectoryExtractionMode;
                    CurrentSettings.DefaultExtractPath = settings.DefaultExtractPath;
                    CurrentSettings.RecentImages = settings.RecentImages;
                    CurrentSettings.ExtractPreserveAttributes = settings.ExtractPreserveAttributes;
                    CurrentSettings.ExtractPreserveDates = settings.ExtractPreserveDates;
                    CurrentSettings.AutoIncrementFilename = settings.AutoIncrementFilename;
                    CurrentSettings.ConfirmDeletion = settings.ConfirmDeletion;
                    CurrentSettings.ConfirmInjection = settings.ConfirmInjection;
                    CurrentSettings.ConfirmOverwriteExtraction = settings.ConfirmOverwriteExtraction;
                    CurrentSettings.MemoryMappingThreshold = settings.MemoryMappingThreshold;
                    CurrentSettings.FileListShowDirSize = settings.FileListShowDirSize;
                }
            }
            catch (IOException)
            {
                // We can't read the file. Carry on.
            }
        }

        /// <summary>
        /// Reloads the settings from the settings.json file. If the file doesn't exist or can't be used, the default values are loaded instead.
        /// </summary>
        public static void ReloadSettings()
        {
            var json = File.ReadAllText(SettingsFile);
            var settings = JsonSerializer.Deserialize<SettingsModel>(json);

            if (settings is not null)
                CurrentSettings = settings;
            else
            {
                // for some reason we didn't read any settings
                // what do we do? let's just load default settings for now
                CurrentSettings = new SettingsModel();
                Save();
            }
        }

        /// <summary>
        /// Reloads the UI state from the uistate.json file. If the file doesn't exist or can't be used, the default values are loaded instead.
        /// </summary>
        public static void ReloadUIState()
        {
            var json = File.ReadAllText(UIStateFile);
            var state = JsonSerializer.Deserialize<UIStateModel>(json);

            if (state is not null)
                CurrentUIState = state;
            else
            {
                CurrentUIState = new UIStateModel();
                SaveUIState();
            }
        }

        /// <summary>
        /// Resets all settings to their default values.
        /// </summary>
        public static void LoadDefaults()
        {
            //Set all settings to a default value here
            CurrentSettings.OpenFolderAfterExtract = true;
            CurrentSettings.DefaultDirectoryExtractionMode = DirectoryExtractionMode.Preserve;
            CurrentSettings.DefaultExtractPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            CurrentSettings.SizeUnit = SizeUnit.Bytes;
            CurrentSettings.ShowCommandBar = true;
            CurrentSettings.ShowDeletedItems = false;
            CurrentSettings.ShowDirectoryTree = true;
            CurrentSettings.ShowHiddenItems = true;
            CurrentSettings.ShowStatusBar = true;
            CurrentSettings.FilesSortingColumn = 0;
            CurrentSettings.FilesSortOrder = SortOrder.Ascending;
            CurrentSettings.FilesView = View.Details;
            CurrentSettings.ExtractPreserveAttributes = false;
            CurrentSettings.ExtractPreserveDates = true;
            CurrentSettings.ExtractAlwaysAsk = true;
            CurrentSettings.QueryShellForFileTypeInfo = true;
            CurrentSettings.AutoIncrementFilename = true;
            CurrentSettings.ConfirmDeletion = true;
            CurrentSettings.ConfirmInjection = true;
            CurrentSettings.ConfirmOverwriteExtraction = true;
            CurrentSettings.MemoryMappingThreshold = 1048576;
            CurrentSettings.FileListShowDirSize = false;
        }

        /// <summary>
        /// Saves all settings to file settings.json.
        /// </summary>
        public static void Save()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(CurrentSettings, options);

            //Just in case...
            if (!Directory.Exists(SettingsDir))
            {
                Directory.CreateDirectory(SettingsDir);
            }

            // Let's not make us reload the very settings we're about to save
            if (settingsWatcher is not null)
            {
                settingsWatcher.EnableRaisingEvents = false;
            }

            try
            {
                File.WriteAllText(SettingsFile, json);
            }
            catch (IOException)
            {
                //We can't write to the file, probably because it's opened by another instance, so let's just silently give up
            }

            if (settingsWatcher is not null)
            {
                settingsWatcher.EnableRaisingEvents = true;
            }
        }

        /// <summary>
        /// Saves the current UI state to file uistate.json.
        /// </summary>
        public static void SaveUIState()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(CurrentUIState, options);

            if (!Directory.Exists(SettingsDir))
            {
                Directory.CreateDirectory(SettingsDir);
            }

            try
            {
                File.WriteAllText(UIStateFile, json);
            }
            catch (IOException)
            {
                //We can't write to the file, probably because it's opened by another instance, so let's just silently give up
            }
        }

        /// <summary>
        /// Adds a single item to the recent images list. If the number of items exceedes 10, the oldest item is removed.
        /// If the provided item already exists in the list, it is removed from its old position and readded at the beginning.
        /// </summary>
        /// <param name="path">The file path to add to the list.</param>
        public static void AddRecentImage(string path)
        {
            //This prevents duplicate entries by removing the old entry first - the new one is then put at the start of the list
            if (CurrentSettings.RecentImages.Any())
            {
                if (CurrentSettings.RecentImages.LastIndexOf(path) > -1)
                    CurrentSettings.RecentImages.RemoveAt(CurrentSettings.RecentImages.LastIndexOf(path));
            }

            //10 entries seems like a reasonable number
            if (CurrentSettings.RecentImages.Count >= 10)
                CurrentSettings.RecentImages.RemoveAt(0);
            CurrentSettings.RecentImages.Add(path);
        }

        /// <summary>
        /// Removes a single item from the recent images list.
        /// </summary>
        /// <param name="path">The file path to be removed from the list.</param>
        public static void RemoveRecentImage(string path)
        {
            CurrentSettings.RecentImages.Remove(path);
        }

        /// <summary>
        /// Removed all items from the recent images list.
        /// </summary>
        public static void ClearRecentImages()
        {
            CurrentSettings.RecentImages.Clear();
        }

        /// <summary>
        /// Checks all the items in the recent images list and removes any that no longer exist.
        /// </summary>
        /// <remarks>PopulateRecentList() method of frmMain should be called after this method to sync the menu items with the new state of the list.</remarks>
        public static void CheckRecentImages()
        {
            for(int i = CurrentSettings.RecentImages.Count - 1; i >= 0; i--)
            {
                if (!File.Exists(CurrentSettings.RecentImages[i]))
                {
                    RemoveRecentImage(CurrentSettings.RecentImages[i]);
                }
            }
        }
    }
}
