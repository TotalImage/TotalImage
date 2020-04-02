using System.Windows.Forms;

namespace TotalImage
{
    public class CommandLink : Button
    {
        const int BS_COMMANDLINK = 0x0000000E;

        public CommandLink()
        {
            FlatStyle = FlatStyle.System; //FlastStyle needs to be System apparently
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cParams = base.CreateParams;
                cParams.Style |= BS_COMMANDLINK; //Set the Vista+ commandlink style
                return cParams;
            }
        }
    }
}