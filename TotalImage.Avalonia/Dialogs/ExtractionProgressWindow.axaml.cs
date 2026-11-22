using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using TotalImage.ViewModels.Extraction;

namespace TotalImage.Avalonia.Dialogs;

/// <summary>
/// Shows the progress of an extraction while it runs. It runs the extraction once it is open and
/// closes when the extraction has stopped; Cancel (or closing the window) cancels it, and the
/// window waits until the extraction has actually stopped.
/// </summary>
public partial class ExtractionProgressWindow : Window
{
    private readonly CancellationTokenSource cancellation = new();
    private readonly Func<IProgress<ExtractionProgress>, CancellationToken, Task>? extraction;
    private bool finished;

    // For the XAML previewer.
    public ExtractionProgressWindow()
    {
        InitializeComponent();
    }

    public ExtractionProgressWindow(Func<IProgress<ExtractionProgress>, CancellationToken, Task> extraction) : this()
    {
        this.extraction = extraction;
        cancelButton.Click += (_, _) => Cancel();
        Opened += async (_, _) => await RunAsync();
        Closing += (_, e) =>
        {
            // The image must not be used before the extraction has stopped.
            if (!finished)
            {
                e.Cancel = true;
                Cancel();
            }
        };
    }

    /// <summary>
    /// Raised once the window is open and the extraction starts, so questions it asks can be
    /// owned by this window.
    /// </summary>
    public event EventHandler? ExtractionStarting;

    /// <summary>
    /// How the extraction failed, or <c>null</c>.
    /// </summary>
    public Exception? Failure { get; private set; }

    private async Task RunAsync()
    {
        if (extraction is null)
            return;

        var progress = new Progress<ExtractionProgress>(report =>
        {
            progressBar.IsIndeterminate = false;
            progressBar.Maximum = Math.Max(report.Total, 1);
            progressBar.Value = report.Completed;
            if (report.FileName is not null)
                statusText.Text = $"Extracting {report.FileName}";
        });

        ExtractionStarting?.Invoke(this, EventArgs.Empty);
        try
        {
            await extraction(progress, cancellation.Token);
        }
        catch (Exception ex)
        {
            Failure = ex;
        }
        finally
        {
            finished = true;
            cancellation.Dispose();
            Close();
        }
    }

    private void Cancel()
    {
        if (finished)
            return;

        cancelButton.IsEnabled = false;
        statusText.Text = "Cancelling...";
        cancellation.Cancel();
    }
}
