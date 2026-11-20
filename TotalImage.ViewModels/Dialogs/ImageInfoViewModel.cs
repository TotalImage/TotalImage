using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TotalImage.Containers;
using TotalImage.Containers.Anex86;
using TotalImage.Containers.NHD;
using TotalImage.Containers.VHD;
using TotalImage.FileSystems.BPB;
using TotalImage.FileSystems.FAT;
using TotalImage.FileSystems.ISO;
using TotalImage.Partitions;
using TotalImage.ViewModels.Formatting;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Settings;

namespace TotalImage.ViewModels.Dialogs;

/// <summary>
/// The part of an image a property describes. Each group is one page of the image information.
/// </summary>
public enum PropertyGroup
{
    File,
    Container,
    PartitionTable,
    Partition,
    FileSystem
}

/// <summary>
/// One property of the image, ready to show.
/// </summary>
/// <param name="Group">The part of the image it describes.</param>
/// <param name="Name">The property name.</param>
/// <param name="Value">The formatted value, <see cref="NotAvailable"/> when there is none.</param>
/// <param name="IsPending">The value is still being calculated.</param>
public sealed record PropertyRow(PropertyGroup Group, string Name, string Value, bool IsPending = false)
{
    /// <summary>
    /// The value shown when a property doesn't apply or is unknown.
    /// </summary>
    public const string NotAvailable = "N/A";

    /// <summary>
    /// Whether the property has no value.
    /// </summary>
    public bool IsNotAvailable => Value == NotAvailable;
}

/// <summary>
/// Information about the open image: properties of its file, container, partition table,
/// selected partition and file system, hashes of the image file, and an export to a text report.
/// </summary>
public partial class ImageInfoViewModel : ObservableObject
{
    /// <summary>
    /// The value of a hash row while it is being calculated.
    /// </summary>
    public const string PendingHashText = "Please wait...";

    /// <summary>
    /// The comment shown for containers that can't store one.
    /// </summary>
    public const string NoCommentText = "This container type does not support comments.";

    /// <summary>
    /// The file types offered when exporting the report.
    /// </summary>
    public static readonly IReadOnlyList<FileTypeFilter> ReportFileTypes =
    [
        new("Plain text file", ["*.txt"]),
        ImageFileTypes.AllFiles
    ];

    /// <summary>
    /// The extension of an exported report whose name has none.
    /// </summary>
    public const string ReportExtension = "txt";

    private static readonly IReadOnlyList<(ImageHashKind Kind, string Name)> Hashes =
    [
        (ImageHashKind.Crc32, "CRC-32 hash"),
        (ImageHashKind.Md5, "MD5 hash"),
        (ImageHashKind.Sha1, "SHA-1 hash")
    ];

    private readonly Container image;
    private readonly IImageHashService hashService;
    private readonly IFilePickerService filePicker;
    private readonly IMessageService messages;
    private readonly Assembly? applicationAssembly;
    private readonly TimeProvider timeProvider;
    private readonly SizeUnit sizeUnit;

    private readonly ObservableCollection<PropertyRow> fileRows;

    /// <param name="image">The open image.</param>
    /// <param name="filePath">The image file, or <c>null</c> when the image hasn't been saved yet.</param>
    /// <param name="partitionIndex">The selected partition.</param>
    /// <param name="settings">Supplies the size unit.</param>
    /// <param name="hashService">Calculates the hashes of the image file.</param>
    /// <param name="filePicker">Asks where to export the report.</param>
    /// <param name="messages">Shows warnings and errors.</param>
    /// <param name="applicationAssembly">The assembly whose version is written to the report, normally the entry assembly.</param>
    /// <param name="timeProvider">Supplies the report's creation time; the system clock when <c>null</c>.</param>
    public ImageInfoViewModel(Container image, string? filePath, int partitionIndex, ISettingsService settings,
        IImageHashService hashService, IFilePickerService filePicker, IMessageService messages,
        Assembly? applicationAssembly, TimeProvider? timeProvider = null)
    {
        this.image = image;
        this.hashService = hashService;
        this.filePicker = filePicker;
        this.messages = messages;
        this.applicationAssembly = applicationAssembly;
        this.timeProvider = timeProvider ?? TimeProvider.System;
        sizeUnit = settings.Current.SizeUnit;

        fileRows = new(CreateFileRows(filePath));
        FileRows = new(fileRows);
        ContainerRows = CreateContainerRows();
        PartitionTableRows = CreatePartitionTableRows(partitionIndex);
        PartitionRows = CreatePartitionRows(partitionIndex);
        FileSystemRows = CreateFileSystemRows(partitionIndex);

        SupportsComment = image is IContainerComment;
        Comment = image is IContainerComment containerComment ? containerComment.Comment ?? string.Empty : NoCommentText;
    }

