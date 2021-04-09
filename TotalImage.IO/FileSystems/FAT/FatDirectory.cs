using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TotalImage.FileSystems.FAT
{
    /*
     * FAT12/FAT16/FAT32 directory class. Implements directory entry enumeration.
     */
    public class FatDirectory : Directory, IFatFileSystemObject
    {
        private DirectoryEntry? entry = null;
        private LongDirectoryEntry[]? lfnEntries = null;

        public FatDirectory(FatFileSystem fat) : base(fat, null)
        {
            
        }

        public FatDirectory(FatFileSystem fat, DirectoryEntry entry, LongDirectoryEntry[]? lfnEntries, Directory parent) : base(fat, parent)
        {
            this.entry = entry;
            this.lfnEntries = lfnEntries;
        }

        /// <inheritdoc />
        public string ShortName
        {
            get => entry.HasValue ? Helper.TrimFileName(Encoding.ASCII.GetString(entry.Value.name)) : string.Empty;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string? LongName
        {
            get => lfnEntries != null ? LongDirectoryEntry.CombineEntries(lfnEntries) : null;
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
            get => entry.HasValue ? (FileAttributes)entry.Value.attr : FileAttributes.Directory;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override DateTime? LastAccessTime
        {
            get => entry.HasValue ? Helper.FatToDateTime(entry.Value.lstAccDate) : null;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override DateTime? LastWriteTime
        {
            get => entry.HasValue ? Helper.FatToDateTime(entry.Value.wrtDate, entry.Value.wrtTime) : null;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override DateTime? CreationTime
        {
            get => entry.HasValue ? Helper.FatToDateTime(entry.Value.crtDate, entry.Value.crtTime, entry.Value.crtTimeTenth) : null;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override ulong Length
        {
            get => 0;
            set => throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override Directory CreateSubdirectory(string path)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override void Delete()
        {
            throw new NotImplementedException();
            /* When deleting a directory, the first character of the name needs to be changed to 0xE5.
            * The directory's directory entry can then be reused, and its clusters are marked as free until they're
            * overwritten. The same must then be done for all files and subdirectories inside.
            * This code is untested until this class is hooked up to the UI... */

            //entry.name[0] = 0xE5;

            //And then mark all clusters in the chain as free, and do the same for all files and subdirectories inside.
        }

        /// <inheritdoc />
        public override IEnumerable<FileSystemObject> EnumerateFileSystemObjects(bool showHidden, bool showDeleted)
        {
            var fat = (FatFileSystem)FileSystem;
            var lfn = new Stack<LongDirectoryEntry>();
            var useLfn = false;

            foreach (var entry in entry.HasValue ? DirectoryEntry.ReadSubdirectory(fat, entry.Value, showDeleted) : DirectoryEntry.ReadRootDirectory(fat, showDeleted))
            {
                if (entry.attr == FatAttributes.LongName)
                {
                    var lfnEntry = (LongDirectoryEntry)entry;

                    if (lfnEntry.type != 0)
                    {
                        // Type is supposed to be zero
                        useLfn = false;
                    }
                    else if (lfnEntry.ord == 0xE5)
                    {
                        // This is a deleted LFN entry
                        useLfn = false;
                    }
                    else if (Convert.ToBoolean(lfnEntry.ord & 0x40))
                    {
                        // This is the first LFN entry
                        useLfn = true;
                        lfn.Clear();
                        lfn.Push(lfnEntry);
                    }
                    else if (useLfn)
                    {
                        if ((lfn.Peek().ord & 0x1F) != (lfnEntry.ord & 0x1F) + 1)
                        {
                            // The LFN entry is out of order
                            useLfn = false;
                        }
                        else if (lfn.Peek().chksum != lfnEntry.chksum)
                        {
                            // Short name checksum is different from the last entry
                            useLfn = false;
                        }
                        else
                        {
                            lfn.Push(lfnEntry);
                        }
                    }

                    continue;
                }
                else if (useLfn)
                {
                    // We reached a non-LFN entry, so let's see if we retrieved
                    // a valid long file name.

                    if ((lfn.Peek().ord & 0x1F) != 0x01)
                    {
                        // The top LFN entry is not the logically first entry
                        useLfn = false;
                    }
                    else if (lfn.Peek().chksum != LongDirectoryEntry.GetShortNameChecksum(entry.name))
                    {
                        // This is a valid long name for a different file!
                        useLfn = false;
                    }
                }

                //Skip volume label entries for now
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
                    yield return new FatDirectory(fat, entry, useLfn ? lfn.ToArray() : null, this);
                }
                //File entry
                else
                {
                    yield return new FatFile(fat, entry, useLfn ? lfn.ToArray() : null, this);
                }

                useLfn = false;
                lfn.Clear();
            }
        }

        /// <inheritdoc />
        public override void MoveTo(string path)
        {
            throw new NotImplementedException();
        }

        //Checks if an entry with the specified name already exists in this directory
        public bool EntryExists(string fullname)
        {
            if (string.IsNullOrEmpty(fullname))
                throw new ArgumentNullException(nameof(fullname), "fullname cannot be null!");

            string name = fullname.Substring(0, fullname.IndexOf('.')).PadRight(8, ' ');
            string ext = fullname.Substring(fullname.IndexOf('.'), fullname.Length - 1).PadRight(3, ' ');

            return false; //Bogus, needs to actually check all the entries which aren't in this class yet...
        }
    }
}