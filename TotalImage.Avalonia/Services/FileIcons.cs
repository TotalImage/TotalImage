using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using TotalImage.Avalonia.Platform.Windows;
using TotalImage.ViewModels.Browsing;
using TotalImage.ViewModels.Settings;

namespace TotalImage.Avalonia.Services;

/// <summary>
/// Maps the view models' icon information to images: the shell's icons on Windows (unless the
/// settings say not to ask the shell), the bundled ones elsewhere. Faded icons for hidden entries
/// come from the "hidden" style class, not from here.
/// </summary>
public sealed class FileIcons(ISettingsService settings)
{
    private readonly Dictionary<(IconKey Key, bool Large, bool Shell), IImage> cache = [];

    private static readonly Lazy<IImage> SmallFile = new(() => Load("page_white.ico"));
    private static readonly Lazy<IImage> LargeFile = new(() => Load("page_white_32.ico"));
    private static readonly Lazy<IImage> SmallFolder = new(() => Load("folder.ico"));
    private static readonly Lazy<IImage> LargeFolder = new(() => Load("folder_32.ico"));
    private static readonly Lazy<IImage> SmallParentFolder = new(() => Load("folder_up.png"));
    private static readonly Lazy<IImage> LargeParentFolder = new(() => Load("folder_up_32.png"));
    private static readonly Lazy<IImage> SmallVolume = new(() => Load("drive_go.png"));

    /// <summary>
    /// Gets the icon for an entry or folder.
    /// </summary>
    /// <param name="large">The 32 px icon rather than the 16 px one.</param>
    public IImage Get(IconKey key, bool large)
    {
        var shell = OperatingSystem.IsWindows() && settings.Current.QueryShellForFileTypeInfo;
        if (cache.TryGetValue((key, large, shell), out var icon))
            return icon;

        icon = (shell ? GetShellIcon(key, large) : null) ?? GetBundledIcon(key, large);
        cache[(key, large, shell)] = icon;
        return icon;
    }

    private static IImage? GetShellIcon(IconKey key, bool large)
    {
        if (!OperatingSystem.IsWindows())
            return null;

        return key.Kind switch
        {
            IconKind.File => ShellFileInfo.GetIcon("file" + key.Extension, FileAttributes.Normal, large),
            IconKind.Folder => ShellFileInfo.GetIcon("folder", FileAttributes.Directory, large),
            _ => null // The ".." entry and volumes always use the bundled icons.
        };
    }

    private static IImage GetBundledIcon(IconKey key, bool large) => key.Kind switch
    {
        IconKind.Folder => large ? LargeFolder.Value : SmallFolder.Value,
        IconKind.ParentFolder => large ? LargeParentFolder.Value : SmallParentFolder.Value,
        IconKind.Volume => SmallVolume.Value,
        _ => large ? LargeFile.Value : SmallFile.Value
    };

    private static Bitmap Load(string asset)
    {
        using var stream = AssetLoader.Open(new Uri($"avares://TotalImage.Avalonia/Assets/{asset}"));
        return new Bitmap(stream);
    }
}
