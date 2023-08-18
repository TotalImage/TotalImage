using System.IO;

namespace TotalImage.Containers.BinCue
{
    /// <summary>
    /// Class for handling BIN/CUE images of optical discs, commonly used for copy-protected and mixed-mode discs
    /// </summary>
    public class BinCueContainer : Container
    {
        /// <inheritdoc />
        public override Stream Content => containerStream;

        /// <inheritdoc />
        public override string DisplayName => "BIN/CUE container";

        /// <summary>
        /// The type of this CD track
        /// </summary>
        public CdTrackType TrackType { get; set; }

        /// <inheritdoc />
        public BinCueContainer(string path, bool memoryMapping) : base(path, memoryMapping)
        {
        }
    }
}
