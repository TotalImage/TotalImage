using System;
using System.Text;

namespace TotalImage.FileSystems.ExFAT;

public class VolumeLabelDirectoryEntry : PrimaryDirectoryEntry
{
    public byte CharacterCount { get; }
    public string VolumeLabel { get; }

    public VolumeLabelDirectoryEntry(in ReadOnlySpan<byte> entry) : base(entry)
    {
        CharacterCount = entry[1];
        VolumeLabel = Encoding.Unicode.GetString(entry[2..24]);
    }

    public override ushort GeneralPrimaryFlags => throw new InvalidOperationException();
    public override uint FirstCluster => throw new InvalidOperationException();
    public override ulong DataLength => throw new InvalidOperationException();
}
