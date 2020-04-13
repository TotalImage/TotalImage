using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
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

        protected dlgProperties()
        {
            InitializeComponent();
        }

        public dlgProperties(FatDirEntry entry) : this()
        {
            //this.entry = entry;

            dateCreated.CustomFormat = CultureInfo.CurrentCulture.DateTimeFormat.UniversalSortableDateTimePattern;
            dateModified.CustomFormat = CultureInfo.CurrentCulture.DateTimeFormat.UniversalSortableDateTimePattern;
            dateAccessed.CustomFormat = CultureInfo.CurrentCulture.DateTimeFormat.UniversalSortableDateTimePattern;

            if(entry != null)
            {
                //For now both fields display the same 8.3 filename, once VFAT/LFN support is added, the textbox will display
                //the long filename instead
                string filename = Encoding.ASCII.GetString(entry.filename).TrimEnd(' ');
                string extension = Encoding.ASCII.GetString(entry.extension).TrimEnd(' ');
                txtFilename.Text = filename.ToUpper();
                lblShortFilename1.Text = filename.ToUpper();
                if (!string.IsNullOrEmpty(extension))
                {
                    txtFilename.Text += "." + extension;
                    lblShortFilename1.Text += "." + extension;
                }

                lblSize1.Text = string.Format("{0:n0}", entry.fileSize).ToString() + " B";

                ushort year = 1980;
                byte month = 1;
                byte day = 1;
                byte hours = 0;
                byte minutes = 0;
                byte seconds = 0;
                if (entry.modifiedDate > 0)
                {
                    year = (ushort)(((entry.modifiedDate & 0xFE00) >> 9) + 1980);
                    month = (byte)((entry.modifiedDate & 0x1E0) >> 5);
                    day = (byte)(entry.modifiedDate & 0x1F);
                }
                if (entry.modifiedTime > 0)
                {
                    hours = (byte)((entry.modifiedTime & 0xF800) >> 11);
                    minutes = (byte)((entry.modifiedTime & 0x7E0) >> 5);
                    seconds = (byte)((entry.modifiedTime & 0x1F) * 2);
                }

                DateTime modified = new DateTime(year, month, day, hours, minutes, seconds);
                dateModified.Value = modified;
                dateModified.Checked = true;

                dateAccessed.Checked = entry.lastAccessDate != 0;
                dateCreated.Checked = entry.creationDate != 0;

                if(Convert.ToBoolean(entry.attribute & 0x01))
                {
                    cbxReadOnly.Checked = true;
                }
                if (Convert.ToBoolean(entry.attribute & 0x02))
                {
                    cbxHidden.Checked = true;
                }
                if (Convert.ToBoolean(entry.attribute & 0x04))
                {
                    cbxSystem.Checked = true;
                }
                if (Convert.ToBoolean(entry.attribute & 0x20))
                {
                    cbxArchive.Checked = true;
                }

                SetIconAndType($"{filename}.{extension}", (FileAttributes)entry.attribute);
            }
        }

        public dlgProperties(FileSystemObject entry) : this()
        {
            txtFilename.Text = entry.Name.ToUpper();
            lblShortFilename1.Text = entry.Name.ToUpper();
            lblSize1.Text = $"{entry.Length:n0} B";

            if(entry is FileSystems.File file)
                lblLocation1.Text = file.DirectoryName;
            else if(entry is FileSystems.Directory dir)
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