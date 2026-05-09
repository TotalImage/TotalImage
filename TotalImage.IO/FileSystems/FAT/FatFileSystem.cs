using System;
using System.IO;
using System.Text;
using TotalImage.Changes;
using TotalImage.FileSystems.BPB;

namespace TotalImage.FileSystems.FAT
{
/// <summary>
/// Base class for FAT12, FAT16, and FAT32 file systems.
/// </summary>
public abstract class FatFileSystem : FileSystem
{
        /// <summary>
        /// The BIOS parameter block that describes the volume layout.
        /// </summary>
        protected readonly BiosParameterBlock _bpb;

        /// <inheritdoc />
        public override Directory RootDirectory { get; }

        /// <inheritdoc />
        public override long TotalSize => TotalSectors * _bpb.BytesPerLogicalSector;

        /// <inheritdoc />
        public override long TotalFreeSpace => TotalFreeClusters * BytesPerCluster;

        /// <inheritdoc />
        public override long AllocationUnitSize => BytesPerCluster;

        /// <summary>
        /// The BIOS Parameter Block for this FAT file system.
        /// </summary>
        public BiosParameterBlock BiosParameterBlock => _bpb;

        /// <summary>
        /// The number of free clusters in this volume.
        /// </summary>
        public virtual uint TotalFreeClusters
        {
            get
            {
                var clusters = 0u;

                for (var i = 0u; i < ClusterCount; i++)
                {
                    if (MainFat[i + 2] == 0) clusters++;
                }

                return clusters;
            }
        }

        /// <summary>
        /// The address of the first sector of the data area.
        /// </summary>
        public uint DataAreaFirstSector
        {
            get
            {
                var fatOffset = (uint)_bpb.ReservedLogicalSectors;
                var fatSize = (uint)_bpb.NumberOfFATs * _bpb.LogicalSectorsPerFAT;
                var rootDirSize = (uint)_bpb.RootDirectoryEntries * 32 / _bpb.BytesPerLogicalSector;
                return fatOffset + fatSize + RootDirectorySectors;
            }
        }

        /// <summary>
        /// The total number of logical sectors in this volume.
        /// </summary>
        public uint TotalSectors
            => _bpb.TotalLogicalSectors;

        /// <summary>
        /// The number of reserved logical sectors in this volume.
        /// </summary>
        public uint ReservedSectors
            => _bpb.ReservedLogicalSectors;

        /// <summary>
        /// The number of sectors used for all File allocation tables.
        /// </summary>
        public uint ClusterMapsSectors
            => (uint)_bpb.NumberOfFATs * _bpb.LogicalSectorsPerFAT;

        /// <summary>
        /// The number of sectors used for the root directory.
        /// </summary>
        public uint RootDirectorySectors
            => (uint)_bpb.RootDirectoryEntries * 32 / _bpb.BytesPerLogicalSector;

        /// <summary>
        /// The number of sectors used for the data area.
        /// </summary>
        public uint DataAreaSectors
            => TotalSectors - ReservedSectors - ClusterMapsSectors - RootDirectorySectors;

        /// <summary>
        /// The size of a cluster in bytes.
        /// </summary>
        public uint BytesPerCluster
            => (uint)_bpb.LogicalSectorsPerCluster * _bpb.BytesPerLogicalSector;

        /// <summary>
        /// The size of a File allocation table in bytes.
        /// </summary>
        public uint BytesPerClusterMap
            => (uint)_bpb.LogicalSectorsPerFAT * _bpb.BytesPerLogicalSector;

        /// <summary>
        /// The number of clusters in this volume.
        /// </summary>
        public uint ClusterCount
            => DataAreaSectors / _bpb.LogicalSectorsPerCluster;

        /// <summary>
        /// File allocation tables. Usually 2.
        /// </summary>
        public abstract FileAllocationTable[] Fats { get; }

        /// <summary>
        /// The "primary" FAT (usually the first one).
        /// </summary>
        public FileAllocationTable MainFat => Fats[0];

