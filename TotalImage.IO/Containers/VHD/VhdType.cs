namespace TotalImage.Containers.VHD;

/// <summary>
/// The type of virtual hard drive
/// </summary>
public enum VhdType : uint
{
    /// <summary>
    /// No type of virtual hard drive is specified
    /// </summary>
    None = 0,

    /// <summary>
    /// The virtual hard drive is fixed-size
    /// </summary>
    FixedHardDisk = 2,

    /// <summary>
    /// The virtual hard drive sizes dynamically
    /// </summary>
    DynamicHardDisk = 3,

    /// <summary>
    /// The virtual hard drive represents a difference set from another virtual hard drive
    /// </summary>
    DifferencingHardDisk = 4
}
