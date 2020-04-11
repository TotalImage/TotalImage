using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TotalImage
{
    public partial class dlgProperties : Form
    {
        public dlgProperties()
        {
            InitializeComponent();
        }

        private void dlgProperties_Load(object sender, EventArgs e)
        {
            dateCreated.CustomFormat = CultureInfo.CurrentCulture.DateTimeFormat.UniversalSortableDateTimePattern;
            dateModified.CustomFormat = CultureInfo.CurrentCulture.DateTimeFormat.UniversalSortableDateTimePattern;
            dateAccessed.CustomFormat = CultureInfo.CurrentCulture.DateTimeFormat.UniversalSortableDateTimePattern;
        }
    }
}
