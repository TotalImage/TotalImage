using System.Drawing;
using System.Windows.Forms;
using TotalImage.Controls;
using TotalImage.ViewModels.Dialogs;

namespace TotalImage.Dialogs
{
    public partial class SelectPartitionDialog : Form
    {
        private readonly SelectPartitionViewModel viewModel;

        public SelectPartitionDialog(SelectPartitionViewModel viewModel)
        {
            this.viewModel = viewModel;
            InitializeComponent();

            partitionTableValueLabel.BindOneTime(nameof(Label.Text), viewModel, nameof(SelectPartitionViewModel.PartitionTableName));
            diskTypeValueLabel.BindOneTime(nameof(Label.Text), viewModel, nameof(SelectPartitionViewModel.DiskType));
            diskTotalSizeValueLabel.BindOneTime(nameof(Label.Text), viewModel, nameof(SelectPartitionViewModel.DiskTotalSize));
            diskTotalSectorsValueLabel.BindOneTime(nameof(Label.Text), viewModel, nameof(SelectPartitionViewModel.DiskTotalSectors));
            diskUsableSizeValueLabel.BindOneTime(nameof(Label.Text), viewModel, nameof(SelectPartitionViewModel.DiskUsableSize));
            diskGuidValueLabel.BindOneTime(nameof(Label.Text), viewModel, nameof(SelectPartitionViewModel.DiskGuid));
            diskTimestampValueLabel.BindOneTime(nameof(Label.Text), viewModel, nameof(SelectPartitionViewModel.DiskTimestamp));
            diskSerialValueLabel.BindOneTime(nameof(Label.Text), viewModel, nameof(SelectPartitionViewModel.DiskSerial));

            partitionTypeValueLabel.BindOneWay(nameof(Label.Text), viewModel, nameof(SelectPartitionViewModel.PartitionType));
            partitionGuidValueLabel.BindOneWay(nameof(Label.Text), viewModel, nameof(SelectPartitionViewModel.PartitionGuid));
            partitionSerialValueLabel.BindOneWay(nameof(Label.Text), viewModel, nameof(SelectPartitionViewModel.PartitionSerial));
            partitionFirstLbaValueLabel.BindOneWay(nameof(Label.Text), viewModel, nameof(SelectPartitionViewModel.PartitionFirstLba));
            partitionLastLbaValueLabel.BindOneWay(nameof(Label.Text), viewModel, nameof(SelectPartitionViewModel.PartitionLastLba));
            partitionStartOffsetValueLabel.BindOneWay(nameof(Label.Text), viewModel, nameof(SelectPartitionViewModel.PartitionStartOffset));
            partitionEndOffsetValueLabel.BindOneWay(nameof(Label.Text), viewModel, nameof(SelectPartitionViewModel.PartitionEndOffset));
            partitionTotalSectorsValueLabel.BindOneWay(nameof(Label.Text), viewModel, nameof(SelectPartitionViewModel.PartitionTotalSectors));

            flagColumnHeader.Text = viewModel.FlagColumnHeader;
            foreach (var row in viewModel.Rows)
            {
                ListViewItem item = new(row.Index.ToString());
                item.SubItems.Add(row.FileSystem);
                item.SubItems.Add(row.VolumeLabel);
                item.SubItems.Add(row.Size);
                if (row.Flag is not null)
                    item.SubItems.Add(row.Flag);
                if (row.IsDimmed)
                    item.ForeColor = Color.Gray;
                partitionsListView.Items.Add(item);
            }
            partitionsListView.BindSelectedIndex(viewModel, nameof(SelectPartitionViewModel.SelectedIndex));

            readOnlyCheckBox.BindTwoWay(nameof(CheckBox.Checked), viewModel, nameof(SelectPartitionViewModel.ReadOnly));
            readOnlyCheckBox.BindOneTime(nameof(CheckBox.Enabled), viewModel, nameof(SelectPartitionViewModel.CanChangeReadOnly));
            okButton.Command = viewModel.AcceptCommand;
        }

        private async void partitionsListView_MouseDoubleClick(object? sender, MouseEventArgs e)
        {
            if (partitionsListView.HitTest(e.X, e.Y).Item is not { } item)
                return;

            await viewModel.OpenPartitionCommand.ExecuteAsync(item.Index);
            if (viewModel.Answer is not null)
                DialogResult = DialogResult.OK;
        }
    }
}
