using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TotalImage.Containers;
using TotalImage.FileSystems.FAT;
using TotalImage.Partitions;
using TotalImage.ViewModels.Services;

namespace TotalImage.ViewModels.Dialogs;

/// <summary>
/// Which boot sector is shown.
/// </summary>
public enum BootSectorSource
{
    /// <summary>
    /// The volume boot record of the selected partition.
    /// </summary>
    Partition,

    /// <summary>
    /// The master boot record of the disk.
    /// </summary>
    Disk
}

/// <summary>
/// Shows the boot sector of the disk or the selected partition as hexadecimal bytes and exports
/// it to a file. Loading a boot sector from a file and presets are not implemented yet.
/// </summary>
public partial class BootSectorViewModel : ObservableObject
{
    /// <summary>
    /// The size of the disk's boot sector. Fixed because it was the sector size of the vast
    /// majority of hard disks.
    /// </summary>
    public const int DiskSectorSize = 512;

    /// <summary>
    /// The file types offered when exporting the boot sector.
    /// </summary>
    public static readonly IReadOnlyList<FileTypeFilter> ExportFileTypes = [ImageFileTypes.AllFiles];

    /// <summary>
    /// The extension of an exported boot sector whose name has none.
    /// </summary>
    public const string ExportExtension = "bin";

    private readonly Container image;
    private readonly int partitionIndex;
    private readonly IFilePickerService filePicker;
    private readonly IMessageService messages;

    /// <param name="image">The open image.</param>
    /// <param name="partitionIndex">The selected partition.</param>
    /// <param name="filePicker">Asks where to export the boot sector.</param>
    /// <param name="messages">Reports how the export went.</param>
    public BootSectorViewModel(Container image, int partitionIndex, IFilePickerService filePicker, IMessageService messages)
    {
        this.image = image;
        this.partitionIndex = partitionIndex;
        this.filePicker = filePicker;
        this.messages = messages;

        CanShowDisk = image.PartitionTable is not NoPartitionTable;
        Bytes = ReadBootSector(Source);
    }

    /// <summary>
    /// Whether the disk has a master boot record, that is, whether it is partitioned.
    /// </summary>
    public bool CanShowDisk { get; }

    /// <summary>
    /// Which boot sector is shown. <see cref="BootSectorSource.Disk"/> is ignored unless
    /// <see cref="CanShowDisk"/>.
    /// </summary>
    public BootSectorSource Source
    {
        get;
        set
        {
            if (value == BootSectorSource.Disk && !CanShowDisk)
                return;

            if (SetProperty(ref field, value))
            {
                OnPropertyChanged(nameof(IsPartitionSelected));
                OnPropertyChanged(nameof(IsDiskSelected));
                Bytes = ReadBootSector(value);
            }
        }
    } = BootSectorSource.Partition;

    /// <summary>
    /// Whether the selected partition's boot sector is shown. Setting it to <c>false</c> does
    /// nothing; select the other source instead (this suits radio buttons).
    /// </summary>
    public bool IsPartitionSelected
    {
        get => Source == BootSectorSource.Partition;
        set
        {
            if (value)
                Source = BootSectorSource.Partition;
        }
    }

    /// <summary>
    /// Whether the disk's boot sector is shown. Setting it to <c>false</c> does nothing.
    /// </summary>
    public bool IsDiskSelected
    {
        get => Source == BootSectorSource.Disk;
        set
        {
            if (value)
                Source = BootSectorSource.Disk;
        }
    }

    /// <summary>
    /// The bytes of the shown boot sector.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HexText))]
    public partial byte[] Bytes { get; private set; }

    /// <summary>
    /// <see cref="Bytes"/> as two-digit hexadecimal numbers, 16 per line.
    /// </summary>
    public string HexText
    {
        get
        {
            StringBuilder sb = new(Bytes.Length * 3);
            for (int i = 1; i <= Bytes.Length; i++)
            {
                sb.Append($"{Bytes[i - 1]:X2}");
                if (i % 16 == 0 && i < Bytes.Length)
                    sb.Append(Environment.NewLine);
                else
                    sb.Append(' ');
            }

            return sb.ToString();
        }
    }

    /// <summary>
    /// Asks where to save the shown boot sector and writes its bytes there.
    /// </summary>
    [RelayCommand]
    private async Task ExportAsync()
    {
        var path = await filePicker.PickSaveFileAsync(ExportFileTypes, ExportExtension);
        if (path is null)
            return;

        try
        {
            await File.WriteAllBytesAsync(path, Bytes);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            await messages.ShowErrorAsync($"Exporting the boot sector to \"{path}\"", ex);
            return;
        }

        await messages.ShowExportSucceededAsync(path);
    }

    private byte[] ReadBootSector(BootSectorSource source)
    {
        long offset;
        int length;
        if (source == BootSectorSource.Disk)
        {
            offset = 0;
            length = DiskSectorSize;
        }
        else
        {
            var partition = image.PartitionTable.Partitions[partitionIndex];
            offset = partition.Offset;
            length = partition.FileSystem is FatFileSystem fat ? fat.BiosParameterBlock.BytesPerLogicalSector : DiskSectorSize;
        }

        var bytes = new byte[length];
        var content = image.Content;
        content.Seek(offset, SeekOrigin.Begin);
        content.ReadAtLeast(bytes, length, throwOnEndOfStream: false);
        return bytes;
    }
}
