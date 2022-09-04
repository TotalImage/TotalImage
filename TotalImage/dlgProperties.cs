using System;
using System.IO;
using System.Windows.Forms;
using TotalImage.FileSystems;

namespace TotalImage
{
    public partial class dlgProperties : Form
    {
        private FileSystemObject entry;
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

            this.entry = entry;
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
            txtFilename.Text = entry.Name;           
            if (entry is IFatFileSystemObject fatObj)
            {
                txtShortFilename1.Text = fatObj.ShortName;
                txtFirstCluster1.Text = fatObj.FirstCluster.ToString();
            }
            else
            {
                txtShortFilename1.Text = "N/A"; //For now. We might want to always generate this anyway even for other file systems?
                txtFirstCluster1.Text = "N/A";
            }

            if (entry is FileSystems.File file)
            {
                txtLocation1.Text = file.DirectoryName;
                txtContains1.Text = "N/A";
                txtSize1.Text = Settings.CurrentSettings.SizeUnit.FormatSize(entry.Length, Settings.CurrentSettings.SizeUnit != SizeUnit.Bytes);
                txtSizeOnDisk1.Text = Settings.CurrentSettings.SizeUnit.FormatSize(entry.LengthOnDisk, Settings.CurrentSettings.SizeUnit != SizeUnit.Bytes);
            }
            else if (entry is FileSystems.Directory dir)
            {
                txtLocation1.Text = dir.Parent?.FullName;
                txtContains1.Text = $"Files: {dir.FileCount(true)}, subdirectories: {dir.SubdirectoryCount(true)}";
                txtSize1.Text = Settings.CurrentSettings.SizeUnit.FormatSize(dir.Size(true, false), Settings.CurrentSettings.SizeUnit != SizeUnit.Bytes);
                txtSizeOnDisk1.Text = Settings.CurrentSettings.SizeUnit.FormatSize(dir.Size(true, true), Settings.CurrentSettings.SizeUnit != SizeUnit.Bytes);
            }

            // These are indeed supposed to be assignments in the conditions.
            if (cbxDateAccessed.Checked = dtpAccessed.Enabled = entry.LastAccessTime.HasValue)
            {
                if (entry is IFatFileSystemObject)
                    dtpAccessed.CustomFormat = "yyyy-MM-dd";
                else
                    dtpAccessed.CustomFormat = "yyyy-MM-dd HH:mm:ss";
                dtpAccessed.Value = entry.LastAccessTime!.Value;
            }
            else
            {
                dtpAccessed.Text = "";
                dtpAccessed.CustomFormat = " ";
            }

            if (cbxDateModified.Checked = dtpModified.Enabled = entry.LastWriteTime.HasValue)
            {
                dtpModified.CustomFormat = "yyyy-MM-dd HH:mm:ss";
                dtpModified.Value = entry.LastWriteTime!.Value;
            }
            else
            {
                dtpModified.Text = "";
                dtpModified.CustomFormat = " ";
            }

            if (cbxDateCreated.Checked = dtpCreated.Enabled = entry.CreationTime.HasValue)
            {
                dtpCreated.CustomFormat = "yyyy-MM-dd HH:mm:ss";
                dtpCreated.Value = entry.CreationTime!.Value;
            }
            else
            {
                dtpCreated.Text = "";
                dtpCreated.CustomFormat = " ";
            }

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

            //Prevent any changes to deleted items as well as ISO file system objects
            if (entry.Name.StartsWith("?") || entry is FileSystems.ISO.IsoFile || entry is FileSystems.ISO.IsoDirectory)
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

        private void cbxDateCreated_CheckedChanged(object sender, EventArgs e)
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

        private void cbxDateModified_CheckedChanged(object sender, EventArgs e)
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

        private void cbxDateAccessed_CheckedChanged(object sender, EventArgs e)
        {
            dtpAccessed.Enabled = cbxDateAccessed.Checked;
            if (dtpAccessed.Enabled)
            {
                if(entry is IFatFileSystemObject)
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
