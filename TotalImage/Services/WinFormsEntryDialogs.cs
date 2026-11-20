using System.Threading.Tasks;
using System.Windows.Forms;
using TotalImage.Dialogs;
using TotalImage.ViewModels.Dialogs;
using TotalImage.ViewModels.Services;

namespace TotalImage.Services
{
    public sealed class WinFormsEntryDialogs(DialogOwner owner) : IEntryDialogs
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
    }
}
