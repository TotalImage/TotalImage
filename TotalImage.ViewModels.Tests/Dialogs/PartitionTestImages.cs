using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TotalImage.Containers;
using TotalImage.DiskGeometries;
using TotalImage.FileSystems.BPB;
using TotalImage.Partitions;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Settings;

namespace TotalImage.ViewModels.Tests.Dialogs;

/// <summary>
/// Partitioned test images (written to temp files, since only file-backed raw containers can be opened from outside TotalImage.IO) and service fakes for the partition dialog tests.
/// </summary>
internal sealed class PartitionTestImages : IDisposable
{
    private readonly TempFolder temp = new();
    private readonly List<Container> opened = [];

    public const string FatVolumeLabel = "DISK";
    public const uint FatSerialNumber = 0x1234ABCD;

    /// <summary>
    /// The number of sectors of the FAT12 partition (a 360 KiB floppy).
    /// </summary>
    public const uint FatSectors = 720;

    /// <summary>
    /// The number of sectors of the empty (RAW) partition after it.
    /// </summary>
    public const uint RawSectors = 8;

    public const uint MbrSerialNumber = 0xDEADBEEF;

    /// <summary>
    /// An MBR image with an active FAT12 partition at sector 1 and an empty (RAW) NTFS-typed
    /// partition right after it.
    /// </summary>
    public RawContainer CreateMbrImage(bool validTimestamp = true)
    {
        byte[] floppyBytes = CreateFatVolume();
        byte[] image = new byte[(1 + FatSectors + RawSectors) * 512];
        floppyBytes.CopyTo(image, 512);

        Span<byte> mbr = image.AsSpan(0, 512);
        // Timestamp: drive number, seconds, minutes, hours
        mbr[0xDC] = 0x80;
        mbr[0xDD] = validTimestamp ? (byte)56 : (byte)99;
        mbr[0xDE] = 34;
        mbr[0xDF] = 12;
        BinaryPrimitives.WriteUInt32LittleEndian(mbr[0x1B8..], validTimestamp ? MbrSerialNumber : 0);
        WriteMbrEntry(mbr[0x1BE..], true, MbrPartitionTable.MbrPartitionType.Fat12, 1, FatSectors);
        WriteMbrEntry(mbr[0x1CE..], false, MbrPartitionTable.MbrPartitionType.HpfsNtfsExFat, 1 + FatSectors, RawSectors);
        BinaryPrimitives.WriteUInt16LittleEndian(mbr[510..], 0xAA55);

        return Open(image);
    }

    public static readonly Guid GptDiskGuid = new(Enumerable16(0x11));
    public static readonly Guid GptEntryId = Guid.Parse("01234567-89AB-CDEF-0123-456789ABCDEF");