    /// <summary>
    /// Properties of the image file. The hash rows change when <see cref="ComputeHashesCommand"/> finishes each hash.
    /// </summary>
    public ReadOnlyObservableCollection<PropertyRow> FileRows { get; }

    /// <summary>
    /// Properties of the container format.
    /// </summary>
    public IReadOnlyList<PropertyRow> ContainerRows { get; }

    /// <summary>
    /// Properties of the partition table.
    /// </summary>
    public IReadOnlyList<PropertyRow> PartitionTableRows { get; }

    /// <summary>
    /// Properties of the selected partition.
    /// </summary>
    public IReadOnlyList<PropertyRow> PartitionRows { get; }

    /// <summary>
    /// Properties of the file system in the selected partition.
    /// </summary>
    public IReadOnlyList<PropertyRow> FileSystemRows { get; }

    /// <summary>
    /// Whether the container can store a comment.
    /// </summary>
    public bool SupportsComment { get; }

    /// <summary>
    /// The container's comment, or <see cref="NoCommentText"/> when it can't store one.
    /// </summary>
    public string Comment { get; }

    /// <summary>
    /// Whether hashing has finished or was cancelled.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ComputeHashesCommand))]
    public partial bool HashesDone { get; private set; }

    /// <summary>
    /// Whether a row's value can be copied: it has a value that isn't still being calculated.
    /// </summary>
    public static bool CanCopyValue(PropertyRow? row)
        => row is not null && !row.IsPending && !row.IsNotAvailable && !string.IsNullOrWhiteSpace(row.Value);

    /// <summary>
    /// Calculates the hashes of the image file one after another on a background thread,
    /// updating each hash row as it finishes. Runs once; cancelling it also counts as done.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanComputeHashes))]
    private async Task ComputeHashesAsync(CancellationToken cancellationToken)
    {
        try
        {
            foreach (var (kind, name) in Hashes)
            {
                // ContainerFileStream is the file itself, not the (possibly decoded) image content.
                var value = await hashService.ComputeAsync(image.ContainerFileStream, kind, cancellationToken);
                var index = IndexOf(fileRows, name);
                fileRows[index] = fileRows[index] with { Value = value, IsPending = false };
            }
        }
        catch (OperationCanceledException)
        {
            // The dialog was closed before the hashes were done.
        }

        HashesDone = true;
    }

    private bool CanComputeHashes() => !HashesDone;

    /// <summary>
    /// Asks where to save a text report of all properties and writes it. Warns first when the
    /// hashes are not done, since they are left out of the report.
    /// </summary>
    [RelayCommand]
    private async Task ExportAsync()
    {
        if (!HashesDone && !await messages.ConfirmExportWithoutHashesAsync())
            return;

        var path = await filePicker.PickSaveFileAsync(ReportFileTypes, ReportExtension);
        if (path is null)
            return;

        try
        {
            await File.WriteAllTextAsync(path, BuildReport());
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            await messages.ShowErrorAsync($"Exporting to \"{path}\"", ex);
        }
    }

    /// <summary>
    /// The text report written by <see cref="ExportCommand"/>. Hash rows are left out until
    /// <see cref="HashesDone"/>.
    /// </summary>
    public string BuildReport()
    {
        var version = applicationAssembly?.GetName().Version;
        var gitHash = applicationAssembly?.GetCustomAttributes<AssemblyMetadataAttribute>()
            .FirstOrDefault(a => a.Key == "GitHash")?.Value;

        // Aside from the "created on" time at the top, dates and numbers are in the user's locale.
        StringBuilder sb = new();
        sb.AppendLine("************************************");
        sb.AppendLine("*            TotalImage            *");
        sb.AppendLine("*     Image information report     *");
        sb.AppendLine("************************************");
        sb.AppendLine($"-TotalImage version: {version} ({gitHash})");
        sb.AppendLine($"-Created on: {timeProvider.GetLocalNow():yyyy-MM-dd HH:mm:ss}");
        sb.AppendLine();

        var hashNames = Hashes.Select(h => h.Name).ToHashSet();
        AppendSection(sb, "File information", FileRows.Where(r => HashesDone || !hashNames.Contains(r.Name)));
        AppendSection(sb, "Container information", ContainerRows);
        AppendSection(sb, "Partition table information", PartitionTableRows);
        AppendSection(sb, "Selected partition information", PartitionRows);
        AppendSection(sb, "File system information", FileSystemRows);

        if (SupportsComment)
        {
            sb.AppendLine("** Container comment **");
            sb.AppendLine($"\"{Comment}\"");
            sb.AppendLine();
        }

        sb.Append("*** End of file ***");
        return sb.ToString();
    }

