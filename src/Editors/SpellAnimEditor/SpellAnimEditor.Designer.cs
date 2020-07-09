namespace EmblemMagic.Editors
{
    partial class SpellAnimEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpellAnimEditor));
            this.EntryArrayBox = new EmblemMagic.Components.ByteArrayBox();
            this.Constructor_PointerBox = new EmblemMagic.Components.PointerBox();
            this.Palette_PointerBox = new EmblemMagic.Components.PointerBox();
            this.Tileset_PointerBox = new EmblemMagic.Components.PointerBox();
            this.TSA_PointerBox = new EmblemMagic.Components.PointerBox();
            this.Palette_Label = new System.Windows.Forms.Label();
            this.Tileset_Label = new System.Windows.Forms.Label();
            this.TSA_Label = new System.Windows.Forms.CheckBox();
            this.Spell_ImageBox = new EmblemMagic.Components.ImageBox();
            this.Refresh_Button = new System.Windows.Forms.Button();
            this.Palette_CheckBox = new System.Windows.Forms.CheckBox();
            this.Tileset_CheckBox = new System.Windows.Forms.CheckBox();
            this.TSA_CheckBox = new System.Windows.Forms.CheckBox();
            this.Spell_PaletteBox = new EmblemMagic.Components.PaletteBox();
            this.ASM_ListBox = new System.Windows.Forms.ListBox();
            this.Constructor_Label = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.Name_TextBox = new System.Windows.Forms.TextBox();
            this.Looped_CheckBox = new System.Windows.Forms.CheckBox();
            this.Name_Label = new System.Windows.Forms.Label();
            this.LoopRoutine_PointerBox = new EmblemMagic.Components.PointerBox();
            this.LoopRoutine_Label = new System.Windows.Forms.RadioButton();
            this.AnimLoading_PointerBox = new EmblemMagic.Components.PointerBox();
            this.AnimLoading_Label = new System.Windows.Forms.RadioButton();
            this.AnimCodeBox = new EmblemMagic.Components.CodeBox();
            this.MagicButton = new EmblemMagic.Components.MagicButton();
            this.Palette_Prev_Button = new System.Windows.Forms.Button();
            this.Palette_Next_Button = new System.Windows.Forms.Button();
            this.Tileset_Next_Button = new System.Windows.Forms.Button();
            this.Tileset_Prev_Button = new System.Windows.Forms.Button();
            this.TSA_Next_Button = new System.Windows.Forms.Button();
            this.TSA_Prev_Button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Constructor_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Palette_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tileset_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TSA_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoopRoutine_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AnimLoading_PointerBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AnimCodeBox)).BeginInit();
            this.SuspendLayout();
            // 
            // EntryArrayBox
            // 
            this.EntryArrayBox.Location = new System.Drawing.Point(12, 12);
            this.EntryArrayBox.MaximumSize = new System.Drawing.Size(10000, 26);
            this.EntryArrayBox.MinimumSize = new System.Drawing.Size(100, 26);
            this.EntryArrayBox.Size = new System.Drawing.Size(240, 26);
            this.EntryArrayBox.TabIndex = 0;
            // 
            // Constructor_PointerBox
            // 
            this.Constructor_PointerBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Constructor_PointerBox.Hexadecimal = true;
            this.Constructor_PointerBox.Location = new System.Drawing.Point(651, 4);
            this.Constructor_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Constructor_PointerBox.Name = "Constructor_PointerBox";
            this.Constructor_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Constructor_PointerBox.TabIndex = 1;
            // 
            // Palette_PointerBox
            // 
            this.Palette_PointerBox.Hexadecimal = true;
            this.Palette_PointerBox.Location = new System.Drawing.Point(79, 140);
            this.Palette_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Palette_PointerBox.Name = "Palette_PointerBox";
            this.Palette_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Palette_PointerBox.TabIndex = 2;
            // 
            // Tileset_PointerBox
            // 
            this.Tileset_PointerBox.Hexadecimal = true;
            this.Tileset_PointerBox.Location = new System.Drawing.Point(79, 166);
            this.Tileset_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Tileset_PointerBox.Name = "Tileset_PointerBox";
            this.Tileset_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.Tileset_PointerBox.TabIndex = 3;
            // 
            // TSA_PointerBox
            // 
            this.TSA_PointerBox.Hexadecimal = true;
            this.TSA_PointerBox.Location = new System.Drawing.Point(79, 193);
            this.TSA_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.TSA_PointerBox.Name = "TSA_PointerBox";
            this.TSA_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.TSA_PointerBox.TabIndex = 4;
            // 
            // Palette_Label
            // 
            this.Palette_Label.AutoSize = true;
            this.Palette_Label.Location = new System.Drawing.Point(27, 142);
            this.Palette_Label.Name = "Palette_Label";
            this.Palette_Label.Size = new System.Drawing.Size(46, 13);
            this.Palette_Label.TabIndex = 6;
            this.Palette_Label.Text = "Palette :";
            // 
            // Tileset_Label
            // 
            this.Tileset_Label.AutoSize = true;
            this.Tileset_Label.Location = new System.Drawing.Point(29, 169);
            this.Tileset_Label.Name = "Tileset_Label";
            this.Tileset_Label.Size = new System.Drawing.Size(44, 13);
            this.Tileset_Label.TabIndex = 7;
            this.Tileset_Label.Text = "Tileset :";
            // 
            // TSA_Label
            // 
            this.TSA_Label.AutoSize = true;
            this.TSA_Label.Location = new System.Drawing.Point(20, 194);
            this.TSA_Label.Name = "TSA_Label";
            this.TSA_Label.Size = new System.Drawing.Size(53, 17);
            this.TSA_Label.TabIndex = 8;
            this.TSA_Label.Text = "TSA :";
            this.TSA_Label.CheckedChanged += new System.EventHandler(this.TSA_Label_CheckedChanged);
            // 
            // Spell_ImageBox
            // 
            this.Spell_ImageBox.BackColor = System.Drawing.SystemColors.Control;
            this.Spell_ImageBox.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.Spell_ImageBox.Location = new System.Drawing.Point(22, 69);
            this.Spell_ImageBox.Name = "Spell_ImageBox";
            this.Spell_ImageBox.Size = new System.Drawing.Size(256, 64);
            this.Spell_ImageBox.TabIndex = 10;
            this.Spell_ImageBox.TabStop = false;
            this.Spell_ImageBox.Text = "Spell_ImageBox";
            // 
            // Refresh_Button
            // 
            this.Refresh_Button.Location = new System.Drawing.Point(284, 170);
            this.Refresh_Button.Name = "Refresh_Button";
            this.Refresh_Button.Size = new System.Drawing.Size(46, 43);
            this.Refresh_Button.TabIndex = 13;
            this.Refresh_Button.Text = "Load Image";
            this.Refresh_Button.UseVisualStyleBackColor = true;
            this.Refresh_Button.Click += new System.EventHandler(this.Refresh_Button_Click);
            // 
            // Palette_CheckBox
            // 
            this.Palette_CheckBox.AutoSize = true;
            this.Palette_CheckBox.Location = new System.Drawing.Point(155, 141);
            this.Palette_CheckBox.Name = "Palette_CheckBox";
            this.Palette_CheckBox.Size = new System.Drawing.Size(51, 17);
            this.Palette_CheckBox.TabIndex = 14;
            this.Palette_CheckBox.Text = "LZ77";
            this.Palette_CheckBox.UseVisualStyleBackColor = true;
            // 
            // Tileset_CheckBox
            // 
            this.Tileset_CheckBox.AutoSize = true;
            this.Tileset_CheckBox.Location = new System.Drawing.Point(155, 168);
            this.Tileset_CheckBox.Name = "Tileset_CheckBox";
            this.Tileset_CheckBox.Size = new System.Drawing.Size(51, 17);
            this.Tileset_CheckBox.TabIndex = 15;
            this.Tileset_CheckBox.Text = "LZ77";
            this.Tileset_CheckBox.UseVisualStyleBackColor = true;
            // 
            // TSA_CheckBox
            // 
            this.TSA_CheckBox.AutoSize = true;
            this.TSA_CheckBox.Location = new System.Drawing.Point(155, 194);
            this.TSA_CheckBox.Name = "TSA_CheckBox";
            this.TSA_CheckBox.Size = new System.Drawing.Size(51, 17);
            this.TSA_CheckBox.TabIndex = 16;
            this.TSA_CheckBox.Text = "LZ77";
            this.TSA_CheckBox.UseVisualStyleBackColor = true;
            // 
            // Spell_PaletteBox
            // 
            this.Spell_PaletteBox.ColorsPerLine = 16;
            this.Spell_PaletteBox.Location = new System.Drawing.Point(284, 9);
            this.Spell_PaletteBox.Name = "Spell_PaletteBox";
            this.Spell_PaletteBox.Size = new System.Drawing.Size(128, 64);
            this.Spell_PaletteBox.TabIndex = 17;
            this.Spell_PaletteBox.TabStop = false;
            this.Spell_PaletteBox.Text = "paletteBox1";
            // 
            // ASM_ListBox
            // 
            this.ASM_ListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ASM_ListBox.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.ASM_ListBox.FormattingEnabled = true;
            this.ASM_ListBox.Location = new System.Drawing.Point(336, 79);
            this.ASM_ListBox.Name = "ASM_ListBox";
            this.ASM_ListBox.Size = new System.Drawing.Size(385, 407);
            this.ASM_ListBox.TabIndex = 19;
            // 
            // Constructor_Label
            // 
            this.Constructor_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Constructor_Label.AutoSize = true;
            this.Constructor_Label.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Constructor_Label.Checked = true;
            this.Constructor_Label.Location = new System.Drawing.Point(560, 4);
            this.Constructor_Label.Name = "Constructor_Label";
            this.Constructor_Label.Size = new System.Drawing.Size(85, 17);
            this.Constructor_Label.TabIndex = 21;
            this.Constructor_Label.TabStop = true;
            this.Constructor_Label.Text = "Constructor :";
            this.Constructor_Label.Click += new System.EventHandler(this.ASM_Label_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(284, 79);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(46, 54);
            this.button1.TabIndex = 28;
            this.button1.Text = "Copy ASM";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.CopyASM_Click);
            // 
            // Name_TextBox
            // 
            this.Name_TextBox.Location = new System.Drawing.Point(59, 44);
            this.Name_TextBox.Name = "Name_TextBox";
            this.Name_TextBox.Size = new System.Drawing.Size(126, 20);
            this.Name_TextBox.TabIndex = 30;
            // 
            // Looped_CheckBox
            // 
            this.Looped_CheckBox.AutoSize = true;
            this.Looped_CheckBox.Location = new System.Drawing.Point(191, 46);
            this.Looped_CheckBox.Name = "Looped_CheckBox";
            this.Looped_CheckBox.Size = new System.Drawing.Size(62, 17);
            this.Looped_CheckBox.TabIndex = 31;
            this.Looped_CheckBox.Text = "Looped";
            this.Looped_CheckBox.UseVisualStyleBackColor = true;
            // 
            // Name_Label
            // 
            this.Name_Label.AutoSize = true;
            this.Name_Label.Location = new System.Drawing.Point(12, 47);
            this.Name_Label.Name = "Name_Label";
            this.Name_Label.Size = new System.Drawing.Size(41, 13);
            this.Name_Label.TabIndex = 32;
            this.Name_Label.Text = "Name :";
            // 
            // LoopRoutine_PointerBox
            // 
            this.LoopRoutine_PointerBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LoopRoutine_PointerBox.Hexadecimal = true;
            this.LoopRoutine_PointerBox.Location = new System.Drawing.Point(651, 30);
            this.LoopRoutine_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.LoopRoutine_PointerBox.Name = "LoopRoutine_PointerBox";
            this.LoopRoutine_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.LoopRoutine_PointerBox.TabIndex = 18;
            // 
            // LoopRoutine_Label
            // 
            this.LoopRoutine_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LoopRoutine_Label.AutoSize = true;
            this.LoopRoutine_Label.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.LoopRoutine_Label.Location = new System.Drawing.Point(550, 30);
            this.LoopRoutine_Label.Name = "LoopRoutine_Label";
            this.LoopRoutine_Label.Size = new System.Drawing.Size(95, 17);
            this.LoopRoutine_Label.TabIndex = 23;
            this.LoopRoutine_Label.Text = "Loop Routine :";
            this.LoopRoutine_Label.Click += new System.EventHandler(this.ASM_Label_CheckedChanged);
            // 
            // AnimLoading_PointerBox
            // 
            this.AnimLoading_PointerBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AnimLoading_PointerBox.Hexadecimal = true;
            this.AnimLoading_PointerBox.Location = new System.Drawing.Point(651, 56);
            this.AnimLoading_PointerBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.AnimLoading_PointerBox.Name = "AnimLoading_PointerBox";
            this.AnimLoading_PointerBox.Size = new System.Drawing.Size(70, 20);
            this.AnimLoading_PointerBox.TabIndex = 24;
            // 
            // AnimLoading_Label
            // 
            this.AnimLoading_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AnimLoading_Label.AutoSize = true;
            this.AnimLoading_Label.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.AnimLoading_Label.Location = new System.Drawing.Point(550, 56);
            this.AnimLoading_Label.Name = "AnimLoading_Label";
            this.AnimLoading_Label.Size = new System.Drawing.Size(95, 17);
            this.AnimLoading_Label.TabIndex = 27;
            this.AnimLoading_Label.Text = "Anim Loading :";
            this.AnimLoading_Label.Click += new System.EventHandler(this.ASM_Label_CheckedChanged);
            // 
            // AnimCodeBox
            // 
            this.AnimCodeBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AnimCodeBox.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.AnimCodeBox.AutoScrollMinSize = new System.Drawing.Size(23, 12);
            this.AnimCodeBox.BackBrush = null;
            this.AnimCodeBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AnimCodeBox.CharHeight = 12;
            this.AnimCodeBox.CharWidth = 6;
            this.AnimCodeBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.AnimCodeBox.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.AnimCodeBox.Font = new System.Drawing.Font("Consolas", 8F);
            this.AnimCodeBox.IsReplaceMode = false;
            this.AnimCodeBox.Location = new System.Drawing.Point(12, 219);
            this.AnimCodeBox.Name = "AnimCodeBox";
            this.AnimCodeBox.Paddings = new System.Windows.Forms.Padding(0);
            this.AnimCodeBox.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.AnimCodeBox.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("AnimCodeBox.ServiceColors")));
            this.AnimCodeBox.Size = new System.Drawing.Size(318, 267);
            this.AnimCodeBox.TabIndex = 33;
            this.AnimCodeBox.Zoom = 100;
            // 
            // MagicButton
            // 
            this.MagicButton.Location = new System.Drawing.Point(306, 140);
            this.MagicButton.MaximumSize = new System.Drawing.Size(24, 24);
            this.MagicButton.MinimumSize = new System.Drawing.Size(24, 24);
            this.MagicButton.Name = "MagicButton";
            this.MagicButton.Size = new System.Drawing.Size(24, 24);
            this.MagicButton.TabIndex = 34;
            this.MagicButton.UseVisualStyleBackColor = true;
            this.MagicButton.Click += new System.EventHandler(this.MagicButton_Click);
            // 
            // Palette_Prev_Button
            // 
            this.Palette_Prev_Button.Location = new System.Drawing.Point(212, 140);
            this.Palette_Prev_Button.Name = "Palette_Prev_Button";
            this.Palette_Prev_Button.Size = new System.Drawing.Size(30, 20);
            this.Palette_Prev_Button.TabIndex = 35;
            this.Palette_Prev_Button.Text = "<-";
            this.Palette_Prev_Button.UseVisualStyleBackColor = true;
            this.Palette_Prev_Button.Click += new System.EventHandler(this.Palette_Prev_Button_Click);
            // 
            // Palette_Next_Button
            // 
            this.Palette_Next_Button.Location = new System.Drawing.Point(248, 140);
            this.Palette_Next_Button.Name = "Palette_Next_Button";
            this.Palette_Next_Button.Size = new System.Drawing.Size(30, 20);
            this.Palette_Next_Button.TabIndex = 36;
            this.Palette_Next_Button.Text = "->";
            this.Palette_Next_Button.UseVisualStyleBackColor = true;
            this.Palette_Next_Button.Click += new System.EventHandler(this.Palette_Next_Button_Click);
            // 
            // Tileset_Next_Button
            // 
            this.Tileset_Next_Button.Location = new System.Drawing.Point(248, 166);
            this.Tileset_Next_Button.Name = "Tileset_Next_Button";
            this.Tileset_Next_Button.Size = new System.Drawing.Size(30, 20);
            this.Tileset_Next_Button.TabIndex = 38;
            this.Tileset_Next_Button.Text = "->";
            this.Tileset_Next_Button.UseVisualStyleBackColor = true;
            this.Tileset_Next_Button.Click += new System.EventHandler(this.Tileset_Next_Button_Click);
            // 
            // Tileset_Prev_Button
            // 
            this.Tileset_Prev_Button.Location = new System.Drawing.Point(212, 166);
            this.Tileset_Prev_Button.Name = "Tileset_Prev_Button";
            this.Tileset_Prev_Button.Size = new System.Drawing.Size(30, 20);
            this.Tileset_Prev_Button.TabIndex = 37;
            this.Tileset_Prev_Button.Text = "<-";
            this.Tileset_Prev_Button.UseVisualStyleBackColor = true;
            this.Tileset_Prev_Button.Click += new System.EventHandler(this.Tileset_Prev_Button_Click);
            // 
            // TSA_Next_Button
            // 
            this.TSA_Next_Button.Location = new System.Drawing.Point(248, 192);
            this.TSA_Next_Button.Name = "TSA_Next_Button";
            this.TSA_Next_Button.Size = new System.Drawing.Size(30, 20);
            this.TSA_Next_Button.TabIndex = 40;
            this.TSA_Next_Button.Text = "->";
            this.TSA_Next_Button.UseVisualStyleBackColor = true;
            this.TSA_Next_Button.Click += new System.EventHandler(this.TSA_Next_Button_Click);
            // 
            // TSA_Prev_Button
            // 
            this.TSA_Prev_Button.Location = new System.Drawing.Point(212, 192);
            this.TSA_Prev_Button.Name = "TSA_Prev_Button";
            this.TSA_Prev_Button.Size = new System.Drawing.Size(30, 20);
            this.TSA_Prev_Button.TabIndex = 39;
            this.TSA_Prev_Button.Text = "<-";
            this.TSA_Prev_Button.UseVisualStyleBackColor = true;
            this.TSA_Prev_Button.Click += new System.EventHandler(this.TSA_Prev_Button_Click);
            // 
            // SpellAnimEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 500);
            this.Controls.Add(this.TSA_Next_Button);
            this.Controls.Add(this.TSA_Prev_Button);
            this.Controls.Add(this.Tileset_Next_Button);
            this.Controls.Add(this.Tileset_Prev_Button);
            this.Controls.Add(this.Palette_Next_Button);
            this.Controls.Add(this.Palette_Prev_Button);
            this.Controls.Add(this.MagicButton);
            this.Controls.Add(this.AnimCodeBox);
            this.Controls.Add(this.Name_Label);
            this.Controls.Add(this.Looped_CheckBox);
            this.Controls.Add(this.Name_TextBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.AnimLoading_Label);
            this.Controls.Add(this.AnimLoading_PointerBox);
            this.Controls.Add(this.LoopRoutine_Label);
            this.Controls.Add(this.Constructor_Label);
            this.Controls.Add(this.ASM_ListBox);
            this.Controls.Add(this.LoopRoutine_PointerBox);
            this.Controls.Add(this.Spell_PaletteBox);
            this.Controls.Add(this.TSA_CheckBox);
            this.Controls.Add(this.Tileset_CheckBox);
            this.Controls.Add(this.Palette_CheckBox);
            this.Controls.Add(this.Refresh_Button);
            this.Controls.Add(this.Spell_ImageBox);
            this.Controls.Add(this.TSA_Label);
            this.Controls.Add(this.Tileset_Label);
            this.Controls.Add(this.Palette_Label);
            this.Controls.Add(this.TSA_PointerBox);
            this.Controls.Add(this.Tileset_PointerBox);
            this.Controls.Add(this.Palette_PointerBox);
            this.Controls.Add(this.Constructor_PointerBox);
            this.Controls.Add(this.EntryArrayBox);
            this.Name = "SpellAnimEditor";
            this.Text = "SpellAnimEditor";
            ((System.ComponentModel.ISupportInitialize)(this.Constructor_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Palette_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tileset_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TSA_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoopRoutine_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AnimLoading_PointerBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AnimCodeBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Components.ByteArrayBox EntryArrayBox;
        private Components.PointerBox Constructor_PointerBox;
        private Components.PointerBox Palette_PointerBox;
        private Components.PointerBox Tileset_PointerBox;
        private Components.PointerBox TSA_PointerBox;
        private System.Windows.Forms.Label Palette_Label;
        private System.Windows.Forms.Label Tileset_Label;
        private System.Windows.Forms.CheckBox TSA_Label;
        private Components.ImageBox Spell_ImageBox;
        private System.Windows.Forms.Button Refresh_Button;
        private System.Windows.Forms.CheckBox Palette_CheckBox;
        private System.Windows.Forms.CheckBox Tileset_CheckBox;
        private System.Windows.Forms.CheckBox TSA_CheckBox;
        private Components.PaletteBox Spell_PaletteBox;
        private System.Windows.Forms.ListBox ASM_ListBox;
        private System.Windows.Forms.RadioButton Constructor_Label;
        private System.Windows.Forms.RadioButton LoopRoutine_Label;
        private System.Windows.Forms.RadioButton AnimLoading_Label;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox Name_TextBox;
        private System.Windows.Forms.CheckBox Looped_CheckBox;
        private System.Windows.Forms.Label Name_Label;
        private Components.PointerBox LoopRoutine_PointerBox;
        private Components.PointerBox AnimLoading_PointerBox;
        private Components.CodeBox AnimCodeBox;
        private Components.MagicButton MagicButton;
        private System.Windows.Forms.Button Palette_Prev_Button;
        private System.Windows.Forms.Button Palette_Next_Button;
        private System.Windows.Forms.Button Tileset_Next_Button;
        private System.Windows.Forms.Button Tileset_Prev_Button;
        private System.Windows.Forms.Button TSA_Next_Button;
        private System.Windows.Forms.Button TSA_Prev_Button;
    }
}