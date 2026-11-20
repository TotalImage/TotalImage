namespace TotalImage.FileSystems.FAT;

/// <summary>
/// Base class for accessing a FAT-style cluster map
/// </summary>
public abstract class FileAllocationTable : FileAllocationTableBase
{
    /// <summary>
    /// Writes the value stored at a given index.
    /// </summary>
    protected abstract void SetEntry(uint index, uint value);

    /// <summary>
    /// Access the value stored at a given index.
    /// </summary>
    public new uint this[uint index]
    {
        get => GetEntry(index);
        set => SetEntry(index, value);
    }

    /// <summary>
    /// Cluster number bit mask. This is also the maximum cluster map value.
    /// </summary>
    protected abstract uint Mask { get; }

    /// <summary>
    /// The end-of-chain marker value for this FAT type (the highest valid EOC value).
    /// </summary>
    public uint EndOfChainMarker => Mask;

    /// <summary>
    /// Access the value stored at a given index.
    /// </summary>
    public new uint this[int index]
    {
        get => GetEntry((uint)index);
        set => SetEntry((uint)index, value);
    }

    /// <summary>
    /// Classifies a FAT cluster map value following the FAT specification, where MAX is the highest
    /// cluster number in the table (<see cref="FileAllocationTableBase.Length"/> - 1): 0 is free,
    /// 2 to MAX is a data cluster pointer, MAX+1 to xF6 (reserved, must not be used) is invalid,
    /// xF7 is bad and xF8–xFF are end-of-chain. 1 is not defined by the specification; DOS writes it temporarily while building
    /// a chain, so it only appears on disk after an interrupted allocation.
    /// </summary>
    public override ClusterType GetClusterType(uint cluster) => cluster switch
    {
        0 => ClusterType.Free,
        1 => ClusterType.NonFree,
        _ when cluster == Mask - 0x8 => ClusterType.Bad,
        _ when cluster >= Mask - 0x7 && cluster <= Mask => ClusterType.EndOfChain,
        _ when cluster < Length => ClusterType.Data,
        _ => ClusterType.Invalid
    };

}
