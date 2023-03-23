using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TotalImage.Containers;
using TotalImage.Containers.NHD;
using TotalImage.Containers.VHD;
using TotalImage.Partitions;

namespace TotalImage
{
    public partial class dlgImageInfo : Form
    {
        private bool hashesDone = false;

        //TODO: Obtain actual data from the main form/relevant classes and display it
        public dlgImageInfo()
        {
            InitializeComponent();
        }

        private void dlgImageInfo_Load(object sender, System.EventArgs e)
        {
            frmMain mainForm = (frmMain)Application.OpenForms["frmMain"];
            FileInfo fileInfo = new FileInfo(mainForm.filepath);

            //Fixes the column width on high DPI screens
            lstProperties.Columns[1].Width = lstProperties.ClientRectangle.Width - lstProperties.Columns[0].Width;

            lstProperties.FindItemWithText("Filename").SubItems[1].Text = mainForm.filename;
            lstProperties.FindItemWithText("Size").SubItems[1].Text = Settings.CurrentSettings.SizeUnit.FormatSize(fileInfo.Length, Settings.CurrentSettings.SizeUnit != SizeUnit.Bytes);
            lstProperties.FindItemWithText("Created").SubItems[1].Text = fileInfo.CreationTime.ToString();
            lstProperties.FindItemWithText("Modified").SubItems[1].Text = fileInfo.LastWriteTime.ToString();
            lstProperties.FindItemWithText("Accessed").SubItems[1].Text = fileInfo.LastAccessTime.ToString();
            lstProperties.FindItemWithText("Attributes").SubItems[1].Text = fileInfo.Attributes.ToString();
            lstProperties.FindItemWithText("Container type").SubItems[1].Text = mainForm.image.DisplayName;

            //VHD specifics
            if (mainForm.image is VhdContainer vhd)
            {
                string vhdType = vhd.Footer.Type.ToString();
                lstProperties.FindItemWithText("Container subtype").SubItems[1].Text = vhdType.Substring(0, vhdType.IndexOf("HardDisk"));

                string containerVersion = $"{vhd.Footer.FormatVersionMajor}.{vhd.Footer.FormatVersionMinor}";
                if (containerVersion != "0.0")
                    lstProperties.FindItemWithText("Container version").SubItems[1].Text = containerVersion;

                //We might want to prettify this further so we show "Windows" instead of "win", for example.
                lstProperties.FindItemWithText("Created by").SubItems[1].Text = string.IsNullOrWhiteSpace(vhd.Footer.CreatorApplication) ? "Unknown" : vhd.Footer.CreatorApplication;

                string creatorVersion = $"{vhd.Footer.CreatorVersionMajor}.{vhd.Footer.CreatorVersionMinor}";
                if(creatorVersion != "0.0")
                    lstProperties.FindItemWithText("Creator version").SubItems[1].Text = creatorVersion;
            }

            lstProperties.FindItemWithText("File system").SubItems[1].Text = mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem.DisplayName;

            var volLabel = mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem.VolumeLabel;
            if(!string.IsNullOrWhiteSpace(volLabel))
                lstProperties.FindItemWithText("Volume label").SubItems[1].Text = volLabel;

            lstProperties.FindItemWithText("Partitioning scheme").SubItems[1].Text = mainForm.image.PartitionTable.DisplayName;
            lstProperties.FindItemWithText("No. of partitions").SubItems[1].Text = mainForm.image.PartitionTable.Partitions.Count.ToString();
            lstProperties.FindItemWithText("Selected partition").SubItems[1].Text = mainForm.CurrentPartitionIndex.ToString();

            //We might want to prettify this once the image-loading branchis merged in
            if(mainForm.image.PartitionTable is MbrPartitionTable mbr)
            {
                MbrPartitionTable.MbrPartitionEntry entry = (MbrPartitionTable.MbrPartitionEntry)mbr.Partitions[mainForm.CurrentPartitionIndex];
                lstProperties.FindItemWithText("Partition ID/type").SubItems[1].Text = $"0x{entry.Type:X}";
            }
            else if(mainForm.image.PartitionTable is GptPartitionTable gpt)
            {
                GptPartitionTable.GptPartitionEntry entry = (GptPartitionTable.GptPartitionEntry)gpt.Partitions[mainForm.CurrentPartitionIndex];
                lstProperties.FindItemWithText("Partition ID/type").SubItems[1].Text = entry.TypeId.ToString();
            }

            lstProperties.FindItemWithText("Files").SubItems[1].Text = mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem.RootDirectory.GetFileCount(true).ToString();
            lstProperties.FindItemWithText("Subdirectories").SubItems[1].Text = mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem.RootDirectory.GetSubdirectoryCount(true).ToString();
            lstProperties.FindItemWithText("Total storage capacity").SubItems[1].Text = Settings.CurrentSettings.SizeUnit.FormatSize(mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].Length, Settings.CurrentSettings.SizeUnit != SizeUnit.Bytes);
            lstProperties.FindItemWithText("Free space").SubItems[1].Text = Settings.CurrentSettings.SizeUnit.FormatSize(mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem.TotalFreeSpace, Settings.CurrentSettings.SizeUnit != SizeUnit.Bytes);

