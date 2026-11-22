using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TotalImage.Avalonia.Dialogs;
using TotalImage.ViewModels.Dialogs;
using TotalImage.ViewModels.Extraction;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Settings;

namespace TotalImage.Avalonia.Services;

/*
 * Entry dialogs (IEntryDialogs).
 *
 *   Done:      Rename, New folder, Volume label, Extract options, Properties, Extraction progress
 *
 * How to add a dialog window (the pattern every dialog follows):
 *   1. Dialogs/<Name>Dialog.axaml with x:DataType set to the shared view model
 *      (TotalImage.ViewModels.Dialogs.<Name>ViewModel), compiled bindings to its properties, an OK
 *      button whose Command is bound to AcceptCommand (so it is disabled while the input is invalid)
 *      and a Cancel button with IsCancel="True". No logic in the window: binding only.
 *   2. Dialogs/<Name>Dialog.axaml.cs: a parameterless constructor for the previewer, and one taking
 *      the view model that sets DataContext and calls this.BindAccept(okButton, viewModel.AcceptCommand)
 *      and this.BindCancel(cancelButton) (Dialogs/DialogWindowExtensions.cs).
 *   3. Here, in the method's own region: create the view model, show the window with
 *      DialogHost.ShowDialogAsync(dialog, owner.Window), and return viewModel.Answer when it was
 *      accepted, or null. A view model that asks further questions (pickers, messages) gets services
 *      owned by the dialog: var dialogOwner = new WindowOwner(); var services = DialogHost.ServicesFor(dialogOwner);
 *      create the view model with them, then set dialogOwner.Window = dialog before showing it.
 * See AskRenameAsync below for a worked example.
 */
public sealed class AvaloniaEntryDialogs(WindowOwner owner, ISettingsService settings, IFileTypeInfo fileTypeInfo, FileIcons icons)
    : EntryDialogsBase(settings, fileTypeInfo)
{
    #region Rename

    public override async Task<string?> AskRenameAsync(string currentName)
    {
        var viewModel = new RenameViewModel(currentName);
        return await DialogHost.ShowDialogAsync(new RenameDialog(viewModel), owner.Window) ? viewModel.Answer : null;
    }

    #endregion

    #region New folder

    public override async Task<string?> AskNewFolderNameAsync()
    {
        var viewModel = new NewFolderViewModel();
        return await DialogHost.ShowDialogAsync(new NewFolderDialog(viewModel), owner.Window) ? viewModel.Answer : null;
    }

    #endregion

    #region Volume label

    public override async Task<VolumeLabelChange?> AskVolumeLabelAsync(string rootDirectoryLabel, string bpbLabel)
    {
        var viewModel = new VolumeLabelViewModel(rootDirectoryLabel, bpbLabel);
        return await DialogHost.ShowDialogAsync(new ChangeVolumeLabelDialog(viewModel), owner.Window) ? viewModel.Answer : null;
    }

    #endregion

    #region Extract options

    public override async Task<ExtractOptions?> AskExtractOptionsAsync(int itemCount)
    {
        // The folder picker and the question about creating the folder belong to the dialog.
        var dialogOwner = new WindowOwner();
        var services = DialogHost.ServicesFor(dialogOwner);
        var viewModel = new ExtractViewModel(itemCount, Settings, services.FilePicker, services.Messages);
        var dialog = new ExtractDialog(viewModel);
        dialogOwner.Window = dialog;
        return await DialogHost.ShowDialogAsync(dialog, owner.Window) ? viewModel.Answer : null;
    }

    #endregion

    #region Properties

    private static readonly Lazy<global::Avalonia.Media.IImage> MultipleEntriesIcon = new(() =>
    {
        using var stream = global::Avalonia.Platform.AssetLoader.Open(new Uri("avares://TotalImage.Avalonia/Assets/page_white_stack.png"));
        return new global::Avalonia.Media.Imaging.Bitmap(stream);
    });

    protected override async Task<EntryPropertiesChange?> ShowPropertiesAsync(PropertiesViewModel viewModel, string firstName, FileAttributes firstAttributes)
    {
        var icon = viewModel.IsMultiple
            ? MultipleEntriesIcon.Value
            : icons.Get(ViewModels.Browsing.IconKey.ForEntry(firstName, firstAttributes), large: true);
        return await DialogHost.ShowDialogAsync(new PropertiesDialog(viewModel, icon), owner.Window) ? viewModel.Answer : null;
    }

    #endregion

    #region Extraction progress

    public override async Task ShowExtractionProgressAsync(Func<IProgress<ExtractionProgress>, CancellationToken, Task> extraction)
    {
        var window = new ExtractionProgressWindow(extraction);

        // Questions asked during the extraction (like overwriting a file) belong to the progress window.
        var parent = owner.Window;
        window.ExtractionStarting += (_, _) => owner.Window = window;
        try
        {
            await DialogHost.ShowDialogAsync(window, parent);
        }
        finally
        {
            owner.Window = parent;
        }

        if (window.Failure is { } failure)
            throw failure;
    }

    #endregion
}
