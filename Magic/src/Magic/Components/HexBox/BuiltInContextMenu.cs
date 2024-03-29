using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

namespace Magic.Components
{
    /// <summary>
    /// Defines a build-in ContextMenuStrip manager for HexBox control to show Copy, Cut, Paste menu in contextmenu of the control.
    /// </summary>
    [TypeConverterAttribute(typeof(ExpandableObjectConverter))]
    public sealed class BuiltInContextMenu : Component
    {
        /// <summary>
        /// Contains the HexBox control.
        /// </summary>
        HexBox _hexBox;
        /// <summary>
        /// Contains the ContextMenuStrip control.
        /// </summary>
        ContextMenuStrip _contextMenuStrip;
        /// <summary>
        /// Contains the "Cut"-ToolStripMenuItem object.
        /// </summary>
        ToolStripMenuItem _cutToolStripMenuItem;
        /// <summary>
        /// Contains the "Copy"-ToolStripMenuItem object.
        /// </summary>
        ToolStripMenuItem _copyToolStripMenuItem;
        /// <summary>
        /// Contains the "Paste"-ToolStripMenuItem object.
        /// </summary>
        ToolStripMenuItem _pasteToolStripMenuItem;
        /// <summary>
        /// Contains the "Select All"-ToolStripMenuItem object.
        /// </summary>
        ToolStripMenuItem _selectAllToolStripMenuItem;
        /// <summary>
        /// Initializes a new instance of BuildInContextMenu class.
        /// </summary>
        /// <param name="hexBox">the HexBox control</param>
        internal BuiltInContextMenu(HexBox hexBox)
        {
            this._hexBox = hexBox;
            this._hexBox.ByteProviderChanged += new EventHandler(this.HexBox_ByteProviderChanged);
        }
        /// <summary>
        /// If ByteProvider
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event data</param>
        void HexBox_ByteProviderChanged(Object sender, EventArgs e)
        {
            this.CheckBuiltInContextMenu();
        }
        /// <summary>
        /// Assigns the ContextMenuStrip control to the HexBox control.
        /// </summary>
        void CheckBuiltInContextMenu()
        {
            if (this._contextMenuStrip == null)
            {
                ContextMenuStrip cms = new ContextMenuStrip();
                this._cutToolStripMenuItem = new ToolStripMenuItem(this.CutMenuItemTextInternal, this.CutMenuItemImage, new EventHandler(this.CutMenuItem_Click));
                cms.Items.Add(this._cutToolStripMenuItem);
                this._copyToolStripMenuItem = new ToolStripMenuItem(this.CopyMenuItemTextInternal, this.CopyMenuItemImage, new EventHandler(this.CopyMenuItem_Click));
                cms.Items.Add(this._copyToolStripMenuItem);
                this._pasteToolStripMenuItem = new ToolStripMenuItem(this.PasteMenuItemTextInternal, this.PasteMenuItemImage, new EventHandler(this.PasteMenuItem_Click));
                cms.Items.Add(this._pasteToolStripMenuItem);

                cms.Items.Add(new ToolStripSeparator());

                this._selectAllToolStripMenuItem = new ToolStripMenuItem(this.SelectAllMenuItemTextInternal, this.SelectAllMenuItemImage, new EventHandler(this.SelectAllMenuItem_Click));
                cms.Items.Add(this._selectAllToolStripMenuItem);
                cms.Opening += new CancelEventHandler(this.BuildInContextMenuStrip_Opening);

                this._contextMenuStrip = cms;
            }

            if (this._hexBox.ByteProvider == null && this._hexBox.ContextMenuStrip == this._contextMenuStrip)
                this._hexBox.ContextMenuStrip = null;
            else if (this._hexBox.ByteProvider != null && this._hexBox.ContextMenuStrip == null)
                this._hexBox.ContextMenuStrip = this._contextMenuStrip;
        }
        /// <summary>
        /// Before opening the ContextMenuStrip, we manage the availability of the items.
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event data</param>
        void BuildInContextMenuStrip_Opening(Object sender, CancelEventArgs e)
        {
            this._cutToolStripMenuItem.Enabled = this._hexBox.CanCut();
            this._copyToolStripMenuItem.Enabled = this._hexBox.CanCopy();
            this._pasteToolStripMenuItem.Enabled = this._hexBox.CanPaste();
            this._selectAllToolStripMenuItem.Enabled = this._hexBox.CanSelectAll();
        }
        /// <summary>
        /// The handler for the "Cut"-Click event
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event data</param>
        void CutMenuItem_Click(Object sender, EventArgs e) { this._hexBox.Cut(); }
        /// <summary>
        /// The handler for the "Copy"-Click event
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event data</param>
        void CopyMenuItem_Click(Object sender, EventArgs e) { this._hexBox.Copy(); }
        /// <summary>
        /// The handler for the "Paste"-Click event
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event data</param>
        void PasteMenuItem_Click(Object sender, EventArgs e) { this._hexBox.Paste(); }
        /// <summary>
        /// The handler for the "Select All"-Click event
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event data</param>
        void SelectAllMenuItem_Click(Object sender, EventArgs e) { this._hexBox.SelectAll(); }
        /// <summary>
        /// Gets or sets the custom text of the "Copy" ContextMenuStrip item.
        /// </summary>
        [Category("BuiltIn-ContextMenu"), DefaultValue(null), Localizable(true)]
        public String CopyMenuItemText
        {
            get { return this._copyMenuItemText; }
            set { this._copyMenuItemText = value; }
        }
        String _copyMenuItemText;

