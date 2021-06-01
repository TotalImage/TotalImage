using System;
using System.Windows.Forms;
using TotalImage.DiskGeometries;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TotalImage.FileSystems.BPB;

namespace TotalImage
{
    public partial class dlgNewImageAdvanced : Form
    {
        private FloppyGeometry.FriendlyName selectedItem;
        public FloppyGeometry Geometry { get; private set; }
        public string OEMID { get; private set; } = "";
        public string VolumeLabel { get; private set; } = "";
        public string SerialNumber { get; private set; } = "";
        public string FileSystemType { get; private set; } = "";
        public bool WriteBPB { get; private set; }
        public BiosParameterBlockVersion BPBVersion { get; private set; }

        public dlgNewImageAdvanced()
        {
            InitializeComponent();
        }

        //TODO: Should the capacities list be loaded from the KnownGeometries list or something like that?
        private void dlgNewImage_Load(object sender, EventArgs e)
        {
            //Default values are for a 1440 KiB disk with DOS 4.0+ BPB
            cbxFloppyBPB.Checked = true;
            WriteBPB = true;
            lstFloppyBPB.SelectedIndex = 3;
            BPBVersion = BiosParameterBlockVersion.Dos40;

            //Get the list of presets from KnownGeometries and display the fancy Name attribute of the enum
            lstFloppyCapacity.DisplayMember = "Name";
            lstFloppyCapacity.ValueMember = "Value";
            lstFloppyCapacity.DataSource = Enum.GetValues(typeof(FloppyGeometry.FriendlyName))
                .Cast<Enum>()
                .Select(value => new
                {
                    (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DisplayAttribute)) as
                    DisplayAttribute).Name,
                    value
                })
                .OrderBy(item => item.value)
                .ToList();

