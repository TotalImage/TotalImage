﻿using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TotalImage.FileSystems;
using TotalImage.FileSystems.FAT;
using System.Diagnostics;
using static Interop.Shell32;
using static Interop.User32;

namespace TotalImage
{
    public partial class dlgProperties : Form
    {
        public string? NewName { get; private set; }
        public DateTime? DateModified { get; private set; }
        public DateTime? DateCreated { get; private set; }
        public DateTime? DateAccessed { get; private set; }
        public bool AttrReadOnly { get; private set; }
        public bool AttrHidden { get; private set; }
        public bool AttrSystem { get; private set; }
        public bool AttrArchive { get; private set; }

        private dlgProperties()
        {
            InitializeComponent();
        }

        public dlgProperties(FileSystemObject entry) : this()
        {
            if (entry == null)
                throw new ArgumentNullException(nameof(entry), "entry cannot be null!");

            txtFilename.Text = entry.Name;

            if (entry is IFatFileSystemObject fatObj)
                lblShortFilename1.Text = fatObj.ShortName;

            lblSize1.Text = Settings.CurrentSettings.SizeUnit.FormatSize(entry.Length, true);
            lblSizeOnDisk1.Text = Settings.CurrentSettings.SizeUnit.FormatSize(entry.LengthOnDisk, true);

            if (entry is FileSystems.File file)
                lblLocation1.Text = file.DirectoryName;
            else if (entry is FileSystems.Directory dir)
                lblLocation1.Text = dir.Parent?.FullName;

            // These are indeed supposed to be assignments in the conditions.
            if (dateAccessed.Checked = entry.LastAccessTime.HasValue)
                dateAccessed.Value = entry.LastAccessTime!.Value;

            if (dateModified.Checked = entry.LastWriteTime.HasValue)
                dateModified.Value = entry.LastWriteTime!.Value;

            if (dateCreated.Checked = entry.CreationTime.HasValue)
                dateCreated.Value = entry.CreationTime!.Value;

            cbxReadOnly.Checked = entry.Attributes.HasFlag(FileAttributes.ReadOnly);
            cbxHidden.Checked = entry.Attributes.HasFlag(FileAttributes.Hidden);
            cbxSystem.Checked = entry.Attributes.HasFlag(FileAttributes.System);
            cbxArchive.Checked = entry.Attributes.HasFlag(FileAttributes.Archive);

            frmMain mainForm = (frmMain)Application.OpenForms["frmMain"];
            if (Settings.CurrentSettings.QueryShellForFileTypeInfo)
            {
                var extension = entry.Attributes.HasFlag(FileAttributes.Directory) ? "folder" : Path.GetExtension(entry.Name);
                string key = frmMain.fileTypes[extension].iconIndex.ToString();
                imgIcon.Image = mainForm.imgFilesLarge.Images[key];
                lblType1.Text = frmMain.fileTypes[extension].name;
            }
            else
            {               
                string extension = Path.GetExtension(entry.Name);
                string key = entry.Attributes.HasFlag(FileAttributes.Directory) ? "folder" : "file";
                imgIcon.Image = mainForm.imgFilesLarge.Images[key];

                if (entry.Attributes.HasFlag(FileAttributes.Directory))
                    lblType1.Text = "File folder";
                else if (extension.Length > 0)
                    lblType1.Text = $"{extension.Substring(1).ToUpper()} File";
                else
                    lblType1.Text = "File";
            }

            //Prevent any changes to deleted items
            if (entry.Name.StartsWith("?"))
            {
                txtFilename.Enabled = false;
                cbxArchive.Enabled = false;
                cbxHidden.Enabled = false;
                cbxReadOnly.Enabled = false;
                cbxSystem.Enabled = false;
                dateAccessed.Enabled = false;
                dateCreated.Enabled = false;
                dateModified.Enabled = false;
            }
        }

        //TODO: Perform filename validation
        private void btnOK_Click(object sender, EventArgs e)
        {
            NewName = txtFilename.Text;

            if (dateModified.Checked)
                DateModified = dateModified.Value;
            if (dateCreated.Checked)
                DateCreated = dateCreated.Value;
            if (dateAccessed.Checked)
                DateAccessed = dateAccessed.Value;

            AttrArchive = cbxArchive.Checked;
            AttrHidden = cbxHidden.Checked;
            AttrReadOnly = cbxReadOnly.Checked;
            AttrSystem = cbxSystem.Checked;
        }

        //TODO: Perform short file name (8.3) generation (via the FAT helper class?)
        private void txtFilename_TextChanged(object sender, EventArgs e)
        {

        }

        //TODO: Maybe we should follow the user's locale for such things?
        private void dlgProperties_Load(object sender, EventArgs e)
        {
            /* This is effectively the same as using CultureInfo.CurrentCulture.DateTimeFormat.UniversalSortableDateTimePattern, just without
             * the silly 'Z' at the end... */
            dateCreated.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dateModified.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dateAccessed.CustomFormat = "yyyy-MM-dd";
        }
    }
}