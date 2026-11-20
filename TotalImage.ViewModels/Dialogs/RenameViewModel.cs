using CommunityToolkit.Mvvm.ComponentModel;

namespace TotalImage.ViewModels.Dialogs;

/// <summary>
/// Asks for a new name for a file or folder. The answer is the new name.
/// </summary>
public partial class RenameViewModel : DialogViewModel<string>
{
    public RenameViewModel(string currentName)
    {
        CurrentName = currentName;
        Name = currentName;
    }

    /// <summary>
    /// The name the entry has now.
    /// </summary>
    public string CurrentName { get; }

    /// <summary>
    /// The new name.
    /// </summary>
    [ObservableProperty]
    public partial string Name { get; set; }

    /// <summary>
    /// The 8.3 short name for <see cref="Name"/>. Not computed yet.
    /// </summary>
    public string ShortName => string.Empty;

    partial void OnNameChanged(string value) => RefreshCanAccept();

    protected override bool CanAccept() => !string.IsNullOrWhiteSpace(Name);

    protected override string CreateAnswer() => Name;
}
