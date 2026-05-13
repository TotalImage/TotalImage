using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TotalImage.Compression.Xpress;
using DirectoryBase = TotalImage.FileSystems.Directory;
using FileBase = TotalImage.FileSystems.File;

namespace TotalImage.FileSystems.IMGFS
{
    /// <summary>
    /// Read-only representation of a Windows CE IMGFS file system.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This implementation follows the layout documented by the original <c>eimgfs</c> C++ tool.
    /// </para>
    /// <para>
    /// IMGFS stores a linked list of directory blocks. Each directory block contains fixed-size directory
    /// entries, and file contents are described through separate index blocks that point at data chunks.
    /// Chunk data may be stored compressed. This implementation fully parses metadata and can read files whose
    /// chunks are stored uncompressed. Compressed chunks and ROM module reconstruction are detected and reported
    /// explicitly rather than being returned incorrectly.
    /// </para>
    /// </remarks>
    public class CEImageFileSystem : FileSystem
    {
        private static readonly byte[] ImgfsSignature = [0xf8, 0xac, 0x2c, 0x9d, 0xe3, 0xd4, 0x2b, 0x4d, 0xbd, 0x30, 0x91, 0x6e, 0xd8, 0x4f, 0x31, 0xdc];

        private const uint DirectoryBlockMagic = 0x2F5314CE;
        private const uint FileEntryMagic = 0xFFFFF6FE;
        private const uint ModuleEntryMagic = 0xFFFFFEFE;
        private const uint SectionEntryMagic = 0xFFFFF6FD;
        private const uint NameEntryMagic = 0xFFFFFEFB;
        private const uint SupportedDirectoryEntrySize = 0x34;
        private const ushort NameStoredInDirectoryEntryCharacterLimit = 4;
        private const ushort NameStoredInNameEntryCharacterLimit = 24;
        private const ushort NameIsStoredInNameEntryFlag = 0x0002;

        private readonly ImgfsHeader _header;
        private readonly ImgfsRootDirectory _rootDirectory;
        private readonly Dictionary<uint, string> _nameCache = new();
        private readonly ushort _moduleMachineType;

        /// <inheritdoc />
        public override string DisplayName => "CE Image File System";

        /// <inheritdoc />
        public override string VolumeLabel { get; set; } = string.Empty;

        /// <inheritdoc />
        public override DirectoryBase RootDirectory => _rootDirectory;

        /// <inheritdoc />
        /// <remarks>
        /// IMGFS tracks a free sector count in its header. The reference implementation treats one IMGFS block as
        /// the unit of that count, so the reported free space is derived from <c>FreeSectorCount * BytesPerBlock</c>.
        /// </remarks>
        public override long TotalFreeSpace => 0; // (long)_header.FreeSectorCount * _header.BytesPerBlock;

        /// <inheritdoc />
        public override long TotalSize { get; }

        /// <inheritdoc />
        public override long AllocationUnitSize => _header.BytesPerChunk;

        /// <inheritdoc />
        public override bool SupportsSubdirectories => true;

        /// <inheritdoc />
        public override bool IsReadOnly => true;

        /// <summary>
        /// Creates a Windows CE IMGFS file system view over a stream whose position 0 is the IMGFS header.
        /// </summary>
        /// <param name="containerStream">The stream containing the IMGFS image.</param>
        /// <exception cref="InvalidDataException">Thrown when the stream does not contain a supported IMGFS image.</exception>
        public CEImageFileSystem(Stream containerStream)
            : this(containerStream, CEImageModulePeReconstructor.DefaultMachineTypeArm)
        {
        }

        internal CEImageFileSystem(Stream containerStream, ushort moduleMachineType) : base(containerStream)
        {
            _moduleMachineType = moduleMachineType;
            _header = ImgfsHeader.Read(_stream);
            TotalSize = _stream.Length;

            _rootDirectory = new ImgfsRootDirectory(this);
            BuildDirectoryTree(ParseFiles().ToList());
        }

        private IEnumerable<ImgfsFileRecord> ParseFiles()
        {
            foreach (long directoryBlockOffset in EnumerateDirectoryBlockOffsets())
            {
                byte[] directoryBlock = ReadBytes(directoryBlockOffset + sizeof(uint) + sizeof(uint), _header.BytesPerBlock - 8);

                for (int entryOffset = 0; entryOffset + _header.DirectoryEntrySize <= directoryBlock.Length; entryOffset += _header.DirectoryEntrySize)
                {
                    ReadOnlySpan<byte> entry = directoryBlock.AsSpan(entryOffset, _header.DirectoryEntrySize);
                    uint magic = BinaryPrimitives.ReadUInt32LittleEndian(entry[0..4]);
                    if (magic != FileEntryMagic && magic != ModuleEntryMagic)
                    {
                        continue;
                    }

                    string fullPath = NormalizePath(ReadName(entry[12..24]));
                    if (string.IsNullOrWhiteSpace(fullPath))
                    {
                        fullPath = $"entry_{directoryBlockOffset + 8 + entryOffset:X8}";
                    }

                    uint size = BinaryPrimitives.ReadUInt32LittleEndian(entry[24..28]);
                    FileAttributes attributes = (FileAttributes)BinaryPrimitives.ReadUInt32LittleEndian(entry[28..32]);
                    long rawFileTime = BinaryPrimitives.ReadInt64LittleEndian(entry[32..40]);
                    uint sectionListOffset = BinaryPrimitives.ReadUInt32LittleEndian(entry[8..12]);
                    uint indexPointer = BinaryPrimitives.ReadUInt32LittleEndian(entry[44..48]);
                    uint indexSize = BinaryPrimitives.ReadUInt32LittleEndian(entry[48..52]);

                    yield return new ImgfsFileRecord(
                        fullPath,
                        size,
                        attributes,
                        ConvertFileTime(rawFileTime),
                        indexPointer,
                        indexSize,
                        sectionListOffset,
                        magic == ModuleEntryMagic,
                        CalculateStoredLength(indexPointer, indexSize));
                }
            }
        }

