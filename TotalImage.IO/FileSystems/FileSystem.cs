namespace TotalImage.FileSystems
{
    public abstract class FileSystem
    {
        protected FileSystem() { }

        public abstract string Format { get; }
        public abstract string VolumeLabel { get; set; }

        public abstract Directory RootDirectory { get; }

        public abstract long AvailableFreeSpace { get; }
        public abstract long TotalFreeSpace { get; }
        public abstract long TotalSize { get; }
    }
}