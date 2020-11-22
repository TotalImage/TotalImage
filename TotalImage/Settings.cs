using System.Collections.Generic;
using System.Windows.Forms;

namespace TotalImage
{
    public static class Settings
    {
        public static View FilesView { get; set; }
        public static int FilesSortingColumn { get; set; }
        public static SortOrder FilesSortOrder { get; set; }
        public static List<string> RecentImages { get; private set; } = new List<string>();
        public static bool ShowHiddenItems { get; set; }
        public static bool ShowDeletedItems { get; set; }
        public static bool ShowCommandBar { get; set; }
        public static bool ShowDirectoryTree { get; set; }
        public static bool ShowFileList { get; set; }
        public static bool ShowStatusBar { get; set; }
        public static SizeUnit SizeUnits { get; set; }
        public static string? DefaultExtractPath { get; set; }
        public static FolderExtract DefaultExtractType { get; set; }
        public static bool OpenFolderAfterExtract { get; set; }

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

        //TODO: Implement loading the values from permanent storage
        public static void Load()
        {
            RecentImages.Clear();
        }

        //TODO: Implement saving the values to permanent storage
        public static void Save()
        {

        }

        public static void AddRecentImage(string path)
        {
            //This prevents duplicate entries by removing the old entry first - the new one is then put at the start of the list
            if (RecentImages.Count > 0) {
                if(RecentImages.LastIndexOf(path) > -1)
                    RecentImages.RemoveAt(RecentImages.LastIndexOf(path));
            }

            //10 entries seems like a reasonable number
            if (RecentImages.Count >= 10)
                RecentImages.RemoveAt(0);
            RecentImages.Add(path);
        }
    }
}