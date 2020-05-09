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
    public partial class dlgAbout : Form
    {
        public dlgAbout()
        {
            InitializeComponent();
        }

        private void dlgAbout_Load(object sender, EventArgs e)
        {
            lblVer.Text = "Version: " + System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
        }
    }
}