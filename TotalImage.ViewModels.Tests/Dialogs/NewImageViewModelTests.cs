using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TotalImage.DiskGeometries;
using TotalImage.FileSystems.BPB;
using TotalImage.ViewModels.Dialogs;
using TotalImage.ViewModels.Services;
using Xunit;
using Name = TotalImage.DiskGeometries.FloppyGeometry.FriendlyName;

namespace TotalImage.ViewModels.Tests.Dialogs;

/// <summary>
/// An <see cref="IMessageService"/> that answers every yes/no question with a fixed answer and
/// records which questions were asked. Built on <see cref="DispatchProxy"/> so it keeps working
/// when methods are added to the interface.
/// </summary>
public class FakeMessageService : DispatchProxy
{
    public bool Answer { get; set; }
    public List<string> Asked { get; } = [];

    public static FakeMessageService Create(bool answer, out IMessageService service)
    {
        service = DispatchProxy.Create<IMessageService, FakeMessageService>();
        var fake = (FakeMessageService)service;
        fake.Answer = answer;
        return fake;
    }

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        Asked.Add(targetMethod!.Name);
        return targetMethod.ReturnType == typeof(Task<bool>) ? Task.FromResult(Answer) : throw new NotSupportedException(targetMethod.Name);
    }
}

public class NewImageViewModelTests
{
    private readonly FakeMessageService messages;
    private readonly IMessageService messageService;
    private readonly List<NewImageAdvancedViewModel> shownAdvanced = [];

    public NewImageViewModelTests()
    {
        messages = FakeMessageService.Create(true, out messageService);
    }

    private NewImageViewModel Create(Action<NewImageAdvancedViewModel>? editAdvanced = null)
        => new(messageService, advanced =>
        {
            shownAdvanced.Add(advanced);
            editAdvanced?.Invoke(advanced);
            return Task.CompletedTask;
        });

    private static NamedOption<Name> Option(Name name) => NewImageViewModel.FloppyGeometries.Single(o => o.Value == name);

    private static NamedOption<BiosParameterBlockVersion> Version(BiosParameterBlockVersion version)
        => NewImageViewModel.BpbVersions.Single(o => o.Value == version);

    [Fact]
    public void Defaults()
    {
        var vm = Create();

        Assert.True(vm.IsFloppyDisk);
        Assert.False(vm.IsHardDisk);
        Assert.Equal([NewImageViewModel.PlainSectorImage], vm.ContainerFormats);
        Assert.Equal(NewImageViewModel.PlainSectorImage, vm.ContainerFormat);
        Assert.Equal(Name.HighDensity1440k, vm.SelectedGeometry.Value);
        Assert.Same(FloppyGeometry.KnownGeometries[Name.HighDensity1440k], vm.Geometry);
        Assert.Equal("", vm.VolumeLabel);
        Assert.True(vm.WriteBpb);
        Assert.True(vm.CanChangeWriteBpb);
        Assert.True(vm.CanChooseBpbVersion);
        Assert.Equal(BiosParameterBlockVersion.Dos40, vm.BpbVersion.Value);
        Assert.Equal("MSDOS5.0", vm.OemId);
        Assert.Equal("FAT12", vm.FileSystemType);
        Assert.Matches("^[0-9A-F]{8}$", vm.SerialNumber);
        Assert.Equal(1, vm.DiskSize);
        Assert.True(vm.WritePartitionTable);
        Assert.True(vm.CanChoosePartitionTable);
        Assert.Equal(NewImageViewModel.MasterBootRecord, vm.PartitionTable);
        Assert.True(vm.AcceptCommand.CanExecute(null));
        Assert.True(vm.CreateCommand.CanExecute(null));
        Assert.Null(vm.Answer);
    }

    [Fact]
    public void FloppyGeometries_AreAllKnownGeometriesInOrderThenCustom()
    {
        var geometries = NewImageViewModel.FloppyGeometries;

        Assert.Equal(Enum.GetValues<Name>(), geometries.Select(o => o.Value));
        Assert.Equal(Name.Custom, geometries[^1].Value);
        Assert.Equal("1440 KiB", Option(Name.HighDensity1440k).Name);
        Assert.Equal("Custom...", Option(Name.Custom).ToString());
        Assert.All(geometries.SkipLast(1), o => Assert.True(FloppyGeometry.KnownGeometries.ContainsKey(o.Value)));
    }

