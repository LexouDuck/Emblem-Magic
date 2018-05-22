using Compression;
using GBA;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EmblemMagic.Editors
{
    public partial class BasicEditor : Editor
    {
        private List<Tuple<bool, Pointer, byte[]>> Accessed;



        public BasicEditor() : base()
        {
            try
            {
                InitializeComponent();

                Accessed = new List<Tuple<bool, Pointer, byte[]>>();

                Write_HexBox.Value = new byte[0];
                Write_HexBox.KeyDown += new KeyEventHandler(this.TextBox_SelectAll);
                Output_TextBox.KeyDown += new KeyEventHandler(this.TextBox_SelectAll);

                Read_AddressBox.Maximum = Core.CurrentROMSize;
                Read_LengthBox.Maximum = Core.CurrentROMSize;
                Write_AddressBox.Maximum = Core.CurrentROMSize;
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not properly open the " + this.Text, ex);

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



        private void ReadData_Click(object sender, EventArgs e)
        {
            Pointer address = Read_AddressBox.Value;
            int length = (int)Read_LengthBox.Value;
            byte[] data = Core.ReadData(address, (Read_LZ77CheckBox.Checked) ? 0 : length);

            Accessed.Add(Tuple.Create(false, address, data));
            Core_Update();
        }
        private void WriteData_Click(object sender, EventArgs e)
        {
            Pointer address = Write_AddressBox.Value;
            byte[] data = (Write_LZ77CheckBox.Checked) ?
                LZ77.Compress(Write_HexBox.Value) : Write_HexBox.Value;

            if (address == 0) return;
            Accessed.Add(Tuple.Create(true, address, data));
            Core.WriteData(this, address, data);
        }

        private void Read_LZ77CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Read_LengthBox.Enabled = !Read_LZ77CheckBox.Checked;
        }
    }
}