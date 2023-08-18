namespace TotalImage.Containers.BinCue;

/// <summary>
/// The type of track on a CD
/// </summary>
public enum CdTrackType
{ 
    /// <summary>
    /// Audio track (2352 bytes for data)
    /// </summary>
    Audio,

    /// <summary>
    /// Mode 1 track (2048 bytes for data)
    /// </summary>
    Mode1,

    /// <summary>
    /// Mode 2 track (2336 bytes for data)
    /// </summary>
    Mode2,

    /// <summary>
    /// Mode 2 XA Form 1 track (2048 bytes for data)
    /// </summary>
    Mode2Form1,

    /// <summary>
    /// Mode 2 XA Form 2 track (2324 bytes for data)
    /// </summary>
    Mode2Form2
}
