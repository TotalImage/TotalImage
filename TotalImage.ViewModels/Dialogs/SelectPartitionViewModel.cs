using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TotalImage.Containers;
using TotalImage.Containers.NHD;
using TotalImage.Containers.VHD;
using TotalImage.FileSystems.BPB;
using TotalImage.FileSystems.FAT;
using TotalImage.Partitions;
using TotalImage.ViewModels.Formatting;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Settings;

namespace TotalImage.ViewModels.Dialogs;

/// <summary>
/// The partition picked to be loaded.
/// </summary>
/// <param name="Index">The index of the partition in the image's partition table.</param>
/// <param name="ReadOnly">Whether the partition should be loaded read-only.</param>
public sealed record PartitionSelection(int Index, bool ReadOnly);

/// <summary>
/// One row of the partition list.
/// </summary>
/// <param name="Index">The index of the partition in the partition table.</param>
/// <param name="FileSystem">The name of the file system, or "RAW" when it is not supported.</param>
/// <param name="VolumeLabel">The volume label, "&lt;N/A&gt;" when there is none.</param>
/// <param name="Size">The partition size, formatted with the configured size unit.</param>
/// <param name="Flag">"Yes" or "No" for the table-specific flag (see <see cref="SelectPartitionViewModel.FlagColumnHeader"/>), <c>null</c> when the table has none.</param>
/// <param name="CanLoad">Whether the partition contains a file system that can be loaded.</param>
/// <param name="IsDimmed">Whether the row is shown dimmed because its file system could not be read.</param>
public sealed record PartitionRow(int Index, string FileSystem, string VolumeLabel, string Size, string? Flag, bool CanLoad, bool IsDimmed);

/// <summary>
/// Picks the partition of an image to load, showing information about the disk and the
/// selected partition.
/// </summary>
public partial class SelectPartitionViewModel : DialogViewModel<PartitionSelection>
{
    /// <summary>
    /// The text shown for values that don't apply to this disk or partition.
    /// </summary>
    public const string NotApplicable = "N/A";

    private readonly PartitionTable partitionTable;
    private readonly SizeUnit sizeUnit;
    private readonly IMessageService messages;

    public SelectPartitionViewModel(Container image, ISettingsService settings, IMessageService messages)
    {
        partitionTable = image.PartitionTable;
        sizeUnit = settings.Current.SizeUnit;
        this.messages = messages;

        PartitionTableName = partitionTable.DisplayName;
        FlagColumnHeader = partitionTable is GptPartitionTable ? "Legacy boot" : "Active";
        // Reading the partitions first also loads the GPT header.
        Rows = CreateRows();
        FillDiskInformation(image);
        ClearPartitionInformation();
        SelectedIndex = -1;
    }

    /// <summary>
    /// The partitions of the image, in partition table order.
    /// </summary>
    public IReadOnlyList<PartitionRow> Rows { get; }

    /// <summary>
    /// The header of the flag column: "Active" for MBR, "Legacy boot" for GPT.
    /// </summary>
    public string FlagColumnHeader { get; }

    /// <summary>
    /// The index of the selected row, or -1 when nothing is selected.
    /// </summary>
    [ObservableProperty]
    public partial int SelectedIndex { get; set; }

    /// <summary>
    /// Whether the partition is loaded read-only.
    /// </summary>
    [ObservableProperty]
    public partial bool ReadOnly { get; set; }

    /// <summary>
    /// Loading partitions read-only is not supported yet, so the option can't be changed.
    /// </summary>
    public bool CanChangeReadOnly => false;

    // Disk information

    public string PartitionTableName { get; }
    public string DiskType { get; } = "Basic"; //TODO: We need to actually determine this - LDM detection...
    public string DiskTotalSize { get; private set; } = string.Empty;
    public string DiskTotalSectors { get; private set; } = string.Empty;
    public string DiskUsableSize { get; private set; } = string.Empty;
    public string DiskGuid { get; private set; } = string.Empty;
    public string DiskTimestamp { get; private set; } = string.Empty;
    public string DiskSerial { get; private set; } = string.Empty;

    // Information about the selected partition

    [ObservableProperty]
    public partial string PartitionType { get; private set; }

    [ObservableProperty]
    public partial string PartitionGuid { get; private set; }

    [ObservableProperty]
    public partial string PartitionSerial { get; private set; }

    [ObservableProperty]
    public partial string PartitionFirstLba { get; private set; }