    /// <summary>
    /// A GPT image (as in the IO tests) with one empty EFI system partition at LBA 3-4, BIOS bootable.
    /// </summary>
    public RawContainer CreateGptImage()
    {
        byte[] image = new byte[2560];

        Span<byte> mbr = image.AsSpan(0, 512);
        mbr[0x1BE + 4] = (byte)MbrPartitionTable.MbrPartitionType.GptProtectivePartition;
        BinaryPrimitives.WriteUInt32LittleEndian(mbr[(0x1BE + 8)..], 1);
        BinaryPrimitives.WriteUInt32LittleEndian(mbr[(0x1BE + 12)..], 0xFFFFFFFF);
        BinaryPrimitives.WriteUInt16LittleEndian(mbr[510..], 0xAA55);

        Span<byte> header = image.AsSpan(512, 92);
        Encoding.ASCII.GetBytes("EFI PART").CopyTo(header);
        BinaryPrimitives.WriteUInt32LittleEndian(header[8..12], 0x00010000);
        BinaryPrimitives.WriteUInt32LittleEndian(header[12..16], 92);
        BinaryPrimitives.WriteUInt64LittleEndian(header[24..32], 1);
        BinaryPrimitives.WriteUInt64LittleEndian(header[32..40], 4);
        BinaryPrimitives.WriteUInt64LittleEndian(header[40..48], 34);
        BinaryPrimitives.WriteUInt64LittleEndian(header[48..56], 200);
        GptDiskGuid.TryWriteBytes(header[56..72]);
        BinaryPrimitives.WriteUInt64LittleEndian(header[72..80], 2);
        BinaryPrimitives.WriteUInt32LittleEndian(header[80..84], 1);
        BinaryPrimitives.WriteUInt32LittleEndian(header[84..88], 128);

        Span<byte> entry = image.AsSpan(1024, 128);
        Guid.Parse("C12A7328-F81F-11D2-BA4B-00A0C93EC93B").TryWriteBytes(entry[0..16]);
        GptEntryId.TryWriteBytes(entry[16..32]);
        BinaryPrimitives.WriteUInt64LittleEndian(entry[32..40], 3);
        BinaryPrimitives.WriteUInt64LittleEndian(entry[40..48], 4);
        BinaryPrimitives.WriteUInt64LittleEndian(entry[48..56], (ulong)GptPartitionTable.GptPartitionFlags.BiosBootable);
        Encoding.Unicode.GetBytes("ESP").CopyTo(entry[56..]);

        return Open(image);
    }

    /// <summary>
    /// An unpartitioned FAT12 floppy image.
    /// </summary>
    public RawContainer CreateFloppyImage()
    {
        return Open(CreateFatVolume());
    }

    public void Dispose()
    {
        foreach (var container in opened)
            container.Dispose();
        temp.Dispose();
    }

    private RawContainer Open(byte[] image)
    {
        var path = temp.File($"{Guid.NewGuid():N}.img");
        File.WriteAllBytes(path, image);
        var container = new RawContainer(path, false);
        opened.Add(container);
        return container;
    }

    /// <summary>
    /// A freshly formatted 360 KiB FAT12 volume with a DOS 4.0 BPB.
    /// </summary>
    internal static byte[] CreateFatVolume()
    {
        var geometry = FloppyGeometry.KnownGeometries[FloppyGeometry.FriendlyName.DoubleDensity360k];
        var bpb = ExtendedBiosParameterBlock.FromGeometry(geometry, BiosParameterBlockVersion.Dos40, "TOTALIMG",
            FatSerialNumber.ToString("X8"), "FAT12", FatVolumeLabel);
        using var floppy = RawContainer.CreateImage(bpb, geometry.Tracks, true);
        byte[] bytes = new byte[floppy.Content.Length];
        floppy.Content.Seek(0, SeekOrigin.Begin);
        floppy.Content.ReadExactly(bytes);

        // BiosParameterBlock.FromGeometry pads the OEM ID to 11 characters and Fat12FileSystem.Create
        // writes all of them, which pushes the BPB 3 bytes past 0x0B. Move it back where it belongs.
        Array.Copy(bytes, 0x0B + 3, bytes, 0x0B, 0x3E - 0x0B);
        return bytes;
    }

    private static void WriteMbrEntry(Span<byte> entry, bool active, MbrPartitionTable.MbrPartitionType type, uint start, uint length)
    {
        entry[0] = active ? (byte)0x80 : (byte)0;
        entry[4] = (byte)type;
        BinaryPrimitives.WriteUInt32LittleEndian(entry[8..12], start);
        BinaryPrimitives.WriteUInt32LittleEndian(entry[12..16], length);
    }

    private static byte[] Enumerable16(byte value)
    {
        var bytes = new byte[16];
        Array.Fill(bytes, value);
        return bytes;
    }
}

internal sealed class PartitionTestSettings : ISettingsService
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

internal sealed class PartitionTestMessages : MessageServiceStub
{
    public int PartitionNotSupportedShown { get; private set; }

    public override Task ShowPartitionNotSupportedAsync()
    {
        PartitionNotSupportedShown++;
        return Task.CompletedTask;
    }
}
