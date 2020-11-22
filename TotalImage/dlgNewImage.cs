using System;
using System.Windows.Forms;
using TotalImage.FileSystems.FAT;
using TotalImage.DiskGeometries;

namespace TotalImage
{
    public partial class dlgNewImage : Form
    {
        public string OEMID { get; private set; }
        public string VolumeLabel { get; private set; }
        public string SerialNumber { get; private set; }
        public string FileSystemType { get; private set; }
        public byte NumberOfFATs { get; private set; }
        public ushort BytesPerSector { get; private set; }
        public byte SectorsPerCluster { get; private set; }
        public ushort SectorsPerTrack { get; private set; }
        public ushort SectorsPerFAT { get; private set; }
        public ushort ReservedSectors { get; private set; }
        public ushort TotalSectors { get; private set; }
        public ushort RootDirEntries { get; private set; }
        public byte TracksPerSide { get; private set; }
        public byte MediaDescriptor { get; private set; }
        public ushort NumberOfSides { get; private set; }
        public bool WriteBPB { get; private set; }
        public BiosParameterBlockVersion BPBVersion { get; private set; }

        public dlgNewImage()
        {
            InitializeComponent();
        }

        /*Value changed handlers should perform additional sanity checks and recalculate relevant values to make sure the user doesn't
         *try creating an impossible image! */

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
            ReservedSectors = Convert.ToUInt16(txtFloppyReservedSect.Value);
            NumberOfSides = (ushort)(lstFloppySides.SelectedIndex + 1);
            SectorsPerFAT = Convert.ToUInt16(txtFloppySPF.Value);
            SectorsPerTrack = Convert.ToUInt16(txtFloppySPT.Value);
            SectorsPerCluster = Convert.ToByte(txtFloppySPC.Value);
            BytesPerSector = Convert.ToUInt16(txtFloppyBPS.Value);
            TotalSectors = Convert.ToUInt16(txtFloppyTotalSect.Value);
            RootDirEntries = Convert.ToUInt16(txtFloppyRootDir.Value);
            TracksPerSide = Convert.ToByte(txtFloppyTracks.Value);
            MediaDescriptor = Convert.ToByte(txtFloppyMediaDesc.Value);

            //Populate the HDD type list
            /*foreach (int[] type in hddTable.hdt)
            {
                int capacity = type[0] * type[1] * type[2] * 512 / 1048576;
                string s = capacity.ToString() + " MiB (CHS: " + type[0] + ", " + type[1] + ", " + type[2] + ")";
                lstHDDType.Items.Add(s);
            }

            lstHDDType.Items.Add("Custom...");
            lstHDDType.SelectedIndex = 0;*/
        }

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

