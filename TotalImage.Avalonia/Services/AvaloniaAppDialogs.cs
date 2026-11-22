using System.Reflection;
using System.Threading.Tasks;
using TotalImage.Avalonia.Dialogs;
using TotalImage.ViewModels.Dialogs;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Settings;

namespace TotalImage.Avalonia.Services;

/*
 * Application dialogs (IAppDialogs).
 *
 *   Done:      About (Dialogs/AboutDialog.axaml), Settings (Dialogs/SettingsDialog.axaml)
 *
 * Each dialog method has its own region, so that dialogs can be added in parallel with few merge
 * conflicts. See AvaloniaEntryDialogs for how a dialog window is added.
 */
public sealed class AvaloniaAppDialogs(WindowOwner owner, ILauncherService launcher,
    ISettingsService settings, ITempDirectoryService tempDirectory, IAppInstances instances) : IAppDialogs
{
    #region About

    public async Task ShowAboutAsync()
    {
        var viewModel = new AboutViewModel(launcher, Assembly.GetEntryAssembly());
        await DialogHost.ShowDialogAsync(new AboutDialog(viewModel), owner.Window);
    }

    #endregion

    #region Settings

    public async Task<SettingsChangeResult?> ShowSettingsAsync()
    {
        // The messages and pickers the dialog's commands show belong on top of the dialog.
        var dialogOwner = new WindowOwner();
        var services = DialogHost.ServicesFor(dialogOwner);
        var viewModel = new SettingsViewModel(settings, tempDirectory, instances, services.Messages, launcher, services.FilePicker);
        var dialog = new SettingsDialog(viewModel);
        dialogOwner.Window = dialog;

        if (!await DialogHost.ShowDialogAsync(dialog, owner.Window))
            return null;

        var change = viewModel.Answer;
        if (change?.ColorThemeChanged == true)
            App.ApplyColorTheme(settings.Current.ColorMode);

        return change;
    }

    #endregion
}
