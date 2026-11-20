using System.Collections.Generic;
using TotalImage.ViewModels.Dialogs;
using Xunit;

namespace TotalImage.ViewModels.Tests.Dialogs;

public class ManagePartitionsViewModelTests
{
    private static readonly ManagedPartitionRow[] TwoRows =
    [
        new("0", "DISK", "FAT12", "1", "720", "360 KiB", "Yes"),
        new("1", "", "HPFS/NTFS/exFAT", "721", "728", "4 KiB", "No"),
    ];

    [Fact]
    public void InitialState_NothingSelected()
    {
        var vm = new ManagePartitionsViewModel(TwoRows);

        Assert.Equal(TwoRows, vm.Rows);
        Assert.Equal(-1, vm.SelectedIndex);
        Assert.False(vm.HasSelection);
    }

    [Fact]
    public void WithoutRows_NothingCanBeSelected()
    {
        var vm = new ManagePartitionsViewModel([]);

        vm.SelectedIndex = 0;

        Assert.Empty(vm.Rows);
        Assert.False(vm.HasSelection);
    }

    [Fact]
    public void SelectingARow_EnablesThePartitionActions()
    {
        var vm = new ManagePartitionsViewModel(TwoRows);
        var changed = new List<string?>();
        vm.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

        vm.SelectedIndex = 1;

        Assert.True(vm.HasSelection);
        Assert.Contains(nameof(ManagePartitionsViewModel.HasSelection), changed);
    }

    [Fact]
    public void ClearingTheSelection_DisablesThePartitionActions()
    {
        var vm = new ManagePartitionsViewModel(TwoRows) { SelectedIndex = 0 };
        var changed = new List<string?>();
        vm.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

        vm.SelectedIndex = -1;

        Assert.False(vm.HasSelection);
        Assert.Contains(nameof(ManagePartitionsViewModel.HasSelection), changed);
    }

    [Fact]
    public void AnOutOfRangeSelection_DoesNotCount()
    {
        var vm = new ManagePartitionsViewModel(TwoRows);

        vm.SelectedIndex = 2;

        Assert.False(vm.HasSelection);
    }
}
