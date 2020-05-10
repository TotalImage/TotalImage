using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TotalImage.FileSystems;
using TotalImage.FileSystems.FAT;
using static Interop.Shell32;
using static Interop.User32;

namespace TotalImage
{
    public partial class dlgProperties : Form
    {
        //private FatDirEntry entry;

        private dlgProperties()
        {
            InitializeComponent();
        }

        public dlgProperties(DirectoryEntry entry) : this()
        {
            /* This is effectively the same as using CultureInfo.CurrentCulture.DateTimeFormat.UniversalSortableDateTimePattern, just without
             * the silly 'Z' at the end... */
            dateCreated.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dateModified.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dateAccessed.CustomFormat = "yyyy-MM-dd HH:mm:ss";

            /* For now both fields display the same 8.3 name, once VFAT/LFN support is added, the textbox will display
             * the long name instead */
            string filename = entry.name.TrimEnd('.');
            txtFilename.Text = filename;
            lblShortFilename1.Text = filename;
            lblSize1.Text = string.Format("{0:n0}", entry.fileSize).ToString() + " B";

            //This needs to obtain the actual cluster size from the BPB/floppyTable and use it...
            uint sizeOnDisk = (uint)Math.Ceiling(entry.fileSize / 1024.0) * 1024;
            lblSizeOnDisk1.Text = string.Format("{0:n0}", sizeOnDisk).ToString() + " B";

            ushort year = 1980;
            byte month = 1;
            byte day = 1;
            byte hours = 0;
            byte minutes = 0;
            byte seconds = 0;
            if (entry.wrtDate > 0)
            {
                year = (ushort)(((entry.wrtDate & 0xFE00) >> 9) + 1980);
                month = (byte)((entry.wrtDate & 0x1E0) >> 5);
                day = (byte)(entry.wrtDate & 0x1F);
            }
            if (entry.wrtTime > 0)
            {
                hours = (byte)((entry.wrtTime & 0xF800) >> 11);
                minutes = (byte)((entry.wrtTime & 0x7E0) >> 5);
                seconds = (byte)((entry.wrtTime & 0x1F) * 2);
            }

            DateTime modified = new DateTime(year, month, day, hours, minutes, seconds);
            dateModified.Value = modified;
            dateModified.Checked = true;

            dateAccessed.Checked = entry.lstAccDate != 0;
            dateCreated.Checked = entry.crtDate != 0;

            cbxReadOnly.Checked = Convert.ToBoolean(entry.attr & 0x01);
            cbxHidden.Checked = Convert.ToBoolean(entry.attr & 0x02);
            cbxSystem.Checked = Convert.ToBoolean(entry.attr & 0x04);
            cbxArchive.Checked = Convert.ToBoolean(entry.attr & 0x20);

            if (Convert.ToBoolean(entry.attr & 0x10))
            {
                SetIconAndType(entry.name, FileAttributes.Directory);
            }
            else
            {
                SetIconAndType(entry.name, FileAttributes.Normal);
            }
        }

        public dlgProperties(FileSystemObject entry) : this()
        {
            txtFilename.Text = entry.Name.ToUpper();
            lblShortFilename1.Text = entry.Name.ToUpper();
            lblSize1.Text = $"{entry.Length:n0} B";

            if (entry is FileSystems.File file)
                lblLocation1.Text = file.DirectoryName;
            else if (entry is FileSystems.Directory dir)
                lblLocation1.Text = dir.Parent?.Name;

            // These are indeed supposed to be assignments in the conditions.
            if (dateAccessed.Checked = entry.LastAccessTime.HasValue)
                dateAccessed.Value = entry.LastAccessTime.Value;

            if (dateModified.Checked = entry.LastWriteTime.HasValue)
                dateModified.Value = entry.LastWriteTime.Value;

            if (dateCreated.Checked = entry.CreationTime.HasValue)
                dateCreated.Value = entry.CreationTime.Value;

            if (entry.Attributes.HasFlag(FileAttributes.ReadOnly))
                cbxReadOnly.Checked = true;

            if (entry.Attributes.HasFlag(FileAttributes.Hidden))
                cbxHidden.Checked = true;

            if (entry.Attributes.HasFlag(FileAttributes.System))
                cbxSystem.Checked = true;

            if (entry.Attributes.HasFlag(FileAttributes.Archive))
                cbxArchive.Checked = true;

            SetIconAndType(entry.Name, entry.Attributes);
        }

        public void SetIconAndType(string filename, FileAttributes attributes)
        {
            var shellInfo = new SHFILEINFO();
            var flags = SHGFI.ICON | SHGFI.LARGEICON | SHGFI.TYPENAME | SHGFI.USEFILEATTRIBUTES;
            if (SHGetFileInfo(filename, attributes, ref shellInfo, (uint)Marshal.SizeOf(shellInfo), flags) != IntPtr.Zero)
            {
                using (var icon = Icon.FromHandle(shellInfo.hIcon))
                {
                    imgIcon.Image = (icon.Clone() as Icon).ToBitmap();
                }

                lblType1.Text = shellInfo.szTypeName;

                DestroyIcon(shellInfo.hIcon);
            }
        }
    }
}