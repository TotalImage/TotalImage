using CommunityToolkit.Mvvm.ComponentModel;

namespace TotalImage.ViewModels.Dialogs;

/// <summary>
/// Asks for the name of a new folder. The answer is the name.
/// </summary>
public partial class NewFolderViewModel : DialogViewModel<string>
{
    public NewFolderViewModel()
    {
        Name = string.Empty;
    }

    /// <summary>
    /// The name of the new folder.
    /// </summary>
    [ObservableProperty]
    public partial string Name { get; set; }

    /// <summary>
    /// The 8.3 short name for <see cref="Name"/>. Not computed yet.
    /// </summary>
    // TODO: Convert long name to short (8.3) name here
    public string ShortName => string.Empty;

    partial void OnNameChanged(string value) => RefreshCanAccept();

    protected override bool CanAccept() => !string.IsNullOrWhiteSpace(Name);

    protected override string CreateAnswer() => Name;
}
