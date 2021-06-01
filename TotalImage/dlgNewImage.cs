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
        private FloppyGeometry.FriendlyName selectedItem;
        public FloppyGeometry Geometry { get; private set; }
        public string VolumeLabel { get; private set; } = "";
        public string OEMID { get; private set; } = "";
        public string SerialNumber { get; private set; } = "";
        public string FileSystemType { get; private set; } = "";
        public bool WriteBPB { get; private set; }
        public BiosParameterBlockVersion BPBVersion { get; private set; }

        public dlgNewImage()
        {
            InitializeComponent();
        }

        private void dlgNewImage_Load(object sender, EventArgs e)
        {
            cbxFloppyBPB.Checked = true;
            WriteBPB = true;
            lstFloppyBPB.SelectedIndex = 3;
            BPBVersion = BiosParameterBlockVersion.Dos40;

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
            VolumeLabel = txtFloppyLabel.Text;
        }

        private void cbxFloppyBPB_CheckedChanged(object sender, EventArgs e)
        {
            lstFloppyBPB.Enabled = cbxFloppyBPB.Checked;
            WriteBPB = cbxFloppyBPB.Checked;
        }

        private void lstFloppyBPB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFloppyBPB.SelectedIndex == 3) //DOS 4.0+ BPB
            {
                BPBVersion = BiosParameterBlockVersion.Dos40;
            }
            else if (lstFloppyBPB.SelectedIndex == 2) //DOS 3.4 BPB
            {
                BPBVersion = BiosParameterBlockVersion.Dos34;
            }
            else if (lstFloppyBPB.SelectedIndex <= 1) //DOS 2.0-3.31 BPB
            {
                BPBVersion = BiosParameterBlockVersion.Dos20;
            }
        }

        private void lstFloppyGeometries_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ = Enum.TryParse(lstFloppyGeometries.SelectedValue.ToString(), out selectedItem);

            /*if (selectedItem != FloppyGeometry.FriendlyName.Custom)
            {*/
                Geometry = FloppyGeometry.KnownGeometries[selectedItem];
            //}

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
        }

        private void txtFloppyLabel_TextChanged(object sender, EventArgs e)
        {
            VolumeLabel = txtFloppyLabel.Text;
        }

        private void btnAdvanced_Click(object sender, EventArgs e)
        {
            using dlgNewImageAdvanced dlgAdvanced = new dlgNewImageAdvanced();
            dlgAdvanced.ShowDialog();
        }

        private void rbnFloppyDisk_CheckedChanged(object sender, EventArgs e)
        {
            lblFloppyGeometry.Visible = rbnFloppyDisk.Checked;
            lstFloppyGeometries.Visible = rbnFloppyDisk.Checked;
            lblFloppyLabel.Visible = rbnFloppyDisk.Checked;
            btnAdvanced.Visible = rbnFloppyDisk.Checked;
            cbxFloppyBPB.Visible = rbnFloppyDisk.Checked;
            txtFloppyLabel.Visible = rbnFloppyDisk.Checked;
            lstFloppyBPB.Visible = rbnFloppyDisk.Checked;
        }
    }
}