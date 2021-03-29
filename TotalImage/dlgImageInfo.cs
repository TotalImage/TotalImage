using System.Windows.Forms;

namespace TotalImage
{
    public partial class dlgImageInfo : Form
    {
        //TODO: Obtain actual data from the main form/relevant classes and display it
        public dlgImageInfo()
        {
            InitializeComponent();
        }

        private void dlgImageInfo_Load(object sender, System.EventArgs e)
        {
            frmMain mainForm = (frmMain)Application.OpenForms["frmMain"];
            string sizeUnitName = System.Enum.GetName(typeof(Settings.SizeUnit), Settings.CurrentSettings.SizeUnits);

            lblFilename1.Text = mainForm.filename;
            lblFileSize1.Text = $"{mainForm.image.Length:n0} B";
            if (Settings.CurrentSettings.SizeUnits != Settings.SizeUnit.B)
            {
                float sizeInUnit = mainForm.image.Length / (float)Settings.CurrentSettings.SizeUnits;
                lblFileSize1.Text = lblFileSize1.Text.Insert(0, $"{sizeInUnit:n2} {sizeUnitName} (");
                lblFileSize1.Text = lblFileSize1.Text.Insert(lblFileSize1.Text.Length, ")");
            }

            lblContainerType1.Text = mainForm.image.DisplayName;
            lblFileSystem1.Text = mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem.Format;
        }
    }
}