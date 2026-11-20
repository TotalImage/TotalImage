using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TotalImage.ViewModels.Dialogs;

/// <summary>
/// Base for the view model behind a dialog that answers a question. Only the UI's dialog
/// service uses it: it creates the view model, shows a window bound to it, and returns
/// <see cref="Answer"/> when the window was accepted.
/// </summary>
/// <typeparam name="TAnswer">The typed answer the dialog produces.</typeparam>
public abstract partial class DialogViewModel<TAnswer> : ObservableObject where TAnswer : class
{
    /// <summary>
    /// The answer, set when <see cref="AcceptCommand"/> runs. <c>null</c> until then.
    /// </summary>
    public TAnswer? Answer { get; private set; }

    /// <summary>
    /// Accepts the dialog: builds <see cref="Answer"/>. The view closes the window afterwards.
    /// Disabled while the input is not valid.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanAccept))]
    private void Accept() => Answer = CreateAnswer();

    /// <summary>
    /// Whether the current input can be accepted. Call <see cref="RefreshCanAccept"/> when
    /// the result may have changed.
    /// </summary>
    protected virtual bool CanAccept() => true;

    /// <summary>
    /// Builds the answer from the current input. Only called when <see cref="CanAccept"/> is true.
    /// </summary>
    protected abstract TAnswer CreateAnswer();

    /// <summary>
    /// Re-evaluates whether <see cref="AcceptCommand"/> can run.
    /// </summary>
    protected void RefreshCanAccept() => AcceptCommand.NotifyCanExecuteChanged();
}
