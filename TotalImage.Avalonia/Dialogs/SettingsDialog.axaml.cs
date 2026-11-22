using System;
using System.Diagnostics;
using Avalonia.Controls;
using TotalImage.ViewModels.Dialogs;

namespace TotalImage.Avalonia.Dialogs;

/// <summary>
/// Edits the application settings. Binding only, plus the Windows-only file associations button.
/// </summary>
public partial class SettingsDialog : Window
{
    // For the XAML previewer.
    public SettingsDialog()
    {
        InitializeComponent();

        // Both options of the Integration tab are about Windows.
        integrationTab.IsVisible = OperatingSystem.IsWindows();
        fileAssociationsButton.Click += (_, _) => OpenDefaultAppsSettings();
    }

    public SettingsDialog(SettingsViewModel viewModel) : this()
    {
        DataContext = viewModel;
        this.BindAccept(okButton, viewModel.AcceptCommand);
        this.BindCancel(cancelButton);
    }

    // Opening the Windows "Default apps" settings is Windows-only, so it stays in this view.
    private static void OpenDefaultAppsSettings()
    {
        if (OperatingSystem.IsWindows())
            Process.Start(new ProcessStartInfo("ms-settings:defaultapps") { UseShellExecute = true })?.Dispose();
    }
}
