using System.IO;
using System.Text;
using TotalImage.Containers;

namespace TotalImage.Partitions
{
    /// <summary>
    /// Creates Microsoft Flash partition table instances.
    /// </summary>
    public class MsFlashFactory : IPartitionTableFactory
    {
        /// <inheritdoc />
        public PartitionTable? TryLoadPartitionTable(Container container)
        {
            container.Content.Seek(0x800, SeekOrigin.Begin);

            byte[] signatureBytes = new byte[8];
            container.Content.Read(signatureBytes);

            if (Encoding.ASCII.GetString(signatureBytes) != "MSFLSH50")
            {
                return null;
            }

            return new MsFlashPartitionTable(container);
        }
    }
}
