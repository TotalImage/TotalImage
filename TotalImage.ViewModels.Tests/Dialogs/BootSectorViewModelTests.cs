using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TotalImage.Containers;
using TotalImage.ViewModels.Dialogs;
using Xunit;
using static TotalImage.ViewModels.Tests.Dialogs.ImageDialogFakes;

namespace TotalImage.ViewModels.Tests.Dialogs;

public class BootSectorViewModelTests
{
    private readonly Messages messages = new();
    private readonly FilePicker picker = new();

    private BootSectorViewModel Create(Container image) => new(image, 0, picker, messages);

    private static byte[] Read(Container image, long offset, int length)
    {
        var bytes = new byte[length];
        image.Content.Seek(offset, SeekOrigin.Begin);
        image.Content.ReadExactly(bytes);
        return bytes;
    }

    [Fact]
    public void AnUnpartitionedImageShowsItsVolumeBootRecordOnly()
    {
        var image = CreateFloppy();
        var vm = Create(image);

        Assert.False(vm.CanShowDisk);
        Assert.Equal(BootSectorSource.Partition, vm.Source);
        Assert.True(vm.IsPartitionSelected);
        Assert.False(vm.IsDiskSelected);
        Assert.Equal(Read(image, 0, 512), vm.Bytes);
    }

    [Fact]
    public void TheDiskCannotBeSelectedWithoutAPartitionTable()
    {
        var vm = Create(CreateFloppy());
        var changes = new List<string?>();
        vm.PropertyChanged += (_, e) => changes.Add(e.PropertyName);

        vm.IsDiskSelected = true;
        vm.Source = BootSectorSource.Disk;

        Assert.Equal(BootSectorSource.Partition, vm.Source);
        Assert.Empty(changes);
    }

    [Fact]
    public void APartitionedDiskShowsThePartitionFirstAndTheMbrOnRequest()
    {
        var image = CreatePartitionedDisk();
        var vm = Create(image);

        Assert.True(vm.CanShowDisk);
        Assert.Equal(Read(image, 512, 512), vm.Bytes);

        var changes = new List<string?>();
        vm.PropertyChanged += (_, e) => changes.Add(e.PropertyName);
        vm.IsDiskSelected = true;

        Assert.Equal(BootSectorSource.Disk, vm.Source);
        Assert.True(vm.IsDiskSelected);
        Assert.False(vm.IsPartitionSelected);
        Assert.Equal(Read(image, 0, 512), vm.Bytes);
        Assert.Equal(0xFA, vm.Bytes[0]);
        Assert.Contains(nameof(BootSectorViewModel.IsPartitionSelected), changes);
        Assert.Contains(nameof(BootSectorViewModel.IsDiskSelected), changes);
        Assert.Contains(nameof(BootSectorViewModel.Bytes), changes);
        Assert.Contains(nameof(BootSectorViewModel.HexText), changes);

        vm.IsPartitionSelected = true;
        Assert.Equal(Read(image, 512, 512), vm.Bytes);
    }

    [Fact]
    public void ClearingASelectionDoesNothing()
    {
        var vm = Create(CreatePartitionedDisk());

        vm.IsPartitionSelected = false;

        Assert.Equal(BootSectorSource.Partition, vm.Source);
    }

    [Fact]
    public void HexTextShowsSixteenBytesPerLine()
    {
        var vm = Create(CreateFloppy());

        var lines = vm.HexText.Split(Environment.NewLine);

        Assert.Equal(32, lines.Length);
        Assert.All(lines, l => Assert.Equal(16, l.TrimEnd().Split(' ').Length));
        Assert.Equal(string.Join(" ", vm.Bytes.Take(16).Select(b => b.ToString("X2"))), lines[0]);
        Assert.EndsWith("55 AA ", lines[^1]);
    }

    [Fact]
    public async Task ExportWritesTheShownBytes()
    {
        using var folder = new TempFolder();
        picker.SavePath = folder.File("boot.bin");
        var vm = Create(CreatePartitionedDisk());
        vm.IsDiskSelected = true;

        await vm.ExportCommand.ExecuteAsync(null);

        Assert.Equal(BootSectorViewModel.ExportFileTypes, picker.LastFilters);
        Assert.Equal("bin", picker.LastDefaultExtension);
        Assert.Equal(vm.Bytes, File.ReadAllBytes(picker.SavePath));
        Assert.Equal([picker.SavePath], messages.ExportSucceeded);
        Assert.Empty(messages.Errors);
    }

    [Fact]
    public async Task CancellingThePickerExportsNothing()
    {
        var vm = Create(CreateFloppy());

        await vm.ExportCommand.ExecuteAsync(null);

        Assert.Equal(1, picker.SaveRequests);
        Assert.Empty(messages.ExportSucceeded);
        Assert.Empty(messages.Errors);
    }

    [Fact]
    public async Task AFailedExportShowsTheError()
    {
        using var folder = new TempFolder();
        picker.SavePath = Path.Combine(folder.Path, "missing", "boot.bin");
        var vm = Create(CreateFloppy());

        await vm.ExportCommand.ExecuteAsync(null);

        var error = Assert.Single(messages.Errors);
        Assert.Contains(picker.SavePath, error.Operation);
        Assert.IsAssignableFrom<IOException>(error.Exception);
        Assert.Empty(messages.ExportSucceeded);
    }

    [Fact]
    public void ExportIsAlwaysAvailable()
    {
        Assert.True(Create(CreateFloppy()).ExportCommand.CanExecute(null));
    }
}
