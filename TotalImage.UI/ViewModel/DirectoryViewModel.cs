using System.Collections.Generic;
using System.Linq;
using TotalImage.UI.Converters;

namespace TotalImage.UI.ViewModel
{
    public class DirectoryViewModel : FileSystemObjectViewModel<FileSystems.Directory>
    {
        public IEnumerable<DirectoryViewModel> Directories
            => _fsObject.EnumerateDirectories(true, true)
                .Select(e => new DirectoryViewModel(e));

        public IEnumerable<IFileSystemObjectViewModel> Objects
            => _fsObject.EnumerateFileSystemObjects(true, true)
                .Select(e => e is FileSystems.Directory dir
                    ? new DirectoryViewModel(dir)
                    : new FileViewModel((FileSystems.File)e) as IFileSystemObjectViewModel);

        public IEnumerable<FileViewModel> Files
            => _fsObject.EnumerateFiles(true, true)
                .Select(e => new FileViewModel(e));

        public string FooterLabel
            => Files.Any()
                ? $"{SizeConverter.Convert(Files.Sum(e => (long)e.Length))} in {Files.Count()} item(s)"
                : "";

        public DirectoryViewModel(FileSystems.Directory obj) : base(obj)
        {
        }
    }
}
