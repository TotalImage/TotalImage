using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TotalImage.Changes;
using TotalImage.Containers;
using TotalImage.DiskGeometries;
using TotalImage.FileSystems.BPB;
using TotalImage.FileSystems.FAT;
using TotalImage.ViewModels.Dialogs;
using TotalImage.ViewModels.Entries;
using TotalImage.ViewModels.Formatting;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Settings;
using Xunit;
using TiDirectory = TotalImage.FileSystems.Directory;
using TiFile = TotalImage.FileSystems.File;
using TiFileSystem = TotalImage.FileSystems.FileSystem;
using TiFileSystemObject = TotalImage.FileSystems.FileSystemObject;

namespace TotalImage.ViewModels.Tests.Dialogs;

public class PropertiesViewModelTests : IDisposable
{
    private static readonly DateTime Created = new(2001, 2, 3, 4, 5, 6);
    private static readonly DateTime Written = new(2002, 3, 4, 5, 6, 8);
    private static readonly DateTime Accessed = new(2003, 4, 5);

    // README.TXT (1000 bytes, read-only), NOTES.TXT (10 bytes, hidden), all files archive,
    // DOCS\A.TXT (100 bytes), DOCS\SUB\B.TXT (2000 bytes), EMPTY (empty directory)
    private static readonly byte[] ReadmeContents = Enumerable.Range(0, 1000).Select(i => (byte)i).ToArray();

    private readonly PropertiesTestImage image = new(add =>
    {
        add(["README.TXT"], ReadmeContents, FatAttributes.ReadOnly | FatAttributes.Archive);
        add(["NOTES.TXT"], new byte[10], FatAttributes.Hidden);
        add(["DOCS", "A.TXT"], new byte[100], FatAttributes.Archive);
        add(["DOCS", "SUB", "B.TXT"], new byte[2000], FatAttributes.Archive);
        add(["EMPTY"], null, default);
    });

    private readonly PropertiesFakeSettings settings = new();
    private readonly PropertiesFakeFileTypeInfo fileTypeInfo = new();

    public void Dispose() => image.Dispose();

    private PropertiesViewModel Create(params TiFileSystemObject[] entries) => new(entries, settings, fileTypeInfo);

    private TiFile Readme => (TiFile)image.Get("README.TXT");
    private TiFile Notes => (TiFile)image.Get("NOTES.TXT");
    private TiDirectory Docs => (TiDirectory)image.Get("DOCS");

    [Fact]
    public void NeedsAtLeastOneEntry()
    {
        Assert.Throws<ArgumentException>(() => new PropertiesViewModel([], settings, fileTypeInfo));
    }

    [Fact]
    public void SingleFileShowsItsProperties()
    {
        var file = Readme;
        var fat = (IFatFileSystemObject)file;

        var vm = Create(file);

        Assert.False(vm.IsMultiple);
        Assert.True(vm.IsSingle);
        Assert.False(vm.IsReadOnly);
        Assert.Equal("README.TXT", vm.Name);
        Assert.True(vm.CanEditName);
        Assert.Equal("", vm.SelectionSummary);
        Assert.Equal("type of README.TXT", vm.TypeName);
        Assert.Equal(file.DirectoryName, vm.Location);
        Assert.True(vm.IsFatInfoAvailable);
        Assert.Equal(fat.ShortName, vm.ShortName);
        Assert.Equal(fat.FirstCluster.ToString(), vm.FirstCluster);
        Assert.False(vm.IsContainsAvailable);
        Assert.Equal(PropertiesViewModel.NotApplicable, vm.ContainsText);
    }

    [Fact]
    public void SingleFileSizeIsKnownWithoutLoading()
    {
        var vm = Create(Readme);

        Assert.True(vm.IsMeasured);
        Assert.Equal(SizeUnit.Bytes.FormatSize(1000, false), vm.SizeText);
        Assert.Equal(SizeUnit.Bytes.FormatSize(1024, false), vm.SizeOnDiskText);
    }

