using System;
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
                btnOK.Enabled = true;
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
            lstPartitions.Items.Clear();

            for (int i = 0; i < PartitionTable.Partitions.Count; i++)
            {
                var entry = PartitionTable.Partitions[i];
                ListViewItem lvi = new ListViewItem(i.ToString());
                lvi.SubItems.Add(entry.FileSystem.VolumeLabel);
                lvi.SubItems.Add(entry.FileSystem.Format);
                lvi.SubItems.Add(entry.Offset.ToString());
                lvi.SubItems.Add((entry.Offset + entry.Length).ToString());
                lvi.SubItems.Add($"{entry.Length / (int)Settings.CurrentSettings.SizeUnits} {Enum.GetName(typeof(Settings.SizeUnit), Settings.CurrentSettings.SizeUnits)}");
                lvi.SubItems.Add(((MbrPartitionTable.MbrPartitionEntry)entry).Active.ToString());
                lstPartitions.Items.Add(lvi);
            }
        }
    }
}