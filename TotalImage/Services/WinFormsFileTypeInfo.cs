using System.Drawing;
using System.IO;
using TotalImage.Properties;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Settings;

namespace TotalImage.Services
{
    /// <summary>
    /// File type names and icons from the shell, or generic ones when the settings say not to
    /// ask the shell.
    /// </summary>
    public sealed class WinFormsFileTypeInfo(ISettingsService settings) : IFileTypeInfo
    {
        public string GetTypeName(string name, FileAttributes attributes)
            => settings.Current.QueryShellForFileTypeInfo
                ? ShellInterop.GetFileTypeName(name, attributes)
                : GenericFileTypeInfo.GetGenericTypeName(name, attributes);

        /// <summary>
        /// Gets the 32x32 icon of an entry. <c>Stretch</c> is true for the built-in icons, which
        /// need scaling to the picture box.
        /// </summary>
        public (Image Image, bool Stretch) GetLargeIcon(string name, FileAttributes attributes)
        {
            if (settings.Current.QueryShellForFileTypeInfo)
                return (ShellInterop.GetFileTypeIcon(name, attributes).ToBitmap(), false);

            var icon = attributes.HasFlag(FileAttributes.Directory) ? Resources.icon_folder_32 : Resources.icon_page_white_32;
            return (icon.ToBitmap(), true);
        }
    }
}
