using System;
using System.IO;
using TotalImage.FileSystems.MTF;
using TotalImage.Partitions;

namespace TotalImage.Containers.TAP;

/// <summary>
/// A read-only SIMH tape image containing a Microsoft Tape Format backup.
/// </summary>
public sealed class TapContainer : Container
{
    /// <summary>
    /// The logical tape payload with SIMH record framing removed.
    /// </summary>
    private TapStream _content = null!;

    /// <inheritdoc />
    public override Stream Content => _content;
    /// <inheritdoc />
    public override string DisplayName => "SIMH tape image (NT Backup)";
    /// <inheritdoc />
    public override bool SupportsWriting => false;

    /// <summary>
    /// Opens a tape image and validates its record framing and MTF backup index.
    /// </summary>
    /// <param name="path">The path of the tape image to open.</param>
    /// <param name="memoryMapping">Whether to use a memory-mapped view of the backing file.</param>
    /// <exception cref="InvalidDataException">The tape framing or backup data is invalid or incomplete.</exception>
    /// <exception cref="NotSupportedException">The tape or backup uses an unsupported feature.</exception>
    public TapContainer(string path, bool memoryMapping = false) : base(path, memoryMapping)
    {
        Initialize();
    }

    /// <summary>
    /// Opens and validates a tape image from a stream, taking ownership of the stream.
    /// </summary>
    /// <param name="stream">The readable, seekable stream containing the framed tape image.</param>
    /// <remarks>The stream is disposed when the container is disposed or validation fails.</remarks>
    /// <exception cref="InvalidDataException">The tape framing or backup data is invalid or incomplete.</exception>
    /// <exception cref="NotSupportedException">The tape or backup uses an unsupported feature.</exception>
    public TapContainer(Stream stream) : base(stream)
    {
        Initialize();
    }

    /// <summary>
    /// Creates the logical tape view and eagerly loads the backup index, releasing resources on failure.
    /// </summary>
    private void Initialize()
    {
        try
        {
            _content = new TapStream(containerStream);
            // Load eagerly so malformed/unsupported backups are reported by the open-image dialog.
            if (PartitionTable.Partitions[0].FileSystem is not MtfFileSystem)
            {
                throw new InvalidDataException("The tape does not contain a supported NT Backup (MTF) archive.");
            }
        }
        catch
        {
            Dispose();
            throw;
        }
    }

    /// <inheritdoc />
    protected override PartitionTable LoadPartitionTable()
    {
        return new NoPartitionTable(this);
    }

    /// <inheritdoc />
    /// <exception cref="NotSupportedException">Tape images do not support writing.</exception>
    protected override Container CloneWriteable(Stream stream)
    {
        throw new NotSupportedException("Tape images are read-only.");
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _content?.Dispose();
        }
        base.Dispose(disposing);
    }
}
