using System;
using System.IO;

namespace TotalImage.Containers.VHD;

/// <summary>
/// Represents a Microsoft Virtual Hard Disk (VHD) container file
/// </summary>
public class VhdContainer : Container
{
    private readonly Stream _contentStream;
    private readonly VhdFooter _footer; //The footer structure at the end of the file
    private readonly VhdFooter? _footerCopy; //The backup copy of the footer at the start of the file, sometimes called "header"
    private readonly VhdDynamicHeader? _dynamicHeader; //A separate header structure for dynamic and differencing disks, following the footer copy
    private readonly VhdBlockAllocationTable? _bat; //Block Allocation Table for dynamic and differencing disks

    /// <summary>
    /// The footer structure of this VHD
    /// </summary>
    public VhdFooter Footer => _footer;

    /// <summary>
    /// The dynamic disk header structure of this VHD if present, otherwise <c>null</c>
    /// </summary>
    public VhdDynamicHeader? DynamicHeader => _dynamicHeader;

    /// <summary>
    /// The block allocation table structure of this VHD if present, otherwise <c>null</c>
    /// </summary>
    public VhdBlockAllocationTable? BlockAllocationTable => _bat;

    /// <summary>
    /// Determines whether the VHD is fixed, dynamic or differencing
    /// </summary>
    public VhdType DiskType => Footer.Type;

    /// <summary>
    /// Sector length in bytes; always 512 bytes
    /// </summary>
    public const int SectorLength = 512;

    /// <inheritdoc />
    public VhdContainer(string path, bool memoryMapping) : base(path, memoryMapping)
    {
        byte[] footer = new byte[512];
        containerStream.Seek(-512, SeekOrigin.End);
        containerStream.Read(footer, 0, 512);
        _footer = new VhdFooter(footer);

        if (!_footer.VerifyChecksum())
            throw new InvalidDataException("The VHD footer may be corrupted, checksum verification failed");

        if (_footer.Type is VhdType.DynamicHardDisk or VhdType.DifferencingHardDisk)
        {
            containerStream.Seek(0, SeekOrigin.Begin);
            containerStream.Read(footer, 0, 512);
            _footerCopy = new VhdFooter(footer);

            var dynamicHeader = new byte[1024];
            containerStream.Seek((long)_footer.DataOffset, SeekOrigin.Begin);
            containerStream.Read(dynamicHeader);
            _dynamicHeader = new VhdDynamicHeader(dynamicHeader);

            if (!_dynamicHeader.VerifyChecksum())
                throw new InvalidDataException("The VHD dynamic header may be corrupted, checksum verification failed");

            var bat = new byte[_dynamicHeader.MaxTableEntries * 4];
            containerStream.Seek((long)_dynamicHeader.TableOffset, SeekOrigin.Begin);
            containerStream.Read(bat);

            _bat = new VhdBlockAllocationTable(bat, _dynamicHeader.BlockSize);

            _contentStream = new VhdStream(this, containerStream);
        }
        else
        {
            _contentStream = new PartialStream(containerStream, 0, containerStream.Length - 512);
        }
    }

    /// <inheritdoc />
    public override Stream Content => _contentStream;

    /// <inheritdoc />
    public override string DisplayName => "Microsoft VHD";
}
