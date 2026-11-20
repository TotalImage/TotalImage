using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TotalImage.Containers;
using TotalImage.ViewModels.Dialogs;
using TotalImage.ViewModels.Formatting;
using TotalImage.ViewModels.Services;
using Xunit;
using static TotalImage.ViewModels.Tests.Dialogs.ImageDialogFakes;

namespace TotalImage.ViewModels.Tests.Dialogs;

public class ImageInfoViewModelTests
{
    private readonly Messages messages = new();
    private readonly FilePicker picker = new();

    private ImageInfoViewModel Create(Container image, string? filePath = null, IImageHashService? hashes = null,
        SizeUnit sizeUnit = SizeUnit.Bytes, TimeProvider? time = null)
        => new(image, filePath, 0, new FakeSettings(sizeUnit), hashes ?? new ImageHashService(), picker, messages,
            typeof(ImageInfoViewModelTests).Assembly, time);

    private static string Value(System.Collections.Generic.IEnumerable<PropertyRow> rows, string name)
        => rows.Single(r => r.Name == name).Value;

    [Fact]
    public void FileRowsAreNotAvailableForAnUnsavedImageAndHashesArePending()
    {
        var vm = Create(CreateFloppy());

        Assert.Equal(
            ["Filename", "Path", "Size", "Created", "Modified", "Accessed", "Attributes", "CRC-32 hash", "MD5 hash", "SHA-1 hash"],
            vm.FileRows.Select(r => r.Name));
        Assert.All(vm.FileRows.Take(7), r => Assert.True(r.IsNotAvailable));
        Assert.All(vm.FileRows.Skip(7), r =>
        {
            Assert.True(r.IsPending);
            Assert.Equal(ImageInfoViewModel.PendingHashText, r.Value);
        });
        Assert.All(vm.FileRows, r => Assert.Equal(PropertyGroup.File, r.Group));
        Assert.False(vm.HashesDone);
    }

    [Fact]
    public void FileRowsDescribeTheImageFile()
    {
        using var folder = new TempFolder();
        var path = folder.File("disk.img");
        File.WriteAllBytes(path, new byte[2048]);

        var vm = Create(CreateFloppy(), path);

        Assert.Equal("disk.img", Value(vm.FileRows, "Filename"));
        Assert.Equal(path, Value(vm.FileRows, "Path"));
        Assert.Equal(SizeUnit.Bytes.FormatSize(2048, false), Value(vm.FileRows, "Size"));
    }

    [Fact]
    public void SizesUseTheSizeUnitSetting()
    {
        var bytes = Create(CreateFloppy(), sizeUnit: SizeUnit.Bytes);
        var binary = Create(CreateFloppy(), sizeUnit: SizeUnit.Binary);

        Assert.Equal(SizeUnit.Bytes.FormatSize(1474560, false), Value(bytes.FileSystemRows, "Total storage capacity"));
        Assert.Equal(SizeUnit.Binary.FormatSize(1474560, true), Value(binary.FileSystemRows, "Total storage capacity"));
    }

    [Fact]
    public void ContainerAndPartitionTableRowsDescribeARawUnpartitionedImage()
    {
        var vm = Create(CreateFloppy());

        Assert.Equal(["Container type", "Container subtype", "Container version", "Created by", "Creator version"], vm.ContainerRows.Select(r => r.Name));
        Assert.Equal("Raw sector image", Value(vm.ContainerRows, "Container type"));
        Assert.True(vm.ContainerRows.Single(r => r.Name == "Created by").IsNotAvailable);

        Assert.Equal(["Partitioning scheme", "Number of partitions", "Selected partition"], vm.PartitionTableRows.Select(r => r.Name));
        Assert.Equal("Unpartitioned", Value(vm.PartitionTableRows, "Partitioning scheme"));
        Assert.Equal("1", Value(vm.PartitionTableRows, "Number of partitions"));
        Assert.Equal("0", Value(vm.PartitionTableRows, "Selected partition"));

        Assert.Equal(["Partition ID/type"], vm.PartitionRows.Select(r => r.Name));
        Assert.True(vm.PartitionRows[0].IsNotAvailable);
    }

