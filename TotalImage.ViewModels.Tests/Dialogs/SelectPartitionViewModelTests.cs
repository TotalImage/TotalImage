using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TotalImage.Partitions;
using TotalImage.ViewModels.Dialogs;
using TotalImage.ViewModels.Formatting;
using Xunit;
using static TotalImage.ViewModels.Tests.Dialogs.PartitionTestImages;

namespace TotalImage.ViewModels.Tests.Dialogs;

public sealed class SelectPartitionViewModelTests : IDisposable
{
    private readonly PartitionTestImages images = new();
    private readonly PartitionTestSettings settings = new();
    private readonly PartitionTestMessages messages = new();

    private SelectPartitionViewModel CreateMbr(bool validTimestamp = true)
        => new(images.CreateMbrImage(validTimestamp), settings, messages);

    [Fact]
    public void Mbr_ListsEveryPartition()
    {
        var vm = CreateMbr();

        Assert.Equal("Active", vm.FlagColumnHeader);
        Assert.Equal(
        [
            new PartitionRow(0, "FAT12", FatVolumeLabel, SizeUnit.Bytes.FormatSize(FatSectors * 512), "Yes", true, false),
            new PartitionRow(1, "RAW", "<N/A>", SizeUnit.Bytes.FormatSize(RawSectors * 512), "No", false, false),
        ], vm.Rows);
    }

    [Fact]
    public void Mbr_ShowsDiskInformation()
    {
        var vm = CreateMbr();
        ulong length = (1 + FatSectors + RawSectors) * 512;

        Assert.Equal("Master Boot Record", vm.PartitionTableName);
        Assert.Equal("Basic", vm.DiskType);
        Assert.Equal(SizeUnit.Bytes.FormatSize(length), vm.DiskTotalSize);
        Assert.Equal($"{length / 512:N0}", vm.DiskTotalSectors);
        Assert.Equal(SizeUnit.Bytes.FormatSize(length - 512), vm.DiskUsableSize);
        Assert.Equal("N/A", vm.DiskGuid);
        Assert.Equal("12:34:56", vm.DiskTimestamp);
        Assert.Equal("DEADBEEF", vm.DiskSerial);
    }

    [Fact]
    public void Mbr_WithoutSerialOrValidTimestamp_ShowsNotApplicable()
    {
        var vm = CreateMbr(validTimestamp: false);

        Assert.Equal("N/A", vm.DiskTimestamp);
        Assert.Equal("N/A", vm.DiskSerial);
    }

    [Fact]
    public void SizesUseTheConfiguredSizeUnit()
    {
        settings.Current.SizeUnit = SizeUnit.Binary;
        var vm = CreateMbr();
        vm.SelectedIndex = 0;

        Assert.Equal(SizeUnit.Binary.FormatSize(FatSectors * 512), vm.Rows[0].Size);
        Assert.Equal(SizeUnit.Binary.FormatSize((1 + FatSectors + RawSectors) * 512), vm.DiskTotalSize);
        Assert.Equal(SizeUnit.Binary.FormatSize(512), vm.PartitionStartOffset);
        Assert.Equal(SizeUnit.Binary.FormatSize((1 + FatSectors) * 512), vm.PartitionEndOffset);
    }

    [Fact]
    public void InitialState_NothingSelectedAndCannotAccept()
    {
        var vm = CreateMbr();

        Assert.Equal(-1, vm.SelectedIndex);
        Assert.False(vm.ReadOnly);
        Assert.False(vm.CanChangeReadOnly);
        Assert.False(vm.AcceptCommand.CanExecute(null));
        Assert.Null(vm.Answer);
        AssertPartitionInformationEmpty(vm);
    }

    [Fact]
    public void SelectingASupportedPartition_EnablesAcceptAndShowsItsInformation()
    {
        var vm = CreateMbr();
        var canExecuteChanged = 0;
        vm.AcceptCommand.CanExecuteChanged += (_, _) => canExecuteChanged++;
        var changed = new List<string?>();
        vm.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

        vm.SelectedIndex = 0;

        Assert.True(vm.AcceptCommand.CanExecute(null));
        Assert.Equal(1, canExecuteChanged);
        Assert.Equal("0x01 - FAT12", vm.PartitionType);
        Assert.Equal("N/A", vm.PartitionGuid);
        Assert.Equal("1234ABCD", vm.PartitionSerial);
        Assert.Equal($"{1:N0}", vm.PartitionFirstLba);
        Assert.Equal($"{FatSectors:N0}", vm.PartitionLastLba);
        Assert.Equal($"{FatSectors:N0}", vm.PartitionTotalSectors);
        Assert.Equal(SizeUnit.Bytes.FormatSize(512), vm.PartitionStartOffset);
        Assert.Equal(SizeUnit.Bytes.FormatSize((1 + FatSectors) * 512), vm.PartitionEndOffset);
        Assert.Contains(nameof(SelectPartitionViewModel.PartitionType), changed);
        Assert.Contains(nameof(SelectPartitionViewModel.PartitionSerial), changed);
    }

    [Fact]
    public void SelectingAnUnsupportedPartition_DisablesAcceptButShowsItsInformation()
    {
        var vm = CreateMbr();
        vm.SelectedIndex = 0;

        vm.SelectedIndex = 1;

        Assert.False(vm.AcceptCommand.CanExecute(null));
        Assert.Equal("0x07 - HPFS/NTFS/exFAT", vm.PartitionType);
        Assert.Equal("N/A", vm.PartitionSerial);
        Assert.Equal($"{1 + FatSectors:N0}", vm.PartitionFirstLba);
        Assert.Equal($"{FatSectors + RawSectors:N0}", vm.PartitionLastLba);
        Assert.Equal($"{RawSectors:N0}", vm.PartitionTotalSectors);
    }

