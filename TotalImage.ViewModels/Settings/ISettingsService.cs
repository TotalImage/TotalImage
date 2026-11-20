using System;

namespace TotalImage.ViewModels.Settings;

/// <summary>
/// Owns the application settings stored in settings.json.
/// </summary>
public interface ISettingsService
{
    /// <summary>
    /// The settings currently in use. Changes are kept in memory until <see cref="Save"/> is called.
    /// </summary>
    SettingsModel Current { get; }

    /// <summary>
    /// Raised after settings.json was changed by someone else (for example another instance)
    /// and the non-UI settings were reloaded from it. Raised on the thread that created the
    /// service when it had a synchronization context.
    /// </summary>
    event EventHandler? ExternallyChanged;

    /// <summary>
    /// Saves all settings to settings.json. Fails silently when the file can't be written.
    /// </summary>
    void Save();

    /// <summary>
    /// Reloads all settings from settings.json, falling back to defaults when it can't be used.
    /// </summary>
    void Reload();

    /// <summary>
    /// Resets all settings except the recent images list to their default values.
    /// </summary>
    void ResetToDefaults();

    /// <summary>
    /// Adds a path to the start of the recent images list, removing an older copy of it and
    /// keeping at most 10 entries.
    /// </summary>
    void AddRecentImage(string path);

    /// <summary>
    /// Removes a path from the recent images list.
    /// </summary>
    void RemoveRecentImage(string path);

    /// <summary>
    /// Removes all paths from the recent images list.
    /// </summary>
    void ClearRecentImages();

    /// <summary>
    /// Removes paths that no longer exist from the recent images list.
    /// </summary>
    void RemoveMissingRecentImages();
}
