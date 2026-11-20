using System.Windows.Forms;
using TotalImage.Controls;
using TotalImage.ViewModels.Dialogs;

namespace TotalImage.Dialogs
{
    public partial class NewImageAdvancedDialog : Form
    {
        public NewImageAdvancedDialog(NewImageAdvancedViewModel viewModel)
        {
            InitializeComponent();

            sidesComboBox.BindItems(viewModel.SideCounts, viewModel, nameof(NewImageAdvancedViewModel.Sides));
            tracksNumericUpDown.BindTwoWay(nameof(NumericUpDown.Value), viewModel, nameof(NewImageAdvancedViewModel.Tracks));
            sectorsPerTrackNumericUpDown.BindTwoWay(nameof(NumericUpDown.Value), viewModel, nameof(NewImageAdvancedViewModel.SectorsPerTrack));
            bytesPerSectorNumericUpDown.BindTwoWay(nameof(NumericUpDown.Value), viewModel, nameof(NewImageAdvancedViewModel.BytesPerSector));
            mediaDescriptorNumericUpDown.BindTwoWay(nameof(NumericUpDown.Value), viewModel, nameof(NewImageAdvancedViewModel.MediaDescriptor));
            sectorsPerClusterNumericUpDown.BindTwoWay(nameof(NumericUpDown.Value), viewModel, nameof(NewImageAdvancedViewModel.SectorsPerCluster));
            fatCountNumericUpDown.BindTwoWay(nameof(NumericUpDown.Value), viewModel, nameof(NewImageAdvancedViewModel.NumberOfFats));
            sectorsPerFatNumericUpDown.BindTwoWay(nameof(NumericUpDown.Value), viewModel, nameof(NewImageAdvancedViewModel.SectorsPerFat));
            rootDirectoryEntriesNumericUpDown.BindTwoWay(nameof(NumericUpDown.Value), viewModel, nameof(NewImageAdvancedViewModel.RootDirectoryEntries));
            reservedSectorsNumericUpDown.BindTwoWay(nameof(NumericUpDown.Value), viewModel, nameof(NewImageAdvancedViewModel.ReservedSectors));
            totalSectorsNumericUpDown.BindOneWay(nameof(NumericUpDown.Value), viewModel, nameof(NewImageAdvancedViewModel.TotalSectors));

            oemIdTextBox.MaxLength = serialNumberTextBox.MaxLength = fileSystemTypeTextBox.MaxLength = NewImageAdvancedViewModel.MaxBootSectorFieldLength;
            oemIdTextBox.BindTwoWay(nameof(TextBox.Text), viewModel, nameof(NewImageAdvancedViewModel.OemId));
            oemIdTextBox.BindOneTime(nameof(TextBox.Enabled), viewModel, nameof(NewImageAdvancedViewModel.IsOemIdEnabled));
            serialNumberTextBox.BindTwoWay(nameof(TextBox.Text), viewModel, nameof(NewImageAdvancedViewModel.SerialNumber));
            serialNumberTextBox.BindOneTime(nameof(TextBox.Enabled), viewModel, nameof(NewImageAdvancedViewModel.IsSerialNumberEnabled));
            fileSystemTypeTextBox.BindTwoWay(nameof(TextBox.Text), viewModel, nameof(NewImageAdvancedViewModel.FileSystemType));
            fileSystemTypeTextBox.BindOneTime(nameof(TextBox.Enabled), viewModel, nameof(NewImageAdvancedViewModel.IsFileSystemTypeEnabled));
        }
    }
}