    private static void AppendSection(StringBuilder sb, string heading, IEnumerable<PropertyRow> rows)
    {
        sb.AppendLine($"** {heading} **");
        foreach (var row in rows)
            sb.AppendLine($"-{row.Name}: {row.Value}");
        sb.AppendLine();
    }

    private string FormatSize(ulong size) => sizeUnit.FormatSize(size, sizeUnit != SizeUnit.Bytes);

    private static string OrNotAvailable(string? value) => string.IsNullOrWhiteSpace(value) ? PropertyRow.NotAvailable : value;

    private static int IndexOf(IList<PropertyRow> rows, string name)
    {
        for (var i = 0; i < rows.Count; i++)
        {
            if (rows[i].Name == name)
                return i;
        }

        throw new ArgumentException($"There is no row named {name}.", nameof(name));
    }

    private List<PropertyRow> CreateFileRows(string? filePath)
    {
        RowList rows = new(PropertyGroup.File);

        if (filePath is not null)
        {
            FileInfo fileInfo = new(filePath);
            rows.Add("Filename", Path.GetFileName(filePath));
            rows.Add("Path", filePath);
            rows.Add("Size", FormatSize((ulong)fileInfo.Length));
            rows.Add("Created", fileInfo.CreationTime.ToString());
            rows.Add("Modified", fileInfo.LastWriteTime.ToString());
            rows.Add("Accessed", fileInfo.LastAccessTime.ToString());
            rows.Add("Attributes", fileInfo.Attributes.ToString());
        }
        else
        {
            foreach (var name in new[] { "Filename", "Path", "Size", "Created", "Modified", "Accessed", "Attributes" })
                rows.Add(name, PropertyRow.NotAvailable);
        }

        foreach (var (_, name) in Hashes)
            rows.Rows.Add(new(PropertyGroup.File, name, PendingHashText, IsPending: true));

        return rows.Rows;
    }

    private List<PropertyRow> CreateContainerRows()
    {
        RowList rows = new(PropertyGroup.Container);
        rows.Add("Container type", image.DisplayName);
        rows.Add("Container subtype", PropertyRow.NotAvailable);
        rows.Add("Container version", PropertyRow.NotAvailable);
        rows.Add("Created by", PropertyRow.NotAvailable);
        rows.Add("Creator version", PropertyRow.NotAvailable);

        if (image is VhdContainer vhd)
        {
            string vhdType = vhd.Footer.Type.ToString();
            rows.Set("Container subtype", vhdType[..vhdType.IndexOf("HardDisk")]);

            string containerVersion = $"{vhd.Footer.FormatVersionMajor}.{vhd.Footer.FormatVersionMinor}";
            if (containerVersion != "0.0")
                rows.Set("Container version", containerVersion);

            //We might want to prettify this further so we show "Windows" instead of "win", for example.
            rows.Set("Created by", string.IsNullOrWhiteSpace(vhd.Footer.CreatorApplication) ? "Unknown" : vhd.Footer.CreatorApplication);

            string creatorVersion = $"{vhd.Footer.CreatorVersionMajor}.{vhd.Footer.CreatorVersionMinor}";
            if (creatorVersion != "0.0")
                rows.Set("Creator version", creatorVersion);

            rows.Add("Disk GUID", $"{vhd.Footer.UniqueId}");
            rows.Add("Features", $"{vhd.Footer.Features}");
            rows.Add("Creation time", $"{vhd.Footer.CreationTime.DateTime}");
            rows.Add("Original size", sizeUnit.FormatSize(vhd.Footer.OriginalSize, true));
            rows.Add("Current size", sizeUnit.FormatSize(vhd.Footer.CurrentSize, true));
            rows.Add("Cylinders", $"{vhd.Footer.DiskCylinders:N0}");
            rows.Add("Heads", $"{vhd.Footer.DiskHeads:N0}");
            rows.Add("Sectors per track", $"{vhd.Footer.DiskSectorsPerCylinder:N0}");
            rows.Add("Checksum", $"0x{vhd.Footer.Checksum:X8}");
            rows.Add("Saved state", vhd.Footer.SavedState ? "Yes" : "No");
        }
        else if (image is NhdContainer nhd)
        {
            rows.Add("Header size", SizeUnit.Bytes.FormatSize(nhd.Header.HeaderSize, false));
            rows.Add("Sector size", SizeUnit.Bytes.FormatSize(nhd.Header.SectorSize, false));
            rows.Add("Cylinders", $"{nhd.Header.Cylinders:N0}");
            rows.Add("Heads", $"{nhd.Header.Heads:N0}");
            rows.Add("Sectors per track", $"{nhd.Header.SectorsPerTrack:N0}");
        }
        else if (image is Anex86Container anex86)
        {
            rows.Add("Disk type", $"0x{anex86.Header.DiskType:X4}");
            rows.Add("Header size", SizeUnit.Bytes.FormatSize((ulong)anex86.Header.HeaderSize, false));
            rows.Add("Data area size", SizeUnit.Bytes.FormatSize((ulong)anex86.Header.DataSize, false));
            rows.Add("Sector size", SizeUnit.Bytes.FormatSize((ulong)anex86.Header.SectorSize, false));
            rows.Add("Cylinders", $"{anex86.Header.Cylinders:N0}");
            rows.Add("Heads", $"{anex86.Header.Heads:N0}");
            rows.Add("Sectors per track", $"{anex86.Header.Sectors:N0}");
        }

        return rows.Rows;
    }

