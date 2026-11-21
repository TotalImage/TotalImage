using Microsoft.Extensions.DependencyInjection;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Settings;

namespace TotalImage.ViewModels;

/// <summary>
/// Registers the framework-neutral services shared by every UI.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the shared app-wide services: settings, temp-directory management and the launcher.
    /// Each UI adds its own implementations of the dialog and picker services.
    /// </summary>
    public static IServiceCollection AddTotalImageViewModels(this IServiceCollection services)
    {
        services.AddSingleton<ISettingsService>(_ => new SettingsService(SettingsService.DefaultDirectory));
        services.AddSingleton<ITempDirectoryService, TempDirectoryService>();
        services.AddSingleton<ILauncherService, ProcessLauncherService>();
        services.AddSingleton<IAppInstances, ProcessAppInstances>();
        services.AddSingleton<IImageHashService, ImageHashService>();
        services.AddSingleton<IImageFileService, ImageFileService>();
        return services;
    }
}
