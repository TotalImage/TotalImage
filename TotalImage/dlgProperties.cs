using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using TotalImage.FileSystems;
using TotalImage.Properties;
using static System.Net.WebRequestMethods;

namespace TotalImage
{
    public partial class dlgProperties : Form
    {
        private List<FileSystemObject> entries;
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

        public dlgProperties(List<FileSystemObject> entries) : this()
        {
            if (entries == null || entries.Count == 0)
                throw new ArgumentNullException(nameof(entries), "Entries list cannot be null or have 0 items!");

            this.entries = entries;
        }

        //TODO: Perform filename validation
        private void btnOK_Click(object sender, EventArgs e)
        {
            NewName = txtFilename.Text;

            if (cbxDateModified.Checked)
                DateModified = dtpModified.Value;
            if (cbxDateCreated.Checked)
                DateCreated = dtpCreated.Value;
            if (cbxDateAccessed.Checked)
                DateAccessed = dtpAccessed.Value;

            AttrArchive = cbxArchive.Checked;
            AttrHidden = cbxHidden.Checked;
            AttrReadOnly = cbxReadOnly.Checked;
            AttrSystem = cbxSystem.Checked;
        }

        //TODO: Perform short file name (8.3) generation (via the FAT helper class?)
        private void txtFilename_TextChanged(object sender, EventArgs e)
        {

        }

