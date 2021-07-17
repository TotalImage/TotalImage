using System;
using System.IO;
using System.Windows.Forms;
using TotalImage.FileSystems;

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
                txtShortFilename1.Text = fatObj.ShortName;

            txtSize1.Text = Settings.CurrentSettings.SizeUnit.FormatSize(entry.Length, true);
            txtSizeOnDisk1.Text = Settings.CurrentSettings.SizeUnit.FormatSize(entry.LengthOnDisk, true);

            if (entry is FileSystems.File file)
            {
                txtLocation1.Text = file.DirectoryName;
                txtContains1.Enabled = false;
                txtContains1.Text = "N/A";
                txtContains.Enabled = false;
                txtSize1.Text = Settings.CurrentSettings.SizeUnit.FormatSize(entry.Length, true);
                txtSizeOnDisk1.Text = Settings.CurrentSettings.SizeUnit.FormatSize(entry.LengthOnDisk, true);
            }
            else if (entry is FileSystems.Directory dir)
            {
                txtLocation1.Text = dir.Parent?.FullName;
                txtContains1.Enabled = true;
                txtContains1.Text = $"Files: {dir.FileCount(true)}, subdirectories: {dir.SubdirectoryCount(true)}";
                txtContains.Enabled = true;
                txtSize1.Text = Settings.CurrentSettings.SizeUnit.FormatSize(dir.Size(true, false), true);
                txtSizeOnDisk1.Text = Settings.CurrentSettings.SizeUnit.FormatSize(dir.Size(true, true), true);
            }

            // These are indeed supposed to be assignments in the conditions.
            if (cbxDateAccessed.Checked = dateAccessed.Enabled = entry.LastAccessTime.HasValue)
                dateAccessed.Value = entry.LastAccessTime!.Value;

            if (cbxDateModified.Checked = dateModified.Enabled = entry.LastWriteTime.HasValue)
                dateModified.Value = entry.LastWriteTime!.Value;

            if (cbxDateCreated.Checked = dateCreated.Enabled = entry.CreationTime.HasValue)
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
                txtType1.Text = frmMain.fileTypes[extension].name;
            }
            else
            {               
                string extension = Path.GetExtension(entry.Name);
                string key = entry.Attributes.HasFlag(FileAttributes.Directory) ? "folder" : "file";
                imgIcon.Image = mainForm.imgFilesLarge.Images[key];

                if (entry.Attributes.HasFlag(FileAttributes.Directory))
                    txtType1.Text = "File folder";
                else if (extension.Length > 0)
                    txtType1.Text = $"{extension.Substring(1).ToUpper()} File";
                else
                    txtType1.Text = "File";
            }

            //Prevent any changes to deleted items
            if (entry.Name.StartsWith("?"))
            {
                txtFilename.Enabled = false;
                cbxArchive.Enabled = false;
                cbxHidden.Enabled = false;
                cbxReadOnly.Enabled = false;
                cbxSystem.Enabled = false;
                cbxDateCreated.Enabled = false;
                cbxDateAccessed.Enabled = false;
                cbxDateModified.Enabled = false;
                dateAccessed.Enabled = false;
                dateCreated.Enabled = false;
                dateModified.Enabled = false;
            }
        }

        //TODO: Perform filename validation
        private void btnOK_Click(object sender, EventArgs e)
        {
            NewName = txtFilename.Text;

            if (cbxDateModified.Checked)
                DateModified = dateModified.Value;
            if (cbxDateCreated.Checked)
                DateCreated = dateCreated.Value;
            if (cbxDateAccessed.Checked)
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

        private void cbxDateCreated_CheckedChanged(object sender, EventArgs e)
        {
            dateCreated.Enabled = cbxDateCreated.Checked;
        }

        private void cbxDateModified_CheckedChanged(object sender, EventArgs e)
        {
            dateModified.Enabled = cbxDateModified.Checked;
        }

        private void cbxDateAccessed_CheckedChanged(object sender, EventArgs e)
        {
            dateAccessed.Enabled = cbxDateAccessed.Checked;
        }
    }
}