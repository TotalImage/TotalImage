using System.IO;
using TotalImage.Partitions;

namespace TotalImage.FileSystems
{
    public abstract class FileSystem
    {
        protected readonly Stream _stream;

        protected FileSystem(Stream containerStream)
        {
            _stream = containerStream;
        }

        public abstract string Format { get; }
        public abstract string VolumeLabel { get; set; }

        public abstract Directory RootDirectory { get; }

        public abstract long AvailableFreeSpace { get; }
        public abstract long TotalFreeSpace { get; }
        public abstract long TotalSize { get; }
    }
}