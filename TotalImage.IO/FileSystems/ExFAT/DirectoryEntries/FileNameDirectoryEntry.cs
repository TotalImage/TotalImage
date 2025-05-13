using System;
using System.Text;

namespace TotalImage.FileSystems.ExFAT;

public class FileNameDirectoryEntry : SecondaryDirectoryEntry
{
    public string FileName { get; }

    public FileNameDirectoryEntry(in ReadOnlySpan<byte> entry) : base(entry)
    {
        FileName = Encoding.Unicode.GetString(entry[2..32]);
    }

    public override uint FirstCluster => throw new InvalidOperationException();
    public override ulong DataLength => throw new InvalidOperationException();
}
