using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TotalImage.FileSystems.UDF
{
    /// <summary>
    /// Representation of a Universal Disk Format file system.
    /// </summary>
    public class UdfFileSystem : FileSystem
    {
        private readonly Dictionary<ushort, UdfPartitionDescriptorInfo> _partitionsByNumber = new();
        private readonly Dictionary<ushort, UdfPartitionMapInfo> _partitionMapsByReference = new();
        private readonly UdfLongAllocationDescriptor _fileSetDescriptorLocation;

        /// <inheritdoc />
        public override string DisplayName => "UDF";

        /// <inheritdoc />
        public override string VolumeLabel { get; set; }

        /// <inheritdoc />
        public override Directory RootDirectory { get; }

        /// <inheritdoc />
        public override long TotalFreeSpace => 0;

        /// <inheritdoc />
        public override long TotalSize { get; }

        /// <inheritdoc />
        public override long AllocationUnitSize { get; }

        /// <inheritdoc />
        public override bool SupportsSubdirectories => true;

        /// <inheritdoc />
        public override bool IsReadOnly => true;

        /// <summary>
        /// Create a UDF file system.
        /// </summary>
        /// <param name="containerStream">The underlying stream</param>
        public UdfFileSystem(Stream containerStream) : base(containerStream)
        {
            byte[] anchor = ReadAnchorDescriptor();

            uint mainDescriptorSequenceLength = UdfUtilities.ReadUInt32(anchor, 16);
            uint mainDescriptorSequenceBlock = UdfUtilities.ReadUInt32(anchor, 20);
            uint reserveDescriptorSequenceLength = UdfUtilities.ReadUInt32(anchor, 24);
            uint reserveDescriptorSequenceBlock = UdfUtilities.ReadUInt32(anchor, 28);
            if (mainDescriptorSequenceLength == 0 && reserveDescriptorSequenceLength == 0)
            {
                throw new InvalidDataException("Volume descriptor sequence is empty");
            }

            byte[]? primaryVolumeDescriptor;
            byte[]? logicalVolumeDescriptor;
            bool descriptorsLoaded = TryReadDescriptorSet(
                mainDescriptorSequenceBlock,
                mainDescriptorSequenceLength,
                out primaryVolumeDescriptor,
                out logicalVolumeDescriptor);

            if (!descriptorsLoaded && reserveDescriptorSequenceLength > 0)
            {
                _partitionsByNumber.Clear();
                _partitionMapsByReference.Clear();

                descriptorsLoaded = TryReadDescriptorSet(
                    reserveDescriptorSequenceBlock,
                    reserveDescriptorSequenceLength,
                    out primaryVolumeDescriptor,
                    out logicalVolumeDescriptor);
            }

            if (!descriptorsLoaded || logicalVolumeDescriptor == null)
            {
                throw new InvalidDataException("No logical volume descriptor");
            }

            AllocationUnitSize = UdfUtilities.ReadUInt32(logicalVolumeDescriptor, 212);
            if (AllocationUnitSize <= 0)
            {
                throw new InvalidDataException("Invalid logical block size");
            }

            ReadPartitionMaps(logicalVolumeDescriptor);
            ResolveSpecialPartitionMaps();

            _fileSetDescriptorLocation = ReadLongAllocationDescriptor(logicalVolumeDescriptor, 248);
            byte[] fileSetDescriptor = ReadBlock(_fileSetDescriptorLocation.LogicalBlockNumber, _fileSetDescriptorLocation.PartitionReferenceNumber);
            if (UdfUtilities.ReadTagIdentifier(fileSetDescriptor) != UdfUtilities.FileSetDescriptor)
            {
                throw new InvalidDataException("No file set descriptor");
            }

            UdfLongAllocationDescriptor rootDirectoryLocation = ReadLongAllocationDescriptor(fileSetDescriptor, 400);
            UdfFileEntryInfo rootEntry = ReadFileEntry(rootDirectoryLocation);

            VolumeLabel = UdfUtilities.DecodeDString(logicalVolumeDescriptor.AsSpan(84, 128));
            if (string.IsNullOrWhiteSpace(VolumeLabel) && primaryVolumeDescriptor != null)
            {
                VolumeLabel = UdfUtilities.DecodeDString(primaryVolumeDescriptor.AsSpan(24, 32));
            }

            TotalSize = _partitionMapsByReference.Values
                .Where(partition => partition.Kind == UdfPartitionMapKind.Physical)
                .Sum(partition => (long)partition.LengthInBlocks) * AllocationUnitSize;
            RootDirectory = new UdfDirectory(string.Empty, rootEntry, this, null, false);
        }

        private bool TryReadDescriptorSet(uint startBlock, uint sequenceLength, out byte[]? primaryVolumeDescriptor, out byte[]? logicalVolumeDescriptor)
        {
            primaryVolumeDescriptor = null;
            logicalVolumeDescriptor = null;

            if (sequenceLength == 0)
            {
                return false;
            }

            var pendingSequences = new Queue<(uint StartBlock, uint Length)>();
            var visitedSequences = new HashSet<(uint StartBlock, uint Length)>();
            pendingSequences.Enqueue((startBlock, sequenceLength));

            while (pendingSequences.Count > 0)
            {
                (uint sequenceStartBlock, uint currentSequenceLength) = pendingSequences.Dequeue();
                if (currentSequenceLength == 0 || !visitedSequences.Add((sequenceStartBlock, currentSequenceLength)))
                {
                    continue;
                }

                int descriptorBlockCount = checked((int)Math.Ceiling(currentSequenceLength / (double)UdfUtilities.VolumeStructureDescriptorSize));
                for (uint block = sequenceStartBlock; block < sequenceStartBlock + descriptorBlockCount; block++)
                {
                    byte[] descriptor = ReadFixedBlock(block, UdfUtilities.VolumeStructureDescriptorSize);
                    ushort tagIdentifier = UdfUtilities.ReadTagIdentifier(descriptor);

                    switch (tagIdentifier)
                    {
                        case UdfUtilities.PrimaryVolumeDescriptor:
                            primaryVolumeDescriptor ??= descriptor;
                            break;
                        case UdfUtilities.PartitionDescriptor:
                            {
                                ushort partitionNumber = UdfUtilities.ReadUInt16(descriptor, 22);
                                uint startingBlock = UdfUtilities.ReadUInt32(descriptor, 188);
                                uint lengthInBlocks = UdfUtilities.ReadUInt32(descriptor, 192);
                                _partitionsByNumber[partitionNumber] = new UdfPartitionDescriptorInfo(partitionNumber, startingBlock, lengthInBlocks);
                                break;
                            }
                        case UdfUtilities.LogicalVolumeDescriptor:
                            logicalVolumeDescriptor ??= descriptor;
                            break;
                        case UdfUtilities.VolumeDescriptorPointer:
                            {
                                uint nextSequenceLength = UdfUtilities.ReadUInt32(descriptor, 20);
                                uint nextSequenceBlock = UdfUtilities.ReadUInt32(descriptor, 24);
                                if (nextSequenceLength > 0)
                                {
                                    pendingSequences.Enqueue((nextSequenceBlock, nextSequenceLength));
                                }
                                break;
                            }
                        case UdfUtilities.TerminatingDescriptor:
                            block = sequenceStartBlock + (uint)descriptorBlockCount;
                            break;
                    }
                }
            }

            return logicalVolumeDescriptor != null;
        }

        internal UdfFileEntryInfo ReadFileEntry(UdfLongAllocationDescriptor icb)
        {
            byte[] descriptor = ReadBlock(icb.LogicalBlockNumber, icb.PartitionReferenceNumber);
            ushort tagIdentifier = UdfUtilities.ReadTagIdentifier(descriptor);
            bool isExtendedFileEntry = tagIdentifier == UdfUtilities.ExtendedFileEntry;
            if (tagIdentifier != UdfUtilities.FileEntry && !isExtendedFileEntry)
            {
                throw new InvalidDataException("Invalid file entry descriptor");
            }

            int headerSize = isExtendedFileEntry ? UdfUtilities.ExtendedFileEntrySize : UdfUtilities.FileEntrySize;
            byte fileType = descriptor[27];
            ushort allocationDescriptorType = (ushort)(UdfUtilities.ReadUInt16(descriptor, 34) & 0x0007);
            ulong informationLength = UdfUtilities.ReadUInt64(descriptor, 56);
            DateTime? creationTime = isExtendedFileEntry ? UdfUtilities.ReadTimestamp(descriptor, 104) : null;
            DateTime? lastAccessTime = UdfUtilities.ReadTimestamp(descriptor, 72);
            DateTime? lastWriteTime = UdfUtilities.ReadTimestamp(descriptor, 84);
            uint lengthOfExtendedAttributes = UdfUtilities.ReadUInt32(descriptor, isExtendedFileEntry ? 208 : 168);
            uint lengthOfAllocationDescriptors = UdfUtilities.ReadUInt32(descriptor, isExtendedFileEntry ? 212 : 172);
            int allocationStart = checked(headerSize + (int)lengthOfExtendedAttributes);

            if (allocationStart < 0 || allocationStart + lengthOfAllocationDescriptors > descriptor.Length)
            {
                throw new InvalidDataException("Invalid allocation descriptor length");
            }

            byte[] allocationDescriptors = descriptor
                .AsSpan(allocationStart, checked((int)lengthOfAllocationDescriptors))
                .ToArray();

            return new UdfFileEntryInfo(
                fileType,
                allocationDescriptorType,
                icb.PartitionReferenceNumber,
                informationLength,
                lengthOfAllocationDescriptors,
                allocationDescriptors,
                creationTime,
                lastAccessTime,
                lastWriteTime);
        }

        internal IEnumerable<UdfFileIdentifierDescriptorInfo> EnumerateDirectoryEntries(UdfFileEntryInfo entry)
        {
            byte[] contents = ReadContent(entry);

            int offset = 0;
            while (offset + UdfUtilities.FileIdentifierDescriptorHeaderSize <= contents.Length)
            {
                ReadOnlySpan<byte> remaining = contents.AsSpan(offset);
                if (remaining.Length == 0 || remaining[0] == 0)
                {
                    break;
                }

                if (UdfUtilities.ReadTagIdentifier(remaining) != UdfUtilities.FileIdentifierDescriptor)
                {
                    break;
                }

                byte fileCharacteristics = remaining[18];
                byte lengthOfFileIdentifier = remaining[19];
                ushort lengthOfImplementationUse = UdfUtilities.ReadUInt16(remaining, 36);
                int descriptorLength = UdfUtilities.Align(UdfUtilities.FileIdentifierDescriptorHeaderSize + lengthOfImplementationUse + lengthOfFileIdentifier, 4);
                if (descriptorLength <= 0 || descriptorLength > remaining.Length)
                {
                    throw new InvalidDataException("Invalid file identifier descriptor length");
                }

                int nameOffset = UdfUtilities.FileIdentifierDescriptorHeaderSize + lengthOfImplementationUse;
                ReadOnlySpan<byte> nameBytes = remaining.Slice(nameOffset, lengthOfFileIdentifier);
                string name = UdfUtilities.DecodeCompressedUnicode(nameBytes);

                yield return new UdfFileIdentifierDescriptorInfo(
                    name,
                    fileCharacteristics,
                    ReadLongAllocationDescriptor(remaining, 20));

                offset += descriptorLength;
            }
        }

        internal byte[] ReadContent(UdfFileEntryInfo entry)
        {
            if (entry.InformationLength == 0)
            {
                return Array.Empty<byte>();
            }

            if (entry.InformationLength > int.MaxValue)
            {
                throw new NotSupportedException("Buffered reads are not supported for UDF entries larger than 2 GiB. Use OpenContentStream instead.");
            }

            using Stream content = OpenContentStream(entry);
            byte[] buffer = new byte[(int)entry.InformationLength];
            content.ReadExactly(buffer);
            return buffer;
        }

        internal Stream OpenContentStream(UdfFileEntryInfo entry)
        {
            return new UdfFileContentStream(this, entry);
        }

        internal int ReadContent(UdfFileEntryInfo entry, long position, Span<byte> buffer)
        {
            if (position < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            long entryLength = checked((long)Math.Min(entry.InformationLength, (ulong)long.MaxValue));
            if (buffer.Length == 0 || position >= entryLength)
            {
                return 0;
            }

            if (entry.AllocationDescriptorType == UdfUtilities.IcbAllocationDescriptorInIcb)
            {
                int available = Math.Max(0, Math.Min(entry.AllocationDescriptors.Length, (int)Math.Min(entry.InformationLength, (ulong)int.MaxValue)) - (int)position);
                int bytesToCopy = Math.Min(buffer.Length, available);
                if (bytesToCopy > 0)
                {
                    entry.AllocationDescriptors.AsSpan((int)position, bytesToCopy).CopyTo(buffer);
                }
                return bytesToCopy;
            }

            int requested = (int)Math.Min((long)buffer.Length, entryLength - position);
            int totalRead = 0;
            long extentStart = 0;

            foreach (UdfContentExtent extent in EnumerateExtents(entry))
            {
                uint extentType = extent.RawLength & UdfUtilities.ExtentTypeMask;
                long extentLength = extent.RawLength & UdfUtilities.ExtentLengthMask;
                if (extentLength == 0)
                {
                    continue;
                }

                if (position >= extentStart + extentLength)
                {
                    extentStart += extentLength;
                    continue;
                }

                long offsetWithinExtent = Math.Max(0, position - extentStart);
                if (extentType == UdfUtilities.ExtentNotRecordedAllocated || extentType == UdfUtilities.ExtentNotRecordedNotAllocated)
                {
                    int bytesToZero = (int)Math.Min(extentLength - offsetWithinExtent, requested - totalRead);
                    buffer.Slice(totalRead, bytesToZero).Clear();
                    totalRead += bytesToZero;
                    extentStart += extentLength;
                    continue;
                }

                if (extentType != UdfUtilities.ExtentRecordedAllocated)
                {
                    throw new NotSupportedException($"Unsupported UDF extent type 0x{extentType:X8}");
                }

                while (offsetWithinExtent < extentLength && totalRead < requested)
                {
                    long blockIndex = offsetWithinExtent / AllocationUnitSize;
                    int blockOffset = (int)(offsetWithinExtent % AllocationUnitSize);
                    byte[] block = ReadBlock(extent.LogicalBlockNumber + (uint)blockIndex, extent.PartitionReferenceNumber);
                    int bytesAvailableInExtent = (int)Math.Min(block.Length - blockOffset, extentLength - offsetWithinExtent);
                    int bytesToCopy = Math.Min(bytesAvailableInExtent, requested - totalRead);
                    block.AsSpan(blockOffset, bytesToCopy).CopyTo(buffer[totalRead..]);
                    totalRead += bytesToCopy;
                    offsetWithinExtent += bytesToCopy;
                }

                if (totalRead == requested)
                {
                    break;
                }

                extentStart += extentLength;
            }

            return totalRead;
        }

        internal byte[] ReadBlock(uint logicalBlockNumber, ushort partitionReferenceNumber)
        {
            uint physicalBlock = ResolvePhysicalBlock(logicalBlockNumber, partitionReferenceNumber);
            if (physicalBlock == uint.MaxValue)
            {
                throw new InvalidDataException($"Unable to resolve partition reference {partitionReferenceNumber}");
            }

            return ReadFixedBlock(physicalBlock, checked((int)AllocationUnitSize));
        }

        private void WriteExtent(MemoryStream output, uint rawLength, uint logicalBlockNumber, ushort partitionReferenceNumber, ulong informationLength)
        {
            uint extentType = rawLength & UdfUtilities.ExtentTypeMask;
            uint length = rawLength & UdfUtilities.ExtentLengthMask;
            if (length == 0)
            {
                return;
            }

            if (extentType != UdfUtilities.ExtentRecordedAllocated)
            {
                throw new NotSupportedException("Only recorded allocated UDF extents are supported");
            }

            long remaining = (long)Math.Min((ulong)long.MaxValue, informationLength) - output.Length;
            if (remaining <= 0)
            {
                return;
            }

            int bytesRemaining = checked((int)Math.Min(length, (uint)Math.Min(int.MaxValue, remaining)));
            uint currentLogicalBlock = logicalBlockNumber;

            while (bytesRemaining > 0)
            {
                byte[] block = ReadBlock(currentLogicalBlock, partitionReferenceNumber);
                int bytesToCopy = Math.Min(block.Length, bytesRemaining);
                output.Write(block, 0, bytesToCopy);
                bytesRemaining -= bytesToCopy;
                currentLogicalBlock++;
            }
        }

        private IEnumerable<UdfContentExtent> EnumerateExtents(UdfFileEntryInfo entry)
        {
            foreach (UdfContentExtent extent in EnumerateExtents(entry.AllocationDescriptorType, entry.PartitionReferenceNumber, entry.AllocationDescriptors))
            {
                yield return extent;
            }
        }

        private IEnumerable<UdfContentExtent> EnumerateExtents(ushort descriptorType, ushort defaultPartitionReferenceNumber, byte[] allocationDescriptors)
        {
            if (descriptorType == UdfUtilities.IcbAllocationDescriptorShort)
            {
                foreach (ReadOnlyMemory<byte> chunk in EnumerateAllocationDescriptorChunks(allocationDescriptors, 8, defaultPartitionReferenceNumber))
                {
                    byte[] descriptors = chunk.ToArray();
                    for (int offset = 0; offset + 8 <= descriptors.Length; offset += 8)
                    {
                        UdfShortAllocationDescriptor descriptor = ReadShortAllocationDescriptor(descriptors, offset);
                        yield return new UdfContentExtent(descriptor.Length, descriptor.LogicalBlockNumber, defaultPartitionReferenceNumber);
                    }
                }

                yield break;
            }

            if (descriptorType == UdfUtilities.IcbAllocationDescriptorLong)
            {
                foreach (ReadOnlyMemory<byte> chunk in EnumerateAllocationDescriptorChunks(allocationDescriptors, 16, defaultPartitionReferenceNumber))
                {
                    byte[] descriptors = chunk.ToArray();
                    for (int offset = 0; offset + 16 <= descriptors.Length; offset += 16)
                    {
                        UdfLongAllocationDescriptor descriptor = ReadLongAllocationDescriptor(descriptors, offset);
                        yield return new UdfContentExtent(descriptor.Length, descriptor.LogicalBlockNumber, descriptor.PartitionReferenceNumber);
                    }
                }

                yield break;
            }

            if (descriptorType == UdfUtilities.IcbAllocationDescriptorExtended)
            {
                foreach (ReadOnlyMemory<byte> chunk in EnumerateAllocationDescriptorChunks(allocationDescriptors, 20, defaultPartitionReferenceNumber))
                {
                    byte[] descriptors = chunk.ToArray();
                    for (int offset = 0; offset + 20 <= descriptors.Length; offset += 20)
                    {
                        UdfExtendedAllocationDescriptor descriptor = ReadExtendedAllocationDescriptor(descriptors, offset);
                        yield return new UdfContentExtent(descriptor.Length, descriptor.LogicalBlockNumber, descriptor.PartitionReferenceNumber);
                    }
                }

                yield break;
            }

            throw new NotSupportedException($"Unsupported UDF allocation descriptor type {descriptorType}");
        }

        private IEnumerable<ReadOnlyMemory<byte>> EnumerateAllocationDescriptorChunks(byte[] allocationDescriptors, int descriptorSize, ushort defaultPartitionReferenceNumber)
        {
            byte[] currentChunk = allocationDescriptors;
            var visited = new HashSet<(uint LogicalBlock, ushort PartitionReference)>();

            while (currentChunk.Length > 0)
            {
                int nextExtentOffset = FindNextAllocationExtentOffset(currentChunk, descriptorSize);
                if (nextExtentOffset < 0)
                {
                    yield return currentChunk;
                    yield break;
                }

                if (nextExtentOffset > 0)
                {
                    yield return new ReadOnlyMemory<byte>(currentChunk, 0, nextExtentOffset);
                }

                UdfLongAllocationDescriptor nextAllocationExtent = descriptorSize switch
                {
                    8 => ReadShortAllocationExtentPointer(currentChunk, nextExtentOffset, defaultPartitionReferenceNumber),
                    16 => ReadLongAllocationDescriptor(currentChunk, nextExtentOffset),
                    20 => ReadExtendedAllocationExtentPointer(currentChunk, nextExtentOffset),
                    _ => throw new NotSupportedException($"Unsupported allocation descriptor size {descriptorSize}")
                };

                if (!visited.Add((nextAllocationExtent.LogicalBlockNumber, nextAllocationExtent.PartitionReferenceNumber)))
                {
                    throw new InvalidDataException("Allocation extent descriptor chain loops");
                }

                byte[] allocationExtentDescriptor = ReadDescriptorBlock(nextAllocationExtent.LogicalBlockNumber, nextAllocationExtent.PartitionReferenceNumber);
                if (UdfUtilities.ReadTagIdentifier(allocationExtentDescriptor) != UdfUtilities.AllocationExtentDescriptor)
                {
                    throw new InvalidDataException("Invalid allocation extent descriptor");
                }

                uint lengthOfAllocationDescriptors = UdfUtilities.ReadUInt32(allocationExtentDescriptor, 20);
                if (24 + lengthOfAllocationDescriptors > allocationExtentDescriptor.Length)
                {
                    throw new InvalidDataException("Invalid allocation extent descriptor length");
                }

                currentChunk = allocationExtentDescriptor.AsSpan(24, (int)lengthOfAllocationDescriptors).ToArray();
            }
        }

        private static int FindNextAllocationExtentOffset(ReadOnlySpan<byte> descriptors, int descriptorSize)
        {
            for (int offset = 0; offset + descriptorSize <= descriptors.Length; offset += descriptorSize)
            {
                uint rawLength = UdfUtilities.ReadUInt32(descriptors, offset);
                if ((rawLength & UdfUtilities.ExtentTypeMask) == 0xC0000000)
                {
                    return offset;
                }
            }

            return -1;
        }

        private static UdfLongAllocationDescriptor ReadShortAllocationExtentPointer(ReadOnlySpan<byte> bytes, int offset, ushort defaultPartitionReferenceNumber)
        {
            UdfShortAllocationDescriptor descriptor = ReadShortAllocationDescriptor(bytes, offset);
            return new UdfLongAllocationDescriptor(descriptor.Length, descriptor.LogicalBlockNumber, defaultPartitionReferenceNumber);
        }

        private static UdfLongAllocationDescriptor ReadExtendedAllocationExtentPointer(ReadOnlySpan<byte> bytes, int offset)
        {
            UdfExtendedAllocationDescriptor descriptor = ReadExtendedAllocationDescriptor(bytes, offset);
            return new UdfLongAllocationDescriptor(descriptor.Length, descriptor.LogicalBlockNumber, descriptor.PartitionReferenceNumber);
        }

        internal byte[] ReadDescriptorBlock(uint logicalBlockNumber, ushort partitionReferenceNumber)
        {
            return ReadBlock(logicalBlockNumber, partitionReferenceNumber);
        }

        private void ReadPartitionMaps(byte[] logicalVolumeDescriptor)
        {
            uint mapTableLength = UdfUtilities.ReadUInt32(logicalVolumeDescriptor, 264);
            uint partitionMapCount = UdfUtilities.ReadUInt32(logicalVolumeDescriptor, 268);
            int offset = 440;

            for (ushort reference = 0; reference < partitionMapCount; reference++)
            {
                if (offset + 2 > logicalVolumeDescriptor.Length)
                {
                    throw new InvalidDataException("Partition map table exceeds descriptor size");
                }

                byte mapType = logicalVolumeDescriptor[offset];
                byte mapLength = logicalVolumeDescriptor[offset + 1];
                if (mapLength == 0 || offset + mapLength > logicalVolumeDescriptor.Length)
                {
                    throw new InvalidDataException("Invalid partition map length");
                }

                if (mapType == 1 && mapLength >= 6)
                {
                    ushort partitionNumber = BinaryPrimitives.ReadUInt16LittleEndian(logicalVolumeDescriptor.AsSpan(offset + 4, 2));
                    if (!_partitionsByNumber.TryGetValue(partitionNumber, out UdfPartitionDescriptorInfo partition))
                    {
                        throw new InvalidDataException($"Partition descriptor {partitionNumber} not found");
                    }

                    _partitionMapsByReference[reference] = new UdfPartitionMapInfo
                    {
                        ReferenceNumber = reference,
                        PartitionNumber = partitionNumber,
                        Kind = UdfPartitionMapKind.Physical,
                        StartingBlock = partition.StartingBlock,
                        LengthInBlocks = partition.LengthInBlocks
                    };
                }
                else if (mapType == 2 && mapLength >= 64)
                {
                    string partitionIdentifier = UdfUtilities.ReadEntityIdentifier(logicalVolumeDescriptor, offset + 4);
                    ushort partitionNumber = BinaryPrimitives.ReadUInt16LittleEndian(logicalVolumeDescriptor.AsSpan(offset + 38, 2));

                    if (partitionIdentifier == UdfUtilities.VirtualPartitionIdentifier)
                    {
                        _partitionMapsByReference[reference] = new UdfPartitionMapInfo
                        {
                            ReferenceNumber = reference,
                            PartitionNumber = partitionNumber,
                            Kind = UdfPartitionMapKind.Virtual
                        };
                    }
                    else if (partitionIdentifier == UdfUtilities.MetadataPartitionIdentifier && mapLength >= 64)
                    {
                        _partitionMapsByReference[reference] = new UdfPartitionMapInfo
                        {
                            ReferenceNumber = reference,
                            PartitionNumber = partitionNumber,
                            Kind = UdfPartitionMapKind.Metadata,
                            MetadataFileLocation = BinaryPrimitives.ReadUInt32LittleEndian(logicalVolumeDescriptor.AsSpan(offset + 40, 4)),
                            MetadataMirrorFileLocation = BinaryPrimitives.ReadUInt32LittleEndian(logicalVolumeDescriptor.AsSpan(offset + 44, 4))
                        };
                    }
                }

                offset += mapLength;
            }

            if (mapTableLength == 0 || _partitionMapsByReference.Count == 0)
            {
                throw new InvalidDataException("No supported partition maps found");
            }
        }

        private byte[] ReadAnchorDescriptor()
        {
            foreach (uint anchorBlock in UdfUtilities.GetAnchorCandidateBlocks(_stream.Length, UdfUtilities.VolumeStructureDescriptorSize))
            {
                byte[] descriptor = ReadFixedBlock(anchorBlock, UdfUtilities.VolumeStructureDescriptorSize);
                ushort tagIdentifier = UdfUtilities.ReadTagIdentifier(descriptor);
                if (tagIdentifier == UdfUtilities.AnchorVolumeDescriptorPointer)
                {
                    return descriptor;
                }
            }

            throw new InvalidDataException("No anchor volume descriptor pointer");
        }

        private void ResolveSpecialPartitionMaps()
        {
            foreach (UdfPartitionMapInfo map in _partitionMapsByReference.Values)
            {
                if (map.Kind == UdfPartitionMapKind.Metadata || map.Kind == UdfPartitionMapKind.Virtual)
                {
                    UdfPartitionMapInfo? physicalMap = _partitionMapsByReference.Values
                        .FirstOrDefault(candidate => candidate.Kind == UdfPartitionMapKind.Physical && candidate.PartitionNumber == map.PartitionNumber);
                    if (physicalMap == null)
                    {
                        continue;
                    }

                    map.ParentReferenceNumber = physicalMap.ReferenceNumber;
                    map.StartingBlock = physicalMap.StartingBlock;
                    map.LengthInBlocks = physicalMap.LengthInBlocks;
                }

                if (map.Kind == UdfPartitionMapKind.Metadata)
                {
                    TryLoadMetadataPartitionEntries(map);
                }
                else if (map.Kind == UdfPartitionMapKind.Virtual)
                {
                    TryLoadVatEntries(map);
                }
            }
        }

        private void TryLoadMetadataPartitionEntries(UdfPartitionMapInfo map)
        {
            if (map.MetadataFileLocation != 0 && map.MetadataFileLocation != uint.MaxValue)
            {
                map.MetadataFileEntry = TryReadFileEntry(new UdfLongAllocationDescriptor((uint)AllocationUnitSize, map.MetadataFileLocation, map.ParentReferenceNumber));
            }

            if (map.MetadataMirrorFileLocation != 0 && map.MetadataMirrorFileLocation != uint.MaxValue)
            {
                map.MetadataMirrorFileEntry = TryReadFileEntry(new UdfLongAllocationDescriptor((uint)AllocationUnitSize, map.MetadataMirrorFileLocation, map.ParentReferenceNumber));
            }
        }

        private void TryLoadVatEntries(UdfPartitionMapInfo map)
        {
            if (map.LengthInBlocks == 0)
            {
                return;
            }

            for (uint block = map.LengthInBlocks; block > 0 && block + 4 >= map.LengthInBlocks; block--)
            {
                UdfFileEntryInfo? fileEntry = TryReadFileEntry(new UdfLongAllocationDescriptor((uint)AllocationUnitSize, block - 1, map.ParentReferenceNumber));
                if (fileEntry == null || fileEntry.Value.FileType != UdfUtilities.IcbFileTypeVat)
                {
                    continue;
                }

                byte[] contents = ReadContent(fileEntry.Value);
                if (contents.Length < 152)
                {
                    continue;
                }

                ushort headerLength = BinaryPrimitives.ReadUInt16LittleEndian(contents.AsSpan(0, 2));
                if (headerLength > contents.Length)
                {
                    continue;
                }

                int entryCount = (contents.Length - headerLength) / 4;
                uint[] vatEntries = new uint[entryCount];
                for (int i = 0; i < entryCount; i++)
                {
                    vatEntries[i] = BinaryPrimitives.ReadUInt32LittleEndian(contents.AsSpan(headerLength + (i * 4), 4));
                }

                map.VatEntries = vatEntries;
                break;
            }
        }

        internal UdfFileEntryInfo? TryReadFileEntry(UdfLongAllocationDescriptor icb)
        {
            try
            {
                return ReadFileEntry(icb);
            }
            catch (InvalidDataException)
            {
                return null;
            }
        }

        private uint ResolvePhysicalBlock(uint logicalBlockNumber, ushort partitionReferenceNumber, int depth = 0)
        {
            if (depth > 8)
            {
                return uint.MaxValue;
            }

            if (!_partitionMapsByReference.TryGetValue(partitionReferenceNumber, out UdfPartitionMapInfo? partitionMap))
            {
                return uint.MaxValue;
            }

            return partitionMap.Kind switch
            {
                UdfPartitionMapKind.Physical => partitionMap.StartingBlock + logicalBlockNumber,
                UdfPartitionMapKind.Virtual => ResolveVirtualBlock(logicalBlockNumber, partitionMap, depth),
                UdfPartitionMapKind.Metadata => ResolveMetadataBlock(logicalBlockNumber, partitionMap, depth),
                _ => uint.MaxValue
            };
        }

        private uint ResolveVirtualBlock(uint logicalBlockNumber, UdfPartitionMapInfo partitionMap, int depth)
        {
            if (partitionMap.VatEntries == null || logicalBlockNumber >= partitionMap.VatEntries.Length)
            {
                return uint.MaxValue;
            }

            uint remappedBlock = partitionMap.VatEntries[logicalBlockNumber];
            if (remappedBlock == 0xFFFFFFFF)
            {
                return uint.MaxValue;
            }

            return ResolvePhysicalBlock(remappedBlock, partitionMap.ParentReferenceNumber, depth + 1);
        }

        private uint ResolveMetadataBlock(uint logicalBlockNumber, UdfPartitionMapInfo partitionMap, int depth)
        {
            UdfFileEntryInfo? metadataEntry = partitionMap.MetadataFileEntry ?? partitionMap.MetadataMirrorFileEntry;
            if (metadataEntry == null)
            {
                return uint.MaxValue;
            }

            uint relativeBlock = ResolveRelativeBlock(metadataEntry.Value, logicalBlockNumber);
            if (relativeBlock == uint.MaxValue)
            {
                return uint.MaxValue;
            }

            return ResolvePhysicalBlock(relativeBlock, partitionMap.ParentReferenceNumber, depth + 1);
        }

        private uint ResolveRelativeBlock(UdfFileEntryInfo entry, uint logicalBlockNumber)
        {
            ulong blockSize = (uint)AllocationUnitSize;
            ulong targetBlockOffset = logicalBlockNumber * blockSize;
            ulong extentStart = 0;

            foreach (UdfContentExtent extent in EnumerateExtents(entry))
            {
                uint extentType = extent.RawLength & UdfUtilities.ExtentTypeMask;
                ulong extentLength = extent.RawLength & UdfUtilities.ExtentLengthMask;
                if (extentLength == 0)
                {
                    continue;
                }

                if (targetBlockOffset < extentStart + extentLength)
                {
                    if (extentType != UdfUtilities.ExtentRecordedAllocated)
                    {
                        return uint.MaxValue;
                    }

                    ulong blockOffsetWithinExtent = (targetBlockOffset - extentStart) / blockSize;
                    return checked(extent.LogicalBlockNumber + (uint)blockOffsetWithinExtent);
                }

                extentStart += extentLength;
            }

            return uint.MaxValue;
        }

        private byte[] ReadFixedBlock(uint blockNumber, int blockSize)
        {
            byte[] buffer = new byte[blockSize];
            _stream.Position = blockNumber * (long)blockSize;
            _stream.ReadExactly(buffer);
            return buffer;
        }

        private static UdfLongAllocationDescriptor ReadLongAllocationDescriptor(ReadOnlySpan<byte> bytes, int offset)
            => new(
                UdfUtilities.ReadUInt32(bytes, offset),
                UdfUtilities.ReadUInt32(bytes, offset + 4),
                UdfUtilities.ReadUInt16(bytes, offset + 8));

        private static UdfShortAllocationDescriptor ReadShortAllocationDescriptor(ReadOnlySpan<byte> bytes, int offset)
            => new(
                UdfUtilities.ReadUInt32(bytes, offset),
                UdfUtilities.ReadUInt32(bytes, offset + 4));

        private static UdfExtendedAllocationDescriptor ReadExtendedAllocationDescriptor(ReadOnlySpan<byte> bytes, int offset)
            => new(
                UdfUtilities.ReadUInt32(bytes, offset),
                UdfUtilities.ReadUInt32(bytes, offset + 4),
                UdfUtilities.ReadUInt32(bytes, offset + 8),
                UdfUtilities.ReadUInt32(bytes, offset + 12),
                UdfUtilities.ReadUInt16(bytes, offset + 16));
    }
}
