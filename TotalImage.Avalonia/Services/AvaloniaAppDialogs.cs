using System;
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
 *   Done:      About (Dialogs/AboutDialog.axaml)
 *   Remaining: Settings (TODO phase B: Dialogs/SettingsDialog.axaml)
 *
 * Each dialog method has its own region, so that dialogs can be added in parallel with few merge
 * conflicts. See AvaloniaEntryDialogs for how a dialog window is added.
 */
public sealed class AvaloniaAppDialogs(WindowOwner owner, ILauncherService launcher,
    ISettingsService settings, ITempDirectoryService tempDirectory, IAppInstances instances,
    IMessageService messages, IFilePickerService filePicker) : IAppDialogs
{
    #region About

    public async Task ShowAboutAsync()
    {
        var viewModel = new AboutViewModel(launcher, Assembly.GetEntryAssembly());
        await DialogHost.ShowDialogAsync(new AboutDialog(viewModel), owner.Window);
    }

    #endregion

    #region Settings

    public Task<SettingsChangeResult?> ShowSettingsAsync()
    {
        // TODO (phase B): Dialogs/SettingsDialog.axaml bound to the view model below. The messages
        // and pickers its commands show belong on top of the dialog: give it its own WindowOwner and
        // services (DialogHost.ServicesFor) instead of the scope's messages and filePicker.
        // After accepting, apply the color theme (App.ApplyColorTheme) if it changed.
        _ = (settings, tempDirectory, instances, messages, filePicker);
        throw new NotImplementedException("The settings dialog isn't available in the Avalonia UI yet.");
    }

    #endregion
}
