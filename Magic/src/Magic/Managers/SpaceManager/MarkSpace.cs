using System;
using System.Windows.Forms;

namespace Magic
{
    public partial class MarkSpace : Form
    {
        public IApp App;
        public MarkSpace(IApp app)
        {
            this.App = app;

            this.InitializeComponent();

            this.Space_LengthBox.Maximum = Core.CurrentROMSize;

            this.Space_MarkAsComboBox.DataSource = this.App.MHF.Marks.GetStringList(true);
        }

        void Space_EndByteLabel_Click(Object sender, EventArgs e)
        {
            this.Space_EndByteBox.Enabled = true;
            this.Space_EndByteBox.Focus();
            this.Space_LengthBox.Enabled = false;
            this.Space_LengthBox.ResetText();
        }
        void Space_LengthLabel_Click(Object sender, EventArgs e)
        {
            this.Space_LengthBox.Enabled = true;
            this.Space_LengthBox.Focus();
            this.Space_EndByteBox.Enabled = false;
            this.Space_EndByteBox.ResetText();
        }
        void Space_OKButton_Click(Object sender, EventArgs e)
        {
            String mark = this.Space_MarkAsComboBox.SelectedItem.ToString();
            GBA.Pointer address = this.Space_AddressBox.Value;
            GBA.Pointer endbyte = this.Space_EndByteBox.Value;
            Int32 length = (Int32)this.Space_LengthBox.Value;

            if ((this.Space_EndByteLabel.Checked && endbyte <= address)
             || (this.Space_LengthLabel.Checked && length <= 0))
            {
                UI.ShowMessage("Marked space can't be of null or be of negative length.");
                this.Space_AddressBox.ResetText();
                this.Space_EndByteBox.ResetText();
                this.Space_LengthBox.ResetText();
                return;
            }

            if (mark == "(unmark)")
            {
                if (this.Space_EndByteLabel.Checked)
                    this.App.MHF.Space.UnmarkSpace(address, endbyte);
                else
                    this.App.MHF.Space.UnmarkSpace(address, address + length);
            }
            else
            {
                if (this.Space_EndByteLabel.Checked)
                    this.App.MHF.Space.MarkSpace(mark, address, endbyte);
                else
                    this.App.MHF.Space.MarkSpace(mark, address, address + length);
            }

            UI.PerformUpdate();
        }
    }
}
