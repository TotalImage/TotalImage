using System.Collections.Generic;
using System.Windows.Forms;

namespace TotalImage
{
    public static class Settings
    {
        public static View FilesView { get; set; }
        public static int FilesSortingColumn { get; set; }
        public static SortOrder FilesSortOrder { get; set; }
        public static List<string> RecentImages { get; }
        public static bool ShowHiddenItems { get; set; }
        public static bool ShowDeletedItems { get; set; }
        public static bool ShowCommandBar { get; set; }
        public static bool ShowDirectoryTree { get; set; }
        public static bool ShowFileList { get; set; }
        public static bool ShowStatusBar { get; set; }
        public static SizeUnit SizeUnits { get; set; }
        public static string DefaultExtractPath { get; set; }
        public static FolderExtract DefaultExtractType { get; set; }

        public enum SizeUnit
        {
            B = 1,
            KB = 1000,
            KiB = 1024,
            MB = 1000000,
            MiB = 1048576
        }
        public enum FolderExtract
        {
            Ignore,
            Merge,
            Preserve,
            AlwaysAsk
        }

        public static void Load()
        {

        }

        public static void Save()
        {

        }

        public static void AddRecentImage(string path)
        {
            if (RecentImages.Count >= 10)
                RecentImages.RemoveAt(0);
            RecentImages.Add(path);
        }
    }
}