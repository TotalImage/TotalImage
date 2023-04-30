using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Storage.FileSystem;
using Windows.Win32.UI.Controls;
using Windows.Win32.UI.Shell;

namespace TotalImage;

internal static class ShellInterop
{
    static Dictionary<string, (string Name, int IconIndex)> _fileTypes = new(StringComparer.InvariantCultureIgnoreCase);

    unsafe static (string Name, int IconIndex) GetFileTypeInfo(string fileName, FileAttributes attributes)
    {
        var extension = attributes.HasFlag(FileAttributes.Directory) ? "folder" : Path.GetExtension(fileName);

        if (!_fileTypes.ContainsKey(extension))
        {
            var shinfo = new SHFILEINFOW();
            var flags = SHGFI_FLAGS.SHGFI_TYPENAME | SHGFI_FLAGS.SHGFI_SYSICONINDEX | SHGFI_FLAGS.SHGFI_USEFILEATTRIBUTES;

            PInvoke.SHGetFileInfo(fileName, (FILE_FLAGS_AND_ATTRIBUTES)attributes, &shinfo, (uint)Marshal.SizeOf(shinfo), flags);

            _fileTypes.Add(extension, (shinfo.szTypeName.ToString(), shinfo.iIcon));
        }

        return _fileTypes[extension];
    }

    public static string GetFileTypeName(string fileName, FileAttributes attributes) => GetFileTypeInfo(fileName, attributes).Name;
    public static int GetFileTypeIconIndex(string fileName, FileAttributes attributes) => GetFileTypeInfo(fileName, attributes).IconIndex;

    public static Icon GetFileTypeIcon(int index, bool large)
    {
        PInvoke.SHGetImageList(large ? (int)PInvoke.SHIL_LARGE : (int)PInvoke.SHIL_SMALL, typeof(IImageList).GUID, out var obj);

        var imageList = (IImageList)obj;
        imageList.GetIcon(index, (uint)IMAGE_LIST_DRAW_STYLE.ILD_TRANSPARENT, out var icon);

        using (icon)
        {
            return (Icon)Icon.FromHandle(icon.DangerousGetHandle()).Clone();
        }
    }

    public static Icon GetFileTypeIcon(string fileName, FileAttributes attributes, bool large = true)
        => GetFileTypeIcon(GetFileTypeIconIndex(fileName, attributes), large);

    public static Icon LargeFolderIcon => GetFileTypeIcon(GetFileTypeIconIndex("folder", FileAttributes.Directory), true);
    public static Icon SmallFolderIcon => GetFileTypeIcon(GetFileTypeIconIndex("folder", FileAttributes.Directory), false);
    public static Icon LargeFileIcon => GetFileTypeIcon(GetFileTypeIconIndex("file", 0), true);
    public static Icon SmallFileIcon => GetFileTypeIcon(GetFileTypeIconIndex("file", 0), false);

    public static (Icon Small, Icon Large) GetGoUpIcons()
    {
        if (PInvoke.ExtractIconEx("shell32.dll", 45, out var largeIcon, out var smallIcon, 1) != uint.MaxValue)
        {
            using (largeIcon)
            using (smallIcon)
            {
                return (
                    (Icon)Icon.FromHandle(smallIcon.DangerousGetHandle()).Clone(),
                    (Icon)Icon.FromHandle(largeIcon.DangerousGetHandle()).Clone()
                );
            }
        }

        throw Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error())!;
    }
}
