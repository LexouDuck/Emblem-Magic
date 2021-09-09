using Magic.Components;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Magic.Editors
{
    /// <summary>
    /// This class is not abstract only because that causes a problem with the winforms designer
    /// </summary>
    public class Editor : Form
    {
        protected ToolTip Help_ToolTip;
        private System.ComponentModel.IContainer components;

        public String Identifier;

        public IApp App;

        public Editor(IApp app)
        {
            this.App = app;

            InitializeComponent();

            this.Load += new EventHandler(Core_OnLoad);
            this.FormClosing += new FormClosingEventHandler(Core_OnExit);
            this.FormClosed += new FormClosedEventHandler(Core_CloseEditor);
        }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Help_ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // Editor
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Name = "Editor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);

        }

        public void Core_OnLoad(Object sender, EventArgs e)
        {
            try
            {
                Core_OnOpen();
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while loading the " + this.Text, ex);
            }
        }

        /// <summary>
        /// Is called by EM shortcut buttons to set the correct index
        /// </summary>
        public virtual void Core_SetEntry(UInt32 entry) { }
        /// <summary>
        /// Is called when the editor opens (after the constructor)
        /// </summary>
        public virtual void Core_OnOpen() { }
        /// <summary>
        /// Is called whenever a change is made to the ROM outside this editor
        /// </summary>
        public virtual void Core_Update() { }
        /// <summary>
        /// Is called when (just before) the editor closes
        /// </summary>
        public virtual void Core_OnExit(Object sender, FormClosingEventArgs e) { }

        /// <summary>
        /// Is called when the editor closes.
        /// </summary>
        public void Core_CloseEditor(Object sender, FormClosedEventArgs e)
        {
            App.Core_ExitEditor(this);
            this.Dispose();
        }



        /// <summary>
        /// Allows ctrl + A on multiline textboxes
        /// </summary>
        protected void TextBox_SelectAll(Object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (sender != null)
                {
                    if (sender is TextBox) ((TextBox)sender).SelectAll();
                    if (sender is HexBox) ((HexBox)sender).SelectAll();
                }
            }
        }
        /// <summary>
        /// Allows for using Ctrl+Z or Ctrl+Y for undos and redos from any editor window
        /// </summary>
        protected override Boolean ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (App.Edit_Undo.Enabled && keyData == (Keys.Control | Keys.Z))
            {
                App.Core_Undo();
                return true;
            }
            if (App.Edit_Redo.Enabled && keyData == (Keys.Control | Keys.Y))
            {
                App.Core_Redo();
                return true;
            }
            if (keyData == (Keys.Control | Keys.M))
            {
                App.Core_MarkSpace();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
