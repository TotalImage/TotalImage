using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TotalImage.ViewModels.Extraction;
using TotalImage.ViewModels.Formatting;
using TotalImage.ViewModels.Settings;
using Xunit;

namespace TotalImage.ViewModels.Tests.Settings;

public class SettingsServiceTests
{
    // A settings.json as written by the WinForms app before the settings moved to the shared
    // library: enums are stored as numbers (View, SortOrder and SystemColorMode values).
    private const string LegacySettingsJson = """
        {
          "FilesView": 3,
          "FilesSortingColumn": 2,
          "FilesSortOrder": 2,
          "RecentImages": [
            "C:\\Images\\a.img",
            "C:\\Images\\b.vhd"
          ],
          "ShowHiddenItems": false,
          "ShowCommandBar": false,
          "ShowDirectoryTree": true,
          "ShowStatusBar": false,
          "SizeUnit": 1024,
          "DefaultExtractPath": "D:\\Extracted",
          "DefaultDirectoryExtractionMode": 2,
          "ExtractAlwaysAsk": false,
          "OpenFolderAfterExtract": false,
          "ExtractPreserveDates": false,
          "ExtractPreserveAttributes": true,
          "QueryShellForFileTypeInfo": false,
          "ConfirmInjection": false,
          "ConfirmDeletion": false,
          "ConfirmOverwriteExtraction": false,
          "AutoIncrementFilename": false,
          "MemoryMappingThreshold": 52428800,
          "FileListShowDirSize": true,
          "ColorMode": 2
        }
        """;

    [Fact]
    public void LoadsExistingSettingsFile()
    {
        using var folder = new TempFolder();
        File.WriteAllText(folder.File("settings.json"), LegacySettingsJson);

        using var service = new SettingsService(folder.Path, watchForChanges: false);
        var s = service.Current;

        Assert.Equal(ViewMode.List, s.FilesView);
        Assert.Equal(2, s.FilesSortingColumn);
        Assert.Equal(SortDirection.Descending, s.FilesSortOrder);
        Assert.Equal(["C:\\Images\\a.img", "C:\\Images\\b.vhd"], s.RecentImages);
        Assert.False(s.ShowHiddenItems);
        Assert.False(s.ShowCommandBar);
        Assert.True(s.ShowDirectoryTree);
        Assert.False(s.ShowStatusBar);
        Assert.Equal(SizeUnit.Binary, s.SizeUnit);
        Assert.Equal("D:\\Extracted", s.DefaultExtractPath);
        Assert.Equal(DirectoryExtractionMode.Merge, s.DefaultDirectoryExtractionMode);
        Assert.False(s.ExtractAlwaysAsk);
        Assert.False(s.OpenFolderAfterExtract);
        Assert.False(s.ExtractPreserveDates);
        Assert.True(s.ExtractPreserveAttributes);
        Assert.False(s.QueryShellForFileTypeInfo);
        Assert.False(s.ConfirmInjection);
        Assert.False(s.ConfirmDeletion);
        Assert.False(s.ConfirmOverwriteExtraction);
        Assert.False(s.AutoIncrementFilename);
        Assert.Equal(52428800, s.MemoryMappingThreshold);
        Assert.True(s.FileListShowDirSize);
        Assert.Equal(ColorTheme.Dark, s.ColorMode);
    }

    [Fact]
    public void SavingAnExistingFile_KeepsItsContentAndFormat()
    {
        using var folder = new TempFolder();
        var path = folder.File("settings.json");
        File.WriteAllText(path, LegacySettingsJson);

        using (var service = new SettingsService(folder.Path, watchForChanges: false))
            service.Save();

        using var original = JsonDocument.Parse(LegacySettingsJson);
        using var saved = JsonDocument.Parse(File.ReadAllText(path));

        var originalProps = original.RootElement.EnumerateObject().ToDictionary(p => p.Name, p => p.Value.GetRawText());
        var savedProps = saved.RootElement.EnumerateObject().ToDictionary(p => p.Name, p => p.Value.GetRawText());

        Assert.Equal(originalProps.Keys.OrderBy(k => k), savedProps.Keys.OrderBy(k => k));
        foreach (var (name, value) in originalProps)
            Assert.Equal(Normalize(value), Normalize(savedProps[name]));

        static string Normalize(string json) => JsonSerializer.Serialize(JsonDocument.Parse(json).RootElement);
    }

    [Fact]
    public void ChangesSurviveSaveAndReload()
    {
        using var folder = new TempFolder();

        using (var service = new SettingsService(folder.Path, watchForChanges: false))
        {
            service.Current.FilesView = ViewMode.Tile;
            service.Current.ColorMode = ColorTheme.Classic;
            service.Current.SizeUnit = SizeUnit.Decimal;
            service.Save();
        }

        using var reloaded = new SettingsService(folder.Path, watchForChanges: false);
        Assert.Equal(ViewMode.Tile, reloaded.Current.FilesView);
        Assert.Equal(ColorTheme.Classic, reloaded.Current.ColorMode);
        Assert.Equal(SizeUnit.Decimal, reloaded.Current.SizeUnit);
    }

