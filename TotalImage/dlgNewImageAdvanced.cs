using System;
using System.Windows.Forms;
using TotalImage.DiskGeometries;

namespace TotalImage
{
    public partial class dlgNewImageAdvanced : Form
    {
        private dlgNewImage baseDialog;

        public dlgNewImageAdvanced()
        {
            InitializeComponent();
        }

        //TODO: Should the capacities list be loaded from the KnownGeometries list or something like that?
        private void dlgNewImageAdvanced_Load(object sender, EventArgs e)
        {           
            baseDialog = (dlgNewImage)Application.OpenForms["dlgNewImage"];

            lstFloppySides.SelectedIndex = baseDialog.Geometry.Sides - 1;
            txtFloppyTracks.Value = baseDialog.Geometry.Tracks;
            txtFloppySPT.Value = baseDialog.Geometry.SPT;
            txtFloppyBPS.Value = 128 << baseDialog.Geometry.BPS;
            txtFloppyMediaDesc.Value = baseDialog.Geometry.MediaDescriptor;
            txtFloppySPC.Value = baseDialog.Geometry.SPC;
            txtFloppyNumFATs.Value = baseDialog.Geometry.NoOfFATs;
            txtFloppySPF.Value = baseDialog.Geometry.SPF;
            txtFloppyRootDir.Value = baseDialog.Geometry.RootDirectoryEntries;
            txtFloppyReservedSect.Value = baseDialog.Geometry.ReservedSectors;
            txtFloppyTotalSect.Value = baseDialog.Geometry.Tracks * baseDialog.Geometry.SPT * baseDialog.Geometry.Sides;

            if(baseDialog.selectedItem == FloppyGeometry.FriendlyName.Acorn800k)
            {
                txtFloppyOEMID.Text = "";
                txtFloppyOEMID.Enabled = false;
                txtFloppyFSType.Text = "";
                txtFloppyFSType.Enabled = false;
                txtFloppySerial.Text = "";
                txtFloppySerial.Enabled = false;
            }
            else
            {
                txtFloppyOEMID.Enabled = true;
                txtFloppySerial.Enabled = baseDialog.cbxFloppyBPB.Checked && (baseDialog.lstFloppyBPB.SelectedIndex >= 1);
                txtFloppyFSType.Enabled = baseDialog.cbxFloppyBPB.Checked && (baseDialog.lstFloppyBPB.SelectedIndex == 2);

                txtFloppyOEMID.Text = baseDialog.OEMID;
                txtFloppySerial.Text = baseDialog.SerialNumber;
                txtFloppyFSType.Text = baseDialog.FileSystemType;
            }
        }
        
        private void btnOK_Click(object sender, EventArgs e)
        {
            //TODO: Perform some validation of parameters before leaving in case the user tries to create an impossible image
        }
    }
}