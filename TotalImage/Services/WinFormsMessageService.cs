using System.Threading.Tasks;
using System.Windows.Forms;
using TotalImage.ViewModels.Services;

namespace TotalImage.Services
{
    public sealed class WinFormsMessageService(DialogOwner owner) : IMessageService
    {
        public Task<bool> ConfirmFormatAsync()
        {
            var result = ShowTaskDialog(new TaskDialogPage()
            {
                Text = "Are you sure you want to continue?",
                Heading = "WARNING: Formatting will erase all data in the current partition!",
                Caption = "Warning",
                Buttons =
                {
                    TaskDialogButton.Yes,
                    TaskDialogButton.No
                },
                Icon = TaskDialogIcon.Warning,
            });

            return Task.FromResult(result == TaskDialogButton.Yes);
        }

        private TaskDialogButton ShowTaskDialog(TaskDialogPage page)
            => owner.Window is null ? TaskDialog.ShowDialog(page) : TaskDialog.ShowDialog(owner.Window, page);
    }
}
