using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TotalImage.Containers;
using TotalImage.Containers.Anex86;
using TotalImage.Containers.NHD;
using TotalImage.Containers.VHD;
using TotalImage.FileSystems.BPB;
using TotalImage.FileSystems.FAT;
using TotalImage.FileSystems.ISO;
using TotalImage.Partitions;

namespace TotalImage
{
    public partial class dlgImageInfo : Form
    {
        private bool hashesDone = false;
        private frmMain mainForm;

        public dlgImageInfo()
        {
            InitializeComponent();
        }

        private void dlgImageInfo_Load(object sender, System.EventArgs e)
        {
            mainForm = (frmMain)Application.OpenForms["frmMain"];
            FileInfo fileInfo = new(mainForm.filepath);

            if (mainForm.image is null)
                return;

            //File
            lstPropertiesFile.FindItemWithText("Filename").SubItems[1].Text = mainForm.filename;
            lstPropertiesFile.FindItemWithText("Path").SubItems[1].Text = mainForm.filepath;
            lstPropertiesFile.FindItemWithText("Size").SubItems[1].Text = Settings.CurrentSettings.SizeUnit.FormatSize((ulong)fileInfo.Length, Settings.CurrentSettings.SizeUnit != SizeUnit.Bytes);
            lstPropertiesFile.FindItemWithText("Created").SubItems[1].Text = fileInfo.CreationTime.ToString();
            lstPropertiesFile.FindItemWithText("Modified").SubItems[1].Text = fileInfo.LastWriteTime.ToString();
            lstPropertiesFile.FindItemWithText("Accessed").SubItems[1].Text = fileInfo.LastAccessTime.ToString();
            lstPropertiesFile.FindItemWithText("Attributes").SubItems[1].Text = fileInfo.Attributes.ToString();
            lstPropertiesFile.FindItemWithText("MD5 hash").SubItems[1].Text = "Please wait...";
            lstPropertiesFile.FindItemWithText("SHA-1 hash").SubItems[1].Text = "Please wait...";

            //Container
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
            //NHD specifics
            else if (mainForm.image is NhdContainer nhd)
            {
                lstPropertiesContainer.Items.Add(new ListViewItem("Header size")).SubItems.Add($"{SizeUnit.Bytes.FormatSize(nhd.Header.HeaderSize, false)}");
                lstPropertiesContainer.Items.Add(new ListViewItem("Cylinders")).SubItems.Add($"{nhd.Header.Cylinders}");
                lstPropertiesContainer.Items.Add(new ListViewItem("Heads")).SubItems.Add($"{nhd.Header.Heads}");
                lstPropertiesContainer.Items.Add(new ListViewItem("Sectors per track")).SubItems.Add($"{nhd.Header.SectorsPerTrack}");
                lstPropertiesContainer.Items.Add(new ListViewItem("Sector size")).SubItems.Add($"{SizeUnit.Bytes.FormatSize(nhd.Header.SectorSize, false)}");
            }
            else if (mainForm.image is Anex86Container anex86)
            {
                lstPropertiesContainer.Items.Add(new ListViewItem("Disk type")).SubItems.Add($"0x{anex86.Header.DiskType:X8}");
                lstPropertiesContainer.Items.Add(new ListViewItem("Header size")).SubItems.Add($"{SizeUnit.Bytes.FormatSize((ulong)anex86.Header.HeaderSize, false)}");
                lstPropertiesContainer.Items.Add(new ListViewItem("Data area size")).SubItems.Add($"{SizeUnit.Bytes.FormatSize((ulong)anex86.Header.DataSize, false)}");
                lstPropertiesContainer.Items.Add(new ListViewItem("Sector size")).SubItems.Add($"{SizeUnit.Bytes.FormatSize((ulong)anex86.Header.SectorSize, false)}");
                lstPropertiesContainer.Items.Add(new ListViewItem("Cylinders")).SubItems.Add($"{anex86.Header.Cylinders}");
                lstPropertiesContainer.Items.Add(new ListViewItem("Heads")).SubItems.Add($"{anex86.Header.Heads}");
                lstPropertiesContainer.Items.Add(new ListViewItem("Sectors per track")).SubItems.Add($"{anex86.Header.Sectors}");
            }

            //Partition table
            lstPropertiesPT.FindItemWithText("Partitioning scheme").SubItems[1].Text = mainForm.image.PartitionTable.DisplayName;
            lstPropertiesPT.FindItemWithText("Number of partitions").SubItems[1].Text = mainForm.image.PartitionTable.Partitions.Count.ToString();
            lstPropertiesPT.FindItemWithText("Selected partition").SubItems[1].Text = mainForm.CurrentPartitionIndex.ToString();

            //MBR specifics
            if (mainForm.image.PartitionTable is MbrPartitionTable mbr)
            {
                MbrPartitionTable.MbrPartitionEntry entry = (MbrPartitionTable.MbrPartitionEntry)mbr.Partitions[mainForm.CurrentPartitionIndex];

                //First we go over the MBR fields
                lstPropertiesPT.Items.Insert(1, new ListViewItem("Physical drive number")).SubItems.Add($"0x{mbr.DriveNumber:X2}");
                lstPropertiesPT.Items.Insert(2, new ListViewItem("Disk timestamp"));

                if (mbr.TimestampMinutes < 60 && mbr.TimestampSeconds < 60 && mbr.TimestampHours < 24)
                    lstPropertiesPT.FindItemWithText("Disk timestamp").SubItems.Add($"{mbr.TimestampHours:D2}:{mbr.TimestampMinutes:D2}:{mbr.TimestampSeconds:D2}");
                else
                    lstPropertiesPT.FindItemWithText("Disk timestamp").SubItems.Add("N/A");

                lstPropertiesPT.Items.Insert(3, new ListViewItem("Disk serial number"));

                if (mbr.SerialNumber > 0)
                    lstPropertiesPT.FindItemWithText("Disk serial number").SubItems.Add($"0x{mbr.SerialNumber:X8}");
                else
                    lstPropertiesPT.FindItemWithText("Disk serial number").SubItems.Add("N/A");

                //And then for the selected MBR partition entry as well
                string entryType = "Unknown";
                try
                {
                    entryType = (Attribute.GetCustomAttribute(entry.Type.GetType().GetField(entry.Type.ToString()), typeof(DisplayAttribute)) as DisplayAttribute).Name;
                }
                catch (Exception) { }
                lstPropertiesPartition.FindItemWithText("Partition ID/type").SubItems[1].Text = $"0x{(byte)entry.Type:X2} - {entryType}";
                lstPropertiesPartition.Items.Add("Active").SubItems.Add(entry.Active ? "Yes" : "No");
                lstPropertiesPartition.Items.Add("First sector CHS").SubItems.Add($"({entry.ChsStart.Cylinder},{entry.ChsStart.Head},{entry.ChsStart.Sector})");
                lstPropertiesPartition.Items.Add("Last sector CHS").SubItems.Add($"({entry.ChsEnd.Cylinder},{entry.ChsEnd.Head},{entry.ChsEnd.Sector})");
                lstPropertiesPartition.Items.Add("First sector LBA").SubItems.Add($"{entry.LbaStart}");
                lstPropertiesPartition.Items.Add("Total sectors").SubItems.Add($"{entry.LbaLength}");
                lstPropertiesPartition.Items.Add("Total size").SubItems.Add($"{Settings.CurrentSettings.SizeUnit.FormatSize(entry.LbaLength * 512, true)}");
            }
            //GPT specifics
            else if (mainForm.image.PartitionTable is GptPartitionTable gpt)
            {
                GptPartitionTable.GptPartitionEntry entry = (GptPartitionTable.GptPartitionEntry)gpt.Partitions[mainForm.CurrentPartitionIndex];

                //Likewise we first go over GPT fields
                lstPropertiesPT.Items.Insert(1, new ListViewItem("Major version")).SubItems.Add($"0x{gpt.Header.VersionMajor:X4}");
                lstPropertiesPT.Items.Insert(2, new ListViewItem("Minor version")).SubItems.Add($"0x{gpt.Header.VersionMinor:X4}");
                lstPropertiesPT.Items.Insert(3, new ListViewItem("Header size")).SubItems.Add($"{SizeUnit.Bytes.FormatSize(gpt.Header.HeaderSize, false)}");
                lstPropertiesPT.Items.Insert(4, new ListViewItem("Header CRC32 hash")).SubItems.Add($"0x{gpt.Header.HeaderHash:X8}");
                lstPropertiesPT.Items.Insert(5, new ListViewItem("Current header LBA")).SubItems.Add($"{gpt.Header.CurrentLBA}");
                lstPropertiesPT.Items.Insert(6, new ListViewItem("Backup header LBA")).SubItems.Add($"{gpt.Header.BackupLBA}");
                lstPropertiesPT.Items.Insert(7, new ListViewItem("First usable LBA")).SubItems.Add($"{gpt.Header.FirstUsableLBA}");
                lstPropertiesPT.Items.Insert(8, new ListViewItem("Last usable LBA")).SubItems.Add($"{gpt.Header.LastUsableLBA}");
                lstPropertiesPT.Items.Insert(9, new ListViewItem("Disk GUID")).SubItems.Add($"{gpt.Header.DiskGuid}");
                lstPropertiesPT.Items.Insert(10, new ListViewItem("First partition table LBA")).SubItems.Add($"{gpt.Header.TableLBA}");
                lstPropertiesPT.Items.Insert(11, new ListViewItem("Partition entry size")).SubItems.Add($"{SizeUnit.Bytes.FormatSize(gpt.Header.SizeOfPartitionEntry, false)}");
                lstPropertiesPT.Items.Insert(12, new ListViewItem("Partition table CRC32 hash")).SubItems.Add($"0x{gpt.Header.TableHash:X8}");

                //And then for the selected GPT partition entry as well
                lstPropertiesPartition.FindItemWithText("Partition ID/type").SubItems[1].Text = GptPartitionTable.GptPartitionTypes[entry.TypeId];
                lstPropertiesPartition.Items.Add("Partition type GUID").SubItems.Add($"{entry.TypeId}");
                lstPropertiesPartition.Items.Add("Partition GUID").SubItems.Add($"{entry.EntryId}");
                lstPropertiesPartition.Items.Add("First LBA").SubItems.Add($"{entry.FirstLBA}");
                lstPropertiesPartition.Items.Add("Last LBA").SubItems.Add($"{entry.LastLBA}");
                lstPropertiesPartition.Items.Add("Attribute flags").SubItems.Add($"0x{(ulong)entry.Flags:X16}");
                lstPropertiesPartition.Items.Add("Partition name").SubItems.Add($"{entry.Name}");
            }

            //File system
            lstPropertiesFS.FindItemWithText("File system").SubItems[1].Text = mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem.DisplayName;
            lstPropertiesFS.FindItemWithText("Files").SubItems[1].Text = mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem.RootDirectory.CountFiles(true).ToString();
            lstPropertiesFS.FindItemWithText("Subdirectories").SubItems[1].Text = mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem.RootDirectory.CountSubdirectories(true).ToString();
            lstPropertiesFS.FindItemWithText("Total storage capacity").SubItems[1].Text = Settings.CurrentSettings.SizeUnit.FormatSize((ulong)mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].Length, Settings.CurrentSettings.SizeUnit != SizeUnit.Bytes);
            lstPropertiesFS.FindItemWithText("Free storage capacity").SubItems[1].Text = Settings.CurrentSettings.SizeUnit.FormatSize((ulong)mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem.TotalFreeSpace, Settings.CurrentSettings.SizeUnit != SizeUnit.Bytes);