    [Fact]
    public void BpbVersions_AreNamedLikeDos()
    {
        Assert.Equal(["DOS 2.0", "DOS 3.4", "DOS 4.0+"], NewImageViewModel.BpbVersions.Select(o => o.Name));
        Assert.Equal([BiosParameterBlockVersion.Dos20, BiosParameterBlockVersion.Dos34, BiosParameterBlockVersion.Dos40],
            NewImageViewModel.BpbVersions.Select(o => o.Value));
    }

    [Fact]
    public void HardDisk_OffersVhdAndResetsTheContainerFormat()
    {
        var vm = Create();
        var changed = new List<string?>();
        vm.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

        vm.IsHardDisk = true;

        Assert.False(vm.IsFloppyDisk);
        Assert.Equal([NewImageViewModel.PlainSectorImage, NewImageViewModel.MicrosoftVhd], vm.ContainerFormats);
        Assert.Contains(nameof(NewImageViewModel.IsHardDisk), changed);
        Assert.Contains(nameof(NewImageViewModel.ContainerFormats), changed);

        vm.ContainerFormat = NewImageViewModel.MicrosoftVhd;
        vm.IsFloppyDisk = true;

        Assert.Equal([NewImageViewModel.PlainSectorImage], vm.ContainerFormats);
        Assert.Equal(NewImageViewModel.PlainSectorImage, vm.ContainerFormat);
    }

    [Fact]
    public void PartitionTable_CanOnlyBeChosenWhenWritten()
    {
        var vm = Create();
        var changed = new List<string?>();
        vm.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

        vm.WritePartitionTable = false;

        Assert.False(vm.CanChoosePartitionTable);
        Assert.Contains(nameof(NewImageViewModel.CanChoosePartitionTable), changed);

        vm.WritePartitionTable = true;

        Assert.True(vm.CanChoosePartitionTable);
    }

    [Fact]
    public void BpbVersion_CanOnlyBeChosenWhenWritten()
    {
        var vm = Create();
        var changed = new List<string?>();
        vm.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

        vm.WriteBpb = false;

        Assert.False(vm.CanChooseBpbVersion);
        Assert.True(vm.CanChangeWriteBpb);
        Assert.Contains(nameof(NewImageViewModel.CanChooseBpbVersion), changed);
    }

    [Theory]
    [MemberData(nameof(KnownNames))]
    public void ChoosingAKnownGeometry_SetsIt(Name name)
    {
        var vm = Create();

        vm.SelectedGeometry = Option(name);

        Assert.Same(FloppyGeometry.KnownGeometries[name], vm.Geometry);
    }

    public static TheoryData<Name> KnownNames() => new(FloppyGeometry.KnownGeometries.Keys);

    [Fact]
    public void ChoosingCustom_KeepsTheCurrentGeometry()
    {
        var vm = Create();
        vm.SelectedGeometry = Option(Name.DoubleDensity720k);

        vm.SelectedGeometry = Option(Name.Custom);

        Assert.Same(FloppyGeometry.KnownGeometries[Name.DoubleDensity720k], vm.Geometry);
    }

    [Fact]
    public void ChoosingAGeometry_TurnsTheBpbBackOn()
    {
        var vm = Create();
        vm.WriteBpb = false;

        vm.SelectedGeometry = Option(Name.DoubleDensity720k);

        Assert.True(vm.WriteBpb);
        Assert.True(vm.CanChooseBpbVersion);
    }

    [Fact]
    public void Acorn_HasNoBootSector()
    {
        var vm = Create();
        var changed = new List<string?>();
        vm.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

        vm.SelectedGeometry = Option(Name.Acorn800k);

        Assert.False(vm.WriteBpb);
        Assert.False(vm.CanChangeWriteBpb);
        Assert.False(vm.CanChooseBpbVersion);
        Assert.Equal(BiosParameterBlockVersion.Dos20, vm.BpbVersion.Value);
        Assert.Contains(nameof(NewImageViewModel.CanChangeWriteBpb), changed);
        Assert.Contains(nameof(NewImageViewModel.CanChooseBpbVersion), changed);
    }

