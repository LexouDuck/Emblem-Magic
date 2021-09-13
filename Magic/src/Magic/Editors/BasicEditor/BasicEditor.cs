using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Compression;
using GBA;

namespace Magic.Editors
{
    public partial class BasicEditor : Editor
    {
        private List<Tuple<Boolean, Pointer, Byte[]>> Accessed;



        public BasicEditor()
        {
            try
            {
                this.InitializeComponent();

                this.Accessed = new List<Tuple<Boolean, Pointer, Byte[]>>();

                this.Write_HexBox.Value = new Byte[0];
                this.Write_HexBox.KeyDown += new KeyEventHandler(this.TextBox_SelectAll);
                this.Output_TextBox.KeyDown += new KeyEventHandler(this.TextBox_SelectAll);

                this.Read_AddressBox.Maximum = Core.CurrentROMSize;
                this.Read_LengthBox.Maximum = Core.CurrentROMSize;
                this.Write_AddressBox.Maximum = Core.CurrentROMSize;
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not properly open the " + this.Text, ex);

                this.Core_CloseEditor(this, null);
            }
        }
        
        public override void Core_Update()
        {
            this.Output_TextBox.Text = "";
            foreach (var access in this.Accessed)
            {
                if (access.Item1)
                {
                    this.Output_TextBox.Text += "\r\nData successfully written at offset " + access.Item2 +
                                    "\r\nThe written data was : \r\n" + Util.BytesToSpacedHex(access.Item3) + "\r\n";
                }
                else
                {
                    this.Output_TextBox.Text += "\r\noffset : " + access.Item2 +
                                    "\r\ndata: (length: " + access.Item3.Length + " bytes)\r\n" +
                                    Util.BytesToSpacedHex(access.Item3) + "\r\n";
                }
            }
            this.Output_TextBox.SelectionStart = this.Output_TextBox.Text.Length;
            this.Output_TextBox.ScrollToCaret();
        }

        public void Core_SetEntry(Pointer address, Int32 length = 0, Byte[] data = null)
        {
            this.Read_AddressBox.Value = address;
            this.Write_AddressBox.Value = address;
            if (length > 0)
            {
                this.Read_LengthBox.Value = length;
            }
            if (data != null && data.Length > 0)
            {
                this.Write_HexBox.Value = data;
            }
        }



        private void ReadData_Click(Object sender, EventArgs e)
        {
            Pointer address = this.Read_AddressBox.Value;
            Int32 length = (Int32)this.Read_LengthBox.Value;
            Byte[] data = Core.ReadData(address, (this.Read_LZ77CheckBox.Checked) ? 0 : length);

            this.Accessed.Add(Tuple.Create(false, address, data));
            this.Core_Update();
        }
        private void WriteData_Click(Object sender, EventArgs e)
        {
            Pointer address = this.Write_AddressBox.Value;
            Byte[] data = (this.Write_LZ77CheckBox.Checked) ?
                LZ77.Compress(this.Write_HexBox.Value) : this.Write_HexBox.Value;

            if (address == 0) return;
            this.Accessed.Add(Tuple.Create(true, address, data));
            Core.WriteData(this, address, data);
        }

        private void Read_LZ77CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            this.Read_LengthBox.Enabled = !this.Read_LZ77CheckBox.Checked;
        }
    }
}