    private List<PropertyRow> CreatePartitionTableRows(int partitionIndex)
    {
        var table = image.PartitionTable;
        RowList rows = new(PropertyGroup.PartitionTable);
        rows.Add("Partitioning scheme", table.DisplayName);
        rows.Add("Number of partitions", table.Partitions.Count.ToString());
        rows.Add("Selected partition", partitionIndex.ToString());

        if (table is MbrPartitionTable mbr)
        {
            rows.Insert(1, "Physical drive number", $"0x{mbr.DriveNumber:X2}");
            rows.Insert(2, "Disk timestamp", mbr.TimestampMinutes < 60 && mbr.TimestampSeconds < 60 && mbr.TimestampHours < 24
                ? $"{mbr.TimestampHours:D2}:{mbr.TimestampMinutes:D2}:{mbr.TimestampSeconds:D2}"
                : PropertyRow.NotAvailable);
            rows.Insert(3, "Disk serial number", mbr.SerialNumber > 0 ? $"0x{mbr.SerialNumber:X8}" : PropertyRow.NotAvailable);
        }
        else if (table is GptPartitionTable gpt)
        {
            rows.Insert(1, "Major version", $"0x{gpt.Header.VersionMajor:X4}");
            rows.Insert(2, "Minor version", $"0x{gpt.Header.VersionMinor:X4}");
            rows.Insert(3, "Header size", SizeUnit.Bytes.FormatSize(gpt.Header.HeaderSize, false));
            rows.Insert(4, "Header CRC32 hash", $"0x{gpt.Header.HeaderHash:X8}");
            rows.Insert(5, "Current header LBA", $"{gpt.Header.CurrentLBA:N0}");
            rows.Insert(6, "Backup header LBA", $"{gpt.Header.BackupLBA:N0}");
            rows.Insert(7, "First usable LBA", $"{gpt.Header.FirstUsableLBA:N0}");
            rows.Insert(8, "Last usable LBA", $"{gpt.Header.LastUsableLBA:N0}");
            rows.Insert(9, "Disk GUID", $"{gpt.Header.DiskGuid}");
            rows.Insert(10, "First partition table LBA", $"{gpt.Header.TableLBA:N0}");
            rows.Insert(11, "Partition entry size", SizeUnit.Bytes.FormatSize(gpt.Header.SizeOfPartitionEntry, false));
            rows.Insert(12, "Partition table CRC32 hash", $"0x{gpt.Header.TableHash:X8}");
        }

        return rows.Rows;
    }