    [Fact]
    public void MissingFile_IsCreatedWithDefaults()
    {
        using var folder = new TempFolder();
        var dir = Path.Combine(folder.Path, "nested");

        using var service = new SettingsService(dir, watchForChanges: false);

        Assert.True(File.Exists(Path.Combine(dir, "settings.json")));
        Assert.Equal(ViewMode.Details, service.Current.FilesView);
        Assert.Equal(ColorTheme.System, service.Current.ColorMode);
    }

    [Fact]
    public void CorruptFile_FallsBackToDefaults()
    {
        using var folder = new TempFolder();
        File.WriteAllText(folder.File("settings.json"), "{ not json");

        using var service = new SettingsService(folder.Path, watchForChanges: false);

        Assert.Equal(ViewMode.Details, service.Current.FilesView);
    }

    [Fact]
    public void Reload_ReplacesInMemoryChanges()
    {
        using var folder = new TempFolder();
        File.WriteAllText(folder.File("settings.json"), LegacySettingsJson);
        using var service = new SettingsService(folder.Path, watchForChanges: false);

        service.Current.FilesView = ViewMode.Tile;
        service.Reload();

        Assert.Equal(ViewMode.List, service.Current.FilesView);
    }

    [Fact]
    public void ResetToDefaults_KeepsRecentImagesAndTheSameInstance()
    {
        using var folder = new TempFolder();
        File.WriteAllText(folder.File("settings.json"), LegacySettingsJson);
        using var service = new SettingsService(folder.Path, watchForChanges: false);
        var instance = service.Current;

        service.ResetToDefaults();

        Assert.Same(instance, service.Current);
        Assert.Equal(ViewMode.Details, service.Current.FilesView);
        Assert.Equal(ColorTheme.System, service.Current.ColorMode);
        Assert.True(service.Current.ConfirmDeletion);
        Assert.Equal(2, service.Current.RecentImages.Count);
    }

    [Fact]
    public void AddRecentImage_MovesDuplicatesAndKeepsTen()
    {
        using var folder = new TempFolder();
        using var service = new SettingsService(folder.Path, watchForChanges: false);

        for (int i = 0; i < 12; i++)
            service.AddRecentImage($"image{i}");
        service.AddRecentImage("image5");

        Assert.Equal(10, service.Current.RecentImages.Count);
        Assert.Equal("image5", service.Current.RecentImages[^1]);
        Assert.Single(service.Current.RecentImages, p => p == "image5");
        Assert.DoesNotContain("image0", service.Current.RecentImages);
        Assert.DoesNotContain("image1", service.Current.RecentImages);
    }

    [Fact]
    public void RemoveAndClearRecentImages()
    {
        using var folder = new TempFolder();
        using var service = new SettingsService(folder.Path, watchForChanges: false);
        service.AddRecentImage("a");
        service.AddRecentImage("b");

        service.RemoveRecentImage("a");
        Assert.Equal(["b"], service.Current.RecentImages);

        service.ClearRecentImages();
        Assert.Empty(service.Current.RecentImages);
    }

    [Fact]
    public void RemoveMissingRecentImages_KeepsOnlyExistingFiles()
    {
        using var folder = new TempFolder();
        using var service = new SettingsService(folder.Path, watchForChanges: false);
        var existing = folder.File("exists.img");
        File.WriteAllText(existing, "");
        service.AddRecentImage(folder.File("missing.img"));
        service.AddRecentImage(existing);

        service.RemoveMissingRecentImages();

        Assert.Equal([existing], service.Current.RecentImages);
    }

    [Fact]
    public async Task ExternalChange_UpdatesNonUISettingsAndRaisesEvent()
    {
        using var folder = new TempFolder();
        using var service = new SettingsService(folder.Path);
        var changed = new TaskCompletionSource();
        service.ExternallyChanged += (_, _) => changed.TrySetResult();

        var external = LegacySettingsJson;
        File.WriteAllText(folder.File("settings.json"), external);

        var finished = await Task.WhenAny(changed.Task, Task.Delay(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken));
        Assert.Same(changed.Task, finished);

        // Non-UI settings are picked up...
        Assert.False(service.Current.ConfirmDeletion);
        Assert.Equal(ColorTheme.Dark, service.Current.ColorMode);
        // ...UI options are left alone.
        Assert.Equal(ViewMode.Details, service.Current.FilesView);
        Assert.True(service.Current.ShowStatusBar);
    }
}
