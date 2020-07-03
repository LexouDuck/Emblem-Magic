namespace EmblemMagic.Editors
{
    partial class PatchEditor
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
            this.Patch_DataGrid = new System.Windows.Forms.DataGridView();
            this.Column_Author = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_PatchName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Applied = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DescriptionPanel = new System.Windows.Forms.Panel();
            this.Patch_Description_Label = new System.Windows.Forms.Label();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.ApplyButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Patch_DataGrid)).BeginInit();
            this.DescriptionPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Patch_DataGrid
            // 
            this.Patch_DataGrid.AllowUserToAddRows = false;
            this.Patch_DataGrid.AllowUserToDeleteRows = false;
            this.Patch_DataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Patch_DataGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Patch_DataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Patch_DataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_Author,
            this.Column_PatchName,
            this.Column_Applied});
            this.Patch_DataGrid.Location = new System.Drawing.Point(259, 12);
            this.Patch_DataGrid.MultiSelect = false;
            this.Patch_DataGrid.Name = "Patch_DataGrid";
            this.Patch_DataGrid.ReadOnly = true;
            this.Patch_DataGrid.Size = new System.Drawing.Size(433, 299);
            this.Patch_DataGrid.TabIndex = 0;
            this.Patch_DataGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.PatchesDataGrid_Click);
            this.Patch_DataGrid.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.PatchesDataGrid_Click);
            // 
            // Column_Author
            // 
            this.Column_Author.HeaderText = "Author";
            this.Column_Author.Name = "Column_Author";
            this.Column_Author.ReadOnly = true;
            // 
            // Column_PatchName
            // 
            this.Column_PatchName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column_PatchName.HeaderText = "Patch Name";
            this.Column_PatchName.Name = "Column_PatchName";
            this.Column_PatchName.ReadOnly = true;
            // 
            // Column_Applied
            // 
            this.Column_Applied.HeaderText = "Is Applied ?";
            this.Column_Applied.Name = "Column_Applied";
            this.Column_Applied.ReadOnly = true;
            this.Column_Applied.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column_Applied.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DescriptionPanel
            // 
            this.DescriptionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.DescriptionPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.DescriptionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DescriptionPanel.Controls.Add(this.Patch_Description_Label);
            this.DescriptionPanel.Location = new System.Drawing.Point(12, 12);
            this.DescriptionPanel.Name = "DescriptionPanel";
            this.DescriptionPanel.Size = new System.Drawing.Size(241, 338);
            this.DescriptionPanel.TabIndex = 1;
            // 
            // Patch_Description_Label
            // 
            this.Patch_Description_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Patch_Description_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.Patch_Description_Label.Location = new System.Drawing.Point(15, 2);
            this.Patch_Description_Label.Name = "Patch_Description_Label";
            this.Patch_Description_Label.Size = new System.Drawing.Size(208, 334);
            this.Patch_Description_Label.TabIndex = 0;
            this.Patch_Description_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RefreshButton
            // 
            this.RefreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RefreshButton.Location = new System.Drawing.Point(259, 317);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(131, 33);
            this.RefreshButton.TabIndex = 2;
            this.RefreshButton.Text = "Refresh List";
            this.Help_ToolTip.SetToolTip(this.RefreshButton, "Refresh the list of patches, to display new files you\'ve just put inside the \"Pat" +
        "ches\" folder.");
            this.RefreshButton.UseVisualStyleBackColor = true;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // ApplyButton
            // 
            this.ApplyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplyButton.Location = new System.Drawing.Point(561, 317);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(131, 33);
            this.ApplyButton.TabIndex = 3;
            this.ApplyButton.Text = "Apply Selected Patch";
            this.Help_ToolTip.SetToolTip(this.ApplyButton, "Apply the patch currently selected to the game.\r\nWill write to ROM if clicked.");
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // PatchEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 362);
            this.Controls.Add(this.ApplyButton);
            this.Controls.Add(this.RefreshButton);
            this.Controls.Add(this.DescriptionPanel);
            this.Controls.Add(this.Patch_DataGrid);
            this.MinimumSize = new System.Drawing.Size(650, 240);
            this.Name = "PatchEditor";
            this.Text = "Patch Editor";
            ((System.ComponentModel.ISupportInitialize)(this.Patch_DataGrid)).EndInit();
            this.DescriptionPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView Patch_DataGrid;
        private System.Windows.Forms.Panel DescriptionPanel;
        private System.Windows.Forms.Label Patch_Description_Label;
        private System.Windows.Forms.Button RefreshButton;
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn Patch_Column_Author;
        private System.Windows.Forms.DataGridViewTextBoxColumn Patch_Column_PatchName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Patch_Column_Applied;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Author;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_PatchName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Applied;
    }
}