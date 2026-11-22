using System;
using System.IO;
using System.Runtime.Versioning;
using TotalImage.Avalonia.Platform.Windows;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Settings;

namespace TotalImage.Avalonia.Services;

/// <summary>
/// Picks the file type names for the platform: the shell's on Windows, generic ones elsewhere.
/// </summary>
public static class FileTypeInfoFactory
{
    public static IFileTypeInfo Create(ISettingsService settings)
        => OperatingSystem.IsWindows() ? new WindowsShellFileTypeInfo(settings) : new GenericFileTypeInfo();
}

/// <summary>
/// File type names from the Windows shell, or generic ones when the settings say not to ask the shell.
/// </summary>
[SupportedOSPlatform("windows")]
public sealed class WindowsShellFileTypeInfo(ISettingsService settings) : IFileTypeInfo
{
    public string GetTypeName(string name, FileAttributes attributes)
        => GenericFileTypeInfo.GetReparsePointTypeName(attributes)
            ?? (settings.Current.QueryShellForFileTypeInfo
                ? ShellFileInfo.GetTypeName(name, attributes)
                : GenericFileTypeInfo.GetGenericTypeName(name, attributes));
}
