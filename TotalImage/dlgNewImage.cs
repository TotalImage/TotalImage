using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using TotalImage.FileSystems;
using TotalImage.ImageFormats;

namespace TotalImage
{
    public partial class dlgNewImage : Form
    {
        public dlgNewImage()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dlgNewSectorImage_Load(object sender, EventArgs e)
        {
            //Default values are for a 1440 KiB disk with DOS 4.0+ BPB
            cbxFloppyBPB.Checked = true;
            lstFloppyBPB.SelectedIndex = 3;
            lstFloppyCapacity.SelectedIndex = 13;
            txtFloppySerial.Text = GenerateVolumeID().ToString("X8");

            //Populate the HDD type list
            foreach(int[] type in hddTable.hdt)
            {
                int capacity = type[0] * type[1] * type[2] * 512 / 1048576;
                string s = capacity.ToString() + " MiB (CHS: " + type[0] + ", " + type[1] + ", " + type[2] + ")";
                lstHDDType.Items.Add(s);
            }

            lstHDDType.Items.Add("Custom...");
            lstHDDType.Items.Add("Custom (large)...");
            lstHDDType.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabFloppy)
            {
                if (!cbxFloppyBPB.Checked && lstFloppyCapacity.SelectedIndex != 6)
                {
                    DialogResult bpb = MessageBox.Show("You chose not to write a DOS BIOS parameter block (BPB) to the boot sector of the image." +
                        " Many programs and operating systems may not recognize the disk because of this." +
                        "\n\nAre you sure you want to create an image without the BPB?", "BIOS Parameter Block", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (bpb == DialogResult.No)
                    {
                        return;
                    }
                }
                else if(cbxFloppyBPB.Checked && lstFloppyCapacity.SelectedIndex == 6) //RX50 disks do not have a BPB...
                {
                    DialogResult bpb = MessageBox.Show("DEC RX50 400 KiB disks do not have an on-disk BIOS paraneter block (BPB), but you still chose to write a DOS BPB to the boot sector of the image." +
                        " The disk may not work in an RX50 drive because of this." +
                        "\n\nAre you sure you want to create an RX50 image with a BPB?", "BIOS Parameter Block", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (bpb == DialogResult.No)
                    {
                        return;
                    }
                }
                
                if(lstFloppyCapacity.SelectedIndex >= 21)
                {
                    /* Create a new floppy disk image with selected parameters */
                }
                else
                {
                    int i = lstFloppyCapacity.SelectedIndex;
                    frmMain main = (frmMain)Application.OpenForms["frmMain"];
                    if (cbxFloppyBPB.Checked)
                    {
                        BiosParameterBlock bpb = new BiosParameterBlock();
                        if (lstFloppyBPB.SelectedIndex <= 1) //DOS 2.0 BPB (on floppies it's the same as 3.31)
                        {
                            bpb = new BiosParameterBlock
                            {
                                BpbVersion = 0x20,
                                BytesPerLogicalSector = (ushort)(128 << floppyTable.fdt[i][7]),
                                HiddenSectors = 0,
                                LargeTotalLogicalSectors = 0,
                                LogicalSectorsPerCluster = (byte)floppyTable.fdt[i][9],
                                LogicalSectorsPerFAT = (ushort)floppyTable.fdt[i][11],
                                MediaDescriptor = (byte)floppyTable.fdt[i][8],
                                NumberOfFATs = (byte)floppyTable.fdt[i][10],
                                NumberOfHeads = (ushort)floppyTable.fdt[i][1],
                                PhysicalSectorsPerTrack = (ushort)floppyTable.fdt[i][6],
                                ReservedLogicalSectors = (ushort)floppyTable.fdt[i][13],
                                RootDirectoryEntries = (ushort)floppyTable.fdt[i][12],
                                VolumeLabel = Encoding.ASCII.GetBytes(txtFloppyLabel.Text.ToUpper()),
                                //TLS = (ushort)(bpb.PhysicalSectorsPerTrack * bpb.NumberOfHeads * floppyTable.fdt[i][5])
                            };

                            string oemID = txtFloppyOEMID.Text.ToUpper();
                            main.image.CreateImage(bpb, oemID, floppyTable.fdt[i][5]);
                        }
                        else if (lstFloppyBPB.SelectedIndex == 2) //DOS 3.4 BPB
                        {
                            bpb = new BiosParameterBlock40
                            {
                                BpbVersion = 0x34,
                                BytesPerLogicalSector = (ushort)(128 << floppyTable.fdt[i][7]),
                                ExtendedBootSignature = 0x28,
                                FileSystemType = new byte[1],
                                Flags = 0,
                                HiddenSectors = 0,
                                LargeTotalLogicalSectors = 0,
                                LogicalSectorsPerCluster = (byte)floppyTable.fdt[i][9],
                                LogicalSectorsPerFAT = (ushort)floppyTable.fdt[i][11],
                                MediaDescriptor = (byte)floppyTable.fdt[i][8],
                                NumberOfFATs = (byte)floppyTable.fdt[i][10],
                                NumberOfHeads = (ushort)floppyTable.fdt[i][1],
                                PhysicalDriveNumber = 0,
                                PhysicalSectorsPerTrack = (ushort)floppyTable.fdt[i][6],
                                ReservedLogicalSectors = (ushort)floppyTable.fdt[i][13],
                                RootDirectoryEntries = (ushort)floppyTable.fdt[i][12],
                                VolumeLabel = Encoding.ASCII.GetBytes(txtFloppyLabel.Text.ToUpper()),
                                VolumeSerialNumber = uint.Parse(txtFloppySerial.Text),
                                //TLS = (ushort)(bpb.PhysicalSectorsPerTrack * bpb.NumberOfHeads * floppyTable.fdt[i][5])
                            };

                            string oemID = txtFloppyOEMID.Text.ToUpper();
                            main.image.CreateImage(bpb, oemID, floppyTable.fdt[i][5]);
                        }
                        else if (lstFloppyBPB.SelectedIndex == 3) //DOS 4.0+ BPB
                        {
                            bpb = new BiosParameterBlock40();
                            // {
                            bpb.BpbVersion = 0x40;
                            bpb.BytesPerLogicalSector = (ushort)(128 << floppyTable.fdt[i][7]);
                            ((BiosParameterBlock40)bpb).ExtendedBootSignature = 0x29;
                            ((BiosParameterBlock40)bpb).FileSystemType = Encoding.ASCII.GetBytes(txtFloppyFSType.Text.ToUpper());
                            ((BiosParameterBlock40)bpb).Flags = 0;
                            //bpb.TLS = (ushort)(bpb.PhysicalSectorsPerTrack * bpb.NumberOfHeads * floppyTable.fdt[i][5]);
                            bpb.HiddenSectors = 0;
                            bpb.LargeTotalLogicalSectors = 0;
                            bpb.LogicalSectorsPerCluster = floppyTable.fdt[i][9];
                            bpb.LogicalSectorsPerFAT = floppyTable.fdt[i][11];
                            bpb.MediaDescriptor = floppyTable.fdt[i][8];
                            bpb.NumberOfFATs = floppyTable.fdt[i][10];
                            bpb.NumberOfHeads = floppyTable.fdt[i][1];
                            ((BiosParameterBlock40)bpb).PhysicalDriveNumber = 0;
                            bpb.PhysicalSectorsPerTrack = floppyTable.fdt[i][6];
                            bpb.ReservedLogicalSectors = floppyTable.fdt[i][13];
                            bpb.RootDirectoryEntries = floppyTable.fdt[i][12];
                            ((BiosParameterBlock40)bpb).VolumeLabel = Encoding.ASCII.GetBytes(txtFloppyLabel.Text.ToUpper());
                            ((BiosParameterBlock40)bpb).VolumeSerialNumber = uint.Parse(txtFloppySerial.Text, System.Globalization.NumberStyles.HexNumber);
                                
                            //};
                            //Console.WriteLine(bpb.TLS);
                            string oemID = txtFloppyOEMID.Text.ToUpper();
                            main.image.CreateImage(bpb, oemID, floppyTable.fdt[i][5]);
                        }
                    }
                    
                    else
                    {
                        /* Create a new floppy disk image without a BPB */
                        //image.CreateImageNoBpb();
                    }
                    /* Create a new floppy disk image with standard parameters */
                }
            }
            else if (tabControl.SelectedTab == tabHDD)
            {
                /* Create a new hard disk image with selected parameters*/
            }
            Close();
        }

        private void cbxFloppyBPB_CheckedChanged(object sender, EventArgs e)
        {
            //Disable volume ID and FS type fields since they were only added in DOS 3.4/4.0
            lstFloppyBPB.Enabled = cbxFloppyBPB.Checked;
            txtFloppySerial.Enabled = cbxFloppyBPB.Checked && (lstFloppyBPB.SelectedIndex >= 2);
            txtFloppyFSType.Enabled = cbxFloppyBPB.Checked && (lstFloppyBPB.SelectedIndex == 3);
        }

        //Generates a random volume ID/serial number for the DOS 3.4+ BPB
        private int GenerateVolumeID()
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
            }
            else if (lstFloppyBPB.SelectedIndex == 2) //DOS 3.4 BPB
            {
                txtFloppyFSType.Enabled = false;
                txtFloppySerial.Enabled = true;
            }
            else if (lstFloppyBPB.SelectedIndex <= 1) //DOS 2.0-3.31 BPB
            {
                txtFloppyFSType.Enabled = false;
                txtFloppySerial.Enabled = false;
            }
        }

