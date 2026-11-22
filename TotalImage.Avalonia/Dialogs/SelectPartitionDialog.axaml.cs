using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using TotalImage.ViewModels.Dialogs;

namespace TotalImage.Avalonia.Dialogs;

/// <summary>
/// Picks the partition of an image to load. Binding only, plus glue that forwards a
/// double-click on a row to the view model.
/// </summary>
public partial class SelectPartitionDialog : Window
{
    private readonly SelectPartitionViewModel? viewModel;

    // For the XAML previewer.
    public SelectPartitionDialog()
    {
        InitializeComponent();
    }

    public SelectPartitionDialog(SelectPartitionViewModel viewModel) : this()
    {
        this.viewModel = viewModel;
        DataContext = viewModel;
        // The last column shows the table-specific flag. Columns don't get the DataContext, so no binding.
        partitionsGrid.Columns[^1].Header = viewModel.FlagColumnHeader;
        partitionsGrid.LoadingRow += (_, e) => e.Row.Classes.Set("dimmed", e.Row.DataContext is PartitionRow { IsDimmed: true });
        partitionsGrid.DoubleTapped += PartitionsGrid_DoubleTapped;
        this.BindAccept(okButton, viewModel.AcceptCommand);
        this.BindCancel(cancelButton);
    }

    private async void PartitionsGrid_DoubleTapped(object? sender, TappedEventArgs e)
    {
        if (viewModel is null || (e.Source as Control)?.FindAncestorOfType<DataGridRow>(includeSelf: true) is not { } row)
            return;

        await viewModel.OpenPartitionCommand.ExecuteAsync(row.Index);
        if (viewModel.Answer is not null)
            Close(true);
    }
}
