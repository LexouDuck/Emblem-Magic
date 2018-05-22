namespace EmblemMagic.Editors
{
    partial class WriteEditor
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WriteEditor));
            this.WriteApplyButton = new System.Windows.Forms.Button();
            this.WriteDeleteButton = new System.Windows.Forms.Button();
            this.WriteHistoryList = new System.Windows.Forms.DataGridView();
            this.List_TimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.List_AddressColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.List_LengthColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.List_AuthorColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.List_PhraseColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.List_IsSavedColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.List_PatchedColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WriteBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.WriteDataHexBox = new EmblemMagic.Components.HexBox();
            this.EditPanel = new System.Windows.Forms.GroupBox();
            this.WriteEditorLabel = new System.Windows.Forms.Label();
            this.WriteLength = new System.Windows.Forms.Label();
            this.WriteLengthLabel = new System.Windows.Forms.Label();
            this.WriteMergeButton = new System.Windows.Forms.Button();
            this.WritePhraseTextBox = new System.Windows.Forms.TextBox();
            this.MarkSpaceBox = new EmblemMagic.Components.MarkingBox();
            this.WritePointerBox = new EmblemMagic.Components.PointerBox();
            this.MarkSpaceButton = new System.Windows.Forms.Button();
            this.AddressLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.WriteHistoryList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WriteBindingSource)).BeginInit();
            this.EditPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WritePointerBox)).BeginInit();
            this.SuspendLayout();
            // 
            // WriteApplyButton
            // 
            this.WriteApplyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.WriteApplyButton.Location = new System.Drawing.Point(534, 131);
            this.WriteApplyButton.Name = "WriteApplyButton";
            this.WriteApplyButton.Size = new System.Drawing.Size(96, 34);
            this.WriteApplyButton.TabIndex = 1;
            this.WriteApplyButton.Text = "Apply Changes";
            this.WriteApplyButton.UseVisualStyleBackColor = true;
            this.WriteApplyButton.Click += new System.EventHandler(this.ApplyWrite_Click);
            // 
            // WriteDeleteButton
            // 
            this.WriteDeleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.WriteDeleteButton.Location = new System.Drawing.Point(432, 131);
            this.WriteDeleteButton.Name = "WriteDeleteButton";
            this.WriteDeleteButton.Size = new System.Drawing.Size(96, 34);
            this.WriteDeleteButton.TabIndex = 2;
            this.WriteDeleteButton.Text = "Delete Write(s)";
            this.WriteDeleteButton.UseVisualStyleBackColor = true;
            this.WriteDeleteButton.Click += new System.EventHandler(this.DeleteWrite_Click);
            // 
            // WriteHistoryList
            // 
            this.WriteHistoryList.AllowUserToAddRows = false;
            this.WriteHistoryList.AllowUserToDeleteRows = false;
            this.WriteHistoryList.AllowUserToOrderColumns = true;
            this.WriteHistoryList.AllowUserToResizeRows = false;
            this.WriteHistoryList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WriteHistoryList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.WriteHistoryList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.WriteHistoryList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.List_TimeColumn,
            this.List_AddressColumn,
            this.List_LengthColumn,
            this.List_AuthorColumn,
            this.List_PhraseColumn,
            this.List_IsSavedColumn,
            this.List_PatchedColumn});
            this.WriteHistoryList.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.WriteHistoryList.Location = new System.Drawing.Point(12, 12);
            this.WriteHistoryList.Name = "WriteHistoryList";
            this.WriteHistoryList.ReadOnly = true;
            this.WriteHistoryList.RowHeadersWidth = 24;
            this.WriteHistoryList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.WriteHistoryList.Size = new System.Drawing.Size(639, 353);
            this.WriteHistoryList.TabIndex = 3;
            this.WriteHistoryList.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.SelectWrite_Click);
            this.WriteHistoryList.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.SelectWrite_Click);
            // 
            // List_TimeColumn
            // 
            this.List_TimeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.List_TimeColumn.HeaderText = "Time of write";
            this.List_TimeColumn.MinimumWidth = 100;
            this.List_TimeColumn.Name = "List_TimeColumn";
            this.List_TimeColumn.ReadOnly = true;
            // 
            // List_AddressColumn
            // 
            this.List_AddressColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.List_AddressColumn.FillWeight = 70F;
            this.List_AddressColumn.HeaderText = "Address";
            this.List_AddressColumn.MinimumWidth = 70;
            this.List_AddressColumn.Name = "List_AddressColumn";
            this.List_AddressColumn.ReadOnly = true;
            this.List_AddressColumn.Width = 70;
            // 
            // List_LengthColumn
            // 
            this.List_LengthColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.List_LengthColumn.FillWeight = 20F;
            this.List_LengthColumn.HeaderText = "Length (bytes)";
            this.List_LengthColumn.MinimumWidth = 50;
            this.List_LengthColumn.Name = "List_LengthColumn";
            this.List_LengthColumn.ReadOnly = true;
            this.List_LengthColumn.Width = 50;
            // 
            // List_AuthorColumn
            // 
            this.List_AuthorColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.List_AuthorColumn.FillWeight = 80F;
            this.List_AuthorColumn.HeaderText = "Editor used";
            this.List_AuthorColumn.MinimumWidth = 100;
            this.List_AuthorColumn.Name = "List_AuthorColumn";
            this.List_AuthorColumn.ReadOnly = true;
            // 
            // List_PhraseColumn
            // 
            this.List_PhraseColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.List_PhraseColumn.HeaderText = "Description";
            this.List_PhraseColumn.MinimumWidth = 150;
            this.List_PhraseColumn.Name = "List_PhraseColumn";
            this.List_PhraseColumn.ReadOnly = true;
            // 
            // List_IsSavedColumn
            // 
            this.List_IsSavedColumn.FillWeight = 50F;
            this.List_IsSavedColumn.HeaderText = "Saved to FEH";
            this.List_IsSavedColumn.MinimumWidth = 50;
            this.List_IsSavedColumn.Name = "List_IsSavedColumn";
            this.List_IsSavedColumn.ReadOnly = true;
            this.List_IsSavedColumn.Width = 50;
            // 
            // List_PatchedColumn
            // 
            this.List_PatchedColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.List_PatchedColumn.FillWeight = 50F;
            this.List_PatchedColumn.HeaderText = "Patched to ROM";
            this.List_PatchedColumn.MinimumWidth = 50;
            this.List_PatchedColumn.Name = "List_PatchedColumn";
            this.List_PatchedColumn.ReadOnly = true;
            this.List_PatchedColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.List_PatchedColumn.Width = 50;
            // 
            // WriteBindingSource
            // 
            this.WriteBindingSource.DataSource = typeof(EmblemMagic.Editors.WriteEditor);
            // 
            // WriteDataHexBox
            // 
            this.WriteDataHexBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.WriteDataHexBox.Font = new System.Drawing.Font("Consolas", 8F);
            this.WriteDataHexBox.InfoForeColor = System.Drawing.Color.Empty;
            this.WriteDataHexBox.Location = new System.Drawing.Point(6, 19);
            this.WriteDataHexBox.Name = "WriteDataHexBox";
            this.WriteDataHexBox.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.WriteDataHexBox.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.WriteDataHexBox.Size = new System.Drawing.Size(315, 146);
            this.WriteDataHexBox.TabIndex = 8;
            this.WriteDataHexBox.UseFixedBytesPerLine = true;
            this.WriteDataHexBox.VScrollBarVisible = true;
            // 
            // EditPanel
            // 
            this.EditPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EditPanel.Controls.Add(this.WriteEditorLabel);
            this.EditPanel.Controls.Add(this.WriteLength);
            this.EditPanel.Controls.Add(this.WriteLengthLabel);
            this.EditPanel.Controls.Add(this.WriteMergeButton);
            this.EditPanel.Controls.Add(this.WritePhraseTextBox);
            this.EditPanel.Controls.Add(this.MarkSpaceBox);
            this.EditPanel.Controls.Add(this.WritePointerBox);
            this.EditPanel.Controls.Add(this.MarkSpaceButton);
            this.EditPanel.Controls.Add(this.AddressLabel);
            this.EditPanel.Controls.Add(this.WriteDataHexBox);
            this.EditPanel.Controls.Add(this.WriteApplyButton);
            this.EditPanel.Controls.Add(this.WriteDeleteButton);
            this.EditPanel.Location = new System.Drawing.Point(12, 371);
            this.EditPanel.Name = "EditPanel";
            this.EditPanel.Size = new System.Drawing.Size(640, 175);
            this.EditPanel.TabIndex = 9;
            this.EditPanel.TabStop = false;
            this.EditPanel.Text = "Write Editing Panel";
            // 
            // WriteEditorLabel
            // 
            this.WriteEditorLabel.AutoSize = true;
            this.WriteEditorLabel.Location = new System.Drawing.Point(327, 22);
            this.WriteEditorLabel.Name = "WriteEditorLabel";
            this.WriteEditorLabel.Size = new System.Drawing.Size(0, 13);
            this.WriteEditorLabel.TabIndex = 19;
            // 
            // WriteLength
            // 
            this.WriteLength.AutoSize = true;
            this.WriteLength.Location = new System.Drawing.Point(548, 22);
            this.WriteLength.Name = "WriteLength";
            this.WriteLength.Size = new System.Drawing.Size(10, 13);
            this.WriteLength.TabIndex = 18;
            this.WriteLength.Text = "-";
            // 
            // WriteLengthLabel
            // 
            this.WriteLengthLabel.AutoSize = true;
            this.WriteLengthLabel.Location = new System.Drawing.Point(496, 22);
            this.WriteLengthLabel.Name = "WriteLengthLabel";
            this.WriteLengthLabel.Size = new System.Drawing.Size(46, 13);
            this.WriteLengthLabel.TabIndex = 17;
            this.WriteLengthLabel.Text = "Length :";
            // 
            // WriteMergeButton
            // 
            this.WriteMergeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.WriteMergeButton.Location = new System.Drawing.Point(330, 131);
            this.WriteMergeButton.Name = "WriteMergeButton";
            this.WriteMergeButton.Size = new System.Drawing.Size(96, 34);
            this.WriteMergeButton.TabIndex = 15;
            this.WriteMergeButton.Text = "Merge Writes";
            this.WriteMergeButton.UseVisualStyleBackColor = true;
            this.WriteMergeButton.Click += new System.EventHandler(this.MergeWrites_Click);
            // 
            // WritePhraseTextBox
            // 
            this.WritePhraseTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WritePhraseTextBox.Location = new System.Drawing.Point(330, 73);
            this.WritePhraseTextBox.MaxLength = 64;
            this.WritePhraseTextBox.Multiline = true;
            this.WritePhraseTextBox.Name = "WritePhraseTextBox";
            this.WritePhraseTextBox.Size = new System.Drawing.Size(301, 52);
            this.WritePhraseTextBox.TabIndex = 14;
            // 
            // MarkSpaceBox
            // 
            this.MarkSpaceBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.MarkSpaceBox.DataSource = ((object)(resources.GetObject("MarkSpaceBox.DataSource")));
            this.MarkSpaceBox.Location = new System.Drawing.Point(476, 46);
            this.MarkSpaceBox.MaxLength = 4;
            this.MarkSpaceBox.Name = "MarkSpaceBox";
            this.MarkSpaceBox.Size = new System.Drawing.Size(66, 21);
            this.MarkSpaceBox.TabIndex = 13;
            // 
            // WritePointerBox
            // 
            this.WritePointerBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.WritePointerBox.Hexadecimal = true;
            this.WritePointerBox.Location = new System.Drawing.Point(386, 47);
            this.WritePointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.WritePointerBox.Name = "WritePointerBox";
            this.WritePointerBox.Size = new System.Drawing.Size(71, 20);
            this.WritePointerBox.TabIndex = 11;
            // 
            // MarkSpaceButton
            // 
            this.MarkSpaceButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.MarkSpaceButton.Location = new System.Drawing.Point(548, 41);
            this.MarkSpaceButton.Name = "MarkSpaceButton";
            this.MarkSpaceButton.Size = new System.Drawing.Size(83, 29);
            this.MarkSpaceButton.TabIndex = 12;
            this.MarkSpaceButton.Text = "Mark Space";
            this.MarkSpaceButton.UseVisualStyleBackColor = true;
            this.MarkSpaceButton.Click += new System.EventHandler(this.MarkSpaceButton_Click);
            // 
            // AddressLabel
            // 
            this.AddressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddressLabel.Location = new System.Drawing.Point(327, 45);
            this.AddressLabel.Name = "AddressLabel";
            this.AddressLabel.Size = new System.Drawing.Size(53, 20);
            this.AddressLabel.TabIndex = 10;
            this.AddressLabel.Text = "Address :";
            this.AddressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // WriteEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 558);
            this.Controls.Add(this.WriteHistoryList);
            this.Controls.Add(this.EditPanel);
            this.MinimumSize = new System.Drawing.Size(680, 200);
            this.Name = "WriteEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Write History Manager";
            ((System.ComponentModel.ISupportInitialize)(this.WriteHistoryList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WriteBindingSource)).EndInit();
            this.EditPanel.ResumeLayout(false);
            this.EditPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WritePointerBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView WriteHistoryList;
        private System.Windows.Forms.BindingSource WriteBindingSource;

        private System.Windows.Forms.GroupBox EditPanel;
        private Components.MarkingBox MarkSpaceBox;
        private Components.HexBox WriteDataHexBox;
        private Components.PointerBox WritePointerBox;
        private System.Windows.Forms.TextBox WritePhraseTextBox;
        private System.Windows.Forms.Label AddressLabel;
        private System.Windows.Forms.Button MarkSpaceButton;
        private System.Windows.Forms.Button WriteApplyButton;
        private System.Windows.Forms.Button WriteDeleteButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn List_TimeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn List_AddressColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn List_LengthColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn List_AuthorColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn List_PhraseColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn List_IsSavedColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn List_PatchedColumn;
        private System.Windows.Forms.Button WriteMergeButton;
        private System.Windows.Forms.Label WriteLength;
        private System.Windows.Forms.Label WriteLengthLabel;
        private System.Windows.Forms.Label WriteEditorLabel;
    }
}