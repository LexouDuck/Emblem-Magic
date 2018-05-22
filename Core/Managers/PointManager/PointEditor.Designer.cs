namespace EmblemMagic
{
    partial class PointEditor
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
            this.PointerDataGrid = new System.Windows.Forms.DataGridView();
            this.AssetColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RepointedColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AddressColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReferencesColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReferenceListBox = new System.Windows.Forms.ListBox();
            this.Repoint_GroupBox = new System.Windows.Forms.GroupBox();
            this.Repoint_CurrentPointerBox = new EmblemMagic.Components.PointerBox();
            this.Repoint_CurrentLabel = new System.Windows.Forms.Label();
            this.Repoint_DefaultLabel = new System.Windows.Forms.Label();
            this.Repoint_DefaultPointerBox = new EmblemMagic.Components.PointerBox();
            this.Repoint_NameTextBox = new System.Windows.Forms.TextBox();
            this.CreateButton = new System.Windows.Forms.Button();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.RepointButton = new System.Windows.Forms.Button();
            this.ShowHideCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.PointerDataGrid)).BeginInit();
            this.Repoint_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Repoint_CurrentPointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Repoint_DefaultPointerBox)).BeginInit();
            this.SuspendLayout();
            // 
            // PointerDataGrid
            // 
            this.PointerDataGrid.AllowUserToAddRows = false;
            this.PointerDataGrid.AllowUserToDeleteRows = false;
            this.PointerDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PointerDataGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PointerDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PointerDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AssetColumn,
            this.RepointedColumn,
            this.AddressColumn,
            this.ReferencesColumn});
            this.PointerDataGrid.Location = new System.Drawing.Point(12, 12);
            this.PointerDataGrid.Name = "PointerDataGrid";
            this.PointerDataGrid.ReadOnly = true;
            this.PointerDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.PointerDataGrid.Size = new System.Drawing.Size(474, 309);
            this.PointerDataGrid.TabIndex = 0;
            this.PointerDataGrid.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.SelectPointer_Click);
            // 
            // AssetColumn
            // 
            this.AssetColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.AssetColumn.HeaderText = "Asset Name";
            this.AssetColumn.MinimumWidth = 100;
            this.AssetColumn.Name = "AssetColumn";
            this.AssetColumn.ReadOnly = true;
            // 
            // RepointedColumn
            // 
            this.RepointedColumn.HeaderText = "Repointed";
            this.RepointedColumn.MinimumWidth = 70;
            this.RepointedColumn.Name = "RepointedColumn";
            this.RepointedColumn.ReadOnly = true;
            this.RepointedColumn.Width = 70;
            // 
            // AddressColumn
            // 
            this.AddressColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.AddressColumn.HeaderText = "Address";
            this.AddressColumn.MinimumWidth = 70;
            this.AddressColumn.Name = "AddressColumn";
            this.AddressColumn.ReadOnly = true;
            // 
            // ReferencesColumn
            // 
            this.ReferencesColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ReferencesColumn.HeaderText = "References";
            this.ReferencesColumn.MinimumWidth = 50;
            this.ReferencesColumn.Name = "ReferencesColumn";
            this.ReferencesColumn.ReadOnly = true;
            // 
            // ReferenceListBox
            // 
            this.ReferenceListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ReferenceListBox.FormattingEnabled = true;
            this.ReferenceListBox.Location = new System.Drawing.Point(378, 327);
            this.ReferenceListBox.Name = "ReferenceListBox";
            this.ReferenceListBox.ScrollAlwaysVisible = true;
            this.ReferenceListBox.Size = new System.Drawing.Size(108, 108);
            this.ReferenceListBox.TabIndex = 1;
            // 
            // Repoint_GroupBox
            // 
            this.Repoint_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Repoint_GroupBox.Controls.Add(this.Repoint_CurrentPointerBox);
            this.Repoint_GroupBox.Controls.Add(this.Repoint_CurrentLabel);
            this.Repoint_GroupBox.Controls.Add(this.Repoint_DefaultLabel);
            this.Repoint_GroupBox.Controls.Add(this.Repoint_DefaultPointerBox);
            this.Repoint_GroupBox.Controls.Add(this.Repoint_NameTextBox);
            this.Repoint_GroupBox.Location = new System.Drawing.Point(12, 327);
            this.Repoint_GroupBox.Name = "Repoint_GroupBox";
            this.Repoint_GroupBox.Size = new System.Drawing.Size(183, 108);
            this.Repoint_GroupBox.TabIndex = 2;
            this.Repoint_GroupBox.TabStop = false;
            this.Repoint_GroupBox.Text = "Repointing";
            // 
            // Repoint_CurrentPointerBox
            // 
            this.Repoint_CurrentPointerBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Repoint_CurrentPointerBox.Hexadecimal = true;
            this.Repoint_CurrentPointerBox.Location = new System.Drawing.Point(103, 76);
            this.Repoint_CurrentPointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Repoint_CurrentPointerBox.Name = "Repoint_CurrentPointerBox";
            this.Repoint_CurrentPointerBox.Size = new System.Drawing.Size(70, 20);
            this.Repoint_CurrentPointerBox.TabIndex = 4;
            // 
            // Repoint_CurrentLabel
            // 
            this.Repoint_CurrentLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Repoint_CurrentLabel.AutoSize = true;
            this.Repoint_CurrentLabel.Location = new System.Drawing.Point(9, 78);
            this.Repoint_CurrentLabel.Name = "Repoint_CurrentLabel";
            this.Repoint_CurrentLabel.Size = new System.Drawing.Size(88, 13);
            this.Repoint_CurrentLabel.TabIndex = 3;
            this.Repoint_CurrentLabel.Text = "Current Address :";
            // 
            // Repoint_DefaultLabel
            // 
            this.Repoint_DefaultLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Repoint_DefaultLabel.AutoSize = true;
            this.Repoint_DefaultLabel.Location = new System.Drawing.Point(9, 52);
            this.Repoint_DefaultLabel.Name = "Repoint_DefaultLabel";
            this.Repoint_DefaultLabel.Size = new System.Drawing.Size(88, 13);
            this.Repoint_DefaultLabel.TabIndex = 2;
            this.Repoint_DefaultLabel.Text = "Default Address :";
            // 
            // Repoint_DefaultPointerBox
            // 
            this.Repoint_DefaultPointerBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Repoint_DefaultPointerBox.Hexadecimal = true;
            this.Repoint_DefaultPointerBox.Location = new System.Drawing.Point(103, 50);
            this.Repoint_DefaultPointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Repoint_DefaultPointerBox.Name = "Repoint_DefaultPointerBox";
            this.Repoint_DefaultPointerBox.ReadOnly = true;
            this.Repoint_DefaultPointerBox.Size = new System.Drawing.Size(70, 20);
            this.Repoint_DefaultPointerBox.TabIndex = 1;
            // 
            // Repoint_NameTextBox
            // 
            this.Repoint_NameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Repoint_NameTextBox.Location = new System.Drawing.Point(12, 19);
            this.Repoint_NameTextBox.MaxLength = 32;
            this.Repoint_NameTextBox.Name = "Repoint_NameTextBox";
            this.Repoint_NameTextBox.Size = new System.Drawing.Size(161, 20);
            this.Repoint_NameTextBox.TabIndex = 0;
            // 
            // CreateButton
            // 
            this.CreateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CreateButton.Location = new System.Drawing.Point(201, 346);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new System.Drawing.Size(81, 46);
            this.CreateButton.TabIndex = 3;
            this.CreateButton.Text = "Create new Asset Pointer";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DeleteButton.Location = new System.Drawing.Point(288, 346);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(84, 46);
            this.DeleteButton.TabIndex = 4;
            this.DeleteButton.Text = "Delete Selection";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // RepointButton
            // 
            this.RepointButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RepointButton.Location = new System.Drawing.Point(201, 395);
            this.RepointButton.Name = "RepointButton";
            this.RepointButton.Size = new System.Drawing.Size(171, 40);
            this.RepointButton.TabIndex = 5;
            this.RepointButton.Text = "Apply changes and Repoint";
            this.RepointButton.UseVisualStyleBackColor = true;
            this.RepointButton.Click += new System.EventHandler(this.RepointButton_Click);
            // 
            // ShowHideCheckBox
            // 
            this.ShowHideCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ShowHideCheckBox.AutoSize = true;
            this.ShowHideCheckBox.Location = new System.Drawing.Point(207, 327);
            this.ShowHideCheckBox.Name = "ShowHideCheckBox";
            this.ShowHideCheckBox.Size = new System.Drawing.Size(165, 17);
            this.ShowHideCheckBox.TabIndex = 6;
            this.ShowHideCheckBox.Text = "Hide non-repointed list entries";
            this.ShowHideCheckBox.UseVisualStyleBackColor = true;
            this.ShowHideCheckBox.CheckedChanged += new System.EventHandler(this.ShowHideCheckBox_CheckedChanged);
            // 
            // PointEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 447);
            this.Controls.Add(this.ShowHideCheckBox);
            this.Controls.Add(this.RepointButton);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.CreateButton);
            this.Controls.Add(this.Repoint_GroupBox);
            this.Controls.Add(this.ReferenceListBox);
            this.Controls.Add(this.PointerDataGrid);
            this.Name = "PointEditor";
            this.Text = "Pointer Manager";
            ((System.ComponentModel.ISupportInitialize)(this.PointerDataGrid)).EndInit();
            this.Repoint_GroupBox.ResumeLayout(false);
            this.Repoint_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Repoint_CurrentPointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Repoint_DefaultPointerBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView PointerDataGrid;
        private System.Windows.Forms.ListBox ReferenceListBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn AssetColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn RepointedColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AddressColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReferencesColumn;
        private System.Windows.Forms.GroupBox Repoint_GroupBox;
        private System.Windows.Forms.TextBox Repoint_NameTextBox;
        private Components.PointerBox Repoint_CurrentPointerBox;
        private System.Windows.Forms.Label Repoint_CurrentLabel;
        private System.Windows.Forms.Label Repoint_DefaultLabel;
        private Components.PointerBox Repoint_DefaultPointerBox;
        private System.Windows.Forms.Button CreateButton;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button RepointButton;
        private System.Windows.Forms.CheckBox ShowHideCheckBox;
    }
}