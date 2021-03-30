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

            lstProperties.FindItemWithText("Filename").SubItems[1].Text = mainForm.filename;
            lstProperties.FindItemWithText("File size").SubItems[1].Text = $"{mainForm.image.Length:n0} B";

            if (Settings.CurrentSettings.SizeUnits != Settings.SizeUnit.B)
            {
                float sizeInUnit = mainForm.image.Length / (float)Settings.CurrentSettings.SizeUnits;
                lstProperties.FindItemWithText("File size").SubItems[1].Text = lstProperties.FindItemWithText("File size").SubItems[1].Text.Insert(0, $"{sizeInUnit:n2} {sizeUnitName} (");
                lstProperties.FindItemWithText("File size").SubItems[1].Text = lstProperties.FindItemWithText("File size").SubItems[1].Text.Insert(lstProperties.FindItemWithText("File size").SubItems[1].Text.Length, ")");
            }

            lstProperties.FindItemWithText("Container type").SubItems[1].Text = mainForm.image.DisplayName;
            lstProperties.FindItemWithText("File system").SubItems[1].Text = mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem.Format;
            lstProperties.FindItemWithText("Partitioning scheme").SubItems[1].Text = mainForm.image.PartitionTable.DisplayName;
            lstProperties.FindItemWithText("No. of partitions").SubItems[1].Text = mainForm.image.PartitionTable.Partitions.Count.ToString();
            lstProperties.FindItemWithText("Selected partition").SubItems[1].Text = mainForm.CurrentPartitionIndex.ToString();
        }
    }
}