using System;
using System.Windows.Forms;

namespace TotalImage
{
    public partial class dlgNewImageAdvanced : Form
    {
        public dlgNewImageAdvanced()
        {
            InitializeComponent();
        }

        //TODO: Should the capacities list be loaded from the KnownGeometries list or something like that?
        private void dlgNewImageAdvanced_Load(object sender, EventArgs e)
        {
            txtFloppySerial.Text = GenerateVolumeID().ToString("X8");
        }

        //TODO: Perform some validation of parameters before leaving in case the user tries to create an impossible image
        private void btnOK_Click(object sender, EventArgs e)
        {
            /*if (!cbxFloppyBPB.Checked)
            {
                TaskDialogButton result = TaskDialog.ShowDialog(this, new TaskDialogPage()
                {
                    Text = "Some programs and operating systems may not recognize the disk because of this. Are you sure you want to continue?",
                    Heading = "You chose not to write a DOS BIOS Parameter Block (BPB)",
                    Caption = "Warning",
                    Buttons =
                        {
                            TaskDialogButton.Yes,
                            TaskDialogButton.No
                        },
                    Icon = TaskDialogIcon.Warning,
                });

                if (result == TaskDialogButton.No)
                {
                    DialogResult = DialogResult.None; //This is needed so the dialog doesn't close anyway
                    return;
                }
            }*/

            //If the "Custom..." option is selected, create a new FloppyGeometry with the custom parameters
            //We don't need some of the parameters yet for raw images, so let's just ignore them for now
            /*if (selectedItem == FloppyGeometry.FriendlyName.Custom)
            {
                Geometry = new FloppyGeometry(0, (byte)(lstFloppySides.SelectedIndex + 1), 0, 0, 0, (byte)txtFloppyTracks.Value,
                    (byte)txtFloppySPT.Value, (byte)(Math.Log((double)txtFloppyBPS.Value, 2) - 7), (byte)txtFloppyMediaDesc.Value, (byte)txtFloppySPC.Value,
                    (byte)txtFloppyNumFATs.Value, (byte)txtFloppySPF.Value, (ushort)txtFloppyRootDir.Value, (byte)txtFloppyReservedSect.Value);
            }*/
        }

        /* Generates a random volume ID/serial number for DOS 3.4+ BPB
         * TODO: Should this be moved elsewhere? */
        private static int GenerateVolumeID()
        {
            Random rnd = new Random();
            return rnd.Next();
        }

        private void txtFloppyNumFATs_ValueChanged(object sender, EventArgs e)
        {
            /*if (selectedItem != FloppyGeometry.FriendlyName.Custom && txtFloppyNumFATs.Value != Geometry.NoOfFATs)
            {
                lstFloppyCapacity.SelectedValue = FloppyGeometry.FriendlyName.Custom;
            }*/
        }

        private void txtFloppyReserved_ValueChanged(object sender, EventArgs e)
        {
            /*if (selectedItem != FloppyGeometry.FriendlyName.Custom && txtFloppyReservedSect.Value != Geometry.ReservedSectors)
            {
                lstFloppyCapacity.SelectedValue = FloppyGeometry.FriendlyName.Custom;
            }*/
        }

        private void lstFloppySides_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*if (selectedItem != FloppyGeometry.FriendlyName.Custom && lstFloppySides.SelectedIndex != Geometry.Sides - 1)
            {
                lstFloppyCapacity.SelectedValue = FloppyGeometry.FriendlyName.Custom;
            }*/
        }

        private void txtFloppySPF_ValueChanged(object sender, EventArgs e)
        {
            /*if (selectedItem != FloppyGeometry.FriendlyName.Custom && txtFloppySPF.Value != Geometry.SPF)
            {
                lstFloppyCapacity.SelectedValue = FloppyGeometry.FriendlyName.Custom;
            }*/
        }

        private void txtFloppySPT_ValueChanged(object sender, EventArgs e)
        {
            /*if (selectedItem != FloppyGeometry.FriendlyName.Custom && txtFloppySPT.Value != Geometry.SPT)
            {
                lstFloppyCapacity.SelectedValue = FloppyGeometry.FriendlyName.Custom;
            }*/
        }

        private void txtFloppySPC_ValueChanged(object sender, EventArgs e)
        {
            /*if (selectedItem != FloppyGeometry.FriendlyName.Custom && txtFloppySPC.Value != Geometry.SPC)
            {
                lstFloppyCapacity.SelectedValue = FloppyGeometry.FriendlyName.Custom;
            }*/
        }

        private void txtFloppyBPS_ValueChanged(object sender, EventArgs e)
        {
            /*if (selectedItem != FloppyGeometry.FriendlyName.Custom && txtFloppyBPS.Value != (128 << Geometry.BPS))
            {
                lstFloppyCapacity.SelectedValue = FloppyGeometry.FriendlyName.Custom;
            }*/
        }

        private void txtFloppyTotalSect_ValueChanged(object sender, EventArgs e)
        {
            /*if (selectedItem != FloppyGeometry.FriendlyName.Custom)
            {
                int total_sect = Geometry.Tracks * Geometry.SPT * Geometry.Sides;

                if (txtFloppyTotalSect.Value != total_sect)
                {
                    lstFloppyCapacity.SelectedValue = FloppyGeometry.FriendlyName.Custom;
                }
            }*/
        }

        private void txtFloppyRootDirEntries_ValueChanged(object sender, EventArgs e)
        {
            /*if (selectedItem != FloppyGeometry.FriendlyName.Custom && txtFloppyRootDir.Value != Geometry.RootDirectoryEntries)
            {
                lstFloppyCapacity.SelectedValue = FloppyGeometry.FriendlyName.Custom;
            }*/
        }

        private void txtFloppyTracks_ValueChanged(object sender, EventArgs e)
        {
            /*if (selectedItem != FloppyGeometry.FriendlyName.Custom && txtFloppyTracks.Value != Geometry.Tracks)
            {
                lstFloppyCapacity.SelectedValue = FloppyGeometry.FriendlyName.Custom;
            }*/
        }

        private void txtFloppyMediaDesc_ValueChanged(object sender, EventArgs e)
        {
            /*if (selectedItem != FloppyGeometry.FriendlyName.Custom && txtFloppyMediaDesc.Value != Geometry.MediaDescriptor)
            {
                lstFloppyCapacity.SelectedValue = FloppyGeometry.FriendlyName.Custom;
            }*/
        }
    }
}