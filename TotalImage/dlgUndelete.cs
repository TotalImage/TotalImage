using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            FirstChar = txtFirstChar.Text[0]; //Character needs to be validated first...
        }
    }
}
