using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using TotalImage.Avalonia.Views;

namespace TotalImage.Avalonia.Services;

/// <summary>
/// Opens main windows, each in its own scope (an "image session"), and keeps track of the open ones.
/// </summary>
public sealed class MainWindows(IServiceProvider services)
{
    private readonly List<MainWindow> windows = [];

    /// <summary>
    /// The number of open main windows.
    /// </summary>
    public int Count => windows.Count;

    /// <summary>
    /// Opens a new main window, optionally opening an image file in it.
    /// </summary>
    public MainWindow Open(string? imagePath = null)
    {
        var scope = services.CreateScope();
        var window = scope.ServiceProvider.GetRequiredService<MainWindow>();
        window.StartupImagePath = imagePath;
        windows.Add(window);
        window.Closed += (_, _) =>
        {
            windows.Remove(window);
            scope.Dispose();
        };
        window.Show();
        return window;
    }
}
