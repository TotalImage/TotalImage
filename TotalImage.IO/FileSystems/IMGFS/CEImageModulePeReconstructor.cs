using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TotalImage.FileSystems.IMGFS
{
    /// <summary>
    /// Reconstructs Windows CE IMGFS ROM modules into portable PE files.
    /// </summary>
    internal static class CEImageModulePeReconstructor
    {
        internal const ushort DefaultMachineTypeArm = 0x01C0;

        private const int DosHeaderSize = 0x80;
        private const int CoffHeaderSize = 0x18;
        private const int OptionalHeaderSize = 0xE0;
        private const int SectionHeaderSize = 0x28;
        private const uint FileAlignment = 0x200;
        private const uint SectionAlignment = 0x1000;
        private const uint ImageScnCntCode = 0x00000020;
        private const uint ImageScnCntInitializedData = 0x00000040;
        private const uint ImageScnCntUninitializedData = 0x00000080;
        private const uint ImageScnTypeNoLoad = 0x00000002;
        private const uint ImageScnCompressed = 0x00002000;
        private const ushort ImageRelBasedHighLow = 3;
        private const int ImportDescriptorSize = 0x14;
        private const int DataDirectoryCount = 16;
        private const int ExportDirectoryIndex = 0;
        private const int ImportDirectoryIndex = 1;
        private const int ResourceDirectoryIndex = 2;
        private const int ExceptionDirectoryIndex = 3;
        private const int BaseRelocationDirectoryIndex = 5;
        private const int ComDescriptorDirectoryIndex = 14;

        private static readonly byte[] DosStub =
        [
            0x4d, 0x5a, 0x90, 0x00, 0x03, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0xff, 0xff, 0x00, 0x00,
            0xb8, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00,
            0x0e, 0x1f, 0xba, 0x0e, 0x00, 0xb4, 0x09, 0xcd, 0x21, 0xb8, 0x01, 0x4c, 0xcd, 0x21, 0x54, 0x68,
            0x69, 0x73, 0x20, 0x70, 0x72, 0x6f, 0x67, 0x72, 0x61, 0x6d, 0x20, 0x63, 0x61, 0x6e, 0x6e, 0x6f,
            0x74, 0x20, 0x62, 0x65, 0x20, 0x72, 0x75, 0x6e, 0x20, 0x69, 0x6e, 0x20, 0x44, 0x4f, 0x53, 0x20,
            0x6d, 0x6f, 0x64, 0x65, 0x2e, 0x0d, 0x0d, 0x0a, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        ];

        internal static byte[] Reconstruct(byte[] romHeaderData, IReadOnlyList<ModuleSectionData> sections, ushort machineType = DefaultMachineTypeArm)
        {
            ArgumentNullException.ThrowIfNull(romHeaderData);
            ArgumentNullException.ThrowIfNull(sections);

            E32Rom e32 = E32Rom.Parse(romHeaderData, out int e32Size);
            if (sections.Count != e32.ObjectCount)
            {
                throw new InvalidDataException($"IMGFS ROM module declares {e32.ObjectCount} sections but the section chain yielded {sections.Count} entries.");
            }

            var o32Sections = new List<O32Rom>(e32.ObjectCount);
            for (int index = 0; index < e32.ObjectCount; index++)
            {
                o32Sections.Add(O32Rom.Parse(romHeaderData.AsSpan(e32Size + (index * O32Rom.Size), O32Rom.Size)));
            }

            var outputSections = new List<SectionOutput>(e32.ObjectCount);
            for (int index = 0; index < e32.ObjectCount; index++)
            {
                outputSections.Add(SectionOutput.Create(o32Sections[index], sections[index], e32));
            }

            var rvaPatches = BuildRvaPatchMap(outputSections);
            DataDirectory[] directories = BuildPatchedDirectories(e32, rvaPatches);
            uint entryPoint = PatchRva(e32.EntryRva, rvaPatches);
            uint codeBase = FindFirstSectionRva(outputSections, ImageScnCntCode);
            uint dataBase = FindFirstSectionRva(outputSections, ImageScnCntInitializedData);

            foreach (SectionOutput section in outputSections)
            {
                if (!section.IsFixup)
                {
                    continue;
                }

                byte[] rebuiltFixups = ExpandFixupData(section.RawData, rvaPatches);
                section.RawData = rebuiltFixups;
                section.VirtualSize = (uint)rebuiltFixups.Length;
                directories[BaseRelocationDirectoryIndex] = directories[BaseRelocationDirectoryIndex] with { Size = (uint)rebuiltFixups.Length };
            }

            uint headerSize = AlignUp((uint)(DosHeaderSize + CoffHeaderSize + OptionalHeaderSize + (outputSections.Count * SectionHeaderSize)), FileAlignment);
            uint nextRawPointer = headerSize;
            foreach (SectionOutput section in outputSections)
            {
                section.RawSize = AlignUp(section.VirtualSize, FileAlignment);
                section.RawPointer = nextRawPointer;
                nextRawPointer += section.RawSize;
            }

            uint sizeOfImage = AlignUp(outputSections.Max(static section => section.Rva + Math.Max(section.VirtualSize, SectionAlignment)), SectionAlignment);
            byte[] image = new byte[nextRawPointer];
            WriteHeaders(image, machineType, e32, outputSections, directories, entryPoint, codeBase, dataBase, headerSize, sizeOfImage);

            foreach (SectionOutput section in outputSections)
            {
                if (section.RawData.Length == 0)
                {
                    continue;
                }

                Buffer.BlockCopy(section.RawData, 0, image, checked((int)section.RawPointer), section.RawData.Length);
            }

            RepairImportTable(image, directories[ImportDirectoryIndex], outputSections, rvaPatches);
            RepairExportTable(image, directories[ExportDirectoryIndex], outputSections, rvaPatches);
            return image;
        }

        private static void WriteHeaders(byte[] image, ushort machineType, E32Rom e32, IReadOnlyList<SectionOutput> sections, IReadOnlyList<DataDirectory> directories, uint entryPoint, uint codeBase, uint dataBase, uint sizeOfHeaders, uint sizeOfImage)
        {
            DosStub.CopyTo(image, 0);
            int peOffset = DosHeaderSize;

            WriteUInt32(image, peOffset + 0x00, 0x00004550);
            WriteUInt16(image, peOffset + 0x04, machineType);
            WriteUInt16(image, peOffset + 0x06, checked((ushort)sections.Count));
            WriteUInt32(image, peOffset + 0x08, e32.Timestamp);
            WriteUInt32(image, peOffset + 0x0C, 0);
            WriteUInt32(image, peOffset + 0x10, 0);
            WriteUInt16(image, peOffset + 0x14, OptionalHeaderSize);
            WriteUInt16(image, peOffset + 0x16, e32.ImageFlags);

            int optionalHeaderOffset = peOffset + CoffHeaderSize;
            WriteUInt16(image, optionalHeaderOffset + 0x00, 0x010B);
            image[optionalHeaderOffset + 0x02] = 6;
            image[optionalHeaderOffset + 0x03] = 1;
            WriteUInt32(image, optionalHeaderOffset + 0x04, CalculateSegmentSizeSum(sections, ImageScnCntCode));
            WriteUInt32(image, optionalHeaderOffset + 0x08, CalculateSegmentSizeSum(sections, ImageScnCntInitializedData));
            WriteUInt32(image, optionalHeaderOffset + 0x0C, CalculateSegmentSizeSum(sections, ImageScnCntUninitializedData));
            WriteUInt32(image, optionalHeaderOffset + 0x10, entryPoint);
            WriteUInt32(image, optionalHeaderOffset + 0x14, codeBase);
            WriteUInt32(image, optionalHeaderOffset + 0x18, dataBase);
            WriteUInt32(image, optionalHeaderOffset + 0x1C, e32.VirtualBase);
            WriteUInt32(image, optionalHeaderOffset + 0x20, SectionAlignment);
            WriteUInt32(image, optionalHeaderOffset + 0x24, FileAlignment);
            WriteUInt16(image, optionalHeaderOffset + 0x28, 4);
            WriteUInt16(image, optionalHeaderOffset + 0x2A, 0);
            WriteUInt16(image, optionalHeaderOffset + 0x2C, 0);
            WriteUInt16(image, optionalHeaderOffset + 0x2E, 0);
            WriteUInt16(image, optionalHeaderOffset + 0x30, e32.SubsystemMajor);
            WriteUInt16(image, optionalHeaderOffset + 0x32, e32.SubsystemMinor);
            WriteUInt32(image, optionalHeaderOffset + 0x38, sizeOfImage);
            WriteUInt32(image, optionalHeaderOffset + 0x3C, sizeOfHeaders);
            WriteUInt32(image, optionalHeaderOffset + 0x40, 0);
            WriteUInt16(image, optionalHeaderOffset + 0x44, e32.Subsystem);
            WriteUInt16(image, optionalHeaderOffset + 0x46, 0);
            WriteUInt32(image, optionalHeaderOffset + 0x48, e32.StackMax);
            WriteUInt32(image, optionalHeaderOffset + 0x4C, 0x1000);
            WriteUInt32(image, optionalHeaderOffset + 0x50, 0x100000);
            WriteUInt32(image, optionalHeaderOffset + 0x54, 0x1000);
            WriteUInt32(image, optionalHeaderOffset + 0x58, 0);
            WriteUInt32(image, optionalHeaderOffset + 0x5C, DataDirectoryCount);

            for (int index = 0; index < directories.Count; index++)
            {
                int directoryOffset = optionalHeaderOffset + 0x60 + (index * 8);
                WriteUInt32(image, directoryOffset + 0x00, directories[index].Rva);
                WriteUInt32(image, directoryOffset + 0x04, directories[index].Size);
            }

            int sectionHeaderOffset = optionalHeaderOffset + OptionalHeaderSize;
            foreach (SectionOutput section in sections)
            {
                WriteAscii(image, sectionHeaderOffset + 0x00, 8, section.Name);
                WriteUInt32(image, sectionHeaderOffset + 0x08, section.VirtualSize);
                WriteUInt32(image, sectionHeaderOffset + 0x0C, section.Rva);
                WriteUInt32(image, sectionHeaderOffset + 0x10, section.RawSize);
                WriteUInt32(image, sectionHeaderOffset + 0x14, section.RawPointer);
                WriteUInt32(image, sectionHeaderOffset + 0x18, 0);
                WriteUInt32(image, sectionHeaderOffset + 0x1C, 0);
                WriteUInt32(image, sectionHeaderOffset + 0x20, 0);
                WriteUInt32(image, sectionHeaderOffset + 0x24, section.Flags);
                sectionHeaderOffset += SectionHeaderSize;
            }
        }

        private static DataDirectory[] BuildPatchedDirectories(E32Rom e32, IReadOnlyList<RvaPatchRange> rvaPatches)
        {
            var directories = new DataDirectory[DataDirectoryCount];
            for (int index = 0; index < Math.Min(9, e32.Info.Length); index++)
            {
                directories[index] = new DataDirectory(PatchRva(e32.Info[index].Rva, rvaPatches), e32.Info[index].Size);
            }

            directories[ComDescriptorDirectoryIndex] = new DataDirectory(PatchRva(e32.Section14Rva, rvaPatches), e32.Section14Size);
            return directories;
        }

        private static List<RvaPatchRange> BuildRvaPatchMap(IEnumerable<SectionOutput> sections)
        {
            var patches = new List<RvaPatchRange>();
            foreach (SectionOutput section in sections)
            {
                if (section.Name.Equals(".pdata", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (patches.Any(existing => existing.OriginalRva == section.OriginalRva))
                {
                    continue;
                }

                patches.Add(new RvaPatchRange(section.OriginalRva, section.Rva, section.VirtualSize));
            }

            return patches;
        }

        private static uint PatchRva(uint romRva, IReadOnlyList<RvaPatchRange> patches)
        {
            foreach (RvaPatchRange patch in patches)
            {
                if (patch.Contains(romRva))
                {
                    return romRva - patch.OriginalRva + patch.NewRva;
                }
            }

            return romRva;
        }

        private static uint FindFirstSectionRva(IEnumerable<SectionOutput> sections, uint flag)
        {
            foreach (SectionOutput section in sections)
            {
                if ((section.Flags & flag) != 0)
                {
                    return section.Rva;
                }
            }

            return 0;
        }

        private static uint CalculateSegmentSizeSum(IEnumerable<SectionOutput> sections, uint flag)
        {
            uint total = 0;
            foreach (SectionOutput section in sections)
            {
                if ((section.Flags & flag) != 0)
                {
                    total += AlignUp(section.VirtualSize, FileAlignment);
                }
            }

            return total;
        }

        private static byte[] ExpandFixupData(ReadOnlySpan<byte> compressedFixups, IReadOnlyList<RvaPatchRange> patches)
        {
            var relocations = new List<uint>();
            int offset = 0;
            while (offset + 4 <= compressedFixups.Length)
            {
                ushort marker = BinaryPrimitives.ReadUInt16LittleEndian(compressedFixups.Slice(offset, 2));
                ushort size = BinaryPrimitives.ReadUInt16LittleEndian(compressedFixups.Slice(offset + 2, 2));
                offset += 4;
                if (offset + size > compressedFixups.Length)
                {
                    throw new InvalidDataException("Compressed relocation stream extends past the end of the IMGFS section.");
                }

                if (marker != 0xFD00 && marker != 0xFE00)
                {
                    DecompressRelocationString(compressedFixups.Slice(offset, size), relocations);
                }

                offset += size;
            }

            for (int index = 0; index < relocations.Count; index++)
            {
                relocations[index] = PatchRva(relocations[index], patches);
            }

            return PackRelocations(relocations);
        }

        private static void DecompressRelocationString(ReadOnlySpan<byte> data, ICollection<uint> relocations)
        {
            uint currentBase = 0;
            int offset = 0;
            while (offset < data.Length)
            {
                byte current = data[offset++];
                bool bit7 = (current & 0x80) != 0;
                if (bit7)
                {
                    uint increment = 4u * (((uint)(current >> 5) & 0x03u) + 1u);
                    uint count = (uint)(current & 0x1Fu) + 1u;
                    uint value = 0;
                    offset += ReadVariableLengthValue(data[offset..], ref value, 0);
                    currentBase += value;
                    relocations.Add(currentBase);
                    while (count-- > 0)
                    {
                        currentBase += increment;
                        relocations.Add(currentBase);
                    }

                    continue;
                }

                bool bit6 = (current & 0x40) != 0;
                bool bit5 = (current & 0x20) != 0;
                uint delta = (uint)(current & 0x1F);
                if (bit5)
                {
                    offset += ReadVariableLengthValue(data[offset..], ref delta, 5);
                }

                currentBase = bit6 ? currentBase + delta : delta;
                relocations.Add(currentBase);
            }
        }

        private static int ReadVariableLengthValue(ReadOnlySpan<byte> data, ref uint value, int shift)
        {
            int consumed = 0;
            foreach (byte current in data)
            {
                consumed++;
                value |= (uint)(current & 0x7F) << shift;
                if ((current & 0x80) == 0)
                {
                    return consumed;
                }

                shift += 7;
            }

            throw new InvalidDataException("Compressed relocation value runs past the end of the section.");
        }

        private static byte[] PackRelocations(IEnumerable<uint> relocations)
        {
            var output = new List<byte>();
            uint currentPage = 0;
            var pageRelocations = new List<ushort>();

            foreach (uint relocation in relocations)
            {
                uint relocationPage = relocation & ~0xFFFu;
                if (currentPage == 0)
                {
                    currentPage = relocationPage;
                }
                else if (currentPage != relocationPage)
                {
                    FlushRelocationPage(output, currentPage, pageRelocations);
                    currentPage = relocationPage;
                    pageRelocations.Clear();
                }

                pageRelocations.Add((ushort)((ImageRelBasedHighLow << 12) | (relocation & 0x0FFF)));
            }

            if (pageRelocations.Count > 0)
            {
                FlushRelocationPage(output, currentPage, pageRelocations);
            }

            return output.ToArray();
        }

        private static void FlushRelocationPage(ICollection<byte> output, uint pageRva, IList<ushort> pageRelocations)
        {
            if ((pageRelocations.Count & 1) != 0)
            {
                pageRelocations.Add(0);
            }

            int blockSize = 8 + (pageRelocations.Count * 2);
            AddUInt32(output, pageRva);
            AddUInt32(output, (uint)blockSize);
            foreach (ushort relocation in pageRelocations)
            {
                AddUInt16(output, relocation);
            }
        }

        private static void RepairImportTable(byte[] image, DataDirectory importDirectory, IReadOnlyList<SectionOutput> sections, IReadOnlyList<RvaPatchRange> patches)
        {
            if (importDirectory.Rva == 0)
            {
                return;
            }

            uint importOffset = RvaToFileOffset(importDirectory.Rva, sections);
            while (importOffset + ImportDescriptorSize <= image.Length)
            {
                uint firstThunk = ReadUInt32(image, checked((int)importOffset + 0x10));
                if (firstThunk == 0)
                {
                    return;
                }

                WriteUInt32(image, checked((int)importOffset + 0x10), PatchRva(firstThunk, patches));
                importOffset += ImportDescriptorSize;
            }
        }

        private static void RepairExportTable(byte[] image, DataDirectory exportDirectory, IReadOnlyList<SectionOutput> sections, IReadOnlyList<RvaPatchRange> patches)
        {
            if (exportDirectory.Rva == 0)
            {
                return;
            }

            uint exportOffset = RvaToFileOffset(exportDirectory.Rva, sections);
            uint exportAddressTableRva = ReadUInt32(image, checked((int)exportOffset + 0x1C));
            uint exportCount = ReadUInt32(image, checked((int)exportOffset + 0x14));
            uint exportAddressTableOffset = RvaToFileOffset(exportAddressTableRva, sections);
            for (uint index = 0; index < exportCount; index++)
            {
                int entryOffset = checked((int)(exportAddressTableOffset + (index * 4)));
                uint originalRva = ReadUInt32(image, entryOffset);
                WriteUInt32(image, entryOffset, PatchRva(originalRva, patches));
            }
        }

        private static uint RvaToFileOffset(uint rva, IReadOnlyList<SectionOutput> sections)
        {
            foreach (SectionOutput section in sections)
            {
                uint sectionLength = Math.Max(section.VirtualSize, section.RawSize);
                if (section.Rva <= rva && rva < section.Rva + sectionLength)
                {
                    return rva - section.Rva + section.RawPointer;
                }
            }

            throw new InvalidDataException($"Module RVA 0x{rva:X8} does not map to any reconstructed PE section.");
        }

        private static uint AlignUp(uint value, uint alignment)
        {
            if (value == 0)
            {
                return 0;
            }

            return ((value - 1) | (alignment - 1)) + 1;
        }

        private static uint ReadUInt32(byte[] buffer, int offset) => BinaryPrimitives.ReadUInt32LittleEndian(buffer.AsSpan(offset, 4));

        private static void WriteUInt16(byte[] buffer, int offset, ushort value) => BinaryPrimitives.WriteUInt16LittleEndian(buffer.AsSpan(offset, 2), value);

        private static void WriteUInt32(byte[] buffer, int offset, uint value) => BinaryPrimitives.WriteUInt32LittleEndian(buffer.AsSpan(offset, 4), value);

        private static void WriteAscii(byte[] buffer, int offset, int length, string value)
        {
            Span<byte> destination = buffer.AsSpan(offset, length);
            destination.Clear();
            Encoding.ASCII.GetBytes(value.AsSpan(0, Math.Min(value.Length, length)), destination);
        }

        private static void AddUInt16(ICollection<byte> buffer, ushort value)
        {
            buffer.Add((byte)value);
            buffer.Add((byte)(value >> 8));
        }

        private static void AddUInt32(ICollection<byte> buffer, uint value)
        {
            buffer.Add((byte)value);
            buffer.Add((byte)(value >> 8));
            buffer.Add((byte)(value >> 16));
            buffer.Add((byte)(value >> 24));
        }

        internal sealed record ModuleSectionData(string Name, byte[] Data);

        private readonly record struct DataDirectory(uint Rva, uint Size);

        private readonly record struct InfoItem(uint Rva, uint Size)
        {
            public bool Matches(uint rva, uint size) => Rva == rva && Size == size;
        }

        private readonly record struct RvaPatchRange(uint OriginalRva, uint NewRva, uint Size)
        {
            public bool Contains(uint rva) => OriginalRva <= rva && rva < OriginalRva + Size;
        }

        private sealed class SectionOutput
        {
            private SectionOutput(string name, uint originalRva, uint rva, uint virtualSize, uint flags, bool isFixup, byte[] rawData)
            {
                Name = name;
                OriginalRva = originalRva;
                Rva = rva;
                VirtualSize = virtualSize;
                Flags = flags;
                IsFixup = isFixup;
                RawData = rawData;
            }

            public string Name { get; }

            public uint OriginalRva { get; }

            public uint Rva { get; }

            public uint VirtualSize { get; set; }

            public uint RawSize { get; set; }

            public uint RawPointer { get; set; }

            public uint Flags { get; }

            public bool IsFixup { get; }

            public byte[] RawData { get; set; }

            public static SectionOutput Create(O32Rom o32, ModuleSectionData section, E32Rom e32)
            {
                string name;
                bool isFixup = false;
                if (e32.Info[ResourceDirectoryIndex].Matches(o32.Rva, o32.VirtualSize))
                {
                    name = ".rsrc";
                }
                else if (e32.Info[ExceptionDirectoryIndex].Matches(o32.Rva, o32.VirtualSize))
                {
                    name = ".pdata";
                }
                else if (e32.Info[BaseRelocationDirectoryIndex].Matches(o32.Rva, o32.VirtualSize))
                {
                    name = ".reloc";
                    isFixup = true;
                }
                else if ((o32.Flags & ImageScnCntCode) != 0)
                {
                    name = ".text";
                }
                else if ((o32.Flags & ImageScnCntInitializedData) != 0)
                {
                    name = ".data";
                }
                else if ((o32.Flags & ImageScnCntUninitializedData) != 0)
                {
                    name = ".pdata";
                }
                else
                {
                    name = string.IsNullOrWhiteSpace(section.Name) ? ".other" : section.Name;
                }

                uint rva = ((o32.Flags & ImageScnTypeNoLoad) != 0) && o32.RealAddress == 0
                    ? o32.Rva
                    : o32.RealAddress - e32.VirtualBase;

                return new SectionOutput(name, o32.Rva, rva, o32.VirtualSize, o32.Flags & ~ImageScnCompressed, isFixup, section.Data);
            }
        }

        private sealed class E32Rom
        {
            private E32Rom(ushort objectCount, ushort imageFlags, uint entryRva, uint virtualBase, ushort subsystemMajor, ushort subsystemMinor, uint stackMax, uint virtualSize, uint section14Rva, uint section14Size, uint timestamp, InfoItem[] info, ushort subsystem)
            {
                ObjectCount = objectCount;
                ImageFlags = imageFlags;
                EntryRva = entryRva;
                VirtualBase = virtualBase;
                SubsystemMajor = subsystemMajor;
                SubsystemMinor = subsystemMinor;
                StackMax = stackMax;
                VirtualSize = virtualSize;
                Section14Rva = section14Rva;
                Section14Size = section14Size;
                Timestamp = timestamp;
                Info = info;
                Subsystem = subsystem;
            }

            public ushort ObjectCount { get; }

            public ushort ImageFlags { get; }

            public uint EntryRva { get; }

            public uint VirtualBase { get; }

            public ushort SubsystemMajor { get; }

            public ushort SubsystemMinor { get; }

            public uint StackMax { get; }

            public uint VirtualSize { get; }

            public uint Section14Rva { get; }

            public uint Section14Size { get; }

            public uint Timestamp { get; }

            public InfoItem[] Info { get; }

            public ushort Subsystem { get; }

            public static E32Rom Parse(ReadOnlySpan<byte> data, out int headerSize)
            {
                if (data.Length < 0x6C)
                {
                    throw new InvalidDataException("IMGFS ROM module PE metadata block is too small to contain an E32 header.");
                }

                ushort objectCount = BinaryPrimitives.ReadUInt16LittleEndian(data[0x00..0x02]);
                int withTimestamp = 0x70 + (objectCount * O32Rom.Size);
                int withoutTimestamp = 0x6C + (objectCount * O32Rom.Size);
                bool hasTimestamp = data.Length == withTimestamp || (data.Length >= withTimestamp && data.Length != withoutTimestamp);
                headerSize = hasTimestamp ? 0x70 : 0x6C;
                int expectedSize = headerSize + (objectCount * O32Rom.Size);
                if (data.Length != expectedSize)
                {
                    throw new InvalidDataException($"IMGFS ROM module PE metadata block has size 0x{data.Length:X}, expected 0x{expectedSize:X}.");
                }

                var info = new InfoItem[9];
                int infoOffset = hasTimestamp ? 0x24 : 0x20;
                for (int index = 0; index < info.Length; index++)
                {
                    int entryOffset = infoOffset + (index * 8);
                    info[index] = new InfoItem(
                        BinaryPrimitives.ReadUInt32LittleEndian(data.Slice(entryOffset, 4)),
                        BinaryPrimitives.ReadUInt32LittleEndian(data.Slice(entryOffset + 4, 4)));
                }

                return new E32Rom(
                    objectCount,
                    BinaryPrimitives.ReadUInt16LittleEndian(data[0x02..0x04]),
                    BinaryPrimitives.ReadUInt32LittleEndian(data[0x04..0x08]),
                    BinaryPrimitives.ReadUInt32LittleEndian(data[0x08..0x0C]),
                    BinaryPrimitives.ReadUInt16LittleEndian(data[0x0C..0x0E]),
                    BinaryPrimitives.ReadUInt16LittleEndian(data[0x0E..0x10]),
                    BinaryPrimitives.ReadUInt32LittleEndian(data[0x10..0x14]),
                    BinaryPrimitives.ReadUInt32LittleEndian(data[0x14..0x18]),
                    BinaryPrimitives.ReadUInt32LittleEndian(data[0x18..0x1C]),
                    BinaryPrimitives.ReadUInt32LittleEndian(data[0x1C..0x20]),
                    hasTimestamp ? BinaryPrimitives.ReadUInt32LittleEndian(data[0x20..0x24]) : 0,
                    info,
                    BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(headerSize - 4, 2)));
            }
        }

        private sealed class O32Rom
        {
            public const int Size = 0x18;

            private O32Rom(uint virtualSize, uint rva, uint physicalSize, uint dataPointer, uint realAddress, uint flags)
            {
                VirtualSize = virtualSize;
                Rva = rva;
                PhysicalSize = physicalSize;
                DataPointer = dataPointer;
                RealAddress = realAddress;
                Flags = flags;
            }

            public uint VirtualSize { get; }

            public uint Rva { get; }

            public uint PhysicalSize { get; }

            public uint DataPointer { get; }

            public uint RealAddress { get; }

            public uint Flags { get; }

            public static O32Rom Parse(ReadOnlySpan<byte> data)
            {
                if (data.Length < Size)
                {
                    throw new InvalidDataException("IMGFS ROM module O32 header is truncated.");
                }

                return new O32Rom(
                    BinaryPrimitives.ReadUInt32LittleEndian(data[0x00..0x04]),
                    BinaryPrimitives.ReadUInt32LittleEndian(data[0x04..0x08]),
                    BinaryPrimitives.ReadUInt32LittleEndian(data[0x08..0x0C]),
                    BinaryPrimitives.ReadUInt32LittleEndian(data[0x0C..0x10]),
                    BinaryPrimitives.ReadUInt32LittleEndian(data[0x10..0x14]),
                    BinaryPrimitives.ReadUInt32LittleEndian(data[0x14..0x18]));
            }
        }
    }
}
