using System;
using System.IO;
using System.Threading.Tasks;
using TotalImage.FileSystems;

namespace TotalImage.UI.ViewModel
{
    public abstract class FileSystemObjectViewModel<T> : IFileSystemObjectViewModel
        where T : FileSystemObject
    {
        protected readonly T _fsObject;

        public string Name
            => _fsObject.Name == "\0" || _fsObject.Name == ""
                ? "\\"
                : _fsObject.Name;

        public string FullName
            => _fsObject.FullName == "\0" || _fsObject.Name == ""
                ? "\\"
        : _fsObject.FullName;

        public ulong Length => _fsObject.Length;

        public ulong LengthOnDisk => _fsObject.LengthOnDisk;

        public DateTime? LastWriteTime => _fsObject.LastWriteTime;

        public string Attributes { get; }

        public bool IsHidden => _fsObject.Attributes.HasFlag(FileAttributes.Hidden);

        public abstract Task Extract(string destination);

        protected FileSystemObjectViewModel(T obj)
        {
            _fsObject = obj;

            char[] attr = new char[]
            {
                _fsObject.Attributes.HasFlag(FileAttributes.Directory) ? 'D' : '-',
                _fsObject.Attributes.HasFlag(FileAttributes.Archive) ? 'A' : '-',
                _fsObject.Attributes.HasFlag(FileAttributes.System) ? 'S' : '-',
                _fsObject.Attributes.HasFlag(FileAttributes.Hidden) ? 'H' : '-',
                _fsObject.Attributes.HasFlag(FileAttributes.ReadOnly) ? 'R' : '-'
            };

            Attributes = new string(attr);
        }
    }
}
