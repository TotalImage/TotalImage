using System.IO;

namespace TotalImage.ViewModels.Services;

/// <summary>
/// Names the type of a file or folder, for example "Text Document" or "File folder". Each UI
/// provides its own: WinForms can ask the shell, other platforms use <see cref="GenericFileTypeInfo"/>.
/// </summary>
public interface IFileTypeInfo
{
    /// <summary>
    /// Gets the type name of an entry from its name (for the extension) and attributes.
    /// Reparse points are named by <see cref="GenericFileTypeInfo.GetReparsePointTypeName"/>.
    /// </summary>
    string GetTypeName(string name, FileAttributes attributes);
}

/// <summary>
/// Names types without asking the operating system: "Junction" or "Symbolic link" for reparse
/// points, "File folder", "XYZ File", or "File" when there is no extension.
/// </summary>
public sealed class GenericFileTypeInfo : IFileTypeInfo
{
    public string GetTypeName(string name, FileAttributes attributes) => GetGenericTypeName(name, attributes);

    /// <summary>
    /// The type name of a reparse point ("Junction" for a folder, "Symbolic link" for a file),
    /// or <c>null</c> for any other entry.
    /// </summary>
    public static string? GetReparsePointTypeName(FileAttributes attributes)
    {
        if (!attributes.HasFlag(FileAttributes.ReparsePoint))
            return null;

        return attributes.HasFlag(FileAttributes.Directory) ? "Junction" : "Symbolic link";
    }

    /// <inheritdoc cref="GenericFileTypeInfo"/>
    public static string GetGenericTypeName(string name, FileAttributes attributes)
    {
        if (GetReparsePointTypeName(attributes) is { } reparsePointName)
            return reparsePointName;

        if (attributes.HasFlag(FileAttributes.Directory))
            return "File folder";

        var extension = Path.GetExtension(name);
        return extension.Length > 0 ? $"{extension[1..].ToUpper()} File" : "File";
    }
}
