using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.InteropServices;

namespace TotalImage.FileSystems.FAT
{
    /// <summary>
    /// Represents a standard boot sector for a FAT file system
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1, Size = 11)]
    public struct BootSector
    {
        public static readonly byte[] DefaultBootJump = new byte[] { 0xEB, 0x58, 0x90 };

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        private byte[] _bootJump;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        private char[] _fileSystemName;

        public IReadOnlyList<byte> BootJump => _bootJump.ToImmutableArray();

        public string FileSystemName => new string(_fileSystemName);

        /// <summary>
        /// Creates a boot sector
        /// </summary>
        /// <param name="oemId"></param>
        /// <param name="boot"></param>
        public BootSector(string oemId, byte[]? boot = null)
        {
            _bootJump = boot ?? DefaultBootJump;
            _fileSystemName = new char[] { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' };

            char[] oemArray = oemId.ToCharArray();
            Array.Copy(oemArray, _fileSystemName, Math.Min(8, oemArray.Length));
        }
    }
}
