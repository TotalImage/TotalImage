using System.Reflection;
using System.Threading.Tasks;
using TotalImage.Dialogs;
using TotalImage.ViewModels.Dialogs;
using TotalImage.ViewModels.Services;

namespace TotalImage.Services
{
    public sealed class WinFormsAppDialogs(DialogOwner owner, ILauncherService launcher) : IAppDialogs
    {
        public Task ShowAboutAsync()
        {
            var viewModel = new AboutViewModel(launcher, Assembly.GetEntryAssembly());
            using var dialog = new AboutDialog(viewModel);
            dialog.ShowDialog(owner.Window);
            return Task.CompletedTask;
        }
    }
}
