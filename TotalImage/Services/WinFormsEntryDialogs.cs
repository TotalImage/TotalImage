using System.Threading.Tasks;
using System.Windows.Forms;
using TotalImage.Dialogs;
using TotalImage.ViewModels.Dialogs;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Settings;

namespace TotalImage.Services
{
    public sealed class WinFormsEntryDialogs(DialogOwner owner, ISettingsService settings, IFilePickerService filePicker, IMessageService messages) : IEntryDialogs
    {
        public Task<string?> AskRenameAsync(string currentName)
        {
            var viewModel = new RenameViewModel(currentName);
            using var dialog = new RenameDialog(viewModel);
            return Task.FromResult(dialog.ShowDialog(owner.Window) == DialogResult.OK ? viewModel.Answer : null);
        }

        public Task<string?> AskNewFolderNameAsync()
        {
            var viewModel = new NewFolderViewModel();
            using var dialog = new NewFolderDialog(viewModel);
            return Task.FromResult(dialog.ShowDialog(owner.Window) == DialogResult.OK ? viewModel.Answer : null);
        }

        public Task<VolumeLabelChange?> AskVolumeLabelAsync(string rootDirectoryLabel, string bpbLabel)
        {
            var viewModel = new VolumeLabelViewModel(rootDirectoryLabel, bpbLabel);
            using var dialog = new ChangeVolumeLabelDialog(viewModel);
            return Task.FromResult(dialog.ShowDialog(owner.Window) == DialogResult.OK ? viewModel.Answer : null);
        }

        public Task<ExtractOptions?> AskExtractOptionsAsync(int itemCount)
        {
            var viewModel = new ExtractViewModel(itemCount, settings, filePicker, messages);
            using var dialog = new ExtractDialog(viewModel);
            return Task.FromResult(ShowWithNestedDialogs(dialog) == DialogResult.OK ? viewModel.Answer : null);
        }

        // Shows a dialog whose view model asks further questions, so that the pickers and
        // messages it opens are owned by the dialog rather than the main window.
        private DialogResult ShowWithNestedDialogs(Form dialog)
        {
            var parent = owner.Window;
            owner.Window = dialog;
            try
            {
                return dialog.ShowDialog(parent);
            }
            finally
            {
                owner.Window = parent;
            }
        }
    }
}
