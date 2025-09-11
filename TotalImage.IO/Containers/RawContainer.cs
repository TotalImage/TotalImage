using System;
using System.IO;
using TotalImage.FileSystems.BPB;
using TotalImage.FileSystems.FAT;

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
        public override bool IsReadOnly => false;

        /// <inheritdoc />
        public RawContainer(string path, bool memoryMapping) : base(path, memoryMapping) { }

        /// <inheritdoc />
        private RawContainer(MemoryStream stream) : base(stream) { }

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
            byte[] imageBytes = new byte[imageSize];

            //TODO: At this point we need to consider writeBPB value...
            MemoryStream stream = new MemoryStream(imageBytes, true);
            Fat12FileSystem.Create(stream, bpb, writeBPB);

            return new RawContainer(stream);
        }
    }
}
