using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