    [Fact]
    public void FileSystemRowsListTheBpbInOrder()
    {
        var vm = Create(CreateFloppy());

        Assert.Equal(
        [
            "File system", "Jump instruction", "OEM ID", "Sector size", "Sectors per cluster", "Reserved sectors",
            "Number of FATs", "Root directory entries", "Total sectors", "Media descriptor", "Sectors per FAT",
            "Sectors per track", "Heads/sides", "Hidden sectors", "Volume label", "Volume serial number",
            "Total storage capacity", "Free storage capacity", "Files", "Subdirectories"
        ], vm.FileSystemRows.Select(r => r.Name));
        Assert.Equal("FAT12", Value(vm.FileSystemRows, "File system"));
        Assert.Equal("TESTDISK", Value(vm.FileSystemRows, "Volume label"));
        Assert.Equal("0xF0", Value(vm.FileSystemRows, "Media descriptor"));
        Assert.Equal("0", Value(vm.FileSystemRows, "Files"));
        Assert.True(vm.FileSystemRows.Single(r => r.Name == "Volume serial number").IsNotAvailable);
        Assert.All(vm.FileSystemRows, r => Assert.Equal(PropertyGroup.FileSystem, r.Group));
    }

    [Fact]
    public void AnMbrDiskListsTheTableAndTheSelectedEntry()
    {
        var vm = Create(CreatePartitionedDisk());

        Assert.Equal(
            ["Partitioning scheme", "Physical drive number", "Disk timestamp", "Disk serial number", "Number of partitions", "Selected partition"],
            vm.PartitionTableRows.Select(r => r.Name));
        Assert.Equal("Master Boot Record", Value(vm.PartitionTableRows, "Partitioning scheme"));
        Assert.Equal("1", Value(vm.PartitionTableRows, "Number of partitions"));
        Assert.True(vm.PartitionTableRows.Single(r => r.Name == "Disk serial number").IsNotAvailable);

        Assert.Equal(
            ["Partition ID/type", "Active", "First sector CHS", "Last sector CHS", "First sector LBA", "Total sectors", "Total size"],
            vm.PartitionRows.Select(r => r.Name));
        Assert.StartsWith("0x01 - ", Value(vm.PartitionRows, "Partition ID/type"));
        Assert.Equal("Yes", Value(vm.PartitionRows, "Active"));
        Assert.Equal("1", Value(vm.PartitionRows, "First sector LBA"));
        Assert.Equal("FAT12", Value(vm.FileSystemRows, "File system"));
    }

    [Fact]
    public void ARawContainerHasNoComment()
    {
        var vm = Create(CreateFloppy());

        Assert.False(vm.SupportsComment);
        Assert.Equal(ImageInfoViewModel.NoCommentText, vm.Comment);
    }

    [Theory]
    [InlineData("0x12", false, true)]
    [InlineData("N/A", false, false)]
    [InlineData("  ", false, false)]
    [InlineData("Please wait...", true, false)]
    public void CanCopyOnlyAvailableFinishedValues(string value, bool pending, bool expected)
    {
        Assert.Equal(expected, ImageInfoViewModel.CanCopyValue(new PropertyRow(PropertyGroup.File, "X", value, pending)));
    }

    [Fact]
    public void CannotCopyWithoutARow()
    {
        Assert.False(ImageInfoViewModel.CanCopyValue(null));
    }

