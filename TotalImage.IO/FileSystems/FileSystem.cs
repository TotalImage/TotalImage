using System.IO;
using TotalImage.Partitions;

namespace TotalImage.FileSystems
{
    public abstract class FileSystem
    {
        protected FileSystem() { }

        public FileSystem(Stream containerStream, PartitionEntry partition) { }

        public abstract string Format { get; }
        public abstract string VolumeLabel { get; set; }

        public abstract Directory RootDirectory { get; }

        public abstract long AvailableFreeSpace { get; }
        public abstract long TotalFreeSpace { get; }
        public abstract long TotalSize { get; }
    }
}