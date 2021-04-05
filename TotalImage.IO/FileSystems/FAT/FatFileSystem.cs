using System;
using System.IO;
using System.Text;
using TotalImage.FileSystems.BPB;

namespace TotalImage.FileSystems.FAT
{
    public abstract class FatFileSystem : FileSystem
    {
        protected FatFileSystem(Stream stream) : base(stream) { }

        public abstract BiosParameterBlock BiosParameterBlock { get; }
        public abstract uint DataAreaFirstSector { get; }
        public abstract uint BytesPerCluster { get; }
        public abstract uint ClusterCount { get; }

        /// <summary>
        /// Retrieves the number of the next cluster in a cluster chain.
        /// </summary>
        /// <param name="index">Current cluster</param>
        /// <param name="fat">Specifies which copy of the FAT should be used</param>
        /// <returns>The next cluster number if any, otherwise <c>null</c>.</returns>
        public abstract uint? GetNextCluster(uint index, int fat = 0);

        public abstract ClusterMap[] ClusterMaps { get; }
        public ClusterMap MainClusterMap => ClusterMaps[0];

        public abstract class ClusterMap
        {
            public abstract uint this[uint index] { get; set; }
        }

        public override string VolumeLabel { get => RootDirectoryVolumeLabel ?? BpbVolumeLabel ?? "NO NAME"; set => throw new NotImplementedException(); }

        public string? BpbVolumeLabel
        {
            get => BiosParameterBlock.Version == BiosParameterBlockVersion.Dos40 ? ((BiosParameterBlock40)BiosParameterBlock).VolumeLabel : null;
            set => throw new NotImplementedException();
        }

        public string? RootDirectoryVolumeLabel
        {
            get
            {
                foreach(var entry in DirectoryEntry.ReadRootDirectory(this))
                {
                    if (entry.attr.HasFlag(FatAttributes.VolumeId) && !entry.attr.HasFlag(FatAttributes.LongName))
                    {
                        return Helper.TrimFileName(Encoding.ASCII.GetString(entry.name));
                    }
                }
                return null;
            }
            set => throw new NotImplementedException();
        }
    }
}