        private void lstFloppyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;
            if (i < lstFloppyCapacity.Items.Count - 1) //Ignore the last item ("Custom...")
            {
                lstFloppySides.SelectedIndex = floppyTable.fdt[i][1] - 1;
                txtFloppyTracks.Value = floppyTable.fdt[i][5];
                txtFloppySPT.Value = floppyTable.fdt[i][6];
                txtFloppyBPS.Value = (128 << floppyTable.fdt[i][7]);
                txtFloppyMediaDesc.Value = floppyTable.fdt[i][8];
                txtFloppySPC.Value = floppyTable.fdt[i][9];
                txtFloppyNumFATs.Value = floppyTable.fdt[i][10];
                txtFloppySPF.Value = floppyTable.fdt[i][11];
                txtFloppyRootDir.Value = floppyTable.fdt[i][12];
                txtFloppyReservedSect.Value = floppyTable.fdt[i][13];
                txtFloppyTotalSect.Value = floppyTable.fdt[i][5] * floppyTable.fdt[i][6] * floppyTable.fdt[i][1];
            }
            if(lstFloppyCapacity.SelectedIndex == 6) //DEC RX50 disks don't have a BPB...
            {
                cbxFloppyBPB.Checked = false;
            }
            else
            {
                cbxFloppyBPB.Checked = true;
            }
        }

        private void lstHDDCapacity_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = lstHDDType.SelectedIndex;
            if (lstHDDType.SelectedIndex < lstHDDType.Items.Count - 2) //Ignore the last two items - "Custom..." and "Custom (large)..."
            {
                //HDD cpacity is displayed in MiB
                txtHDDCapacity.Value = (hddTable.hdt[i][0] * hddTable.hdt[i][1] * hddTable.hdt[i][2] * 512 / 1048576);
                txtHDDCylinders.Value = hddTable.hdt[i][0];
                txtHDDHeads.Value = hddTable.hdt[i][1];
                txtHDDSectors.Value = hddTable.hdt[i][2];
            }
        }

        private void txtFloppyNumFATs_ValueChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;

            if(lstFloppyCapacity.SelectedIndex < lstFloppyCapacity.Items.Count -1 && txtFloppyNumFATs.Value != floppyTable.fdt[i][10])
            {
                lstFloppyCapacity.SelectedIndex = 21;
            }
        }

        private void txtFloppyReserved_ValueChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;

            if (lstFloppyCapacity.SelectedIndex < lstFloppyCapacity.Items.Count - 1 && txtFloppyReservedSect.Value != floppyTable.fdt[i][13])
            {
                lstFloppyCapacity.SelectedIndex = 21;
            }
        }

        private void lstFloppySides_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;

            if (lstFloppyCapacity.SelectedIndex < lstFloppyCapacity.Items.Count - 1 && lstFloppySides.SelectedIndex != floppyTable.fdt[i][1] - 1)
            {
                lstFloppyCapacity.SelectedIndex = 21;
            }
        }

        private void txtFloppySPF_ValueChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;

            if (lstFloppyCapacity.SelectedIndex < lstFloppyCapacity.Items.Count - 1 && txtFloppySPF.Value != floppyTable.fdt[i][11])
            {
                lstFloppyCapacity.SelectedIndex = 21;
            }
        }

        private void txtFloppySPT_ValueChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;

            if (lstFloppyCapacity.SelectedIndex < lstFloppyCapacity.Items.Count - 1 && txtFloppySPT.Value != floppyTable.fdt[i][6])
            {
                lstFloppyCapacity.SelectedIndex = 21;
            }
        }

        private void txtFloppySPC_ValueChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;

            if (lstFloppyCapacity.SelectedIndex < lstFloppyCapacity.Items.Count - 1 && txtFloppySPC.Value != floppyTable.fdt[i][9])
            {
                lstFloppyCapacity.SelectedIndex = 21;
            }
        }

        private void txtFloppyBPS_ValueChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;

            if (lstFloppyCapacity.SelectedIndex < lstFloppyCapacity.Items.Count - 1 && txtFloppyBPS.Value != (128 << floppyTable.fdt[i][7]))
            {
                lstFloppyCapacity.SelectedIndex = 21;
            }
        }

        private void txtFloppyTotalSect_ValueChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;
            if (lstFloppyCapacity.SelectedIndex < lstFloppyCapacity.Items.Count - 1)
            {
                int total_sect = floppyTable.fdt[i][5] * floppyTable.fdt[i][6] * floppyTable.fdt[i][1];
                if (txtFloppyTotalSect.Value != total_sect)
                {
                    lstFloppyCapacity.SelectedIndex = 21;
                }
            }
        }

        private void txtFloppyRootDirEntries_ValueChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;

            if (lstFloppyCapacity.SelectedIndex < lstFloppyCapacity.Items.Count - 1 && txtFloppyRootDir.Value != floppyTable.fdt[i][12])
            {
                lstFloppyCapacity.SelectedIndex = 21;
            }
        }

        private void txtFloppyTracks_ValueChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;

            if (lstFloppyCapacity.SelectedIndex < lstFloppyCapacity.Items.Count - 1 && txtFloppyTracks.Value != floppyTable.fdt[i][5])
            {
                lstFloppyCapacity.SelectedIndex = 21;
            }
        }

        private void txtFloppyMediaDesc_ValueChanged(object sender, EventArgs e)
        {
            int i = lstFloppyCapacity.SelectedIndex;

            if (lstFloppyCapacity.SelectedIndex < lstFloppyCapacity.Items.Count - 1 && txtFloppyMediaDesc.Value != floppyTable.fdt[i][8])
            {
                lstFloppyCapacity.SelectedIndex = 21;
            }
        }

        private void txtHDDCylinders_ValueChanged(object sender, EventArgs e)
        {
            int i = lstHDDType.SelectedIndex;

            if(lstHDDType.SelectedIndex < lstHDDType.Items.Count -2 && txtHDDCylinders.Value != hddTable.hdt[i][0])
            {
                lstHDDType.SelectedIndex = lstHDDType.Items.Count - 1;
            }
        }
    }
}