            var volLabel = mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem.VolumeLabel;
            if (!string.IsNullOrWhiteSpace(volLabel))
                lstPropertiesFS.FindItemWithText("Volume label").SubItems[1].Text = volLabel;

            //FAT specifics
            if (mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem is FatFileSystem fatFS)
            {
                //We're using insert here because Add only appends the item collection, whereas insert, well, inserts items at the specified index...
                BiosParameterBlock bpb = fatFS.BiosParameterBlock;

                lstPropertiesFS.Items.Insert(1, new ListViewItem("Jump instruction")).SubItems.Add($"0x{bpb.BootJump[0]:X2} 0x{bpb.BootJump[1]:X2} 0x{bpb.BootJump[2]:X2}");
                lstPropertiesFS.Items.Insert(2, new ListViewItem("OEM ID")).SubItems.Add(string.IsNullOrWhiteSpace(fatFS.BiosParameterBlock.OemId) ? "N/A" : fatFS.BiosParameterBlock.OemId);
                lstPropertiesFS.Items.Insert(3, new ListViewItem("Sector size")).SubItems.Add($"{SizeUnit.Bytes.FormatSize(bpb.BytesPerLogicalSector, false)}");
                lstPropertiesFS.Items.Insert(4, new ListViewItem("Sectors per cluster")).SubItems.Add($"{bpb.LogicalSectorsPerCluster}");
                lstPropertiesFS.Items.Insert(5, new ListViewItem("Reserved sectors")).SubItems.Add($"{bpb.ReservedLogicalSectors}");
                lstPropertiesFS.Items.Insert(6, new ListViewItem("Number of FATs")).SubItems.Add($"{bpb.NumberOfFATs}");
                lstPropertiesFS.Items.Insert(7, new ListViewItem("Root directory entries")).SubItems.Add(fatFS is Fat32FileSystem ? "N/A" : $"{bpb.RootDirectoryEntries}");
                lstPropertiesFS.Items.Insert(8, new ListViewItem("Total sectors")).SubItems.Add($"{bpb.TotalLogicalSectors}");
                lstPropertiesFS.Items.Insert(9, new ListViewItem("Media descriptor")).SubItems.Add($"0x{bpb.MediaDescriptor:X2}");
                lstPropertiesFS.Items.Insert(10, new ListViewItem("Sectors per FAT")).SubItems.Add($"{bpb.LogicalSectorsPerFAT}");
                lstPropertiesFS.Items.Insert(11, new ListViewItem("Sectors per track")).SubItems.Add($"{bpb.PhysicalSectorsPerTrack}");
                lstPropertiesFS.Items.Insert(12, new ListViewItem("Heads/sides")).SubItems.Add($"{bpb.NumberOfHeads}");
                lstPropertiesFS.Items.Insert(13, new ListViewItem("Hidden sectors")).SubItems.Add($"{bpb.HiddenSectors}");

                //These fields only exist in an extended BPB (either DOS 3.4/4.0 or FAT32).
                if (bpb is ExtendedBiosParameterBlock ebpb)
                {
                    lstPropertiesFS.FindItemWithText("Volume serial number").SubItems[1].Text = ebpb.VolumeSerialNumber == 0 ? "N/A" : $"0x{ebpb.VolumeSerialNumber:X8}";
                    lstPropertiesFS.Items.Insert(14, new ListViewItem("Physical drive number")).SubItems.Add($"0x{ebpb.PhysicalDriveNumber:X2}");
                    lstPropertiesFS.Items.Insert(15, new ListViewItem("Flags")).SubItems.Add($"0x{ebpb.Flags:X2}");
                    lstPropertiesFS.Items.Insert(16, new ListViewItem("Boot signature")).SubItems.Add($"0x{(byte)ebpb.ExtendedBootSignature:X2}");
                    lstPropertiesFS.Items.Insert(17, new ListViewItem("File system type string")).SubItems.Add($"{ebpb.FileSystemType}");
                }

                //Root directory is relocatable in FAT32. We add these fields last to keep proper order of items.
                if (bpb is Fat32BiosParameterBlock f32bpb)
                {
                    lstPropertiesFS.Items.Insert(8, new ListViewItem("Root directory cluster")).SubItems.Add($"{f32bpb.RootDirectoryCluster}");
                    lstPropertiesFS.Items.Insert(18, new ListViewItem("Extended flags")).SubItems.Add($"0x{f32bpb.ExtFlags:X2}");
                    lstPropertiesFS.Items.Insert(19, new ListViewItem("Version")).SubItems.Add($"0x{f32bpb.FileSystemVersion:X2}");
                    lstPropertiesFS.Items.Insert(20, new ListViewItem("FSInfo sector")).SubItems.Add($"{f32bpb.FsInfo}");
                    lstPropertiesFS.Items.Insert(21, new ListViewItem("Backup boot sector")).SubItems.Add($"{f32bpb.BackupBootSector}");
                }
            }
            //ISO9660 specifics
            else if (mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem is Iso9660FileSystem isoFS)
            {
                //First remove some items that don't apply to ISO9660 or have specific alternatives
                lstPropertiesFS.Items.Remove(lstPropertiesFS.FindItemWithText("Volume label"));
                lstPropertiesFS.Items.Remove(lstPropertiesFS.FindItemWithText("Free storage capacity"));
                lstPropertiesFS.Items.Remove(lstPropertiesFS.FindItemWithText("Volume serial number"));
                lstPropertiesFS.FindItemWithText("Total storage capacity").Text = "Volume size";

                //Then add the ISO9660-specific items and style them
                lstPropertiesFS.Items.Add(new ListViewItem("System identifier")).SubItems.Add(string.IsNullOrWhiteSpace(isoFS.PrimaryVolumeDescriptor.SystemIdentifier) ? "N/A" : isoFS.PrimaryVolumeDescriptor.SystemIdentifier);
                lstPropertiesFS.Items.Add(new ListViewItem("Volume identifier")).SubItems.Add(string.IsNullOrWhiteSpace(isoFS.PrimaryVolumeDescriptor.VolumeIdentifier) ? "N/A" : isoFS.PrimaryVolumeDescriptor.VolumeIdentifier);
                lstPropertiesFS.Items.Add(new ListViewItem("Volume set size")).SubItems.Add(string.IsNullOrWhiteSpace(isoFS.PrimaryVolumeDescriptor.VolumeSetSize.ToString()) ? "N/A" : isoFS.PrimaryVolumeDescriptor.VolumeSetSize.ToString());
                lstPropertiesFS.Items.Add(new ListViewItem("Volume sequence no.")).SubItems.Add(string.IsNullOrWhiteSpace(isoFS.PrimaryVolumeDescriptor.VolumeSequenceNumber.ToString()) ? "N/A" : isoFS.PrimaryVolumeDescriptor.VolumeSequenceNumber.ToString());
                lstPropertiesFS.Items.Add(new ListViewItem("Volume set identifier")).SubItems.Add(string.IsNullOrWhiteSpace(isoFS.PrimaryVolumeDescriptor.VolumeSetIdentifier) ? "N/A" : isoFS.PrimaryVolumeDescriptor.VolumeSetIdentifier);
                lstPropertiesFS.Items.Add(new ListViewItem("Publisher")).SubItems.Add(string.IsNullOrWhiteSpace(isoFS.PrimaryVolumeDescriptor.PublisherIdentifier) ? "N/A" : isoFS.PrimaryVolumeDescriptor.PublisherIdentifier);
                lstPropertiesFS.Items.Add(new ListViewItem("Data preparer")).SubItems.Add(string.IsNullOrWhiteSpace(isoFS.PrimaryVolumeDescriptor.DataPreparerIdentifier) ? "N/A" : isoFS.PrimaryVolumeDescriptor.DataPreparerIdentifier);
                lstPropertiesFS.Items.Add(new ListViewItem("Application identifier")).SubItems.Add(string.IsNullOrWhiteSpace(isoFS.PrimaryVolumeDescriptor.ApplicationIdentifier) ? "N/A" : isoFS.PrimaryVolumeDescriptor.ApplicationIdentifier);
                lstPropertiesFS.Items.Add(new ListViewItem("Copyright file")).SubItems.Add(string.IsNullOrWhiteSpace(isoFS.PrimaryVolumeDescriptor.CopyrightFileIdentifier) ? "N/A" : isoFS.PrimaryVolumeDescriptor.CopyrightFileIdentifier);
                lstPropertiesFS.Items.Add(new ListViewItem("Abstract file")).SubItems.Add(string.IsNullOrWhiteSpace(isoFS.PrimaryVolumeDescriptor.AbstractFileIdentifier) ? "N/A" : isoFS.PrimaryVolumeDescriptor.AbstractFileIdentifier);
                lstPropertiesFS.Items.Add(new ListViewItem("Bibliographic file")).SubItems.Add(string.IsNullOrWhiteSpace(isoFS.PrimaryVolumeDescriptor.BibliographicFileIdentifier) ? "N/A" : isoFS.PrimaryVolumeDescriptor.BibliographicFileIdentifier);
                lstPropertiesFS.Items.Add(new ListViewItem("Creation time")).SubItems.Add(string.IsNullOrWhiteSpace(isoFS.PrimaryVolumeDescriptor.VolumeCreationTime.ToString()) ? "N/A" : isoFS.PrimaryVolumeDescriptor.VolumeCreationTime.ToString());
                lstPropertiesFS.Items.Add(new ListViewItem("Modification time")).SubItems.Add(string.IsNullOrWhiteSpace(isoFS.PrimaryVolumeDescriptor.VolumeModificationTime.ToString()) ? "N/A" : isoFS.PrimaryVolumeDescriptor.VolumeModificationTime.ToString());
                lstPropertiesFS.Items.Add(new ListViewItem("Expiration time")).SubItems.Add(string.IsNullOrWhiteSpace(isoFS.PrimaryVolumeDescriptor.VolumeExpirationTime.ToString()) ? "N/A" : isoFS.PrimaryVolumeDescriptor.VolumeExpirationTime.ToString());
                lstPropertiesFS.Items.Add(new ListViewItem("Effective time")).SubItems.Add(string.IsNullOrWhiteSpace(isoFS.PrimaryVolumeDescriptor.VolumeEffectiveTime.ToString()) ? "N/A" : isoFS.PrimaryVolumeDescriptor.VolumeEffectiveTime.ToString());
            }

