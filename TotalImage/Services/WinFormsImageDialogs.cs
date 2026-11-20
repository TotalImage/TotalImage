using System.Threading.Tasks;
using System.Windows.Forms;
using TotalImage.Containers;
using TotalImage.Dialogs;
using TotalImage.ViewModels.Dialogs;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Settings;

namespace TotalImage.Services
{
    public sealed class WinFormsImageDialogs(DialogOwner owner, ISettingsService settings) : IImageDialogs
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
    }
}