        /// <summary>
        /// Gets or sets the custom text of the "Cut" ContextMenuStrip item.
        /// </summary>
        [Category("BuiltIn-ContextMenu"), DefaultValue(null), Localizable(true)]
        public String CutMenuItemText
        {
            get { return this._cutMenuItemText; }
            set { this._cutMenuItemText = value; }
        }
        String _cutMenuItemText;

        /// <summary>
        /// Gets or sets the custom text of the "Paste" ContextMenuStrip item.
        /// </summary>
        [Category("BuiltIn-ContextMenu"), DefaultValue(null), Localizable(true)]
        public String PasteMenuItemText
        {
            get { return this._pasteMenuItemText; }
            set { this._pasteMenuItemText = value; }
        }
        String _pasteMenuItemText;

        /// <summary>
        /// Gets or sets the custom text of the "Select All" ContextMenuStrip item.
        /// </summary>
        [Category("BuiltIn-ContextMenu"), DefaultValue(null), Localizable(true)]
        public String SelectAllMenuItemText
        {
            get { return this._selectAllMenuItemText; }
            set { this._selectAllMenuItemText = value; }
        }
        String _selectAllMenuItemText = null;

        /// <summary>
        /// Gets the text of the "Cut" ContextMenuStrip item.
        /// </summary>
        internal String CutMenuItemTextInternal { get { return !String.IsNullOrEmpty(this.CutMenuItemText) ? this.CutMenuItemText : "Cut"; } }
        /// <summary>
        /// Gets the text of the "Copy" ContextMenuStrip item.
        /// </summary>
        internal String CopyMenuItemTextInternal { get { return !String.IsNullOrEmpty(this.CopyMenuItemText) ? this.CopyMenuItemText : "Copy"; } }
        /// <summary>
        /// Gets the text of the "Paste" ContextMenuStrip item.
        /// </summary>
        internal String PasteMenuItemTextInternal { get { return !String.IsNullOrEmpty(this.PasteMenuItemText) ? this.PasteMenuItemText : "Paste"; } }
        /// <summary>
        /// Gets the text of the "Select All" ContextMenuStrip item.
        /// </summary>
        internal String SelectAllMenuItemTextInternal { get { return !String.IsNullOrEmpty(this.SelectAllMenuItemText) ? this.SelectAllMenuItemText : "SelectAll"; } }

        /// <summary>
        /// Gets or sets the image of the "Cut" ContextMenuStrip item.
        /// </summary>
        [Category("BuiltIn-ContextMenu"), DefaultValue(null)]
        public Image CutMenuItemImage
        {
            get { return this._cutMenuItemImage; }
            set { this._cutMenuItemImage = value; }
        } Image _cutMenuItemImage = null;
        /// <summary>
        /// Gets or sets the image of the "Copy" ContextMenuStrip item.
        /// </summary>
        [Category("BuiltIn-ContextMenu"), DefaultValue(null)]
        public Image CopyMenuItemImage
        {
            get { return this._copyMenuItemImage; }
            set { this._copyMenuItemImage = value; }
        } Image _copyMenuItemImage = null;
        /// <summary>
        /// Gets or sets the image of the "Paste" ContextMenuStrip item.
        /// </summary>
        [Category("BuiltIn-ContextMenu"), DefaultValue(null)]
        public Image PasteMenuItemImage
        {
            get { return this._pasteMenuItemImage; }
            set { this._pasteMenuItemImage = value; }
        } Image _pasteMenuItemImage = null;
        /// <summary>
        /// Gets or sets the image of the "Select All" ContextMenuStrip item.
        /// </summary>
        [Category("BuiltIn-ContextMenu"), DefaultValue(null)]
        public Image SelectAllMenuItemImage
        {
            get { return this._selectAllMenuItemImage; }
            set { this._selectAllMenuItemImage = value; }
        } Image _selectAllMenuItemImage = null;
    }
}
