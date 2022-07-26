using System.Collections.Generic;
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using TotalImage.Partitions;

namespace TotalImage.UI
{
    public partial class SelectPartitionDialog : Window
    {
        public SelectPartitionDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object? sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CancelButton_Click(object? sender, RoutedEventArgs e)
        {
            if (DataContext is SelectPartitionViewModel spvm)
            {
                spvm.SelectedPartition = null;
            }

            Close();
        }
    }

    public class SelectPartitionViewModel : INotifyPropertyChanged
    {
        private readonly PartitionTable _partitionTable;
        private PartitionEntry? _selectedPartition;

        public string SchemeLabel => _partitionTable.DisplayName;

        public IReadOnlyList<PartitionEntry> Entries => _partitionTable.Partitions;

        public PartitionEntry? SelectedPartition
        {
            get => _selectedPartition;
            set
            {
                _selectedPartition = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedPartition)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsPartitionSelected)));
            }
        }

        public bool IsPartitionSelected
        {
            get => _selectedPartition != null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public SelectPartitionViewModel(PartitionTable table)
        {
            _partitionTable = table;
            _selectedPartition = null;
        }
    }
}
