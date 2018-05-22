namespace EmblemMagic.Editors
{
    partial class ASMEditor
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
            this.Read_AddressBox = new EmblemMagic.Components.PointerBox();
            this.Read_AddressLabel = new System.Windows.Forms.Label();
            this.Read_LengthLabel = new System.Windows.Forms.Label();
            this.Read_LengthBox = new System.Windows.Forms.NumericUpDown();
            this.Read_ARM_RadioButton = new System.Windows.Forms.RadioButton();
            this.Read_ThumbRadioButton = new System.Windows.Forms.RadioButton();
            this.Read_OKButton = new System.Windows.Forms.Button();
            this.Test_R0 = new System.Windows.Forms.NumericUpDown();
            this.Test_R0_Label = new System.Windows.Forms.Label();
            this.Read_GroupBox = new System.Windows.Forms.GroupBox();
            this.Test_GroupBox = new System.Windows.Forms.GroupBox();
            this.Test_Stack = new System.Windows.Forms.ListBox();
            this.Test_ResetCPUButton = new System.Windows.Forms.Button();
            this.Test_ReadLineButton = new System.Windows.Forms.Button();
            this.Test_CPSR_ModeLabel = new System.Windows.Forms.Label();
            this.Test_CPSR_Mode = new EmblemMagic.Components.ByteBox();
            this.Test_CPSR_T = new System.Windows.Forms.CheckBox();
            this.Test_CPSR_F = new System.Windows.Forms.CheckBox();
            this.Test_CPSR_I = new System.Windows.Forms.CheckBox();
            this.Test_CPSR_V = new System.Windows.Forms.CheckBox();
            this.Test_CPSR_C = new System.Windows.Forms.CheckBox();
            this.Test_CPSR_Z = new System.Windows.Forms.CheckBox();
            this.Test_CPSR_N = new System.Windows.Forms.CheckBox();
            this.Test_PC = new System.Windows.Forms.NumericUpDown();
            this.Test_PC_Label = new System.Windows.Forms.Label();
            this.Test_LR = new System.Windows.Forms.NumericUpDown();
            this.Test_LR_Label = new System.Windows.Forms.Label();
            this.Test_SP = new System.Windows.Forms.NumericUpDown();
            this.Test_SP_Label = new System.Windows.Forms.Label();
            this.Test_R12 = new System.Windows.Forms.NumericUpDown();
            this.Test_R12_Label = new System.Windows.Forms.Label();
            this.Test_R11 = new System.Windows.Forms.NumericUpDown();
            this.Test_R11_Label = new System.Windows.Forms.Label();
            this.Test_R10 = new System.Windows.Forms.NumericUpDown();
            this.Test_R10_Label = new System.Windows.Forms.Label();
            this.Test_R9 = new System.Windows.Forms.NumericUpDown();
            this.Test_R9_Label = new System.Windows.Forms.Label();
            this.Test_R8 = new System.Windows.Forms.NumericUpDown();
            this.Test_R8_Label = new System.Windows.Forms.Label();
            this.Test_R7 = new System.Windows.Forms.NumericUpDown();
            this.Test_R7_Label = new System.Windows.Forms.Label();
            this.Test_R6 = new System.Windows.Forms.NumericUpDown();
            this.Test_R6_Label = new System.Windows.Forms.Label();
            this.Test_R5 = new System.Windows.Forms.NumericUpDown();
            this.Test_R5_Label = new System.Windows.Forms.Label();
            this.Test_R4 = new System.Windows.Forms.NumericUpDown();
            this.Test_R4_Label = new System.Windows.Forms.Label();
            this.Test_R3 = new System.Windows.Forms.NumericUpDown();
            this.Test_R3_Label = new System.Windows.Forms.Label();
            this.Test_R2 = new System.Windows.Forms.NumericUpDown();
            this.Test_R2_Label = new System.Windows.Forms.Label();
            this.Test_R1 = new System.Windows.Forms.NumericUpDown();
            this.Test_R1_Label = new System.Windows.Forms.Label();
            this.Write_GroupBox = new System.Windows.Forms.GroupBox();
            this.IO_SaveButton = new System.Windows.Forms.Button();
            this.IO_WriteButton = new System.Windows.Forms.Button();
            this.CodeBox = new System.Windows.Forms.ListView();
            this.CodeBox_Address = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CodeBox_Data = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CodeBox_ASM = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.Read_AddressBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Read_LengthBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_R0)).BeginInit();
            this.Read_GroupBox.SuspendLayout();
            this.Test_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Test_CPSR_Mode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_PC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_LR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_SP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_R12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_R11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_R10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_R9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_R8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_R7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_R6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_R5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_R4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_R3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_R2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_R1)).BeginInit();
            this.Write_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // Read_AddressBox
            // 
            this.Read_AddressBox.Hexadecimal = true;
            this.Read_AddressBox.Location = new System.Drawing.Point(61, 38);
            this.Read_AddressBox.Maximum = new decimal(new int[] {
            33554432,
            0,
            0,
            0});
            this.Read_AddressBox.Name = "Read_AddressBox";
            this.Read_AddressBox.Size = new System.Drawing.Size(70, 20);
            this.Read_AddressBox.TabIndex = 5;
            // 
            // Read_AddressLabel
            // 
            this.Read_AddressLabel.Location = new System.Drawing.Point(3, 37);
            this.Read_AddressLabel.Name = "Read_AddressLabel";
            this.Read_AddressLabel.Size = new System.Drawing.Size(52, 18);
            this.Read_AddressLabel.TabIndex = 3;
            this.Read_AddressLabel.Text = "Address :";
            this.Read_AddressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Read_LengthLabel
            // 
            this.Read_LengthLabel.Location = new System.Drawing.Point(6, 63);
            this.Read_LengthLabel.Name = "Read_LengthLabel";
            this.Read_LengthLabel.Size = new System.Drawing.Size(49, 18);
            this.Read_LengthLabel.TabIndex = 4;
            this.Read_LengthLabel.Text = "Length :";
            this.Read_LengthLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Read_LengthBox
            // 
            this.Read_LengthBox.Hexadecimal = true;
            this.Read_LengthBox.Location = new System.Drawing.Point(61, 64);
            this.Read_LengthBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Read_LengthBox.Name = "Read_LengthBox";
            this.Read_LengthBox.Size = new System.Drawing.Size(70, 20);
            this.Read_LengthBox.TabIndex = 6;
            this.Read_LengthBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // Read_ARM_RadioButton
            // 
            this.Read_ARM_RadioButton.AutoSize = true;
            this.Read_ARM_RadioButton.Location = new System.Drawing.Point(13, 17);
            this.Read_ARM_RadioButton.Name = "Read_ARM_RadioButton";
            this.Read_ARM_RadioButton.Size = new System.Drawing.Size(49, 17);
            this.Read_ARM_RadioButton.TabIndex = 7;
            this.Read_ARM_RadioButton.Text = "ARM";
            this.Read_ARM_RadioButton.UseVisualStyleBackColor = true;
            // 
            // Read_ThumbRadioButton
            // 
            this.Read_ThumbRadioButton.AutoSize = true;
            this.Read_ThumbRadioButton.Checked = true;
            this.Read_ThumbRadioButton.Location = new System.Drawing.Point(73, 17);
            this.Read_ThumbRadioButton.Name = "Read_ThumbRadioButton";
            this.Read_ThumbRadioButton.Size = new System.Drawing.Size(58, 17);
            this.Read_ThumbRadioButton.TabIndex = 8;
            this.Read_ThumbRadioButton.TabStop = true;
            this.Read_ThumbRadioButton.Text = "Thumb";
            this.Read_ThumbRadioButton.UseVisualStyleBackColor = true;
            // 
            // Read_OKButton
            // 
            this.Read_OKButton.Location = new System.Drawing.Point(6, 92);
            this.Read_OKButton.Name = "Read_OKButton";
            this.Read_OKButton.Size = new System.Drawing.Size(125, 28);
            this.Read_OKButton.TabIndex = 9;
            this.Read_OKButton.Text = "Disassemble";
            this.Read_OKButton.UseVisualStyleBackColor = true;
            this.Read_OKButton.Click += new System.EventHandler(this.DissassembleButton_Click);
            // 
            // Test_R0
            // 
            this.Test_R0.Hexadecimal = true;
            this.Test_R0.Location = new System.Drawing.Point(44, 19);
            this.Test_R0.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.Test_R0.Name = "Test_R0";
            this.Test_R0.Size = new System.Drawing.Size(87, 20);
            this.Test_R0.TabIndex = 12;
            // 
            // Test_R0_Label
            // 
            this.Test_R0_Label.Location = new System.Drawing.Point(6, 18);
            this.Test_R0_Label.Name = "Test_R0_Label";
            this.Test_R0_Label.Size = new System.Drawing.Size(32, 18);
            this.Test_R0_Label.TabIndex = 13;
            this.Test_R0_Label.Text = "R0 :";
            this.Test_R0_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Read_GroupBox
            // 
            this.Read_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Read_GroupBox.Controls.Add(this.Read_OKButton);
            this.Read_GroupBox.Controls.Add(this.Read_LengthBox);
            this.Read_GroupBox.Controls.Add(this.Read_LengthLabel);
            this.Read_GroupBox.Controls.Add(this.Read_AddressLabel);
            this.Read_GroupBox.Controls.Add(this.Read_AddressBox);
            this.Read_GroupBox.Controls.Add(this.Read_ARM_RadioButton);
            this.Read_GroupBox.Controls.Add(this.Read_ThumbRadioButton);
            this.Read_GroupBox.Location = new System.Drawing.Point(409, 3);
            this.Read_GroupBox.Name = "Read_GroupBox";
            this.Read_GroupBox.Size = new System.Drawing.Size(139, 127);
            this.Read_GroupBox.TabIndex = 14;
            this.Read_GroupBox.TabStop = false;
            this.Read_GroupBox.Text = "Read ASM from ROM";
            // 
            // Test_GroupBox
            // 
            this.Test_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Test_GroupBox.Controls.Add(this.Test_Stack);
            this.Test_GroupBox.Controls.Add(this.Test_ResetCPUButton);
            this.Test_GroupBox.Controls.Add(this.Test_ReadLineButton);
            this.Test_GroupBox.Controls.Add(this.Test_CPSR_ModeLabel);
            this.Test_GroupBox.Controls.Add(this.Test_CPSR_Mode);
            this.Test_GroupBox.Controls.Add(this.Test_CPSR_T);
            this.Test_GroupBox.Controls.Add(this.Test_CPSR_F);
            this.Test_GroupBox.Controls.Add(this.Test_CPSR_I);
            this.Test_GroupBox.Controls.Add(this.Test_CPSR_V);
            this.Test_GroupBox.Controls.Add(this.Test_CPSR_C);
            this.Test_GroupBox.Controls.Add(this.Test_CPSR_Z);
            this.Test_GroupBox.Controls.Add(this.Test_CPSR_N);
            this.Test_GroupBox.Controls.Add(this.Test_PC);
            this.Test_GroupBox.Controls.Add(this.Test_PC_Label);
            this.Test_GroupBox.Controls.Add(this.Test_LR);
            this.Test_GroupBox.Controls.Add(this.Test_LR_Label);
            this.Test_GroupBox.Controls.Add(this.Test_SP);
            this.Test_GroupBox.Controls.Add(this.Test_SP_Label);
            this.Test_GroupBox.Controls.Add(this.Test_R12);
            this.Test_GroupBox.Controls.Add(this.Test_R12_Label);
            this.Test_GroupBox.Controls.Add(this.Test_R11);
            this.Test_GroupBox.Controls.Add(this.Test_R11_Label);
            this.Test_GroupBox.Controls.Add(this.Test_R10);
            this.Test_GroupBox.Controls.Add(this.Test_R10_Label);
            this.Test_GroupBox.Controls.Add(this.Test_R9);
            this.Test_GroupBox.Controls.Add(this.Test_R9_Label);
            this.Test_GroupBox.Controls.Add(this.Test_R8);
            this.Test_GroupBox.Controls.Add(this.Test_R8_Label);
            this.Test_GroupBox.Controls.Add(this.Test_R7);
            this.Test_GroupBox.Controls.Add(this.Test_R7_Label);
            this.Test_GroupBox.Controls.Add(this.Test_R6);
            this.Test_GroupBox.Controls.Add(this.Test_R6_Label);
            this.Test_GroupBox.Controls.Add(this.Test_R5);
            this.Test_GroupBox.Controls.Add(this.Test_R5_Label);
            this.Test_GroupBox.Controls.Add(this.Test_R4);
            this.Test_GroupBox.Controls.Add(this.Test_R4_Label);
            this.Test_GroupBox.Controls.Add(this.Test_R3);
            this.Test_GroupBox.Controls.Add(this.Test_R3_Label);
            this.Test_GroupBox.Controls.Add(this.Test_R2);
            this.Test_GroupBox.Controls.Add(this.Test_R2_Label);
            this.Test_GroupBox.Controls.Add(this.Test_R1);
            this.Test_GroupBox.Controls.Add(this.Test_R1_Label);
            this.Test_GroupBox.Controls.Add(this.Test_R0);
            this.Test_GroupBox.Controls.Add(this.Test_R0_Label);
            this.Test_GroupBox.Location = new System.Drawing.Point(409, 136);
            this.Test_GroupBox.Name = "Test_GroupBox";
            this.Test_GroupBox.Size = new System.Drawing.Size(283, 347);
            this.Test_GroupBox.TabIndex = 10;
            this.Test_GroupBox.TabStop = false;
            this.Test_GroupBox.Text = "ASM Test CPU";
            // 
            // Test_Stack
            // 
            this.Test_Stack.FormattingEnabled = true;
            this.Test_Stack.Location = new System.Drawing.Point(6, 225);
            this.Test_Stack.Name = "Test_Stack";
            this.Test_Stack.ScrollAlwaysVisible = true;
            this.Test_Stack.Size = new System.Drawing.Size(82, 82);
            this.Test_Stack.TabIndex = 54;
            // 
            // Test_ResetCPUButton
            // 
            this.Test_ResetCPUButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Test_ResetCPUButton.Location = new System.Drawing.Point(6, 313);
            this.Test_ResetCPUButton.Name = "Test_ResetCPUButton";
            this.Test_ResetCPUButton.Size = new System.Drawing.Size(133, 28);
            this.Test_ResetCPUButton.TabIndex = 18;
            this.Test_ResetCPUButton.Text = "Reset registers";
            this.Test_ResetCPUButton.UseVisualStyleBackColor = true;
            this.Test_ResetCPUButton.Click += new System.EventHandler(this.Test_ResetCPUButton_Click);
            // 
            // Test_ReadLineButton
            // 
            this.Test_ReadLineButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Test_ReadLineButton.Location = new System.Drawing.Point(145, 313);
            this.Test_ReadLineButton.Name = "Test_ReadLineButton";
            this.Test_ReadLineButton.Size = new System.Drawing.Size(132, 28);
            this.Test_ReadLineButton.TabIndex = 17;
            this.Test_ReadLineButton.Text = "Read selected line";
            this.Test_ReadLineButton.UseVisualStyleBackColor = true;
            this.Test_ReadLineButton.Click += new System.EventHandler(this.Test_ReadLineButton_Click);
            // 
            // Test_CPSR_ModeLabel
            // 
            this.Test_CPSR_ModeLabel.AutoSize = true;
            this.Test_CPSR_ModeLabel.Location = new System.Drawing.Point(230, 289);
            this.Test_CPSR_ModeLabel.Name = "Test_CPSR_ModeLabel";
            this.Test_CPSR_ModeLabel.Size = new System.Drawing.Size(34, 13);
            this.Test_CPSR_ModeLabel.TabIndex = 53;
            this.Test_CPSR_ModeLabel.Text = "Mode";
            this.Test_CPSR_ModeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Test_CPSR_Mode
            // 
            this.Test_CPSR_Mode.Hexadecimal = true;
            this.Test_CPSR_Mode.Location = new System.Drawing.Point(184, 287);
            this.Test_CPSR_Mode.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Test_CPSR_Mode.Name = "Test_CPSR_Mode";
            this.Test_CPSR_Mode.Size = new System.Drawing.Size(40, 20);
            this.Test_CPSR_Mode.TabIndex = 52;
            this.Test_CPSR_Mode.Value = ((byte)(0));
            // 
            // Test_CPSR_T
            // 
            this.Test_CPSR_T.AutoSize = true;
            this.Test_CPSR_T.Location = new System.Drawing.Point(184, 267);
            this.Test_CPSR_T.Name = "Test_CPSR_T";
            this.Test_CPSR_T.Size = new System.Drawing.Size(75, 17);
            this.Test_CPSR_T.TabIndex = 51;
            this.Test_CPSR_T.Text = "T - Thumb";
            this.Test_CPSR_T.UseVisualStyleBackColor = true;
            // 
            // Test_CPSR_F
            // 
            this.Test_CPSR_F.AutoSize = true;
            this.Test_CPSR_F.Location = new System.Drawing.Point(184, 247);
            this.Test_CPSR_F.Name = "Test_CPSR_F";
            this.Test_CPSR_F.Size = new System.Drawing.Size(94, 17);
            this.Test_CPSR_F.TabIndex = 50;
            this.Test_CPSR_F.Text = "F - FIQ disable";
            this.Test_CPSR_F.UseVisualStyleBackColor = true;
            // 
            // Test_CPSR_I
            // 
            this.Test_CPSR_I.AutoSize = true;
            this.Test_CPSR_I.Location = new System.Drawing.Point(184, 227);
            this.Test_CPSR_I.Name = "Test_CPSR_I";
            this.Test_CPSR_I.Size = new System.Drawing.Size(93, 17);
            this.Test_CPSR_I.TabIndex = 49;
            this.Test_CPSR_I.Text = "I - IRQ disable";
            this.Test_CPSR_I.UseVisualStyleBackColor = true;
            // 
            // Test_CPSR_V
            // 
            this.Test_CPSR_V.AutoSize = true;
            this.Test_CPSR_V.Location = new System.Drawing.Point(94, 287);
            this.Test_CPSR_V.Name = "Test_CPSR_V";
            this.Test_CPSR_V.Size = new System.Drawing.Size(84, 17);
            this.Test_CPSR_V.TabIndex = 47;
            this.Test_CPSR_V.Text = "V - Overflow";
            this.Test_CPSR_V.UseVisualStyleBackColor = true;
            // 
            // Test_CPSR_C
            // 
            this.Test_CPSR_C.AutoSize = true;
            this.Test_CPSR_C.Location = new System.Drawing.Point(94, 267);
            this.Test_CPSR_C.Name = "Test_CPSR_C";
            this.Test_CPSR_C.Size = new System.Drawing.Size(86, 17);
            this.Test_CPSR_C.TabIndex = 46;
            this.Test_CPSR_C.Text = "C - Carry flag";
            this.Test_CPSR_C.UseVisualStyleBackColor = true;
            // 
            // Test_CPSR_Z
            // 
            this.Test_CPSR_Z.AutoSize = true;
            this.Test_CPSR_Z.Location = new System.Drawing.Point(94, 247);
            this.Test_CPSR_Z.Name = "Test_CPSR_Z";
            this.Test_CPSR_Z.Size = new System.Drawing.Size(84, 17);
            this.Test_CPSR_Z.TabIndex = 45;
            this.Test_CPSR_Z.Text = "Z - Zero flag";
            this.Test_CPSR_Z.UseVisualStyleBackColor = true;
            // 
            // Test_CPSR_N
            // 
            this.Test_CPSR_N.AutoSize = true;
            this.Test_CPSR_N.Location = new System.Drawing.Point(94, 227);
            this.Test_CPSR_N.Name = "Test_CPSR_N";
            this.Test_CPSR_N.Size = new System.Drawing.Size(84, 17);
            this.Test_CPSR_N.TabIndex = 44;
            this.Test_CPSR_N.Text = "N - Sign flag";
            this.Test_CPSR_N.UseVisualStyleBackColor = true;
            // 
            // Test_PC
            // 
            this.Test_PC.Hexadecimal = true;
            this.Test_PC.Location = new System.Drawing.Point(179, 201);
            this.Test_PC.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.Test_PC.Name = "Test_PC";
            this.Test_PC.Size = new System.Drawing.Size(87, 20);
            this.Test_PC.TabIndex = 42;
            // 
            // Test_PC_Label
            // 
            this.Test_PC_Label.Location = new System.Drawing.Point(137, 200);
            this.Test_PC_Label.Name = "Test_PC_Label";
            this.Test_PC_Label.Size = new System.Drawing.Size(36, 18);
            this.Test_PC_Label.TabIndex = 43;
            this.Test_PC_Label.Text = "PC :";
            this.Test_PC_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Test_LR
            // 
            this.Test_LR.Hexadecimal = true;
            this.Test_LR.Location = new System.Drawing.Point(179, 175);
            this.Test_LR.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.Test_LR.Name = "Test_LR";
            this.Test_LR.Size = new System.Drawing.Size(87, 20);
            this.Test_LR.TabIndex = 40;
            // 
            // Test_LR_Label
            // 
            this.Test_LR_Label.Location = new System.Drawing.Point(137, 174);
            this.Test_LR_Label.Name = "Test_LR_Label";
            this.Test_LR_Label.Size = new System.Drawing.Size(36, 18);
            this.Test_LR_Label.TabIndex = 41;
            this.Test_LR_Label.Text = "LR :";
            this.Test_LR_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Test_SP
            // 
            this.Test_SP.Hexadecimal = true;
            this.Test_SP.Location = new System.Drawing.Point(179, 149);
            this.Test_SP.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.Test_SP.Name = "Test_SP";
            this.Test_SP.Size = new System.Drawing.Size(87, 20);
            this.Test_SP.TabIndex = 38;
            // 
            // Test_SP_Label
            // 
            this.Test_SP_Label.Location = new System.Drawing.Point(137, 148);
            this.Test_SP_Label.Name = "Test_SP_Label";
            this.Test_SP_Label.Size = new System.Drawing.Size(36, 18);
            this.Test_SP_Label.TabIndex = 39;
            this.Test_SP_Label.Text = "SP :";
            this.Test_SP_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Test_R12
            // 
            this.Test_R12.Hexadecimal = true;
            this.Test_R12.Location = new System.Drawing.Point(179, 123);
            this.Test_R12.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.Test_R12.Name = "Test_R12";
            this.Test_R12.Size = new System.Drawing.Size(87, 20);
            this.Test_R12.TabIndex = 36;
            // 
            // Test_R12_Label
            // 
            this.Test_R12_Label.Location = new System.Drawing.Point(137, 122);
            this.Test_R12_Label.Name = "Test_R12_Label";
            this.Test_R12_Label.Size = new System.Drawing.Size(36, 18);
            this.Test_R12_Label.TabIndex = 37;
            this.Test_R12_Label.Text = "R12 :";
            this.Test_R12_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Test_R11
            // 
            this.Test_R11.Hexadecimal = true;
            this.Test_R11.Location = new System.Drawing.Point(179, 97);
            this.Test_R11.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.Test_R11.Name = "Test_R11";
            this.Test_R11.Size = new System.Drawing.Size(87, 20);
            this.Test_R11.TabIndex = 34;
            // 
            // Test_R11_Label
            // 
            this.Test_R11_Label.Location = new System.Drawing.Point(137, 96);
            this.Test_R11_Label.Name = "Test_R11_Label";
            this.Test_R11_Label.Size = new System.Drawing.Size(36, 18);
            this.Test_R11_Label.TabIndex = 35;
            this.Test_R11_Label.Text = "R11 :";
            this.Test_R11_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Test_R10
            // 
            this.Test_R10.Hexadecimal = true;
            this.Test_R10.Location = new System.Drawing.Point(179, 71);
            this.Test_R10.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.Test_R10.Name = "Test_R10";
            this.Test_R10.Size = new System.Drawing.Size(87, 20);
            this.Test_R10.TabIndex = 32;
            // 
            // Test_R10_Label
            // 
            this.Test_R10_Label.Location = new System.Drawing.Point(137, 70);
            this.Test_R10_Label.Name = "Test_R10_Label";
            this.Test_R10_Label.Size = new System.Drawing.Size(36, 18);
            this.Test_R10_Label.TabIndex = 33;
            this.Test_R10_Label.Text = "R10 :";
            this.Test_R10_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Test_R9
            // 
            this.Test_R9.Hexadecimal = true;
            this.Test_R9.Location = new System.Drawing.Point(179, 45);
            this.Test_R9.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.Test_R9.Name = "Test_R9";
            this.Test_R9.Size = new System.Drawing.Size(87, 20);
            this.Test_R9.TabIndex = 30;
            // 
            // Test_R9_Label
            // 
            this.Test_R9_Label.Location = new System.Drawing.Point(137, 44);
            this.Test_R9_Label.Name = "Test_R9_Label";
            this.Test_R9_Label.Size = new System.Drawing.Size(36, 18);
            this.Test_R9_Label.TabIndex = 31;
            this.Test_R9_Label.Text = "R9 :";
            this.Test_R9_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Test_R8
            // 
            this.Test_R8.Hexadecimal = true;
            this.Test_R8.Location = new System.Drawing.Point(179, 19);
            this.Test_R8.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.Test_R8.Name = "Test_R8";
            this.Test_R8.Size = new System.Drawing.Size(87, 20);
            this.Test_R8.TabIndex = 28;
            // 
            // Test_R8_Label
            // 
            this.Test_R8_Label.Location = new System.Drawing.Point(137, 18);
            this.Test_R8_Label.Name = "Test_R8_Label";
            this.Test_R8_Label.Size = new System.Drawing.Size(36, 18);
            this.Test_R8_Label.TabIndex = 29;
            this.Test_R8_Label.Text = "R8 :";
            this.Test_R8_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Test_R7
            // 
            this.Test_R7.Hexadecimal = true;
            this.Test_R7.Location = new System.Drawing.Point(44, 201);
            this.Test_R7.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.Test_R7.Name = "Test_R7";
            this.Test_R7.Size = new System.Drawing.Size(87, 20);
            this.Test_R7.TabIndex = 26;
            // 
            // Test_R7_Label
            // 
            this.Test_R7_Label.Location = new System.Drawing.Point(6, 200);
            this.Test_R7_Label.Name = "Test_R7_Label";
            this.Test_R7_Label.Size = new System.Drawing.Size(32, 18);
            this.Test_R7_Label.TabIndex = 27;
            this.Test_R7_Label.Text = "R7 :";
            this.Test_R7_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Test_R6
            // 
            this.Test_R6.Hexadecimal = true;
            this.Test_R6.Location = new System.Drawing.Point(44, 175);
            this.Test_R6.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.Test_R6.Name = "Test_R6";
            this.Test_R6.Size = new System.Drawing.Size(87, 20);
            this.Test_R6.TabIndex = 24;
            // 
            // Test_R6_Label
            // 
            this.Test_R6_Label.Location = new System.Drawing.Point(6, 174);
            this.Test_R6_Label.Name = "Test_R6_Label";
            this.Test_R6_Label.Size = new System.Drawing.Size(32, 18);
            this.Test_R6_Label.TabIndex = 25;
            this.Test_R6_Label.Text = "R6 :";
            this.Test_R6_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Test_R5
            // 
            this.Test_R5.Hexadecimal = true;
            this.Test_R5.Location = new System.Drawing.Point(44, 149);
            this.Test_R5.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.Test_R5.Name = "Test_R5";
            this.Test_R5.Size = new System.Drawing.Size(87, 20);
            this.Test_R5.TabIndex = 22;
            // 
            // Test_R5_Label
            // 
            this.Test_R5_Label.Location = new System.Drawing.Point(6, 148);
            this.Test_R5_Label.Name = "Test_R5_Label";
            this.Test_R5_Label.Size = new System.Drawing.Size(32, 18);
            this.Test_R5_Label.TabIndex = 23;
            this.Test_R5_Label.Text = "R5 :";
            this.Test_R5_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Test_R4
            // 
            this.Test_R4.Hexadecimal = true;
            this.Test_R4.Location = new System.Drawing.Point(44, 123);
            this.Test_R4.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.Test_R4.Name = "Test_R4";
            this.Test_R4.Size = new System.Drawing.Size(87, 20);
            this.Test_R4.TabIndex = 20;
            // 
            // Test_R4_Label
            // 
            this.Test_R4_Label.Location = new System.Drawing.Point(6, 122);
            this.Test_R4_Label.Name = "Test_R4_Label";
            this.Test_R4_Label.Size = new System.Drawing.Size(32, 18);
            this.Test_R4_Label.TabIndex = 21;
            this.Test_R4_Label.Text = "R4 :";
            this.Test_R4_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Test_R3
            // 
            this.Test_R3.Hexadecimal = true;
            this.Test_R3.Location = new System.Drawing.Point(44, 97);
            this.Test_R3.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.Test_R3.Name = "Test_R3";
            this.Test_R3.Size = new System.Drawing.Size(87, 20);
            this.Test_R3.TabIndex = 18;
            // 
            // Test_R3_Label
            // 
            this.Test_R3_Label.Location = new System.Drawing.Point(6, 96);
            this.Test_R3_Label.Name = "Test_R3_Label";
            this.Test_R3_Label.Size = new System.Drawing.Size(32, 18);
            this.Test_R3_Label.TabIndex = 19;
            this.Test_R3_Label.Text = "R3 :";
            this.Test_R3_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Test_R2
            // 
            this.Test_R2.Hexadecimal = true;
            this.Test_R2.Location = new System.Drawing.Point(44, 70);
            this.Test_R2.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.Test_R2.Name = "Test_R2";
            this.Test_R2.Size = new System.Drawing.Size(87, 20);
            this.Test_R2.TabIndex = 16;
            // 
            // Test_R2_Label
            // 
            this.Test_R2_Label.Location = new System.Drawing.Point(6, 70);
            this.Test_R2_Label.Name = "Test_R2_Label";
            this.Test_R2_Label.Size = new System.Drawing.Size(32, 18);
            this.Test_R2_Label.TabIndex = 17;
            this.Test_R2_Label.Text = "R2 :";
            this.Test_R2_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Test_R1
            // 
            this.Test_R1.Hexadecimal = true;
            this.Test_R1.Location = new System.Drawing.Point(44, 45);
            this.Test_R1.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.Test_R1.Name = "Test_R1";
            this.Test_R1.Size = new System.Drawing.Size(87, 20);
            this.Test_R1.TabIndex = 14;
            // 
            // Test_R1_Label
            // 
            this.Test_R1_Label.Location = new System.Drawing.Point(6, 44);
            this.Test_R1_Label.Name = "Test_R1_Label";
            this.Test_R1_Label.Size = new System.Drawing.Size(32, 18);
            this.Test_R1_Label.TabIndex = 15;
            this.Test_R1_Label.Text = "R1 :";
            this.Test_R1_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Write_GroupBox
            // 
            this.Write_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Write_GroupBox.Controls.Add(this.IO_SaveButton);
            this.Write_GroupBox.Controls.Add(this.IO_WriteButton);
            this.Write_GroupBox.Location = new System.Drawing.Point(554, 3);
            this.Write_GroupBox.Name = "Write_GroupBox";
            this.Write_GroupBox.Size = new System.Drawing.Size(138, 127);
            this.Write_GroupBox.TabIndex = 15;
            this.Write_GroupBox.TabStop = false;
            this.Write_GroupBox.Text = "Save/Write ASM";
            // 
            // IO_SaveButton
            // 
            this.IO_SaveButton.Enabled = false;
            this.IO_SaveButton.Location = new System.Drawing.Point(5, 19);
            this.IO_SaveButton.Name = "IO_SaveButton";
            this.IO_SaveButton.Size = new System.Drawing.Size(127, 48);
            this.IO_SaveButton.TabIndex = 13;
            this.IO_SaveButton.Text = "Save dissassembly to an ASM text file...";
            this.IO_SaveButton.UseVisualStyleBackColor = true;
            this.IO_SaveButton.Click += new System.EventHandler(this.IO_SaveButton_Click);
            // 
            // IO_WriteButton
            // 
            this.IO_WriteButton.Enabled = false;
            this.IO_WriteButton.Location = new System.Drawing.Point(5, 73);
            this.IO_WriteButton.Name = "IO_WriteButton";
            this.IO_WriteButton.Size = new System.Drawing.Size(127, 48);
            this.IO_WriteButton.TabIndex = 12;
            this.IO_WriteButton.Text = "Browse for ASM file, assemble and write";
            this.IO_WriteButton.UseVisualStyleBackColor = true;
            this.IO_WriteButton.Click += new System.EventHandler(this.IO_WriteButton_Click);
            // 
            // CodeBox
            // 
            this.CodeBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CodeBox.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.CodeBox_Address,
            this.CodeBox_Data,
            this.CodeBox_ASM});
            this.CodeBox.Font = new System.Drawing.Font("Consolas", 8F);
            this.CodeBox.FullRowSelect = true;
            this.CodeBox.Location = new System.Drawing.Point(12, 3);
            this.CodeBox.MultiSelect = false;
            this.CodeBox.Name = "CodeBox";
            this.CodeBox.Size = new System.Drawing.Size(391, 479);
            this.CodeBox.TabIndex = 16;
            this.CodeBox.UseCompatibleStateImageBehavior = false;
            this.CodeBox.View = System.Windows.Forms.View.Details;
            this.CodeBox.SelectedIndexChanged += new System.EventHandler(this.CodeBox_SelectedIndexChanged);
            // 
            // CodeBox_Address
            // 
            this.CodeBox_Address.Text = "Address";
            this.CodeBox_Address.Width = 70;
            // 
            // CodeBox_Data
            // 
            this.CodeBox_Data.Text = "Data";
            this.CodeBox_Data.Width = 70;
            // 
            // CodeBox_ASM
            // 
            this.CodeBox_ASM.Text = "ASM";
            this.CodeBox_ASM.Width = 247;
            // 
            // ASMEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 494);
            this.Controls.Add(this.CodeBox);
            this.Controls.Add(this.Write_GroupBox);
            this.Controls.Add(this.Test_GroupBox);
            this.Controls.Add(this.Read_GroupBox);
            this.Name = "ASMEditor";
            this.Text = "ASM Editor";
            ((System.ComponentModel.ISupportInitialize)(this.Read_AddressBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Read_LengthBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_R0)).EndInit();
            this.Read_GroupBox.ResumeLayout(false);
            this.Read_GroupBox.PerformLayout();
            this.Test_GroupBox.ResumeLayout(false);
            this.Test_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Test_CPSR_Mode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_PC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_LR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_SP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_R12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_R11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_R10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_R9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_R8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_R7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_R6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_R5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_R4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_R3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_R2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Test_R1)).EndInit();
            this.Write_GroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Components.PointerBox Read_AddressBox;
        private System.Windows.Forms.Label Read_AddressLabel;
        private System.Windows.Forms.Label Read_LengthLabel;
        private System.Windows.Forms.NumericUpDown Read_LengthBox;
        private System.Windows.Forms.RadioButton Read_ARM_RadioButton;
        private System.Windows.Forms.RadioButton Read_ThumbRadioButton;
        private System.Windows.Forms.Button Read_OKButton;
        private System.Windows.Forms.Label Test_R0_Label;
        private System.Windows.Forms.GroupBox Read_GroupBox;
        private System.Windows.Forms.GroupBox Test_GroupBox;
        private System.Windows.Forms.NumericUpDown Test_R0;
        private System.Windows.Forms.NumericUpDown Test_R1;
        private System.Windows.Forms.NumericUpDown Test_R2;
        private System.Windows.Forms.NumericUpDown Test_R3;
        private System.Windows.Forms.NumericUpDown Test_R4;
        private System.Windows.Forms.NumericUpDown Test_R5;
        private System.Windows.Forms.NumericUpDown Test_R6;
        private System.Windows.Forms.NumericUpDown Test_R7;
        private System.Windows.Forms.NumericUpDown Test_R8;
        private System.Windows.Forms.NumericUpDown Test_R9;
        private System.Windows.Forms.NumericUpDown Test_R10;
        private System.Windows.Forms.NumericUpDown Test_R11;
        private System.Windows.Forms.NumericUpDown Test_R12;
        private System.Windows.Forms.NumericUpDown Test_SP;
        private System.Windows.Forms.NumericUpDown Test_LR;
        private System.Windows.Forms.NumericUpDown Test_PC;
        private System.Windows.Forms.Label Test_R7_Label;
        private System.Windows.Forms.Label Test_R6_Label;
        private System.Windows.Forms.Label Test_R5_Label;
        private System.Windows.Forms.Label Test_R4_Label;
        private System.Windows.Forms.Label Test_R3_Label;
        private System.Windows.Forms.Label Test_R2_Label;
        private System.Windows.Forms.Label Test_R1_Label;
        private System.Windows.Forms.Label Test_PC_Label;
        private System.Windows.Forms.Label Test_LR_Label;
        private System.Windows.Forms.Label Test_SP_Label;
        private System.Windows.Forms.Label Test_R12_Label;
        private System.Windows.Forms.Label Test_R11_Label;
        private System.Windows.Forms.Label Test_R10_Label;
        private System.Windows.Forms.Label Test_R9_Label;
        private System.Windows.Forms.Label Test_R8_Label;
        private System.Windows.Forms.CheckBox Test_CPSR_V;
        private System.Windows.Forms.CheckBox Test_CPSR_C;
        private System.Windows.Forms.CheckBox Test_CPSR_Z;
        private System.Windows.Forms.CheckBox Test_CPSR_N;
        private System.Windows.Forms.GroupBox Write_GroupBox;
        private System.Windows.Forms.ListView CodeBox;
        private System.Windows.Forms.ColumnHeader CodeBox_Address;
        private System.Windows.Forms.ColumnHeader CodeBox_Data;
        private System.Windows.Forms.ColumnHeader CodeBox_ASM;
        private System.Windows.Forms.Button IO_WriteButton;
        private System.Windows.Forms.Button Test_ReadLineButton;
        private System.Windows.Forms.Button Test_ResetCPUButton;
        private System.Windows.Forms.CheckBox Test_CPSR_T;
        private System.Windows.Forms.CheckBox Test_CPSR_F;
        private System.Windows.Forms.CheckBox Test_CPSR_I;
        private System.Windows.Forms.Label Test_CPSR_ModeLabel;
        private Components.ByteBox Test_CPSR_Mode;
        private System.Windows.Forms.ListBox Test_Stack;
        private System.Windows.Forms.Button IO_SaveButton;
    }
}