    [ObservableProperty]
    public partial string PartitionLastLba { get; private set; }

    [ObservableProperty]
    public partial string PartitionStartOffset { get; private set; }

    [ObservableProperty]
    public partial string PartitionEndOffset { get; private set; }

    [ObservableProperty]
    public partial string PartitionTotalSectors { get; private set; }

    private PartitionRow? SelectedRow
        => SelectedIndex >= 0 && SelectedIndex < Rows.Count ? Rows[SelectedIndex] : null;

    /// <summary>
    /// Opens a partition directly (for example by double-clicking its row): selects it and
    /// accepts the dialog when it can be loaded, or explains why it can't.
    /// </summary>
    /// <param name="index">The row to open.</param>
    [RelayCommand]
    private async Task OpenPartition(int index)
    {
        SelectedIndex = index;

        if (AcceptCommand.CanExecute(null))
            AcceptCommand.Execute(null);
        else if (SelectedRow is not null)
            await messages.ShowPartitionNotSupportedAsync();
    }

    protected override bool CanAccept() => SelectedRow?.CanLoad ?? false;

    protected override PartitionSelection CreateAnswer() => new(SelectedIndex, ReadOnly);

    partial void OnSelectedIndexChanged(int value)
    {
        if (SelectedRow is null)
            ClearPartitionInformation();
        else
            FillPartitionInformation(partitionTable.Partitions[value]);

        RefreshCanAccept();
    }

    private List<PartitionRow> CreateRows()
    {
        var rows = new List<PartitionRow>();
        for (int i = 0; i < partitionTable.Partitions.Count; i++)
        {
            var entry = partitionTable.Partitions[i];
            string fileSystem, volumeLabel;
            bool dimmed = false;
            try
            {
                fileSystem = entry.FileSystem.DisplayName;
                volumeLabel = string.IsNullOrWhiteSpace(entry.FileSystem.VolumeLabel) ? "<N/A>" : entry.FileSystem.VolumeLabel;
            }
            catch (InvalidDataException)
            {
                // this is probably an unsupported file system - we'll just mark it as RAW
                fileSystem = "RAW";
                volumeLabel = string.Empty;
                dimmed = true;
            }

            string? flag = entry switch
            {
                MbrPartitionTable.MbrPartitionEntry mbrEntry => YesNo(mbrEntry.Active),
                GptPartitionTable.GptPartitionEntry gptEntry => YesNo(gptEntry.Flags.HasFlag(GptPartitionTable.GptPartitionFlags.BiosBootable)),
                _ => null
            };

            rows.Add(new PartitionRow(i, fileSystem, volumeLabel, sizeUnit.FormatSize((ulong)entry.Length), flag, fileSystem != "RAW", dimmed));
        }

        return rows;
    }

