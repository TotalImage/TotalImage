using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using TotalImage.ViewModels.Browsing;
using TotalImage.ViewModels.Settings;

namespace TotalImage.Adapters
{
    /// <summary>
    /// Maps the view models' icon information to the images of the file list and folder tree:
    /// the shell's icons, or the built-in ones when the settings say not to ask the shell. Every
    /// icon has a faded variant for hidden entries.
    /// </summary>
    internal sealed class FileIcons
    {
        private const string FolderKey = "folder";
        private const string FileKey = "file";
        private const string ParentFolderKey = "up";

        private static readonly ImageAttributes HiddenIconAttributes = CreateHiddenIconAttributes();

        private readonly ImageList small;
        private readonly ImageList large;
        private readonly ISettingsService settings;

        public FileIcons(ImageList small, ImageList large, ISettingsService settings)
        {
            this.small = small;
            this.large = large;
            this.settings = settings;
        }

        /// <summary>
        /// Raised after <see cref="Reset"/>: image indices handed out before are no longer valid.
        /// </summary>
        public event System.EventHandler? Changed;

        /// <summary>
        /// Gets the key of an icon, which is the same in both image lists. Shell icons are added
        /// to the lists the first time they are needed.
        /// </summary>
        public string GetKey(IconKey icon, bool isHidden)
        {
            string key;
            switch (icon.Kind)
            {
                case IconKind.ParentFolder:
                    // Always our own, and never hidden.
                    return ParentFolderKey;

                case IconKind.File when settings.Current.QueryShellForFileTypeInfo:
                    var fileName = FileKey + icon.Extension;
                    var index = ShellInterop.GetFileTypeIconIndex(fileName, FileAttributes.Normal);
                    key = index.ToString();
                    if (!small.Images.ContainsKey(key))
                    {
                        Add(small, key, ShellInterop.GetFileTypeIcon(index, false));
                        Add(large, key, ShellInterop.GetFileTypeIcon(index, true));
                    }

                    break;

                case IconKind.File:
                    key = FileKey;
                    break;

                default:
                    key = FolderKey;
                    break;
            }

            return isHidden ? HiddenKey(key) : key;
        }

        /// <summary>
        /// Gets the index of an icon in both image lists.
        /// </summary>
        public int GetIndex(IconKey icon, bool isHidden) => small.Images.IndexOfKey(GetKey(icon, isHidden));

        /// <summary>
        /// Adds the small and large icons for a generic file and folder (normal and hidden) and
        /// for the ".." entry, replacing the ones added before, for example after the settings changed.
        /// </summary>
        public void Reset()
        {
            //Clear any existing icons from the lists to make sure we're not adding duplicate keys
            foreach (var list in new[] { small, large })
            {
                foreach (var key in new[] { FolderKey, HiddenKey(FolderKey), FileKey, HiddenKey(FileKey), ParentFolderKey })
                    list.Images.RemoveByKey(key);
            }

            //If shell integration is disabled, we use our own icons for file and folder, otherwise we obtain them from the shell
            var shell = settings.Current.QueryShellForFileTypeInfo;
            Add(small, FileKey, shell ? ShellInterop.SmallFileIcon : Properties.Resources.icon_page_white);
            Add(large, FileKey, shell ? ShellInterop.LargeFileIcon : Properties.Resources.icon_page_white_32);
            Add(small, FolderKey, shell ? ShellInterop.SmallFolderIcon : Properties.Resources.icon_folder);
            Add(large, FolderKey, shell ? ShellInterop.LargeFolderIcon : Properties.Resources.icon_folder_32);

            //The "Up one folder" icon is special - it's always our own and doesn't need a hidden variant
            small.Images.Add(ParentFolderKey, Properties.Resources.folder_up);
            large.Images.Add(ParentFolderKey, Properties.Resources.folder_up_32);

            Changed?.Invoke(this, System.EventArgs.Empty);
        }

        private static string HiddenKey(string key) => $"{key} (Hidden)";

        // Adds an icon and its faded variant for hidden entries.
        private static void Add(ImageList list, string key, Icon icon)
        {
            list.Images.Add(key, icon);
            list.Images.Add(HiddenKey(key), CreateHiddenIcon(icon.ToBitmap()));
        }

        private static ImageAttributes CreateHiddenIconAttributes()
        {
            var attributes = new ImageAttributes();
            attributes.SetColorMatrix(new ColorMatrix { Matrix33 = 0.65f }, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            return attributes;
        }

        private static Bitmap CreateHiddenIcon(Bitmap normalIcon)
        {
            Point[] points = [new(0, 0), new(normalIcon.Width, 0), new(0, normalIcon.Height)];
            Rectangle rect = new(0, 0, normalIcon.Width, normalIcon.Height);

            Bitmap bmp = new(normalIcon.Width, normalIcon.Height);
            using (Graphics gr = Graphics.FromImage(bmp))
            {
                gr.DrawImage(normalIcon, points, rect, GraphicsUnit.Pixel, HiddenIconAttributes);
            }

            return bmp;
        }
    }
}
