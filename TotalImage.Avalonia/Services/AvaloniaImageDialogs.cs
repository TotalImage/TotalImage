using System;
using System.Threading.Tasks;
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

    public override Task<FormatOptions?> AskFormatOptionsAsync()
    {
        // TODO (phase B): Dialogs/FormatDialog.axaml bound to new FormatViewModel().
        _ = owner; // The window the dialogs are shown on top of.
        throw new NotImplementedException("The format dialog isn't available in the Avalonia UI yet.");
    }

    #endregion

    #region Select partition

    protected override Task<PartitionSelection?> ShowSelectPartitionAsync(Func<DialogServices, SelectPartitionViewModel> createViewModel)
    {
        // TODO (phase B): Dialogs/SelectPartitionDialog.axaml (the prototype on origin/avalonia-prototype
        // has a layout). Needed to open images with several partitions.
        throw new NotImplementedException("The partition selection dialog isn't available in the Avalonia UI yet.");
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

    public override Task<NewImageOptions?> AskNewImageAsync()
    {
        // TODO (phase B): Dialogs/NewImageDialog.axaml bound to
        // new NewImageViewModel(new AvaloniaMessageService(dialogOwner), ShowAdvancedOptions), where
        // ShowAdvancedOptions shows Dialogs/NewImageAdvancedDialog.axaml on top of the new image dialog.
        throw new NotImplementedException("The new image dialog isn't available in the Avalonia UI yet.");
    }

    #endregion

    #region Image information

    protected override Task ShowImageInfoAsync(Func<DialogServices, ImageInfoViewModel> createViewModel)
    {
        // TODO (phase B): Dialogs/ImageInfoDialog.axaml.
        throw new NotImplementedException("The image information dialog isn't available in the Avalonia UI yet.");
    }

    #endregion

    #region Boot sector

    protected override Task ShowBootSectorAsync(Func<DialogServices, BootSectorViewModel> createViewModel)
    {
        // TODO (phase B): Dialogs/BootSectorDialog.axaml.
        throw new NotImplementedException("The boot sector dialog isn't available in the Avalonia UI yet.");
    }

    #endregion
}