    private void FillDiskInformation(Container image)
    {
        if (partitionTable is GptPartitionTable gpt)
        {
            DiskGuid = NotApplicable;
            DiskTimestamp = NotApplicable;
            DiskSerial = NotApplicable;

            if (gpt.Header is { } header)
            {
                //Let's just assume 512-byte sectors for now
                DiskTotalSize = sizeUnit.FormatSize(header.BackupLBA * 512 + 512);
                DiskTotalSectors = $"{header.BackupLBA + 1:N0}";
                DiskUsableSize = sizeUnit.FormatSize((header.LastUsableLBA + 1 - header.FirstUsableLBA) * 512);
                DiskGuid = header.DiskGuid.ToString();
            }
        }
        else if (partitionTable is MbrPartitionTable mbr)
        {
            DiskGuid = NotApplicable;
            DiskSerial = NotApplicable;
            if (mbr.SerialNumber > 0) //This is a very simplistic check...
                DiskSerial = $"{mbr.SerialNumber:X}";

            //Timestamp can be completely bogus/absent in modern MBRs, so let's ignore it if it is
            DiskTimestamp = NotApplicable;
            if (mbr.TimestampMinutes < 60 && mbr.TimestampSeconds < 60 && mbr.TimestampHours < 24)
                DiskTimestamp = $"{mbr.TimestampHours:D2}:{mbr.TimestampMinutes:D2}:{mbr.TimestampSeconds:D2}";

            //TODO: this code below is pretty hacky and needs to be reworked...
            //TODO2: add Anex86 container specific path, if it's applicable

            //For raw, there is no container metadata that could be used, so we just need to estimate based on file size and sector size (assumed 512 B)
            if (image is RawContainer)
            {
                long sectorCount = image.Length / 512;
                DiskTotalSectors = $"{sectorCount:N0}";
                DiskTotalSize = sizeUnit.FormatSize((ulong)image.Length);

                /* Technically everything after the bootsector/MBR is free to be partitioned, so we're disregarding any gaps that some software may make
                 * between the MBR and the first partition. */
                DiskUsableSize = sizeUnit.FormatSize((ulong)image.Length - 512);
            }
            //For VHD we can use the CurrentSize property
            else if (image is VhdContainer vhd)
            {
                DiskTotalSize = sizeUnit.FormatSize(vhd.Footer.CurrentSize);
                DiskUsableSize = sizeUnit.FormatSize(vhd.Footer.CurrentSize - 512);
                DiskTotalSectors = $"{vhd.Footer.CurrentSize / 512:N0}";
            }
            /* For NHD there's only the CHS values and sector size. Apparently some programs adjust image size to fit a CHS combo perfectly?
             * Once the PC98 partition table is supported, that would have to be taken into account, since it can co-exist with MBR... */
            else if (image is NhdContainer nhd)
            {
                ulong sectors = nhd.Header.Cylinders * nhd.Header.Heads * nhd.Header.SectorsPerTrack;
                DiskTotalSectors = $"{sectors:N0}";
                DiskTotalSize = sizeUnit.FormatSize(sectors * nhd.Header.SectorSize);
                DiskUsableSize = sizeUnit.FormatSize(sectors * nhd.Header.SectorSize - nhd.Header.SectorSize);
            }
        }
    }

    private void FillPartitionInformation(PartitionEntry entry)
    {
        ClearPartitionInformation();

        if (entry is GptPartitionTable.GptPartitionEntry gptEntry)
        {
            PartitionType = GptPartitionTable.GptPartitionTypes.TryGetValue(gptEntry.TypeId, out var typeName) ? typeName : gptEntry.TypeId.ToString();
            PartitionStartOffset = FormatOffset(entry.Offset);
            PartitionEndOffset = FormatOffset(entry.Offset + entry.Length);
            PartitionTotalSectors = $"{gptEntry.Length / 512:N0}";
            PartitionFirstLba = $"{gptEntry.FirstLBA:N0}";
            PartitionLastLba = $"{gptEntry.LastLBA:N0}";
            PartitionGuid = gptEntry.EntryId.ToString();
            PartitionSerial = NotApplicable;
        }
        else if (entry is MbrPartitionTable.MbrPartitionEntry mbrEntry)
        {
            PartitionType = $"0x{mbrEntry.Type:X} - {GetDisplayName(mbrEntry.Type)}";
            PartitionStartOffset = FormatOffset(entry.Offset);
            PartitionEndOffset = FormatOffset(entry.Offset + entry.Length);
            PartitionGuid = NotApplicable;
            PartitionFirstLba = $"{mbrEntry.LbaStart:N0}";
            PartitionLastLba = $"{mbrEntry.LbaStart + mbrEntry.LbaLength - 1:N0}";
            PartitionTotalSectors = $"{mbrEntry.LbaLength:N0}";
            PartitionSerial = NotApplicable;

            try
            {
                if (mbrEntry.FileSystem is FatFileSystem { BiosParameterBlock: ExtendedBiosParameterBlock ebpb })
                    PartitionSerial = $"{ebpb.VolumeSerialNumber:X8}";
            }
            catch (InvalidDataException)
            {
                // No readable file system, so there's no serial number either.
            }
        }
    }

    private void ClearPartitionInformation()
    {
        PartitionType = string.Empty;
        PartitionGuid = string.Empty;
        PartitionSerial = string.Empty;
        PartitionFirstLba = string.Empty;
        PartitionLastLba = string.Empty;
        PartitionStartOffset = string.Empty;
        PartitionEndOffset = string.Empty;
        PartitionTotalSectors = string.Empty;
    }

    private string FormatOffset(long offset) => sizeUnit.FormatSize((ulong)offset);

    private static string YesNo(bool value) => value ? "Yes" : "No";

    private static string GetDisplayName(MbrPartitionTable.MbrPartitionType type)
        => typeof(MbrPartitionTable.MbrPartitionType).GetField(type.ToString())?.GetCustomAttribute<DisplayAttribute>()?.Name ?? "Unknown";
}
