using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Magic;
using Magic.Editors;

namespace Magic.Components
{
    public class MagicButton : Button
    {
        protected IApp App;

        public String EditorToOpen = null;
        public UInt32 EntryToSelect = 0;

        public MagicButton(IApp app) : base()
        {
            this.App = app;

            this.Name = "";
            this.Text = "";
            this.Size = new Size(24, 24);
            this.MinimumSize = new Size(24, 24);
            this.MaximumSize = new Size(24, 24);
        }
    }
}