    [Fact]
    public async Task ComputeHashesFillsTheHashRows()
    {
        var image = CreateFloppy();
        var vm = Create(image);
        var bytes = ((MemoryStream)image.ContainerFileStream).ToArray();

        Assert.True(vm.ComputeHashesCommand.CanExecute(null));
        await vm.ComputeHashesCommand.ExecuteAsync(null);

        Assert.True(vm.HashesDone);
        Assert.Equal(Convert.ToHexStringLower(MD5.HashData(bytes)), Value(vm.FileRows, "MD5 hash"));
        Assert.Equal(Convert.ToHexStringLower(SHA1.HashData(bytes)), Value(vm.FileRows, "SHA-1 hash"));
        Assert.Matches("^[0-9a-f]{8}$", Value(vm.FileRows, "CRC-32 hash"));
        Assert.DoesNotContain(vm.FileRows, r => r.IsPending);
        Assert.False(vm.ComputeHashesCommand.CanExecute(null));
    }

    [Fact]
    public async Task HashRowsChangeOneByOne()
    {
        var hashes = new ManualHashService();
        var vm = Create(CreateFloppy(), hashes: hashes);
        var replaced = 0;
        ((System.Collections.Specialized.INotifyCollectionChanged)vm.FileRows).CollectionChanged += (_, _) => replaced++;

        var run = vm.ComputeHashesCommand.ExecuteAsync(null);
        Assert.False(vm.ComputeHashesCommand.CanExecute(null));
        Assert.Equal([ImageHashKind.Crc32], hashes.Requests);

        hashes.Complete("crc");
        await WaitUntil(() => hashes.Requests.Count == 2);
        Assert.Equal("crc", Value(vm.FileRows, "CRC-32 hash"));
        Assert.True(vm.FileRows.Single(r => r.Name == "MD5 hash").IsPending);
        Assert.False(vm.HashesDone);

        hashes.Complete("md5");
        await WaitUntil(() => hashes.Requests.Count == 3);
        hashes.Complete("sha1");
        await run;

        Assert.Equal([ImageHashKind.Crc32, ImageHashKind.Md5, ImageHashKind.Sha1], hashes.Requests);
        Assert.Equal(3, replaced);
        Assert.True(vm.HashesDone);
    }

    [Fact]
    public async Task CancellingTheHashesMarksThemDone()
    {
        var hashes = new ManualHashService();
        var vm = Create(CreateFloppy(), hashes: hashes);
        var canExecuteChanged = 0;
        vm.ComputeHashesCommand.CanExecuteChanged += (_, _) => canExecuteChanged++;

        var run = vm.ComputeHashesCommand.ExecuteAsync(null);
        vm.ComputeHashesCommand.Cancel();
        await run;

        Assert.True(vm.HashesDone);
        Assert.True(vm.FileRows.Single(r => r.Name == "CRC-32 hash").IsPending);
        Assert.False(vm.ComputeHashesCommand.CanExecute(null));
        Assert.True(canExecuteChanged > 0);
    }

    [Fact]
    public async Task ExportWhileHashingAsksFirstAndStopsWhenDeclined()
    {
        var vm = Create(CreateFloppy(), hashes: new ManualHashService());
        messages.ConfirmAnswer = false;

        await vm.ExportCommand.ExecuteAsync(null);

        Assert.Equal(1, messages.ExportWithoutHashesQuestions);
        Assert.Equal(0, picker.SaveRequests);
    }

    [Fact]
    public async Task ExportWhileHashingLeavesTheHashesOut()
    {
        using var folder = new TempFolder();
        picker.SavePath = folder.File("report.txt");
        var vm = Create(CreateFloppy(), hashes: new ManualHashService());

        await vm.ExportCommand.ExecuteAsync(null);

        Assert.Equal(1, messages.ExportWithoutHashesQuestions);
        Assert.Equal(ImageInfoViewModel.ReportFileTypes, picker.LastFilters);
        Assert.Equal("txt", picker.LastDefaultExtension);
        var report = File.ReadAllText(picker.SavePath);
        Assert.Contains("-Filename: N/A", report);
        Assert.DoesNotContain("hash:", report);
        Assert.Empty(messages.Errors);
    }