        //TODO: Maybe we should follow the user's locale for the date format?
        private void dlgProperties_Load(object sender, EventArgs e)
        {
            frmMain mainForm = (frmMain)Application.OpenForms["frmMain"];
            if (entries.Count == 1) //Single object is straightforward
            {
                txtFilename.Text = entries[0].Name;
                if (entries[0] is IFatFileSystemObject fatObj)
                {
                    txtShortFilename1.Text = fatObj.ShortName;
                    txtFirstCluster1.Text = fatObj.FirstCluster.ToString();
                }
                else
                {
                    txtShortFilename1.Text = "N/A"; //For now. We might want to always generate this anyway even for other file systems?
                    txtFirstCluster1.Text = "N/A";
                }

                if (entries[0] is FileSystems.File file)
                {
                    txtLocation1.Text = file.DirectoryName;
                    txtContains1.Text = "N/A";
                    txtSize1.Text = Settings.CurrentSettings.SizeUnit.FormatSize(entries[0].Length, Settings.CurrentSettings.SizeUnit != SizeUnit.Bytes);
                    txtSizeOnDisk1.Text = Settings.CurrentSettings.SizeUnit.FormatSize(entries[0].LengthOnDisk, Settings.CurrentSettings.SizeUnit != SizeUnit.Bytes);
                }
                else if (entries[0] is FileSystems.Directory dir)
                {
                    txtLocation1.Text = dir.Parent?.FullName;
                    txtContains1.Text = $"Files: {dir.FileCount(true)}, subdirectories: {dir.SubdirectoryCount(true)}";
                    txtSize1.Text = Settings.CurrentSettings.SizeUnit.FormatSize(dir.Size(true, false), Settings.CurrentSettings.SizeUnit != SizeUnit.Bytes);
                    txtSizeOnDisk1.Text = Settings.CurrentSettings.SizeUnit.FormatSize(dir.Size(true, true), Settings.CurrentSettings.SizeUnit != SizeUnit.Bytes);
                }

                // These are indeed supposed to be assignments in the conditions.
                if (cbxDateAccessed.Checked = dtpAccessed.Enabled = entries[0].LastAccessTime.HasValue)
                {
                    if (entries[0] is IFatFileSystemObject)
                        dtpAccessed.CustomFormat = "yyyy-MM-dd";
                    else
                        dtpAccessed.CustomFormat = "yyyy-MM-dd HH:mm:ss";
                    dtpAccessed.Value = entries[0].LastAccessTime!.Value;
                }
                else
                {
                    dtpAccessed.Text = "";
                    dtpAccessed.CustomFormat = " ";
                }

                if (cbxDateModified.Checked = dtpModified.Enabled = entries[0].LastWriteTime.HasValue)
                {
                    dtpModified.CustomFormat = "yyyy-MM-dd HH:mm:ss";
                    dtpModified.Value = entries[0].LastWriteTime!.Value;
                }
                else
                {
                    dtpModified.Text = "";
                    dtpModified.CustomFormat = " ";
                }

                if (cbxDateCreated.Checked = dtpCreated.Enabled = entries[0].CreationTime.HasValue)
                {
                    dtpCreated.CustomFormat = "yyyy-MM-dd HH:mm:ss";
                    dtpCreated.Value = entries[0].CreationTime!.Value;
                }
                else
                {
                    dtpCreated.Text = "";
                    dtpCreated.CustomFormat = " ";
                }

                cbxReadOnly.Checked = entries[0].Attributes.HasFlag(FileAttributes.ReadOnly);
                cbxHidden.Checked = entries[0].Attributes.HasFlag(FileAttributes.Hidden);
                cbxSystem.Checked = entries[0].Attributes.HasFlag(FileAttributes.System);
                cbxArchive.Checked = entries[0].Attributes.HasFlag(FileAttributes.Archive);

                if (Settings.CurrentSettings.QueryShellForFileTypeInfo)
                {
                    var extension = entries[0].Attributes.HasFlag(FileAttributes.Directory) ? "folder" : Path.GetExtension(entries[0].Name);
                    string key = frmMain.fileTypes[extension].iconIndex.ToString();
                    imgIcon.Image = mainForm.imgFilesLarge.Images[key];
                    txtType1.Text = frmMain.fileTypes[extension].name;
                }
                else
                {
                    string extension = Path.GetExtension(entries[0].Name);
                    string key = entries[0].Attributes.HasFlag(FileAttributes.Directory) ? "folder" : "file";
                    imgIcon.Image = mainForm.imgFilesLarge.Images[key];

                    if (entries[0].Attributes.HasFlag(FileAttributes.Directory))
                        txtType1.Text = "File folder";
                    else if (extension.Length > 0)
                        txtType1.Text = $"{extension.Substring(1).ToUpper()} File";
                    else
                        txtType1.Text = "File";
                }

                //Prevent any changes to deleted items as well as ISO file system objects
                if (entries[0].Name.StartsWith("?") || entries[0] is FileSystems.ISO.IsoFile || entries[0] is FileSystems.ISO.IsoDirectory)
                {
                    txtFilename.Enabled = false;
                    cbxArchive.Enabled = false;
                    cbxHidden.Enabled = false;
                    cbxReadOnly.Enabled = false;
                    cbxSystem.Enabled = false;
                    cbxDateCreated.Enabled = false;
                    cbxDateAccessed.Enabled = false;
                    cbxDateModified.Enabled = false;
                    dtpAccessed.Enabled = false;
                    dtpCreated.Enabled = false;
                    dtpModified.Enabled = false;
                }
            }
            else //If we're showing info for multiple objects, it needs to be treated separately
            {
                txtFilename.Enabled = txtFilename.Visible = false;
                lblMultipleObjectsCount.Visible = true;

                int dirs = 0;
                int files = 0;
                long size = 0;
                long sizeOnDisk = 0;
                uint attribReadOnly = 0;
                uint attribSystem = 0;
                uint attribHidden = 0;
                uint attribArchive = 0;
                string foExt = entries[0] is FileSystems.Directory ? "folder" : Path.GetExtension(entries[0].Name);
                bool differentTypes = false;

                foreach (FileSystemObject fso in entries)
                {
                    if (fso is FileSystems.Directory dir)
                    {
                        dirs++;
                        size += dir.Size(true, false);
                        sizeOnDisk += dir.Size(true, true);

                        if (!foExt.Equals("folder"))
                            differentTypes = true;
                    }
                    else
                    {
                        files++;
                        size += fso.Length;
                        sizeOnDisk += fso.LengthOnDisk;

                        if (!foExt.Equals(Path.GetExtension(fso.Name)))
                            differentTypes = true;
                    }

                    if (fso.Attributes.HasFlag(FileAttributes.ReadOnly))
                        attribReadOnly++;
                    if (fso.Attributes.HasFlag(FileAttributes.System))
                        attribSystem++;
                    if (fso.Attributes.HasFlag(FileAttributes.Hidden))
                        attribHidden++;
                    if (fso.Attributes.HasFlag(FileAttributes.Archive))
                        attribArchive++;
                }

                if (differentTypes)
                    txtType1.Text = "Multiple types";
                else
                {
                    if (Settings.CurrentSettings.QueryShellForFileTypeInfo)
                        txtType1.Text = frmMain.fileTypes[foExt].name;
                    else
                    {
                        if (entries[0].Attributes.HasFlag(FileAttributes.Directory))
                            txtType1.Text = "File folder";
                        else if (foExt.Length > 0)
                            txtType1.Text = $"{foExt.Substring(1).ToUpper()} File";
                        else
                            txtType1.Text = "File";
                    }
                }

                if (attribReadOnly > 0)
                {
                    cbxReadOnly.Checked = true;
                    cbxReadOnly.CheckState = attribReadOnly < entries.Count ? CheckState.Indeterminate : CheckState.Checked;
                }

                if (attribSystem > 0)
                {
                    cbxSystem.Checked = true;
                    cbxSystem.CheckState = attribSystem < entries.Count ? CheckState.Indeterminate : CheckState.Checked;
                }

                if (attribHidden > 0)
                {
                    cbxHidden.Checked = true;
                    cbxHidden.CheckState = attribHidden < entries.Count ? CheckState.Indeterminate : CheckState.Checked;
                }

                if (attribArchive > 0)
                {
                    cbxArchive.Checked = true;
                    cbxArchive.CheckState = attribArchive < entries.Count ? CheckState.Indeterminate : CheckState.Checked;
                }

                lblMultipleObjectsCount.Text = $"Files: {files}, directories: {dirs}";
                txtSize1.Text = Settings.CurrentSettings.SizeUnit.FormatSize(size, Settings.CurrentSettings.SizeUnit != SizeUnit.Bytes);
                txtSizeOnDisk1.Text = Settings.CurrentSettings.SizeUnit.FormatSize(sizeOnDisk, Settings.CurrentSettings.SizeUnit != SizeUnit.Bytes);

                if (entries[0] is FileSystems.File file)
                    txtLocation1.Text = file.DirectoryName;
                else if (entries[0] is FileSystems.Directory dir)
                    txtLocation1.Text = dir.Parent?.FullName;

                txtShortFilename1.Text = txtFirstCluster1.Text = txtContains1.Text = "N/A";
                cbxDateCreated.Enabled = false;
                cbxDateAccessed.Enabled = false;
                cbxDateModified.Enabled = false;
                dtpAccessed.Enabled = false;
                dtpAccessed.Text = "";
                dtpAccessed.CustomFormat = " ";
                dtpCreated.Enabled = false;
                dtpCreated.Text = "";
                dtpCreated.CustomFormat = " ";
                dtpModified.Enabled = false;
                dtpModified.Text = "";
                dtpModified.CustomFormat = " ";

                imgIcon.Image = Resources.multiple_select_32;
                imgIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void cbxDateCreated_CheckedChanged(object sender, EventArgs e)
        {
            if (entries.Count == 1)
            {
                dtpCreated.Enabled = cbxDateCreated.Checked;
                if (dtpCreated.Enabled)
                {
                    dtpCreated.CustomFormat = "yyyy-MM-dd HH:mm:ss";
                    dtpCreated.Value = DateTime.Now;
                }
                else
                {
                    dtpCreated.Text = "";
                    dtpCreated.CustomFormat = " ";
                }
            }
        }

        private void cbxDateModified_CheckedChanged(object sender, EventArgs e)
        {
            if (entries.Count == 1)
            {
                dtpModified.Enabled = cbxDateModified.Checked;
                if (dtpModified.Enabled)
                {
                    dtpModified.CustomFormat = "yyyy-MM-dd HH:mm:ss";
                    dtpModified.Value = DateTime.Now;
                }
                else
                {
                    dtpModified.Text = "";
                    dtpModified.CustomFormat = " ";
                }
            }
        }

        private void cbxDateAccessed_CheckedChanged(object sender, EventArgs e)
        {
            if (entries.Count == 1)
            {
                dtpAccessed.Enabled = cbxDateAccessed.Checked;
                if (dtpAccessed.Enabled)
                {
                    if (entries[0] is IFatFileSystemObject)
                        dtpAccessed.CustomFormat = "yyyy-MM-dd";
                    else
                        dtpAccessed.CustomFormat = "yyyy-MM-dd HH:mm:ss";
                    dtpAccessed.Value = DateTime.Now;
                }
                else
                {
                    dtpAccessed.Text = "";
                    dtpAccessed.CustomFormat = " ";
                }
            }
        }
    }
}
