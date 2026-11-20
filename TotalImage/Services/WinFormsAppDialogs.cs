using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using TotalImage.Dialogs;
using TotalImage.ViewModels.Dialogs;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Settings;

namespace TotalImage.Services
{
    public sealed class WinFormsAppDialogs(DialogOwner owner, ILauncherService launcher,
        ISettingsService settings, ITempDirectoryService tempDirectory, IAppInstances instances,
        IMessageService messages, IFilePickerService filePicker) : IAppDialogs
    {
        public Task ShowAboutAsync()
        {
            var viewModel = new AboutViewModel(launcher, Assembly.GetEntryAssembly());
            using var dialog = new AboutDialog(viewModel);
            dialog.ShowDialog(owner.Window);
            return Task.CompletedTask;
        }

        public Task<SettingsChangeResult?> ShowSettingsAsync()
        {
            var viewModel = new SettingsViewModel(settings, tempDirectory, instances, messages, launcher, filePicker);
            using var dialog = new SettingsDialog(viewModel);

            // The messages and pickers the dialog's commands show belong on top of the dialog,
            // so it owns them while it is open.
            var mainWindow = owner.Window;
            owner.Window = dialog;
            try
            {
                return Task.FromResult(dialog.ShowDialog(mainWindow) == DialogResult.OK ? viewModel.Answer : null);
            }
            finally
            {
                owner.Window = mainWindow;
            }
        }
    }
}
