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
            //Make the groups collapsible for .NET 5.0 or later
#if NET5_0_OR_GREATER
            lstProperties.Groups[0].CollapsedState = ListViewGroupCollapsedState.Expanded;
            lstProperties.Groups[1].CollapsedState = ListViewGroupCollapsedState.Expanded;
            lstProperties.Groups[2].CollapsedState = ListViewGroupCollapsedState.Expanded;
            lstProperties.Groups[3].CollapsedState = ListViewGroupCollapsedState.Expanded;
#endif
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
            lstProperties.FindItemWithText("File system").SubItems[1].Text = mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem.DisplayName;
            lstProperties.FindItemWithText("Partitioning scheme").SubItems[1].Text = mainForm.image.PartitionTable.DisplayName;
            lstProperties.FindItemWithText("No. of partitions").SubItems[1].Text = mainForm.image.PartitionTable.Partitions.Count.ToString();
            lstProperties.FindItemWithText("Selected partition").SubItems[1].Text = mainForm.CurrentPartitionIndex.ToString();
            lstProperties.FindItemWithText("Created by").SubItems[1].Text = "N/A"; //Obtain this from the container metadata if it exists
            lstProperties.FindItemWithText("Total storage capacity").SubItems[1].Text = $"{ mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].Length:n0} B";

            if (Settings.CurrentSettings.SizeUnits != Settings.SizeUnit.B)
            {
                float sizeInUnit = mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].Length / (float)Settings.CurrentSettings.SizeUnits;
                lstProperties.FindItemWithText("Total storage capacity").SubItems[1].Text = lstProperties.FindItemWithText("Total storage capacity").SubItems[1].Text.Insert(0, $"{sizeInUnit:n2} {sizeUnitName} (");
                lstProperties.FindItemWithText("Total storage capacity").SubItems[1].Text = lstProperties.FindItemWithText("Total storage capacity").SubItems[1].Text.Insert(lstProperties.FindItemWithText("Total storage capacity").SubItems[1].Text.Length, ")");
            }
        }
    }
}