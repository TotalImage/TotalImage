using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TotalImage.FileSystems;
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

        //TODO: VFAT/LFN support, proper size calculations
        public dlgProperties(FileSystemObject entry) : this()
        {
            string sizeUnitName = Enum.GetName(typeof(Settings.SizeUnit), Settings.CurrentSettings.SizeUnits);

            if (entry == null)
                throw new ArgumentNullException(nameof(entry), "entry cannot be null!");

            txtFilename.Text = entry.Name.ToUpper();
            lblShortFilename1.Text = entry.Name.ToUpper();

            lblSize1.Text = $"{entry.Length:n0} B";
            if (Settings.CurrentSettings.SizeUnits != Settings.SizeUnit.B)
            {
                float sizeInUnit = entry.Length / (float)Settings.CurrentSettings.SizeUnits;
                lblSize1.Text = lblSize1.Text.Insert(0, $"{sizeInUnit:n2} {sizeUnitName} (");
                lblSize1.Text = lblSize1.Text.Insert(lblSize1.Text.Length, ")");
            }

            frmMain mainForm = (frmMain)Application.OpenForms["frmMain"];
            FileSystems.FAT.Fat12 fs = (FileSystems.FAT.Fat12)mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem;
            uint clusterSize = (uint)fs.BiosParameterBlock.LogicalSectorsPerCluster * fs.BiosParameterBlock.BytesPerLogicalSector;
            Debug.WriteLine($"Cluster size: {clusterSize}");
            uint sizeOnDisk = (uint)Math.Ceiling(entry.Length / (double)clusterSize) * clusterSize;
            Debug.WriteLine($"Size on disk: {sizeOnDisk}");

            lblSizeOnDisk1.Text = $"{sizeOnDisk:n0} B";
            if (Settings.CurrentSettings.SizeUnits != Settings.SizeUnit.B)
            {
                float sizeInUnit = sizeOnDisk / (float)Settings.CurrentSettings.SizeUnits;
                lblSizeOnDisk1.Text = lblSizeOnDisk1.Text.Insert(0, $"{sizeInUnit:n2} {sizeUnitName} (");
                lblSizeOnDisk1.Text = lblSizeOnDisk1.Text.Insert(lblSizeOnDisk1.Text.Length, ")");
            }

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

            SetIconAndType(entry.Name, entry.Attributes);

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

        public void SetIconAndType(string filename, FileAttributes attributes)
        {
            var shellInfo = new SHFILEINFO();
            var flags = SHGFI.ICON | SHGFI.LARGEICON | SHGFI.TYPENAME | SHGFI.USEFILEATTRIBUTES;
            if (SHGetFileInfo(filename, attributes, ref shellInfo, (uint)Marshal.SizeOf(shellInfo), flags) != IntPtr.Zero)
            {
                using (var icon = Icon.FromHandle(shellInfo.hIcon))
                {
                    imgIcon.Image = (icon.Clone() as Icon)?.ToBitmap();
                }

                lblType1.Text = shellInfo.szTypeName;

                DestroyIcon(shellInfo.hIcon);
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