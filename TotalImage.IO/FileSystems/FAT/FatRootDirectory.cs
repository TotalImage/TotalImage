using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TotalImage.FileSystems.FAT
{
    /*
     * FAT12/FAT16/FAT32 root directory class. Implements root dir enumeration
     *
     * The root directory is a special directory in the FAT filesystem, as it is
     * present in a well-known location, as opposed to being referred to from an
     * entry in the parent directory.
     *
     * Separate class for now, aim to integrate this into FatDirectory.cs
     */
    public class FatRootDirectory : Directory, IFatFileSystemObject
    {
        public FatRootDirectory(Fat12 fat) : base(fat, null) { }

        /// <inheritdoc />
        public override IEnumerable<FileSystemObject> EnumerateFileSystemObjects(bool showHidden, bool showDeleted)
        {
            if (!(FileSystem is Fat12 fat))
            {
                throw new NotSupportedException("Only FAT12 is supported at the moment");
            }

            //Read the entries top to bottom
            foreach(var entry in DirectoryEntry.ReadRootDirectory(fat, showDeleted))
            {
                //Skip LFN and volume label entries for now
                if (entry.attr.HasFlag(FatAttributes.VolumeId))
                {
                    continue;
                }
                //Skip hidden files unless showHidden is true
                else if (entry.attr.HasFlag(FatAttributes.Hidden) && !showHidden)
                {
                    continue;
                }
                //Folder entry
                else if (entry.attr.HasFlag(FatAttributes.Subdirectory))
                {
                    yield return new FatDirectory(fat, entry, this);
                }
                //File entry
                else
                {
                    yield return new FatFile(fat, entry, this);
                }
            }
        }

        /// <inheritdoc />
        public override Directory CreateSubdirectory(string path)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override void Delete()
            => throw new InvalidOperationException();

        /// <inheritdoc />
        public override void MoveTo(string path)
            => throw new InvalidOperationException();

        /// <inheritdoc />
        public string ShortName
        {
            get => string.Empty;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string? LongName
        {
            get => null;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override string Name
        {
            get => LongName ?? ShortName;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override FileAttributes Attributes
        {
            get => FileAttributes.Directory;
            set => throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override DateTime? LastAccessTime
        {
            get => null;
            set => throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override DateTime? LastWriteTime
        {
            get => null;
            set => throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override DateTime? CreationTime
        {
            get => null;
            set => throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override ulong Length
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        //Checks if an entry with the specified name already exists in the root directory
        public bool EntryExists(string fullname)
        {
            if (string.IsNullOrEmpty(fullname))
                throw new ArgumentNullException(nameof(fullname), "fullname cannot be null!");

            string name = fullname.Substring(0, fullname.IndexOf('.')).PadRight(8, ' ');
            string ext = fullname.Substring(fullname.IndexOf('.'), fullname.Length - 1).PadRight(3, ' ');

            return false; //Bogus, needs to actually check all the entries which aren't in this class yet...
        }

        //Returns whether the root directory is full
        public bool IsFull()
        {
            /* Bogus, this needs to actually check the number of normal (non-deleted) entries in the root directory and compare it
             * to the value in the BPB/floppyTable, then return the result... */
            return false;
        }
    }
}