using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using TotalImage.ViewModels.Dialogs;

namespace TotalImage.Avalonia.Dialogs;

/// <summary>
/// Shows the properties of the open image. Binding only, plus the clipboard: the hashes are
/// calculated while the window is open, and the value of a property can be copied.
/// </summary>
public partial class ImageInfoDialog : Window
{
    // For the XAML previewer.
    public ImageInfoDialog()
    {
        InitializeComponent();
    }

    public ImageInfoDialog(ImageInfoViewModel viewModel) : this()
    {
        DataContext = viewModel;
        this.BindClose(okButton);

        foreach (var grid in new[] { fileGrid, containerGrid, partitionTableGrid, partitionGrid, fileSystemGrid })
            AddCopyMenu(grid);

        Opened += (_, _) => viewModel.ComputeHashesCommand.Execute(null);
        Closing += (_, _) => viewModel.ComputeHashesCommand.Cancel();
    }

    // "Copy value" on the selected row, offered only when ImageInfoViewModel.CanCopyValue allows it.
    private void AddCopyMenu(DataGrid grid)
    {
        MenuItem copyValueItem = new() { Header = "Copy value" };
        ContextMenu menu = new() { Items = { copyValueItem } };

        menu.Opening += (_, e) =>
        {
            if (grid.SelectedItem is PropertyRow row)
                copyValueItem.IsEnabled = ImageInfoViewModel.CanCopyValue(row);
            else
                e.Cancel = true; // Nothing is selected
        };
        copyValueItem.Click += async (_, _) =>
        {
            if (grid.SelectedItem is PropertyRow row && ImageInfoViewModel.CanCopyValue(row) && Clipboard is { } clipboard)
                await clipboard.SetTextAsync(row.Value);
        };

        grid.ContextMenu = menu;

        // Right-clicking a row selects it first, so the menu copies the row that was clicked.
        grid.AddHandler(PointerPressedEvent, (_, e) =>
        {
            if (!e.GetCurrentPoint(grid).Properties.IsRightButtonPressed)
                return;

            for (var visual = e.Source as Visual; visual is not null && visual != grid; visual = visual.GetVisualParent())
            {
                if (visual is DataGridRow { DataContext: PropertyRow row })
                {
                    grid.SelectedItem = row;
                    return;
                }
            }
        }, RoutingStrategies.Tunnel, handledEventsToo: true);
    }
}
