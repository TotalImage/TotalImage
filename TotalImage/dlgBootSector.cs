using System;
using System.IO;
using System.Windows.Forms;
using TotalImage.Partitions;

namespace TotalImage
{
    public partial class dlgBootSector : Form
    {
        private byte[] BootSectorBytes;
        private frmMain mainForm;

        public dlgBootSector()
        {
            InitializeComponent();
        }

        private void dlgBootSector_Load(object sender, EventArgs e)
        {
            mainForm = (frmMain)Application.OpenForms["frmMain"];

            rbnMBR.Enabled = mainForm.image is not null && mainForm.image.PartitionTable is not NoPartitionTable;
            rbnVBR.Checked = true;
        }

        //Keep this commented out for now
        private void btnLoad_Click(object sender, EventArgs e)
        {
            /*using OpenFileDialog ofd = new OpenFileDialog();
            ofd.AutoUpgradeEnabled = true;
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Multiselect = false;
            ofd.Filter = "All files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                FileInfo fi = new FileInfo(ofd.FileName);*/
            /* TODO: Reliable check against bootsector size of the current image to make sure the user doesn't try to load an oversized
             * bootsector. */
            /*
                FileSystems.FAT.FatFileSystem fs = (FileSystems.FAT.FatFileSystem)mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem;
                if (fi.Length > fs.BiosParameterBlock.ReservedLogicalSectors * fs.BiosParameterBlock.BytesPerLogicalSector)
                {
                    TaskDialog.ShowDialog(this, new TaskDialogPage()
                    {
                        Text = $"The file you are trying to use as the bootsector is larger than current bootsector size and cannot be used.{Environment.NewLine}{Environment.NewLine}" +
                        $"Select another file that's {fs.BiosParameterBlock.ReservedLogicalSectors * fs.BiosParameterBlock.BytesPerLogicalSector} bytes in size or smaller.",
                        Heading = "File too large",
                        Caption = "Error",
                        Buttons =
                    {
                        TaskDialogButton.OK
                    },
                        Icon = TaskDialogIcon.Error,
                        DefaultButton = TaskDialogButton.OK
                    });

                    return;
                }

                //Apply the bootsector
            }*/
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using SaveFileDialog sfd = new();
            sfd.AutoUpgradeEnabled = true;
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            sfd.OverwritePrompt = true;
            sfd.DefaultExt = "bin";
            sfd.Filter = "All files (*.*)|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                //Write all bytes to the file
                try
                {
                    File.WriteAllBytes(sfd.FileName, BootSectorBytes);

                    TaskDialog.ShowDialog(this, new TaskDialogPage()
                    {
                        Text = $"The bootsector was successfully exported to {sfd.FileName}",
                        Heading = "Export successful",
                        Caption = "Success",
                        Buttons =
                    {
                        TaskDialogButton.OK
                    },
                        Icon = TaskDialogIcon.Information,
                        DefaultButton = TaskDialogButton.OK
                    });
                }
                catch (UnauthorizedAccessException)
                {
                    TaskDialog.ShowDialog(this, new TaskDialogPage()
                    {
                        Text = $"File \"{sfd.FileName}\" could not be written to because you do not have the necessary permissions. Make sure you have the necessary permissions and try again.{Environment.NewLine}{Environment.NewLine}" +
                    $"If you think this is a bug, please submit a bug report on our GitHub repo.",
                        Heading = "Access denied",
                        Caption = "Error",
                        Buttons =
                        {
                            TaskDialogButton.OK
                        },
                        Icon = TaskDialogIcon.Error,
                    });
                }
                catch (IOException ex) when ((ex.HResult & 0x0000FFFF) == 32)
                {
                    TaskDialog.ShowDialog(this, new TaskDialogPage()
                    {
                        Text = $"File \"{sfd.FileName}\" could not be written to because it's locked by another process. Close all processes locking this file and try again.{Environment.NewLine}{Environment.NewLine}" +
                    $"If you think this is a bug, please submit a bug report on our GitHub repo.",
                        Heading = "Access denied",
                        Caption = "Error",
                        Buttons =
                        {
                            TaskDialogButton.OK
                        },
                        Icon = TaskDialogIcon.Error,
                    });
                }
                catch (Exception)
                {
                    TaskDialog.ShowDialog(this, new TaskDialogPage()
                    {
                        Text = $"Cannot write to the target file. Make sure you have the required permissions and that the file is not locked by another process, then try again.{Environment.NewLine}{Environment.NewLine}" +
                    $"If you think this is a bug, please submit a bug report on our GitHub repo.",
                        Heading = "Cannot write to file",
                        Caption = "Error",
                        Buttons =
                    {
                        TaskDialogButton.OK
                    },
                        Icon = TaskDialogIcon.Error,
                        DefaultButton = TaskDialogButton.OK
                    });
                }
            }
        }

        private void rbnVBR_CheckedChanged(object sender, EventArgs e)
        {
            if (mainForm.image is not null && rbnVBR.Checked)
            {
                FileSystems.FAT.FatFileSystem fs = (FileSystems.FAT.FatFileSystem)mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].FileSystem;
                var sectorSize = fs.BiosParameterBlock.BytesPerLogicalSector;
                ReadBootSectorBytes(false, sectorSize);
                PrintBootSectorBytes();
            }
        }

        private void rbnMBR_CheckedChanged(object sender, EventArgs e)
        {
            if (rbnMBR.Checked)
            {
                ReadBootSectorBytes(true, 512); //Hardcoding this to 512 bytes for now, since this was the sector size on the vast majority of hard disks
                PrintBootSectorBytes();
            }
        }

        //Prints the read bootsector bytes into the textbox as hex values with leading zeroes
        private void PrintBootSectorBytes()
        {
            txtBootSector.Clear();

            for (int i = 1; i <= BootSectorBytes.Length; i++)
            {
                txtBootSector.Text += string.Format("{0:X2}", BootSectorBytes[i - 1]);
                if (i % 16 == 0 && i < BootSectorBytes.Length)
                    txtBootSector.Text += Environment.NewLine;
                else
                    txtBootSector.Text += " ";
            }
        }

        //Reads the desired number of bootsector bytes of the MBR or the selected partition's VBR into an array
        private void ReadBootSectorBytes(bool mbr, int length)
        {
            if (mainForm.image is null)
                return;

            BootSectorBytes = new byte[length];

            if (mbr)
                mainForm.image.Content.Seek(0, SeekOrigin.Begin);
            else
                mainForm.image.Content.Seek(mainForm.image.PartitionTable.Partitions[mainForm.CurrentPartitionIndex].Offset, SeekOrigin.Begin);

            mainForm.image.Content.Read(BootSectorBytes, 0, length);
        }
    }
}
