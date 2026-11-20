using System;
using System.Linq;
using TotalImage.ViewModels.Settings;
using Xunit;

namespace TotalImage.ViewModels.Tests;

public class ReferenceGuardTests
{
    private static readonly string[] ForbiddenPrefixes =
    [
        "System.Windows.Forms",
        "System.Drawing",
        "Avalonia",
        "PresentationFramework",
        "PresentationCore",
        "WindowsBase"
    ];

    [Fact]
    public void ViewModelsAssembly_DoesNotReferenceAnyUIFramework()
    {
        var references = typeof(SettingsService).Assembly.GetReferencedAssemblies();

        var forbidden = references
            .Where(r => ForbiddenPrefixes.Any(p => r.Name!.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            .Select(r => r.Name)
            .ToList();

        Assert.Empty(forbidden);
    }
}