        /// <summary>
        /// The volume label. Returns the root directory volume label if it exists, otherwise the BPB volume label if it exists, otherwise a place holder.
        /// </summary>
        public override string VolumeLabel
        {
            get
            {
                var rootLabel = RootDirectoryVolumeLabel;
                if (!string.IsNullOrWhiteSpace(rootLabel))
                    return rootLabel;
                var bpbLabel = BpbVolumeLabel;
                if (!string.IsNullOrWhiteSpace(bpbLabel) && !bpbLabel.Equals("NO NAME", StringComparison.OrdinalIgnoreCase))
                    return bpbLabel;
                return "<N/A>";
            }

            set => throw new NotImplementedException();
        }

        /// <summary>
        /// The volume label stored in the BIOS Parameter Block. This field is rarely used in place of the root directory volume label.
        /// </summary>
        public string? BpbVolumeLabel
        {
            get => BiosParameterBlock.Version == BiosParameterBlockVersion.Dos40 ? ((ExtendedBiosParameterBlock)BiosParameterBlock).VolumeLabel : null;
            set => throw new NotImplementedException();
        }

        /// <summary>
        /// The volume label stored as a file in the root directory. This field is usually used by most software.
        /// </summary>
        public string? RootDirectoryVolumeLabel
        {
            get
            {
                foreach (var (entry, _) in DirectoryEntry.EnumerateRootDirectory(this))
                {
                    if (entry.Attributes.HasFlag(FatAttributes.VolumeId) && !entry.Attributes.HasFlag(FatAttributes.LongName))
                    {
                        return Encoding.ASCII.GetString(entry.FileNameBytes).TrimEnd();
                    }
                }
                return null;
            }
            set => throw new NotImplementedException();
        }

        /// <summary>
        /// Stages a volume label change on this file system.
        /// </summary>
        /// <param name="newLabel">The new volume label (max 11 characters, ASCII).</param>
        public void EnqueueSetVolumeLabel(string newLabel)
        {
            var container = OwningContainer
                ?? throw new InvalidOperationException("This file system is not associated with a container.");

            // Find this filesystem's partition index in the owning container
            int partitionIndex = 0;
            var partitions = container.PartitionTable.Partitions;
            for (int i = 0; i < partitions.Count; i++)
            {
                if (partitions[i].FileSystem == this)
                {
                    partitionIndex = i;
                    break;
                }
            }

            container.PendingChanges.Add(new VolumeLabelChange(partitionIndex, newLabel ?? string.Empty));
        }

