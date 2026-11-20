using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using TotalImage.ViewModels.Settings;

namespace TotalImage.Services
{
    /// <summary>
    /// The UI state model for storing values related to the state of the main window and its child controls.
    /// </summary>
    public class UIStateModel
    {
        /// <summary>
        /// Location of the splitter between the directory tree and the file list. Only relevant if the directory tree is enabled.
        /// </summary>
        public int SplitterDistance { get; set; } = 280;
        /// <summary>
        /// Size of the main window.
        /// </summary>
        public Size WindowSize { get; set; } = new Size(1000, 700);
        /// <summary>
        /// Position of the main window on the screen.
        /// </summary>
        public Point WindowPosition { get; set; } = new Point((Screen.PrimaryScreen!.Bounds.Width - 1000) / 2, (Screen.PrimaryScreen.Bounds.Height - 700) / 2);
        /// <summary>
        /// State of the main window.
        /// </summary>
        public FormWindowState WindowState { get; set; } = FormWindowState.Normal;
        /// <summary>
        /// Order of the columns in the file list in the main window.
        /// </summary>
        public List<int> MWColumnOrder { get; set; } = new List<int>(new[] { 0, 1, 2, 3, 4 });
        /// <summary>
        /// Width of each column in the file list in the main window.
        /// </summary>
        public List<int> MWColumnWidth { get; set; } = new List<int>(new[] { 150, 150, 150, 150, 85 });
    }

    /// <summary>
    /// Stores the WinForms UI state in uistate.json, next to settings.json.
    /// </summary>
    public sealed class UIStateService
    {
        private static readonly JsonSerializerOptions WriteOptions = new() { WriteIndented = true };

        private readonly string directory;
        private readonly string file;

        /// <summary>
        /// The currently used UI state.
        /// </summary>
        public UIStateModel Current { get; private set; }

        public UIStateService() : this(SettingsService.DefaultDirectory) { }

        public UIStateService(string directory)
        {
            this.directory = directory;
            file = Path.Combine(directory, "uistate.json");
            Current = new UIStateModel();
            Reload();
        }

        /// <summary>
        /// Reloads the UI state from the uistate.json file. If the file doesn't exist or can't be used, the default values are loaded instead.
        /// </summary>
        public void Reload()
        {
            UIStateModel? state = null;
            try
            {
                if (File.Exists(file))
                    state = JsonSerializer.Deserialize<UIStateModel>(File.ReadAllText(file));
            }
            catch (System.Exception ex) when (ex is IOException or JsonException)
            {
            }

            if (state is not null)
            {
                Current = state;
            }
            else
            {
                Current = new UIStateModel();
                Save();
            }
        }

        /// <summary>
        /// Saves the current UI state to file uistate.json.
        /// </summary>
        public void Save()
        {
            var json = JsonSerializer.Serialize(Current, WriteOptions);

            Directory.CreateDirectory(directory);

            try
            {
                File.WriteAllText(file, json);
            }
            catch (IOException)
            {
                //We can't write to the file, probably because it's opened by another instance, so let's just silently give up
            }
        }
    }
}