    [Fact]
    public void SizesUseTheSizeUnitFromTheSettings()
    {
        settings.Current.SizeUnit = SizeUnit.Binary;

        var vm = Create(Readme);

        Assert.Equal(SizeUnit.Binary.FormatSize(1000, true), vm.SizeText);
        Assert.Equal(SizeUnit.Binary.FormatSize(1024, true), vm.SizeOnDiskText);
    }

    [Fact]
    public void SingleFileShowsItsDates()
    {
        var vm = Create(Readme);

        Assert.True(vm.Created.IsSet);
        Assert.Equal(Created, vm.Created.Value);
        Assert.Equal(EntryDateViewModel.DateTimePattern, vm.Created.Format);
        Assert.True(vm.Modified.IsSet);
        Assert.Equal(Written, vm.Modified.Value);
        Assert.Equal(EntryDateViewModel.DateTimePattern, vm.Modified.Format);
        Assert.True(vm.Accessed.IsSet);
        Assert.Equal(Accessed, vm.Accessed.Value);
        // FAT stores the last access date without a time.
        Assert.Equal(EntryDateViewModel.DatePattern, vm.Accessed.Format);
        foreach (var date in new[] { vm.Created, vm.Modified, vm.Accessed })
        {
            Assert.True(date.CanToggle);
            Assert.True(date.CanEditValue);
        }
    }

    [Fact]
    public void SingleFileShowsItsAttributes()
    {
        var vm = Create(Readme);

        Assert.True(vm.CanEditAttributes);
        Assert.True(vm.ReadOnly);
        Assert.False(vm.Hidden);
        Assert.False(vm.System);
        Assert.True(vm.Archive);
    }

    [Fact]
    public void AcceptWithoutChangesAnswersWithTheCurrentValues()
    {
        var vm = Create(Readme);

        Assert.True(vm.AcceptCommand.CanExecute(null));
        vm.AcceptCommand.Execute(null);

        Assert.Equal(new EntryPropertiesChange("README.TXT", Created, Written, Accessed, true, false, false, true), vm.Answer);
    }

    [Fact]
    public void AcceptAnswersWithTheEditedValues()
    {
        var vm = Create(Readme);
        var newWriteTime = new DateTime(2020, 1, 2, 3, 4, 6);

        vm.Name = "RENAMED.TXT";
        vm.Created.IsSet = false;
        vm.Modified.Value = newWriteTime;
        vm.ReadOnly = false;
        vm.Hidden = true;
        vm.System = true;
        vm.Archive = false;
        vm.AcceptCommand.Execute(null);

        Assert.Equal(new EntryPropertiesChange("RENAMED.TXT", null, newWriteTime, Accessed, false, true, true, false), vm.Answer);
    }

    [Fact]
    public void SingleDirectoryIsMeasuredWhenLoaded()
    {
        var vm = Create(Docs);

        Assert.False(vm.IsMeasured);
        Assert.True(vm.IsContainsAvailable);
        Assert.Equal(PropertiesViewModel.Measuring, vm.SizeText);
        Assert.Equal(PropertiesViewModel.Measuring, vm.SizeOnDiskText);
        Assert.Equal(PropertiesViewModel.Measuring, vm.ContainsText);
        Assert.Equal("type of DOCS", vm.TypeName);
        Assert.Equal(Docs.Parent!.FullName, vm.Location);
    }

    [Fact]
    public async Task LoadMeasuresASingleDirectoryRecursively()
    {
        var vm = Create(Docs);
        var changed = new List<string?>();
        vm.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

        await vm.LoadCommand.ExecuteAsync(null);

        Assert.True(vm.IsMeasured);
        Assert.Equal("Files: 2, subdirectories: 1", vm.ContainsText);
        Assert.Equal(SizeUnit.Bytes.FormatSize(2100, false), vm.SizeText);
        Assert.Equal(SizeUnit.Bytes.FormatSize(512 + 2048, false), vm.SizeOnDiskText);
        Assert.Contains(nameof(PropertiesViewModel.SizeText), changed);
        Assert.Contains(nameof(PropertiesViewModel.SizeOnDiskText), changed);
        Assert.Contains(nameof(PropertiesViewModel.ContainsText), changed);
        Assert.Contains(nameof(PropertiesViewModel.IsMeasured), changed);
    }

