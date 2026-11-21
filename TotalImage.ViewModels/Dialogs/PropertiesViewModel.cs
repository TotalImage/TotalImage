using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TotalImage.FileSystems.ISO;
using TotalImage.ViewModels.Entries;
using TotalImage.ViewModels.Formatting;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Settings;
using TiDirectory = TotalImage.FileSystems.Directory;
using TiFile = TotalImage.FileSystems.File;
using TiFileSystemObject = TotalImage.FileSystems.FileSystemObject;

namespace TotalImage.ViewModels.Dialogs;

/// <summary>
/// The values in the properties dialog when it was accepted.
/// </summary>
/// <param name="Name">The name of the entry, or <c>null</c> when several entries were shown.</param>
/// <param name="CreationTime">The creation time, or <c>null</c> when it is not set.</param>
/// <param name="LastWriteTime">The last write time, or <c>null</c> when it is not set.</param>
/// <param name="LastAccessTime">The last access time, or <c>null</c> when it is not set.</param>
/// <param name="ReadOnly">The read-only attribute, or <c>null</c> when it differs between the entries and was left alone.</param>
/// <param name="Hidden">The hidden attribute, or <c>null</c> as for <paramref name="ReadOnly"/>.</param>
/// <param name="System">The system attribute, or <c>null</c> as for <paramref name="ReadOnly"/>.</param>
/// <param name="Archive">The archive attribute, or <c>null</c> as for <paramref name="ReadOnly"/>.</param>
public sealed record EntryPropertiesChange(
    string? Name,
    DateTime? CreationTime, DateTime? LastWriteTime, DateTime? LastAccessTime,
    bool? ReadOnly, bool? Hidden, bool? System, bool? Archive);

/// <summary>
/// One of the dates in the properties dialog: whether it is set, and its value.
/// </summary>
public partial class EntryDateViewModel : ObservableObject
{
    /// <summary>
    /// The pattern for a date with time.
    /// </summary>
    public const string DateTimePattern = "yyyy-MM-dd HH:mm:ss";

    /// <summary>
    /// The pattern for a date alone (FAT stores no time for the last access).
    /// </summary>
    public const string DatePattern = "yyyy-MM-dd";

    private readonly string pattern;

    internal EntryDateViewModel(DateTime? value, bool canToggle, string pattern)
    {
        this.pattern = pattern;
        CanToggle = canToggle;
        IsSet = value.HasValue;
        Value = value ?? DateTime.Now;
    }

    /// <summary>
    /// Whether the date can be set or cleared, and so whether <see cref="IsSet"/> can change.
    /// </summary>
    public bool CanToggle { get; }

    /// <summary>
    /// Whether the entry has this date.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanEditValue), nameof(Format))]
    public partial bool IsSet { get; set; }

    /// <summary>
    /// The date. Only meaningful while <see cref="IsSet"/> is true.
    /// </summary>
    [ObservableProperty]
    public partial DateTime Value { get; set; }

    /// <summary>
    /// Whether <see cref="Value"/> can be edited.
    /// </summary>
    public bool CanEditValue => CanToggle && IsSet;

    /// <summary>
    /// The custom date format pattern to show <see cref="Value"/> with, or <c>null</c> to show
    /// nothing because the date is not set.
    /// </summary>
    public string? Format => IsSet ? pattern : null;

    /// <summary>
    /// The date for the answer: <see cref="Value"/> if set, otherwise <c>null</c>.
    /// </summary>
    public DateTime? Result => IsSet ? Value : null;

    partial void OnIsSetChanged(bool value)
    {
        // A newly set date starts at the current time.
        if (value)
            Value = DateTime.Now;
    }
}

/// <summary>
/// Shows the properties of one or more entries and lets the user edit the name (of a single
/// entry), the dates and the attributes. Sizes of directories are measured in the background by
/// <see cref="LoadCommand"/>; hashes of a single file are calculated on request.
/// </summary>
public partial class PropertiesViewModel : DialogViewModel<EntryPropertiesChange>
{
    /// <summary>
    /// The text shown for values that do not apply.
    /// </summary>
    public const string NotApplicable = "N/A";

    /// <summary>
    /// The text shown in the size fields while the entries are measured.
    /// </summary>
    public const string Measuring = "Calculating...";

    /// <summary>
    /// The text in the hash fields before the hashes are calculated.
    /// </summary>
    public const string HashPrompt = "Click to calculate";

    /// <summary>
    /// The text in the hash fields while the hashes are calculated.
    /// </summary>
    public const string HashCalculating = "Please wait...";

    private readonly IReadOnlyList<TiFileSystemObject> entries;
    private readonly SizeUnit sizeUnit;
    private readonly CancellationTokenSource cancellation = new();
    private bool hashesStarted;

