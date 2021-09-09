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



        public BasicEditor(IApp app) : base(app)
        {
            try
            {
                InitializeComponent();

                Accessed = new List<Tuple<Boolean, Pointer, Byte[]>>();

                Write_HexBox.Value = new Byte[0];
                Write_HexBox.KeyDown += new KeyEventHandler(this.TextBox_SelectAll);
                Output_TextBox.KeyDown += new KeyEventHandler(this.TextBox_SelectAll);

                Read_AddressBox.Maximum = Core.CurrentROMSize;
                Read_LengthBox.Maximum = Core.CurrentROMSize;
                Write_AddressBox.Maximum = Core.CurrentROMSize;
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not properly open the " + this.Text, ex);

                Core_CloseEditor(this, null);
            }
        }
        
        public override void Core_Update()
        {
            Output_TextBox.Text = "";
            foreach (var access in Accessed)
            {
                if (access.Item1)
                {
                    Output_TextBox.Text += "\r\nData successfully written at offset " + access.Item2 +
                                    "\r\nThe written data was : \r\n" + Util.BytesToSpacedHex(access.Item3) + "\r\n";
                }
                else
                {
                    Output_TextBox.Text += "\r\noffset : " + access.Item2 +
                                    "\r\ndata: (length: " + access.Item3.Length + " bytes)\r\n" +
                                    Util.BytesToSpacedHex(access.Item3) + "\r\n";
                }
            }
            Output_TextBox.SelectionStart = Output_TextBox.Text.Length;
            Output_TextBox.ScrollToCaret();
        }

        public void Core_SetEntry(Pointer address, Int32 length = 0, Byte[] data = null)
        {
            Read_AddressBox.Value = address;
            Write_AddressBox.Value = address;
            if (length > 0)
            {
                Read_LengthBox.Value = length;
            }
            if (data != null && data.Length > 0)
            {
                Write_HexBox.Value = data;
            }
        }



        private void ReadData_Click(Object sender, EventArgs e)
        {
            Pointer address = Read_AddressBox.Value;
            Int32 length = (Int32)Read_LengthBox.Value;
            Byte[] data = Core.ReadData(address, (Read_LZ77CheckBox.Checked) ? 0 : length);

            Accessed.Add(Tuple.Create(false, address, data));
            Core_Update();
        }
        private void WriteData_Click(Object sender, EventArgs e)
        {
            Pointer address = Write_AddressBox.Value;
            Byte[] data = (Write_LZ77CheckBox.Checked) ?
                LZ77.Compress(Write_HexBox.Value) : Write_HexBox.Value;

            if (address == 0) return;
            Accessed.Add(Tuple.Create(true, address, data));
            Core.WriteData(this, address, data);
        }

        private void Read_LZ77CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            Read_LengthBox.Enabled = !Read_LZ77CheckBox.Checked;
        }
    }
}