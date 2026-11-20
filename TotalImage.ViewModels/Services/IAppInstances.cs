namespace TotalImage.ViewModels.Services;

/// <summary>
/// Knows about other running instances of the application, which share its temporary folder.
/// </summary>
public interface IAppInstances
{
    /// <summary>
    /// Whether another instance of the application is running.
    /// </summary>
    bool AreOtherInstancesRunning();
}
