using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace TotalImage.ViewModels.Settings;

/// <summary>
/// Stores settings in settings.json inside a data directory and watches the file for changes
/// made by other instances.
/// </summary>
public sealed class SettingsService : ISettingsService, IDisposable
{
    private static readonly JsonSerializerOptions WriteOptions = new() { WriteIndented = true };

    private readonly string directory;
    private readonly string settingsFile;
    private readonly SynchronizationContext? syncContext;
    private FileSystemWatcher? watcher;

    /// <inheritdoc/>
    public SettingsModel Current { get; private set; }

    /// <inheritdoc/>
    public event EventHandler? ExternallyChanged;

    /// <summary>
    /// Loads settings from settings.json in <paramref name="directory"/>, creating the directory
    /// and the file with default values if they don't exist yet.
    /// </summary>
    /// <param name="directory">The directory holding settings.json.</param>
    /// <param name="watchForChanges">Watch the file for changes made by other processes.</param>
    public SettingsService(string directory, bool watchForChanges = true)
    {
        this.directory = directory;
        settingsFile = Path.Combine(directory, "settings.json");
        syncContext = SynchronizationContext.Current;

        Directory.CreateDirectory(directory);

        var loaded = File.Exists(settingsFile) ? TryRead() : null;
        Current = loaded ?? new SettingsModel();
        if (loaded is null)
            Save();

        if (watchForChanges)
        {
            watcher = new FileSystemWatcher(directory, "settings.json")
            {
                NotifyFilter = NotifyFilters.LastWrite
            };
            watcher.Changed += Watcher_Changed;
            watcher.EnableRaisingEvents = true;
        }
    }

    /// <summary>
    /// The settings directory used by the application: TotalImage in the local application data folder.
    /// </summary>
    public static string DefaultDirectory { get; } =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TotalImage");

    private SettingsModel? TryRead()
    {
        try
        {
            return JsonSerializer.Deserialize<SettingsModel>(File.ReadAllText(settingsFile));
        }
        catch (Exception ex) when (ex is IOException or JsonException or UnauthorizedAccessException)
        {
            return null;
        }
    }

    private void Watcher_Changed(object sender, FileSystemEventArgs e)
    {
        SettingsModel? settings;
        try
        {
            settings = JsonSerializer.Deserialize<SettingsModel>(File.ReadAllText(settingsFile));
        }
        catch (Exception ex) when (ex is IOException or JsonException or UnauthorizedAccessException)
        {
            // We can't read the file. Carry on.
            return;
        }

        if (settings is null)
            return;

        if (syncContext is not null)
            syncContext.Post(_ => ApplyExternalChanges(settings), null);
        else
            ApplyExternalChanges(settings);
    }

    // We could just reload the entire settings, however the loaded settings could include
    // changes to UI options, and it is not nice to change UI options out of the blue.
    private void ApplyExternalChanges(SettingsModel settings)
    {
        Current.OpenFolderAfterExtract = settings.OpenFolderAfterExtract;
        Current.DefaultDirectoryExtractionMode = settings.DefaultDirectoryExtractionMode;
        Current.DefaultExtractPath = settings.DefaultExtractPath;
        Current.RecentImages = settings.RecentImages;
        Current.ExtractPreserveAttributes = settings.ExtractPreserveAttributes;
        Current.ExtractPreserveDates = settings.ExtractPreserveDates;
        Current.AutoIncrementFilename = settings.AutoIncrementFilename;
        Current.ConfirmDeletion = settings.ConfirmDeletion;
        Current.ConfirmInjection = settings.ConfirmInjection;
        Current.ConfirmOverwriteExtraction = settings.ConfirmOverwriteExtraction;
        Current.MemoryMappingThreshold = settings.MemoryMappingThreshold;
        Current.FileListShowDirSize = settings.FileListShowDirSize;
        Current.ColorMode = settings.ColorMode;

        ExternallyChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <inheritdoc/>
    public void Save()
    {
        var json = JsonSerializer.Serialize(Current, WriteOptions);

        Directory.CreateDirectory(directory);

        // Let's not make us reload the very settings we're about to save
        if (watcher is not null)
            watcher.EnableRaisingEvents = false;

        try
        {
            File.WriteAllText(settingsFile, json);
        }
        catch (IOException)
        {
            // We can't write to the file, probably because it's opened by another instance, so let's just silently give up
        }
        finally
        {
            if (watcher is not null)
                watcher.EnableRaisingEvents = true;
        }
    }

    /// <inheritdoc/>
    public void Reload()
    {
        var settings = TryRead();
        if (settings is not null)
        {
            Current = settings;
        }
        else
        {
            Current = new SettingsModel();
            Save();
        }
    }

    /// <inheritdoc/>
    public void ResetToDefaults()
    {
        // Reset in place so that anyone holding Current sees the defaults too.
        var defaults = new SettingsModel();
        foreach (var property in typeof(SettingsModel).GetProperties())
        {
            if (property.Name != nameof(SettingsModel.RecentImages))
                property.SetValue(Current, property.GetValue(defaults));
        }
    }

    /// <inheritdoc/>
    public void AddRecentImage(string path)
    {
        var list = Current.RecentImages;

        //This prevents duplicate entries by removing the old entry first - the new one is then put at the start of the list
        var index = list.LastIndexOf(path);
        if (index > -1)
            list.RemoveAt(index);

        //10 entries seems like a reasonable number
        if (list.Count >= 10)
            list.RemoveAt(0);
        list.Add(path);
    }

    /// <inheritdoc/>
    public void RemoveRecentImage(string path) => Current.RecentImages.Remove(path);

    /// <inheritdoc/>
    public void ClearRecentImages() => Current.RecentImages.Clear();

    /// <inheritdoc/>
    public void RemoveMissingRecentImages()
    {
        foreach (var path in Current.RecentImages.Where(p => !File.Exists(p)).ToList())
            Current.RecentImages.Remove(path);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        watcher?.Dispose();
        watcher = null;
    }
}
