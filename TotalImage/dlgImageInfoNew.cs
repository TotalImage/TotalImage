using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TotalImage.Containers;
using TotalImage.Containers.VHD;
using TotalImage.Partitions;

namespace TotalImage
{
    public partial class dlgImageInfoNew : Form
    {
        private bool hashesDone = false;
        private frmMain mainForm;

        //TODO: Obtain actual data from the main form/relevant classes and display it
        public dlgImageInfoNew()
        {
            InitializeComponent();
        }

        private void dlgImageInfo_Load(object sender, System.EventArgs e)
        {
            mainForm = (frmMain)Application.OpenForms["frmMain"];
            FileInfo fileInfo = new(mainForm.filepath);

            if (mainForm.image is null)
                return;

            //Fixes the column width on high DPI screens
            lstPropertiesFile.Columns[1].Width = lstPropertiesFile.ClientRectangle.Width - lstPropertiesFile.Columns[0].Width;
            lstPropertiesContainer.Columns[1].Width = lstPropertiesContainer.ClientRectangle.Width - lstPropertiesContainer.Columns[0].Width;
            lstPropertiesPT.Columns[1].Width = lstPropertiesPT.ClientRectangle.Width - lstPropertiesPT.Columns[0].Width;
            lstPropertiesPartition.Columns[1].Width = lstPropertiesPartition.ClientRectangle.Width - lstPropertiesPartition.Columns[0].Width;
            lstPropertiesFS.Columns[1].Width = lstPropertiesFS.ClientRectangle.Width - lstPropertiesFS.Columns[0].Width;

            lstPropertiesFile.FindItemWithText("Filename").SubItems[1].Text = mainForm.filename;
            lstPropertiesFile.FindItemWithText("Size").SubItems[1].Text = Settings.CurrentSettings.SizeUnit.FormatSize((ulong)fileInfo.Length, Settings.CurrentSettings.SizeUnit != SizeUnit.Bytes);
            lstPropertiesFile.FindItemWithText("Created").SubItems[1].Text = fileInfo.CreationTime.ToString();
            lstPropertiesFile.FindItemWithText("Modified").SubItems[1].Text = fileInfo.LastWriteTime.ToString();
            lstPropertiesFile.FindItemWithText("Accessed").SubItems[1].Text = fileInfo.LastAccessTime.ToString();
            lstPropertiesFile.FindItemWithText("Attributes").SubItems[1].Text = fileInfo.Attributes.ToString();
            lstPropertiesContainer.FindItemWithText("Container type").SubItems[1].Text = mainForm.image.DisplayName;

            //VHD specifics
            if (mainForm.image is VhdContainer vhd)
            {
                string vhdType = vhd.Footer.Type.ToString();
                lstPropertiesContainer.FindItemWithText("Container subtype").SubItems[1].Text = vhdType[..vhdType.IndexOf("HardDisk")];

                string containerVersion = $"{vhd.Footer.FormatVersionMajor}.{vhd.Footer.FormatVersionMinor}";
                if (containerVersion != "0.0")
                    lstPropertiesContainer.FindItemWithText("Container version").SubItems[1].Text = containerVersion;

                //We might want to prettify this further so we show "Windows" instead of "win", for example.
                lstPropertiesContainer.FindItemWithText("Created by").SubItems[1].Text = string.IsNullOrWhiteSpace(vhd.Footer.CreatorApplication) ? "Unknown" : vhd.Footer.CreatorApplication;

                string creatorVersion = $"{vhd.Footer.CreatorVersionMajor}.{vhd.Footer.CreatorVersionMinor}";
                if (creatorVersion != "0.0")
                    lstPropertiesContainer.FindItemWithText("Creator version").SubItems[1].Text = creatorVersion;
            }

            lstPropertiesFS.FindItemWithText("File system").SubItems[1].Text = mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem.DisplayName;

            var volLabel = mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem.VolumeLabel;
            if (!string.IsNullOrWhiteSpace(volLabel))
                lstPropertiesFS.FindItemWithText("Volume label").SubItems[1].Text = volLabel;

            lstPropertiesPT.FindItemWithText("Partitioning scheme").SubItems[1].Text = mainForm.image.PartitionTable.DisplayName;
            lstPropertiesPT.FindItemWithText("No. of partitions").SubItems[1].Text = mainForm.image.PartitionTable.Partitions.Count.ToString();
            lstPropertiesPT.FindItemWithText("Selected partition").SubItems[1].Text = mainForm.CurrentPartitionIndex.ToString();

            //We might want to prettify this once the image-loading branchis merged in
            if (mainForm.image.PartitionTable is MbrPartitionTable mbr)
            {
                MbrPartitionTable.MbrPartitionEntry entry = (MbrPartitionTable.MbrPartitionEntry)mbr.Partitions[mainForm.CurrentPartitionIndex];

                string entryType = "Unknown";
                try
                {
                    entryType = (Attribute.GetCustomAttribute(entry.Type.GetType().GetField(entry.Type.ToString()), typeof(DisplayAttribute)) as DisplayAttribute).Name;
                }
                catch (Exception) { }

                lstPropertiesPartition.FindItemWithText("Partition ID/type").SubItems[1].Text = $"0x{entry.Type:X} {entryType}";
            }
            else if (mainForm.image.PartitionTable is GptPartitionTable gpt)
            {
                GptPartitionTable.GptPartitionEntry entry = (GptPartitionTable.GptPartitionEntry)gpt.Partitions[mainForm.CurrentPartitionIndex];
                lstPropertiesPartition.FindItemWithText("Partition ID/type").SubItems[1].Text = GptPartitionTable.GptPartitionTypes[entry.TypeId];
            }

            lstPropertiesFS.FindItemWithText("Files").SubItems[1].Text = mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem.RootDirectory.CountFiles(true).ToString();
            lstPropertiesFS.FindItemWithText("Subdirectories").SubItems[1].Text = mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem.RootDirectory.CountSubdirectories(true).ToString();
            lstPropertiesFS.FindItemWithText("Total storage capacity").SubItems[1].Text = Settings.CurrentSettings.SizeUnit.FormatSize((ulong)mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].Length, Settings.CurrentSettings.SizeUnit != SizeUnit.Bytes);
            lstPropertiesFS.FindItemWithText("Free space").SubItems[1].Text = Settings.CurrentSettings.SizeUnit.FormatSize((ulong)mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem.TotalFreeSpace, Settings.CurrentSettings.SizeUnit != SizeUnit.Bytes);

