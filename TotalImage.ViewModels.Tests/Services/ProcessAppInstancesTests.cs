using System.Collections.Generic;
using TotalImage.ViewModels.Services;
using Xunit;

namespace TotalImage.ViewModels.Tests.Services;

public class ProcessAppInstancesTests
{
    private const int CurrentId = 100;

    private static ProcessAppInstances Create(Dictionary<string, int[]> processes)
        => new(name => processes.TryGetValue(name, out var ids) ? ids : [], CurrentId);

    [Fact]
    public void OnlyThisProcess_IsNotAnotherInstance()
    {
        var instances = Create(new() { ["TotalImage.Avalonia"] = [CurrentId] });

        Assert.False(instances.AreOtherInstancesRunning());
    }

    [Theory]
    [InlineData("TotalImage")]
    [InlineData("TotalImage.Avalonia")]
    public void AnotherProcessOfEitherApp_IsAnotherInstance(string otherName)
    {
        var processes = new Dictionary<string, int[]> { ["TotalImage.Avalonia"] = [CurrentId] };
        processes[otherName] = [.. processes.GetValueOrDefault(otherName, []), 200];

        Assert.True(Create(processes).AreOtherInstancesRunning());
    }

    [Fact]
    public void ProcessesOfOtherApps_DontCount()
    {
        var instances = Create(new() { ["TotalImage"] = [CurrentId], ["notepad"] = [300] });

        Assert.False(instances.AreOtherInstancesRunning());
    }

    [Fact]
    public void BothAppsShareTheTemporaryFolder()
        => Assert.Equal(["TotalImage", "TotalImage.Avalonia"], ProcessAppInstances.ProcessNames);
}
