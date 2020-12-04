using System;
using System.Windows.Forms;
using TotalImage.FileSystems.FAT;
using TotalImage.DiskGeometries;

namespace TotalImage
{
    public partial class dlgNewImage : Form
    {
        public string OEMID { get; private set; } = "";
        public string VolumeLabel { get; private set; } = "";
        public string SerialNumber { get; private set; } = "";
        public string FileSystemType { get; private set; } = "";
        public byte NumberOfFATs { get; private set; }
        public ushort BytesPerSector { get; private set; }
        public byte SectorsPerCluster { get; private set; }
        public byte SectorsPerTrack { get; private set; }
        public byte SectorsPerFAT { get; private set; }
        public byte ReservedSectors { get; private set; }
        public ushort TotalSectors { get; private set; }
        public ushort RootDirEntries { get; private set; }
        public byte TracksPerSide { get; private set; }
        public byte MediaDescriptor { get; private set; }
        public byte NumberOfSides { get; private set; }
        public bool WriteBPB { get; private set; }
        public BiosParameterBlockVersion BPBVersion { get; private set; }

        public dlgNewImage()
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
            lstFloppyCapacity.SelectedItem = "1440 KiB";
            txtFloppySerial.Text = GenerateVolumeID().ToString("X8");
            SerialNumber = txtFloppySerial.Text;
            OEMID = txtFloppyOEMID.Text;
            VolumeLabel = txtFloppyLabel.Text;
            FileSystemType = txtFloppyFSType.Text;
            NumberOfFATs = Convert.ToByte(txtFloppyNumFATs.Value);
            ReservedSectors = Convert.ToByte(txtFloppyReservedSect.Value);
            NumberOfSides = (byte)(lstFloppySides.SelectedIndex + 1);
            SectorsPerFAT = Convert.ToByte(txtFloppySPF.Value);
            SectorsPerTrack = Convert.ToByte(txtFloppySPT.Value);
            SectorsPerCluster = Convert.ToByte(txtFloppySPC.Value);
            BytesPerSector = Convert.ToUInt16(txtFloppyBPS.Value);
            TotalSectors = Convert.ToUInt16(txtFloppyTotalSect.Value);
            RootDirEntries = Convert.ToUInt16(txtFloppyRootDir.Value);
            TracksPerSide = Convert.ToByte(txtFloppyTracks.Value);
            MediaDescriptor = Convert.ToByte(txtFloppyMediaDesc.Value);
        }