            if (mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem is FileSystems.FAT.FatFileSystem fs)
            {
                if (fs.BiosParameterBlock is FileSystems.BPB.ExtendedBiosParameterBlock ebpb)
                    lstPropertiesFS.FindItemWithText("Volume serial number").SubItems[1].Text = ebpb.VolumeSerialNumber == 0 ? "N/A" : $"{ebpb.VolumeSerialNumber:X}";
                else if (fs.BiosParameterBlock is FileSystems.BPB.Fat32BiosParameterBlock f32bpb)
                    lstPropertiesFS.FindItemWithText("Volume serial number").SubItems[1].Text = f32bpb.VolumeSerialNumber == 0 ? "N/A" : $"{f32bpb.VolumeSerialNumber:X}";
            }

            lstPropertiesFile.FindItemWithText("MD5 hash").SubItems[1].Text = "Please wait...";
            lstPropertiesFile.FindItemWithText("SHA-1 hash").SubItems[1].Text = "Please wait...";

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
                TaskDialogPage page = new()
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

            using SaveFileDialog sfd = new();
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
                //This is needed for the version and git hash
                var asm = Assembly.GetEntryAssembly();
                var attrs = asm.GetCustomAttributes<AssemblyMetadataAttribute>();
                var hash = attrs.FirstOrDefault(a => a.Key == "GitHash");