            //TODO: This needs to be editeable (ReadOnly = false) once we have write support
            if (mainForm.image is IContainerComment containerComment)
            {
                txtComment.Text = containerComment.Comment;
            }

            //Fixes the column width on high DPI screens
            lstPropertiesFile.Columns[1].Width = lstPropertiesFile.ClientRectangle.Width - lstPropertiesFile.Columns[0].Width;
            lstPropertiesContainer.Columns[1].Width = lstPropertiesContainer.ClientRectangle.Width - lstPropertiesContainer.Columns[0].Width;
            lstPropertiesPT.Columns[1].Width = lstPropertiesPT.ClientRectangle.Width - lstPropertiesPT.Columns[0].Width;
            lstPropertiesPartition.Columns[1].Width = lstPropertiesPartition.ClientRectangle.Width - lstPropertiesPartition.Columns[0].Width;
            lstPropertiesFS.Columns[1].Width = lstPropertiesFS.ClientRectangle.Width - lstPropertiesFS.Columns[0].Width;

            //Apply the appropriate styling to all items
            StyleListViewItems();
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
                //Same for the decimal and thousands separator in numbers. Might be worth revisiting this at some point...
                StreamWriter sw = File.CreateText(sfd.FileName);
                sw.WriteLine("************************************");
                sw.WriteLine("*            TotalImage            *");
                sw.WriteLine("*     Image information report     *");
                sw.WriteLine("************************************");
                sw.WriteLine($"-TotalImage version: {asm.GetName().Version} ({hash.Value})");
                sw.WriteLine($"-Created on: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                sw.WriteLine();

                //Iterate over all the displayed listview items for each listview
                sw.WriteLine("** File information **");
                foreach (ListViewItem lvi in lstPropertiesFile.Items)
                {
                    //Skip the file hashes if they're not done yet
                    if (!hashesDone && (lvi.Text == "MD5 hash" || lvi.Text == "SHA-1 hash"))
                        continue;

                    sw.WriteLine($"-{lvi.Text}: {lvi.SubItems[1].Text}");
                }
                sw.WriteLine();

                sw.WriteLine("** Container information **");
                foreach (ListViewItem lvi in lstPropertiesContainer.Items)
                {
                    sw.WriteLine($"-{lvi.Text}: {lvi.SubItems[1].Text}");
                }
                sw.WriteLine();

                sw.WriteLine("** Partition table information **");
                foreach (ListViewItem lvi in lstPropertiesPT.Items)
                {
                    sw.WriteLine($"-{lvi.Text}: {lvi.SubItems[1].Text}");
                }
                sw.WriteLine();

                sw.WriteLine("** Selected partition information **");
                foreach (ListViewItem lvi in lstPropertiesPartition.Items)
                {
                    sw.WriteLine($"-{lvi.Text}: {lvi.SubItems[1].Text}");
                }
                sw.WriteLine();

                sw.WriteLine("** File system information **");
                foreach (ListViewItem lvi in lstPropertiesFS.Items)
                {
                    sw.WriteLine($"-{lvi.Text}: {lvi.SubItems[1].Text}");
                }
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
            //Obtain the listView that triggered the context menu
            ListView lst = cmsCopy.SourceControl as ListView;

            if (lst is not null && lst.SelectedItems.Count == 1)
                Clipboard.SetText(lst.SelectedItems[0].SubItems[1].Text);
        }