    [Fact]
    public async Task LoadDoesNothingForASingleFile()
    {
        var vm = Create(Readme);
        var changed = new List<string?>();
        vm.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

        await vm.LoadCommand.ExecuteAsync(null);

        Assert.Empty(changed);
    }

    [Fact]
    public async Task LoadAfterStoppingLeavesTheSizesUnknown()
    {
        var vm = Create(Docs);

        vm.StopBackgroundWork();
        await vm.LoadCommand.ExecuteAsync(null);

        Assert.False(vm.IsMeasured);
        Assert.Equal(PropertiesViewModel.Measuring, vm.SizeText);
    }

    [Fact]
    public void HashesOfASingleFileStartUncalculated()
    {
        var vm = Create(Readme);

        Assert.True(vm.AreHashesAvailable);
        Assert.Equal(PropertiesViewModel.HashPrompt, vm.Crc32Text);
        Assert.Equal(PropertiesViewModel.HashPrompt, vm.Md5Text);
        Assert.Equal(PropertiesViewModel.HashPrompt, vm.Sha1Text);
        Assert.True(vm.CalculateHashesCommand.CanExecute(null));
    }

    [Fact]
    public async Task CalculatesTheHashesOnce()
    {
        var vm = Create(Readme);
        var texts = new List<string>();
        vm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(PropertiesViewModel.Md5Text))
                texts.Add(vm.Md5Text);
        };
        var canExecuteChanged = 0;
        vm.CalculateHashesCommand.CanExecuteChanged += (_, _) => canExecuteChanged++;

        await vm.CalculateHashesCommand.ExecuteAsync(null);

        Assert.Equal(Convert.ToHexStringLower(MD5.HashData(ReadmeContents)), vm.Md5Text);
        Assert.Equal(Convert.ToHexStringLower(SHA1.HashData(ReadmeContents)), vm.Sha1Text);
        Assert.Equal(await HashCalculator.CalculateCrc32HashAsync(new MemoryStream(ReadmeContents)), vm.Crc32Text);
        Assert.Equal([PropertiesViewModel.HashCalculating, vm.Md5Text], texts);
        Assert.False(vm.CalculateHashesCommand.CanExecute(null));
        Assert.True(canExecuteChanged > 0);
    }

    [Fact]
    public async Task StoppedHashingKeepsWaitingText()
    {
        var vm = Create(Readme);

        vm.StopBackgroundWork();
        await vm.CalculateHashesCommand.ExecuteAsync(null);

        Assert.Equal(PropertiesViewModel.HashCalculating, vm.Md5Text);
        Assert.False(vm.CalculateHashesCommand.CanExecute(null));
    }

    [Fact]
    public void DirectoriesHaveNoHashes()
    {
        var vm = Create(Docs);

        Assert.False(vm.AreHashesAvailable);
        Assert.Equal(PropertiesViewModel.NotApplicable, vm.Md5Text);
        Assert.False(vm.CalculateHashesCommand.CanExecute(null));
    }

    [Fact]
    public void ClearingADateHidesIt()
    {
        var vm = Create(Readme);
        var changed = new List<string?>();
        vm.Modified.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

        vm.Modified.IsSet = false;

        Assert.False(vm.Modified.CanEditValue);
        Assert.Null(vm.Modified.Format);
        Assert.Null(vm.Modified.Result);
        Assert.Contains(nameof(EntryDateViewModel.CanEditValue), changed);
        Assert.Contains(nameof(EntryDateViewModel.Format), changed);
    }

    [Fact]
    public void SettingADateStartsAtTheCurrentTime()
    {
        var vm = Create(Readme);
        vm.Created.IsSet = false;

        var before = DateTime.Now;
        vm.Created.IsSet = true;
        var after = DateTime.Now;

        Assert.InRange(vm.Created.Value, before, after);
        Assert.True(vm.Created.CanEditValue);
        Assert.Equal(EntryDateViewModel.DateTimePattern, vm.Created.Format);
        Assert.Equal(vm.Created.Value, vm.Created.Result);
    }

    [Fact]
    public void AnUnsetDateShowsNothing()
    {
        var date = new EntryDateViewModel(null, canToggle: true, EntryDateViewModel.DateTimePattern);

        Assert.False(date.IsSet);
        Assert.False(date.CanEditValue);
        Assert.Null(date.Format);
        Assert.Null(date.Result);
    }

    [Fact]
    public void MultipleEntriesShowASummary()
    {
        var vm = Create(Readme, Docs);

        Assert.True(vm.IsMultiple);
        Assert.False(vm.IsSingle);
        Assert.Equal("", vm.Name);
        Assert.False(vm.CanEditName);
        Assert.Equal("Files: 1, directories: 1", vm.SelectionSummary);
        Assert.Equal("Multiple types", vm.TypeName);
        Assert.Equal(Readme.DirectoryName, vm.Location);
        Assert.False(vm.IsFatInfoAvailable);
        Assert.Equal(PropertiesViewModel.NotApplicable, vm.ShortName);
        Assert.Equal(PropertiesViewModel.NotApplicable, vm.FirstCluster);
        Assert.False(vm.IsContainsAvailable);
        Assert.Equal(PropertiesViewModel.NotApplicable, vm.ContainsText);
        Assert.False(vm.AreHashesAvailable);
        Assert.Equal(PropertiesViewModel.NotApplicable, vm.Crc32Text);
        Assert.False(vm.CalculateHashesCommand.CanExecute(null));
        Assert.True(vm.CanEditAttributes);
    }

    [Fact]
    public void MultipleEntriesHaveNoDates()
    {
        var vm = Create(Readme, Notes);

        foreach (var date in new[] { vm.Created, vm.Modified, vm.Accessed })
        {
            Assert.False(date.IsSet);
            Assert.False(date.CanToggle);
            Assert.False(date.CanEditValue);
            Assert.Null(date.Format);
        }
    }

    [Fact]
    public async Task LoadMeasuresMultipleEntries()
    {
        var vm = Create(Readme, Docs);
        Assert.Equal(PropertiesViewModel.Measuring, vm.SizeText);

        await vm.LoadCommand.ExecuteAsync(null);

        Assert.Equal(SizeUnit.Bytes.FormatSize(1000 + 2100, false), vm.SizeText);
        Assert.Equal(SizeUnit.Bytes.FormatSize(1024 + 512 + 2048, false), vm.SizeOnDiskText);
        Assert.Equal(PropertiesViewModel.NotApplicable, vm.ContainsText);
    }

    [Fact]
    public void AttributesThatDifferAreIndeterminate()
    {
        // README is read-only, NOTES is hidden, both are archive (added files always are).
        var vm = Create(Readme, Notes);

        Assert.Null(vm.ReadOnly);
        Assert.Null(vm.Hidden);
        Assert.False(vm.System);
        Assert.True(vm.Archive);
    }

    [Fact]
    public void AttributesSharedByAllEntriesAreSet()
    {
        var vm = Create(Readme, (TiFile)image.Get("DOCS", "A.TXT"));

        Assert.Null(vm.ReadOnly);
        Assert.True(vm.Archive);
        Assert.False(vm.Hidden);
    }

    [Fact]
    public void MultipleEntriesAnswerWithoutNameOrDates()
    {
        var vm = Create(Readme, Notes);

        vm.System = true;
        vm.AcceptCommand.Execute(null);

        Assert.Equal(new EntryPropertiesChange(null, null, null, null, null, null, true, true), vm.Answer);
    }

    [Fact]
    public void FilesWithTheSameExtensionShareTheirType()
    {
        var vm = Create(Readme, Notes);

        Assert.Equal("type of README.TXT", vm.TypeName);
    }

    [Fact]
    public void DirectoriesShareTheirType()
    {
        var vm = Create(Docs, image.Get("EMPTY"));

        Assert.Equal("type of DOCS", vm.TypeName);
        Assert.Equal("Files: 0, directories: 2", vm.SelectionSummary);
    }

    [Fact]
    public void EntriesOnAReadOnlyFileSystemCannotBeEdited()
    {
        var fileSystem = new ReadOnlyTestFileSystem();
        var vm = Create(fileSystem.File);

        Assert.True(vm.IsReadOnly);
        Assert.False(vm.CanEditName);
        Assert.False(vm.CanEditAttributes);
        Assert.False(vm.Modified.CanToggle);
        Assert.False(vm.Modified.CanEditValue);
        // The values are still shown.
        Assert.True(vm.Modified.IsSet);
        Assert.Equal(EntryDateViewModel.DateTimePattern, vm.Modified.Format);
        // Not a FAT entry: no short name, and the access time has a time.
        Assert.False(vm.IsFatInfoAvailable);
        Assert.Equal(PropertiesViewModel.NotApplicable, vm.ShortName);
        Assert.Equal(EntryDateViewModel.DateTimePattern, vm.Accessed.Format);
        // Hashes can still be calculated.
        Assert.True(vm.CalculateHashesCommand.CanExecute(null));
    }

    [Fact]
    public void OneReadOnlyEntryMakesAllReadOnly()
    {
        var vm = Create(Readme, new ReadOnlyTestFileSystem().File);

        Assert.True(vm.IsReadOnly);
        Assert.False(vm.CanEditAttributes);
    }

    [Fact]
    public void GenericTypeNames()
    {
        var types = new GenericFileTypeInfo();

        Assert.Equal("File folder", types.GetTypeName("DOCS", FileAttributes.Directory));
        Assert.Equal("TXT File", types.GetTypeName("readme.txt", FileAttributes.Archive));
        Assert.Equal("File", types.GetTypeName("README", FileAttributes.Archive));
    }

    [Fact]
    public void ReparsePointsAreNamedByTheirKind()
    {
        var types = new GenericFileTypeInfo();

        Assert.Equal("Junction", types.GetTypeName("LINK", FileAttributes.Directory | FileAttributes.ReparsePoint));
        Assert.Equal("Symbolic link", types.GetTypeName("link.txt", FileAttributes.Archive | FileAttributes.ReparsePoint));
        Assert.Null(GenericFileTypeInfo.GetReparsePointTypeName(FileAttributes.Directory));
    }

    [Fact]
    public async Task MeasurementsCountSelectedAndContainedEntries()
    {
        var measured = await EntryMeasurements.MeasureAsync([Readme, Notes, Docs, image.Get("EMPTY")]);

        Assert.Equal(new EntryMeasurements(2, 2, 2, 1, 1000 + 10 + 2100, 1024 + 512 + 512 + 2048), measured);
    }
}

