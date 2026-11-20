using System.Windows.Forms;

namespace TotalImage.Services
{
    /// <summary>
    /// The window that owns the dialogs shown in the current scope. The main window sets itself
    /// here when it is created.
    /// </summary>
    public sealed class DialogOwner
    {
        public IWin32Window? Window { get; set; }
    }
}