            if (mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem is FileSystems.FAT.FatFileSystem fs)
            {
                if (fs.BiosParameterBlock is FileSystems.BPB.ExtendedBiosParameterBlock ebpb)
                    lstProperties.FindItemWithText("Volume serial number").SubItems[1].Text = ebpb.VolumeSerialNumber == 0 ? "N/A" : $"{ebpb.VolumeSerialNumber:X}";
                else if (fs.BiosParameterBlock is FileSystems.BPB.Fat32BiosParameterBlock f32bpb)
                    lstProperties.FindItemWithText("Volume serial number").SubItems[1].Text = f32bpb.VolumeSerialNumber == 0 ? "N/A" : $"{f32bpb.VolumeSerialNumber:X}";
            }

            lstProperties.FindItemWithText("MD5 hash").SubItems[1].Text = "Please wait...";
            lstProperties.FindItemWithText("SHA-1 hash").SubItems[1].Text = "Please wait...";

            //TODO: This needs to be editeable (ReadOnly = false) once we have write support
            if (mainForm.image is IContainerComment containerComment)
            {
                txtComment.Text = containerComment.Comment;
            }
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            if (!hashesDone)
            {
                TaskDialogPage page = new TaskDialogPage()
                {
                    Text = $"File hashes are still being calculated and won't be included in the exported file{Environment.NewLine}unless you wait for the calculations to complete first.",
                    Heading = "Hash calculations in progress",
                    Caption = "Warning",
                    Buttons =
                    {
                        TaskDialogButton.Cancel,
                        TaskDialogButton.Continue
                    },
                    Icon = TaskDialogIcon.Warning,
                    DefaultButton = TaskDialogButton.Continue,
                    SizeToContent = true
                };

                TaskDialogButton warningResult = TaskDialog.ShowDialog(this, page);

                if (warningResult == TaskDialogButton.Cancel)
                {
                    return;
                }
            }

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
                sw.WriteLine("File information");
                sw.WriteLine($"-Filename: {lstProperties.FindItemWithText("Filename").SubItems[1].Text}");
                sw.WriteLine($"-Size: {lstProperties.FindItemWithText("Size").SubItems[1].Text}");
                sw.WriteLine($"-Created: {lstProperties.FindItemWithText("Created").SubItems[1].Text}");
                sw.WriteLine($"-Modified: {lstProperties.FindItemWithText("Modified").SubItems[1].Text}");
                sw.WriteLine($"-Accessed: {lstProperties.FindItemWithText("Accessed").SubItems[1].Text}");
                sw.WriteLine($"-Attributes: {lstProperties.FindItemWithText("Attributes").SubItems[1].Text}");

                //Skip the hashes if they're not done yet
                if (hashesDone)
                {
                    sw.WriteLine($"-MD5 hash: {lstProperties.FindItemWithText("MD5 hash").SubItems[1].Text}");
                    sw.WriteLine($"-SHA-1 hash: {lstProperties.FindItemWithText("SHA-1 hash").SubItems[1].Text}");
                }

                sw.WriteLine();
                sw.WriteLine("Container information");
                sw.WriteLine($"-Container type: {lstProperties.FindItemWithText("Container type").SubItems[1].Text}");
                sw.WriteLine($"-Container subtype: {lstProperties.FindItemWithText("Container subtype").SubItems[1].Text}");
                sw.WriteLine($"-Container version: {lstProperties.FindItemWithText("Container version").SubItems[1].Text}");
                sw.WriteLine($"-Created by: {lstProperties.FindItemWithText("Created by").SubItems[1].Text}");
                sw.WriteLine($"-Creator version: {lstProperties.FindItemWithText("Created by").SubItems[1].Text}");
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
                sw.WriteLine();

                if (!string.IsNullOrWhiteSpace(txtComment.Text))
                {
                    sw.WriteLine("Comment");
                    sw.WriteLine($"\"{txtComment.Text}\"");
                    sw.WriteLine();
                }

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

                catch (Exception ex) when (ex is TaskCanceledException || ex is OperationCanceledException)
                {
                    // Hash calculation was canceled, carry on
                }

                hashesDone = true;
            }
        }

        private void dlgImageInfo_FormClosing(object sender, FormClosingEventArgs e)
            => cts.Cancel(); // Cancel the background work if it's still in progress

        private void copyValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(lstProperties.SelectedItems.Count == 1)
                Clipboard.SetText(lstProperties.SelectedItems[0].SubItems[1].Text);
        }

        private void cmsCopy_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (lstProperties.SelectedItems.Count == 1)
            {
                if (lstProperties.SelectedItems[0].SubItems[1].Text == "N/A" || (
                    !hashesDone && (lstProperties.SelectedItems[0].Tag == "md5" || lstProperties.SelectedItems[0].Tag == "sha1")))
                    copyValueToolStripMenuItem.Enabled = false;
                else
                    copyValueToolStripMenuItem.Enabled = true;
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