            lstFloppyCapacity.SelectedValue = FloppyGeometry.FriendlyName.HighDensity1440k;
            txtFloppySerial.Text = GenerateVolumeID().ToString("X8");
            SerialNumber = txtFloppySerial.Text;
            OEMID = txtFloppyOEMID.Text;
            VolumeLabel = txtFloppyLabel.Text;
            FileSystemType = txtFloppyFSType.Text;
        }

        //TODO: Perform some validation of parameters before leaving in case the user tries to create an impossible image
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!cbxFloppyBPB.Checked)
            {
                TaskDialogButton result = TaskDialog.ShowDialog(this, new TaskDialogPage()
                {
                    Text = "Some programs and operating systems may not recognize the disk because of this. Are you sure you want to continue?",
                    Heading = "You chose not to write a DOS BIOS Parameter Block (BPB)",
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

            //If the "Custom..." option is selected, create a new FloppyGeometry with the custom parameters
            //We don't need some of the parameters yet for raw images, so let's just ignore them for now
            /*if (selectedItem == FloppyGeometry.FriendlyName.Custom)
            {
                Geometry = new FloppyGeometry(0, (byte)(lstFloppySides.SelectedIndex + 1), 0, 0, 0, (byte)txtFloppyTracks.Value,
                    (byte)txtFloppySPT.Value, (byte)(Math.Log((double)txtFloppyBPS.Value, 2) - 7), (byte)txtFloppyMediaDesc.Value, (byte)txtFloppySPC.Value,
                    (byte)txtFloppyNumFATs.Value, (byte)txtFloppySPF.Value, (ushort)txtFloppyRootDir.Value, (byte)txtFloppyReservedSect.Value);
            }*/
        }

        private void cbxFloppyBPB_CheckedChanged(object sender, EventArgs e)
        {
            //Disable volume ID and FS type fields since they were only added in DOS 3.4/4.0
            lstFloppyBPB.Enabled = cbxFloppyBPB.Checked;
            txtFloppySerial.Enabled = cbxFloppyBPB.Checked && (lstFloppyBPB.SelectedIndex >= 2);
            txtFloppyFSType.Enabled = cbxFloppyBPB.Checked && (lstFloppyBPB.SelectedIndex == 3);
            WriteBPB = cbxFloppyBPB.Checked;
        }

        /* Generates a random volume ID/serial number for DOS 3.4+ BPB
         * TODO: Should this be moved elsewhere? */
        private static int GenerateVolumeID()
        {
            Random rnd = new Random();
            return rnd.Next();
        }

        private void lstFloppyBPB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFloppyBPB.SelectedIndex == 3) //DOS 4.0+ BPB
            {
                txtFloppyFSType.Enabled = true;
                txtFloppySerial.Enabled = true;
                BPBVersion = BiosParameterBlockVersion.Dos40;
            }
            else if (lstFloppyBPB.SelectedIndex == 2) //DOS 3.4 BPB
            {
                txtFloppyFSType.Enabled = false;
                txtFloppySerial.Enabled = true;
                BPBVersion = BiosParameterBlockVersion.Dos34;
            }
            else if (lstFloppyBPB.SelectedIndex <= 1) //DOS 2.0-3.31 BPB
            {
                txtFloppyFSType.Enabled = false;
                txtFloppySerial.Enabled = false;
                BPBVersion = BiosParameterBlockVersion.Dos20;
            }
        }

        private void lstFloppyCapacity_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ = Enum.TryParse(lstFloppyCapacity.SelectedValue.ToString(), out selectedItem);

            /*if (selectedItem != FloppyGeometry.FriendlyName.Custom)
            {*/
                Geometry = FloppyGeometry.KnownGeometries[selectedItem];
                lstFloppySides.SelectedIndex = Geometry.Sides - 1;
                txtFloppyTracks.Value = Geometry.Tracks;
                txtFloppySPT.Value = Geometry.SPT;
                txtFloppyBPS.Value = 128 << Geometry.BPS;
                txtFloppyMediaDesc.Value = Geometry.MediaDescriptor;
                txtFloppySPC.Value = Geometry.SPC;
                txtFloppyNumFATs.Value = Geometry.NoOfFATs;
                txtFloppySPF.Value = Geometry.SPF;
                txtFloppyRootDir.Value = Geometry.RootDirectoryEntries;
                txtFloppyReservedSect.Value = Geometry.ReservedSectors;
                txtFloppyTotalSect.Value = Geometry.Tracks * Geometry.SPT * Geometry.Sides;
            //}

            if (selectedItem == FloppyGeometry.FriendlyName.Acorn800k)
            {
                cbxFloppyBPB.Checked = false;
                cbxFloppyBPB.Enabled = false;
                lstFloppyBPB.Enabled = false;
                txtFloppyFSType.Text = string.Empty;
                txtFloppyFSType.Enabled = false;
                txtFloppyOEMID.Text = string.Empty;
                txtFloppyOEMID.Enabled = false;
                txtFloppySerial.Text = string.Empty;
                txtFloppySerial.Enabled = false;
                BPBVersion = BiosParameterBlockVersion.Dos20;
            }
            else
            {
                cbxFloppyBPB.Checked = true;
                cbxFloppyBPB.Enabled = true;
                lstFloppyBPB.Enabled = true;
                txtFloppyFSType.Text = "FAT12";
                txtFloppyFSType.Enabled = true;
                if (selectedItem == FloppyGeometry.FriendlyName.DMF1024 || selectedItem == FloppyGeometry.FriendlyName.DMF2048)
                    txtFloppyOEMID.Text = "MSDMF3.2";
                else
                    txtFloppyOEMID.Text = "MSDOS5.0";
                txtFloppyOEMID.Enabled = true;
                txtFloppySerial.Text = GenerateVolumeID().ToString("X8");
                txtFloppySerial.Enabled = true;
            }
        }

        private void txtFloppyNumFATs_ValueChanged(object sender, EventArgs e)
        {
            /*if (selectedItem != FloppyGeometry.FriendlyName.Custom && txtFloppyNumFATs.Value != Geometry.NoOfFATs)
            {
                lstFloppyCapacity.SelectedValue = FloppyGeometry.FriendlyName.Custom;
            }*/
        }

        private void txtFloppyReserved_ValueChanged(object sender, EventArgs e)
        {
            /*if (selectedItem != FloppyGeometry.FriendlyName.Custom && txtFloppyReservedSect.Value != Geometry.ReservedSectors)
            {
                lstFloppyCapacity.SelectedValue = FloppyGeometry.FriendlyName.Custom;
            }*/
        }

        private void lstFloppySides_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*if (selectedItem != FloppyGeometry.FriendlyName.Custom && lstFloppySides.SelectedIndex != Geometry.Sides - 1)
            {
                lstFloppyCapacity.SelectedValue = FloppyGeometry.FriendlyName.Custom;
            }*/
        }

        private void txtFloppySPF_ValueChanged(object sender, EventArgs e)
        {
            /*if (selectedItem != FloppyGeometry.FriendlyName.Custom && txtFloppySPF.Value != Geometry.SPF)
            {
                lstFloppyCapacity.SelectedValue = FloppyGeometry.FriendlyName.Custom;
            }*/
        }

        private void txtFloppySPT_ValueChanged(object sender, EventArgs e)
        {
            /*if (selectedItem != FloppyGeometry.FriendlyName.Custom && txtFloppySPT.Value != Geometry.SPT)
            {
                lstFloppyCapacity.SelectedValue = FloppyGeometry.FriendlyName.Custom;
            }*/
        }

        private void txtFloppySPC_ValueChanged(object sender, EventArgs e)
        {
            /*if (selectedItem != FloppyGeometry.FriendlyName.Custom && txtFloppySPC.Value != Geometry.SPC)
            {
                lstFloppyCapacity.SelectedValue = FloppyGeometry.FriendlyName.Custom;
            }*/
        }

        private void txtFloppyBPS_ValueChanged(object sender, EventArgs e)
        {
            /*if (selectedItem != FloppyGeometry.FriendlyName.Custom && txtFloppyBPS.Value != (128 << Geometry.BPS))
            {
                lstFloppyCapacity.SelectedValue = FloppyGeometry.FriendlyName.Custom;
            }*/
        }

        private void txtFloppyTotalSect_ValueChanged(object sender, EventArgs e)
        {
            /*if (selectedItem != FloppyGeometry.FriendlyName.Custom)
            {
                int total_sect = Geometry.Tracks * Geometry.SPT * Geometry.Sides;

                if (txtFloppyTotalSect.Value != total_sect)
                {
                    lstFloppyCapacity.SelectedValue = FloppyGeometry.FriendlyName.Custom;
                }
            }*/
        }

        private void txtFloppyRootDirEntries_ValueChanged(object sender, EventArgs e)
        {
            /*if (selectedItem != FloppyGeometry.FriendlyName.Custom && txtFloppyRootDir.Value != Geometry.RootDirectoryEntries)
            {
                lstFloppyCapacity.SelectedValue = FloppyGeometry.FriendlyName.Custom;
            }*/
        }

        private void txtFloppyTracks_ValueChanged(object sender, EventArgs e)
        {
            /*if (selectedItem != FloppyGeometry.FriendlyName.Custom && txtFloppyTracks.Value != Geometry.Tracks)
            {
                lstFloppyCapacity.SelectedValue = FloppyGeometry.FriendlyName.Custom;
            }*/
        }

        private void txtFloppyMediaDesc_ValueChanged(object sender, EventArgs e)
        {
            /*if (selectedItem != FloppyGeometry.FriendlyName.Custom && txtFloppyMediaDesc.Value != Geometry.MediaDescriptor)
            {
                lstFloppyCapacity.SelectedValue = FloppyGeometry.FriendlyName.Custom;
            }*/
        }

        private void txtFloppySerial_TextChanged(object sender, EventArgs e)
        {
            SerialNumber = txtFloppySerial.Text;
        }

        private void txtFloppyOEMID_TextChanged(object sender, EventArgs e)
        {
            OEMID = txtFloppyOEMID.Text;
        }

        private void txtFloppyLabel_TextChanged(object sender, EventArgs e)
        {
            VolumeLabel = txtFloppyLabel.Text;
        }

        private void txtFloppyFSType_TextChanged(object sender, EventArgs e)
        {
            FileSystemType = txtFloppyFSType.Text;
        }
    }
}