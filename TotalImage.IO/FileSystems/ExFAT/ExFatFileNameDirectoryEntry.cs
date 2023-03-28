using System;
using System.Text;

namespace TotalImage.FileSystems.ExFAT;

public class ExFatFileNameDirectoryEntry
{
    public byte EntryType { get; }
    public byte GeneralSecondaryFlags { get; }
    public string FileName { get; }

    public ExFatFileNameDirectoryEntry(in ReadOnlySpan<byte> entry)
    {
        EntryType = entry[0];
        GeneralSecondaryFlags = entry[1];
        FileName = Encoding.Unicode.GetString(entry[2..32]);
    }
}
