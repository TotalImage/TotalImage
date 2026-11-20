namespace TotalImage.ViewModels.Dialogs;

/// <summary>
/// A value offered in a list, with the text shown for it.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
/// <param name="Value">The value.</param>
/// <param name="Name">The text shown for the value.</param>
public sealed record NamedOption<T>(T Value, string Name)
{
    /// <summary>
    /// Returns <see cref="Name"/>, so list controls show it without extra setup.
    /// </summary>
    public override string ToString() => Name;
}
