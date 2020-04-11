namespace TotalImage.FileSystems
{
    public abstract class FileSystem
    {
        protected FileSystem() { }

        public string Format { get; }
        public string VolumeLabel { get; set; }

        public Directory RootDirectory { get; }

        public long AvailableFreeSpace { get; }
        public long TotalFreeSpace { get; }
        public long TotalSize { get; }
    }
}