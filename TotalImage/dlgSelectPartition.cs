using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TotalImage.Containers;
using TotalImage.Containers.NHD;
using TotalImage.Containers.VHD;
using TotalImage.Partitions;

namespace TotalImage
{
    public partial class dlgSelectPartition : Form
    {
        public PartitionTable PartitionTable { get; set; }

        public bool ReadOnly { get; private set; }
        public int SelectedEntry { get; private set; }

        public dlgSelectPartition()
        {
            InitializeComponent();
        }

        private void lstPartitions_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (lstPartitions.SelectedItems.Count >= 1)
            {
                btnOK.Enabled = lstPartitions.SelectedItems[0].SubItems[1].Text != "RAW";

                if(PartitionTable is GptPartitionTable gpt)
                {
                    GptPartitionTable.GptPartitionEntry entry = (GptPartitionTable.GptPartitionEntry)gpt.Partitions[int.Parse(lstPartitions.SelectedItems[0].SubItems[0].Text)];
                    lblPartitionType1.Text = GptPartitionTable.GptPartitionTypes[entry.TypeId];
                    lblPartitionTotalSectors1.Text = $"{entry.Length / 512:N0}";
                    lblPartitionFirstLba1.Text = $"{entry.FirstLBA:N0}";
                    lblPartitionLastLba1.Text = $"{entry.LastLBA:N0}";
                    lblPartitionStartOffset1.Text = $"{Settings.CurrentSettings.SizeUnit.FormatSize((ulong)entry.Offset)}";
                    lblPartitionEndOffset1.Text = $"{Settings.CurrentSettings.SizeUnit.FormatSize((ulong)(entry.Offset + entry.Length))}";
                    lblPartitionGuid1.Text = entry.EntryId.ToString();
                    lblPartitionSerial1.Text = "N/A";
                }
                else if(PartitionTable is MbrPartitionTable mbr)
                {
                    MbrPartitionTable.MbrPartitionEntry entry = (MbrPartitionTable.MbrPartitionEntry)mbr.Partitions[int.Parse(lstPartitions.SelectedItems[0].SubItems[0].Text)];
                    
                    //TODO: This, too, might be a hack. See if there's any way to make it better.
                    string entryType = "Unknown";
                    try
                    {
                        entryType = (Attribute.GetCustomAttribute(entry.Type.GetType().GetField(entry.Type.ToString()), typeof(DisplayAttribute)) as DisplayAttribute).Name;
                    }
                    catch(Exception) { }

                    lblPartitionType1.Text = $"0x{entry.Type:X} {entryType}";
                    lblPartitionGuid1.Text = "N/A";
                    lblPartitionFirstLba1.Text = $"{entry.LbaStart:N0}";
                    lblPartitionLastLba1.Text = $"{entry.LbaStart + entry.LbaLength - 1:N0}";
                    lblPartitionStartOffset1.Text = $"{Settings.CurrentSettings.SizeUnit.FormatSize((ulong)entry.Offset)}";
                    lblPartitionEndOffset1.Text = $"{Settings.CurrentSettings.SizeUnit.FormatSize((ulong)(entry.Offset + entry.Length))}";
                    lblPartitionTotalSectors1.Text = $"{entry.LbaLength:N0}";
                    lblPartitionSerial1.Text = "N/A";

                    if (entry.FileSystem is FileSystems.FAT.FatFileSystem fs)
                    {
                        if (fs.BiosParameterBlock is FileSystems.BPB.ExtendedBiosParameterBlock ebpb)
                            lblPartitionSerial1.Text = $"{ebpb.VolumeSerialNumber:X}";
                        else if (fs.BiosParameterBlock is FileSystems.BPB.Fat32BiosParameterBlock f32bpb)
                            lblPartitionSerial1.Text = $"{f32bpb.VolumeSerialNumber:X}";
                    }
                }
            }
            else
            {
                btnOK.Enabled = false;
                lblPartitionLastLba1.Text = string.Empty;
                lblPartitionFirstLba1.Text = string.Empty;
                lblPartitionSerial1.Text = string.Empty;
                lblPartitionTotalSectors1.Text = string.Empty;
                lblPartitionType1.Text = string.Empty;
                lblPartitionEndOffset1.Text = string.Empty;
                lblPartitionGuid1.Text = string.Empty;
                lblPartitionStartOffset1.Text = string.Empty;
            }
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            ReadOnly = cbxReadOnly.Checked;
            SelectedEntry = int.Parse(lstPartitions.SelectedItems[0].Text);
        }

