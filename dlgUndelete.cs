using TotalImage.Validation.ValidationMethods;
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
            FirstCharacterValidation FCV = new FirstCharacterValidation();

            // Convert to object

            // Don't do anything if it's not valid
            object _ = FCV.Validate(txtFirstChar.Text[0]);

            if (_ == null) return;

            FirstChar = (char)_; //Character needs to be validated first...
        }
    }
}