    [Fact]
    public void LeavingAcorn_TurnsTheBpbBackOnWithTheVersionItSet()
    {
        var vm = Create();
        vm.SelectedGeometry = Option(Name.Acorn800k);

        vm.SelectedGeometry = Option(Name.HighDensity1440k);

        Assert.True(vm.WriteBpb);
        Assert.True(vm.CanChangeWriteBpb);
        Assert.True(vm.CanChooseBpbVersion);
        Assert.Equal(BiosParameterBlockVersion.Dos20, vm.BpbVersion.Value);
    }

    [Theory]
    [InlineData(Name.DMF1024, "MSDMF3.2")]
    [InlineData(Name.DMF2048, "MSDMF3.2")]
    [InlineData(Name.HighDensity1440k, "MSDOS5.0")]
    [InlineData(Name.Acorn800k, "MSDOS5.0")]
    [InlineData(Name.Custom, "MSDOS5.0")]
    public void OemId_FollowsTheGeometry(Name name, string oemId)
    {
        var vm = Create();
        vm.SelectedGeometry = Option(Name.DMF1024);

        vm.SelectedGeometry = Option(name);

        Assert.Equal(oemId, vm.OemId);
    }

    [Fact]
    public void AdvancedOptions_StartFromTheCurrentState()
    {
        var vm = Create();
        vm.SelectedGeometry = Option(Name.DoubleDensity360k);
        vm.BpbVersion = Version(BiosParameterBlockVersion.Dos34);

        vm.ShowAdvancedOptionsCommand.Execute(null);

        var advanced = Assert.Single(shownAdvanced);
        Assert.Equal(FloppyGeometry.KnownGeometries[Name.DoubleDensity360k], advanced.ToGeometry());
        Assert.Equal(vm.OemId, advanced.OemId);
        Assert.Equal(vm.SerialNumber, advanced.SerialNumber);
        Assert.Equal(vm.FileSystemType, advanced.FileSystemType);
        Assert.True(advanced.IsOemIdEnabled);
        Assert.True(advanced.IsSerialNumberEnabled);
        Assert.False(advanced.IsFileSystemTypeEnabled);
    }

    [Fact]
    public void AdvancedOptions_WithoutBpb_DisableSerialAndFileSystemType()
    {
        var vm = Create();
        vm.WriteBpb = false;

        vm.ShowAdvancedOptionsCommand.Execute(null);

        var advanced = Assert.Single(shownAdvanced);
        Assert.True(advanced.IsOemIdEnabled);
        Assert.False(advanced.IsSerialNumberEnabled);
        Assert.False(advanced.IsFileSystemTypeEnabled);
    }

    [Fact]
    public void AdvancedOptions_ForAcorn_AreEmptyAndApplyEmptyFields()
    {
        var vm = Create();
        vm.SelectedGeometry = Option(Name.Acorn800k);

        vm.ShowAdvancedOptionsCommand.Execute(null);

        var advanced = Assert.Single(shownAdvanced);
        Assert.False(advanced.IsOemIdEnabled);
        Assert.Equal("", advanced.OemId);
        Assert.Equal("", advanced.SerialNumber);
        Assert.Equal("", advanced.FileSystemType);
        Assert.Equal(Name.Acorn800k, vm.SelectedGeometry.Value);
        Assert.Equal("", vm.OemId);
        Assert.Equal("", vm.SerialNumber);
        Assert.Equal("", vm.FileSystemType);
    }

    [Fact]
    public void AdvancedOptions_WithTheSameGeometry_KeepTheKnownGeometryAndApplyTheBootSectorFields()
    {
        var vm = Create(advanced =>
        {
            advanced.OemId = "IBM  3.3";
            advanced.SerialNumber = "12345678";
            advanced.FileSystemType = "FAT";
        });

        vm.ShowAdvancedOptionsCommand.Execute(null);

        Assert.Equal(Name.HighDensity1440k, vm.SelectedGeometry.Value);
        Assert.Same(FloppyGeometry.KnownGeometries[Name.HighDensity1440k], vm.Geometry);
        Assert.Equal("IBM  3.3", vm.OemId);
        Assert.Equal("12345678", vm.SerialNumber);
        Assert.Equal("FAT", vm.FileSystemType);
    }

