using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace TotalImage.ViewModels;

public sealed class SettingsFormViewModel : ObservableObject
{
    private Settings.SettingsModel Model { get; } = Settings.CurrentSettings;

    public SettingsFormViewModel()
    {
        OkCommand = new RelayCommand(Ok);
        ClearRecentImageListCommand = new RelayCommand(ClearRecentImageList);
        ClearTemporaryFolderCommand = new RelayCommand(ClearTemporaryFolder);
        OpenFileAssociationsCommand = new RelayCommand(OpenFileAssociations);
    }

    public ICommand OkCommand { get; }

    private void Ok()
    {
        Settings.CurrentSettings = Model;
        Settings.Save();
    }

    public bool ConfirmOverwriteExtraction
    {
        get => Model.ConfirmOverwriteExtraction;
        set => SetProperty(ConfirmOverwriteExtraction, value, x => Model.ConfirmOverwriteExtraction = x);
    }

    public bool ConfirmDeletion
    {
        get => Model.ConfirmDeletion;
        set => SetProperty(ConfirmDeletion, value, x => Model.ConfirmDeletion = x);
    }

    public bool ConfirmInjection
    {
        get => Model.ConfirmInjection;
        set => SetProperty(ConfirmInjection, value, x => Model.ConfirmInjection = x);
    }

    public bool AutoIncrementFilename
    {
        get => Model.AutoIncrementFilename;
        set => SetProperty(AutoIncrementFilename, value, x => Model.AutoIncrementFilename = x);
    }

    public long MemoryMappingThreshold
    {
        get => Model.MemoryMappingThreshold;
        set => SetProperty(MemoryMappingThreshold, value, x => Model.MemoryMappingThreshold = x);
    }

    public ICommand ClearRecentImageListCommand { get; }

    private void ClearRecentImageList() =>
        Settings.ClearRecentImages();

    public ICommand ClearTemporaryFolderCommand { get; }

    private void ClearTemporaryFolder()
    {
        var path = Path.Combine(Path.GetTempPath(), "TotalImage");

        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }

        Directory.CreateDirectory(path);
    }

    public View FilesView
    {
        get => Model.FilesView;
        set => SetProperty(FilesView, value, x => Model.FilesView = x);
    }

    public SizeUnit SizeUnit
    {
        get => Model.SizeUnit;
        set => SetProperty(SizeUnit, value, x => Model.SizeUnit = x);
    }

    public int FilesSortingColumn
    {
        get => Model.FilesSortingColumn;
        set => SetProperty(FilesSortingColumn, value, x => Model.FilesSortingColumn = x);
    }

    public SortOrder FilesSortOrder
    {
        get => Model.FilesSortOrder;
        set => SetProperty(FilesSortOrder, value, x => Model.FilesSortOrder = x);
    }

    public bool ShowCommandBar
    {
        get => Model.ShowCommandBar;
        set => SetProperty(ShowCommandBar, value, x => Model.ShowCommandBar = x);
    }

    public bool ShowStatusBar
    {
        get => Model.ShowStatusBar;
        set => SetProperty(ShowStatusBar, value, x => Model.ShowStatusBar = x);
    }

    public bool ShowDirectoryTree
    {
        get => Model.ShowDirectoryTree;
        set => SetProperty(ShowDirectoryTree, value, x => Model.ShowDirectoryTree = x);
    }

    public bool ShowDeletedItems
    {
        get => Model.ShowDeletedItems;
        set => SetProperty(ShowDeletedItems, value, x => Model.ShowDeletedItems = x);
    }

    public bool ShowHiddenItems
    {
        get => Model.ShowHiddenItems;
        set => SetProperty(ShowHiddenItems, value, x => Model.ShowHiddenItems = x);
    }

    public bool FileListShowDirSize
    {
        get => Model.FileListShowDirSize;
        set => SetProperty(FileListShowDirSize, value, x => Model.FileListShowDirSize = x);
    }

    public bool ExtractAlwaysAsk
    {
        get => Model.ExtractAlwaysAsk;
        set => SetProperty(ExtractAlwaysAsk, value, x => Model.ExtractAlwaysAsk = x);
    }

    public string DefaultExtractPath
    {
        get => Model.DefaultExtractPath;
        set => SetProperty(DefaultExtractPath, value, x => Model.DefaultExtractPath = x);
    }

    public DirectoryExtractionMode DefaultDirectoryExtractionMode
    {
        get => Model.DefaultDirectoryExtractionMode;
        set => SetProperty(DefaultDirectoryExtractionMode, value, x => Model.DefaultDirectoryExtractionMode = x);
    }

    public bool ExtractSkipDirectories
    {
        get => Model.DefaultDirectoryExtractionMode == DirectoryExtractionMode.Skip;
        set => SetProperty(ExtractSkipDirectories, value, x =>
        {
            if (x) Model.DefaultDirectoryExtractionMode = DirectoryExtractionMode.Skip;
        });
    }

    public bool ExtractMergeDirectories
    {
        get => Model.DefaultDirectoryExtractionMode == DirectoryExtractionMode.Merge;
        set => SetProperty(ExtractMergeDirectories, value, x =>
        {
            if (x) Model.DefaultDirectoryExtractionMode = DirectoryExtractionMode.Merge;
        });
    }

    public bool ExtractPreserveDirectories
    {
        get => Model.DefaultDirectoryExtractionMode == DirectoryExtractionMode.Preserve;
        set => SetProperty(ExtractPreserveDirectories, value, x =>
        {
            if (x) Model.DefaultDirectoryExtractionMode = DirectoryExtractionMode.Preserve;
        });
    }

    public bool OpenFolderAfterExtract
    {
        get => Model.OpenFolderAfterExtract;
        set => SetProperty(OpenFolderAfterExtract, value, x => Model.OpenFolderAfterExtract = x);
    }

    public bool ExtractPreserveDates
    {
        get => Model.ExtractPreserveDates;
        set => SetProperty(ExtractPreserveDates, value, x => Model.ExtractPreserveDates = x);
    }

    public bool ExtractPreserveAttributes
    {
        get => Model.ExtractPreserveAttributes;
        set => SetProperty(ExtractPreserveAttributes, value, x => Model.ExtractPreserveAttributes = x);
    }

    public ICommand OpenFileAssociationsCommand { get; }

    private void OpenFileAssociations()
    {
        /* While the old command opens the Default apps page of Win10 Settings
         * just fine, we still do it "the right way" on Win10 just in case the
         * old one ever stops working. */
        var process = new ProcessStartInfo
        {
            FileName = Environment.OSVersion.Version.Major > 6 ? "ms-settings:defaultapps" : "control",
            Arguments = Environment.OSVersion.Version.Major == 6 ? "/name Microsoft.DefaultPrograms /page pageDefaultProgram" : "",
            UseShellExecute = true
        };

        Process.Start(process);
    }

    public bool QueryShellForFileTypeInfo
    {
        get => Model.QueryShellForFileTypeInfo;
        set => SetProperty(QueryShellForFileTypeInfo, value, x => Model.QueryShellForFileTypeInfo = x);
    }
}
