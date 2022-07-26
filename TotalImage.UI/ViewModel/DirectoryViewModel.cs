using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using TotalImage.UI.Converters;
using System;

namespace TotalImage.UI.ViewModel
{
    public class DirectoryViewModel : FileSystemObjectViewModel<FileSystems.Directory>
    {
        public IEnumerable<DirectoryViewModel> Directories
            => _fsObject.EnumerateDirectories(true, true)
                .Select(e => new DirectoryViewModel(e));
        public int DirectoryCount
            => _fsObject.EnumerateDirectories(true, true).Count();

        public IEnumerable<IFileSystemObjectViewModel> Objects
            => _fsObject.EnumerateFileSystemObjects(true, true)
                .Select(e => e is FileSystems.Directory dir
                    ? new DirectoryViewModel(dir)
                    : new FileViewModel((FileSystems.File)e) as IFileSystemObjectViewModel);

        public IEnumerable<FileViewModel> Files
            => _fsObject.EnumerateFiles(true, true)
                .Select(e => new FileViewModel(e));

        public int FileCount
            => _fsObject.EnumerateFiles(true, true).Count();

        public string FooterLabel
            => Files.Any()
                ? $"{SizeConverter.Convert(Files.Sum(e => (long)e.Length))} in {Files.Count()} item(s)"
                : "";

        // Additional properties for UI parity with FilePropertiesWindow
        public DateTime? CreationTime => _fsObject.CreationTime;
        public DateTime? LastAccessTime => _fsObject.LastAccessTime;
        public string? ParentDirectoryName => _fsObject.Parent?.FullName;
        public string TypeDescription => "File folder";
        public ulong Size => _fsObject.GetSize(true, false);
        public ulong SizeOnDisk => _fsObject.GetSize(true, true);
        public bool IsReadOnly => _fsObject.Attributes.HasFlag(FileAttributes.ReadOnly);
        public bool IsSystem => _fsObject.Attributes.HasFlag(FileAttributes.System);
        public bool IsArchive => _fsObject.Attributes.HasFlag(FileAttributes.Archive);
        // IsHidden is already exposed by the base class

        public override async Task Extract(string destination)
        {
            string path = Path.Combine(destination, _fsObject.Name);
            Directory.CreateDirectory(path);

            foreach (var item in Objects)
            {
                if (item != null)
                {
                    await item.Extract(path);
                }
            }

            if (_fsObject.CreationTime.HasValue)
            {
                Directory.SetCreationTime(path, _fsObject.CreationTime.Value);
            }

            if (_fsObject.LastAccessTime.HasValue)
            {
                Directory.SetLastAccessTime(path, _fsObject.LastAccessTime.Value);
            }

            if (_fsObject.LastWriteTime.HasValue)
            {
                Directory.SetLastWriteTime(path, _fsObject.LastWriteTime.Value);
            }
        }

        public DirectoryViewModel(FileSystems.Directory obj) : base(obj)
        {
        }
    }
}
