using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
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
                if (lstPartitions.SelectedItems[0].SubItems[2].Text == "RAW")
                {
                    lstPartitions.SelectedItems.Clear();
                }
                else
                {
                    btnOK.Enabled = true;
                }
            }
            else
            {
                btnOK.Enabled = false;
            }
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            ReadOnly = cbxReadOnly.Checked;
            SelectedEntry = int.Parse(lstPartitions.SelectedItems[0].Text);
        }

        private void dlgSelectPartition_Load(object sender, System.EventArgs e)
        {
            lblPartitioningScheme1.Text = PartitionTable.DisplayName;
            lstPartitions.Items.Clear();

            for (int i = 0; i < PartitionTable.Partitions.Count; i++)
            {
                var entry = PartitionTable.Partitions[i];
                ListViewItem lvi = new ListViewItem(i.ToString());
                try
                {
                    lvi.SubItems.Add(entry.FileSystem.VolumeLabel);
                    lvi.SubItems.Add(entry.FileSystem.DisplayName);
                }
                catch (InvalidDataException)
                {
                    // this is probably an unsupported file system - we'll just mark it as RAW
                    lvi.SubItems.Add("");
                    lvi.SubItems.Add("RAW");
                    lvi.ForeColor = Color.Gray;
                }
                lvi.SubItems.Add($"{entry.Offset:n0}");
                lvi.SubItems.Add($"{entry.Offset + entry.Length:n0}");
                lvi.SubItems.Add(Settings.CurrentSettings.SizeUnit.FormatSize((ulong)entry.Length));

                if (entry is MbrPartitionTable.MbrPartitionEntry mbrEntry)
                {
                    lvi.SubItems.Add(mbrEntry.Active ? "Yes" : "No");
                }
                else
                {
                    lvi.SubItems.Add("Unknown");
                }
                lstPartitions.Items.Add(lvi);
            }
        }

        private void lstPartitions_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo hitTestInfo = lstPartitions.HitTest(e.X, e.Y);
            ListViewItem lvi = hitTestInfo.Item;

            if (lvi != null && lvi.SubItems[2].Text != "RAW")
            {
                ReadOnly = cbxReadOnly.Checked;
                SelectedEntry = int.Parse(lvi.Text);
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