    [Fact]
    public void ClearingTheSelection_DisablesAcceptAndClearsInformation()
    {
        var vm = CreateMbr();
        vm.SelectedIndex = 0;

        vm.SelectedIndex = -1;

        Assert.False(vm.AcceptCommand.CanExecute(null));
        AssertPartitionInformationEmpty(vm);
    }

    [Fact]
    public void AnOutOfRangeSelection_CannotBeAccepted()
    {
        var vm = CreateMbr();

        vm.SelectedIndex = 5;

        Assert.False(vm.AcceptCommand.CanExecute(null));
        AssertPartitionInformationEmpty(vm);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Accept_AnswersWithTheSelectedIndexAndReadOnlyFlag(bool readOnly)
    {
        var vm = CreateMbr();
        vm.SelectedIndex = 0;
        vm.ReadOnly = readOnly;

        vm.AcceptCommand.Execute(null);

        Assert.Equal(new PartitionSelection(0, readOnly), vm.Answer);
    }

    [Fact]
    public async Task OpenPartition_Supported_SelectsAndAccepts()
    {
        var vm = CreateMbr();

        await vm.OpenPartitionCommand.ExecuteAsync(0);

        Assert.Equal(0, vm.SelectedIndex);
        Assert.Equal(new PartitionSelection(0, false), vm.Answer);
        Assert.Equal(0, messages.PartitionNotSupportedShown);
    }

    [Fact]
    public async Task OpenPartition_Unsupported_SelectsAndExplainsWithoutAccepting()
    {
        var vm = CreateMbr();

        await vm.OpenPartitionCommand.ExecuteAsync(1);

        Assert.Equal(1, vm.SelectedIndex);
        Assert.Null(vm.Answer);
        Assert.Equal(1, messages.PartitionNotSupportedShown);
    }

    [Fact]
    public void Gpt_ShowsDiskInformationAndLegacyBootFlag()
    {
        var vm = new SelectPartitionViewModel(images.CreateGptImage(), settings, messages);

        Assert.Equal("GUID Partition Table", vm.PartitionTableName);
        Assert.Equal("Legacy boot", vm.FlagColumnHeader);
        var row = Assert.Single(vm.Rows);
        Assert.Equal(new PartitionRow(0, "RAW", "<N/A>", SizeUnit.Bytes.FormatSize(1024), "Yes", false, false), row);
        Assert.Equal(SizeUnit.Bytes.FormatSize(4 * 512 + 512), vm.DiskTotalSize);
        Assert.Equal($"{5:N0}", vm.DiskTotalSectors);
        Assert.Equal(SizeUnit.Bytes.FormatSize((200 + 1 - 34) * 512), vm.DiskUsableSize);
        Assert.Equal(GptDiskGuid.ToString(), vm.DiskGuid);
        Assert.Equal("N/A", vm.DiskTimestamp);
        Assert.Equal("N/A", vm.DiskSerial);
    }

    [Fact]
    public void Gpt_SelectingAPartitionShowsItsInformation()
    {
        var vm = new SelectPartitionViewModel(images.CreateGptImage(), settings, messages);

        vm.SelectedIndex = 0;

        Assert.False(vm.AcceptCommand.CanExecute(null));
        Assert.Equal(GptPartitionTable.GptPartitionTypes[System.Guid.Parse("C12A7328-F81F-11D2-BA4B-00A0C93EC93B")], vm.PartitionType);
        Assert.Equal(GptEntryId.ToString(), vm.PartitionGuid);
        Assert.Equal("N/A", vm.PartitionSerial);
        Assert.Equal($"{3:N0}", vm.PartitionFirstLba);
        Assert.Equal($"{4:N0}", vm.PartitionLastLba);
        Assert.Equal($"{2:N0}", vm.PartitionTotalSectors);
        Assert.Equal(SizeUnit.Bytes.FormatSize(1536), vm.PartitionStartOffset);
        Assert.Equal(SizeUnit.Bytes.FormatSize(2560), vm.PartitionEndOffset);
    }

    [Fact]
    public void Unpartitioned_ListsTheWholeImageWithoutFlag()
    {
        var vm = new SelectPartitionViewModel(images.CreateFloppyImage(), settings, messages);

        Assert.Equal("Unpartitioned", vm.PartitionTableName);
        Assert.Equal("Active", vm.FlagColumnHeader);
        var row = Assert.Single(vm.Rows);
        Assert.Null(row.Flag);
        Assert.True(row.CanLoad);
        Assert.Equal("", vm.DiskTotalSize);
        Assert.Equal("", vm.DiskGuid);

        vm.SelectedIndex = 0;

        Assert.True(vm.AcceptCommand.CanExecute(null));
        AssertPartitionInformationEmpty(vm);
    }

    public void Dispose() => images.Dispose();

    private static void AssertPartitionInformationEmpty(SelectPartitionViewModel vm)
    {
        Assert.Equal("", vm.PartitionType);
        Assert.Equal("", vm.PartitionGuid);
        Assert.Equal("", vm.PartitionSerial);
        Assert.Equal("", vm.PartitionFirstLba);
        Assert.Equal("", vm.PartitionLastLba);
        Assert.Equal("", vm.PartitionStartOffset);
        Assert.Equal("", vm.PartitionEndOffset);
        Assert.Equal("", vm.PartitionTotalSectors);
    }
}
