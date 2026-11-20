using System.Windows.Forms;
using TotalImage.Controls;
using TotalImage.ViewModels.Dialogs;

namespace TotalImage.Dialogs
{
    public partial class ManagePartitionsDialog : Form
    {
        public ManagePartitionsDialog(ManagePartitionsViewModel viewModel)
        {
            InitializeComponent();

            foreach (var row in viewModel.Rows)
                partitionsListView.Items.Add(new ListViewItem([row.Number, row.VolumeLabel, row.Type, row.Start, row.End, row.Size, row.Active]));
            partitionsListView.BindSelectedIndex(viewModel, nameof(ManagePartitionsViewModel.SelectedIndex));

            activeButton.BindOneWay(nameof(Button.Enabled), viewModel, nameof(ManagePartitionsViewModel.HasSelection));
            deleteButton.BindOneWay(nameof(Button.Enabled), viewModel, nameof(ManagePartitionsViewModel.HasSelection));
            formatButton.BindOneWay(nameof(Button.Enabled), viewModel, nameof(ManagePartitionsViewModel.HasSelection));
            resizeButton.BindOneWay(nameof(Button.Enabled), viewModel, nameof(ManagePartitionsViewModel.HasSelection));
        }
    }
}
