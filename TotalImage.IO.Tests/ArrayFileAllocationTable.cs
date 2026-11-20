using TotalImage.FileSystems.FAT;

namespace TotalImage.IO.Tests;

/// <summary>
/// In-memory FAT for unit tests: <paramref name="length"/> entries with the given cluster mask.
/// </summary>
internal sealed class ArrayFileAllocationTable(uint length, uint mask = 0xFFFF) : FileAllocationTable
{
    private readonly uint[] _entries = new uint[length];

    protected override uint Mask => mask;

    public override uint Length => (uint)_entries.Length;

    protected override uint GetEntry(uint index) => _entries[index];

    protected override void SetEntry(uint index, uint value) => _entries[index] = value & mask;
}
