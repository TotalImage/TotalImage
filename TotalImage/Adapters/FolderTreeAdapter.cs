using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TotalImage.ViewModels.Browsing;
using TotalImage.ViewModels.Main;

namespace TotalImage.Adapters
{
    /// <summary>
    /// Shows <see cref="MainViewModel.RootFolder"/> in a tree view, creating the nodes of a
    /// folder's subfolders when it is first expanded, and keeps the selected node in sync with
    /// <see cref="MainViewModel.CurrentFolder"/> and the expanded nodes with
    /// <see cref="FolderViewModel.IsExpanded"/>.
    /// </summary>
    internal sealed class FolderTreeAdapter
    {
        private readonly TreeView tree;
        private readonly MainViewModel viewModel;
        private readonly FileIcons icons;
        private readonly Dictionary<FolderViewModel, TreeNode> nodes = [];

        // Set while the tree view is changed from the view model.
        private bool syncing;

        public FolderTreeAdapter(TreeView tree, MainViewModel viewModel, FileIcons icons)
        {
            this.tree = tree;
            this.viewModel = viewModel;
            this.icons = icons;

            tree.BeforeExpand += (_, e) =>
            {
                if (FolderOf(e.Node) is { } folder)
                {
                    CreateChildNodes(e.Node!);
                    folder.IsExpanded = true;
                }
            };
            tree.AfterCollapse += (_, e) =>
            {
                if (FolderOf(e.Node) is { } folder)
                    folder.IsExpanded = false;
            };
            tree.AfterSelect += (_, e) =>
            {
                if (!syncing && FolderOf(e.Node) is { } folder)
                    viewModel.CurrentFolder = folder;
            };

            viewModel.PropertyChanged += (_, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(MainViewModel.RootFolder):
                        ShowRoot();
                        break;
                    case nameof(MainViewModel.CurrentFolder):
                        ShowCurrentFolder();
                        break;
                }
            };

            ShowRoot();
            ShowCurrentFolder();
        }

        /// <summary>
        /// The folder of a node.
        /// </summary>
        public static FolderViewModel? FolderOf(TreeNode? node) => node?.Tag as FolderViewModel;

        private void ShowRoot()
        {
            foreach (var folder in nodes.Keys)
                folder.PropertyChanged -= OnFolderChanged;
            nodes.Clear();

            syncing = true;
            tree.BeginUpdate();
            try
            {
                tree.Nodes.Clear();
                if (viewModel.RootFolder is { } root)
                    tree.Nodes.Add(CreateNode(root));
            }
            finally
            {
                tree.EndUpdate();
                syncing = false;
            }
        }

        private void ShowCurrentFolder()
        {
            var node = viewModel.CurrentFolder is { } folder ? GetNode(folder) : null;
            if (tree.SelectedNode == node)
                return;

            syncing = true;
            try
            {
                // The tree view expands the node's parents to show it.
                tree.SelectedNode = node;
            }
            finally
            {
                syncing = false;
            }
        }

        // Gets the node of a folder, creating the nodes of its parents' subfolders as needed.
        private TreeNode? GetNode(FolderViewModel folder)
        {
            if (nodes.TryGetValue(folder, out var node))
                return node;

            if (folder.Parent is null || GetNode(folder.Parent) is not { } parentNode)
                return null;

            CreateChildNodes(parentNode);
            return nodes.GetValueOrDefault(folder);
        }

        private TreeNode CreateNode(FolderViewModel folder)
        {
            // The root folder always looks like a normal folder.
            var isHidden = folder.IsHidden && !folder.IsRoot;
            var imageKey = icons.GetKey(folder.IconKey, isHidden);
            var node = new TreeNode(folder.Name)
            {
                Tag = folder,
                ImageKey = imageKey,
                SelectedImageKey = imageKey
            };

            //Hidden folders have a 50% opacity for the icon
            if (isHidden)
                node.ForeColor = Color.Gray;

            if (FileListAdapter.GetPendingColors(folder.PendingState) is (var back, var fore))
            {
                node.BackColor = back;
                node.ForeColor = fore;
            }

            // A placeholder shows the expand button until the subfolders' nodes are created.
            if (folder.HasChildren)
                node.Nodes.Add(new TreeNode());

            folder.PropertyChanged += OnFolderChanged;
            nodes[folder] = node;
            return node;
        }

        private void CreateChildNodes(TreeNode node)
        {
            if (FolderOf(node) is not { } folder || node.Nodes.Count != 1 || node.Nodes[0].Tag is not null)
                return;

            tree.BeginUpdate();
            try
            {
                node.Nodes.Clear();
                foreach (var child in folder.Children)
                    node.Nodes.Add(CreateNode(child));
            }
            finally
            {
                tree.EndUpdate();
            }
        }

        private void OnFolderChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(FolderViewModel.IsExpanded) || sender is not FolderViewModel folder
                || !nodes.TryGetValue(folder, out var node))
                return;

            if (folder.IsExpanded && !node.IsExpanded)
                node.Expand();
            else if (!folder.IsExpanded && node.IsExpanded)
                node.Collapse(ignoreChildren: true);
        }
    }
}