/// <summary>
/// A 1.44 MB FAT12 floppy made with <see cref="RawContainer.CreateImage"/>, with entries added
/// through its change set. The changes are committed to a temporary file, which is opened again.
/// </summary>
internal sealed class PropertiesTestImage : IDisposable
{
    public delegate void AddEntry(string[] path, byte[]? contents, FatAttributes attributes);

    private static readonly DateTime Created = new(2001, 2, 3, 4, 5, 6);
    private static readonly DateTime Written = new(2002, 3, 4, 5, 6, 8);
    private static readonly DateTime Accessed = new(2003, 4, 5);

    private readonly TempFolder folder = new();
    private readonly RawContainer container;

    public PropertiesTestImage(Action<AddEntry> build)
    {
        var geometry = FloppyGeometry.KnownGeometries[FloppyGeometry.FriendlyName.HighDensity1440k];
        var bpb = BiosParameterBlock.FromGeometry(geometry, BiosParameterBlockVersion.Dos20, "TESTOEM ");
        var created = RawContainer.CreateImage(bpb, geometry.Tracks, true);
        var directories = new HashSet<string>();

        build((path, contents, attributes) =>
        {
            // Create the parent directories (and the entry itself if it is a directory) first.
            var directoryCount = contents is null ? path.Length : path.Length - 1;
            for (var i = 1; i <= directoryCount; i++)
            {
                if (directories.Add(string.Join('\\', path[..i])))
                    created.PendingChanges.Add(new CreateDirectoryChange(path[..i]));
            }

            if (contents is not null)
            {
                var source = FileDataSource.Create(new MemoryStream(contents), folder.Path, long.MaxValue);
                created.PendingChanges.Add(new AddFileChange(path, source, attributes, Created, Written, Accessed));
            }
        });

        var imagePath = folder.File("test.img");
        created.CommitChanges(imagePath).GetAwaiter().GetResult();
        container = new RawContainer(imagePath, false);
    }