        /// <summary>
        /// Writes a new volume label to the root directory volume label entry (or creates one if absent),
        /// and updates the BPB volume label field when the BPB version supports it.
        /// Called during commit by <see cref="Changes.ChangeApplicator"/>.
        /// </summary>
        internal void WriteVolumeLabel(string newLabel)
        {
            var stream = GetStream();
            string padded = newLabel.ToUpperInvariant().PadRight(11).Substring(0, 11);
            var labelBytes = System.Text.Encoding.ASCII.GetBytes(padded);

            // Locate the root directory volume label entry and update/create it.
            long rootStart = (ReservedSectors + ClusterMapsSectors) * _bpb.BytesPerLogicalSector;

            if (_bpb is BPB.Fat32BiosParameterBlock fat32bpb)
            {
                // FAT32: volume label is in root cluster chain
                var clusters = MainFat.GetClusterChain(fat32bpb.RootDirectoryCluster);
                long dataBase = DataAreaFirstSector * _bpb.BytesPerLogicalSector;
                long freeSlot = -1;

                foreach (var cl in clusters)
                {
                    long clStart = dataBase + (long)(cl - 2) * BytesPerCluster;
                    for (long pos = clStart; pos < clStart + BytesPerCluster; pos += 32)
                    {
                        stream.Position = pos;
                        var buf = new byte[32];
                        stream.Read(buf, 0, 32);
                        if (buf[0] == 0x00) { if (freeSlot < 0) freeSlot = pos; break; }
                        if (buf[11] == (byte)FatAttributes.VolumeId && buf[11] != (byte)FatAttributes.LongName)
                        {
                            stream.Position = pos;
                            stream.Write(labelBytes, 0, 11);
                            return;
                        }
                        if ((buf[0] == 0xE5) && freeSlot < 0) freeSlot = pos;
                    }
                }

                if (freeSlot >= 0)
                {
                    stream.Position = freeSlot;
                    stream.Write(new byte[32]);
                    stream.Position = freeSlot;
                    stream.Write(labelBytes, 0, 11);
                    stream.Position = freeSlot + 11;
                    stream.WriteByte((byte)FatAttributes.VolumeId);
                }
            }
            else
            {
                long rootEnd = rootStart + (long)_bpb.RootDirectoryEntries * 32;
                long freeSlot = -1;

                for (long pos = rootStart; pos < rootEnd; pos += 32)
                {
                    stream.Position = pos;
                    var buf = new byte[32];
                    stream.Read(buf, 0, 32);
                    if (buf[0] == 0x00) { if (freeSlot < 0) freeSlot = pos; break; }
                    if (buf[11] == (byte)FatAttributes.VolumeId && buf[11] != (byte)FatAttributes.LongName)
                    {
                        stream.Position = pos;
                        stream.Write(labelBytes, 0, 11);
                        return;
                    }
                    if (buf[0] == 0xE5 && freeSlot < 0) freeSlot = pos;
                }

                if (freeSlot >= 0)
                {
                    stream.Position = freeSlot;
                    stream.Write(new byte[32]);
                    stream.Position = freeSlot;
                    stream.Write(labelBytes, 0, 11);
                    stream.Position = freeSlot + 11;
                    stream.WriteByte((byte)FatAttributes.VolumeId);
                }
            }
        }

        /// <summary>
        /// Writes a cluster map value to all FAT copies simultaneously.
        /// </summary>
        /// <param name="index">The cluster index to update.</param>
        /// <param name="value">The value to write.</param>
        internal void SetClusterAllFats(uint index, uint value)
        {
            foreach (var fat in Fats)
                fat[index] = value;
        }

        /// <summary>
        /// Finds and allocates the next free cluster, chains it to <paramref name="previousCluster"/>
        /// if provided, and marks it as end-of-chain. Returns the newly allocated cluster number.
        /// Throws <see cref="IOException"/> if the volume is full.
        /// </summary>
        /// <param name="previousCluster">
        /// The cluster that should point to the new cluster. Pass <see langword="null"/> to allocate
        /// a standalone cluster (e.g. the first cluster of a new file or directory).
        /// </param>
        internal uint AllocateCluster(uint? previousCluster = null)
        {
            // Cluster numbers start at 2; 0 and 1 are reserved.
            for (uint i = 2; i < ClusterCount + 2; i++)
            {
                if (MainFat[i] == 0)
                {
                    // Mark new cluster as end-of-chain in all FATs.
                    SetClusterAllFats(i, MainFat.EndOfChainMarker);

                    // Chain the previous cluster to this one.
                    if (previousCluster.HasValue)
                        SetClusterAllFats(previousCluster.Value, i);

                    return i;
                }
            }
            throw new IOException("No free clusters available on the volume.");
        }

        /// <summary>
        /// Marks every cluster in the chain starting at <paramref name="firstCluster"/> as free (0).
        /// </summary>
        internal void FreeClusterChain(uint firstCluster)
        {
            var chain = MainFat.GetClusterChain(firstCluster);
            foreach (var cluster in chain)
                SetClusterAllFats(cluster, 0);
        }

        /// <summary>
        /// Creates a FAT file system from a stream and BIOS parameter block.
        /// </summary>
        /// <param name="stream">The stream containing the file system.</param>
        /// <param name="bpb">The BIOS parameter block for the volume.</param>
        protected FatFileSystem(Stream stream, BiosParameterBlock bpb) : base(stream)
        {
            _bpb = bpb;
            RootDirectory = new FatDirectory(this);
        }        
    }
}
