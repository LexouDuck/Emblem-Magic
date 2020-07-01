namespace EmblemMagic.Editors
{
    partial class SpaceEditor
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


        private System.Windows.Forms.GroupBox Space_Panel;
        private EmblemMagic.Components.PointerBox Space_AddressBox;
        private EmblemMagic.Components.PointerBox Space_EndByteBox;
        private System.Windows.Forms.NumericUpDown Space_LengthBox;
        private System.Windows.Forms.Label Space_MarkAsLabel;
        private System.Windows.Forms.Label Space_AddressLabel;
        private System.Windows.Forms.RadioButton Space_EndByteLabel;
        private System.Windows.Forms.RadioButton Space_LengthLabel;
        private EmblemMagic.Components.MarkingBox Space_MarkAsComboBox;
        private System.Windows.Forms.Button Space_OKButton;

        private System.Windows.Forms.GroupBox Marks_Panel;
        private System.Windows.Forms.ListBox Marks_ListBox;
        private Components.ColorBox Marks_ColorBox;
        private System.Windows.Forms.TextBox Marks_NameTextBox;
        private System.Windows.Forms.Button Marks_ColorButton;
        private System.Windows.Forms.NumericUpDown Marks_LayerNumBox;
        private System.Windows.Forms.Label Marks_NameLabel;
        private System.Windows.Forms.Label Marks_LayerLabel;
        private System.Windows.Forms.CheckBox Marks_PromptCheckBox;
        private System.Windows.Forms.Button Marks_DeleteMarkButton;
        private System.Windows.Forms.Button Marks_CreateMarkButton;

        private System.Windows.Forms.TextBox Output_TextBox;
        private System.Windows.Forms.Button Output_SortByAddressButton;
        private System.Windows.Forms.Button Output_SortByLengthButton;
        private System.Windows.Forms.Button Output_SortByMarkButton;
        private Components.MarkedSpaceBar Output_SpaceBar;
        private System.Windows.Forms.StatusStrip Output_StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel Output_SpaceAddressStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel Output_SpaceEndByteStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel Output_SpaceLengthStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel Output_SpaceMarkedStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel Output_CurrentAddressStatusLabel;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpaceEditor));
            this.Output_SpaceBar = new EmblemMagic.Components.MarkedSpaceBar();
            this.Output_TextBox = new System.Windows.Forms.TextBox();
            this.Space_OKButton = new System.Windows.Forms.Button();
            this.Space_AddressLabel = new System.Windows.Forms.Label();
            this.Space_EndByteLabel = new System.Windows.Forms.RadioButton();
            this.Space_LengthLabel = new System.Windows.Forms.RadioButton();
            this.Space_AddressBox = new EmblemMagic.Components.PointerBox();
            this.Space_EndByteBox = new EmblemMagic.Components.PointerBox();
            this.Space_LengthBox = new System.Windows.Forms.NumericUpDown();
            this.Space_Panel = new System.Windows.Forms.GroupBox();
            this.Space_MarkAsLabel = new System.Windows.Forms.Label();
            this.Space_MarkAsComboBox = new EmblemMagic.Components.MarkingBox();
            this.Marks_Panel = new System.Windows.Forms.GroupBox();
            this.Marks_DeleteMarkButton = new System.Windows.Forms.Button();
            this.Marks_CreateMarkButton = new System.Windows.Forms.Button();
            this.Marks_LayerLabel = new System.Windows.Forms.Label();
            this.Marks_LayerNumBox = new System.Windows.Forms.NumericUpDown();
            this.Marks_ColorBox = new EmblemMagic.Components.ColorBox();
            this.Marks_PromptCheckBox = new System.Windows.Forms.CheckBox();
            this.Marks_NameTextBox = new System.Windows.Forms.TextBox();
            this.Marks_NameLabel = new System.Windows.Forms.Label();
            this.Marks_ColorButton = new System.Windows.Forms.Button();
            this.Marks_ListBox = new System.Windows.Forms.ListBox();
            this.Output_StatusStrip = new System.Windows.Forms.StatusStrip();
            this.Output_CurrentAddressStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Output_SpaceAddressStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Output_SpaceEndByteStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Output_SpaceLengthStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Output_SpaceMarkedStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Output_SortByAddressButton = new System.Windows.Forms.Button();
            this.Output_SortByLengthButton = new System.Windows.Forms.Button();
            this.Output_SortByMarkButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Space_AddressBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Space_EndByteBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Space_LengthBox)).BeginInit();
            this.Space_Panel.SuspendLayout();
            this.Marks_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Marks_LayerNumBox)).BeginInit();
            this.Output_StatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // Output_SpaceBar
            // 
            this.Output_SpaceBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Output_SpaceBar.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.Output_SpaceBar.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.Output_SpaceBar.Location = new System.Drawing.Point(12, 263);
            this.Output_SpaceBar.Name = "Output_SpaceBar";
            this.Output_SpaceBar.Size = new System.Drawing.Size(690, 44);
            this.Output_SpaceBar.TabIndex = 15;
            // 
            // Output_TextBox
            // 
            this.Output_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Output_TextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Output_TextBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Output_TextBox.Location = new System.Drawing.Point(369, 12);
            this.Output_TextBox.Multiline = true;
            this.Output_TextBox.Name = "Output_TextBox";
            this.Output_TextBox.ReadOnly = true;
            this.Output_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Output_TextBox.Size = new System.Drawing.Size(335, 211);
            this.Output_TextBox.TabIndex = 9;
            // 
            // Space_OKButton
            // 
            this.Space_OKButton.Location = new System.Drawing.Point(213, 45);
            this.Space_OKButton.Name = "Space_OKButton";
            this.Space_OKButton.Size = new System.Drawing.Size(129, 46);
            this.Space_OKButton.TabIndex = 5;
            this.Space_OKButton.Text = "Mark ROM Space";
            this.Space_OKButton.UseVisualStyleBackColor = true;
            this.Space_OKButton.Click += new System.EventHandler(this.Space_OKButton_Click);
            // 
            // Space_AddressLabel
            // 
            this.Space_AddressLabel.AutoSize = true;
            this.Space_AddressLabel.Location = new System.Drawing.Point(25, 21);
            this.Space_AddressLabel.Name = "Space_AddressLabel";
            this.Space_AddressLabel.Size = new System.Drawing.Size(73, 13);
            this.Space_AddressLabel.TabIndex = 1;
            this.Space_AddressLabel.Text = "Start Address:";
            // 
            // Space_EndByteLabel
            // 
            this.Space_EndByteLabel.AutoSize = true;
            this.Space_EndByteLabel.Checked = true;
            this.Space_EndByteLabel.Location = new System.Drawing.Point(10, 45);
            this.Space_EndByteLabel.Name = "Space_EndByteLabel";
            this.Space_EndByteLabel.Size = new System.Drawing.Size(88, 17);
            this.Space_EndByteLabel.TabIndex = 2;
            this.Space_EndByteLabel.TabStop = true;
            this.Space_EndByteLabel.Text = "End Address:";
            this.Space_EndByteLabel.UseVisualStyleBackColor = true;
            this.Space_EndByteLabel.Click += new System.EventHandler(this.Space_EndOffLabel_Click);
            // 
            // Space_LengthLabel
            // 
            this.Space_LengthLabel.AutoSize = true;
            this.Space_LengthLabel.Location = new System.Drawing.Point(10, 74);
            this.Space_LengthLabel.Name = "Space_LengthLabel";
            this.Space_LengthLabel.Size = new System.Drawing.Size(61, 17);
            this.Space_LengthLabel.TabIndex = 3;
            this.Space_LengthLabel.Text = "Length:";
            this.Space_LengthLabel.UseVisualStyleBackColor = true;
            this.Space_LengthLabel.Click += new System.EventHandler(this.Space_LengthLabel_Click);
            // 
            // Space_AddressBox
            // 
            this.Space_AddressBox.Hexadecimal = true;
            this.Space_AddressBox.Location = new System.Drawing.Point(111, 18);
            this.Space_AddressBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Space_AddressBox.Name = "Space_AddressBox";
            this.Space_AddressBox.Size = new System.Drawing.Size(83, 20);
            this.Space_AddressBox.TabIndex = 1;
            // 
            // Space_EndByteBox
            // 
            this.Space_EndByteBox.Hexadecimal = true;
            this.Space_EndByteBox.Location = new System.Drawing.Point(111, 45);
            this.Space_EndByteBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Space_EndByteBox.Name = "Space_EndByteBox";
            this.Space_EndByteBox.Size = new System.Drawing.Size(83, 20);
            this.Space_EndByteBox.TabIndex = 2;
            // 
            // Space_LengthBox
            // 
            this.Space_LengthBox.Enabled = false;
            this.Space_LengthBox.Hexadecimal = true;
            this.Space_LengthBox.Location = new System.Drawing.Point(111, 73);
            this.Space_LengthBox.Name = "Space_LengthBox";
            this.Space_LengthBox.Size = new System.Drawing.Size(83, 20);
            this.Space_LengthBox.TabIndex = 3;
            // 
            // Space_Panel
            // 
            this.Space_Panel.Controls.Add(this.Space_MarkAsLabel);
            this.Space_Panel.Controls.Add(this.Space_MarkAsComboBox);
            this.Space_Panel.Controls.Add(this.Space_EndByteLabel);
            this.Space_Panel.Controls.Add(this.Space_LengthLabel);
            this.Space_Panel.Controls.Add(this.Space_EndByteBox);
            this.Space_Panel.Controls.Add(this.Space_OKButton);
            this.Space_Panel.Controls.Add(this.Space_AddressLabel);
            this.Space_Panel.Controls.Add(this.Space_AddressBox);
            this.Space_Panel.Controls.Add(this.Space_LengthBox);
            this.Space_Panel.Location = new System.Drawing.Point(12, 4);
            this.Space_Panel.Name = "Space_Panel";
            this.Space_Panel.Size = new System.Drawing.Size(351, 99);
            this.Space_Panel.TabIndex = 0;
            this.Space_Panel.TabStop = false;
            this.Space_Panel.Text = "Edit Space Panel";
            // 
            // Space_MarkAsLabel
            // 
            this.Space_MarkAsLabel.AutoSize = true;
            this.Space_MarkAsLabel.Location = new System.Drawing.Point(210, 22);
            this.Space_MarkAsLabel.Name = "Space_MarkAsLabel";
            this.Space_MarkAsLabel.Size = new System.Drawing.Size(48, 13);
            this.Space_MarkAsLabel.TabIndex = 4;
            this.Space_MarkAsLabel.Text = "Mark as:";
            // 
            // Space_MarkAsComboBox
            // 
            this.Space_MarkAsComboBox.DataSource = ((object)(resources.GetObject("Space_MarkAsComboBox.DataSource")));
            this.Space_MarkAsComboBox.FormattingEnabled = true;
            this.Space_MarkAsComboBox.Location = new System.Drawing.Point(264, 18);
            this.Space_MarkAsComboBox.Name = "Space_MarkAsComboBox";
            this.Space_MarkAsComboBox.Size = new System.Drawing.Size(78, 21);
            this.Space_MarkAsComboBox.TabIndex = 4;
            // 
            // Marks_Panel
            // 
            this.Marks_Panel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Marks_Panel.Controls.Add(this.Marks_DeleteMarkButton);
            this.Marks_Panel.Controls.Add(this.Marks_CreateMarkButton);
            this.Marks_Panel.Controls.Add(this.Marks_LayerLabel);
            this.Marks_Panel.Controls.Add(this.Marks_LayerNumBox);
            this.Marks_Panel.Controls.Add(this.Marks_ColorBox);
            this.Marks_Panel.Controls.Add(this.Marks_PromptCheckBox);
            this.Marks_Panel.Controls.Add(this.Marks_NameTextBox);
            this.Marks_Panel.Controls.Add(this.Marks_NameLabel);
            this.Marks_Panel.Controls.Add(this.Marks_ColorButton);
            this.Marks_Panel.Controls.Add(this.Marks_ListBox);
            this.Marks_Panel.Location = new System.Drawing.Point(12, 109);
            this.Marks_Panel.Name = "Marks_Panel";
            this.Marks_Panel.Size = new System.Drawing.Size(351, 148);
            this.Marks_Panel.TabIndex = 0;
            this.Marks_Panel.TabStop = false;
            this.Marks_Panel.Text = "Marking types Panel";
            // 
            // Marks_DeleteMarkButton
            // 
            this.Marks_DeleteMarkButton.Location = new System.Drawing.Point(280, 83);
            this.Marks_DeleteMarkButton.Name = "Marks_DeleteMarkButton";
            this.Marks_DeleteMarkButton.Size = new System.Drawing.Size(62, 56);
            this.Marks_DeleteMarkButton.TabIndex = 18;
            this.Marks_DeleteMarkButton.Text = "Delete Selected";
            this.Marks_DeleteMarkButton.UseVisualStyleBackColor = true;
            this.Marks_DeleteMarkButton.Click += new System.EventHandler(this.Marks_DeleteMarkButton_Click);
            // 
            // Marks_CreateMarkButton
            // 
            this.Marks_CreateMarkButton.Location = new System.Drawing.Point(280, 18);
            this.Marks_CreateMarkButton.Name = "Marks_CreateMarkButton";
            this.Marks_CreateMarkButton.Size = new System.Drawing.Size(62, 56);
            this.Marks_CreateMarkButton.TabIndex = 17;
            this.Marks_CreateMarkButton.Text = "Create Marking Type";
            this.Marks_CreateMarkButton.UseVisualStyleBackColor = true;
            this.Marks_CreateMarkButton.Click += new System.EventHandler(this.Marks_CreateMarkButton_Click);
            // 
            // Marks_LayerLabel
            // 
            this.Marks_LayerLabel.AutoSize = true;
            this.Marks_LayerLabel.Location = new System.Drawing.Point(14, 93);
            this.Marks_LayerLabel.Name = "Marks_LayerLabel";
            this.Marks_LayerLabel.Size = new System.Drawing.Size(80, 13);
            this.Marks_LayerLabel.TabIndex = 16;
            this.Marks_LayerLabel.Text = "Marking Layer :";
            // 
            // Marks_LayerNumBox
            // 
            this.Marks_LayerNumBox.Location = new System.Drawing.Point(111, 91);
            this.Marks_LayerNumBox.Name = "Marks_LayerNumBox";
            this.Marks_LayerNumBox.Size = new System.Drawing.Size(64, 20);
            this.Marks_LayerNumBox.TabIndex = 15;
            this.Marks_LayerNumBox.Leave += new System.EventHandler(this.Marks_LayerValueChanged);
            // 
            // Marks_ColorBox
            // 
            this.Marks_ColorBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.Marks_ColorBox.Color = System.Drawing.Color.Empty;
            this.Marks_ColorBox.ForeColor = System.Drawing.SystemColors.Window;
            this.Marks_ColorBox.Gradient = true;
            this.Marks_ColorBox.Location = new System.Drawing.Point(17, 53);
            this.Marks_ColorBox.Name = "Marks_ColorBox";
            this.Marks_ColorBox.Selected = false;
            this.Marks_ColorBox.Size = new System.Drawing.Size(44, 29);
            this.Marks_ColorBox.TabIndex = 11;
            this.Marks_ColorBox.TabStop = false;
            // 
            // Marks_PromptCheckBox
            // 
            this.Marks_PromptCheckBox.AutoSize = true;
            this.Marks_PromptCheckBox.Location = new System.Drawing.Point(17, 122);
            this.Marks_PromptCheckBox.Name = "Marks_PromptCheckBox";
            this.Marks_PromptCheckBox.Size = new System.Drawing.Size(130, 17);
            this.Marks_PromptCheckBox.TabIndex = 13;
            this.Marks_PromptCheckBox.Text = "Prompt when writing ?";
            this.Marks_PromptCheckBox.UseVisualStyleBackColor = true;
            // 
            // Marks_NameTextBox
            // 
            this.Marks_NameTextBox.Location = new System.Drawing.Point(111, 23);
            this.Marks_NameTextBox.MaxLength = 4;
            this.Marks_NameTextBox.Name = "Marks_NameTextBox";
            this.Marks_NameTextBox.Size = new System.Drawing.Size(64, 20);
            this.Marks_NameTextBox.TabIndex = 10;
            this.Marks_NameTextBox.Leave += new System.EventHandler(this.Marks_NameTextChanged);
            // 
            // Marks_NameLabel
            // 
            this.Marks_NameLabel.AutoSize = true;
            this.Marks_NameLabel.Location = new System.Drawing.Point(14, 26);
            this.Marks_NameLabel.Name = "Marks_NameLabel";
            this.Marks_NameLabel.Size = new System.Drawing.Size(91, 13);
            this.Marks_NameLabel.TabIndex = 10;
            this.Marks_NameLabel.Text = "Marking Identifier:";
            // 
            // Marks_ColorButton
            // 
            this.Marks_ColorButton.Location = new System.Drawing.Point(67, 53);
            this.Marks_ColorButton.Name = "Marks_ColorButton";
            this.Marks_ColorButton.Size = new System.Drawing.Size(108, 29);
            this.Marks_ColorButton.TabIndex = 11;
            this.Marks_ColorButton.Text = "Select Mark Color...";
            this.Marks_ColorButton.UseVisualStyleBackColor = true;
            this.Marks_ColorButton.Click += new System.EventHandler(this.Marks_ColorButton_Click);
            // 
            // Marks_ListBox
            // 
            this.Marks_ListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Marks_ListBox.FormattingEnabled = true;
            this.Marks_ListBox.Location = new System.Drawing.Point(181, 18);
            this.Marks_ListBox.Name = "Marks_ListBox";
            this.Marks_ListBox.Size = new System.Drawing.Size(92, 121);
            this.Marks_ListBox.TabIndex = 14;
            this.Marks_ListBox.SelectedIndexChanged += new System.EventHandler(this.Marks_ListBox_Click);
            // 
            // Output_StatusStrip
            // 
            this.Output_StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Output_CurrentAddressStatusLabel,
            this.Output_SpaceAddressStatusLabel,
            this.Output_SpaceEndByteStatusLabel,
            this.Output_SpaceLengthStatusLabel,
            this.Output_SpaceMarkedStatusLabel});
            this.Output_StatusStrip.Location = new System.Drawing.Point(0, 310);
            this.Output_StatusStrip.Name = "Output_StatusStrip";
            this.Output_StatusStrip.Size = new System.Drawing.Size(714, 22);
            this.Output_StatusStrip.TabIndex = 16;
            this.Output_StatusStrip.Text = "statusStrip1";
            // 
            // Output_CurrentAddressStatusLabel
            // 
            this.Output_CurrentAddressStatusLabel.AutoSize = false;
            this.Output_CurrentAddressStatusLabel.Name = "Output_CurrentAddressStatusLabel";
            this.Output_CurrentAddressStatusLabel.Size = new System.Drawing.Size(100, 17);
            // 
            // Output_SpaceAddressStatusLabel
            // 
            this.Output_SpaceAddressStatusLabel.AutoSize = false;
            this.Output_SpaceAddressStatusLabel.Name = "Output_SpaceAddressStatusLabel";
            this.Output_SpaceAddressStatusLabel.Size = new System.Drawing.Size(170, 17);
            this.Output_SpaceAddressStatusLabel.Text = "Start Address: ";
            this.Output_SpaceAddressStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Output_SpaceEndByteStatusLabel
            // 
            this.Output_SpaceEndByteStatusLabel.AutoSize = false;
            this.Output_SpaceEndByteStatusLabel.Name = "Output_SpaceEndByteStatusLabel";
            this.Output_SpaceEndByteStatusLabel.Size = new System.Drawing.Size(150, 17);
            this.Output_SpaceEndByteStatusLabel.Text = "End Address: ";
            this.Output_SpaceEndByteStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Output_SpaceLengthStatusLabel
            // 
            this.Output_SpaceLengthStatusLabel.AutoSize = false;
            this.Output_SpaceLengthStatusLabel.Name = "Output_SpaceLengthStatusLabel";
            this.Output_SpaceLengthStatusLabel.Size = new System.Drawing.Size(150, 17);
            this.Output_SpaceLengthStatusLabel.Text = "Length:";
            this.Output_SpaceLengthStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Output_SpaceMarkedStatusLabel
            // 
            this.Output_SpaceMarkedStatusLabel.AutoSize = false;
            this.Output_SpaceMarkedStatusLabel.Name = "Output_SpaceMarkedStatusLabel";
            this.Output_SpaceMarkedStatusLabel.Size = new System.Drawing.Size(120, 17);
            this.Output_SpaceMarkedStatusLabel.Text = "Marked: ";
            this.Output_SpaceMarkedStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Output_SortByAddressButton
            // 
            this.Output_SortByAddressButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Output_SortByAddressButton.Location = new System.Drawing.Point(369, 229);
            this.Output_SortByAddressButton.Name = "Output_SortByAddressButton";
            this.Output_SortByAddressButton.Size = new System.Drawing.Size(106, 30);
            this.Output_SortByAddressButton.TabIndex = 6;
            this.Output_SortByAddressButton.Text = "Sort by Address";
            this.Output_SortByAddressButton.UseVisualStyleBackColor = true;
            this.Output_SortByAddressButton.Click += new System.EventHandler(this.Output_SortOffsetButton_Click);
            // 
            // Output_SortByLengthButton
            // 
            this.Output_SortByLengthButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Output_SortByLengthButton.Location = new System.Drawing.Point(482, 229);
            this.Output_SortByLengthButton.Name = "Output_SortByLengthButton";
            this.Output_SortByLengthButton.Size = new System.Drawing.Size(106, 30);
            this.Output_SortByLengthButton.TabIndex = 7;
            this.Output_SortByLengthButton.Text = "Sort by Length";
            this.Output_SortByLengthButton.UseVisualStyleBackColor = true;
            this.Output_SortByLengthButton.Click += new System.EventHandler(this.Output_SortLengthButton_Click);
            // 
            // Output_SortByMarkButton
            // 
            this.Output_SortByMarkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Output_SortByMarkButton.Location = new System.Drawing.Point(596, 229);
            this.Output_SortByMarkButton.Name = "Output_SortByMarkButton";
            this.Output_SortByMarkButton.Size = new System.Drawing.Size(106, 30);
            this.Output_SortByMarkButton.TabIndex = 8;
            this.Output_SortByMarkButton.Text = "Sort by Marking";
            this.Output_SortByMarkButton.UseVisualStyleBackColor = true;
            this.Output_SortByMarkButton.Click += new System.EventHandler(this.Output_SortByNameButton_Click);
            // 
            // SpaceEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 332);
            this.Controls.Add(this.Output_SortByMarkButton);
            this.Controls.Add(this.Output_SortByLengthButton);
            this.Controls.Add(this.Output_SortByAddressButton);
            this.Controls.Add(this.Output_TextBox);
            this.Controls.Add(this.Output_StatusStrip);
            this.Controls.Add(this.Marks_Panel);
            this.Controls.Add(this.Space_Panel);
            this.Controls.Add(this.Output_SpaceBar);
            this.MinimumSize = new System.Drawing.Size(730, 370);
            this.Name = "SpaceEditor";
            this.Text = "ROM Space Marking Manager";
            ((System.ComponentModel.ISupportInitialize)(this.Space_AddressBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Space_EndByteBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Space_LengthBox)).EndInit();
            this.Space_Panel.ResumeLayout(false);
            this.Space_Panel.PerformLayout();
            this.Marks_Panel.ResumeLayout(false);
            this.Marks_Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Marks_LayerNumBox)).EndInit();
            this.Output_StatusStrip.ResumeLayout(false);
            this.Output_StatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}