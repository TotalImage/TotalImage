using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.Json;
using System.IO;
using System.Drawing;
using System.ComponentModel.DataAnnotations;

namespace TotalImage
{
    //This class is a singleton, thread-safe with a double check lock
    public static class Settings
    {
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
            public FolderExtract DefaultExtractType { get; set; } = FolderExtract.Preserve;
            public bool ExtractAlwaysAsk { get; set; } = true;
            public bool OpenFolderAfterExtract { get; set; } = true;
            public bool ExtractPreserveDates { get; set; } = true;
            public bool ExtractPreserveAttributes { get; set; } = false;
            public int SplitterDistance { get; set; } = 280;
            public Size WindowSize { get; set; } = new Size(1000, 700);
            public Point WindowPosition { get; set; } = new Point((Screen.PrimaryScreen.Bounds.Width - 1000) / 2, (Screen.PrimaryScreen.Bounds.Height - 700) / 2);
            public FormWindowState WindowState { get; set; } = FormWindowState.Normal;
            public bool QueryShellForFileTypeInfo { get; set; } = true;
            public bool ConfirmInjection { get; set; } = true;
            public bool ConfirmDeletion { get; set; } = true;
            public bool ConfirmOverwriteExtraction { get; set; } = true;
            public bool AutoIncrementFilename { get; set; } = true;
        }

        public static SettingsModel CurrentSettings { get; private set; }

        private static readonly string SettingsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TotalImage");
        private static readonly string SettingsFile = Path.Combine(SettingsDir, "settings.json");

        private static FileSystemWatcher settingsWatcher;

        //Default folder options for extraction
        public enum FolderExtract
        {
            Ignore,   //Folders will be ignored by default
            Merge,    //All files will be extracted into the same directory
            Preserve, //Directory structure will be preserved
        }

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
                    CurrentSettings.DefaultExtractType = settings.DefaultExtractType;
                    CurrentSettings.DefaultExtractPath = settings.DefaultExtractPath;
                    CurrentSettings.RecentImages = settings.RecentImages;
                    CurrentSettings.ExtractPreserveAttributes = settings.ExtractPreserveAttributes;
                    CurrentSettings.ExtractPreserveDates = settings.ExtractPreserveDates;
                    CurrentSettings.AutoIncrementFilename = settings.AutoIncrementFilename;
                    CurrentSettings.ConfirmDeletion = settings.ConfirmDeletion;
                    CurrentSettings.ConfirmInjection = settings.ConfirmInjection;
                    CurrentSettings.ConfirmOverwriteExtraction = settings.ConfirmOverwriteExtraction;
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

        //Sets all settings to their default values
        public static void LoadDefaults()
        {
            //Set all settings to a default value here
            CurrentSettings.OpenFolderAfterExtract = true;
            CurrentSettings.DefaultExtractType = FolderExtract.Preserve;
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

            //This should probably be preserved...
            /*CurrentSettings.SplitterDistance = 280;
            CurrentSettings.WindowPosition = new Point((Screen.PrimaryScreen.Bounds.Width - 1000) / 2, (Screen.PrimaryScreen.Bounds.Height - 700) / 2);
            CurrentSettings.WindowSize = new Size(800, 600);
            CurrentSettings.WindowState = FormWindowState.Normal;*/
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

            File.WriteAllText(SettingsFile, json);

            if (settingsWatcher != null)
            {
                settingsWatcher.EnableRaisingEvents = true;
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