        private void BuildDirectoryTree(IReadOnlyList<ImgfsFileRecord> files)
        {
            if (TryLoadProvisioningOverlay(files, out ProvisioningOverlay? overlay) && overlay is not null)
            {
                BuildProvisionedDirectoryTree(files, overlay);
                return;
            }

            var seenFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (ImgfsFileRecord file in files.OrderBy(static file => file.FullPath, StringComparer.OrdinalIgnoreCase))
            {
                if (!seenFiles.Add(file.FullPath))
                {
                    continue;
                }

                AddFileToTree(file.FullPath, file);
            }
        }

        private void BuildProvisionedDirectoryTree(IReadOnlyList<ImgfsFileRecord> files, ProvisioningOverlay overlay)
        {
            // Many Windows Mobile 5/6 ROMs expose a flat IMGFS internally while the live file tree is
            // projected during cold boot through DAT provisioning files. When all extracted names are flat and
            // the provisioning data references \Windows, treat the flat IMGFS set as the \Windows backing store.
            bool defaultFlatFilesToWindowsDirectory = files.All(static file => !ContainsPathSeparator(file.FullPath))
                && overlay.ReferencesWindowsDirectory;

            var seenDisplayPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var filesByPath = new Dictionary<string, ImgfsFileRecord>(StringComparer.OrdinalIgnoreCase);
            var filesByName = new Dictionary<string, ImgfsFileRecord>(StringComparer.OrdinalIgnoreCase);

            foreach (ImgfsFileRecord file in files.OrderBy(static file => file.FullPath, StringComparer.OrdinalIgnoreCase))
            {
                string displayPath = ResolveProvisionedBasePath(file.FullPath, defaultFlatFilesToWindowsDirectory);
                if (!seenDisplayPaths.Add(displayPath))
                {
                    continue;
                }

                AddFileToTree(displayPath, file);

                filesByPath.TryAdd(displayPath, file);
                filesByPath.TryAdd(file.FullPath, file);
                filesByName.TryAdd(Path.GetFileName(displayPath), file);
            }

            foreach (string directory in overlay.Directories.OrderBy(static directory => directory, StringComparer.OrdinalIgnoreCase))
            {
                EnsureDirectory(directory);
            }

            foreach ((string targetPath, string sourcePath) in overlay.FileMappings.OrderBy(static mapping => mapping.Key, StringComparer.OrdinalIgnoreCase))
            {
                if (seenDisplayPaths.Contains(targetPath))
                {
                    continue;
                }

                if (!TryResolveProvisionedSource(sourcePath, filesByPath, filesByName, out ImgfsFileRecord? sourceFile) || sourceFile is null)
                {
                    continue;
                }

                AddFileToTree(targetPath, sourceFile);
                seenDisplayPaths.Add(targetPath);
            }
        }

        private bool TryLoadProvisioningOverlay(IReadOnlyList<ImgfsFileRecord> files, out ProvisioningOverlay? overlay)
        {
            overlay = null;

            // initobj.dat is the generic CE cold-boot object initialization source. initflashfiles.dat is the
            // Windows Mobile-specific overlay commonly used to project the user-visible directory tree. Load both,
            // but merge initflashfiles.dat last so it wins when the two specify the same destination path.
            var combinedOverlay = new ProvisioningOverlay();
            bool loadedAnyOverlay = false;

            foreach (string provisioningFileName in new[] { "initobj.dat", "initflashfiles.dat" })
            {
                ImgfsFileRecord? provisioningFile = files.FirstOrDefault(file => string.Equals(Path.GetFileName(file.FullPath), provisioningFileName, StringComparison.OrdinalIgnoreCase));
                if (provisioningFile == null)
                {
                    continue;
                }

                try
                {
                    using MemoryStream content = OpenFileContents(provisioningFile);
                    ProvisioningOverlay parsedOverlay = ParseProvisioningOverlay(content.ToArray());
                    if (!parsedOverlay.IsEmpty)
                    {
                        combinedOverlay.MergeFrom(parsedOverlay);
                        loadedAnyOverlay = true;
                    }
                }
                catch (Exception ex) when (ex is InvalidDataException || ex is NotSupportedException)
                {
                    // If the DAT file is compressed or uses unsupported syntax, fall back to the raw IMGFS tree.
                    continue;
                }
            }

            if (!loadedAnyOverlay)
            {
                return false;
            }

            overlay = combinedOverlay;
            return true;
        }

        internal static ProvisioningOverlayData ParseProvisioningOverlayData(byte[] bytes)
        {
            ProvisioningOverlay overlay = ParseProvisioningOverlayCore(bytes);
            return new ProvisioningOverlayData(
                overlay.Directories.OrderBy(static directory => directory, StringComparer.OrdinalIgnoreCase).ToArray(),
                new Dictionary<string, string>(overlay.FileMappings, StringComparer.OrdinalIgnoreCase));
        }