    private List<PropertyRow> CreatePartitionRows(int partitionIndex)
    {
        var table = image.PartitionTable;
        RowList rows = new(PropertyGroup.Partition);
        rows.Add("Partition ID/type", PropertyRow.NotAvailable);

        if (table is MbrPartitionTable mbr)
        {
            var entry = (MbrPartitionTable.MbrPartitionEntry)mbr.Partitions[partitionIndex];

            string entryType = "Unknown";
            try
            {
                entryType = (Attribute.GetCustomAttribute(entry.Type.GetType().GetField(entry.Type.ToString())!, typeof(DisplayAttribute)) as DisplayAttribute)!.Name!;
            }
            catch (Exception) { }

            rows.Set("Partition ID/type", $"0x{(byte)entry.Type:X2} - {entryType}");
            rows.Add("Active", entry.Active ? "Yes" : "No");
            rows.Add("First sector CHS", $"({entry.ChsStart.Cylinder},{entry.ChsStart.Head},{entry.ChsStart.Sector})");
            rows.Add("Last sector CHS", $"({entry.ChsEnd.Cylinder},{entry.ChsEnd.Head},{entry.ChsEnd.Sector})");
            rows.Add("First sector LBA", $"{entry.LbaStart:N0}");
            rows.Add("Total sectors", $"{entry.LbaLength:N0}");
            rows.Add("Total size", sizeUnit.FormatSize(entry.LbaLength * 512, true));
        }
        else if (table is GptPartitionTable gpt)
        {
            var entry = (GptPartitionTable.GptPartitionEntry)gpt.Partitions[partitionIndex];

            rows.Set("Partition ID/type", GptPartitionTable.GptPartitionTypes.TryGetValue(entry.TypeId, out var typeName) ? typeName : "Unknown");
            rows.Add("Partition type GUID", $"{entry.TypeId}");
            rows.Add("Partition GUID", $"{entry.EntryId}");
            rows.Add("First LBA", $"{entry.FirstLBA:N0}");
            rows.Add("Last LBA", $"{entry.LastLBA:N0}");
            rows.Add("Attribute flags", $"0x{(ulong)entry.Flags:X16}");
            rows.Add("Partition name", $"{entry.Name}");
        }

        return rows.Rows;
    }

