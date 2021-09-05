namespace Nintenlord.Event_Assembler.UserInterface
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.textFileTextBox = new System.Windows.Forms.TextBox();
            this.binaryFileTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.browseText = new System.Windows.Forms.Button();
            this.browseROM = new System.Windows.Forms.Button();
            this.assembleButton = new System.Windows.Forms.Button();
            this.disassembleButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.checkBoxEnder = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.LengthControl = new System.Windows.Forms.NumericUpDown();
            this.OffsetControl = new System.Windows.Forms.NumericUpDown();
            this.reloadRawButton = new System.Windows.Forms.Button();
            this.assemblyBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.disassemblyBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LengthControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OffsetControl)).BeginInit();
            this.SuspendLayout();
            // 
            // textFileTextBox
            // 
            this.textFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textFileTextBox.Location = new System.Drawing.Point(57, 6);
            this.textFileTextBox.Name = "textFileTextBox";
            this.textFileTextBox.Size = new System.Drawing.Size(194, 20);
            this.textFileTextBox.TabIndex = 0;
            this.textFileTextBox.TextChanged += new System.EventHandler(this.textFile_TextChanged);
            // 
            // binaryFileTextBox
            // 
            this.binaryFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.binaryFileTextBox.Location = new System.Drawing.Point(57, 31);
            this.binaryFileTextBox.Name = "binaryFileTextBox";
            this.binaryFileTextBox.Size = new System.Drawing.Size(194, 20);
            this.binaryFileTextBox.TabIndex = 1;
            this.binaryFileTextBox.TextChanged += new System.EventHandler(this.binaryFile_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Text";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "ROM";
            // 
            // browseText
            // 
            this.browseText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browseText.Location = new System.Drawing.Point(257, 4);
            this.browseText.Name = "browseText";
            this.browseText.Size = new System.Drawing.Size(85, 23);
            this.browseText.TabIndex = 5;
            this.browseText.Text = "Browse";
            this.browseText.UseVisualStyleBackColor = true;
            this.browseText.Click += new System.EventHandler(this.button1_Click);
            // 
            // browseROM
            // 
            this.browseROM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browseROM.Location = new System.Drawing.Point(257, 31);
            this.browseROM.Name = "browseROM";
            this.browseROM.Size = new System.Drawing.Size(85, 23);
            this.browseROM.TabIndex = 6;
            this.browseROM.Text = "Browse";
            this.browseROM.UseVisualStyleBackColor = true;
            this.browseROM.Click += new System.EventHandler(this.button2_Click);
            // 
            // assembleButton
            // 
            this.assembleButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.assembleButton.Location = new System.Drawing.Point(257, 165);
            this.assembleButton.Name = "assembleButton";
            this.assembleButton.Size = new System.Drawing.Size(85, 23);
            this.assembleButton.TabIndex = 8;
            this.assembleButton.Text = "Assemble";
            this.assembleButton.UseVisualStyleBackColor = true;
            this.assembleButton.Click += new System.EventHandler(this.assembleButton_Click);
            // 
            // disassembleButton
            // 
            this.disassembleButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.disassembleButton.Location = new System.Drawing.Point(257, 194);
            this.disassembleButton.Name = "disassembleButton";
            this.disassembleButton.Size = new System.Drawing.Size(85, 23);
            this.disassembleButton.TabIndex = 9;
            this.disassembleButton.Text = "Dissassemble";
            this.disassembleButton.UseVisualStyleBackColor = true;
            this.disassembleButton.Click += new System.EventHandler(this.disassembleButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exitButton.Location = new System.Drawing.Point(257, 223);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(85, 23);
            this.exitButton.TabIndex = 10;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.button5_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton3);
            this.groupBox2.Controls.Add(this.radioButton2);
            this.groupBox2.Controls.Add(this.radioButton1);
            this.groupBox2.Location = new System.Drawing.Point(15, 59);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(75, 93);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "The Game";
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(7, 66);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(44, 17);
            this.radioButton3.TabIndex = 17;
            this.radioButton3.Text = "FE8";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Location = new System.Drawing.Point(7, 43);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(44, 17);
            this.radioButton2.TabIndex = 16;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "FE7";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(7, 20);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(44, 17);
            this.radioButton1.TabIndex = 15;
            this.radioButton1.Text = "FE6";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.checkBoxEnder);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.LengthControl);
            this.groupBox1.Controls.Add(this.OffsetControl);
            this.groupBox1.Location = new System.Drawing.Point(96, 59);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(154, 187);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Disassembling options";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButton5);
            this.groupBox3.Controls.Add(this.radioButton6);
            this.groupBox3.Controls.Add(this.radioButton4);
            this.groupBox3.Location = new System.Drawing.Point(6, 76);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(128, 83);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Disassembly mode";
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(6, 36);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(86, 17);
            this.radioButton5.TabIndex = 6;
            this.radioButton5.Text = "To end code";
            this.radioButton5.UseVisualStyleBackColor = true;
            this.radioButton5.CheckedChanged += new System.EventHandler(this.radioButton5_CheckedChanged);
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Location = new System.Drawing.Point(5, 60);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(95, 17);
            this.radioButton6.TabIndex = 7;
            this.radioButton6.Text = "Whole chapter";
            this.radioButton6.UseVisualStyleBackColor = true;
            this.radioButton6.CheckedChanged += new System.EventHandler(this.radioButton6_CheckedChanged);
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Checked = true;
            this.radioButton4.Location = new System.Drawing.Point(6, 12);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(88, 17);
            this.radioButton4.TabIndex = 5;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "Whole length";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
            // 
            // checkBoxEnder
            // 
            this.checkBoxEnder.AutoSize = true;
            this.checkBoxEnder.Checked = true;
            this.checkBoxEnder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEnder.Location = new System.Drawing.Point(6, 165);
            this.checkBoxEnder.Name = "checkBoxEnder";
            this.checkBoxEnder.Size = new System.Drawing.Size(101, 17);
            this.checkBoxEnder.TabIndex = 4;
            this.checkBoxEnder.Text = "Add end guards";
            this.checkBoxEnder.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Length";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Offset";
            // 
            // LengthControl
            // 
            this.LengthControl.Hexadecimal = true;
            this.LengthControl.Location = new System.Drawing.Point(53, 50);
            this.LengthControl.Name = "LengthControl";
            this.LengthControl.Size = new System.Drawing.Size(89, 20);
            this.LengthControl.TabIndex = 1;
            // 
            // OffsetControl
            // 
            this.OffsetControl.Hexadecimal = true;
            this.OffsetControl.Location = new System.Drawing.Point(53, 20);
            this.OffsetControl.Name = "OffsetControl";
            this.OffsetControl.Size = new System.Drawing.Size(89, 20);
            this.OffsetControl.TabIndex = 0;
            // 
            // reloadRawButton
            // 
            this.reloadRawButton.Location = new System.Drawing.Point(12, 223);
            this.reloadRawButton.Name = "reloadRawButton";
            this.reloadRawButton.Size = new System.Drawing.Size(75, 23);
            this.reloadRawButton.TabIndex = 13;
            this.reloadRawButton.Text = "Reload raws";
            this.reloadRawButton.UseVisualStyleBackColor = true;
            this.reloadRawButton.Click += new System.EventHandler(this.button6_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 252);
            this.Controls.Add(this.reloadRawButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.disassembleButton);
            this.Controls.Add(this.assembleButton);
            this.Controls.Add(this.browseROM);
            this.Controls.Add(this.browseText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.binaryFileTextBox);
            this.Controls.Add(this.textFileTextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "Event Assembler";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LengthControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OffsetControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textFileTextBox;
        private System.Windows.Forms.TextBox binaryFileTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button browseText;
        private System.Windows.Forms.Button browseROM;
        private System.Windows.Forms.Button assembleButton;
        private System.Windows.Forms.Button disassembleButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown LengthControl;
        private System.Windows.Forms.NumericUpDown OffsetControl;
        private System.Windows.Forms.Button reloadRawButton;
        private System.Windows.Forms.CheckBox checkBoxEnder;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.ComponentModel.BackgroundWorker assemblyBackgroundWorker;
        private System.ComponentModel.BackgroundWorker disassemblyBackgroundWorker;
    }
}