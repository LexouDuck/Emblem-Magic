namespace EmblemMagic.Editors
{
    partial class ModuleEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Status = new System.Windows.Forms.StatusStrip();
            this.Status_Author = new System.Windows.Forms.ToolStripStatusLabel();
            this.MenuBar = new System.Windows.Forms.MenuStrip();
            this.Menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.File_OpenModule = new EmblemMagic.Components.FolderViewMenu();
            this.File_OpenEMMFile = new System.Windows.Forms.ToolStripMenuItem();
            this.Status_Module = new System.Windows.Forms.ToolStripLabel();
            this.LayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.Status.SuspendLayout();
            this.MenuBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // Status
            // 
            this.Status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Status_Author});
            this.Status.Location = new System.Drawing.Point(0, 240);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(334, 22);
            this.Status.TabIndex = 0;
            this.Status.Text = "Author";
            // 
            // Status_Author
            // 
            this.Status_Author.Name = "Status_Author";
            this.Status_Author.Size = new System.Drawing.Size(99, 17);
            this.Status_Author.Text = "Module author(s)";
            // 
            // MenuBar
            // 
            this.MenuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File,
            this.Status_Module});
            this.MenuBar.Location = new System.Drawing.Point(0, 0);
            this.MenuBar.Name = "MenuBar";
            this.MenuBar.Size = new System.Drawing.Size(334, 24);
            this.MenuBar.TabIndex = 1;
            this.MenuBar.Text = "Menu";
            // 
            // Menu_File
            // 
            this.Menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File_OpenModule,
            this.File_OpenEMMFile});
            this.Menu_File.Name = "Menu_File";
            this.Menu_File.Size = new System.Drawing.Size(37, 20);
            this.Menu_File.Text = "File";
            // 
            // File_OpenModule
            // 
            this.File_OpenModule.Name = "File_OpenModule";
            this.File_OpenModule.Size = new System.Drawing.Size(165, 22);
            this.File_OpenModule.Text = "Open Module";
            this.File_OpenModule.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.File_OpenModule_Click);
            // 
            // File_OpenEMMFile
            // 
            this.File_OpenEMMFile.Name = "File_OpenEMMFile";
            this.File_OpenEMMFile.Size = new System.Drawing.Size(165, 22);
            this.File_OpenEMMFile.Text = "Open .EMM file...";
            this.File_OpenEMMFile.Click += new System.EventHandler(this.File_OpenEMMFile_Click);
            // 
            // Status_Module
            // 
            this.Status_Module.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.Status_Module.Name = "Status_Module";
            this.Status_Module.Padding = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.Status_Module.Size = new System.Drawing.Size(68, 17);
            this.Status_Module.Text = "Module";
            // 
            // LayoutPanel
            // 
            this.LayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LayoutPanel.AutoScroll = true;
            this.LayoutPanel.Location = new System.Drawing.Point(12, 27);
            this.LayoutPanel.Name = "LayoutPanel";
            this.LayoutPanel.Size = new System.Drawing.Size(322, 210);
            this.LayoutPanel.TabIndex = 2;
            // 
            // ModuleEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 262);
            this.Controls.Add(this.LayoutPanel);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.MenuBar);
            this.MainMenuStrip = this.MenuBar;
            this.MinimumSize = new System.Drawing.Size(100, 100);
            this.Name = "ModuleEditor";
            this.Text = "Module Editor";
            this.Status.ResumeLayout(false);
            this.Status.PerformLayout();
            this.MenuBar.ResumeLayout(false);
            this.MenuBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MenuBar;
        private System.Windows.Forms.ToolStripMenuItem Menu_File;
        private EmblemMagic.Components.FolderViewMenu File_OpenModule;
        private System.Windows.Forms.ToolStripMenuItem File_OpenEMMFile;
        private System.Windows.Forms.FlowLayoutPanel LayoutPanel;
        private System.Windows.Forms.StatusStrip Status;
        private System.Windows.Forms.ToolStripLabel Status_Module;
        private System.Windows.Forms.ToolStripStatusLabel Status_Author;
    }
}