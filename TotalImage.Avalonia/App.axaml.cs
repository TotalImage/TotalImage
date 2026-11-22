using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TotalImage.Avalonia.Converters;
using TotalImage.Avalonia.Dialogs;
using TotalImage.Avalonia.Services;
using TotalImage.Avalonia.Views;
using TotalImage.ViewModels;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Settings;

namespace TotalImage.Avalonia;

public partial class App : Application
{
    private ServiceProvider? services;

    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Built here, after Avalonia installed its synchronization context, so services that
            // capture it (like the settings file watcher) post back to the UI thread.
            services = ConfigureServices().BuildServiceProvider(new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });

            ApplyColorTheme(services.GetRequiredService<ISettingsService>().Current.ColorMode);
            Resources[FileIconConverter.SmallKey] = new FileIconConverter(services.GetRequiredService<FileIcons>(), large: false);
            Resources[FileIconConverter.LargeKey] = new FileIconConverter(services.GetRequiredService<FileIcons>(), large: true);

            Dispatcher.UIThread.UnhandledException += OnUnhandledException;

            desktop.ShutdownMode = ShutdownMode.OnLastWindowClose;
            desktop.Exit += (_, _) => services.Dispose();

            // The file dragged onto the executable or passed on the command line.
            services.GetRequiredService<MainWindows>().Open(desktop.Args?.FirstOrDefault());
        }

        base.OnFrameworkInitializationCompleted();
    }

    /// <summary>
    /// Follows the color theme of the settings.
    /// </summary>
    public static void ApplyColorTheme(ColorTheme theme)
    {
        if (Current is not { } app)
            return;

        app.RequestedThemeVariant = theme switch
        {
            ColorTheme.Classic => ThemeVariant.Light,
            ColorTheme.Dark => ThemeVariant.Dark,
            _ => ThemeVariant.Default
        };
    }

    private static ServiceCollection ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddTotalImageViewModels();

        // App-wide
        services.AddSingleton<AvaloniaUIStateService>();
        services.AddSingleton<MainWindows>();
        services.Replace(ServiceDescriptor.Singleton<IAppInstances, AvaloniaAppInstances>());
        services.AddSingleton<IFileTypeInfo>(provider => FileTypeInfoFactory.Create(provider.GetRequiredService<ISettingsService>()));
        services.AddSingleton<FileIcons>();

        // One scope per main window (an "image session").
        services.AddScoped<WindowOwner>();
        services.AddScoped<IMessageService, AvaloniaMessageService>();
        services.AddScoped<IFilePickerService, AvaloniaFilePickerService>();
        services.AddScoped<IEntryDialogs, AvaloniaEntryDialogs>();
        services.AddScoped<IImageDialogs, AvaloniaImageDialogs>();
        services.AddScoped<IAppDialogs, AvaloniaAppDialogs>();
        services.AddScoped<MainWindow>();
        return services;
    }

    // Features that aren't built yet (formatting, for example) throw NotImplementedException.
    // Say so instead of closing the app.
    private void OnUnhandledException(object? sender, DispatcherUnhandledExceptionEventArgs e)
    {
        if (e.Exception is not NotImplementedException)
            return;

        e.Handled = true;
        var owner = (ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows.FirstOrDefault(window => window.IsActive);
        _ = MessageWindow.ShowAsync(owner, new MessageOptions(
            "Not available yet",
            "This feature isn't available in this version of the user interface yet.",
            "Information",
            MessageIcon.Information,
            [MessageButton.Ok]));
    }
}
