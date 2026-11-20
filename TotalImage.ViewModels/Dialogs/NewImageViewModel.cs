using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TotalImage.DiskGeometries;
using TotalImage.FileSystems.BPB;
using TotalImage.ViewModels.Services;

namespace TotalImage.ViewModels.Dialogs;

/// <summary>
/// What a new floppy image looks like.
/// </summary>
/// <param name="Geometry">The disk geometry.</param>
/// <param name="WriteBpb">Write a BPB to the boot sector.</param>
/// <param name="BpbVersion">The version of the BPB.</param>
/// <param name="OemId">The OEM ID.</param>
/// <param name="SerialNumber">The volume serial number, as hexadecimal text (DOS 3.4 and later BPBs).</param>
/// <param name="FileSystemType">The file system type (DOS 4.0 BPBs).</param>
/// <param name="VolumeLabel">The volume label (DOS 4.0 BPBs).</param>
public sealed record NewImageOptions(FloppyGeometry Geometry, bool WriteBpb, BiosParameterBlockVersion BpbVersion,
    string OemId, string SerialNumber, string FileSystemType, string VolumeLabel);

/// <summary>
/// Asks what kind of image to create.
/// </summary>
/// <remarks>
/// Only floppy images can be created so far: the hard disk options are shown, but not part of the answer.
/// </remarks>
public partial class NewImageViewModel : DialogViewModel<NewImageOptions>
{
    /// <summary>
    /// The longest volume label FAT can store.
    /// </summary>
    public const int MaxVolumeLabelLength = 11;

    public const string PlainSectorImage = "Plain sector image";
    public const string MicrosoftVhd = "Microsoft VHD";
    public const string MasterBootRecord = "Master Boot Record";
    public const string GuidPartitionTable = "GUID Partition Table";

    private const string DefaultOemId = "MSDOS5.0";
    private const string DmfOemId = "MSDMF3.2";
    private const string DefaultFileSystemType = "FAT12";

    private static readonly IReadOnlyList<string> FloppyContainerFormats = [PlainSectorImage];
    private static readonly IReadOnlyList<string> HardDiskContainerFormats = [PlainSectorImage, MicrosoftVhd];

    private readonly IMessageService messages;
    private readonly Func<NewImageAdvancedViewModel, Task> showAdvancedOptions;

    /// <param name="messages">Asks for confirmation before creating an image without a BPB.</param>
    /// <param name="showAdvancedOptions">
    /// Shows the advanced options and completes when they were closed. The changes are applied
    /// whichever way they were closed.
    /// </param>
    public NewImageViewModel(IMessageService messages, Func<NewImageAdvancedViewModel, Task> showAdvancedOptions)
    {
        this.messages = messages;
        this.showAdvancedOptions = showAdvancedOptions;

        IsFloppyDisk = true;
        ContainerFormat = PlainSectorImage;
        VolumeLabel = string.Empty;
        BpbVersion = BpbVersions.Single(option => option.Value == BiosParameterBlockVersion.Dos40);
        SerialNumber = Random.Shared.Next().ToString("X8");
        FileSystemType = DefaultFileSystemType;
        OemId = DefaultOemId;
        Geometry = FloppyGeometry.KnownGeometries[FloppyGeometry.FriendlyName.HighDensity1440k];
        SelectedGeometry = GetGeometryOption(FloppyGeometry.FriendlyName.HighDensity1440k);

        DiskSize = 1;
        WritePartitionTable = true;
        PartitionTable = MasterBootRecord;
    }

    /// <summary>
    /// The floppy geometries that can be chosen, ending with <see cref="FloppyGeometry.FriendlyName.Custom"/>.
    /// </summary>
    public static IReadOnlyList<NamedOption<FloppyGeometry.FriendlyName>> FloppyGeometries { get; } =
        Enum.GetValues<FloppyGeometry.FriendlyName>()
            .Order()
            .Select(value => new NamedOption<FloppyGeometry.FriendlyName>(value,
                typeof(FloppyGeometry.FriendlyName).GetField(value.ToString())!.GetCustomAttribute<DisplayAttribute>()?.Name ?? value.ToString()))
            .ToArray();

    /// <summary>
    /// The BPB versions that can be written.
    /// </summary>
    public static IReadOnlyList<NamedOption<BiosParameterBlockVersion>> BpbVersions { get; } =
    [
        new(BiosParameterBlockVersion.Dos20, "DOS 2.0"),
        new(BiosParameterBlockVersion.Dos34, "DOS 3.4"),
        new(BiosParameterBlockVersion.Dos40, "DOS 4.0+"),
    ];

    /// <summary>
    /// The partition tables that can be written to a hard disk.
    /// </summary>
    public static IReadOnlyList<string> PartitionTables { get; } = [MasterBootRecord, GuidPartitionTable];