        //TODO: Perform some validation of parameters before leaving in case the user tries to create an impossible image
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabFloppy)
            {
                if (!cbxFloppyBPB.Checked)
                {
                    DialogResult noBpb = MessageBox.Show("You chose not to write a DOS BIOS parameter block (BPB) to the boot sector of the image." +
                        " Many programs and operating systems may not recognize the disk because of this." +
                        "\n\nAre you sure you want to create an image without the BPB?", "No BPB", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (noBpb == DialogResult.No)
                    {
                        return;
                    }
                }
            }
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
            int i = lstFloppyCapacity.SelectedIndex;
            if (i < lstFloppyCapacity.Items.Count - 1) //Ignore the last item ("Custom...")
            {
                lstFloppySides.SelectedIndex = FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].Sides - 1;
                txtFloppyTracks.Value = FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].Tracks;
                txtFloppySPT.Value = FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].SPT;
                txtFloppyBPS.Value = 128 << FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].BPS;
                txtFloppyMediaDesc.Value = FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].MediaDescriptor;
                txtFloppySPC.Value = FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].SPC;
                txtFloppyNumFATs.Value = FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].NoOfFATs;
                txtFloppySPF.Value = FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].SPF;
                txtFloppyRootDir.Value = FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].RootDirectoryEntries;
                txtFloppyReservedSect.Value = FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].ReservedSectors;
                txtFloppyTotalSect.Value = 
                    FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].Tracks *
                    FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].SPT *
                    FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].Sides;
            }
            if(lstFloppyCapacity.SelectedItem.Equals("800 KiB (Acorn BBC Master 512)"))
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
            }
            else
            {
                cbxFloppyBPB.Checked = true;
                cbxFloppyBPB.Enabled = true;
                lstFloppyBPB.Enabled = true;
                txtFloppyFSType.Text = "FAT12";
                txtFloppyFSType.Enabled = true;
                txtFloppyOEMID.Text = "MSDOS5.0";
                txtFloppyOEMID.Enabled = true;
                txtFloppySerial.Text = GenerateVolumeID().ToString("X8");
                txtFloppySerial.Enabled = true;
            }
        }

        private void txtFloppyNumFATs_ValueChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;
            NumberOfFATs = Convert.ToByte(txtFloppyNumFATs.Value);
            if (lstFloppyCapacity.SelectedIndex < lstFloppyCapacity.Items.Count - 1 && txtFloppyNumFATs.Value != FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].NoOfFATs)
            {
                lstFloppyCapacity.SelectedItem = "Custom...";
            }
        }

        private void txtFloppyReserved_ValueChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;
            ReservedSectors = Convert.ToByte(txtFloppyReservedSect.Value);
            if (lstFloppyCapacity.SelectedIndex < lstFloppyCapacity.Items.Count - 1 && txtFloppyReservedSect.Value != FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].ReservedSectors)
            {
                lstFloppyCapacity.SelectedItem = "Custom...";
            }
        }

        private void lstFloppySides_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;
            NumberOfSides = (byte)(lstFloppySides.SelectedIndex + 1);
            if (lstFloppyCapacity.SelectedIndex < lstFloppyCapacity.Items.Count - 1 && lstFloppySides.SelectedIndex != FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].Sides - 1)
            {
                lstFloppyCapacity.SelectedItem = "Custom...";
            }
        }

        private void txtFloppySPF_ValueChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;
            SectorsPerFAT = Convert.ToByte(txtFloppySPF.Value);
            if (lstFloppyCapacity.SelectedIndex < lstFloppyCapacity.Items.Count - 1 && txtFloppySPF.Value != FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].SPF)
            {
                lstFloppyCapacity.SelectedItem = "Custom...";
            }
        }

        private void txtFloppySPT_ValueChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;
            SectorsPerTrack = Convert.ToByte(txtFloppySPT.Value);
            if (lstFloppyCapacity.SelectedIndex < lstFloppyCapacity.Items.Count - 1 && txtFloppySPT.Value != FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].SPT)
            {
                lstFloppyCapacity.SelectedItem = "Custom...";
            }
        }

        private void txtFloppySPC_ValueChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;
            SectorsPerCluster = Convert.ToByte(txtFloppySPC.Value);
            if (lstFloppyCapacity.SelectedIndex < lstFloppyCapacity.Items.Count - 1 && txtFloppySPC.Value != FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].SPC)
            {
                lstFloppyCapacity.SelectedItem = "Custom...";
            }
        }

        private void txtFloppyBPS_ValueChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;
            BytesPerSector = Convert.ToUInt16(txtFloppyBPS.Value);
            if (lstFloppyCapacity.SelectedIndex < lstFloppyCapacity.Items.Count - 1 && txtFloppyBPS.Value != (128 << FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].BPS))
            {
                lstFloppyCapacity.SelectedItem = "Custom...";
            }
        }

        private void txtFloppyTotalSect_ValueChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;
            TotalSectors = Convert.ToUInt16(txtFloppyTotalSect.Value);
            if (lstFloppyCapacity.SelectedIndex < lstFloppyCapacity.Items.Count - 1)
            {
                int total_sect =
                    FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].Tracks *
                    FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].SPT *
                    FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].Sides;

                if (txtFloppyTotalSect.Value != total_sect)
                {
                    lstFloppyCapacity.SelectedItem = "Custom...";
                }
            }
        }

        private void txtFloppyRootDirEntries_ValueChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;
            RootDirEntries = Convert.ToUInt16(txtFloppyRootDir.Value);
            if (lstFloppyCapacity.SelectedIndex < lstFloppyCapacity.Items.Count - 1 && txtFloppyRootDir.Value != FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].RootDirectoryEntries /*floppyTable.fdt[i][12]*/)
            {
                lstFloppyCapacity.SelectedItem = "Custom...";
            }
        }

        private void txtFloppyTracks_ValueChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;
            TracksPerSide = Convert.ToByte(txtFloppyTracks.Value);
            if (lstFloppyCapacity.SelectedIndex < lstFloppyCapacity.Items.Count - 1 && txtFloppyTracks.Value != FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].Tracks /*floppyTable.fdt[i][5]*/)
            {
                lstFloppyCapacity.SelectedItem = "Custom...";
            }
        }

        private void txtFloppyMediaDesc_ValueChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;
            MediaDescriptor = Convert.ToByte(txtFloppyMediaDesc.Value);
            if (lstFloppyCapacity.SelectedIndex < lstFloppyCapacity.Items.Count - 1 && txtFloppyMediaDesc.Value != FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].MediaDescriptor /*floppyTable.fdt[i][8]*/)
            {
                lstFloppyCapacity.SelectedItem = "Custom...";
            }
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