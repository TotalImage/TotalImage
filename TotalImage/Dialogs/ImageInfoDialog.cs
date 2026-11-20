using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TotalImage.ViewModels.Dialogs;

namespace TotalImage.Dialogs
{
    public partial class ImageInfoDialog : Form
    {
        private readonly ImageInfoViewModel viewModel;

        public ImageInfoDialog(ImageInfoViewModel viewModel)
        {
            InitializeComponent();

            this.viewModel = viewModel;
            Fill(fileListView, viewModel.FileRows);
            Fill(containerListView, viewModel.ContainerRows);
            Fill(partitionTableListView, viewModel.PartitionTableRows);
            Fill(partitionListView, viewModel.PartitionRows);
            Fill(fileSystemListView, viewModel.FileSystemRows);
            commentTextBox.Text = viewModel.Comment;
            exportButton.Command = viewModel.ExportCommand;

            ((INotifyCollectionChanged)viewModel.FileRows).CollectionChanged += FileRows_CollectionChanged;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Fixes the column width on high DPI screens
            foreach (var listView in new[] { fileListView, containerListView, partitionTableListView, partitionListView, fileSystemListView })
                listView.Columns[1].Width = listView.ClientRectangle.Width - listView.Columns[0].Width;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            viewModel.ComputeHashesCommand.Execute(null);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            viewModel.ComputeHashesCommand.Cancel();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            ((INotifyCollectionChanged)viewModel.FileRows).CollectionChanged -= FileRows_CollectionChanged;
            base.OnFormClosed(e);
        }

        private void FileRows_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Replace && e.NewItems?[0] is PropertyRow row)
                fileListView.Items[e.NewStartingIndex] = CreateItem(row);
            else
                Fill(fileListView, viewModel.FileRows);
        }

        private static void Fill(ListView listView, IEnumerable<PropertyRow> rows)
        {
            listView.BeginUpdate();
            listView.Items.Clear();
            foreach (var row in rows)
                listView.Items.Add(CreateItem(row));
            listView.EndUpdate();
        }

        // "N/A" values are greyed out.
        private static ListViewItem CreateItem(PropertyRow row)
        {
            ListViewItem item = new(row.Name) { Tag = row, UseItemStyleForSubItems = false };
            item.SubItems.Add(row.Value);
            item.SubItems[1].ForeColor = row.IsNotAvailable ? SystemColors.ControlDark : SystemColors.WindowText;
            return item;
        }

        private void CopyValueMenuItem_Click(object? sender, EventArgs e)
        {
            if (copyContextMenu.SourceControl is ListView { SelectedItems.Count: 1 } listView && listView.SelectedItems[0].Tag is PropertyRow row)
                Clipboard.SetText(row.Value);
        }

        private void CopyContextMenu_Opening(object? sender, CancelEventArgs e)
        {
            if (copyContextMenu.SourceControl is ListView { SelectedItems.Count: 1 } listView)
                copyValueMenuItem.Enabled = ImageInfoViewModel.CanCopyValue(listView.SelectedItems[0].Tag as PropertyRow);
            else
                e.Cancel = true; //Nothing or several items are selected
        }
    }
}
