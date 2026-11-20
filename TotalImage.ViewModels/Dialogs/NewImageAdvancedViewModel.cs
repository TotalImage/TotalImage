using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using TotalImage.DiskGeometries;
using TotalImage.FileSystems.BPB;

namespace TotalImage.ViewModels.Dialogs;

/// <summary>
/// The advanced options of a new floppy image: the geometry in detail and the boot sector fields.
/// Created by <see cref="NewImageViewModel"/> from its current state each time the advanced options
/// are shown, and read back by it afterwards.
/// </summary>
public partial class NewImageAdvancedViewModel : ObservableObject
{
    /// <summary>
    /// The longest OEM ID, volume serial number and file system type a BPB can store.
    /// </summary>
    public const int MaxBootSectorFieldLength = 8;

    /// <summary>
    /// Creates the advanced options for the current state of the new image.
    /// </summary>
    /// <param name="geometry">The current geometry.</param>
    /// <param name="hasBootSector">Whether the format has a boot sector at all. When it does not, the boot sector fields are empty and disabled.</param>
    /// <param name="writeBpb">Whether a BPB is written to the boot sector.</param>
    /// <param name="bpbVersion">The version of the BPB that is written.</param>
    /// <param name="oemId">The OEM ID.</param>
    /// <param name="serialNumber">The volume serial number, as hexadecimal text.</param>
    /// <param name="fileSystemType">The file system type.</param>
    public NewImageAdvancedViewModel(FloppyGeometry geometry, bool hasBootSector, bool writeBpb, BiosParameterBlockVersion bpbVersion,
        string oemId, string serialNumber, string fileSystemType)
    {
        ArgumentNullException.ThrowIfNull(geometry);

        Sides = geometry.Sides;
        Tracks = geometry.Tracks;
        SectorsPerTrack = geometry.SPT;
        BytesPerSector = 128 << geometry.BPS;
        MediaDescriptor = geometry.MediaDescriptor;
        SectorsPerCluster = geometry.SPC;
        NumberOfFats = geometry.NoOfFATs;
        SectorsPerFat = geometry.SPF;
        RootDirectoryEntries = geometry.RootDirectoryEntries;
        ReservedSectors = geometry.ReservedSectors;

        IsOemIdEnabled = hasBootSector;
        // The volume serial number came with the DOS 3.4 BPB, the file system type with DOS 4.0.
        IsSerialNumberEnabled = hasBootSector && writeBpb && bpbVersion >= BiosParameterBlockVersion.Dos34;
        IsFileSystemTypeEnabled = hasBootSector && writeBpb && bpbVersion == BiosParameterBlockVersion.Dos40;

        OemId = hasBootSector ? oemId : string.Empty;
        SerialNumber = hasBootSector ? serialNumber : string.Empty;
        FileSystemType = hasBootSector ? fileSystemType : string.Empty;
    }

    /// <summary>
    /// The numbers of sides a floppy disk can have.
    /// </summary>
    public IReadOnlyList<int> SideCounts { get; } = [1, 2];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TotalSectors))]
    public partial int Sides { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TotalSectors))]
    public partial int Tracks { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TotalSectors))]
    public partial int SectorsPerTrack { get; set; }

    [ObservableProperty]
    public partial int BytesPerSector { get; set; }

    [ObservableProperty]
    public partial int MediaDescriptor { get; set; }

    [ObservableProperty]
    public partial int SectorsPerCluster { get; set; }

    [ObservableProperty]
    public partial int NumberOfFats { get; set; }

    [ObservableProperty]
    public partial int SectorsPerFat { get; set; }

    [ObservableProperty]
    public partial int RootDirectoryEntries { get; set; }

    [ObservableProperty]
    public partial int ReservedSectors { get; set; }

    /// <summary>
    /// The total number of sectors on the disk: tracks * sectors per track * sides.
    /// </summary>
    public int TotalSectors => Tracks * SectorsPerTrack * Sides;

    [ObservableProperty]
    public partial string OemId { get; set; }

    [ObservableProperty]
    public partial string SerialNumber { get; set; }

    [ObservableProperty]
    public partial string FileSystemType { get; set; }

    /// <summary>
    /// The OEM ID can be edited when the format has a boot sector.
    /// </summary>
    public bool IsOemIdEnabled { get; }

    /// <summary>
    /// The volume serial number can be edited when a DOS 3.4 or later BPB is written.
    /// </summary>
    public bool IsSerialNumberEnabled { get; }

    /// <summary>
    /// The file system type can be edited when a DOS 4.0 BPB is written.
    /// </summary>
    public bool IsFileSystemTypeEnabled { get; }

    /// <summary>
    /// Builds the geometry described by the current values.
    /// </summary>
    public FloppyGeometry ToGeometry()
        => new((byte)Sides, (byte)Tracks, (byte)SectorsPerTrack, (byte)(Math.Log(BytesPerSector, 2) - 7), (byte)MediaDescriptor,
            (byte)SectorsPerCluster, (byte)NumberOfFats, (byte)SectorsPerFat, (ushort)RootDirectoryEntries, (byte)ReservedSectors);
}