        internal static ProvisioningOverlayData MergeProvisioningOverlayData(params byte[][] overlays)
        {
            var combinedOverlay = new ProvisioningOverlay();
            foreach (byte[] overlayBytes in overlays)
            {
                combinedOverlay.MergeFrom(ParseProvisioningOverlayCore(overlayBytes));
            }

            return new ProvisioningOverlayData(
                combinedOverlay.Directories.OrderBy(static directory => directory, StringComparer.OrdinalIgnoreCase).ToArray(),
                new Dictionary<string, string>(combinedOverlay.FileMappings, StringComparer.OrdinalIgnoreCase));
        }

        private ProvisioningOverlay ParseProvisioningOverlay(byte[] bytes)
            => ParseProvisioningOverlayCore(bytes);

        private static ProvisioningOverlay ParseProvisioningOverlayCore(byte[] bytes)
        {
            string text = DecodeProvisioningText(bytes);
            var overlay = new ProvisioningOverlay();

            foreach (string rawLine in text.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
            {
                string line = rawLine.Trim().Trim('\0');
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith(';') || line.StartsWith('#'))
                {
                    continue;
                }

                string currentDirectory = string.Empty;
                foreach (string segment in SplitProvisioningSegments(line))
                {
                    if (segment.Equals("root", StringComparison.OrdinalIgnoreCase))
                    {
                        currentDirectory = string.Empty;
                        continue;
                    }

                    if (!TryParseProvisioningCall(segment, out string? command, out IReadOnlyList<string>? arguments))
                    {
                        continue;
                    }

                    if (command.Equals("Directory", StringComparison.OrdinalIgnoreCase) && arguments.Count >= 1)
                    {
                        currentDirectory = ResolveProvisioningPath(currentDirectory, TrimTrailingDirectorySeparators(arguments[0]));
                        if (!string.IsNullOrEmpty(currentDirectory))
                        {
                            overlay.Directories.Add(currentDirectory);
                        }
                    }
                    else if (command.Equals("File", StringComparison.OrdinalIgnoreCase) && arguments.Count >= 2)
                    {
                        string targetPath = ResolveProvisioningPath(currentDirectory, arguments[0]);
                        string sourcePath = NormalizePath(arguments[1]);
                        if (!string.IsNullOrEmpty(targetPath) && !string.IsNullOrEmpty(sourcePath))
                        {
                            overlay.Directories.Add(Path.GetDirectoryName(targetPath) ?? string.Empty);
                            overlay.FileMappings[targetPath] = sourcePath;
                        }
                    }
                }
            }

            overlay.Directories.Remove(string.Empty);
            return overlay;
        }

        private static string DecodeProvisioningText(byte[] bytes)
        {
            if (bytes.Length >= 2)
            {
                if (bytes[0] == 0xFF && bytes[1] == 0xFE)
                {
                    return Encoding.Unicode.GetString(bytes, 2, bytes.Length - 2);
                }

                if (bytes[0] == 0xFE && bytes[1] == 0xFF)
                {
                    return Encoding.BigEndianUnicode.GetString(bytes, 2, bytes.Length - 2);
                }
            }

            if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
            {
                return Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);
            }

            int zeroByteCount = bytes.Count(static b => b == 0);
            if (zeroByteCount > (bytes.Length / 4))
            {
                return Encoding.Unicode.GetString(bytes);
            }

            return Encoding.UTF8.GetString(bytes);
        }

        private static IEnumerable<string> SplitProvisioningSegments(string line)
        {
            var segments = new List<string>();
            bool inQuotes = false;
            int segmentStart = 0;

            for (int i = 0; i < line.Length - 1; i++)
            {
                char current = line[i];
                if (current == '"' && !IsProvisioningQuoteEscaped(line, i))
                {
                    inQuotes = !inQuotes;
                }

                if (!inQuotes && current == ':' && line[i + 1] == '-')
                {
                    segments.Add(line[segmentStart..i].Trim());
                    segmentStart = i + 2;
                    i++;
                }
            }

            segments.Add(line[segmentStart..].Trim());
            return segments.Where(static segment => !string.IsNullOrWhiteSpace(segment));
        }

        private static bool TryParseProvisioningCall(string segment, out string command, out IReadOnlyList<string> arguments)
        {
            command = string.Empty;
            arguments = Array.Empty<string>();

            int openParen = segment.IndexOf('(');
            int closeParen = segment.LastIndexOf(')');
            if (openParen <= 0 || closeParen <= openParen)
            {
                return false;
            }

            command = segment[..openParen].Trim();
            string argumentList = segment[(openParen + 1)..closeParen];
            arguments = ParseProvisioningArguments(argumentList);
            return !string.IsNullOrWhiteSpace(command);
        }

        private static IReadOnlyList<string> ParseProvisioningArguments(string argumentList)
        {
            var arguments = new List<string>();
            var current = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < argumentList.Length; i++)
            {
                char currentChar = argumentList[i];
                if (currentChar == '"' && !IsProvisioningQuoteEscaped(argumentList, i))
                {
                    inQuotes = !inQuotes;
                    continue;
                }

                if (!inQuotes && currentChar == ',')
                {
                    arguments.Add(DecodeProvisioningEscapes(current.ToString().Trim()));
                    current.Clear();
                    continue;
                }

                current.Append(currentChar);
            }