    [Fact]
    public void AdvancedOptions_WithAnotherGeometry_SwitchToCustom()
    {
        var vm = Create(advanced =>
        {
            advanced.Tracks = 82;
            advanced.OemId = "MYOEM";
        });
        vm.WriteBpb = false;

        vm.ShowAdvancedOptionsCommand.Execute(null);

        Assert.Equal(Name.Custom, vm.SelectedGeometry.Value);
        Assert.Equal(82, vm.Geometry.Tracks);
        Assert.Equal(18, vm.Geometry.SPT);
        // Switching to Custom resets the OEM ID and BPB like any other geometry; the advanced OEM ID wins.
        Assert.Equal("MYOEM", vm.OemId);
        Assert.True(vm.WriteBpb);
    }

    [Fact]
    public void AdvancedOptions_WhenCustomIsAlreadyChosen_ApplyTheGeometry()
    {
        var vm = Create(advanced => advanced.Tracks++);
        vm.SelectedGeometry = Option(Name.Custom);

        vm.ShowAdvancedOptionsCommand.Execute(null);
        vm.ShowAdvancedOptionsCommand.Execute(null);

        Assert.Equal(Name.Custom, vm.SelectedGeometry.Value);
        Assert.Equal(82, vm.Geometry.Tracks);
        Assert.Equal(18, vm.Geometry.SPT);
    }

    [Fact]
    public void Create_WithBpb_AnswersWithoutAsking()
    {
        var vm = Create();
        vm.SelectedGeometry = Option(Name.DoubleDensity720k);
        vm.BpbVersion = Version(BiosParameterBlockVersion.Dos34);
        vm.VolumeLabel = "MYDISK";

        vm.CreateCommand.Execute(null);

        Assert.Empty(messages.Asked);
        Assert.Equal(new NewImageOptions(FloppyGeometry.KnownGeometries[Name.DoubleDensity720k], true, BiosParameterBlockVersion.Dos34,
            "MSDOS5.0", vm.SerialNumber, "FAT12", "MYDISK"), vm.Answer);
    }

    [Fact]
    public void Create_FloppyWithoutBpb_AsksAndAnswersWhenConfirmed()
    {
        var vm = Create();
        vm.WriteBpb = false;

        vm.CreateCommand.Execute(null);

        Assert.Equal([nameof(IMessageService.ConfirmCreateWithoutBpbAsync)], messages.Asked);
        Assert.NotNull(vm.Answer);
        Assert.False(vm.Answer.WriteBpb);
    }

    [Fact]
    public void Create_FloppyWithoutBpb_DoesNotAnswerWhenRefused()
    {
        messages.Answer = false;
        var vm = Create();
        vm.WriteBpb = false;

        vm.CreateCommand.Execute(null);

        Assert.Equal([nameof(IMessageService.ConfirmCreateWithoutBpbAsync)], messages.Asked);
        Assert.Null(vm.Answer);
    }

    [Fact]
    public void Create_HardDisk_DoesNotAsk()
    {
        var vm = Create();
        vm.WriteBpb = false;
        vm.IsHardDisk = true;

        vm.CreateCommand.Execute(null);

        Assert.Empty(messages.Asked);
        Assert.NotNull(vm.Answer);
    }
}

public class NewImageAdvancedViewModelTests
{
    private static readonly FloppyGeometry HighDensity = FloppyGeometry.KnownGeometries[Name.HighDensity1440k];

    private static NewImageAdvancedViewModel Create(FloppyGeometry geometry, bool hasBootSector = true, bool writeBpb = true,
        BiosParameterBlockVersion version = BiosParameterBlockVersion.Dos40)
        => new(geometry, hasBootSector, writeBpb, version, "MSDOS5.0", "ABCD1234", "FAT12");