    /// <summary>
    /// Creates a floppy image (as opposed to a hard disk image).
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsHardDisk), nameof(ContainerFormats))]
    public partial bool IsFloppyDisk { get; set; }

    /// <summary>
    /// Creates a hard disk image. The opposite of <see cref="IsFloppyDisk"/>.
    /// </summary>
    public bool IsHardDisk
    {
        get => !IsFloppyDisk;
        set => IsFloppyDisk = !value;
    }

    /// <summary>
    /// The container formats available for the media type.
    /// </summary>
    public IReadOnlyList<string> ContainerFormats => IsFloppyDisk ? FloppyContainerFormats : HardDiskContainerFormats;

    [ObservableProperty]
    public partial string? ContainerFormat { get; set; }

    /// <summary>
    /// The chosen floppy geometry. Choosing a known geometry sets <see cref="Geometry"/>;
    /// choosing <see cref="FloppyGeometry.FriendlyName.Custom"/> keeps the current one.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanChangeWriteBpb))]
    public partial NamedOption<FloppyGeometry.FriendlyName> SelectedGeometry { get; set; }

    /// <summary>
    /// The geometry of the new floppy image.
    /// </summary>
    public FloppyGeometry Geometry { get; private set; }

    [ObservableProperty]
    public partial string VolumeLabel { get; set; }

    /// <summary>
    /// Whether a BPB is written to the boot sector.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanChooseBpbVersion))]
    public partial bool WriteBpb { get; set; }

    /// <summary>
    /// Whether the BPB can be turned on and off. Formats without a boot sector never get one.
    /// </summary>
    public bool CanChangeWriteBpb => HasBootSector;

    /// <summary>
    /// The BPB version can be chosen while a BPB is written.
    /// </summary>
    public bool CanChooseBpbVersion => WriteBpb;

    [ObservableProperty]
    public partial NamedOption<BiosParameterBlockVersion> BpbVersion { get; set; }

    /// <summary>
    /// The OEM ID. Follows the chosen geometry, and can be changed in the advanced options.
    /// </summary>
    public string OemId { get; private set; }

    /// <summary>
    /// The volume serial number, as hexadecimal text. Random at first, and can be changed in the advanced options.
    /// </summary>
    public string SerialNumber { get; private set; }

    /// <summary>
    /// The file system type. Can be changed in the advanced options.
    /// </summary>
    public string FileSystemType { get; private set; }

    /// <summary>
    /// The size of a new hard disk in MiB.
    /// </summary>
    [ObservableProperty]
    public partial int DiskSize { get; set; }

    /// <summary>
    /// Whether a partition table is written to a new hard disk.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanChoosePartitionTable))]
    public partial bool WritePartitionTable { get; set; }

    /// <summary>
    /// The partition table type can be chosen while a partition table is written.
    /// </summary>
    public bool CanChoosePartitionTable => WritePartitionTable;

    [ObservableProperty]
    public partial string? PartitionTable { get; set; }

    // The Acorn 800k format has no boot sector at all.
    private bool HasBootSector => SelectedGeometry.Value != FloppyGeometry.FriendlyName.Acorn800k;

    private static NamedOption<FloppyGeometry.FriendlyName> GetGeometryOption(FloppyGeometry.FriendlyName name)
        => FloppyGeometries.Single(option => option.Value == name);

    partial void OnIsFloppyDiskChanged(bool value) => ContainerFormat = PlainSectorImage;

    partial void OnSelectedGeometryChanged(NamedOption<FloppyGeometry.FriendlyName> value)
    {
        if (value.Value != FloppyGeometry.FriendlyName.Custom)
            Geometry = FloppyGeometry.KnownGeometries[value.Value];

        if (HasBootSector)
        {
            WriteBpb = true;
        }
        else
        {
            WriteBpb = false;
            BpbVersion = BpbVersions.Single(option => option.Value == BiosParameterBlockVersion.Dos20);
        }

        OemId = value.Value is FloppyGeometry.FriendlyName.DMF1024 or FloppyGeometry.FriendlyName.DMF2048 ? DmfOemId : DefaultOemId;
    }

    /// <summary>
    /// Shows the advanced options for the current state and applies them afterwards. A geometry
    /// that differs from the chosen known one switches to <see cref="FloppyGeometry.FriendlyName.Custom"/>.
    /// </summary>
    [RelayCommand]
    private async Task ShowAdvancedOptionsAsync()
    {
        var advanced = new NewImageAdvancedViewModel(Geometry, HasBootSector, WriteBpb, BpbVersion.Value, OemId, SerialNumber, FileSystemType);
        await showAdvancedOptions(advanced);

        var newGeometry = advanced.ToGeometry();
        if (SelectedGeometry.Value != FloppyGeometry.FriendlyName.Custom && !newGeometry.Equals(Geometry))
        {
            SelectedGeometry = GetGeometryOption(FloppyGeometry.FriendlyName.Custom);
            Geometry = newGeometry;
        }

        SerialNumber = advanced.SerialNumber;
        FileSystemType = advanced.FileSystemType;
        OemId = advanced.OemId;
    }

    /// <summary>
    /// Accepts the dialog. Creating a floppy image without a BPB asks for confirmation first; when it
    /// is refused, nothing is answered and the dialog stays open.
    /// </summary>
    [RelayCommand]
    private async Task CreateAsync()
    {
        if (IsFloppyDisk && !WriteBpb && !await messages.ConfirmCreateWithoutBpbAsync())
            return;

        AcceptCommand.Execute(null);
    }

    protected override NewImageOptions CreateAnswer()
        => new(Geometry, WriteBpb, BpbVersion.Value, OemId, SerialNumber, FileSystemType, VolumeLabel);
}
