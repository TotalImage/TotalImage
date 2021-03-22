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
            public SizeUnit SizeUnits { get; set; } = SizeUnit.B;
            public string DefaultExtractPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            public FolderExtract DefaultExtractType { get; set; } = FolderExtract.AlwaysAsk;
            public bool OpenFolderAfterExtract { get; set; } = true;
            public int SplitterDistance { get; set; } = 280;
            public Size WindowSize { get; set; } = new Size(1000, 700);
            public  Point WindowPosition { get; set; } = new Point((Screen.PrimaryScreen.Bounds.Width - 1000) / 2, (Screen.PrimaryScreen.Bounds.Height - 700) / 2);
            public FormWindowState WindowState { get; set; } = FormWindowState.Normal;
        }

        public static SettingsModel CurrentSettings { get; private set; }

        private static readonly string SettingsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TotalImage");
        private static readonly string SettingsFile = Path.Combine(SettingsDir, "settings.json");

        public enum SizeUnit
        {
            B = 1,
            KB = 1000,
            KiB = 1024,
            MB = 1000000,
            MiB = 1048576
        }

        //Default folder options for extraction
        public enum FolderExtract
        {
            Ignore,   //Folders will be ignored by default
            Merge,    //All files will be extracted into the same directory
            Preserve, //Directory structure will be preserved
            AlwaysAsk //The user will always be prompted with the Extract dialog
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
            CurrentSettings.DefaultExtractType = FolderExtract.AlwaysAsk;
            CurrentSettings.DefaultExtractPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            CurrentSettings.SizeUnits = SizeUnit.B;
            CurrentSettings.ShowCommandBar = true;
            CurrentSettings.ShowDeletedItems = false;
            CurrentSettings.ShowDirectoryTree = true;
            CurrentSettings.ShowHiddenItems = true;
            CurrentSettings.ShowStatusBar = true;
            CurrentSettings.FilesSortingColumn = 0;
            CurrentSettings.FilesSortOrder = SortOrder.Ascending;
            CurrentSettings.FilesView = View.Details;

            //This should probably be preserved...
            /*CurrentSettings.SplitterDistance = 280;
            CurrentSettings.WindowPosition = new Point((Screen.PrimaryScreen.Bounds.Width - 1000) / 2, (Screen.PrimaryScreen.Bounds.Height - 700) / 2);
            CurrentSettings.WindowSize = new Size(800, 600);
            CurrentSettings.WindowState = FormWindowState.Normal;*/
        }

        //Saves all settings to permanent storage (settings.json)
        public static void Save()
        {
            string json = JsonSerializer.Serialize(CurrentSettings);

            //Just in case...
            if (!Directory.Exists(SettingsDir))
            {
                Directory.CreateDirectory(SettingsDir);
            }

            File.WriteAllText(SettingsFile, json);
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

        //Clears all recent images
        public static void ClearRecentImages()
        {
            CurrentSettings.RecentImages.Clear();
        }
    }
}