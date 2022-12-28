using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.Json;
using System.IO;
using System.Drawing;

namespace TotalImage
{
    //This class is a singleton, thread-safe with a double check lock
    public static class Settings
    {
        public class UIStateModel
        {
            public int SplitterDistance { get; set; } = 280;
            public Size WindowSize { get; set; } = new Size(1000, 700);
            public Point WindowPosition { get; set; } = new Point((Screen.PrimaryScreen.Bounds.Width - 1000) / 2, (Screen.PrimaryScreen.Bounds.Height - 700) / 2);
            public FormWindowState WindowState { get; set; } = FormWindowState.Normal;
            public List<int> MWColumnOrder { get; set; } = new List<int>(new[] { 0, 1, 2, 3, 4 });
            public List<int> MWColumnWidth { get; set; } = new List<int>(new[] { 150, 150, 150, 150, 85 });
        }

        public class SettingsModel
        {
            public View FilesView { get; set; } = View.Details;
            public int FilesSortingColumn { get; set; } = 0;
            public SortOrder FilesSortOrder { get; set; } = SortOrder.Ascending;
            public List<string> RecentImages { get; set; } = new List<string>();
            public bool ShowHiddenItems { get; set; } = true;
            public bool ShowDeletedItems { get; set; } = false;
            public bool ShowCommandBar { get; set; } = true;
            public bool ShowDirectoryTree { get; set; } = true;
            public bool ShowStatusBar { get; set; } = true;
            public SizeUnit SizeUnit { get; set; } = SizeUnit.Bytes;
            public string DefaultExtractPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            public DirectoryExtractionMode DefaultDirectoryExtractionMode { get; set; } = DirectoryExtractionMode.Preserve;
            public bool ExtractAlwaysAsk { get; set; } = true;
            public bool OpenFolderAfterExtract { get; set; } = true;
            public bool ExtractPreserveDates { get; set; } = true;
            public bool ExtractPreserveAttributes { get; set; } = false;
            public bool QueryShellForFileTypeInfo { get; set; } = true;
            public bool ConfirmInjection { get; set; } = true;
            public bool ConfirmDeletion { get; set; } = true;
            public bool ConfirmOverwriteExtraction { get; set; } = true;
            public bool AutoIncrementFilename { get; set; } = true;
            public long MemoryMappingThreshold { get; set; } = 104857600; //100 MiB
            public bool FileListShowDirSize { get; set; } = false;
        }

        public static SettingsModel CurrentSettings { get; set; }
        public static UIStateModel CurrentUIState { get; set; }

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

                if (settings != null)
                    CurrentSettings = settings;
                else
                {
                    CurrentSettings = new SettingsModel();
                    Save();
                }
            }

            settingsWatcher = new FileSystemWatcher(SettingsDir, "settings.json");
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

                if (state != null)
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

                if (settings != null)
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

        public static void Reload()
        {
            var json = File.ReadAllText(SettingsFile);
            var settings = JsonSerializer.Deserialize<SettingsModel>(json);

            if (settings != null)
                CurrentSettings = settings;
            else
            {
                // for some reason we didn't read any settings
                // what do we do? let's just load default settings for now
                CurrentSettings = new SettingsModel();
                Save();
            }
        }

        public static void ReloadUIState()
        {
            var json = File.ReadAllText(UIStateFile);
            var state = JsonSerializer.Deserialize<UIStateModel>(json);

            if (state != null)
                CurrentUIState = state;
            else
            {
                CurrentUIState = new UIStateModel();
                SaveUIState();
            }
        }

        //Sets all settings to their default values
        public static void LoadDefaults()
        {
            //Set all settings to a default value here
            CurrentSettings = new SettingsModel();
            Save();
        }

        //Saves all settings to permanent storage (settings.json)
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
            if (settingsWatcher != null)
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

            if (settingsWatcher != null)
            {
                settingsWatcher.EnableRaisingEvents = true;
            }
        }

        //Saves the current UI state to permanent storage (uistate.json)
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

        //Adds an image to the recent list
        public static void AddRecentImage(string path)
        {
            //This prevents duplicate entries by removing the old entry first - the new one is then put at the start of the list
            if (CurrentSettings.RecentImages.Count > 0)
            {
                if (CurrentSettings.RecentImages.LastIndexOf(path) > -1)
                    CurrentSettings.RecentImages.RemoveAt(CurrentSettings.RecentImages.LastIndexOf(path));
            }

            //10 entries seems like a reasonable number
            if (CurrentSettings.RecentImages.Count >= 10)
                CurrentSettings.RecentImages.RemoveAt(0);
            CurrentSettings.RecentImages.Add(path);
        }

        //Removes the specified entry from the recent list
        public static void RemoveRecentImage(string path)
        {
            CurrentSettings.RecentImages.Remove(path);
        }

        //Clears all recent images
        public static void ClearRecentImages()
        {
            CurrentSettings.RecentImages.Clear();
        }
    }
}
