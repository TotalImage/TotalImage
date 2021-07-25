using System.Windows.Forms;
using System.ComponentModel;

namespace TotalImage
{
    public partial class dlgImageInfo : Form
    {
        private BackgroundWorker md5Worker;
        private BackgroundWorker sha1Worker;

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

            /* Do this in the background so we don't hang the UI thread. Because both methods use the same stream, they must absolutely NOT run at 
             * the same time!!! That's why this quasi mutex is here. The second worker will sleep until the first one is done and then do its own 
             * calculation. This seems to guarantee proper results. */
            bool md5Done = false;
            md5Worker = new BackgroundWorker();
            md5Worker.WorkerSupportsCancellation = true;
            md5Worker.WorkerReportsProgress = false;
            md5Worker.DoWork += (sender, e) =>
            {
                BackgroundWorker worker = (BackgroundWorker)sender;
                e.Result = mainForm.image.CalculateMd5Hash();
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                }
            };
            md5Worker.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Cancelled) { }
                else if (e.Error != null) { }
                else
                {
                    md5Done = true;
                    lstProperties.FindItemWithText("MD5 hash").SubItems[1].Text = (string)e.Result;
                }
            };
            md5Worker.RunWorkerAsync();

            sha1Worker = new BackgroundWorker();
            sha1Worker.WorkerSupportsCancellation = true;
            sha1Worker.WorkerReportsProgress = false;
            sha1Worker.DoWork += (sender, e) =>
            {
                BackgroundWorker worker = (BackgroundWorker)sender;
                while (!md5Done)
                {
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                    }
                    System.Threading.Thread.Sleep(100); //So we don't waste processor time
                }
                e.Result = mainForm.image.CalculateSha1Hash();
            };
            sha1Worker.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Cancelled) { }
                else if (e.Error != null) { }
                else
                {
                    lstProperties.FindItemWithText("SHA-1 hash").SubItems[1].Text = (string)e.Result;
                }               
            };
            sha1Worker.RunWorkerAsync();

            //Obtain this from the container metadata if it exists and display it
            txtComment.Text = "This container type does not support comments.";
            txtComment.Enabled = false;
            lblComment.Enabled = false;
        }

        private void dlgImageInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Cancel the background work if it's still in progress
            if (md5Worker.IsBusy)
            {
                md5Worker.CancelAsync();
            }

            if (sha1Worker.IsBusy)
            {
                sha1Worker.CancelAsync();
            }
        }
    }
}