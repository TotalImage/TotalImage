using System.IO;

namespace TotalImage.Containers
{
    /// <summary>
    /// Class for handling an ISO container, typically containing the ISO9660 or UDF file systems
    /// </summary>
    public class IsoContainer : Container
    {
        /// <inheritdoc />
        public override Stream Content => containerStream;

        /// <inheritdoc />
        public override string DisplayName => "ISO disc image";

        /// <inheritdoc />
        public IsoContainer(string path, bool memoryMapping) : base(path, memoryMapping) { }

        /// <inheritdoc />
        public override bool IsReadOnly => true;
    }
}
