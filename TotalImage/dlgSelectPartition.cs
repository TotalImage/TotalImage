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
            string sizeUnitName = System.Enum.GetName(typeof(Settings.SizeUnit), Settings.CurrentSettings.SizeUnits);

            lstPartitions.Items.Clear();

            for (int i = 0; i < PartitionTable.Partitions.Count; i++)
            {
                var entry = PartitionTable.Partitions[i];
                float sizeInUnit = entry.Length / (float)Settings.CurrentSettings.SizeUnits;
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
                lvi.SubItems.Add(Settings.CurrentSettings.SizeUnits == Settings.SizeUnit.B ? $"{sizeInUnit:n0} B" : $"{sizeInUnit:n2} {Enum.GetName(typeof(Settings.SizeUnit), Settings.CurrentSettings.SizeUnits)}");

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

            if (lvi != null)
            {
                ReadOnly = cbxReadOnly.Checked;
                SelectedEntry = int.Parse(lvi.Text);
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}