using System;
using System.Windows.Forms;

namespace EmblemMagic
{
    public partial class MarkSpace : Form
    {
        public MarkSpace()
        {
            InitializeComponent();

            Space_LengthBox.Maximum = Core.CurrentROMSize;

            Space_MarkAsComboBox.DataSource = Program.Core.FEH.Marks.GetStringList(true);
        }

        void Space_EndByteLabel_Click(object sender, EventArgs e)
        {
            Space_EndByteBox.Enabled = true;
            Space_EndByteBox.Focus();
            Space_LengthBox.Enabled = false;
            Space_LengthBox.ResetText();
        }
        void Space_LengthLabel_Click(object sender, EventArgs e)
        {
            Space_LengthBox.Enabled = true;
            Space_LengthBox.Focus();
            Space_EndByteBox.Enabled = false;
            Space_EndByteBox.ResetText();
        }
        void Space_OKButton_Click(object sender, EventArgs e)
        {
            string mark = Space_MarkAsComboBox.SelectedItem.ToString();
            GBA.Pointer address = Space_AddressBox.Value;
            GBA.Pointer endbyte = Space_EndByteBox.Value;
            int length = (int)Space_LengthBox.Value;

            if ((Space_EndByteLabel.Checked && endbyte <= address)
             || (Space_LengthLabel.Checked && length <= 0))
            {
                Program.ShowMessage("Marked space can't be of null or be of negative length.");
                Space_AddressBox.ResetText();
                Space_EndByteBox.ResetText();
                Space_LengthBox.ResetText();
                return;
            }

            if (mark == "(unmark)")
            {
                if (Space_EndByteLabel.Checked)
                    Program.Core.FEH.Space.UnmarkSpace(address, endbyte);
                else
                    Program.Core.FEH.Space.UnmarkSpace(address, address + length);
            }
            else
            {
                if (Space_EndByteLabel.Checked)
                    Program.Core.FEH.Space.MarkSpace(mark, address, endbyte);
                else
                    Program.Core.FEH.Space.MarkSpace(mark, address, address + length);
            }

            Core.PerformUpdate();
        }
    }
}