    [Fact]
    public async Task ExportAfterHashingIncludesTheHashesWithoutAsking()
    {
        using var folder = new TempFolder();
        picker.SavePath = folder.File("report.txt");
        var vm = Create(CreateFloppy());
        await vm.ComputeHashesCommand.ExecuteAsync(null);

        await vm.ExportCommand.ExecuteAsync(null);

        Assert.Equal(0, messages.ExportWithoutHashesQuestions);
        var report = File.ReadAllText(picker.SavePath);
        Assert.Contains($"-MD5 hash: {Value(vm.FileRows, "MD5 hash")}", report);
    }

    [Fact]
    public async Task CancellingThePickerWritesNothing()
    {
        var vm = Create(CreateFloppy());
        await vm.ComputeHashesCommand.ExecuteAsync(null);
        picker.SavePath = null;

        await vm.ExportCommand.ExecuteAsync(null);

        Assert.Equal(1, picker.SaveRequests);
        Assert.Empty(messages.Errors);
    }

    [Fact]
    public async Task AFailedExportShowsTheError()
    {
        using var folder = new TempFolder();
        picker.SavePath = Path.Combine(folder.Path, "missing", "report.txt");
        var vm = Create(CreateFloppy());
        await vm.ComputeHashesCommand.ExecuteAsync(null);

        await vm.ExportCommand.ExecuteAsync(null);

        var error = Assert.Single(messages.Errors);
        Assert.Contains(picker.SavePath, error.Operation);
        Assert.IsAssignableFrom<IOException>(error.Exception);
    }

    [Fact]
    public async Task ExportCannotRunTwiceAtOnce()
    {
        var gate = new TaskCompletionSource<bool>();
        var slowMessages = new SlowConfirmMessages(gate.Task);
        var vm = new ImageInfoViewModel(CreateFloppy(), null, 0, new FakeSettings(), new ManualHashService(), picker, slowMessages, null);

        Assert.True(vm.ExportCommand.CanExecute(null));
        var run = vm.ExportCommand.ExecuteAsync(null);
        Assert.False(vm.ExportCommand.CanExecute(null));

        gate.SetResult(false);
        await run;
        Assert.True(vm.ExportCommand.CanExecute(null));
    }

    [Fact]
    public void TheReportListsEverySection()
    {
        var time = new FixedTime(new DateTimeOffset(2026, 11, 20, 13, 14, 15, TimeSpan.Zero));
        var vm = Create(CreateFloppy(), time: time);

        var report = vm.BuildReport();
        var lines = report.Split(Environment.NewLine);

        Assert.Equal("************************************", lines[0]);
        Assert.Equal("*     Image information report     *", lines[2]);
        Assert.Equal($"-TotalImage version: {typeof(ImageInfoViewModelTests).Assembly.GetName().Version} ()", lines[4]);
        Assert.Equal("-Created on: 2026-11-20 13:14:15", lines[5]);
        Assert.Equal("", lines[6]);
        Assert.Equal("** File information **", lines[7]);
        Assert.Contains("** Container information **", lines);
        Assert.Contains("-Container type: Raw sector image", lines);
        Assert.Contains("** Partition table information **", lines);
        Assert.Contains("** Selected partition information **", lines);
        Assert.Contains("** File system information **", lines);
        Assert.DoesNotContain("** Container comment **", lines);
        Assert.Equal("*** End of file ***", lines[^1]);
    }

    private static async Task WaitUntil(Func<bool> condition)
    {
        for (var i = 0; i < 500 && !condition(); i++)
            await Task.Delay(10, TestContext.Current.CancellationToken);

        Assert.True(condition());
    }

    private sealed class SlowConfirmMessages(Task<bool> answer) : MessageServiceStub
    {
        public override Task<bool> ConfirmFormatAsync() => answer;

        public override Task<bool> ConfirmExportWithoutHashesAsync() => answer;

        public override Task ShowExportSucceededAsync(string path) => Task.CompletedTask;

        public override Task ShowErrorAsync(string operation, Exception exception) => Task.CompletedTask;
    }
}
