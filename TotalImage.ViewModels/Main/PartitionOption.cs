namespace TotalImage.ViewModels.Main;

/// <summary>
/// A partition that can be loaded, as listed in the partition picker of the main window.
/// </summary>
/// <param name="Index">The index of the partition in the partition table.</param>
/// <param name="Text">The label, file system and size, prefixed by the index when the image has several partitions.</param>
public sealed record PartitionOption(int Index, string Text)
{
    public override string ToString() => Text;
}
