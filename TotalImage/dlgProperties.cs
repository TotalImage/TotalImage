using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TotalImage.FileSystems;
using TotalImage.Properties;
using static System.Net.WebRequestMethods;

namespace TotalImage
{
    public partial class dlgProperties : Form
    {
        private List<FileSystemObject> entries; //The file system objects to show the properties of
        private bool hashesDone = false; //Have the MD5 and SHA-1 hashes been calculated yet?

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
            if (entries is null || !entries.Any())
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
            if (entries.Count == 1) //Single object is straightforward
            {
                if (entries[0] is FileSystems.File)
                {
                    txtHashMD5.Enabled = txtHashSHA1.Enabled = true;
                    txtHashMD5.Text = txtHashSHA1.Text = "Click to calculate";
                }
                else
                {
                    txtHashMD5.Enabled = txtHashSHA1.Enabled = false;
                    txtHashMD5.Text = txtHashSHA1.Text = "N/A";
                }

                txtFilename.Text = entries[0].Name;
                if (entries[0] is IFatFileSystemObject fatObj)
                {
                    txtShortFilename.Text = fatObj.ShortName;
                    txtFirstCluster.Text = fatObj.FirstCluster.ToString();
                }
                else
                {
                    txtShortFilename.Text = "N/A"; //For now. We might want to always generate this anyway even for other file systems?
                    txtFirstCluster.Text = "N/A";
                }

                if (entries[0] is FileSystems.File file)
                {
                    txtLocation.Text = file.DirectoryName;
                    txtContains.Text = "N/A";
                    txtSize.Text = Settings.CurrentSettings.SizeUnit.FormatSize(entries[0].Length, Settings.CurrentSettings.SizeUnit != SizeUnit.Bytes);
                    txtSizeOnDisk.Text = Settings.CurrentSettings.SizeUnit.FormatSize(entries[0].LengthOnDisk, Settings.CurrentSettings.SizeUnit != SizeUnit.Bytes);
                }
                else if (entries[0] is FileSystems.Directory dir)
                {
                    txtLocation.Text = dir.Parent?.FullName;
                    txtContains.Text = $"Files: {dir.CountFiles(true)}, subdirectories: {dir.CountSubdirectories(true)}";
                    txtSize.Text = Settings.CurrentSettings.SizeUnit.FormatSize(dir.GetSize(true, false), Settings.CurrentSettings.SizeUnit != SizeUnit.Bytes);
                    txtSizeOnDisk.Text = Settings.CurrentSettings.SizeUnit.FormatSize(dir.GetSize(true, true), Settings.CurrentSettings.SizeUnit != SizeUnit.Bytes);
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
                    imgIcon.SizeMode = PictureBoxSizeMode.Normal;
                    imgIcon.Image = ShellInterop.GetFileTypeIcon(entries[0].Name, entries[0].Attributes).ToBitmap();
                    txtType.Text = ShellInterop.GetFileTypeName(entries[0].Name, entries[0].Attributes);
                }
                else
                {
                    imgIcon.SizeMode = PictureBoxSizeMode.StretchImage;
                    imgIcon.Image = entries[0].Attributes.HasFlag(FileAttributes.Directory)
                        ? Resources.icon_folder_32.ToBitmap()
                        : Resources.icon_page_white_32.ToBitmap();

                    var extension = Path.GetExtension(entries[0].Name);

                    if (entries[0].Attributes.HasFlag(FileAttributes.Directory))
                        txtType.Text = "File folder";
                    else if (extension.Length > 0)
                        txtType.Text = $"{extension[1..].ToUpper()} File";
                    else
                        txtType.Text = "File";
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
                ulong size = 0;
                ulong sizeOnDisk = 0;
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
                        size += dir.GetSize(true, false);
                        sizeOnDisk += dir.GetSize(true, true);

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
                {
                    txtType.Text = "Multiple types";
                }
                else
                {
                    if (Settings.CurrentSettings.QueryShellForFileTypeInfo)
                    {
                        txtType.Text = ShellInterop.GetFileTypeName(entries[0].Name, entries[0].Attributes);
                    }
                    else
                    {
                        if (entries[0].Attributes.HasFlag(FileAttributes.Directory))
                            txtType.Text = "File folder";
                        else if (foExt.Length > 0)
                            txtType.Text = $"{foExt[1..].ToUpper()} File";
                        else
                            txtType.Text = "File";
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
                txtSize.Text = Settings.CurrentSettings.SizeUnit.FormatSize(size, Settings.CurrentSettings.SizeUnit != SizeUnit.Bytes);
                txtSizeOnDisk.Text = Settings.CurrentSettings.SizeUnit.FormatSize(sizeOnDisk, Settings.CurrentSettings.SizeUnit != SizeUnit.Bytes);

                if (entries[0] is FileSystems.File file)
                    txtLocation.Text = file.DirectoryName;
                else if (entries[0] is FileSystems.Directory dir)
                    txtLocation.Text = dir.Parent?.FullName;

                txtShortFilename.Enabled = txtFirstCluster.Enabled = txtContains.Enabled = txtHashMD5.Enabled = txtHashSHA1.Enabled = false;
                txtShortFilename.Text = txtFirstCluster.Text = txtContains.Text = txtHashMD5.Text = txtHashSHA1.Text = "N/A";
                cbxDateCreated.Enabled = cbxDateAccessed.Enabled = cbxDateModified.Enabled = false;
                dtpAccessed.Enabled = dtpCreated.Enabled = dtpModified.Enabled = false;
                dtpAccessed.Text = dtpCreated.Text = dtpModified.Text = "";
                dtpAccessed.CustomFormat = dtpCreated.CustomFormat = dtpModified.CustomFormat = " ";

                imgIcon.Image = Resources.page_white_stack;
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

        private CancellationTokenSource cts = new();

        private async void txtHash_Click(object sender, EventArgs e)
        {
            if (!hashesDone && entries.Count == 1)
            {
                var fileStream = ((FileSystems.File)entries[0]).GetStream();

                var md5 = Task.Run(async () => await HashCalculator.CalculateMd5HashAsync(fileStream, cts.Token));
                var sha1 = Task.Run(async () => await HashCalculator.CalculateSha1HashAsync(fileStream, cts.Token));

                try
                {
                    txtHashMD5.Text = await md5;
                    txtHashSHA1.Text = await sha1;
                }

                catch (Exception ex) when (ex is TaskCanceledException || ex is OperationCanceledException)
                {
                    // Hash calculation was canceled, carry on
                }

                hashesDone = true;
            }
        }

        private void dlgProperties_FormClosing(object sender, FormClosingEventArgs e) 
            => cts.Cancel(); // Cancel the background work if it's still in progress
    }
}
