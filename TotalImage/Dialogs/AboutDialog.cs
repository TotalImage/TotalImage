using System.Windows.Forms;
using TotalImage.ViewModels.Dialogs;

namespace TotalImage.Dialogs
{
    public partial class AboutDialog : Form
    {
        private readonly AboutViewModel viewModel;

        public AboutDialog(AboutViewModel viewModel)
        {
            InitializeComponent();

            this.viewModel = viewModel;
            versionLabel.Text = viewModel.VersionText;
            projectLinkLabel.Text = AboutViewModel.ProjectUrl;
        }

        private void ProjectLinkLabel_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            viewModel.OpenProjectPageCommand.Execute(null);
            projectLinkLabel.LinkVisited = viewModel.ProjectPageVisited;
        }
    }
}
