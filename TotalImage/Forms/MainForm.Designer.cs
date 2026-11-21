using System;
using TotalImage.Controls;

namespace TotalImage.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (disposing)
            {
                _monoFont.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            cmsDirTree = new System.Windows.Forms.ContextMenuStrip(components);
            expandDirectoryTreeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            collapseDirectoryTreeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            injectToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            extractToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            newFolderToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            renameToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            propertiesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            menuBar = new System.Windows.Forms.MenuStrip();
            cmsToolbars = new System.Windows.Forms.ContextMenuStrip(components);
            commandBarToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            directoryTreeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            statusBarToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            reloadImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            closeImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            recentFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator23 = new System.Windows.Forms.ToolStripSeparator();
            injectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            extractToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            newFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            changeVolumeLabelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            changeGeometryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            defragmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            formatDiskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            bootSectorPropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
            selectPartitionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            managePartitionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            parentDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            expandDirectoryTreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            collapseDirectoryTreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            showHiddenObjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            largeIconsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            smallIconsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            listToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            detailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            sortByToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            nameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            typeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            sizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            modifiedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            attributesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolbarsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            menuBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            commandBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            directoryTreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            statusBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            deletedObjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            hexViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            imageInformationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            settingsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            onlineDocumentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            statusBar = new System.Windows.Forms.StatusStrip();
            partitionSizeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            freeSpaceLabel = new System.Windows.Forms.ToolStripStatusLabel();
            freeSpaceProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            selectionSizeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            locationLabel = new System.Windows.Forms.ToolStripStatusLabel();
            notificationsLabel = new System.Windows.Forms.ToolStripButton();
            commandBar = new System.Windows.Forms.ToolStrip();
            newToolStripButton = new System.Windows.Forms.ToolStripButton();
            openToolStripButton = new System.Windows.Forms.ToolStripButton();
            saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            saveAsToolStripButton = new System.Windows.Forms.ToolStripButton();
            reloadImageToolStripButton = new System.Windows.Forms.ToolStripButton();
            closeImageToolStripButton = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            injectToolStripButton = new System.Windows.Forms.ToolStripButton();
            extractToolStripButton = new System.Windows.Forms.ToolStripButton();
            newFolderToolStripButton = new System.Windows.Forms.ToolStripButton();
            deleteToolStripButton = new System.Windows.Forms.ToolStripButton();
            propertiesToolStripButton = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            partitionComboBox = new System.Windows.Forms.ToolStripComboBox();
            managePartitionsToolStripButton = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            labelToolStripMenuButton = new System.Windows.Forms.ToolStripButton();
            bootsectToolStripButton = new System.Windows.Forms.ToolStripButton();
            infoToolStripButton = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            parentDirectoryToolStripButton = new System.Windows.Forms.ToolStripButton();
            viewToolStripButton = new System.Windows.Forms.ToolStripDropDownButton();
            showHiddenObjectsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            largeIconsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            smallIconsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            listToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            detailsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            sortToolStripButton = new System.Windows.Forms.ToolStripDropDownButton();
            nameToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            typeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            sizeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            modifiedToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            attributesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            settingsToolStripButton = new System.Windows.Forms.ToolStripButton();
            splitContainer = new System.Windows.Forms.SplitContainer();
            lstDirectories = new TreeViewEx();
            imgFilesSmall = new System.Windows.Forms.ImageList(components);
            lstFiles = new ListViewEx();
            clmName = new System.Windows.Forms.ColumnHeader();
            clmType = new System.Windows.Forms.ColumnHeader();
            clmSize = new System.Windows.Forms.ColumnHeader();
            clmModified = new System.Windows.Forms.ColumnHeader();
            clmAttributes = new System.Windows.Forms.ColumnHeader();
            cmsFileList = new System.Windows.Forms.ContextMenuStrip(components);
            parentDirectoryToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            viewToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            showHiddenObjectsToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator19 = new System.Windows.Forms.ToolStripSeparator();
            largeIconsToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            smallIconsToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            listToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            detailsToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            sortByToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            nameToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            typeToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            sizeToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            modifiedToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            attributesToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            selectAllToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator22 = new System.Windows.Forms.ToolStripSeparator();
            injectToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            extractToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            newFolderToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            deleteToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            renameToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            propertiesToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            imgFilesLarge = new System.Windows.Forms.ImageList(components);
            openContainingFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            cmsDirTree.SuspendLayout();
            menuBar.SuspendLayout();
            cmsToolbars.SuspendLayout();
            statusBar.SuspendLayout();
            commandBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            cmsFileList.SuspendLayout();
            SuspendLayout();
            // 
            // cmsDirTree
            // 
            cmsDirTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { expandDirectoryTreeToolStripMenuItem1, collapseDirectoryTreeToolStripMenuItem1, toolStripSeparator15, injectToolStripMenuItem1, extractToolStripMenuItem1, newFolderToolStripMenuItem1, deleteToolStripMenuItem1, renameToolStripMenuItem1, propertiesToolStripMenuItem1 });
            cmsDirTree.Name = "cmsDirTree";
            cmsDirTree.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            cmsDirTree.Size = new System.Drawing.Size(217, 186);
            cmsDirTree.Opening += cmsDirTree_Opening;
            // 
            // expandDirectoryTreeToolStripMenuItem1
            // 
            expandDirectoryTreeToolStripMenuItem1.Image = Properties.Resources.arrow_up_down;
            expandDirectoryTreeToolStripMenuItem1.Name = "expandDirectoryTreeToolStripMenuItem1";
            expandDirectoryTreeToolStripMenuItem1.Size = new System.Drawing.Size(216, 22);
            expandDirectoryTreeToolStripMenuItem1.Text = "Expand directory tree";
            // 
            // collapseDirectoryTreeToolStripMenuItem1
            // 
            collapseDirectoryTreeToolStripMenuItem1.Image = Properties.Resources.arrow_down_up;
            collapseDirectoryTreeToolStripMenuItem1.Name = "collapseDirectoryTreeToolStripMenuItem1";
            collapseDirectoryTreeToolStripMenuItem1.Size = new System.Drawing.Size(216, 22);
            collapseDirectoryTreeToolStripMenuItem1.Text = "Collapse directory tree";
            // 
            // toolStripSeparator15
            // 
            toolStripSeparator15.Name = "toolStripSeparator15";
            toolStripSeparator15.Size = new System.Drawing.Size(213, 6);
            // 
            // injectToolStripMenuItem1
            // 
            injectToolStripMenuItem1.Image = Properties.Resources.page_white_add;
            injectToolStripMenuItem1.Name = "injectToolStripMenuItem1";
            injectToolStripMenuItem1.Size = new System.Drawing.Size(216, 22);
            injectToolStripMenuItem1.Text = "Inject files or folders...";
            // 
            // extractToolStripMenuItem1
            // 
            extractToolStripMenuItem1.Image = Properties.Resources.redo;
            extractToolStripMenuItem1.Name = "extractToolStripMenuItem1";
            extractToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X;
            extractToolStripMenuItem1.Size = new System.Drawing.Size(216, 22);
            extractToolStripMenuItem1.Text = "Extract...";
            // 
            // newFolderToolStripMenuItem1
            // 
            newFolderToolStripMenuItem1.Image = Properties.Resources.folder_new;
            newFolderToolStripMenuItem1.Name = "newFolderToolStripMenuItem1";
            newFolderToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.N;
            newFolderToolStripMenuItem1.Size = new System.Drawing.Size(216, 22);
            newFolderToolStripMenuItem1.Text = "New folder...";
            // 
            // deleteToolStripMenuItem1
            // 
            deleteToolStripMenuItem1.Image = Properties.Resources.bin_empty;
            deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            deleteToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            deleteToolStripMenuItem1.Size = new System.Drawing.Size(216, 22);
            deleteToolStripMenuItem1.Text = "Delete";
            // 
            // renameToolStripMenuItem1
            // 
            renameToolStripMenuItem1.Image = Properties.Resources.textfield_rename;
            renameToolStripMenuItem1.Name = "renameToolStripMenuItem1";
            renameToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F2;
            renameToolStripMenuItem1.Size = new System.Drawing.Size(216, 22);
            renameToolStripMenuItem1.Text = "Rename...";
            // 
            // propertiesToolStripMenuItem1
            // 
            propertiesToolStripMenuItem1.Image = Properties.Resources.page_white_edit;
            propertiesToolStripMenuItem1.Name = "propertiesToolStripMenuItem1";
            propertiesToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Enter;
            propertiesToolStripMenuItem1.Size = new System.Drawing.Size(216, 22);
            propertiesToolStripMenuItem1.Text = "Properties...";
            // 
            // menuBar
            // 
            menuBar.ContextMenuStrip = cmsToolbars;
            menuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem, viewToolStripMenuItem, toolsToolStripMenuItem, helpToolStripMenuItem });
            menuBar.Location = new System.Drawing.Point(0, 0);
            menuBar.Name = "menuBar";
            menuBar.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            menuBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            menuBar.Size = new System.Drawing.Size(984, 24);
            menuBar.TabIndex = 0;
            menuBar.Text = "menuStrip1";
            // 
            // cmsToolbars
            // 
            cmsToolbars.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { commandBarToolStripMenuItem1, directoryTreeToolStripMenuItem1, statusBarToolStripMenuItem1 });
            cmsToolbars.Name = "cmsToolbars";
            cmsToolbars.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            cmsToolbars.Size = new System.Drawing.Size(152, 70);
            // 
            // commandBarToolStripMenuItem1
            // 
            commandBarToolStripMenuItem1.Checked = true;
            commandBarToolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            commandBarToolStripMenuItem1.Name = "commandBarToolStripMenuItem1";
            commandBarToolStripMenuItem1.Size = new System.Drawing.Size(151, 22);
            commandBarToolStripMenuItem1.Text = "Command bar";
            // 
            // directoryTreeToolStripMenuItem1
            // 
            directoryTreeToolStripMenuItem1.Checked = true;
            directoryTreeToolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            directoryTreeToolStripMenuItem1.Name = "directoryTreeToolStripMenuItem1";
            directoryTreeToolStripMenuItem1.Size = new System.Drawing.Size(151, 22);
            directoryTreeToolStripMenuItem1.Text = "Directory tree";
            // 
            // statusBarToolStripMenuItem1
            // 
            statusBarToolStripMenuItem1.Checked = true;
            statusBarToolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            statusBarToolStripMenuItem1.Name = "statusBarToolStripMenuItem1";
            statusBarToolStripMenuItem1.Size = new System.Drawing.Size(151, 22);
            statusBarToolStripMenuItem1.Text = "Status bar";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { newToolStripMenuItem, openToolStripMenuItem, openContainingFolderToolStripMenuItem, saveToolStripMenuItem, saveAsToolStripMenuItem, reloadImageToolStripMenuItem, closeImageToolStripMenuItem, toolStripSeparator2, recentFilesToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F;
            fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Image = Properties.Resources.page_white;
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N;
            newToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            newToolStripMenuItem.Text = "New...";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Image = Properties.Resources.folder_go;
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O;
            openToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            openToolStripMenuItem.Text = "Open...";
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Image = Properties.Resources.diskette;
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            saveToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            saveToolStripMenuItem.Text = "Save";
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Image = Properties.Resources.disk_multiple;
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.S;
            saveAsToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            saveAsToolStripMenuItem.Text = "Save as...";
            // 
            // reloadImageToolStripMenuItem
            // 
            reloadImageToolStripMenuItem.Image = Properties.Resources.arrow_refresh;
            reloadImageToolStripMenuItem.Name = "reloadImageToolStripMenuItem";
            reloadImageToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            reloadImageToolStripMenuItem.Text = "Reload image";
            // 
            // closeImageToolStripMenuItem
            // 
            closeImageToolStripMenuItem.Image = Properties.Resources.cross;
            closeImageToolStripMenuItem.Name = "closeImageToolStripMenuItem";
            closeImageToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W;
            closeImageToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            closeImageToolStripMenuItem.Text = "Close image";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(203, 6);
            // 
            // recentFilesToolStripMenuItem
            // 
            recentFilesToolStripMenuItem.Image = Properties.Resources.clock_history_frame;
            recentFilesToolStripMenuItem.Name = "recentFilesToolStripMenuItem";
            recentFilesToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            recentFilesToolStripMenuItem.Text = "Recent files";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(203, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Image = Properties.Resources.door_out;
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4;
            exitToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            exitToolStripMenuItem.Text = "Exit";
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { selectAllToolStripMenuItem, toolStripSeparator23, injectToolStripMenuItem, extractToolStripMenuItem, newFolderToolStripMenuItem, deleteToolStripMenuItem, renameToolStripMenuItem, propertiesToolStripMenuItem, toolStripSeparator3, changeVolumeLabelToolStripMenuItem, changeGeometryToolStripMenuItem, defragmentToolStripMenuItem, formatDiskToolStripMenuItem, bootSectorPropertiesToolStripMenuItem, toolStripSeparator17, selectPartitionToolStripMenuItem, managePartitionsToolStripMenuItem });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.E;
            editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            editToolStripMenuItem.Text = "Edit";
            // 
            // selectAllToolStripMenuItem
            // 
            selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            selectAllToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A;
            selectAllToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            selectAllToolStripMenuItem.Text = "Select all";
            // 
            // toolStripSeparator23
            // 
            toolStripSeparator23.Name = "toolStripSeparator23";
            toolStripSeparator23.Size = new System.Drawing.Size(222, 6);
            // 
            // injectToolStripMenuItem
            // 
            injectToolStripMenuItem.Image = Properties.Resources.page_white_add;
            injectToolStripMenuItem.Name = "injectToolStripMenuItem";
            injectToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Insert;
            injectToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            injectToolStripMenuItem.Text = "Inject files or folders...";
            // 
            // extractToolStripMenuItem
            // 
            extractToolStripMenuItem.Image = Properties.Resources.redo;
            extractToolStripMenuItem.Name = "extractToolStripMenuItem";
            extractToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X;
            extractToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            extractToolStripMenuItem.Text = "Extract...";
            // 
            // newFolderToolStripMenuItem
            // 
            newFolderToolStripMenuItem.Image = Properties.Resources.folder_new;
            newFolderToolStripMenuItem.Name = "newFolderToolStripMenuItem";
            newFolderToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.N;
            newFolderToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            newFolderToolStripMenuItem.Text = "New folder...";
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Image = Properties.Resources.bin_empty;
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            deleteToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            deleteToolStripMenuItem.Text = "Delete";
            // 
            // renameToolStripMenuItem
            // 
            renameToolStripMenuItem.Image = Properties.Resources.textfield_rename;
            renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            renameToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            renameToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            renameToolStripMenuItem.Text = "Rename...";
            // 
            // propertiesToolStripMenuItem
            // 
            propertiesToolStripMenuItem.Image = Properties.Resources.page_white_edit;
            propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            propertiesToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Enter;
            propertiesToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            propertiesToolStripMenuItem.Text = "Properties...";
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(222, 6);
            // 
            // changeVolumeLabelToolStripMenuItem
            // 
            changeVolumeLabelToolStripMenuItem.Image = Properties.Resources.drive_rename;
            changeVolumeLabelToolStripMenuItem.Name = "changeVolumeLabelToolStripMenuItem";
            changeVolumeLabelToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            changeVolumeLabelToolStripMenuItem.Text = "Change volume label...";
            // 
            // changeGeometryToolStripMenuItem
            // 
            changeGeometryToolStripMenuItem.Name = "changeGeometryToolStripMenuItem";
            changeGeometryToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            changeGeometryToolStripMenuItem.Text = "Change floppy geometry...";
            // 
            // defragmentToolStripMenuItem
            // 
            defragmentToolStripMenuItem.Image = Properties.Resources.module;
            defragmentToolStripMenuItem.Name = "defragmentToolStripMenuItem";
            defragmentToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            defragmentToolStripMenuItem.Text = "Defragment...";
            // 
            // formatDiskToolStripMenuItem
            // 
            formatDiskToolStripMenuItem.Name = "formatDiskToolStripMenuItem";
            formatDiskToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            formatDiskToolStripMenuItem.Text = "Format disk/partition...";
            // 
            // bootSectorPropertiesToolStripMenuItem
            // 
            bootSectorPropertiesToolStripMenuItem.Image = Properties.Resources.application_xp_terminal;
            bootSectorPropertiesToolStripMenuItem.Name = "bootSectorPropertiesToolStripMenuItem";
            bootSectorPropertiesToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            bootSectorPropertiesToolStripMenuItem.Text = "Boot sector properties...";
            // 
            // toolStripSeparator17
            // 
            toolStripSeparator17.Name = "toolStripSeparator17";
            toolStripSeparator17.Size = new System.Drawing.Size(222, 6);
            // 
            // selectPartitionToolStripMenuItem
            // 
            selectPartitionToolStripMenuItem.Image = Properties.Resources.drive_go;
            selectPartitionToolStripMenuItem.Name = "selectPartitionToolStripMenuItem";
            selectPartitionToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            selectPartitionToolStripMenuItem.Text = "Select partition...";
            // 
            // managePartitionsToolStripMenuItem
            // 
            managePartitionsToolStripMenuItem.Image = Properties.Resources.drive_edit;
            managePartitionsToolStripMenuItem.Name = "managePartitionsToolStripMenuItem";
            managePartitionsToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            managePartitionsToolStripMenuItem.Text = "Manage partitions...";
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { parentDirectoryToolStripMenuItem, toolStripSeparator4, expandDirectoryTreeToolStripMenuItem, collapseDirectoryTreeToolStripMenuItem, toolStripSeparator16, showHiddenObjectsToolStripMenuItem, toolStripSeparator13, largeIconsToolStripMenuItem, smallIconsToolStripMenuItem, listToolStripMenuItem, detailsToolStripMenuItem, toolStripSeparator6, sortByToolStripMenuItem, toolbarsToolStripMenuItem });
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.V;
            viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            viewToolStripMenuItem.Text = "View";
            // 
            // parentDirectoryToolStripMenuItem
            // 
            parentDirectoryToolStripMenuItem.Image = Properties.Resources.folder_up;
            parentDirectoryToolStripMenuItem.Name = "parentDirectoryToolStripMenuItem";
            parentDirectoryToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            parentDirectoryToolStripMenuItem.Text = "Parent directory";
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(189, 6);
            // 
            // expandDirectoryTreeToolStripMenuItem
            // 
            expandDirectoryTreeToolStripMenuItem.Image = Properties.Resources.arrow_up_down;
            expandDirectoryTreeToolStripMenuItem.Name = "expandDirectoryTreeToolStripMenuItem";
            expandDirectoryTreeToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            expandDirectoryTreeToolStripMenuItem.Text = "Expand directory tree";
            // 
            // collapseDirectoryTreeToolStripMenuItem
            // 
            collapseDirectoryTreeToolStripMenuItem.Image = Properties.Resources.arrow_down_up;
            collapseDirectoryTreeToolStripMenuItem.Name = "collapseDirectoryTreeToolStripMenuItem";
            collapseDirectoryTreeToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            collapseDirectoryTreeToolStripMenuItem.Text = "Collapse directory tree";
            // 
            // toolStripSeparator16
            // 
            toolStripSeparator16.Name = "toolStripSeparator16";
            toolStripSeparator16.Size = new System.Drawing.Size(189, 6);
            // 
            // showHiddenObjectsToolStripMenuItem
            // 
            showHiddenObjectsToolStripMenuItem.Name = "showHiddenObjectsToolStripMenuItem";
            showHiddenObjectsToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            showHiddenObjectsToolStripMenuItem.Text = "Show hidden objects";
            // 
            // toolStripSeparator13
            // 
            toolStripSeparator13.Name = "toolStripSeparator13";
            toolStripSeparator13.Size = new System.Drawing.Size(189, 6);
            // 
            // largeIconsToolStripMenuItem
            // 
            largeIconsToolStripMenuItem.Name = "largeIconsToolStripMenuItem";
            largeIconsToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            largeIconsToolStripMenuItem.Text = "Large icons";
            // 
            // smallIconsToolStripMenuItem
            // 
            smallIconsToolStripMenuItem.Name = "smallIconsToolStripMenuItem";
            smallIconsToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            smallIconsToolStripMenuItem.Text = "Small icons";
            // 
            // listToolStripMenuItem
            // 
            listToolStripMenuItem.Name = "listToolStripMenuItem";
            listToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            listToolStripMenuItem.Text = "List";
            // 
            // detailsToolStripMenuItem
            // 
            detailsToolStripMenuItem.Checked = true;
            detailsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            detailsToolStripMenuItem.Name = "detailsToolStripMenuItem";
            detailsToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            detailsToolStripMenuItem.Text = "Details";
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new System.Drawing.Size(189, 6);
            // 
            // sortByToolStripMenuItem
            // 
            sortByToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { nameToolStripMenuItem, typeToolStripMenuItem, sizeToolStripMenuItem, modifiedToolStripMenuItem, attributesToolStripMenuItem });
            sortByToolStripMenuItem.Image = Properties.Resources.sort_descending;
            sortByToolStripMenuItem.Name = "sortByToolStripMenuItem";
            sortByToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            sortByToolStripMenuItem.Text = "Sort by";
            // 
            // nameToolStripMenuItem
            // 
            nameToolStripMenuItem.Name = "nameToolStripMenuItem";
            nameToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            nameToolStripMenuItem.Text = "Name";
            // 
            // typeToolStripMenuItem
            // 
            typeToolStripMenuItem.Name = "typeToolStripMenuItem";
            typeToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            typeToolStripMenuItem.Text = "Type";
            // 
            // sizeToolStripMenuItem
            // 
            sizeToolStripMenuItem.Name = "sizeToolStripMenuItem";
            sizeToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            sizeToolStripMenuItem.Text = "Size";
            // 
            // modifiedToolStripMenuItem
            // 
            modifiedToolStripMenuItem.Name = "modifiedToolStripMenuItem";
            modifiedToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            modifiedToolStripMenuItem.Text = "Modified";
            // 
            // attributesToolStripMenuItem
            // 
            attributesToolStripMenuItem.Name = "attributesToolStripMenuItem";
            attributesToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            attributesToolStripMenuItem.Text = "Attributes";
            // 
            // toolbarsToolStripMenuItem
            // 
            toolbarsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { menuBarToolStripMenuItem, commandBarToolStripMenuItem, directoryTreeToolStripMenuItem, statusBarToolStripMenuItem });
            toolbarsToolStripMenuItem.Image = Properties.Resources.layout;
            toolbarsToolStripMenuItem.Name = "toolbarsToolStripMenuItem";
            toolbarsToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            toolbarsToolStripMenuItem.Text = "Toolbars";
            // 
            // menuBarToolStripMenuItem
            // 
            menuBarToolStripMenuItem.Checked = true;
            menuBarToolStripMenuItem.CheckOnClick = true;
            menuBarToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            menuBarToolStripMenuItem.Enabled = false;
            menuBarToolStripMenuItem.Name = "menuBarToolStripMenuItem";
            menuBarToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            menuBarToolStripMenuItem.Text = "Menu bar";
            // 
            // commandBarToolStripMenuItem
            // 
            commandBarToolStripMenuItem.Checked = true;
            commandBarToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            commandBarToolStripMenuItem.Name = "commandBarToolStripMenuItem";
            commandBarToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            commandBarToolStripMenuItem.Text = "Command bar";
            // 
            // directoryTreeToolStripMenuItem
            // 
            directoryTreeToolStripMenuItem.Checked = true;
            directoryTreeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            directoryTreeToolStripMenuItem.Name = "directoryTreeToolStripMenuItem";
            directoryTreeToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            directoryTreeToolStripMenuItem.Text = "Directory tree";
            // 
            // statusBarToolStripMenuItem
            // 
            statusBarToolStripMenuItem.Checked = true;
            statusBarToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            statusBarToolStripMenuItem.Name = "statusBarToolStripMenuItem";
            statusBarToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            statusBarToolStripMenuItem.Text = "Status bar";
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { deletedObjectsToolStripMenuItem, hexViewToolStripMenuItem, imageInformationToolStripMenuItem, toolStripSeparator5, settingsToolStripMenuItem1 });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // deletedObjectsToolStripMenuItem
            // 
            deletedObjectsToolStripMenuItem.Name = "deletedObjectsToolStripMenuItem";
            deletedObjectsToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            deletedObjectsToolStripMenuItem.Text = "Deleted objects...";
            // 
            // hexViewToolStripMenuItem
            // 
            hexViewToolStripMenuItem.Image = Properties.Resources.digit_separator;
            hexViewToolStripMenuItem.Name = "hexViewToolStripMenuItem";
            hexViewToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            hexViewToolStripMenuItem.Text = "Hex view...";
            // 
            // imageInformationToolStripMenuItem
            // 
            imageInformationToolStripMenuItem.Image = Properties.Resources.document_info;
            imageInformationToolStripMenuItem.Name = "imageInformationToolStripMenuItem";
            imageInformationToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            imageInformationToolStripMenuItem.Text = "Image information...";
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new System.Drawing.Size(179, 6);
            // 
            // settingsToolStripMenuItem1
            // 
            settingsToolStripMenuItem1.Image = Properties.Resources.cog;
            settingsToolStripMenuItem1.Name = "settingsToolStripMenuItem1";
            settingsToolStripMenuItem1.Size = new System.Drawing.Size(182, 22);
            settingsToolStripMenuItem1.Text = "Settings...";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { onlineDocumentationToolStripMenuItem, aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.H;
            helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            helpToolStripMenuItem.Text = "Help";
            // 
            // onlineDocumentationToolStripMenuItem
            // 
            onlineDocumentationToolStripMenuItem.Image = Properties.Resources.help;
            onlineDocumentationToolStripMenuItem.Name = "onlineDocumentationToolStripMenuItem";
            onlineDocumentationToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            onlineDocumentationToolStripMenuItem.Text = "Online documentation";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Image = Properties.Resources.information;
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            aboutToolStripMenuItem.Text = "About...";
            // 
            // statusBar
            // 
            statusBar.ContextMenuStrip = cmsToolbars;
            statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { partitionSizeLabel, freeSpaceLabel, freeSpaceProgressBar, selectionSizeLabel, locationLabel, notificationsLabel });
            statusBar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            statusBar.Location = new System.Drawing.Point(0, 635);
            statusBar.Name = "statusBar";
            statusBar.Size = new System.Drawing.Size(984, 26);
            statusBar.SizingGrip = false;
            statusBar.TabIndex = 1;
            statusBar.Text = "statusStrip1";
            // 
            // partitionSizeLabel
            // 
            partitionSizeLabel.BackColor = System.Drawing.Color.Transparent;
            partitionSizeLabel.Name = "partitionSizeLabel";
            partitionSizeLabel.Size = new System.Drawing.Size(0, 21);
            // 
            // freeSpaceLabel
            // 
            freeSpaceLabel.BackColor = System.Drawing.Color.Transparent;
            freeSpaceLabel.Name = "freeSpaceLabel";
            freeSpaceLabel.Size = new System.Drawing.Size(0, 21);
            // 
            // freeSpaceProgressBar
            // 
            freeSpaceProgressBar.Margin = new System.Windows.Forms.Padding(0, 3, 4, 2);
            freeSpaceProgressBar.Name = "freeSpaceProgressBar";
            freeSpaceProgressBar.Size = new System.Drawing.Size(100, 21);
            // 
            // selectionSizeLabel
            // 
            selectionSizeLabel.BackColor = System.Drawing.Color.Transparent;
            selectionSizeLabel.Name = "selectionSizeLabel";
            selectionSizeLabel.Size = new System.Drawing.Size(0, 21);
            // 
            // locationLabel
            // 
            locationLabel.BackColor = System.Drawing.Color.Transparent;
            locationLabel.Name = "locationLabel";
            locationLabel.Size = new System.Drawing.Size(0, 21);
            // 
            // notificationsLabel
            // 
            notificationsLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            notificationsLabel.Image = Properties.Resources.error;
            notificationsLabel.ImageTransparentColor = System.Drawing.Color.Magenta;
            notificationsLabel.Name = "notificationsLabel";
            notificationsLabel.Size = new System.Drawing.Size(95, 24);
            notificationsLabel.Text = "Notifications";
            notificationsLabel.Visible = false;
            // 
            // commandBar
            // 
            commandBar.ContextMenuStrip = cmsToolbars;
            commandBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            commandBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { newToolStripButton, openToolStripButton, saveToolStripButton, saveAsToolStripButton, reloadImageToolStripButton, closeImageToolStripButton, toolStripSeparator8, injectToolStripButton, extractToolStripButton, newFolderToolStripButton, deleteToolStripButton, propertiesToolStripButton, toolStripSeparator9, partitionComboBox, managePartitionsToolStripButton, toolStripSeparator11, labelToolStripMenuButton, bootsectToolStripButton, infoToolStripButton, toolStripSeparator10, parentDirectoryToolStripButton, viewToolStripButton, sortToolStripButton, toolStripSeparator12, settingsToolStripButton });
            commandBar.Location = new System.Drawing.Point(0, 24);
            commandBar.Name = "commandBar";
            commandBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            commandBar.Size = new System.Drawing.Size(984, 25);
            commandBar.TabIndex = 2;
            commandBar.Text = "toolStrip1";
            // 
            // newToolStripButton
            // 
            newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            newToolStripButton.Image = Properties.Resources.page_white;
            newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            newToolStripButton.Name = "newToolStripButton";
            newToolStripButton.Size = new System.Drawing.Size(23, 22);
            newToolStripButton.Text = "&New";
            newToolStripButton.ToolTipText = "New image...";
            // 
            // openToolStripButton
            // 
            openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            openToolStripButton.Image = Properties.Resources.folder_go;
            openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            openToolStripButton.Name = "openToolStripButton";
            openToolStripButton.Size = new System.Drawing.Size(23, 22);
            openToolStripButton.Text = "&Open";
            openToolStripButton.ToolTipText = "Open image...";
            // 
            // saveToolStripButton
            // 
            saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            saveToolStripButton.Image = Properties.Resources.diskette;
            saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            saveToolStripButton.Name = "saveToolStripButton";
            saveToolStripButton.Size = new System.Drawing.Size(23, 22);
            saveToolStripButton.Text = "&Save";
            // 
            // saveAsToolStripButton
            // 
            saveAsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            saveAsToolStripButton.Image = Properties.Resources.disk_multiple;
            saveAsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            saveAsToolStripButton.Name = "saveAsToolStripButton";
            saveAsToolStripButton.Size = new System.Drawing.Size(23, 22);
            saveAsToolStripButton.Text = "Save as";
            // 
            // reloadImageToolStripButton
            // 
            reloadImageToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            reloadImageToolStripButton.Image = Properties.Resources.arrow_refresh;
            reloadImageToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            reloadImageToolStripButton.Name = "reloadImageToolStripButton";
            reloadImageToolStripButton.Size = new System.Drawing.Size(23, 22);
            reloadImageToolStripButton.Text = "Reload image";
            // 
            // closeImageToolStripButton
            // 
            closeImageToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            closeImageToolStripButton.Image = Properties.Resources.cross;
            closeImageToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            closeImageToolStripButton.Name = "closeImageToolStripButton";
            closeImageToolStripButton.Size = new System.Drawing.Size(23, 22);
            closeImageToolStripButton.Text = "Close image";
            // 
            // toolStripSeparator8
            // 
            toolStripSeparator8.Name = "toolStripSeparator8";
            toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // injectToolStripButton
            // 
            injectToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            injectToolStripButton.Image = Properties.Resources.page_white_add;
            injectToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            injectToolStripButton.Name = "injectToolStripButton";
            injectToolStripButton.Size = new System.Drawing.Size(23, 22);
            injectToolStripButton.Text = "Inject files";
            injectToolStripButton.ToolTipText = "Inject files...";
            // 
            // extractToolStripButton
            // 
            extractToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            extractToolStripButton.Image = Properties.Resources.redo;
            extractToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            extractToolStripButton.Name = "extractToolStripButton";
            extractToolStripButton.Size = new System.Drawing.Size(23, 22);
            extractToolStripButton.Text = "Extract";
            extractToolStripButton.ToolTipText = "Extract...";
            // 
            // newFolderToolStripButton
            // 
            newFolderToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            newFolderToolStripButton.Image = Properties.Resources.folder_new;
            newFolderToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            newFolderToolStripButton.Name = "newFolderToolStripButton";
            newFolderToolStripButton.Size = new System.Drawing.Size(23, 22);
            newFolderToolStripButton.Text = "New folder";
            newFolderToolStripButton.ToolTipText = "New folder...";
            // 
            // deleteToolStripButton
            // 
            deleteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            deleteToolStripButton.Image = Properties.Resources.bin_empty;
            deleteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            deleteToolStripButton.Name = "deleteToolStripButton";
            deleteToolStripButton.Size = new System.Drawing.Size(23, 22);
            deleteToolStripButton.Text = "Delete";
            // 
            // propertiesToolStripButton
            // 
            propertiesToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            propertiesToolStripButton.Image = Properties.Resources.page_white_edit;
            propertiesToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            propertiesToolStripButton.Name = "propertiesToolStripButton";
            propertiesToolStripButton.Size = new System.Drawing.Size(23, 22);
            propertiesToolStripButton.Text = "Properties";
            propertiesToolStripButton.ToolTipText = "Properties...";
            // 
            // toolStripSeparator9
            // 
            toolStripSeparator9.Name = "toolStripSeparator9";
            toolStripSeparator9.Size = new System.Drawing.Size(6, 25);
            // 
            // partitionComboBox
            // 
            partitionComboBox.AutoSize = false;
            partitionComboBox.AutoToolTip = true;
            partitionComboBox.DropDownHeight = 200;
            partitionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            partitionComboBox.DropDownWidth = 250;
            partitionComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            partitionComboBox.IntegralHeight = false;
            partitionComboBox.MaxDropDownItems = 64;
            partitionComboBox.Name = "partitionComboBox";
            partitionComboBox.Size = new System.Drawing.Size(250, 23);
            partitionComboBox.SelectedIndexChanged += PartitionComboBox_SelectedIndexChanged;
            // 
            // managePartitionsToolStripButton
            // 
            managePartitionsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            managePartitionsToolStripButton.Image = Properties.Resources.drive_edit;
            managePartitionsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            managePartitionsToolStripButton.Name = "managePartitionsToolStripButton";
            managePartitionsToolStripButton.Size = new System.Drawing.Size(23, 22);
            managePartitionsToolStripButton.Text = "Manage partitions";
            managePartitionsToolStripButton.ToolTipText = "Manage partitions...";
            // 
            // toolStripSeparator11
            // 
            toolStripSeparator11.Name = "toolStripSeparator11";
            toolStripSeparator11.Size = new System.Drawing.Size(6, 25);
            // 
            // labelToolStripMenuButton
            // 
            labelToolStripMenuButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            labelToolStripMenuButton.Image = Properties.Resources.drive_rename;
            labelToolStripMenuButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            labelToolStripMenuButton.Name = "labelToolStripMenuButton";
            labelToolStripMenuButton.Size = new System.Drawing.Size(23, 22);
            labelToolStripMenuButton.Text = "Change volume label";
            labelToolStripMenuButton.ToolTipText = "Change volume label...";
            // 
            // bootsectToolStripButton
            // 
            bootsectToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            bootsectToolStripButton.Image = (System.Drawing.Image)resources.GetObject("bootsectToolStripButton.Image");
            bootsectToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            bootsectToolStripButton.Name = "bootsectToolStripButton";
            bootsectToolStripButton.Size = new System.Drawing.Size(23, 22);
            bootsectToolStripButton.Text = "Boot sector properties";
            bootsectToolStripButton.ToolTipText = "Boot sector properties...";
            // 
            // infoToolStripButton
            // 
            infoToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            infoToolStripButton.Image = Properties.Resources.document_info;
            infoToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            infoToolStripButton.Name = "infoToolStripButton";
            infoToolStripButton.Size = new System.Drawing.Size(23, 22);
            infoToolStripButton.Text = "Image information";
            infoToolStripButton.ToolTipText = "Image information...";
            // 
            // toolStripSeparator10
            // 
            toolStripSeparator10.Name = "toolStripSeparator10";
            toolStripSeparator10.Size = new System.Drawing.Size(6, 25);
            // 
            // parentDirectoryToolStripButton
            // 
            parentDirectoryToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            parentDirectoryToolStripButton.Image = Properties.Resources.folder_up;
            parentDirectoryToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            parentDirectoryToolStripButton.Name = "parentDirectoryToolStripButton";
            parentDirectoryToolStripButton.Size = new System.Drawing.Size(23, 22);
            parentDirectoryToolStripButton.Text = "Parent directory";
            // 
            // viewToolStripButton
            // 
            viewToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            viewToolStripButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { showHiddenObjectsToolStripMenuItem1, toolStripSeparator14, largeIconsToolStripMenuItem1, smallIconsToolStripMenuItem1, listToolStripMenuItem1, detailsToolStripMenuItem1 });
            viewToolStripButton.Image = Properties.Resources.application_view_tile;
            viewToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            viewToolStripButton.Name = "viewToolStripButton";
            viewToolStripButton.Size = new System.Drawing.Size(29, 22);
            viewToolStripButton.Text = "View";
            // 
            // showHiddenObjectsToolStripMenuItem1
            // 
            showHiddenObjectsToolStripMenuItem1.Name = "showHiddenObjectsToolStripMenuItem1";
            showHiddenObjectsToolStripMenuItem1.Size = new System.Drawing.Size(184, 22);
            showHiddenObjectsToolStripMenuItem1.Text = "Show hidden objects";
            // 
            // toolStripSeparator14
            // 
            toolStripSeparator14.Name = "toolStripSeparator14";
            toolStripSeparator14.Size = new System.Drawing.Size(181, 6);
            // 
            // largeIconsToolStripMenuItem1
            // 
            largeIconsToolStripMenuItem1.Name = "largeIconsToolStripMenuItem1";
            largeIconsToolStripMenuItem1.Size = new System.Drawing.Size(184, 22);
            largeIconsToolStripMenuItem1.Text = "Large icons";
            // 
            // smallIconsToolStripMenuItem1
            // 
            smallIconsToolStripMenuItem1.Name = "smallIconsToolStripMenuItem1";
            smallIconsToolStripMenuItem1.Size = new System.Drawing.Size(184, 22);
            smallIconsToolStripMenuItem1.Text = "Small icons";
            // 
            // listToolStripMenuItem1
            // 
            listToolStripMenuItem1.Name = "listToolStripMenuItem1";
            listToolStripMenuItem1.Size = new System.Drawing.Size(184, 22);
            listToolStripMenuItem1.Text = "List";
            // 
            // detailsToolStripMenuItem1
            // 
            detailsToolStripMenuItem1.Checked = true;
            detailsToolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            detailsToolStripMenuItem1.Name = "detailsToolStripMenuItem1";
            detailsToolStripMenuItem1.Size = new System.Drawing.Size(184, 22);
            detailsToolStripMenuItem1.Text = "Details";
            // 
            // sortToolStripButton
            // 
            sortToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            sortToolStripButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { nameToolStripMenuItem1, typeToolStripMenuItem1, sizeToolStripMenuItem1, modifiedToolStripMenuItem1, attributesToolStripMenuItem1 });
            sortToolStripButton.Image = Properties.Resources.sort_descending;
            sortToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            sortToolStripButton.Name = "sortToolStripButton";
            sortToolStripButton.Size = new System.Drawing.Size(29, 22);
            sortToolStripButton.Text = "Sort by";
            // 
            // nameToolStripMenuItem1
            // 
            nameToolStripMenuItem1.Name = "nameToolStripMenuItem1";
            nameToolStripMenuItem1.Size = new System.Drawing.Size(126, 22);
            nameToolStripMenuItem1.Text = "Name";
            // 
            // typeToolStripMenuItem1
            // 
            typeToolStripMenuItem1.Name = "typeToolStripMenuItem1";
            typeToolStripMenuItem1.Size = new System.Drawing.Size(126, 22);
            typeToolStripMenuItem1.Text = "Type";
            // 
            // sizeToolStripMenuItem1
            // 
            sizeToolStripMenuItem1.Name = "sizeToolStripMenuItem1";
            sizeToolStripMenuItem1.Size = new System.Drawing.Size(126, 22);
            sizeToolStripMenuItem1.Text = "Size";
            // 
            // modifiedToolStripMenuItem1
            // 
            modifiedToolStripMenuItem1.Name = "modifiedToolStripMenuItem1";
            modifiedToolStripMenuItem1.Size = new System.Drawing.Size(126, 22);
            modifiedToolStripMenuItem1.Text = "Modified";
            // 
            // attributesToolStripMenuItem1
            // 
            attributesToolStripMenuItem1.Name = "attributesToolStripMenuItem1";
            attributesToolStripMenuItem1.Size = new System.Drawing.Size(126, 22);
            attributesToolStripMenuItem1.Text = "Attributes";
            // 
            // toolStripSeparator12
            // 
            toolStripSeparator12.Name = "toolStripSeparator12";
            toolStripSeparator12.Size = new System.Drawing.Size(6, 25);
            // 
            // settingsToolStripButton
            // 
            settingsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            settingsToolStripButton.Image = Properties.Resources.cog;
            settingsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            settingsToolStripButton.Name = "settingsToolStripButton";
            settingsToolStripButton.Size = new System.Drawing.Size(23, 22);
            settingsToolStripButton.Text = "Settings";
            settingsToolStripButton.ToolTipText = "Settings...";
            // 
            // splitContainer
            // 
            splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            splitContainer.Location = new System.Drawing.Point(0, 49);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(lstDirectories);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(lstFiles);
            splitContainer.Size = new System.Drawing.Size(984, 586);
            splitContainer.SplitterDistance = 280;
            splitContainer.SplitterWidth = 3;
            splitContainer.TabIndex = 3;
            // 
            // lstDirectories
            // 
            lstDirectories.AllowDrop = true;
            lstDirectories.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lstDirectories.Dock = System.Windows.Forms.DockStyle.Fill;
            lstDirectories.FullRowSelect = true;
            lstDirectories.HideSelection = false;
            lstDirectories.ImageKey = "folder_closed_16.png";
            lstDirectories.ImageList = imgFilesSmall;
            lstDirectories.LabelEdit = true;
            lstDirectories.Location = new System.Drawing.Point(0, 0);
            lstDirectories.Name = "lstDirectories";
            lstDirectories.SelectedImageIndex = 0;
            lstDirectories.ShowLines = false;
            lstDirectories.Size = new System.Drawing.Size(280, 586);
            lstDirectories.TabIndex = 0;
            lstDirectories.BeforeLabelEdit += lstDirectories_BeforeLabelEdit;
            lstDirectories.AfterLabelEdit += lstDirectories_AfterLabelEdit;
            lstDirectories.ItemDrag += lstDirectories_ItemDrag;
            lstDirectories.AfterSelect += lstDirectories_AfterSelect;
            lstDirectories.DragDrop += list_DragDrop;
            lstDirectories.DragEnter += list_DragEnter;
            lstDirectories.QueryContinueDrag += list_QueryContinueDrag;
            lstDirectories.Enter += lstDirectories_Enter;
            lstDirectories.MouseUp += lstDirectories_MouseUp;
            // 
            // imgFilesSmall
            // 
            imgFilesSmall.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imgFilesSmall.ImageSize = new System.Drawing.Size(16, 16);
            imgFilesSmall.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // lstFiles
            // 
            lstFiles.AllowColumnReorder = true;
            lstFiles.AllowDrop = true;
            lstFiles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lstFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { clmName, clmType, clmSize, clmModified, clmAttributes });
            lstFiles.ContextMenuStrip = cmsFileList;
            lstFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            lstFiles.FullRowSelect = true;
            lstFiles.LabelEdit = true;
            lstFiles.LargeImageList = imgFilesLarge;
            lstFiles.Location = new System.Drawing.Point(0, 0);
            lstFiles.Name = "lstFiles";
            lstFiles.ShowGroups = false;
            lstFiles.ShowItemToolTips = true;
            lstFiles.Size = new System.Drawing.Size(701, 586);
            lstFiles.SmallImageList = imgFilesSmall;
            lstFiles.Sorting = System.Windows.Forms.SortOrder.Ascending;
            lstFiles.TabIndex = 0;
            lstFiles.UseCompatibleStateImageBehavior = false;
            lstFiles.View = System.Windows.Forms.View.Details;
            lstFiles.VirtualMode = true;
            lstFiles.AfterLabelEdit += lstFiles_AfterLabelEdit;
            lstFiles.BeforeLabelEdit += lstFiles_BeforeLabelEdit;
            lstFiles.CacheVirtualItems += lstFiles_CacheVirtualItems;
            lstFiles.ColumnClick += lstFiles_ColumnClick;
            lstFiles.ItemDrag += lstFiles_ItemDrag;
            lstFiles.RetrieveVirtualItem += lstFiles_RetrieveVirtualItem;
            lstFiles.SearchForVirtualItem += lstFiles_SearchForVirtualItem;
            lstFiles.SelectedIndexChanged += lstFiles_SelectedIndexChanged;
            lstFiles.DragDrop += list_DragDrop;
            lstFiles.DragEnter += list_DragEnter;
            lstFiles.QueryContinueDrag += list_QueryContinueDrag;
            lstFiles.Enter += lstFiles_Enter;
            lstFiles.KeyPress += lstFiles_KeyPress;
            lstFiles.KeyUp += lstFiles_KeyUp;
            lstFiles.MouseDoubleClick += lstFiles_MouseDoubleClick;
            // 
            // clmName
            // 
            clmName.Name = "clmName";
            clmName.Tag = "Name";
            clmName.Text = "Name";
            clmName.Width = 150;
            // 
            // clmType
            // 
            clmType.Name = "clmType";
            clmType.Tag = "Type";
            clmType.Text = "Type";
            clmType.Width = 150;
            // 
            // clmSize
            // 
            clmSize.Name = "clmSize";
            clmSize.Tag = "Size";
            clmSize.Text = "Size";
            clmSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            clmSize.Width = 150;
            // 
            // clmModified
            // 
            clmModified.Name = "clmModified";
            clmModified.Tag = "Modified";
            clmModified.Text = "Modified";
            clmModified.Width = 150;
            // 
            // clmAttributes
            // 
            clmAttributes.Name = "clmAttributes";
            clmAttributes.Tag = "Attributes";
            clmAttributes.Text = "Attributes";
            clmAttributes.Width = 85;
            // 
            // cmsFileList
            // 
            cmsFileList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { parentDirectoryToolStripMenuItem1, toolStripSeparator7, viewToolStripMenuItem1, sortByToolStripMenuItem1, toolStripSeparator18, selectAllToolStripMenuItem1, toolStripSeparator22, injectToolStripMenuItem2, extractToolStripMenuItem2, newFolderToolStripMenuItem2, deleteToolStripMenuItem2, renameToolStripMenuItem2, propertiesToolStripMenuItem2 });
            cmsFileList.Name = "cmsFileList";
            cmsFileList.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            cmsFileList.Size = new System.Drawing.Size(217, 242);
            cmsFileList.Opening += cmsFileList_Opening;
            // 
            // parentDirectoryToolStripMenuItem1
            // 
            parentDirectoryToolStripMenuItem1.Image = Properties.Resources.folder_up;
            parentDirectoryToolStripMenuItem1.Name = "parentDirectoryToolStripMenuItem1";
            parentDirectoryToolStripMenuItem1.Size = new System.Drawing.Size(216, 22);
            parentDirectoryToolStripMenuItem1.Text = "Parent directory";
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new System.Drawing.Size(213, 6);
            // 
            // viewToolStripMenuItem1
            // 
            viewToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { showHiddenObjectsToolStripMenuItem2, toolStripSeparator19, largeIconsToolStripMenuItem2, smallIconsToolStripMenuItem2, listToolStripMenuItem2, detailsToolStripMenuItem2 });
            viewToolStripMenuItem1.Image = Properties.Resources.application_view_tile;
            viewToolStripMenuItem1.Name = "viewToolStripMenuItem1";
            viewToolStripMenuItem1.Size = new System.Drawing.Size(216, 22);
            viewToolStripMenuItem1.Text = "View";
            // 
            // showHiddenObjectsToolStripMenuItem2
            // 
            showHiddenObjectsToolStripMenuItem2.Name = "showHiddenObjectsToolStripMenuItem2";
            showHiddenObjectsToolStripMenuItem2.Size = new System.Drawing.Size(184, 22);
            showHiddenObjectsToolStripMenuItem2.Text = "Show hidden objects";
            // 
            // toolStripSeparator19
            // 
            toolStripSeparator19.Name = "toolStripSeparator19";
            toolStripSeparator19.Size = new System.Drawing.Size(181, 6);
            // 
            // largeIconsToolStripMenuItem2
            // 
            largeIconsToolStripMenuItem2.Name = "largeIconsToolStripMenuItem2";
            largeIconsToolStripMenuItem2.Size = new System.Drawing.Size(184, 22);
            largeIconsToolStripMenuItem2.Text = "Large icons";
            // 
            // smallIconsToolStripMenuItem2
            // 
            smallIconsToolStripMenuItem2.Name = "smallIconsToolStripMenuItem2";
            smallIconsToolStripMenuItem2.Size = new System.Drawing.Size(184, 22);
            smallIconsToolStripMenuItem2.Text = "Small icons";
            // 
            // listToolStripMenuItem2
            // 
            listToolStripMenuItem2.Name = "listToolStripMenuItem2";
            listToolStripMenuItem2.Size = new System.Drawing.Size(184, 22);
            listToolStripMenuItem2.Text = "List";
            // 
            // detailsToolStripMenuItem2
            // 
            detailsToolStripMenuItem2.Name = "detailsToolStripMenuItem2";
            detailsToolStripMenuItem2.Size = new System.Drawing.Size(184, 22);
            detailsToolStripMenuItem2.Text = "Details";
            // 
            // sortByToolStripMenuItem1
            // 
            sortByToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { nameToolStripMenuItem2, typeToolStripMenuItem2, sizeToolStripMenuItem2, modifiedToolStripMenuItem2, attributesToolStripMenuItem2 });
            sortByToolStripMenuItem1.Image = Properties.Resources.sort_descending;
            sortByToolStripMenuItem1.Name = "sortByToolStripMenuItem1";
            sortByToolStripMenuItem1.Size = new System.Drawing.Size(216, 22);
            sortByToolStripMenuItem1.Text = "Sort by";
            // 
            // nameToolStripMenuItem2
            // 
            nameToolStripMenuItem2.Name = "nameToolStripMenuItem2";
            nameToolStripMenuItem2.Size = new System.Drawing.Size(126, 22);
            nameToolStripMenuItem2.Text = "Name";
            // 
            // typeToolStripMenuItem2
            // 
            typeToolStripMenuItem2.Name = "typeToolStripMenuItem2";
            typeToolStripMenuItem2.Size = new System.Drawing.Size(126, 22);
            typeToolStripMenuItem2.Text = "Type";
            // 
            // sizeToolStripMenuItem2
            // 
            sizeToolStripMenuItem2.Name = "sizeToolStripMenuItem2";
            sizeToolStripMenuItem2.Size = new System.Drawing.Size(126, 22);
            sizeToolStripMenuItem2.Text = "Size";
            // 
            // modifiedToolStripMenuItem2
            // 
            modifiedToolStripMenuItem2.Name = "modifiedToolStripMenuItem2";
            modifiedToolStripMenuItem2.Size = new System.Drawing.Size(126, 22);
            modifiedToolStripMenuItem2.Text = "Modified";
            // 
            // attributesToolStripMenuItem2
            // 
            attributesToolStripMenuItem2.Name = "attributesToolStripMenuItem2";
            attributesToolStripMenuItem2.Size = new System.Drawing.Size(126, 22);
            attributesToolStripMenuItem2.Text = "Attributes";
            // 
            // toolStripSeparator18
            // 
            toolStripSeparator18.Name = "toolStripSeparator18";
            toolStripSeparator18.Size = new System.Drawing.Size(213, 6);
            // 
            // selectAllToolStripMenuItem1
            // 
            selectAllToolStripMenuItem1.Name = "selectAllToolStripMenuItem1";
            selectAllToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A;
            selectAllToolStripMenuItem1.Size = new System.Drawing.Size(216, 22);
            selectAllToolStripMenuItem1.Text = "Select all";
            // 
            // toolStripSeparator22
            // 
            toolStripSeparator22.Name = "toolStripSeparator22";
            toolStripSeparator22.Size = new System.Drawing.Size(213, 6);
            // 
            // injectToolStripMenuItem2
            // 
            injectToolStripMenuItem2.Image = Properties.Resources.page_white_add;
            injectToolStripMenuItem2.Name = "injectToolStripMenuItem2";
            injectToolStripMenuItem2.Size = new System.Drawing.Size(216, 22);
            injectToolStripMenuItem2.Text = "Inject files or folders...";
            // 
            // extractToolStripMenuItem2
            // 
            extractToolStripMenuItem2.Image = Properties.Resources.redo;
            extractToolStripMenuItem2.Name = "extractToolStripMenuItem2";
            extractToolStripMenuItem2.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X;
            extractToolStripMenuItem2.Size = new System.Drawing.Size(216, 22);
            extractToolStripMenuItem2.Text = "Extract...";
            // 
            // newFolderToolStripMenuItem2
            // 
            newFolderToolStripMenuItem2.Image = Properties.Resources.folder_new;
            newFolderToolStripMenuItem2.Name = "newFolderToolStripMenuItem2";
            newFolderToolStripMenuItem2.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.N;
            newFolderToolStripMenuItem2.Size = new System.Drawing.Size(216, 22);
            newFolderToolStripMenuItem2.Text = "New folder...";
            // 
            // deleteToolStripMenuItem2
            // 
            deleteToolStripMenuItem2.Image = Properties.Resources.bin_empty;
            deleteToolStripMenuItem2.Name = "deleteToolStripMenuItem2";
            deleteToolStripMenuItem2.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            deleteToolStripMenuItem2.Size = new System.Drawing.Size(216, 22);
            deleteToolStripMenuItem2.Text = "Delete";
            // 
            // renameToolStripMenuItem2
            // 
            renameToolStripMenuItem2.Image = Properties.Resources.textfield_rename;
            renameToolStripMenuItem2.Name = "renameToolStripMenuItem2";
            renameToolStripMenuItem2.ShortcutKeys = System.Windows.Forms.Keys.F2;
            renameToolStripMenuItem2.Size = new System.Drawing.Size(216, 22);
            renameToolStripMenuItem2.Text = "Rename...";
            // 
            // propertiesToolStripMenuItem2
            // 
            propertiesToolStripMenuItem2.Image = Properties.Resources.page_white_edit;
            propertiesToolStripMenuItem2.Name = "propertiesToolStripMenuItem2";
            propertiesToolStripMenuItem2.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Enter;
            propertiesToolStripMenuItem2.Size = new System.Drawing.Size(216, 22);
            propertiesToolStripMenuItem2.Text = "Properties...";
            // 
            // imgFilesLarge
            // 
            imgFilesLarge.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imgFilesLarge.ImageSize = new System.Drawing.Size(32, 32);
            imgFilesLarge.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // openContainingFolderToolStripMenuItem
            // 
            openContainingFolderToolStripMenuItem.Name = "openContainingFolderToolStripMenuItem";
            openContainingFolderToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            openContainingFolderToolStripMenuItem.Text = "Open containing folder...";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.SystemColors.Window;
            ClientSize = new System.Drawing.Size(984, 661);
            Controls.Add(splitContainer);
            Controls.Add(commandBar);
            Controls.Add(statusBar);
            Controls.Add(menuBar);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MainMenuStrip = menuBar;
            MinimumSize = new System.Drawing.Size(650, 397);
            Name = "MainForm";
            StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            Text = "TotalImage";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            cmsDirTree.ResumeLayout(false);
            menuBar.ResumeLayout(false);
            menuBar.PerformLayout();
            cmsToolbars.ResumeLayout(false);
            statusBar.ResumeLayout(false);
            statusBar.PerformLayout();
            commandBar.ResumeLayout(false);
            commandBar.PerformLayout();
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            cmsFileList.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuBar;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStrip commandBar;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recentFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem injectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extractToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem changeVolumeLabelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bootSectorPropertiesToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.ToolStripButton newToolStripButton;
        private System.Windows.Forms.ToolStripButton openToolStripButton;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
        private System.Windows.Forms.ColumnHeader clmName;
        private System.Windows.Forms.ColumnHeader clmSize;
        private System.Windows.Forms.ColumnHeader clmType;
        private System.Windows.Forms.ColumnHeader clmModified;
        private System.Windows.Forms.ToolStripStatusLabel partitionSizeLabel;
        private System.Windows.Forms.ToolStripMenuItem selectPartitionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem managePartitionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem largeIconsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem smallIconsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem detailsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem sortByToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem typeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modifiedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolbarsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuBarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem commandBarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem directoryTreeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statusBarToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmsToolbars;
        private System.Windows.Forms.ToolStripMenuItem commandBarToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem directoryTreeToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem statusBarToolStripMenuItem1;
        private System.Windows.Forms.ContextMenuStrip cmsDirTree;
        private System.Windows.Forms.ToolStripMenuItem extractToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem newFolderToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem1;
        private System.Windows.Forms.ContextMenuStrip cmsFileList;
        private System.Windows.Forms.ToolStripMenuItem extractToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem newFolderToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripButton injectToolStripButton;
        private System.Windows.Forms.ToolStripButton extractToolStripButton;
        private System.Windows.Forms.ToolStripButton newFolderToolStripButton;
        private System.Windows.Forms.ToolStripButton deleteToolStripButton;
        private System.Windows.Forms.ToolStripButton propertiesToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripButton managePartitionsToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripButton labelToolStripMenuButton;
        private System.Windows.Forms.ToolStripButton bootsectToolStripButton;
        private System.Windows.Forms.ToolStripButton infoToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripDropDownButton viewToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem largeIconsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem smallIconsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem listToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem detailsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripButton settingsToolStripButton;
        private System.Windows.Forms.ToolStripDropDownButton sortToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem nameToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem sizeToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem typeToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem modifiedToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem closeImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton closeImageToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem formatDiskToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defragmentToolStripMenuItem;
        private TreeViewEx lstDirectories;
        public ListViewEx lstFiles;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem showHiddenObjectsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripMenuItem showHiddenObjectsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripStatusLabel selectionSizeLabel;
        private System.Windows.Forms.ToolStripStatusLabel locationLabel;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem changeGeometryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandDirectoryTreeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseDirectoryTreeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandDirectoryTreeToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem collapseDirectoryTreeToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private System.Windows.Forms.ToolStripComboBox partitionComboBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator16;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageInformationToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator17;
        private System.Windows.Forms.ToolStripMenuItem hexViewToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader clmAttributes;
        private System.Windows.Forms.ToolStripStatusLabel freeSpaceLabel;
        private System.Windows.Forms.ToolStripProgressBar freeSpaceProgressBar;
        private System.Windows.Forms.ToolStripButton notificationsLabel;
        internal System.Windows.Forms.ImageList imgFilesSmall;
        internal System.Windows.Forms.ImageList imgFilesLarge;
        private System.Windows.Forms.ToolStripMenuItem parentDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton parentDirectoryToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem attributesToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem attributesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton saveAsToolStripButton;
        private System.Windows.Forms.ToolStripButton reloadImageToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem deletedObjectsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem parentDirectoryToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem showHiddenObjectsToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem sortByToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator18;
        private System.Windows.Forms.ToolStripMenuItem nameToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem typeToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem sizeToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem modifiedToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem attributesToolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator19;
        private System.Windows.Forms.ToolStripMenuItem largeIconsToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem smallIconsToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem listToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem detailsToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem injectToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem injectToolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator23;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator22;
        private System.Windows.Forms.ToolStripMenuItem onlineDocumentationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openContainingFolderToolStripMenuItem;
    }
}

