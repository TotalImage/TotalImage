using System;
using System.Windows.Forms;

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
            cbxFloppyBPB.Checked = true;
            lstFloppyBPB.SelectedIndex = 5;
            lstFloppyCapacity.SelectedIndex = 13;
            lstFloppySides.SelectedIndex = 1;
            txtFloppySerial.Text = GenerateVolumeID().ToString("X8");
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabFloppy)
            {
                if (!cbxFloppyBPB.Checked)
                {
                    DialogResult bpb = MessageBox.Show("You chose not to write a DOS BIOS parameter block (BPB) to the boot sector of the image." +
                        " Many programs and operating systems may not recognize the disk because of this." +
                        "\n\nAre you sure you want to create an image without the BPB?", "BIOS Parameter Block", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (bpb == DialogResult.No)
                    {
                        return;
                    }
                }
                /* Create a new floppy disk image */
            }
            else if (tabControl.SelectedTab == tabHDD)
            {
                /* Create a new hard disk image */
            }
            Close();
        }

        private void cbxFloppyBPB_CheckedChanged(object sender, EventArgs e)
        {
            lstFloppyBPB.Enabled = cbxFloppyBPB.Checked;
            txtFloppySerial.Enabled = cbxFloppyBPB.Checked && (lstFloppyBPB.SelectedIndex >= 4);
            //txtFloppyLabel.Enabled = cbxFloppyBPB.Checked && (lstFloppyBPB.SelectedIndex == 5);
            txtFloppyFSType.Enabled = cbxFloppyBPB.Checked && (lstFloppyBPB.SelectedIndex == 5);
        }

        //Generates a random volume ID/serial number for the DOS 3.4+ BPB
        private int GenerateVolumeID()
        {
            Random rnd = new Random();
            return rnd.Next();
        }

        private void lstFloppyBPB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFloppyBPB.SelectedIndex == 5) //DOS 4.0+ BPB
            {
                //txtFloppyLabel.Enabled = true;
                txtFloppyFSType.Enabled = true;
                txtFloppySerial.Enabled = true;
            }
            else if (lstFloppyBPB.SelectedIndex == 4) //DOS 3.4 BPB
            {
                //txtFloppyLabel.Enabled = false;
                txtFloppyFSType.Enabled = false;
                txtFloppySerial.Enabled = true;
            }
            else if (lstFloppyBPB.SelectedIndex <= 3) //DOS 2.0-3.31 BPB
            {
                //txtFloppyLabel.Enabled = false;
                txtFloppyFSType.Enabled = false;
                txtFloppySerial.Enabled = false;
            }
        }

        private void lstFloppyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFloppyCapacity.SelectedIndex == 20) //3680 KiB IBM XDF
            {
                txtFloppyBPS.Value = 512; //BPS varies with XDF
                txtFloppySPC.Value = 2; //Uncertain
                txtFloppySPT.Value = 36; //SPT varies with XDF
                txtFloppyTracks.Value = 80; //Uncertain
                lstFloppySides.SelectedIndex = 1;
                txtFloppySPF.Value = 9; //Uncertain
                txtFloppyRootDirEntries.Value = 240; //Uncertain
                txtFloppyMediaDesc.Value = 0xF0; //Uncertain
                txtFloppyTotalSect.Value = 7360; //Probably wrong due to variable SPT
                txtFloppyReserved.Value = 1; //Uncertain
                txtFloppyNumFATs.Value = 2; //Uncertain
            }
            else if (lstFloppyCapacity.SelectedIndex == 19) //2880 KiB
            {
                txtFloppyBPS.Value = 512;
                txtFloppySPC.Value = 2;
                txtFloppySPT.Value = 36;
                txtFloppyTracks.Value = 80;
                lstFloppySides.SelectedIndex = 1;
                txtFloppySPF.Value = 9;
                txtFloppyRootDirEntries.Value = 240;
                txtFloppyMediaDesc.Value = 0xF0;
                txtFloppyTotalSect.Value = 5760;
                txtFloppyReserved.Value = 1;
                txtFloppyNumFATs.Value = 2;
            }
            else if (lstFloppyCapacity.SelectedIndex == 18) //1840 KiB IBM XDF
            {
                txtFloppyBPS.Value = 512; //BPS varies with XDF
                txtFloppySPC.Value = 2; //Uncertain
                txtFloppySPT.Value = 23; //SPT varies with XDF
                txtFloppyTracks.Value = 80; //Uncertain
                lstFloppySides.SelectedIndex = 1;
                txtFloppySPF.Value = 9; //Uncertain
                txtFloppyRootDirEntries.Value = 224; //Uncertain
                txtFloppyMediaDesc.Value = 0xF0; //Uncertain
                txtFloppyTotalSect.Value = 3680; //Probably wrong due to variable SPT
                txtFloppyReserved.Value = 1; //Uncertain
                txtFloppyNumFATs.Value = 2; //Uncertain
            }
            else if (lstFloppyCapacity.SelectedIndex == 17) //1722 KiB
            {
                txtFloppyBPS.Value = 512;
                txtFloppySPC.Value = 2;
                txtFloppySPT.Value = 21;
                txtFloppyTracks.Value = 82;
                lstFloppySides.SelectedIndex = 1;
                txtFloppySPF.Value = 10;
                txtFloppyRootDirEntries.Value = 224;
                txtFloppyMediaDesc.Value = 0xF0;
                txtFloppyTotalSect.Value = 3444;
                txtFloppyReserved.Value = 1;
                txtFloppyNumFATs.Value = 2;
            }
            else if (lstFloppyCapacity.SelectedIndex == 16) //1680 KiB Microsoft DMF 2048 BPC
            {
                txtFloppyBPS.Value = 512;
                txtFloppySPC.Value = 4;
                txtFloppySPT.Value = 21;
                txtFloppyTracks.Value = 80;
                lstFloppySides.SelectedIndex = 1;
                txtFloppySPF.Value = 3;
                txtFloppyRootDirEntries.Value = 16;
                txtFloppyMediaDesc.Value = 0xF0;
                txtFloppyTotalSect.Value = 3360;
                txtFloppyReserved.Value = 1;
                txtFloppyNumFATs.Value = 2;
            }
            else if (lstFloppyCapacity.SelectedIndex == 15) //1680 KiB Microsoft DMF 1024 BPC
            {
                txtFloppyBPS.Value = 512;
                txtFloppySPC.Value = 2;
                txtFloppySPT.Value = 21;
                txtFloppyTracks.Value = 80;
                lstFloppySides.SelectedIndex = 1;
                txtFloppySPF.Value = 5;
                txtFloppyRootDirEntries.Value = 16;
                txtFloppyMediaDesc.Value = 0xF0;
                txtFloppyTotalSect.Value = 3360;
                txtFloppyReserved.Value = 1;
                txtFloppyNumFATs.Value = 2;
            }
            else if (lstFloppyCapacity.SelectedIndex == 14) //1520 KiB IBM XDF
            {
                txtFloppyBPS.Value = 512; //BPS varies with XDF
                txtFloppySPC.Value = 2; //Uncertain
                txtFloppySPT.Value = 23; //SPT varies with XDF
                txtFloppyTracks.Value = 80; //Uncertain
                lstFloppySides.SelectedIndex = 1;
                txtFloppySPF.Value = 9; //Uncertain
                txtFloppyRootDirEntries.Value = 224; //Uncertain
                txtFloppyMediaDesc.Value = 0xF0; //Uncertain
                txtFloppyTotalSect.Value = 3040; //Probably wrong due to variable SPT
                txtFloppyReserved.Value = 1; //Uncertain
                txtFloppyNumFATs.Value = 2; //Uncertain
            }
            else if (lstFloppyCapacity.SelectedIndex == 13) //1440 KiB
            {
                txtFloppyBPS.Value = 512;
                txtFloppySPC.Value = 1;
                txtFloppySPT.Value = 18;
                txtFloppyTracks.Value = 80;
                lstFloppySides.SelectedIndex = 1;
                txtFloppySPF.Value = 9;
                txtFloppyRootDirEntries.Value = 224;
                txtFloppyMediaDesc.Value = 0xF0;
                txtFloppyTotalSect.Value = 2880;
                txtFloppyReserved.Value = 1;
                txtFloppyNumFATs.Value = 2;
            }
            else if (lstFloppyCapacity.SelectedIndex == 12) //1232 KiB NEC PC-98
            {
                txtFloppyBPS.Value = 1024;
                txtFloppySPC.Value = 1;
                txtFloppySPT.Value = 8;
                txtFloppyTracks.Value = 77;
                lstFloppySides.SelectedIndex = 1;
                txtFloppySPF.Value = 2;
                txtFloppyRootDirEntries.Value = 192;
                txtFloppyMediaDesc.Value = 0xFE;
                txtFloppyTotalSect.Value = 1232;
                txtFloppyReserved.Value = 1;
                txtFloppyNumFATs.Value = 2;
            }
            else if (lstFloppyCapacity.SelectedIndex == 11) //1200 KiB
            {
                txtFloppyBPS.Value = 512;
                txtFloppySPC.Value = 1;
                txtFloppySPT.Value = 15;
                txtFloppyTracks.Value = 80;
                lstFloppySides.SelectedIndex = 1;
                txtFloppySPF.Value = 7;
                txtFloppyRootDirEntries.Value = 224;
                txtFloppyMediaDesc.Value = 0xF9;
                txtFloppyTotalSect.Value = 2400;
                txtFloppyReserved.Value = 1;
                txtFloppyNumFATs.Value = 2;
            }
            else if (lstFloppyCapacity.SelectedIndex == 10) //800 KiB
            {
                txtFloppyBPS.Value = 512; //Uncertain
                txtFloppySPC.Value = 2; //Uncertain
                txtFloppySPT.Value = 10;
                txtFloppyTracks.Value = 80;
                lstFloppySides.SelectedIndex = 1;
                txtFloppySPF.Value = 7; //Uncertain
                txtFloppyRootDirEntries.Value = 224; //Uncertain
                txtFloppyMediaDesc.Value = 0xF9; //Uncertain
                txtFloppyTotalSect.Value = 1600;
                txtFloppyReserved.Value = 1;
                txtFloppyNumFATs.Value = 2;
            }
            else if (lstFloppyCapacity.SelectedIndex == 9) //720 KiB 3.5"
            {
                txtFloppyBPS.Value = 512;
                txtFloppySPC.Value = 2;
                txtFloppySPT.Value = 9;
                txtFloppyTracks.Value = 80;
                lstFloppySides.SelectedIndex = 1;
                txtFloppySPF.Value = 3;
                txtFloppyRootDirEntries.Value = 112;
                txtFloppyMediaDesc.Value = 0xF9;
                txtFloppyTotalSect.Value = 1440;
                txtFloppyReserved.Value = 1;
                txtFloppyNumFATs.Value = 2;
            }
            else if (lstFloppyCapacity.SelectedIndex == 8) //720 KiB 5.25" Tandy 2000
            {
                txtFloppyBPS.Value = 512;
                txtFloppySPC.Value = 4;
                txtFloppySPT.Value = 9;
                txtFloppyTracks.Value = 80;
                lstFloppySides.SelectedIndex = 1;
                txtFloppySPF.Value = 2;
                txtFloppyRootDirEntries.Value = 112;
                txtFloppyMediaDesc.Value = 0xED;
                txtFloppyTotalSect.Value = 1440;
                txtFloppyReserved.Value = 1;
                txtFloppyNumFATs.Value = 2;
            }
            else if (lstFloppyCapacity.SelectedIndex == 7) //640 KiB
            {
                txtFloppyBPS.Value = 512;
                txtFloppySPC.Value = 2;
                txtFloppySPT.Value = 8;
                txtFloppyTracks.Value = 80;
                lstFloppySides.SelectedIndex = 1;
                txtFloppySPF.Value = 2;
                txtFloppyRootDirEntries.Value = 112;
                txtFloppyMediaDesc.Value = 0xFB;
                txtFloppyTotalSect.Value = 1280;
                txtFloppyReserved.Value = 1;
                txtFloppyNumFATs.Value = 2;
            }
            else if (lstFloppyCapacity.SelectedIndex == 6) //400 KiB DEC RX50
            {
                txtFloppyBPS.Value = 512; //Uncertain
                txtFloppySPC.Value = 2; //Uncertain
                txtFloppySPT.Value = 10;
                txtFloppyTracks.Value = 80;
                lstFloppySides.SelectedIndex = 0;
                txtFloppySPF.Value = 2; //Uncertain
                txtFloppyRootDirEntries.Value = 112; //Uncertain
                txtFloppyMediaDesc.Value = 0xF9; //Uncertain
                txtFloppyTotalSect.Value = 800;
                txtFloppyReserved.Value = 1;
                txtFloppyNumFATs.Value = 2;
            }
            else if (lstFloppyCapacity.SelectedIndex == 5) //360 KiB 3.5"
            {
                txtFloppyBPS.Value = 512; //Uncertain
                txtFloppySPC.Value = 2; //Uncertain
                txtFloppySPT.Value = 9;
                txtFloppyTracks.Value = 80;
                lstFloppySides.SelectedIndex = 0;
                txtFloppySPF.Value = 2; //Uncertain
                txtFloppyRootDirEntries.Value = 112; //Uncertain
                txtFloppyMediaDesc.Value = 0xF9; //Uncertain
                txtFloppyTotalSect.Value = 720;
                txtFloppyReserved.Value = 1;
                txtFloppyNumFATs.Value = 2;
            }
            else if (lstFloppyCapacity.SelectedIndex == 4) //360 KiB 5.25"
            {
                txtFloppyBPS.Value = 512;
                txtFloppySPC.Value = 2;
                txtFloppySPT.Value = 9;
                txtFloppyTracks.Value = 40;
                lstFloppySides.SelectedIndex = 1;
                txtFloppySPF.Value = 2;
                txtFloppyRootDirEntries.Value = 112;
                txtFloppyMediaDesc.Value = 0xFD;
                txtFloppyTotalSect.Value = 720;
                txtFloppyReserved.Value = 1;
                txtFloppyNumFATs.Value = 2;
            }
            else if (lstFloppyCapacity.SelectedIndex == 3) //320 KiB 3.5"
            {
                txtFloppyBPS.Value = 512; //Uncertain
                txtFloppySPC.Value = 2; //Uncertain
                txtFloppySPT.Value = 8;
                txtFloppyTracks.Value = 80;
                lstFloppySides.SelectedIndex = 0;
                txtFloppySPF.Value = 2; //Uncertain
                txtFloppyRootDirEntries.Value = 112; //Uncertain
                txtFloppyMediaDesc.Value = 0xF9; //Uncertain
                txtFloppyTotalSect.Value = 640;
                txtFloppyReserved.Value = 1;
                txtFloppyNumFATs.Value = 2;
            }
            else if (lstFloppyCapacity.SelectedIndex == 2) //320 KiB 5.25"
            {
                txtFloppyBPS.Value = 512;
                txtFloppySPC.Value = 2;
                txtFloppySPT.Value = 8;
                txtFloppyTracks.Value = 40;
                lstFloppySides.SelectedIndex = 1;
                txtFloppySPF.Value = 1;
                txtFloppyRootDirEntries.Value = 112;
                txtFloppyMediaDesc.Value = 0xFF;
                txtFloppyTotalSect.Value = 640;
                txtFloppyReserved.Value = 1;
                txtFloppyNumFATs.Value = 2;
            }
            else if (lstFloppyCapacity.SelectedIndex == 1) //180 KiB
            {
                txtFloppyBPS.Value = 512;
                txtFloppySPC.Value = 2;
                txtFloppySPT.Value = 9;
                txtFloppyTracks.Value = 40;
                lstFloppySides.SelectedIndex = 0;
                txtFloppySPF.Value = 1;
                txtFloppyRootDirEntries.Value = 64;
                txtFloppyMediaDesc.Value = 0xFC;
                txtFloppyTotalSect.Value = 360;
                txtFloppyReserved.Value = 1;
                txtFloppyNumFATs.Value = 2;
            }
            else if (lstFloppyCapacity.SelectedIndex == 0) //160 KiB
            {
                txtFloppyBPS.Value = 512;
                txtFloppySPC.Value = 2;
                txtFloppySPT.Value = 8;
                txtFloppyTracks.Value = 40;
                lstFloppySides.SelectedIndex = 0;
                txtFloppySPF.Value = 1;
                txtFloppyRootDirEntries.Value = 64;
                txtFloppyMediaDesc.Value = 0xFE;
                txtFloppyTotalSect.Value = 320;
                txtFloppyReserved.Value = 1;
                txtFloppyNumFATs.Value = 2;
            }
        }
    }
}