        private void dlgSelectPartition_Load(object sender, System.EventArgs e)
        {
            lblPartitionTable1.Text = PartitionTable.DisplayName;
            lblDiskType1.Text = "Basic"; //TODO: We need to actually determine this - LDM detection...

            lblPartitionLastLba1.Text = string.Empty;
            lblPartitionFirstLba1.Text = string.Empty;
            lblPartitionSerial1.Text = string.Empty;
            lblPartitionTotalSectors1.Text = string.Empty;
            lblPartitionType1.Text = string.Empty;
            lblPartitionEndOffset1.Text = string.Empty;
            lblPartitionGuid1.Text = string.Empty;
            lblPartitionStartOffset1.Text = string.Empty;

            if(PartitionTable is GptPartitionTable gpt)
            {
                //Let's just assume 512-byte sectors for now
                lblDiskTotalSize1.Text = $"{Settings.CurrentSettings.SizeUnit.FormatSize(gpt.Header.BackupLBA * 512 + 512)}";
                lblDiskTotalSectors1.Text = $"{gpt.Header.BackupLBA + 1:N0}";
                lblDiskUsableSize1.Text = $"{Settings.CurrentSettings.SizeUnit.FormatSize((gpt.Header.LastUsableLBA + 1 - gpt.Header.FirstUsableLBA) * 512)}";
                lblDiskGuid1.Text = gpt.Header.DiskGuid.ToString();
                lblDiskTimestamp1.Text = "N/A";
                lblDiskSerial1.Text = "N/A";
            }    
            else if(PartitionTable is MbrPartitionTable mbr)
            {
                lblDiskGuid1.Text = "N/A";
                lblDiskSerial1.Text = "N/A";
                if(mbr.SerialNumber > 0) //This is a very simplistic check...
                    lblDiskSerial1.Text = $"{mbr.SerialNumber:X}";

                //Timestamp can be completely bogus/absent in modern MBRs, so let's ignore it if it is
                lblDiskTimestamp1.Text = "N/A";
                if (mbr.TimestampMinutes < 60 && mbr.TimestampSeconds < 60 && mbr.TimestampHours < 24)
                    lblDiskTimestamp1.Text = $"{mbr.TimestampHours:D2}:{mbr.TimestampMinutes:D2}:{mbr.TimestampSeconds:D2}";

                Container image = ((frmMain)Application.OpenForms["frmMain"]).image;

                //For raw, there is no container metadata that could be used, so we just need to estimate based on file size and sector size (assumed 512 B)
                if (image is RawContainer)
                {
                    long sectorCount = image.Length / 512;
                    lblDiskTotalSectors1.Text = $"{sectorCount:N0}";
                    lblDiskTotalSize1.Text = $"{Settings.CurrentSettings.SizeUnit.FormatSize((ulong)image.Length)}";

                    /* Technically everything after the bootsector/MBR is free to be partitioned, so we're disregarding any gaps that some software may make
                     * between the MBR and the first partition. */
                    lblDiskUsableSize1.Text = $"{Settings.CurrentSettings.SizeUnit.FormatSize((ulong)image.Length - 512)}";
                }
                //For VHD we can use the CurrentSize property
                else if (image is VhdContainer vhd)
                {
                    lblDiskTotalSize1.Text = $"{Settings.CurrentSettings.SizeUnit.FormatSize(vhd.Footer.CurrentSize)}";
                    lblDiskUsableSize1.Text = $"{Settings.CurrentSettings.SizeUnit.FormatSize(vhd.Footer.CurrentSize - 512)}";
                    lblDiskTotalSectors1.Text = $"{vhd.Footer.CurrentSize / 512:N0}";
                }
                /* For NHD there's only the CHS values and sector size. Apparently some programs adjust image size to fit a CHS combo perfectly?
                 * Once the PC98 partition table is supported, that would have to be taken into account, since it can co-exist with MBR... */
                else if (image is NhdContainer nhd)
                {
                    ulong sectors = nhd.Header.Cylinders * nhd.Header.Heads * nhd.Header.Sectors;
                    lblDiskTotalSectors1.Text = $"{sectors:N0}";
                    lblDiskTotalSize1.Text = $"{Settings.CurrentSettings.SizeUnit.FormatSize(sectors * nhd.Header.SectorSize)}";
                    lblDiskUsableSize1.Text = $"{Settings.CurrentSettings.SizeUnit.FormatSize(sectors * nhd.Header.SectorSize - nhd.Header.SectorSize)}";
                }
            }

            lstPartitions.Items.Clear();

            for (int i = 0; i < PartitionTable.Partitions.Count; i++)
            {
                var entry = PartitionTable.Partitions[i];
                ListViewItem lvi = new ListViewItem(i.ToString());
                try
                {
                    lvi.SubItems.Add(entry.FileSystem.DisplayName);                   
                    lvi.SubItems.Add(entry.FileSystem.VolumeLabel);                   
                }
                catch (InvalidDataException)
                {
                    // this is probably an unsupported file system - we'll just mark it as RAW
                    lvi.SubItems.Add("RAW");
                    lvi.SubItems.Add("");                    
                    lvi.ForeColor = Color.Gray;
                }

                lvi.SubItems.Add(Settings.CurrentSettings.SizeUnit.FormatSize((ulong)entry.Length));

                if (entry is MbrPartitionTable.MbrPartitionEntry mbrEntry)
                {
                    lstPartitions.Columns[4].Text = "Active";
                    lvi.SubItems.Add(mbrEntry.Active ? "Yes" : "No");
                }
                else
                {
                    lstPartitions.Columns[4].Text = "Legacy boot";
                    lvi.SubItems.Add("Unknown"); //TODO: check the GPT attributes for this
                }
                lstPartitions.Items.Add(lvi);
            }
        }

        private void lstPartitions_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo hitTestInfo = lstPartitions.HitTest(e.X, e.Y);
            ListViewItem lvi = hitTestInfo.Item;

            if (lvi.SubItems[1].Text != "RAW")
            {
                ReadOnly = cbxReadOnly.Checked;
                SelectedEntry = int.Parse(lvi.Text);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                TaskDialog.ShowDialog(this, new TaskDialogPage()
                {
                    Text = $"This partition cannot be loaded because it or the file system contained within is not supported. Select a supported partition to load.",
                    Heading = "Unsupported partition type",
                    Caption = "Information",
                    Buttons =
                    {
                        TaskDialogButton.OK
                    },
                    Icon = TaskDialogIcon.Information,
                    DefaultButton = TaskDialogButton.OK,
                    SizeToContent = true
                });

                return;
            }
        }
    }
}