    public PropertiesViewModel(IReadOnlyList<TiFileSystemObject> entries, ISettingsService settings, IFileTypeInfo fileTypeInfo)
    {
        if (entries is null || entries.Count == 0)
            throw new ArgumentException("There must be at least one entry.", nameof(entries));

        this.entries = entries;
        sizeUnit = settings.Current.SizeUnit;
        var first = entries[0];

        IsMultiple = entries.Count > 1;
        IsReadOnly = entries.Any(e => e.FileSystem.IsReadOnly || e.FileSystem.OwningContainer?.SupportsWriting == false);
        var isIso = !IsMultiple && first is IsoFile or IsoDirectory;
        CanEditName = !IsMultiple && !isIso && !IsReadOnly;
        CanEditAttributes = !isIso && !IsReadOnly;
        var canEditDates = !IsMultiple && !isIso && !IsReadOnly;

        Name = IsMultiple ? string.Empty : first.Name;
        SelectionSummary = IsMultiple
            ? $"Files: {entries.Count(e => e is not TiDirectory)}, directories: {entries.Count(e => e is TiDirectory)}"
            : string.Empty;
        TypeName = IsMultiple && HaveDifferentTypes(entries)
            ? "Multiple types"
            : fileTypeInfo.GetTypeName(first.Name, first.Attributes);
        Location = first switch
        {
            TiFile file => file.DirectoryName,
            TiDirectory directory => directory.Parent?.FullName ?? string.Empty,
            _ => string.Empty
        };

        IsFatInfoAvailable = !IsMultiple && first is IFatFileSystemObject;
        ShortName = IsFatInfoAvailable ? ((IFatFileSystemObject)first).ShortName : NotApplicable;
        FirstCluster = IsFatInfoAvailable ? ((IFatFileSystemObject)first).FirstCluster.ToString() : NotApplicable;

        IsContainsAvailable = !IsMultiple && first is TiDirectory;
        if (!IsMultiple && first is TiFile)
        {
            // A single file's size is known without scanning anything.
            SizeText = FormatSize(first.Length);
            SizeOnDiskText = FormatSize(first.LengthOnDisk);
            ContainsText = NotApplicable;
            IsMeasured = true;
        }
        else
        {
            SizeText = SizeOnDiskText = Measuring;
            ContainsText = IsContainsAvailable ? Measuring : NotApplicable;
        }

        AreHashesAvailable = !IsMultiple && first is TiFile;
        Crc32Text = Md5Text = Sha1Text = AreHashesAvailable ? HashPrompt : NotApplicable;

        var accessedPattern = first is IFatFileSystemObject ? EntryDateViewModel.DatePattern : EntryDateViewModel.DateTimePattern;
        Created = new(IsMultiple ? null : first.CreationTime, canEditDates, EntryDateViewModel.DateTimePattern);
        Modified = new(IsMultiple ? null : first.LastWriteTime, canEditDates, EntryDateViewModel.DateTimePattern);
        Accessed = new(IsMultiple ? null : first.LastAccessTime, canEditDates, accessedPattern);

        ReadOnly = CombineAttribute(FileAttributes.ReadOnly);
        Hidden = CombineAttribute(FileAttributes.Hidden);
        System = CombineAttribute(FileAttributes.System);
        Archive = CombineAttribute(FileAttributes.Archive);
    }

    /// <summary>
    /// Whether several entries are shown. Then the name is replaced by <see cref="SelectionSummary"/>.
    /// </summary>
    public bool IsMultiple { get; }

    /// <summary>
    /// Whether a single entry is shown.
    /// </summary>
    public bool IsSingle => !IsMultiple;

    /// <summary>
    /// Whether any entry is on a file system or container that can't be changed.
    /// </summary>
    public bool IsReadOnly { get; }

    /// <summary>
    /// The name of a single entry.
    /// </summary>
    [ObservableProperty]
    public partial string Name { get; set; }

    /// <summary>
    /// Whether <see cref="Name"/> can be edited.
    /// </summary>
    public bool CanEditName { get; }

    /// <summary>
    /// For several entries, how many files and directories are selected.
    /// </summary>
    public string SelectionSummary { get; }

    /// <summary>
    /// The type of the entries, or "Multiple types".
    /// </summary>
    public string TypeName { get; }

    /// <summary>
    /// The folder that contains the (first) entry.
    /// </summary>
    public string Location { get; }

    /// <summary>
    /// The total size, or <see cref="Measuring"/> until <see cref="LoadCommand"/> finished.
    /// </summary>
    [ObservableProperty]
    public partial string SizeText { get; private set; }

