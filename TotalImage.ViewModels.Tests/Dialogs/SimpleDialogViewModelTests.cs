using System.Collections.Generic;
using System.Reflection;
using TotalImage.ViewModels.Dialogs;
using TotalImage.ViewModels.Services;
using Xunit;

namespace TotalImage.ViewModels.Tests.Dialogs;

public class RenameViewModelTests
{
    [Fact]
    public void StartsWithTheCurrentName()
    {
        var vm = new RenameViewModel("OLD.TXT");

        Assert.Equal("OLD.TXT", vm.CurrentName);
        Assert.Equal("OLD.TXT", vm.Name);
        Assert.True(vm.AcceptCommand.CanExecute(null));
        Assert.Null(vm.Answer);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void CannotAcceptAnEmptyName(string name)
    {
        var vm = new RenameViewModel("OLD.TXT") { Name = name };

        Assert.False(vm.AcceptCommand.CanExecute(null));
    }

    [Fact]
    public void CanExecuteChangedIsRaisedWhenTheNameChanges()
    {
        var vm = new RenameViewModel("OLD.TXT");
        var raised = 0;
        vm.AcceptCommand.CanExecuteChanged += (_, _) => raised++;

        vm.Name = "";
        vm.Name = "NEW.TXT";

        Assert.Equal(2, raised);
    }

    [Fact]
    public void AcceptAnswersWithTheNewName()
    {
        var vm = new RenameViewModel("OLD.TXT") { Name = "NEW.TXT" };

        vm.AcceptCommand.Execute(null);

        Assert.Equal("NEW.TXT", vm.Answer);
    }
}

public class NewFolderViewModelTests
{
    [Fact]
    public void StartsEmptyAndCannotAccept()
    {
        var vm = new NewFolderViewModel();

        Assert.Equal("", vm.Name);
        Assert.False(vm.AcceptCommand.CanExecute(null));
    }

    [Fact]
    public void AcceptAnswersWithTheName()
    {
        var vm = new NewFolderViewModel { Name = "DOCS" };

        Assert.True(vm.AcceptCommand.CanExecute(null));
        vm.AcceptCommand.Execute(null);

        Assert.Equal("DOCS", vm.Answer);
    }
}

public class VolumeLabelViewModelTests
{
    [Fact]
    public void InitialState_WritesAndSyncsTheBpbLabel()
    {
        var vm = new VolumeLabelViewModel("ROOT", "BPB");

        Assert.Equal("ROOT", vm.RootDirectoryLabel);
        Assert.True(vm.WriteBpbLabel);
        Assert.True(vm.SyncLabels);
        Assert.Equal("ROOT", vm.BpbLabel);
        Assert.True(vm.CanEditBpbLabel);
        Assert.True(vm.IsBpbLabelReadOnly);
        Assert.True(vm.AcceptCommand.CanExecute(null));
    }

    [Fact]
    public void WhileSynced_BpbLabelFollowsRootLabel()
    {
        var vm = new VolumeLabelViewModel("ROOT", "BPB");

        vm.RootDirectoryLabel = "NEW";

        Assert.Equal("NEW", vm.BpbLabel);
    }

    [Fact]
    public void WhenNotSynced_BpbLabelIsIndependentAndEditable()
    {
        var vm = new VolumeLabelViewModel("ROOT", "BPB") { SyncLabels = false };

        vm.BpbLabel = "OTHER";
        vm.RootDirectoryLabel = "NEW";

        Assert.Equal("OTHER", vm.BpbLabel);
        Assert.False(vm.IsBpbLabelReadOnly);
    }

    [Fact]
    public void ReEnablingSync_CopiesRootLabel()
    {
        var vm = new VolumeLabelViewModel("ROOT", "BPB") { SyncLabels = false };
        vm.BpbLabel = "OTHER";

        vm.SyncLabels = true;

        Assert.Equal("ROOT", vm.BpbLabel);
    }

    [Fact]
    public void TurningOffBpbLabel_ClearsItAndTurnsOffSync()
    {
        var vm = new VolumeLabelViewModel("ROOT", "BPB");
        var changed = new List<string?>();
        vm.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

        vm.WriteBpbLabel = false;

        Assert.Equal("", vm.BpbLabel);
        Assert.False(vm.SyncLabels);
        Assert.False(vm.CanEditBpbLabel);
        Assert.Contains(nameof(VolumeLabelViewModel.CanEditBpbLabel), changed);
        Assert.Contains(nameof(VolumeLabelViewModel.IsBpbLabelReadOnly), changed);
    }

    [Fact]
    public void TurningOnBpbLabel_CopiesRootLabel()
    {
        var vm = new VolumeLabelViewModel("ROOT", "BPB") { WriteBpbLabel = false };

        vm.WriteBpbLabel = true;

        Assert.Equal("ROOT", vm.BpbLabel);
        Assert.True(vm.CanEditBpbLabel);
    }

    [Fact]
    public void Answer_IsUpperCaseAndPadded()
    {
        var vm = new VolumeLabelViewModel("root", "bpb") { SyncLabels = false };
        vm.BpbLabel = "other";

        vm.AcceptCommand.Execute(null);

        Assert.Equal(new VolumeLabelChange("ROOT       ", true, "OTHER      "), vm.Answer);
    }
}

public class FormatViewModelTests
{
    [Fact]
    public void Defaults()
    {
        var vm = new FormatViewModel();

        Assert.True(vm.QuickFormat);
        Assert.Equal("", vm.VolumeLabel);
        Assert.Null(vm.FileSystem);
        Assert.Null(vm.ClusterSize);
        Assert.True(vm.AcceptCommand.CanExecute(null));
    }

    [Fact]
    public void AcceptAnswersWithTheOptions()
    {
        var vm = new FormatViewModel { VolumeLabel = "DISK", QuickFormat = false };

        vm.AcceptCommand.Execute(null);

        Assert.Equal(new FormatOptions(null, null, "DISK", false), vm.Answer);
    }
}

public class AboutViewModelTests
{
    private sealed class FakeLauncher : ILauncherService
    {
        public List<string> Opened { get; } = [];
        public void OpenUrl(string url) => Opened.Add(url);
        public void OpenFolder(string path) => Opened.Add(path);

        public void RevealFile(string path) => throw new System.NotSupportedException();
    }

    [Fact]
    public void VersionText_ContainsVersionAndRelease()
    {
        var assembly = typeof(AboutViewModel).Assembly;
        var vm = new AboutViewModel(new FakeLauncher(), assembly);

        Assert.StartsWith($"Version: {assembly.GetName().Version} {AboutViewModel.ReleaseName} (", vm.VersionText);
    }

    [Fact]
    public void VersionText_WithoutAssembly_DoesNotThrow()
    {
        var vm = new AboutViewModel(new FakeLauncher(), null);

        Assert.StartsWith("Version:", vm.VersionText);
    }

    [Fact]
    public void OpenProjectPage_OpensTheProjectUrl()
    {
        var launcher = new FakeLauncher();
        var vm = new AboutViewModel(launcher, Assembly.GetExecutingAssembly());

        vm.OpenProjectPageCommand.Execute(null);

        Assert.Equal([AboutViewModel.ProjectUrl], launcher.Opened);
        Assert.True(vm.ProjectPageVisited);
    }
}
