using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using TotalImage.Dialogs;
using TotalImage.ViewModels.Dialogs;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Settings;

namespace TotalImage.Services
{
    public sealed class WinFormsImageDialogs(DialogOwner owner, ISettingsService settings, IImageHashService hashService)
        : ImageDialogsBase(settings, hashService)
    {
        public override Task<FormatOptions?> AskFormatOptionsAsync()
        {
            var viewModel = new FormatViewModel();
            using var dialog = new FormatDialog(viewModel);
            return Task.FromResult(dialog.ShowDialog(owner.Window) == DialogResult.OK ? viewModel.Answer : null);
        }

        protected override Task<PartitionSelection?> ShowSelectPartitionAsync(Func<DialogServices, SelectPartitionViewModel> createViewModel)
        {
            // Messages raised from inside the dialog are owned by the dialog, not the main window.
            var dialogOwner = new DialogOwner();
            var viewModel = createViewModel(ServicesFor(dialogOwner));
            using var dialog = new SelectPartitionDialog(viewModel);
            dialogOwner.Window = dialog;
            return Task.FromResult(dialog.ShowDialog(owner.Window) == DialogResult.OK ? viewModel.Answer : null);
        }

        public override Task ShowManagePartitionsAsync()
        {
            // TODO: List the image's partitions once partition management is built.
            var viewModel = new ManagePartitionsViewModel([]);
            using var dialog = new ManagePartitionsDialog(viewModel);
            dialog.ShowDialog(owner.Window);
            return Task.CompletedTask;
        }

        public override Task<NewImageOptions?> AskNewImageAsync()
        {
            // Questions asked while the dialog is open (and the advanced options) belong to the dialog.
            var dialogOwner = new DialogOwner();
            var viewModel = new NewImageViewModel(new WinFormsMessageService(dialogOwner), ShowAdvancedOptions);
            using var dialog = new NewImageDialog(viewModel);
            dialogOwner.Window = dialog;
            return Task.FromResult(dialog.ShowDialog(owner.Window) == DialogResult.OK ? viewModel.Answer : null);

            Task ShowAdvancedOptions(NewImageAdvancedViewModel advanced)
            {
                using var advancedDialog = new NewImageAdvancedDialog(advanced);
                advancedDialog.ShowDialog(dialogOwner.Window);
                return Task.CompletedTask;
            }
        }

        protected override Task ShowImageInfoAsync(Func<DialogServices, ImageInfoViewModel> createViewModel)
        {
            // Pickers and messages opened from the dialog belong to the dialog, not the main window.
            var dialogOwner = new DialogOwner();
            var viewModel = createViewModel(ServicesFor(dialogOwner));
            using var dialog = new ImageInfoDialog(viewModel);
            dialogOwner.Window = dialog;
            dialog.ShowDialog(owner.Window);
            return Task.CompletedTask;
        }

        protected override Task ShowBootSectorAsync(Func<DialogServices, BootSectorViewModel> createViewModel)
        {
            var dialogOwner = new DialogOwner();
            var viewModel = createViewModel(ServicesFor(dialogOwner));
            using var dialog = new BootSectorDialog(viewModel);
            dialogOwner.Window = dialog;
            dialog.ShowDialog(owner.Window);
            return Task.CompletedTask;
        }

        // The pickers and messages of a dialog, owned by its window once it is set on the owner.
        private static DialogServices ServicesFor(DialogOwner dialogOwner)
            => new(new WinFormsMessageService(dialogOwner), new WinFormsFilePickerService(dialogOwner));
    }
}
