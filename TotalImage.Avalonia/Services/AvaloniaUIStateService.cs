using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TotalImage.ViewModels.Settings;

namespace TotalImage.Avalonia.Services;

/// <summary>
/// The state of the main window and its controls, kept between sessions. Separate from the
/// WinForms UI's uistate.json, so the two UIs never overwrite each other's state.
/// </summary>
public sealed class AvaloniaUIState
{
    /// <summary>
    /// The size of the main window, in device-independent pixels.
    /// </summary>
    public double WindowWidth { get; set; } = 1000;

    public double WindowHeight { get; set; } = 700;

    /// <summary>
    /// The position of the main window on the screen, in physical pixels, or <c>null</c> to center it.
    /// </summary>
    public int? WindowX { get; set; }

    public int? WindowY { get; set; }

    public bool IsMaximized { get; set; }

    /// <summary>
    /// The width of the folder tree, in device-independent pixels.
    /// </summary>
    public double FolderTreeWidth { get; set; } = 280;

    /// <summary>
    /// The widths of the file list's columns (name, type, size, modified, attributes).
    /// </summary>
    public List<double> ColumnWidths { get; set; } = [250, 150, 100, 150, 85];
}

/// <summary>
/// Stores the Avalonia UI state in avalonia-uistate.json, next to settings.json.
/// </summary>
public sealed class AvaloniaUIStateService
{
    /// <summary>
    /// The file name, in the settings folder.
    /// </summary>
    public const string FileName = "avalonia-uistate.json";

    private static readonly JsonSerializerOptions WriteOptions = new() { WriteIndented = true };

    private readonly string directory;
    private readonly string file;

    public AvaloniaUIStateService() : this(SettingsService.DefaultDirectory) { }

    public AvaloniaUIStateService(string directory)
    {
        this.directory = directory;
        file = Path.Combine(directory, FileName);
        Current = Load();
    }

    /// <summary>
    /// The current UI state.
    /// </summary>
    public AvaloniaUIState Current { get; private set; }

    /// <summary>
    /// Reads the state again, or the defaults when the file doesn't exist or can't be used.
    /// </summary>
    public void Reload() => Current = Load();

    /// <summary>
    /// Saves the current state. Gives up silently when the file can't be written, for example
    /// while another instance writes it.
    /// </summary>
    public void Save()
    {
        try
        {
            Directory.CreateDirectory(directory);
            File.WriteAllText(file, JsonSerializer.Serialize(Current, WriteOptions));
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
        }
    }

    private AvaloniaUIState Load()
    {
        try
        {
            if (File.Exists(file) && JsonSerializer.Deserialize<AvaloniaUIState>(File.ReadAllText(file)) is { } state)
                return state;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException)
        {
        }

        return new AvaloniaUIState();
    }
}