    [Fact]
    public void StartsWithTheGeometry()
    {
        var vm = Create(HighDensity);

        Assert.Equal([1, 2], vm.SideCounts);
        Assert.Equal(2, vm.Sides);
        Assert.Equal(80, vm.Tracks);
        Assert.Equal(18, vm.SectorsPerTrack);
        Assert.Equal(512, vm.BytesPerSector);
        Assert.Equal(0xF0, vm.MediaDescriptor);
        Assert.Equal(1, vm.SectorsPerCluster);
        Assert.Equal(2, vm.NumberOfFats);
        Assert.Equal(9, vm.SectorsPerFat);
        Assert.Equal(224, vm.RootDirectoryEntries);
        Assert.Equal(1, vm.ReservedSectors);
        Assert.Equal(2880, vm.TotalSectors);
        Assert.Equal("MSDOS5.0", vm.OemId);
        Assert.Equal("ABCD1234", vm.SerialNumber);
        Assert.Equal("FAT12", vm.FileSystemType);
    }

    [Theory]
    [MemberData(nameof(KnownGeometries))]
    public void ToGeometry_RoundTrips(FloppyGeometry geometry)
    {
        Assert.Equal(geometry, Create(geometry).ToGeometry());
    }

    public static TheoryData<FloppyGeometry> KnownGeometries() => new(FloppyGeometry.KnownGeometries.Values);

    [Fact]
    public void ToGeometry_UsesTheEditedValues()
    {
        var vm = Create(HighDensity);
        vm.BytesPerSector = 1024;
        vm.Sides = 1;
        vm.RootDirectoryEntries = 112;

        var geometry = vm.ToGeometry();

        Assert.Equal(3, geometry.BPS);
        Assert.Equal(1, geometry.Sides);
        Assert.Equal(112, geometry.RootDirectoryEntries);
    }

    [Theory]
    [InlineData(nameof(NewImageAdvancedViewModel.Tracks))]
    [InlineData(nameof(NewImageAdvancedViewModel.SectorsPerTrack))]
    [InlineData(nameof(NewImageAdvancedViewModel.Sides))]
    public void TotalSectors_FollowsTheGeometry(string property)
    {
        var vm = Create(HighDensity);
        var changed = new List<string?>();
        vm.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

        typeof(NewImageAdvancedViewModel).GetProperty(property)!.SetValue(vm, 1);

        Assert.Contains(nameof(NewImageAdvancedViewModel.TotalSectors), changed);
        Assert.Equal(property switch
        {
            nameof(NewImageAdvancedViewModel.Tracks) => 1 * 18 * 2,
            nameof(NewImageAdvancedViewModel.SectorsPerTrack) => 80 * 1 * 2,
            _ => 80 * 18 * 1,
        }, vm.TotalSectors);
    }

    [Theory]
    [InlineData(true, true, BiosParameterBlockVersion.Dos40, true, true, true)]
    [InlineData(true, true, BiosParameterBlockVersion.Dos34, true, true, false)]
    [InlineData(true, true, BiosParameterBlockVersion.Dos20, true, false, false)]
    [InlineData(true, false, BiosParameterBlockVersion.Dos40, true, false, false)]
    [InlineData(false, false, BiosParameterBlockVersion.Dos20, false, false, false)]
    [InlineData(false, true, BiosParameterBlockVersion.Dos40, false, false, false)]
    public void BootSectorFields_AreEnabledByTheBpb(bool hasBootSector, bool writeBpb, BiosParameterBlockVersion version,
        bool oemId, bool serialNumber, bool fileSystemType)
    {
        var vm = Create(HighDensity, hasBootSector, writeBpb, version);

        Assert.Equal(oemId, vm.IsOemIdEnabled);
        Assert.Equal(serialNumber, vm.IsSerialNumberEnabled);
        Assert.Equal(fileSystemType, vm.IsFileSystemTypeEnabled);
    }

    [Fact]
    public void WithoutABootSector_TheBootSectorFieldsAreEmpty()
    {
        var vm = Create(HighDensity, hasBootSector: false);

        Assert.Equal("", vm.OemId);
        Assert.Equal("", vm.SerialNumber);
        Assert.Equal("", vm.FileSystemType);
    }
}