    /// <summary>
    /// The total size on disk, or <see cref="Measuring"/> until <see cref="LoadCommand"/> finished.
    /// </summary>
    [ObservableProperty]
    public partial string SizeOnDiskText { get; private set; }

    /// <summary>
    /// Whether <see cref="ContainsText"/> applies: a single directory is shown.
    /// </summary>
    public bool IsContainsAvailable { get; }

    /// <summary>
    /// What a single directory contains, at any depth.
    /// </summary>
    [ObservableProperty]
    public partial string ContainsText { get; private set; }

    /// <summary>
    /// Whether the sizes are known.
    /// </summary>
    [ObservableProperty]
    public partial bool IsMeasured { get; private set; }

    /// <summary>
    /// Whether <see cref="ShortName"/> and <see cref="FirstCluster"/> apply: a single FAT entry is shown.
    /// </summary>
    public bool IsFatInfoAvailable { get; }

    /// <summary>
    /// The 8.3 name of a FAT entry.
    /// </summary>
    public string ShortName { get; }

    /// <summary>
    /// The first cluster of a FAT entry.
    /// </summary>
    public string FirstCluster { get; }

    /// <summary>
    /// Whether hashes can be calculated: a single file is shown.
    /// </summary>
    public bool AreHashesAvailable { get; }

    [ObservableProperty]
    public partial string Crc32Text { get; private set; }

    [ObservableProperty]
    public partial string Md5Text { get; private set; }

    [ObservableProperty]
    public partial string Sha1Text { get; private set; }

    public EntryDateViewModel Created { get; }

    public EntryDateViewModel Modified { get; }

    public EntryDateViewModel Accessed { get; }

    /// <summary>
    /// Whether the attributes can be edited.
    /// </summary>
    public bool CanEditAttributes { get; }

    /// <summary>
    /// The read-only attribute; <c>null</c> when it differs between the entries.
    /// </summary>
    [ObservableProperty]
    public partial bool? ReadOnly { get; set; }

    [ObservableProperty]
    public partial bool? Hidden { get; set; }

    [ObservableProperty]
    public partial bool? System { get; set; }

    [ObservableProperty]
    public partial bool? Archive { get; set; }

    /// <summary>
    /// Measures the sizes of directories (and of several entries) in the background. The view
    /// runs it once when it is shown.
    /// </summary>
    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsMeasured)
            return;

        EntryMeasurements measured;
        try
        {
            measured = await EntryMeasurements.MeasureAsync(entries, cancellation.Token);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        SizeText = FormatSize(measured.Size);
        SizeOnDiskText = FormatSize(measured.SizeOnDisk);
        if (IsContainsAvailable)
            ContainsText = $"Files: {measured.ContainedFiles}, subdirectories: {measured.ContainedDirectories}";
        IsMeasured = true;
    }

    /// <summary>
    /// Calculates the CRC-32, MD5 and SHA-1 hashes of a single file, once.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanCalculateHashes))]
    private async Task CalculateHashesAsync()
    {
        hashesStarted = true;
        CalculateHashesCommand.NotifyCanExecuteChanged();
        Crc32Text = Md5Text = Sha1Text = HashCalculating;

        try
        {
            var hashes = await FileHashes.CalculateAsync((TiFile)entries[0], cancellation.Token);
            Crc32Text = hashes.Crc32;
            Md5Text = hashes.Md5;
            Sha1Text = hashes.Sha1;
        }
        catch (OperationCanceledException)
        {
            // The dialog was closed; nobody sees the result.
        }
    }

    private bool CanCalculateHashes() => AreHashesAvailable && !hashesStarted;

    /// <summary>
    /// Stops measuring and hashing. The view calls this when it closes.
    /// </summary>
    public void StopBackgroundWork() => cancellation.Cancel();

    protected override EntryPropertiesChange CreateAnswer() => new(
        IsMultiple ? null : Name,
        Created.Result, Modified.Result, Accessed.Result,
        ReadOnly, Hidden, System, Archive);

    private string FormatSize(ulong size) => sizeUnit.FormatSize(size, sizeUnit != SizeUnit.Bytes);

    private bool? CombineAttribute(FileAttributes attribute)
    {
        var count = entries.Count(e => e.Attributes.HasFlag(attribute));
        return count == 0 ? false : count == entries.Count ? true : null;
    }

    // Entries have different types when their extensions differ, or when files and directories are mixed.
    private static bool HaveDifferentTypes(IReadOnlyList<TiFileSystemObject> entries)
    {
        static string TypeKey(TiFileSystemObject entry) => entry is TiDirectory ? "folder" : Path.GetExtension(entry.Name);

        var first = TypeKey(entries[0]);
        return entries.Any(e => TypeKey(e) != first);
    }
}