        private void cmsCopy_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Obtain the listView that triggered the context menu
            ListView lst = cmsCopy.SourceControl as ListView;

            if (lst is not null && lst.SelectedItems.Count == 1)
            {
                if (lst.SelectedItems[0].SubItems[1].Text == "N/A" || string.IsNullOrWhiteSpace(lst.SelectedItems[0].SubItems[1].Text) || (lst == lstPropertiesFile &&
                    !hashesDone && (lst.SelectedItems[0].Tag.ToString() == "md5" || lst.SelectedItems[0].Tag.ToString() == "sha1")))
                    copyValueToolStripMenuItem.Enabled = false;
                else
                    copyValueToolStripMenuItem.Enabled = true;
            }
            else
            {
                e.Cancel = true; //Cancel the context menu opening event if multiple items are somehow selected or the source control is null
            }
        }

        //Iterates through all the items and subitems of every listview and sets appropriate styling for subitems with "N/A" text
        private void StyleListViewItems()
        {
            foreach (ListViewItem lvi in lstPropertiesFile.Items)
            {
                lvi.UseItemStyleForSubItems = false;
                foreach (ListViewItem.ListViewSubItem lvi2 in lvi.SubItems)
                {
                    if (lvi2.Text == "N/A")
                        lvi2.ForeColor = SystemColors.ControlDark;
                    else
                        lvi2.ForeColor = SystemColors.WindowText;
                }
            }

            foreach (ListViewItem lvi in lstPropertiesContainer.Items)
            {
                lvi.UseItemStyleForSubItems = false;
                foreach (ListViewItem.ListViewSubItem lvi2 in lvi.SubItems)
                {
                    if (lvi2.Text == "N/A")
                        lvi2.ForeColor = SystemColors.ControlDark;
                    else
                        lvi2.ForeColor = SystemColors.WindowText;
                }
            }

            foreach (ListViewItem lvi in lstPropertiesPT.Items)
            {
                lvi.UseItemStyleForSubItems = false;
                foreach (ListViewItem.ListViewSubItem lvi2 in lvi.SubItems)
                {
                    if (lvi2.Text == "N/A")
                        lvi2.ForeColor = SystemColors.ControlDark;
                    else
                        lvi2.ForeColor = SystemColors.WindowText;
                }
            }

            foreach (ListViewItem lvi in lstPropertiesPartition.Items)
            {
                lvi.UseItemStyleForSubItems = false;
                foreach (ListViewItem.ListViewSubItem lvi2 in lvi.SubItems)
                {
                    if (lvi2.Text == "N/A")
                        lvi2.ForeColor = SystemColors.ControlDark;
                    else
                        lvi2.ForeColor = SystemColors.WindowText;
                }
            }

            foreach (ListViewItem lvi in lstPropertiesFS.Items)
            {
                lvi.UseItemStyleForSubItems = false;
                foreach (ListViewItem.ListViewSubItem lvi2 in lvi.SubItems)
                {
                    if (lvi2.Text == "N/A")
                        lvi2.ForeColor = SystemColors.ControlDark;
                    else
                        lvi2.ForeColor = SystemColors.WindowText;
                }
            }
        }
    }
}
