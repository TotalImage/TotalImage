using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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
            lstProperties.Groups[0].CollapsedState = ListViewGroupCollapsedState.Expanded;
            lstProperties.Groups[1].CollapsedState = ListViewGroupCollapsedState.Expanded;
            lstProperties.Groups[2].CollapsedState = ListViewGroupCollapsedState.Expanded;
            lstProperties.Groups[3].CollapsedState = ListViewGroupCollapsedState.Expanded;

            //Fixes the column width on high DPI screens
            lstProperties.Columns[1].Width = lstProperties.ClientRectangle.Width - lstProperties.Columns[0].Width;

            frmMain mainForm = (frmMain)Application.OpenForms["frmMain"];

            lstProperties.FindItemWithText("File name").SubItems[1].Text = mainForm.filename;
            lstProperties.FindItemWithText("File size").SubItems[1].Text = Settings.CurrentSettings.SizeUnit.FormatSize((ulong)mainForm.image.Length, Settings.CurrentSettings.SizeUnit != SizeUnit.Bytes);
            lstProperties.FindItemWithText("Container type").SubItems[1].Text = mainForm.image.DisplayName;
            lstProperties.FindItemWithText("Container subtype").SubItems[1].Text = "N/A"; //Obtain this from the container metadata if it exists
            lstProperties.FindItemWithText("File system").SubItems[1].Text = mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem.DisplayName;
            var volLabel = mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem.VolumeLabel;
            System.Diagnostics.Debug.WriteLine(volLabel);
            _ = string.IsNullOrWhiteSpace(volLabel) ? lstProperties.FindItemWithText("Volume label").SubItems[1].Text = "N/A" : lstProperties.FindItemWithText("Volume label").SubItems[1].Text = volLabel;
            lstProperties.FindItemWithText("Partitioning scheme").SubItems[1].Text = mainForm.image.PartitionTable.DisplayName;
            lstProperties.FindItemWithText("No. of partitions").SubItems[1].Text = mainForm.image.PartitionTable.Partitions.Count.ToString();
            lstProperties.FindItemWithText("Selected partition").SubItems[1].Text = mainForm.CurrentPartitionIndex.ToString();
            lstProperties.FindItemWithText("Partition ID/type").SubItems[1].Text = "N/A"; //Obtain this from the partition entry, if it exists
            lstProperties.FindItemWithText("Files").SubItems[1].Text = mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem.RootDirectory.FileCount(true).ToString();
            lstProperties.FindItemWithText("Subdirectories").SubItems[1].Text = mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem.RootDirectory.SubdirectoryCount(true).ToString();
            lstProperties.FindItemWithText("Created by").SubItems[1].Text = "N/A"; //Obtain this from the container metadata if it exists
            lstProperties.FindItemWithText("Total storage capacity").SubItems[1].Text = Settings.CurrentSettings.SizeUnit.FormatSize((ulong)mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].Length, Settings.CurrentSettings.SizeUnit != SizeUnit.Bytes);
            lstProperties.FindItemWithText("Free space").SubItems[1].Text = Settings.CurrentSettings.SizeUnit.FormatSize((ulong)mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem.TotalFreeSpace, Settings.CurrentSettings.SizeUnit != SizeUnit.Bytes);
            lstProperties.FindItemWithText("Volume serial number").SubItems[1].Text = "N/A"; //Obtain this from the BPB if it exists
            lstProperties.FindItemWithText("MD5 hash").SubItems[1].Text = "Please wait...";
            lstProperties.FindItemWithText("SHA-1 hash").SubItems[1].Text = "Please wait...";

            //Obtain this from the container metadata if it exists and display it
            txtComment.Text = "This container type does not support comments.";
            txtComment.Enabled = false;
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            using SaveFileDialog sfd = new SaveFileDialog();
            sfd.AutoUpgradeEnabled = true;
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            sfd.OverwritePrompt = true;
            sfd.DefaultExt = "txt";
            sfd.Filter =
                "Plain text file (*.txt)|*.txt|" +
                "All files (*.*)|*.*";

            DialogResult result = sfd.ShowDialog();

            if (result == DialogResult.OK)
            {
                StreamWriter sw = File.CreateText(sfd.FileName);
                sw.WriteLine("*** TotalImage image information ***");
                sw.WriteLine($"Created on {DateTime.Now}");
                sw.WriteLine();
                sw.WriteLine("Container information");
                sw.WriteLine($"-File name: {lstProperties.FindItemWithText("File name").SubItems[1].Text}");
                sw.WriteLine($"-File size: {lstProperties.FindItemWithText("File size").SubItems[1].Text}");
                sw.WriteLine($"-Container type: {lstProperties.FindItemWithText("Container type").SubItems[1].Text}");
                sw.WriteLine($"-Container subtype: {lstProperties.FindItemWithText("Container subtype").SubItems[1].Text}");
                sw.WriteLine($"-Created by: {lstProperties.FindItemWithText("Created by").SubItems[1].Text}");
                sw.WriteLine();
                sw.WriteLine("Partition information");
                sw.WriteLine($"-Partitioning scheme: {lstProperties.FindItemWithText("Partitioning scheme").SubItems[1].Text}");
                sw.WriteLine($"-No. of partitions: {lstProperties.FindItemWithText("No. of partitions").SubItems[1].Text}");
                sw.WriteLine($"-Selected partition: {lstProperties.FindItemWithText("Selected partition").SubItems[1].Text}");
                sw.WriteLine($"-Partition ID/type: {lstProperties.FindItemWithText("Partition ID/type").SubItems[1].Text}");
                sw.WriteLine();
                sw.WriteLine("File system information");
                sw.WriteLine($"-File system: {lstProperties.FindItemWithText("File system").SubItems[1].Text}");
                sw.WriteLine($"-Volume label: {lstProperties.FindItemWithText("Volume label").SubItems[1].Text}");
                sw.WriteLine($"-Volume serial number: {lstProperties.FindItemWithText("Volume serial number").SubItems[1].Text}");
                sw.WriteLine($"-Total storage capacity: {lstProperties.FindItemWithText("Total storage capacity").SubItems[1].Text}");
                sw.WriteLine($"-Free space: {lstProperties.FindItemWithText("Free space").SubItems[1].Text}");
                sw.WriteLine($"-Files: {lstProperties.FindItemWithText("Files").SubItems[1].Text}");
                sw.WriteLine($"-Subdirectories: {lstProperties.FindItemWithText("Subdirectories").SubItems[1].Text}");

                //Commeting this out for now since hash calculation could use some improvements...
                /*sw.WriteLine();
                sw.WriteLine("Miscellaneous");
                sw.WriteLine($"-MD5 hash: {lstProperties.FindItemWithText("MD5 hash").SubItems[1].Text}");
                sw.WriteLine($"-SHA-1 hash: {lstProperties.FindItemWithText("SHA-1 hash").SubItems[1].Text}");*/

                sw.WriteLine();
                sw.Write("*** End of file ***");
                sw.Flush();
                sw.Close();
            }
        }

        CancellationTokenSource cts = new CancellationTokenSource();

        private async void dlgImageInfo_Shown(object sender, EventArgs e)
        {
            if (Application.OpenForms["frmMain"] is frmMain mainForm && mainForm.image is not null)
            {
                var md5 = Task.Run<string>(async () => await mainForm.image.CalculateMd5HashAsync(cts.Token));
                var sha1 = Task.Run<string>(async () => await mainForm.image.CalculateSha1HashAsync(cts.Token));

                try
                {
                    lstProperties.FindItemWithText("MD5 hash").SubItems[1].Text = await md5;
                    lstProperties.FindItemWithText("SHA-1 hash").SubItems[1].Text = await sha1;
                }
                catch(TaskCanceledException)
                {
                    // Hash calculation was canceled, carry on
                }
            }
        }

        private void dlgImageInfo_FormClosing(object sender, FormClosingEventArgs e)
            => cts.Cancel(); // Cancel the background work if it's still in progress

        private void copyValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(lstProperties.SelectedItems[0].SubItems[1].Text);
        }
    }
}
