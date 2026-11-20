using System;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using TotalImage.Forms;
using TotalImage.Services;
using TotalImage.ViewModels;
using TotalImage.ViewModels.Services;
using TotalImage.ViewModels.Settings;

namespace TotalImage
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Install the UI synchronization context before any service is created, so services
            // that capture it (like the settings file watcher) post back to the UI thread.
            SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());

            using var services = ConfigureServices().BuildServiceProvider(new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });

            Application.SetColorMode(services.GetRequiredService<ISettingsService>().Current.ColorMode.ToWinForms());

            // One scope per main window (an "image session").
            using var scope = services.CreateScope();
            Application.Run(scope.ServiceProvider.GetRequiredService<MainForm>());
        }

        private static ServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddTotalImageViewModels();
            services.AddSingleton<UIStateService>();
            services.AddScoped<DialogOwner>();
            services.AddScoped<IEntryDialogs, WinFormsEntryDialogs>();
            services.AddScoped<IImageDialogs, WinFormsImageDialogs>();
            services.AddScoped<IAppDialogs, WinFormsAppDialogs>();
            services.AddScoped<IMessageService, WinFormsMessageService>();
            services.AddScoped<MainForm>();
            return services;
        }
    }
}
