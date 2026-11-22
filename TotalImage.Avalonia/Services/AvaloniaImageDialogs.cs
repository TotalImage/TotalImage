using System;
using System.Threading.Tasks;
using TotalImage.Avalonia.Dialogs;
using TotalImage.ViewModels.Dialogs;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Settings;

namespace TotalImage.Avalonia.Services;

/*
 * Image dialogs (IImageDialogs).
 *
 *   Done:      (none yet)
 *   Remaining (TODO phase B): Format, Select partition, Manage partitions, New image (+ advanced),
 *              Image information, Boot sector
 *
 * Each dialog method has its own region, so that dialogs can be added in parallel with few merge
 * conflicts. See AvaloniaEntryDialogs for how a dialog window is added. The protected Show*Async
 * methods get a factory for the view model: create the dialog's own services first,
 *     var dialogOwner = new WindowOwner();
 *     var viewModel = createViewModel(DialogHost.ServicesFor(dialogOwner));
 *     var dialog = new XDialog(viewModel);
 *     dialogOwner.Window = dialog;
 * so the pickers and messages the dialog opens are owned by the dialog, not the main window.
 */
public sealed class AvaloniaImageDialogs(WindowOwner owner, ISettingsService settings, IImageHashService hashService)
    : ImageDialogsBase(settings, hashService)
{
    #region Format

    public override async Task<FormatOptions?> AskFormatOptionsAsync()
    {
        var viewModel = new FormatViewModel();
        return await DialogHost.ShowDialogAsync(new FormatDialog(viewModel), owner.Window) ? viewModel.Answer : null;
    }

    #endregion

    #region Select partition

    protected override async Task<PartitionSelection?> ShowSelectPartitionAsync(Func<DialogServices, SelectPartitionViewModel> createViewModel)
    {
        // The message about unsupported partitions belongs to the dialog, not the main window.
        var dialogOwner = new WindowOwner();
        var viewModel = createViewModel(DialogHost.ServicesFor(dialogOwner));
        var dialog = new SelectPartitionDialog(viewModel);
        dialogOwner.Window = dialog;
        return await DialogHost.ShowDialogAsync(dialog, owner.Window) ? viewModel.Answer : null;
    }

    #endregion

    #region Manage partitions

    public override Task ShowManagePartitionsAsync()
    {
        // TODO (phase B): Dialogs/ManagePartitionsDialog.axaml bound to new ManagePartitionsViewModel([]),
        // like the WinForms service (partition management isn't built yet).
        throw new NotImplementedException("The partition manager isn't available in the Avalonia UI yet.");
    }

    #endregion

    #region New image

    public override async Task<NewImageOptions?> AskNewImageAsync()
    {
        // Questions asked while the dialog is open (and the advanced options) belong to the dialog.
        var dialogOwner = new WindowOwner();
        var viewModel = new NewImageViewModel(DialogHost.ServicesFor(dialogOwner).Messages, ShowAdvancedOptions);
        var dialog = new NewImageDialog(viewModel);
        dialogOwner.Window = dialog;
        return await DialogHost.ShowDialogAsync(dialog, owner.Window) ? viewModel.Answer : null;

        Task ShowAdvancedOptions(NewImageAdvancedViewModel advanced)
            => DialogHost.ShowDialogAsync(new NewImageAdvancedDialog(advanced), dialogOwner.Window);
    }

    #endregion

    #region Image information

    protected override async Task ShowImageInfoAsync(Func<DialogServices, ImageInfoViewModel> createViewModel)
    {
        // The export's picker and messages belong to the dialog, not the main window.
        var dialogOwner = new WindowOwner();
        var viewModel = createViewModel(DialogHost.ServicesFor(dialogOwner));
        var dialog = new Dialogs.ImageInfoDialog(viewModel);
        dialogOwner.Window = dialog;
        await DialogHost.ShowDialogAsync(dialog, owner.Window);
    }

    #endregion

    #region Boot sector

    protected override async Task ShowBootSectorAsync(Func<DialogServices, BootSectorViewModel> createViewModel)
    {
        // The export's picker and messages belong to the dialog, not the main window.
        var dialogOwner = new WindowOwner();
        var viewModel = createViewModel(DialogHost.ServicesFor(dialogOwner));
        var dialog = new Dialogs.BootSectorDialog(viewModel);
        dialogOwner.Window = dialog;
        await DialogHost.ShowDialogAsync(dialog, owner.Window);
    }

    #endregion
}
