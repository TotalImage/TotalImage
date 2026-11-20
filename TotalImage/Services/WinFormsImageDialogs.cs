using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using TotalImage.Containers;
using TotalImage.Dialogs;
using TotalImage.ViewModels.Dialogs;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Settings;

namespace TotalImage.Services
{
    public sealed class WinFormsImageDialogs(DialogOwner owner, ISettingsService settings, IImageHashService hashService) : IImageDialogs
    {
        public Task<FormatOptions?> AskFormatOptionsAsync()
        {
            var viewModel = new FormatViewModel();
            using var dialog = new FormatDialog(viewModel);
            return Task.FromResult(dialog.ShowDialog(owner.Window) == DialogResult.OK ? viewModel.Answer : null);
        }

        public Task<PartitionSelection?> AskSelectPartitionAsync(Container image)
        {
            // Messages raised from inside the dialog are owned by the dialog, not the main window.
            var dialogOwner = new DialogOwner();
            var viewModel = new SelectPartitionViewModel(image, settings, new WinFormsMessageService(dialogOwner));
            using var dialog = new SelectPartitionDialog(viewModel);
            dialogOwner.Window = dialog;
            return Task.FromResult(dialog.ShowDialog(owner.Window) == DialogResult.OK ? viewModel.Answer : null);
        }

        public Task ShowManagePartitionsAsync()
        {
            // TODO: List the image's partitions once partition management is built.
            var viewModel = new ManagePartitionsViewModel([]);
            using var dialog = new ManagePartitionsDialog(viewModel);
            dialog.ShowDialog(owner.Window);
            return Task.CompletedTask;
        }

        public Task<NewImageOptions?> AskNewImageAsync()
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

        public Task ShowImageInfoAsync(Container image, string? filePath, int partitionIndex)
        {
            // Pickers and messages opened from the dialog belong to the dialog, not the main window.
            var dialogOwner = new DialogOwner();
            var viewModel = new ImageInfoViewModel(image, filePath, partitionIndex, settings, hashService,
                new WinFormsFilePickerService(dialogOwner), new WinFormsMessageService(dialogOwner), Assembly.GetEntryAssembly());
            using var dialog = new ImageInfoDialog(viewModel);
            dialogOwner.Window = dialog;
            dialog.ShowDialog(owner.Window);
            return Task.CompletedTask;
        }

        public Task ShowBootSectorAsync(Container image, int partitionIndex)
        {
            var dialogOwner = new DialogOwner();
            var viewModel = new BootSectorViewModel(image, partitionIndex,
                new WinFormsFilePickerService(dialogOwner), new WinFormsMessageService(dialogOwner));
            using var dialog = new BootSectorDialog(viewModel);
            dialogOwner.Window = dialog;
            dialog.ShowDialog(owner.Window);
            return Task.CompletedTask;
        }
    }
}
