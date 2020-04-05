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

        private void rbnFloppy_CheckedChanged(object sender, EventArgs e)
        {
            gbxFloppyCapacity.Enabled = true;
            gbxHardDiskCapacity.Enabled = false;

        }

        private void rbnHardDisk_CheckedChanged(object sender, EventArgs e)
        {
            gbxFloppyCapacity.Enabled = false;
            gbxHardDiskCapacity.Enabled = true;
        }

        private void dlgNewSectorImage_Load(object sender, EventArgs e)
        {
            rbnFloppy.Checked = true;
            rbn144m.Checked = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            frmMain.surfaceImage = false;

            if (rbnFloppy.Checked)
            {
                frmMain frm = (frmMain)Application.OpenForms["frmMain"];
                if (rbnCustom.Checked)
                {
                    //Custom image parameters selected
                    //frm.imgMan.CreateCustomImage();
                    Close();
                }

                RadioButton checkedRbn;
                foreach (RadioButton rbn in gbxFloppyCapacity.Controls)
                {
                    if (rbn.Checked) {
                        checkedRbn = rbn;
                        frm.imgMan.CreateImage(Convert.ToInt32(checkedRbn.Tag));
                        break;
                    }
                }
                Close();
            }
            else if (rbnHardDisk.Checked)
            {
                /* Let's create a new hard disk image */
            }
        }
    }
}