        //Generates a random volume ID/serial number for DOS 3.4+ BPB
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
                lstFloppySides.SelectedIndex = FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].Sides - 1; /*floppyTable.fdt[i][1] - 1;*/
                txtFloppyTracks.Value = FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].Tracks; /*floppyTable.fdt[i][5];*/
                txtFloppySPT.Value = FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].SPT; /*floppyTable.fdt[i][6];*/
                txtFloppyBPS.Value = 128 << FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].BPS; /*floppyTable.fdt[i][7]*/
                txtFloppyMediaDesc.Value = FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].MediaDescriptor; /*floppyTable.fdt[i][8]*/;
                txtFloppySPC.Value = FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].SPC; /*floppyTable.fdt[i][9];*/
                txtFloppyNumFATs.Value = FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].NoOfFATs; /*floppyTable.fdt[i][10];*/
                txtFloppySPF.Value = FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].SPF; /*floppyTable.fdt[i][11];*/
                txtFloppyRootDir.Value = FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].RootDirectoryEntries; /*floppyTable.fdt[i][12];*/
                txtFloppyReservedSect.Value = FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].ReservedSectors; /*floppyTable.fdt[i][13];*/
                txtFloppyTotalSect.Value = 
                    FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].Tracks *
                    FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].SPT *
                    FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].Sides; /*floppyTable.fdt[i][5] * floppyTable.fdt[i][6] * floppyTable.fdt[i][1];*/
            }
        }

        //Commented out until we implement HDD support
        private void lstHDDCapacity_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*int i = lstHDDType.SelectedIndex;
            if (lstHDDType.SelectedIndex < lstHDDType.Items.Count - 1) //Ignore the last item - "Custom..."
            {
                //HDD cpacity is displayed in MiB
                txtHDDCapacity.Value = (hddTable.hdt[i][0] * hddTable.hdt[i][1] * hddTable.hdt[i][2] * 512 / 1048576);
                txtHDDCylinders.Value = hddTable.hdt[i][0];
                txtHDDHeads.Value = hddTable.hdt[i][1];
                txtHDDSectors.Value = hddTable.hdt[i][2];
            }*/
        }

        private void txtFloppyNumFATs_ValueChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;
            NumberOfFATs = Convert.ToByte(txtFloppyNumFATs.Value);
            if (lstFloppyCapacity.SelectedIndex < lstFloppyCapacity.Items.Count - 1 && txtFloppyNumFATs.Value != FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].NoOfFATs /*floppyTable.fdt[i][10]*/)
            {
                lstFloppyCapacity.SelectedItem = "Custom...";
            }
        }

        private void txtFloppyReserved_ValueChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;
            ReservedSectors = Convert.ToUInt16(txtFloppyReservedSect.Value);
            if (lstFloppyCapacity.SelectedIndex < lstFloppyCapacity.Items.Count - 1 && txtFloppyReservedSect.Value != FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].ReservedSectors /*floppyTable.fdt[i][13]*/)
            {
                lstFloppyCapacity.SelectedItem = "Custom...";
            }
        }

        private void lstFloppySides_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;
            NumberOfSides = (ushort)(lstFloppySides.SelectedIndex + 1);
            if (lstFloppyCapacity.SelectedIndex < lstFloppyCapacity.Items.Count - 1 && lstFloppySides.SelectedIndex != FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].Sides - 1 /*floppyTable.fdt[i][1] - 1*/)
            {
                lstFloppyCapacity.SelectedItem = "Custom...";
            }
        }

        private void txtFloppySPF_ValueChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;
            SectorsPerFAT = Convert.ToUInt16(txtFloppySPF.Value);
            if (lstFloppyCapacity.SelectedIndex < lstFloppyCapacity.Items.Count - 1 && txtFloppySPF.Value != FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].SPF /*floppyTable.fdt[i][11]*/)
            {
                lstFloppyCapacity.SelectedItem = "Custom...";
            }
        }

        private void txtFloppySPT_ValueChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;
            SectorsPerTrack = Convert.ToUInt16(txtFloppySPT.Value);
            if (lstFloppyCapacity.SelectedIndex < lstFloppyCapacity.Items.Count - 1 && txtFloppySPT.Value != FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].SPT /*floppyTable.fdt[i][6]*/)
            {
                lstFloppyCapacity.SelectedItem = "Custom...";
            }
        }

        private void txtFloppySPC_ValueChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;
            SectorsPerCluster = Convert.ToByte(txtFloppySPC.Value);
            if (lstFloppyCapacity.SelectedIndex < lstFloppyCapacity.Items.Count - 1 && txtFloppySPC.Value != FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].SPC /*floppyTable.fdt[i][9]*/)
            {
                lstFloppyCapacity.SelectedItem = "Custom...";
            }
        }

        private void txtFloppyBPS_ValueChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;
            BytesPerSector = Convert.ToUInt16(txtFloppyBPS.Value);
            if (lstFloppyCapacity.SelectedIndex < lstFloppyCapacity.Items.Count - 1 && txtFloppyBPS.Value != (128 << FloppyGeometry.KnownGeometries[(FloppyGeometry.FriendlyName)i].BPS /*floppyTable.fdt[i][7]*/))
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
                /*floppyTable.fdt[i][5] * floppyTable.fdt[i][6] * floppyTable.fdt[i][1];*/
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

        //Commented out until we add HDD support
        private void txtHDDCylinders_ValueChanged(object sender, EventArgs e)
        {
            /*int i = lstHDDType.SelectedIndex;

            if (lstHDDType.SelectedIndex < lstHDDType.Items.Count - 2 && txtHDDCylinders.Value != hddTable.hdt[i][0])
            {
                lstHDDType.SelectedIndex = lstHDDType.Items.Count - 1;
            }*/
        }

        //Temporarily disabled until hard drive support is implemented...
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = tabControl.SelectedTab != tabHDD;
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