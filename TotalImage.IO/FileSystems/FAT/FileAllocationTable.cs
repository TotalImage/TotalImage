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
    /// Determines the type of a cluster map value.
    /// </summary>
    public override ClusterType GetClusterType(uint cluster)
    {
        if (cluster > Mask - 0x10)
        {
            if ((cluster & 0xF) == 7)
                return ClusterType.Bad;
            if ((cluster & 0xF) > 7)
                return ClusterType.EndOfChain;
            
            return ClusterType.Reserved;
        }
        else if (cluster < 2)
        {
            if (cluster == 0)
                return ClusterType.Free;
            if (cluster == 1)
                return ClusterType.NonFree;
        }

        return ClusterType.Data;
    }

}
