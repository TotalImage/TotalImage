using System;
using Avalonia;

namespace TotalImage.Avalonia;

internal static class Program
{
    // Avalonia isn't initialized before AppMain is called: don't use any Avalonia, third-party
    // or SynchronizationContext-reliant code before then.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

    // Also used by the visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();
}
