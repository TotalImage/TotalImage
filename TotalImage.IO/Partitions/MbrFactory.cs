using System.IO;

namespace TotalImage.Partitions
{
    public class MbrFactory : IPartitionTableFactory
    {
        public PartitionTable? TryLoadPartitionTable(Stream stream)
        {
            return null;
        }
    }
}
