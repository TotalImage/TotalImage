using System.Threading.Tasks;
using System.Windows.Forms;
using TotalImage.Dialogs;
using TotalImage.ViewModels.Dialogs;
using TotalImage.ViewModels.Services;

namespace TotalImage.Services
{
    public sealed class WinFormsImageDialogs(DialogOwner owner) : IImageDialogs
    {
        public Task<FormatOptions?> AskFormatOptionsAsync()
        {
            var viewModel = new FormatViewModel();
            using var dialog = new FormatDialog(viewModel);
            return Task.FromResult(dialog.ShowDialog(owner.Window) == DialogResult.OK ? viewModel.Answer : null);
        }
    }
}
