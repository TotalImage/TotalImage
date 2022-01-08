using System;
using System.Data;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using TotalImage.FileSystems.BPB;
using TotalImage.DiskGeometries;

namespace TotalImage
{
    public partial class dlgNewImage : Form
    {
        internal FloppyGeometry.FriendlyName selectedItem;
        internal FloppyGeometry Geometry { get; private set; }
        internal string VolumeLabel { get; private set; } = "";
        internal string OEMID { get; private set; } = "";
        internal string SerialNumber { get; private set; } = "";
        internal string FileSystemType { get; private set; } = "";
        internal bool WriteBPB { get; private set; }
        internal BiosParameterBlockVersion BPBVersion { get; private set; }

        public dlgNewImage()
        {
            InitializeComponent();
        }

        private void dlgNewImage_Load(object sender, EventArgs e)
        {
            cbxFloppyBPB.Checked = true;
            pnlFloppy.Visible = true;
            pnlHardDisk.Visible = false;
            WriteBPB = true;
            lstFloppyBPB.SelectedIndex = lstFloppyBPB.Items.IndexOf("DOS 4.0+");
            BPBVersion = BiosParameterBlockVersion.Dos40;
            lstContainerFormat.SelectedIndex = lstContainerFormat.Items.IndexOf("Plain sector image");
            lstPartitionTable.SelectedIndex = lstPartitionTable.Items.IndexOf("Master Boot Record");

            lstFloppyGeometries.DisplayMember = "Name";
            lstFloppyGeometries.ValueMember = "Value";
            lstFloppyGeometries.DataSource = Enum.GetValues(typeof(FloppyGeometry.FriendlyName))
                .Cast<Enum>()
                .Select(value => new
                {
                    (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DisplayAttribute)) as
                    DisplayAttribute).Name,
                    value
                })
                .OrderBy(item => item.value)
                .ToList();

            lstFloppyGeometries.SelectedValue = FloppyGeometry.FriendlyName.HighDensity1440k;
            SerialNumber = GenerateVolumeID().ToString("X8");
            OEMID = "MSDOS5.0";
            FileSystemType = "FAT12";
        }

        /* Generates a random volume ID/serial number for DOS 3.4+ BPB
         * TODO: Should this be moved elsewhere? */
        private static int GenerateVolumeID()
        {
            Random rnd = new Random();
            return rnd.Next();
        }

        private void cbxFloppyBPB_CheckedChanged(object sender, EventArgs e)
        {
            lstFloppyBPB.Enabled = cbxFloppyBPB.Checked;
            WriteBPB = cbxFloppyBPB.Checked;
        }

        private void lstFloppyBPB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFloppyBPB.SelectedIndex == 2) //DOS 4.0+ BPB
            {
                BPBVersion = BiosParameterBlockVersion.Dos40;
            }
            else if (lstFloppyBPB.SelectedIndex == 1) //DOS 3.4 BPB
            {
                BPBVersion = BiosParameterBlockVersion.Dos34;
            }
            else if (lstFloppyBPB.SelectedIndex == 0) //DOS 2.0-3.31 BPB
            {
                BPBVersion = BiosParameterBlockVersion.Dos20;
            }
        }

        private void lstFloppyGeometries_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ = Enum.TryParse(lstFloppyGeometries.SelectedValue.ToString(), out selectedItem);

            if (selectedItem != FloppyGeometry.FriendlyName.Custom)
            {
                Geometry = FloppyGeometry.KnownGeometries[selectedItem];
            }

            //Acorn 800k format does not have a boot sector at all
            if (selectedItem == FloppyGeometry.FriendlyName.Acorn800k)
            {
                cbxFloppyBPB.Checked = false;
                cbxFloppyBPB.Enabled = false;
                lstFloppyBPB.Enabled = false;
                BPBVersion = BiosParameterBlockVersion.Dos20;
            }
            else
            {
                cbxFloppyBPB.Checked = true;
                cbxFloppyBPB.Enabled = true;
                lstFloppyBPB.Enabled = true;
            }

            if (selectedItem == FloppyGeometry.FriendlyName.DMF1024 || selectedItem == FloppyGeometry.FriendlyName.DMF2048)
            {
                OEMID = "MSDMF3.2";
            }
            else
            {
                OEMID = "MSDOS5.0";
            }
        }

        private void txtFloppyLabel_TextChanged(object sender, EventArgs e)
        {
            VolumeLabel = txtFloppyLabel.Text;
        }

        private void btnAdvanced_Click(object sender, EventArgs e)
        {
            using dlgNewImageAdvanced dlgAdvanced = new dlgNewImageAdvanced();
            dlgAdvanced.ShowDialog();

            FloppyGeometry newGeometry = new FloppyGeometry((byte)(dlgAdvanced.lstFloppySides.SelectedIndex + 1), (byte)dlgAdvanced.txtFloppyTracks.Value,
                (byte)dlgAdvanced.txtFloppySPT.Value, (byte)(Math.Log((double)dlgAdvanced.txtFloppyBPS.Value, 2) - 7), (byte)dlgAdvanced.txtFloppyMediaDesc.Value,
                (byte)dlgAdvanced.txtFloppySPC.Value, (byte)dlgAdvanced.txtFloppyNumFATs.Value, (byte)dlgAdvanced.txtFloppySPF.Value, (ushort)dlgAdvanced.txtFloppyRootDir.Value,
                (byte)dlgAdvanced.txtFloppyReservedSect.Value);

            if (selectedItem != FloppyGeometry.FriendlyName.Custom && !newGeometry.Equals(Geometry))
            {
                lstFloppyGeometries.SelectedValue = FloppyGeometry.FriendlyName.Custom;
                Geometry = newGeometry;
            }

            SerialNumber = dlgAdvanced.txtFloppySerial.Text;
            FileSystemType = dlgAdvanced.txtFloppyFSType.Text;
            OEMID = dlgAdvanced.txtFloppyOEMID.Text;
        }

        private void rbnFloppyDisk_CheckedChanged(object sender, EventArgs e)
        {
            pnlFloppy.Visible = rbnFloppyDisk.Checked;
            pnlHardDisk.Visible = rbnHardDisk.Checked;

            //This will probably need some more thought put into it once we add additional container support...
            if (rbnHardDisk.Checked)
            {
                lstContainerFormat.Items.Add("Microsoft VHD");
            }
            else if (rbnFloppyDisk.Checked)
            {
                lstContainerFormat.Items.Remove("Microsoft VHD");
            }

            lstContainerFormat.SelectedIndex = lstContainerFormat.Items.IndexOf("Plain sector image");
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (rbnFloppyDisk.Checked)
            {
                if (!cbxFloppyBPB.Checked)
                {
                    TaskDialogButton result = TaskDialog.ShowDialog(this, new TaskDialogPage()
                    {
                        Text = "You chose not to create a BIOS Parameter Block (BPB) for this image. Some programs and operating systems may not recognize the disk because of this.\n\nAre you sure you want to continue?",
                        Heading = "No BIOS Parameter Block selected",
                        Caption = "Warning",
                        Buttons =
                        {
                            TaskDialogButton.Yes,
                            TaskDialogButton.No
                        },
                        Icon = TaskDialogIcon.Warning,
                    });

                    if (result == TaskDialogButton.No)
                    {
                        DialogResult = DialogResult.None; //This is needed so the dialog doesn't close anyway
                        return;
                    }
                }
            }
            else if (rbnHardDisk.Checked)
            {

            }
        }

        private void cbxWritePartTable_CheckedChanged(object sender, EventArgs e)
        {
            lstPartitionTable.Enabled = cbxWritePartTable.Checked;
        }
    }
}
