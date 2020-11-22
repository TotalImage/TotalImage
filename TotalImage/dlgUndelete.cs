using System;
using System.Windows.Forms;

namespace TotalImage
{
    public partial class dlgUndelete : Form
    {
        public char FirstChar { get; private set; }

        public dlgUndelete()
        {
            InitializeComponent();
        }

        //TODO: Perform character/full name validation
        private void btnOK_Click(object sender, EventArgs e)
        {
            FirstChar = txtFirstChar.Text[0];
        }
    }
}