                //Note that aside from the "created on" datetime at the top, all datetime values are currently exported in the user's locale!
                //Same for the decimal and thousands separator in numbers.
                StreamWriter sw = File.CreateText(sfd.FileName);
                sw.WriteLine("************************************");
                sw.WriteLine("*            TotalImage            *");
                sw.WriteLine("*     Image information report     *");
                sw.WriteLine("************************************");
                sw.WriteLine($"-TotalImage version: {asm.GetName().Version} ({hash.Value})");
                sw.WriteLine($"-Created on: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                sw.WriteLine();
                sw.WriteLine("** File information **");
                sw.WriteLine($"-Filename: {lstPropertiesFile.FindItemWithText("Filename").SubItems[1].Text}");
                sw.WriteLine($"-Size: {lstPropertiesFile.FindItemWithText("Size").SubItems[1].Text}");
                sw.WriteLine($"-Created: {lstPropertiesFile.FindItemWithText("Created").SubItems[1].Text}");
                sw.WriteLine($"-Modified: {lstPropertiesFile.FindItemWithText("Modified").SubItems[1].Text}");
                sw.WriteLine($"-Accessed: {lstPropertiesFile.FindItemWithText("Accessed").SubItems[1].Text}");
                sw.WriteLine($"-Attributes: {lstPropertiesFile.FindItemWithText("Attributes").SubItems[1].Text}");

                //Skip the file hashes if they're not done yet
                if (hashesDone)
                {
                    sw.WriteLine($"-MD5 hash: {lstPropertiesFile.FindItemWithText("MD5 hash").SubItems[1].Text}");
                    sw.WriteLine($"-SHA-1 hash: {lstPropertiesFile.FindItemWithText("SHA-1 hash").SubItems[1].Text}");
                }

                sw.WriteLine();
                sw.WriteLine("** Container information **");
                sw.WriteLine($"-Container type: {lstPropertiesContainer.FindItemWithText("Container type").SubItems[1].Text}");
                sw.WriteLine($"-Container subtype: {lstPropertiesContainer.FindItemWithText("Container subtype").SubItems[1].Text}");
                sw.WriteLine($"-Container version: {lstPropertiesContainer.FindItemWithText("Container version").SubItems[1].Text}");
                sw.WriteLine($"-Created by: {lstPropertiesContainer.FindItemWithText("Created by").SubItems[1].Text}");
                sw.WriteLine($"-Creator version: {lstPropertiesContainer.FindItemWithText("Creator version").SubItems[1].Text}");
                sw.WriteLine();
                sw.WriteLine("** Partition table information **");
                sw.WriteLine($"-Partitioning scheme: {lstPropertiesPT.FindItemWithText("Partitioning scheme").SubItems[1].Text}");
                sw.WriteLine($"-No. of partitions: {lstPropertiesPT.FindItemWithText("No. of partitions").SubItems[1].Text}");
                sw.WriteLine($"-Selected partition: {lstPropertiesPT.FindItemWithText("Selected partition").SubItems[1].Text}");
                sw.WriteLine();
                sw.WriteLine("** Selected partition information **");
                sw.WriteLine($"-Partition ID/type: {lstPropertiesPartition.FindItemWithText("Partition ID/type").SubItems[1].Text}");
                sw.WriteLine();
                sw.WriteLine("** File system information **");
                sw.WriteLine($"-File system: {lstPropertiesFS.FindItemWithText("File system").SubItems[1].Text}");
                sw.WriteLine($"-Volume label: {lstPropertiesFS.FindItemWithText("Volume label").SubItems[1].Text}");
                sw.WriteLine($"-Volume serial number: {lstPropertiesFS.FindItemWithText("Volume serial number").SubItems[1].Text}");
                sw.WriteLine($"-Total storage capacity: {lstPropertiesFS.FindItemWithText("Total storage capacity").SubItems[1].Text}");
                sw.WriteLine($"-Free space: {lstPropertiesFS.FindItemWithText("Free space").SubItems[1].Text}");
                sw.WriteLine($"-Files: {lstPropertiesFS.FindItemWithText("Files").SubItems[1].Text}");
                sw.WriteLine($"-Subdirectories: {lstPropertiesFS.FindItemWithText("Subdirectories").SubItems[1].Text}");
                sw.WriteLine();

                if (mainForm.image is IContainerComment)
                {
                    sw.WriteLine("** Container comment **");
                    sw.WriteLine($"\"{txtComment.Text}\"");
                    sw.WriteLine();
                }

                sw.Write("*** End of file ***");
                sw.Flush();
                sw.Close();
            }
        }

        CancellationTokenSource cts = new();

        private async void dlgImageInfo_Shown(object sender, EventArgs e)
        {
            if (Application.OpenForms["frmMain"] is frmMain mainForm && mainForm.image is not null)
            {
                var md5 = Task.Run(async () => await mainForm.image.CalculateMd5HashAsync(cts.Token));
                var sha1 = Task.Run(async () => await mainForm.image.CalculateSha1HashAsync(cts.Token));

                try
                {
                    lstPropertiesFile.FindItemWithText("MD5 hash").SubItems[1].Text = await md5;
                    lstPropertiesFile.FindItemWithText("SHA-1 hash").SubItems[1].Text = await sha1;       
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
            //Need to modify this to work with multiple target listviews...

            /*if (lstProperties.SelectedItems.Count == 1)
                Clipboard.SetText(lstProperties.SelectedItems[0].SubItems[1].Text);*/
        }

        private void cmsCopy_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Need to modify this to work with multiple target listviews...

            /*if (lstProperties.SelectedItems.Count == 1)
            {
                if (lstProperties.SelectedItems[0].SubItems[1].Text == "N/A" || (
                    !hashesDone && (lstProperties.SelectedItems[0].Tag.ToString() == "md5" || lstProperties.SelectedItems[0].Tag.ToString() == "sha1")))
                    copyValueToolStripMenuItem.Enabled = false;
                else
                    copyValueToolStripMenuItem.Enabled = true;
            }
            else
            {
                e.Cancel = true;
            }*/
        }
    }
}
