using System;
using System.IO;
using System.Text;
using TotalImage.FileSystems.BPB;
using TotalImage.FileSystems.FAT;
using TotalImage.Partitions;
using File = System.IO.File;

namespace TotalImage.Containers
{
    /// <summary>
    /// Class for handling a container of raw sectors
    /// </summary>
    public class RawContainer : Container
    {
        /// <inheritdoc />
        public override Stream Content => containerStream;

        /// <inheritdoc />
        public override string DisplayName => "Raw sector image";

        /// <inheritdoc />
        public RawContainer(string path) : base(path)
        {
        }

        private RawContainer(MemoryStream stream) : base(stream)
        {
        }

        /// <summary>
        /// Create a new raw image with the provided parameters
        /// </summary>
        /// <param name="bpb">The BIOS Parameter Block to use within the image</param>
        /// <param name="tracks">The number of tracks within the image</param>
        /// <param name="writeBPB">Specifies whether the BIOS Parameter Block should be written to the image</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="bpb"/> is null</exception>
        public static RawContainer CreateImage(BiosParameterBlock bpb, byte tracks, bool writeBPB)
        {
            if (bpb == null)
                throw new ArgumentNullException(nameof(bpb), "BPB cannot be null!");

            uint imageSize = (uint)(bpb.BytesPerLogicalSector * bpb.PhysicalSectorsPerTrack * bpb.NumberOfHeads * tracks);
            var imageBytes = new byte[imageSize];

            //TODO: At this point we need to consider writeBPB value...
            var stream = new MemoryStream(imageBytes, true);
            Fat12.Create(stream, bpb);

            return new RawContainer(stream);
        }

        /* Extracts the file system object from the image to the specified path
         * This is a very basic solution for now. Needs to be decided if attributes and other DateTime values should be preserved
         * in the extracted file as well. */
        /// <summary>
        /// Extract a file system object to the specified path
        /// </summary>
        /// <param name="entry">The directory entry to extract</param>
        /// <param name="path">The path to extract the entry to</param>
        public void ExtractFile(DirectoryEntry entry, string path)
        {
            if (entry.attr.HasFlag(FatAttributes.Subdirectory)) return;

            var name = Encoding.ASCII.GetString(entry.name);
            var cluster = ((uint?)entry.fstClusHI << 16) | entry.fstClusLO;

            using (var fs = new FileStream(path + Path.DirectorySeparatorChar + name, FileMode.Append, FileAccess.Write))
            {
                do
                {
                    byte[] clusterBytes = ((Fat12)PartitionTable.Partitions[0].FileSystem).ReadCluster(cluster.Value);
                    fs.Write(clusterBytes, 0, clusterBytes.Length);
                    cluster = ((Fat12)PartitionTable.Partitions[0].FileSystem).GetNextCluster(cluster.Value);
                }
                while (cluster.HasValue);

                fs.SetLength(entry.fileSize); //Remove the trailing unused bytes from the last cluster
            }

            var date = Helper.FatToDateTime(entry.wrtDate, entry.wrtTime);
            if (date.HasValue)
            {
                File.SetLastWriteTime(Path.Combine(path, name), date.Value);
            }
        }
    }
}