            if (current.Length > 0)
            {
                arguments.Add(DecodeProvisioningEscapes(current.ToString().Trim()));
            }

            return arguments;
        }

        private static bool IsProvisioningQuoteEscaped(string value, int quoteIndex)
        {
            int backslashCount = 0;
            for (int index = quoteIndex - 1; index >= 0 && value[index] == '\\'; index--)
            {
                backslashCount++;
            }

            if ((backslashCount % 2) == 0)
            {
                return false;
            }

            for (int index = quoteIndex + 1; index < value.Length; index++)
            {
                char current = value[index];
                if (char.IsWhiteSpace(current))
                {
                    continue;
                }

                return current != ')' && current != ',' && current != ':';
            }

            return false;
        }

        private static string DecodeProvisioningEscapes(string value)
        {
            if (string.IsNullOrEmpty(value) || value.IndexOf('\\') < 0)
            {
                return value;
            }

            var decoded = new StringBuilder(value.Length);
            for (int index = 0; index < value.Length; index++)
            {
                char current = value[index];
                if (current != '\\' || index == value.Length - 1)
                {
                    decoded.Append(current);
                    continue;
                }

                char next = value[index + 1];
                if (next == '"' || next == '\\')
                {
                    decoded.Append(next);
                    index++;
                    continue;
                }

                if ((next == 'x' || next == 'X') && index + 5 < value.Length)
                {
                    ReadOnlySpan<char> hexDigits = value.AsSpan(index + 2, 4);
                    if (ushort.TryParse(hexDigits, System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.InvariantCulture, out ushort codeUnit))
                    {
                        decoded.Append((char)codeUnit);
                        index += 5;
                        continue;
                    }
                }

                decoded.Append(current);
            }

            return decoded.ToString();
        }

        private static string ResolveProvisioningPath(string currentDirectory, string value)
        {
            string normalizedValue = NormalizePath(value);
            if (string.IsNullOrEmpty(normalizedValue))
            {
                return currentDirectory;
            }

            if (StartsWithDirectorySeparator(value))
            {
                return normalizedValue;
            }

            return string.IsNullOrEmpty(currentDirectory)
                ? normalizedValue
                : NormalizePath(Path.Combine(currentDirectory, normalizedValue));
        }

        private static string TrimTrailingDirectorySeparators(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.TrimEnd('\\', '/');
        }

        private static bool TryResolveProvisionedSource(string sourcePath, IReadOnlyDictionary<string, ImgfsFileRecord> filesByPath, IReadOnlyDictionary<string, ImgfsFileRecord> filesByName, out ImgfsFileRecord? file)
        {
            if (filesByPath.TryGetValue(sourcePath, out ImgfsFileRecord? exactMatch))
            {
                file = exactMatch;
                return true;
            }

            string fileName = Path.GetFileName(sourcePath);
            if (!string.IsNullOrEmpty(fileName) && filesByName.TryGetValue(fileName, out ImgfsFileRecord? nameMatch))
            {
                file = nameMatch;
                return true;
            }

            file = null;
            return false;
        }

        private string ResolveProvisionedBasePath(string rawPath, bool defaultFlatFilesToWindowsDirectory)
        {
            string normalizedPath = NormalizePath(rawPath);
            if (!defaultFlatFilesToWindowsDirectory || string.IsNullOrEmpty(normalizedPath) || ContainsPathSeparator(normalizedPath))
            {
                return normalizedPath;
            }

            return NormalizePath(Path.Combine("Windows", normalizedPath));
        }

        private void AddFileToTree(string fullPath, ImgfsFileRecord file)
        {
            string[] pathParts = fullPath.Split(['\\', '/'], StringSplitOptions.RemoveEmptyEntries);
            if (pathParts.Length == 0)
            {
                return;
            }

            ImgfsDirectory currentDirectory = _rootDirectory;
            for (int i = 0; i < pathParts.Length - 1; i++)
            {
                currentDirectory = currentDirectory.GetOrCreateSubdirectory(pathParts[i]);
            }

            currentDirectory.AddFile(new ImgfsFile(this, currentDirectory, pathParts[^1], file));
        }

        private ImgfsDirectory EnsureDirectory(string fullPath)
        {
            string[] pathParts = fullPath.Split(['\\', '/'], StringSplitOptions.RemoveEmptyEntries);
            ImgfsDirectory currentDirectory = _rootDirectory;
            foreach (string pathPart in pathParts)
            {
                currentDirectory = currentDirectory.GetOrCreateSubdirectory(pathPart);
            }

            return currentDirectory;
        }

        private IEnumerable<long> EnumerateDirectoryBlockOffsets()
        {
            long directoryBlockOffset = _header.BytesPerBlock;
            HashSet<long> visitedOffsets = [];

            while (directoryBlockOffset != 0)
            {
                if (directoryBlockOffset < _header.BytesPerBlock || directoryBlockOffset + 8 > _stream.Length)
                {
                    throw new InvalidDataException($"IMGFS directory block pointer is outside the image: 0x{directoryBlockOffset:X8}.");
                }

                if (!visitedOffsets.Add(directoryBlockOffset))
                {
                    throw new InvalidDataException($"IMGFS directory block chain loops back to 0x{directoryBlockOffset:X8}.");
                }

                byte[] directoryHeader = ReadBytes(directoryBlockOffset, 8);
                uint magic = BinaryPrimitives.ReadUInt32LittleEndian(directoryHeader.AsSpan(0, 4));
                if (magic != DirectoryBlockMagic)
                {
                    throw new InvalidDataException($"Invalid IMGFS directory block magic 0x{magic:X8} at 0x{directoryBlockOffset:X8}.");
                }

                yield return directoryBlockOffset;
                directoryBlockOffset = BinaryPrimitives.ReadUInt32LittleEndian(directoryHeader.AsSpan(4, 4));
            }
        }

        private ulong CalculateStoredLength(uint indexPointer, uint indexSize)
        {
            ulong total = indexPointer == 0 || indexSize == 0 ? 0UL : indexSize;

            foreach (ImgfsChunkIndexRecord chunk in ReadChunkIndex(indexPointer, indexSize))
            {
                total += (ulong)RoundToChunk(chunk.CompressedSize);
            }

            return total;
        }

        private IEnumerable<ImgfsChunkIndexRecord> ReadChunkIndex(uint indexPointer, uint indexSize)
        {
            if (indexPointer == 0 || indexSize == 0)
            {
                yield break;
            }

            byte[] indexBytes = ReadBytes(indexPointer, checked((int)indexSize));
            for (int offset = 0; offset + 8 <= indexBytes.Length; offset += 8)
            {
                ReadOnlySpan<byte> entry = indexBytes.AsSpan(offset, 8);
                ushort compressedSize = BinaryPrimitives.ReadUInt16LittleEndian(entry[0..2]);
                ushort fullSize = BinaryPrimitives.ReadUInt16LittleEndian(entry[2..4]);
                uint dataPointer = BinaryPrimitives.ReadUInt32LittleEndian(entry[4..8]);

                if (compressedSize == 0 || fullSize == 0 || dataPointer == 0)
                {
                    continue;
                }

                yield return new ImgfsChunkIndexRecord(compressedSize, fullSize, dataPointer);
            }
        }

        private string ReadName(ReadOnlySpan<byte> nameInfo)
        {
            ushort characterCount = BinaryPrimitives.ReadUInt16LittleEndian(nameInfo[0..2]);
            ushort flags = BinaryPrimitives.ReadUInt16LittleEndian(nameInfo[2..4]);

            if (characterCount == 0)
            {
                return string.Empty;
            }

            if (characterCount <= NameStoredInDirectoryEntryCharacterLimit)
            {
                int byteCount = characterCount * sizeof(char);
                return DecodeUtf16(nameInfo.Slice(4, byteCount));
            }

            uint pointer = BinaryPrimitives.ReadUInt32LittleEndian(nameInfo[8..12]);
            if (pointer == 0)
            {
                return string.Empty;
            }

            if ((flags & NameIsStoredInNameEntryFlag) != 0)
            {
                if (_nameCache.TryGetValue(pointer, out string? cachedName))
                {
                    return cachedName;
                }

                byte[] nameEntryBytes = ReadBytes(pointer, _header.DirectoryEntrySize);
                uint magic = BinaryPrimitives.ReadUInt32LittleEndian(nameEntryBytes.AsSpan(0, 4));
                if (magic != NameEntryMagic)
                {
                    throw new InvalidDataException($"Invalid IMGFS name entry magic 0x{magic:X8} at 0x{pointer:X8}.");
                }

                int clampedCharacters = Math.Min(characterCount, NameStoredInNameEntryCharacterLimit);
                string name = DecodeUtf16(nameEntryBytes.AsSpan(4, clampedCharacters * sizeof(char)));
                _nameCache[pointer] = name;
                return name;
            }

            string chunkName = DecodeUtf16(ReadBytes(pointer, checked(characterCount * sizeof(char))));
            _nameCache[pointer] = chunkName;
            return chunkName;
        }

        private MemoryStream OpenFileContents(ImgfsFileRecord file)
        {
            if (file.IsModule || file.SectionListOffset != 0)
            {
                return OpenModuleContents(file);
            }

            if (file.Size == 0)
            {
                return new MemoryStream(Array.Empty<byte>(), writable: false);
            }

            var output = new MemoryStream(file.Size <= int.MaxValue ? (int)file.Size : 0);
            foreach (ImgfsChunkIndexRecord chunk in ReadChunkIndex(file.IndexPointer, file.IndexSize))
            {
                if (chunk.CompressedSize > chunk.FullSize)
                {
                    throw new InvalidDataException($"IMGFS chunk at 0x{chunk.DataPointer:X8} reports a compressed size larger than its expanded size.");
                }

                byte[] chunkBytes = ReadBytes(chunk.DataPointer, chunk.CompressedSize);
                if (chunk.CompressedSize < chunk.FullSize)
                {
                    chunkBytes = DecompressChunk(chunkBytes, chunk.FullSize);
                }

                output.Write(chunkBytes, 0, chunkBytes.Length);
            }

            if (output.Length < file.Size)
            {
                throw new InvalidDataException($"IMGFS file '{file.FullPath}' reconstructed to {output.Length} bytes but the directory entry reports {file.Size} bytes.");
            }

            if (output.Length > file.Size)
            {
                output.SetLength(file.Size);
            }

            output.Position = 0;
            return output;
        }

        private MemoryStream OpenModuleContents(ImgfsFileRecord file)
        {
            byte[] romHeaderData = ReadIndexedData(file.IndexPointer, file.IndexSize, file.Size, file.FullPath);
            IReadOnlyList<CEImageModulePeReconstructor.ModuleSectionData> sections = ReadModuleSections(file.SectionListOffset);
            byte[] reconstructedImage = CEImageModulePeReconstructor.Reconstruct(romHeaderData, sections, _moduleMachineType);
            return new MemoryStream(reconstructedImage, writable: false);
        }

        private byte[] ReadIndexedData(uint indexPointer, uint indexSize, uint expectedSize, string pathForErrors)
        {
            if (expectedSize == 0)
            {
                return Array.Empty<byte>();
            }

            var output = new MemoryStream(expectedSize <= int.MaxValue ? (int)expectedSize : 0);
            foreach (ImgfsChunkIndexRecord chunk in ReadChunkIndex(indexPointer, indexSize))
            {
                if (chunk.CompressedSize > chunk.FullSize)
                {
                    throw new InvalidDataException($"IMGFS chunk at 0x{chunk.DataPointer:X8} reports a compressed size larger than its expanded size.");
                }

                byte[] chunkBytes = ReadBytes(chunk.DataPointer, chunk.CompressedSize);
                if (chunk.CompressedSize < chunk.FullSize)
                {
                    chunkBytes = DecompressChunk(chunkBytes, chunk.FullSize);
                }

                output.Write(chunkBytes, 0, chunkBytes.Length);
            }

            if (output.Length < expectedSize)
            {
                throw new InvalidDataException($"IMGFS file '{pathForErrors}' reconstructed to {output.Length} bytes but the directory entry reports {expectedSize} bytes.");
            }

            if (output.Length > expectedSize)
            {
                output.SetLength(expectedSize);
            }

            return output.ToArray();
        }

        private IReadOnlyList<CEImageModulePeReconstructor.ModuleSectionData> ReadModuleSections(uint sectionListOffset)
        {
            var sections = new List<CEImageModulePeReconstructor.ModuleSectionData>();
            var visitedOffsets = new HashSet<uint>();
            uint currentOffset = sectionListOffset;
            while (currentOffset != 0)
            {
                if (!visitedOffsets.Add(currentOffset))
                {
                    throw new InvalidDataException($"IMGFS module section chain loops back to 0x{currentOffset:X8}.");
                }

                byte[] entryBytes = ReadBytes(currentOffset, _header.DirectoryEntrySize);
                ReadOnlySpan<byte> entry = entryBytes;
                uint magic = BinaryPrimitives.ReadUInt32LittleEndian(entry[0..4]);
                if (magic != SectionEntryMagic)
                {
                    throw new InvalidDataException($"Invalid IMGFS module section entry magic 0x{magic:X8} at 0x{currentOffset:X8}.");
                }

                string sectionName = ReadName(entry[12..24]);
                uint sectionSize = BinaryPrimitives.ReadUInt32LittleEndian(entry[24..28]);
                uint indexPointer = BinaryPrimitives.ReadUInt32LittleEndian(entry[28..32]);
                uint indexSize = BinaryPrimitives.ReadUInt32LittleEndian(entry[32..36]);
                sections.Add(new CEImageModulePeReconstructor.ModuleSectionData(
                    sectionName,
                    ReadIndexedData(indexPointer, indexSize, sectionSize, sectionName)));

                currentOffset = BinaryPrimitives.ReadUInt32LittleEndian(entry[8..12]);
            }

            return sections;
        }

        private byte[] ReadBytes(long offset, int length)
        {
            if (offset < 0 || length < 0 || offset + length > _stream.Length)
            {
                throw new InvalidDataException($"Attempted to read IMGFS data outside the stream bounds (offset=0x{offset:X8}, length=0x{length:X8}).");
            }

            byte[] buffer = new byte[length];
            _stream.Seek(offset, SeekOrigin.Begin);
            _stream.ReadExactly(buffer);
            return buffer;
        }

        private int RoundToChunk(int value)
        {
            if (value == 0)
            {
                return 0;
            }

            int chunkSize = _header.BytesPerChunk;
            return ((value - 1) | (chunkSize - 1)) + 1;
        }

        private byte[] DecompressChunk(byte[] compressedData, int fullSize)
        {
            return _header.CompressionType switch
            {
                ImgfsHeader.CompressionXpr => XpressDecoder.Decompress(compressedData, fullSize, XpressKind.PlainLz77),
                ImgfsHeader.CompressionLzx => throw new NotSupportedException("IMGFS LZX decompression is not implemented in TotalImage yet."),
                ImgfsHeader.CompressionXph => throw new NotSupportedException("IMGFS XPH decompression is not implemented in TotalImage yet."),
                _ => throw new NotSupportedException($"Unsupported IMGFS compression type {_header.CompressionName}.")
            };
        }

        private static DateTime? ConvertFileTime(long fileTime)
        {
            if (fileTime <= 0)
            {
                return null;
            }

            try
            {
                return DateTime.FromFileTimeUtc(fileTime);
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        private static string DecodeUtf16(ReadOnlySpan<byte> bytes) => Encoding.Unicode.GetString(bytes).TrimEnd('\0');

        private static string NormalizePath(string path)
        {
            string[] parts = path
                .Split(['\\', '/'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            return parts.Length == 0 ? string.Empty : string.Join('\\', parts);
        }

        private static bool StartsWithDirectorySeparator(string value) => value.StartsWith('\\') || value.StartsWith('/');

        private static bool ContainsPathSeparator(string value) => value.Contains('\\') || value.Contains('/');

        /// <summary>
        /// Lightweight view over the fixed IMGFS header fields used by this reader.
        /// </summary>
        private sealed class ImgfsHeader
        {
            public const uint CompressionXpr = 0x00525058;
            public const uint CompressionLzx = 0x00585A4C;
            public const uint CompressionXph = 0x00485058;

            private ImgfsHeader(int directoryEntrySize, int chunksPerBlock, int bytesPerBlock, uint compressionType, uint freeSectorCount, uint hiddenSectorCount)
            {
                DirectoryEntrySize = directoryEntrySize;
                ChunksPerBlock = chunksPerBlock;
                BytesPerBlock = bytesPerBlock;
                CompressionType = compressionType;
                FreeSectorCount = freeSectorCount;
                HiddenSectorCount = hiddenSectorCount;

                if (chunksPerBlock <= 0 || bytesPerBlock <= 0 || (bytesPerBlock % chunksPerBlock) != 0)
                {
                    throw new InvalidDataException("IMGFS header reports an invalid block/chunk geometry.");
                }

                BytesPerChunk = bytesPerBlock / chunksPerBlock;
            }

            /// <summary>
            /// Size of one on-disk directory entry.
            /// </summary>
            public int DirectoryEntrySize { get; }

            /// <summary>
            /// Number of data chunks contained in a block.
            /// </summary>
            public int ChunksPerBlock { get; }

            /// <summary>
            /// Size of one IMGFS block.
            /// </summary>
            public int BytesPerBlock { get; }

            /// <summary>
            /// Size of one IMGFS allocation chunk.
            /// </summary>
            public int BytesPerChunk { get; }

            /// <summary>
            /// Raw compression identifier from the header.
            /// </summary>
            public uint CompressionType { get; }

            /// <summary>
            /// Human-readable compression name used in exceptions and diagnostics.
            /// </summary>
            public string CompressionName => CompressionType switch
            {
                CompressionXpr => "XPR",
                CompressionLzx => "LZX",
                CompressionXph => "XPH",
                _ => $"0x{CompressionType:X8}"
            };

            /// <summary>
            /// Free sector count reported by the header.
            /// </summary>
            public uint FreeSectorCount { get; }

            /// <summary>
            /// Hidden sector count reported by the header.
            /// </summary>
            public uint HiddenSectorCount { get; }

            public static ImgfsHeader Read(Stream stream)
            {
                if (stream.Length < 0x38)
                {
                    throw new InvalidDataException("Stream is too small to contain an IMGFS header.");
                }

                stream.Seek(0, SeekOrigin.Begin);
                Span<byte> signature = stackalloc byte[16];
                stream.ReadExactly(signature);
                if (!signature.SequenceEqual(ImgfsSignature))
                {
                    throw new InvalidDataException("Stream does not start with an IMGFS signature.");
                }

                stream.Seek(0x1C, SeekOrigin.Begin);
                Span<byte> fields = stackalloc byte[0x1C];
                stream.ReadExactly(fields);

                int directoryEntrySize = checked((int)BinaryPrimitives.ReadUInt32LittleEndian(fields[0..4]));
                int chunksPerBlock = checked((int)BinaryPrimitives.ReadUInt32LittleEndian(fields[4..8]));
                int bytesPerBlock = checked((int)BinaryPrimitives.ReadUInt32LittleEndian(fields[8..12]));
                uint compressionType = BinaryPrimitives.ReadUInt32LittleEndian(fields[16..20]);
                uint freeSectorCount = BinaryPrimitives.ReadUInt32LittleEndian(fields[20..24]);
                uint hiddenSectorCount = BinaryPrimitives.ReadUInt32LittleEndian(fields[24..28]);

                if (directoryEntrySize != SupportedDirectoryEntrySize)
                {
                    throw new InvalidDataException($"Unsupported IMGFS directory entry size 0x{directoryEntrySize:X8}. Only 0x{SupportedDirectoryEntrySize:X2} is currently supported.");
                }

                return new ImgfsHeader(directoryEntrySize, chunksPerBlock, bytesPerBlock, compressionType, freeSectorCount, hiddenSectorCount);
            }
        }

        /// <summary>
        /// Parsed metadata for a file entry in IMGFS.
        /// </summary>
        private sealed record ImgfsFileRecord(
            string FullPath,
            uint Size,
            FileAttributes Attributes,
            DateTime? FileTime,
            uint IndexPointer,
            uint IndexSize,
            uint SectionListOffset,
            bool IsModule,
            ulong StoredLength);

        /// <summary>
        /// Directory and file mappings described by Windows CE DAT provisioning files.
        /// </summary>
        private sealed class ProvisioningOverlay
        {
            public HashSet<string> Directories { get; } = new(StringComparer.OrdinalIgnoreCase);

            public Dictionary<string, string> FileMappings { get; } = new(StringComparer.OrdinalIgnoreCase);

            public bool IsEmpty => Directories.Count == 0 && FileMappings.Count == 0;

            public bool ReferencesWindowsDirectory => Directories.Any(static directory => directory.StartsWith("Windows", StringComparison.OrdinalIgnoreCase))
                || FileMappings.Keys.Any(static path => path.StartsWith("Windows", StringComparison.OrdinalIgnoreCase))
                || FileMappings.Values.Any(static path => path.StartsWith("Windows", StringComparison.OrdinalIgnoreCase));

            public void MergeFrom(ProvisioningOverlay overlay)
            {
                foreach (string directory in overlay.Directories)
                {
                    Directories.Add(directory);
                }

                foreach ((string targetPath, string sourcePath) in overlay.FileMappings)
                {
                    FileMappings[targetPath] = sourcePath;
                }
            }
        }

        internal sealed record ProvisioningOverlayData(
            IReadOnlyList<string> Directories,
            IReadOnlyDictionary<string, string> FileMappings);

        /// <summary>
        /// One 8-byte index entry that maps logical file data to an on-disk chunk.
        /// </summary>
        private readonly record struct ImgfsChunkIndexRecord(int CompressedSize, int FullSize, uint DataPointer);

        /// <summary>
        /// Root directory node for the IMGFS hierarchy exposed to the rest of the application.
        /// </summary>
        private sealed class ImgfsRootDirectory : ImgfsDirectory
        {
            public ImgfsRootDirectory(CEImageFileSystem fileSystem) : base(fileSystem, null, string.Empty)
            {
            }
        }

        /// <summary>
        /// Directory adapter that exposes IMGFS paths through the generic file system object model.
        /// </summary>
        private class ImgfsDirectory : DirectoryBase
        {
            private readonly string _name;
            private readonly List<FileSystemObject> _children = [];
            private readonly Dictionary<string, ImgfsDirectory> _subdirectories = new(StringComparer.OrdinalIgnoreCase);

            public ImgfsDirectory(CEImageFileSystem fileSystem, DirectoryBase? parent, string name) : base(fileSystem, parent)
            {
                _name = name;
            }

            public override string Name
            {
                get => _name;
                set => throw new NotSupportedException("IMGFS is currently exposed as read-only.");
            }

            public override FileAttributes Attributes
            {
                get => FileAttributes.Directory;
                set => throw new NotSupportedException("IMGFS is currently exposed as read-only.");
            }

            public override DateTime? LastAccessTime
            {
                get => null;
                set => throw new NotSupportedException("IMGFS is currently exposed as read-only.");
            }

            public override DateTime? LastWriteTime
            {
                get => null;
                set => throw new NotSupportedException("IMGFS is currently exposed as read-only.");
            }

            public override DateTime? CreationTime
            {
                get => null;
                set => throw new NotSupportedException("IMGFS is currently exposed as read-only.");
            }

            public override ulong Length
            {
                get => 0;
                set => throw new NotSupportedException("IMGFS is currently exposed as read-only.");
            }

            public ImgfsDirectory GetOrCreateSubdirectory(string name)
            {
                if (_subdirectories.TryGetValue(name, out ImgfsDirectory? directory))
                {
                    return directory;
                }

                directory = new ImgfsDirectory((CEImageFileSystem)FileSystem, this, name);
                _subdirectories.Add(name, directory);
                _children.Add(directory);
                return directory;
            }

            public void AddFile(ImgfsFile file) => _children.Add(file);

            public override IEnumerable<FileSystemObject> EnumerateFileSystemObjects(bool showHidden)
            {
                return _children
                    .Where(child => showHidden || !child.Attributes.HasFlag(FileAttributes.Hidden))
                    .OrderBy(static child => child is FileBase ? 1 : 0)
                    .ThenBy(static child => child.Name, StringComparer.OrdinalIgnoreCase);
            }

            public override DirectoryBase CreateSubdirectory(string path)
            {
                throw new NotSupportedException("IMGFS is currently exposed as read-only.");
            }

            public override void Delete()
            {
                throw new NotSupportedException("IMGFS is currently exposed as read-only.");
            }

            public override void MoveTo(string path)
            {
                throw new NotSupportedException("IMGFS is currently exposed as read-only.");
            }
        }

        /// <summary>
        /// File adapter that exposes IMGFS file entries through the generic file system object model.
        /// </summary>
        private sealed class ImgfsFile : FileBase
        {
            private readonly CEImageFileSystem _fileSystem;
            private readonly string _name;
            private readonly ImgfsFileRecord _record;

            public ImgfsFile(CEImageFileSystem fileSystem, DirectoryBase directory, string name, ImgfsFileRecord record) : base(fileSystem, directory)
            {
                _fileSystem = fileSystem;
                _name = name;
                _record = record;
            }

            public override string Name
            {
                get => _name;
                set => throw new NotSupportedException("IMGFS is currently exposed as read-only.");
            }

            public override FileAttributes Attributes
            {
                get => _record.Attributes;
                set => throw new NotSupportedException("IMGFS is currently exposed as read-only.");
            }

            public override DateTime? LastAccessTime
            {
                get => _record.FileTime;
                set => throw new NotSupportedException("IMGFS is currently exposed as read-only.");
            }

            public override DateTime? LastWriteTime
            {
                get => _record.FileTime;
                set => throw new NotSupportedException("IMGFS is currently exposed as read-only.");
            }

            public override DateTime? CreationTime
            {
                get => _record.FileTime;
                set => throw new NotSupportedException("IMGFS is currently exposed as read-only.");
            }

            public override ulong Length
            {
                get => _record.Size;
                set => throw new NotSupportedException("IMGFS is currently exposed as read-only.");
            }

            public override ulong LengthOnDisk => _record.StoredLength == 0 ? base.LengthOnDisk : _record.StoredLength;

            public override void Delete()
            {
                throw new NotSupportedException("IMGFS is currently exposed as read-only.");
            }

            public override Stream GetStream() => _fileSystem.OpenFileContents(_record);

            public override void MoveTo(string path)
            {
                throw new NotSupportedException("IMGFS is currently exposed as read-only.");
            }
        }
    }
}