    public TiFileSystem FileSystem => container.PartitionTable.Partitions[0].FileSystem;

    public TiFileSystemObject Get(params string[] path)
    {
        TiFileSystemObject current = FileSystem.RootDirectory;
        foreach (var name in path)
            current = ((TiDirectory)current).EnumerateFileSystemObjects(true).Single(e => e.Name == name);
        return current;
    }

    public void Dispose()
    {
        container.Dispose();
        folder.Dispose();
    }
}

/// <summary>
/// A read-only file system that is not FAT, holding one 10-byte file with all dates set.
/// </summary>
internal sealed class ReadOnlyTestFileSystem : TiFileSystem
{
    private readonly RootDirectoryEntry root;

    public ReadOnlyTestFileSystem() : base(Stream.Null)
    {
        root = new RootDirectoryEntry(this);
        File = new FileEntry(this, root);
    }

    public TiFile File { get; }

    public override string DisplayName => "Test";
    public override string VolumeLabel { get => ""; set => throw new NotSupportedException(); }
    public override TiDirectory RootDirectory => root;
    public override long TotalFreeSpace => 0;
    public override long TotalSize => 0;
    public override long AllocationUnitSize => 512;
    public override bool SupportsSubdirectories => true;
    public override bool IsReadOnly => true;

    private sealed class RootDirectoryEntry(TiFileSystem fileSystem) : TiDirectory(fileSystem, null)
    {
        public override string Name { get => ""; set => throw new NotSupportedException(); }
        public override FileAttributes Attributes { get => FileAttributes.Directory; set => throw new NotSupportedException(); }
        public override DateTime? LastAccessTime { get => null; set => throw new NotSupportedException(); }
        public override DateTime? LastWriteTime { get => null; set => throw new NotSupportedException(); }
        public override DateTime? CreationTime { get => null; set => throw new NotSupportedException(); }
        public override ulong Length { get => 0; set => throw new NotSupportedException(); }
        public override IEnumerable<TiFileSystemObject> EnumerateFileSystemObjects(bool showHidden) => [((ReadOnlyTestFileSystem)FileSystem).File];
        public override TiDirectory CreateSubdirectory(string path) => throw new NotSupportedException();
        public override void Delete() => throw new NotSupportedException();
        public override void MoveTo(string path) => throw new NotSupportedException();
    }