    private List<PropertyRow> CreateFileSystemRows(int partitionIndex)
    {
        var partition = image.PartitionTable.Partitions[partitionIndex];
        var fileSystem = partition.FileSystem;

        RowList rows = new(PropertyGroup.FileSystem);
        rows.Add("File system", fileSystem.DisplayName);
        var volumeLabel = fileSystem.VolumeLabel;
        rows.Add("Volume label", !string.IsNullOrWhiteSpace(volumeLabel) && !volumeLabel.Equals("<N/A>") ? volumeLabel : PropertyRow.NotAvailable);
        rows.Add("Volume serial number", PropertyRow.NotAvailable);
        rows.Add("Total storage capacity", FormatSize((ulong)partition.Length));
        rows.Add("Free storage capacity", FormatSize((ulong)fileSystem.TotalFreeSpace));
        rows.Add("Files", fileSystem.RootDirectory.CountFiles(true).ToString());
        rows.Add("Subdirectories", fileSystem.RootDirectory.CountSubdirectories(true).ToString());

        if (fileSystem is FatFileSystem fatFS)
        {
            BiosParameterBlock bpb = fatFS.BiosParameterBlock;

            rows.Insert(1, "Jump instruction", $"0x{bpb.BootJump[0]:X2} 0x{bpb.BootJump[1]:X2} 0x{bpb.BootJump[2]:X2}");
            rows.Insert(2, "OEM ID", OrNotAvailable(bpb.OemId));
            rows.Insert(3, "Sector size", SizeUnit.Bytes.FormatSize(bpb.BytesPerLogicalSector, false));
            rows.Insert(4, "Sectors per cluster", $"{bpb.LogicalSectorsPerCluster:N0}");
            rows.Insert(5, "Reserved sectors", $"{bpb.ReservedLogicalSectors:N0}");
            rows.Insert(6, "Number of FATs", $"{bpb.NumberOfFATs:N0}");
            rows.Insert(7, "Root directory entries", fatFS is Fat32FileSystem ? PropertyRow.NotAvailable : $"{bpb.RootDirectoryEntries}");
            rows.Insert(8, "Total sectors", $"{bpb.TotalLogicalSectors:N0}");
            rows.Insert(9, "Media descriptor", $"0x{bpb.MediaDescriptor:X2}");
            rows.Insert(10, "Sectors per FAT", $"{bpb.LogicalSectorsPerFAT:N0}");
            rows.Insert(11, "Sectors per track", $"{bpb.PhysicalSectorsPerTrack:N0}");
            rows.Insert(12, "Heads/sides", $"{bpb.NumberOfHeads:N0}");
            rows.Insert(13, "Hidden sectors", $"{bpb.HiddenSectors:N0}");

            //These fields only exist in an extended BPB (either DOS 3.4/4.0 or FAT32).
            if (bpb is ExtendedBiosParameterBlock ebpb)
            {
                rows.Set("Volume serial number", ebpb.VolumeSerialNumber == 0 ? PropertyRow.NotAvailable : $"0x{ebpb.VolumeSerialNumber:X8}");
                rows.Insert(14, "Physical drive number", $"0x{ebpb.PhysicalDriveNumber:X2}");
                rows.Insert(15, "Flags", $"0x{ebpb.Flags:X2}");
                rows.Insert(16, "Boot signature", $"0x{(byte)ebpb.ExtendedBootSignature:X2}");
                rows.Insert(17, "File system type string", $"{ebpb.FileSystemType}");
            }

            //Root directory is relocatable in FAT32. These fields come last to keep the order of the rows.
            if (bpb is Fat32BiosParameterBlock f32bpb)
            {
                rows.Insert(8, "Root directory cluster", $"{f32bpb.RootDirectoryCluster:N0}");
                rows.Insert(18, "Extended flags", $"0x{f32bpb.ExtFlags:X2}");
                rows.Insert(19, "Version", $"0x{f32bpb.FileSystemVersion:X2}");
                rows.Insert(20, "FSInfo sector", $"{f32bpb.FsInfo:N0}");
                rows.Insert(21, "Backup boot sector", $"{f32bpb.BackupBootSector:N0}");
            }
        }
        else if (fileSystem is Iso9660FileSystem isoFS)
        {
            //Remove the rows that don't apply to ISO9660 or have specific alternatives
            rows.Remove("Volume label");
            rows.Remove("Free storage capacity");
            rows.Remove("Volume serial number");
            rows.Rename("Total storage capacity", "Volume size");

            var pvd = isoFS.PrimaryVolumeDescriptor;
            rows.Add("System identifier", OrNotAvailable(pvd.SystemIdentifier));
            rows.Add("Volume identifier", OrNotAvailable(pvd.VolumeIdentifier));
            rows.Add("Volume set size", OrNotAvailable(pvd.VolumeSetSize.ToString()));
            rows.Add("Volume sequence no.", OrNotAvailable(pvd.VolumeSequenceNumber.ToString()));
            rows.Add("Volume set identifier", OrNotAvailable(pvd.VolumeSetIdentifier));
            rows.Add("Publisher", OrNotAvailable(pvd.PublisherIdentifier));
            rows.Add("Data preparer", OrNotAvailable(pvd.DataPreparerIdentifier));
            rows.Add("Application identifier", OrNotAvailable(pvd.ApplicationIdentifier));
            rows.Add("Copyright file", OrNotAvailable(pvd.CopyrightFileIdentifier));
            rows.Add("Abstract file", OrNotAvailable(pvd.AbstractFileIdentifier));
            rows.Add("Bibliographic file", OrNotAvailable(pvd.BibliographicFileIdentifier));
            rows.Add("Creation time", OrNotAvailable(pvd.VolumeCreationTime.ToString()));
            rows.Add("Modification time", OrNotAvailable(pvd.VolumeModificationTime.ToString()));
            rows.Add("Expiration time", OrNotAvailable(pvd.VolumeExpirationTime.ToString()));
            rows.Add("Effective time", OrNotAvailable(pvd.VolumeEffectiveTime.ToString()));
        }

        return rows.Rows;
    }

    /// <summary>
    /// Builds the rows of one group, mirroring the order the properties were always shown in.
    /// </summary>
    private sealed class RowList(PropertyGroup group)
    {
        public List<PropertyRow> Rows { get; } = [];

        public void Add(string name, string value) => Rows.Add(new(group, name, value));

        public void Insert(int index, string name, string value) => Rows.Insert(index, new(group, name, value));

        public void Set(string name, string value)
        {
            var index = IndexOf(Rows, name);
            Rows[index] = Rows[index] with { Value = value };
        }

        public void Rename(string name, string newName)
        {
            var index = IndexOf(Rows, name);
            Rows[index] = Rows[index] with { Name = newName };
        }

        public void Remove(string name) => Rows.RemoveAt(IndexOf(Rows, name));
    }
}
