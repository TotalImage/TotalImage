﻿using System;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using TotalImage.FileSystems.FAT;

namespace TotalImage
{
    public partial class dlgProperties : Form
    {
        private FatDirEntry entry;

        public dlgProperties()
        {
            InitializeComponent();
        }

        public dlgProperties(FatDirEntry entry)
        {
            InitializeComponent();
            this.entry = entry;
        }

        private void dlgProperties_Load(object sender, EventArgs e)
        {
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
            }
        }
    }
}