    private sealed class FileEntry(TiFileSystem fileSystem, TiDirectory directory) : TiFile(fileSystem, directory)
    {
        public override string Name { get => "DATA.BIN"; set => throw new NotSupportedException(); }
        public override FileAttributes Attributes { get => FileAttributes.Archive; set => throw new NotSupportedException(); }
        public override DateTime? LastAccessTime { get => new DateTime(2003, 4, 5, 6, 7, 8); set => throw new NotSupportedException(); }
        public override DateTime? LastWriteTime { get => new DateTime(2002, 3, 4, 5, 6, 7); set => throw new NotSupportedException(); }
        public override DateTime? CreationTime { get => new DateTime(2001, 2, 3, 4, 5, 6); set => throw new NotSupportedException(); }
        public override ulong Length { get => 10; set => throw new NotSupportedException(); }
        public override Stream GetStream() => new MemoryStream(new byte[10]);
        public override void Delete() => throw new NotSupportedException();
        public override void MoveTo(string path) => throw new NotSupportedException();
    }
}

internal sealed class PropertiesFakeSettings : ISettingsService
{
    public SettingsModel Current { get; } = new();
    public event EventHandler? ExternallyChanged { add { } remove { } }
    public void Save() { }
    public void Reload() { }
    public void ResetToDefaults() { }
    public void AddRecentImage(string path) { }
    public void RemoveRecentImage(string path) { }
    public void ClearRecentImages() { }
    public void RemoveMissingRecentImages() { }
}

internal sealed class PropertiesFakeFileTypeInfo : IFileTypeInfo
{
    public string GetTypeName(string name, FileAttributes attributes) => $"type of {name}";
}
