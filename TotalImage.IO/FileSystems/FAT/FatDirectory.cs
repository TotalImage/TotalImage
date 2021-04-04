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
        private DirectoryEntry entry;
        private DirectoryEntry[]? lfnEntries;

        public FatDirectory(Fat12 fat, DirectoryEntry entry, DirectoryEntry[]? lfnEntries, Directory parent) : base(fat, parent)
        {
            this.entry = entry;
            this.lfnEntries = lfnEntries;
        }

        /// <inheritdoc />
        public string ShortName
        {
            get => Helper.TrimFileName(Encoding.ASCII.GetString(entry.name));
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string? LongName
        {
            get => lfnEntries != null ? Helper.RetrieveLongName(lfnEntries) : null;
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
            get => (FileAttributes)entry.attr;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override DateTime? LastAccessTime
        {
            get => Helper.FatToDateTime(entry.lstAccDate);
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override DateTime? LastWriteTime
        {
            get => Helper.FatToDateTime(entry.wrtDate, entry.wrtTime);
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override DateTime? CreationTime
        {
            get => Helper.FatToDateTime(entry.crtDate, entry.crtTime, entry.crtTimeTenth);
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override ulong Length
        {
            get => entry.fileSize;
            set => throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override Directory CreateSubdirectory(string path)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override void Delete()
        {
            /* When deleting a directory, the first character of the name needs to be changed to 0xE5.
            * The directory's directory entry can then be reused, and its clusters are marked as free until they're
            * overwritten. The same must then be done for all files and subdirectories inside.
            * This code is untested until this class is hooked up to the UI... */

            entry.name[0] = 0xE5;

            //And then mark all clusters in the chain as free, and do the same for all files and subdirectories inside.
        }

        /// <inheritdoc />
        public override IEnumerable<FileSystemObject> EnumerateFileSystemObjects(bool showHidden, bool showDeleted)
        {
            if (!(FileSystem is Fat12 fat))
            {
                throw new NotSupportedException("Only FAT12 is supported at the moment");
            }

            var lfn = new Stack<DirectoryEntry>();
            var useLfn = false;

            foreach (var entry in DirectoryEntry.ReadSubdirectory(fat, entry, showDeleted))
            {
                if (entry.attr == FatAttributes.LongName)
                {
                    if (entry.name[0] == 0xE5)
                    {
                        // This is a deleted LFN entry
                        useLfn = false;
                    }
                    else if (Convert.ToBoolean(entry.name[0] & 0x40))
                    {
                        // This is the first LFN entry
                        useLfn = true;
                        lfn.Clear();
                        lfn.Push(entry);
                    }
                    else if (useLfn)
                    {
                        if ((lfn.Peek().name[0] & 0x1F) != (entry.name[0] & 0x1F) + 1)
                        {
                            // The LFN entry is out of order
                            useLfn = false;
                        }
                        else if (lfn.Peek().crtTimeTenth != entry.crtTimeTenth)
                        {
                            // Short name checksum is different from the last entry
                            useLfn = false;
                        }
                        else
                        {
                            lfn.Push(entry);
                        }
                    }

                    continue;
                }
                else if (useLfn)
                {
                    // We reached a non-LFN entry, so let's see if we retrieved
                    // a valid long file name.

                    if ((lfn.Peek().name[0] & 0x1F) != 0x01)
                    {
                        // The top LFN entry is not the logically first entry
                        useLfn = false;
                    }
                    else if (lfn.Peek().crtTimeTenth != Helper.LfnChecksum(entry.name))
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