using System;
using System.Windows.Forms;

namespace Magic
{
    public partial class MarkSpace : Form
    {
        public IApp App;
        public MarkSpace(IApp app)
        {
            App = app;

            InitializeComponent();

            Space_LengthBox.Maximum = Core.CurrentROMSize;

            Space_MarkAsComboBox.DataSource = App.FEH.Marks.GetStringList(true);
        }

        void Space_EndByteLabel_Click(Object sender, EventArgs e)
        {
            Space_EndByteBox.Enabled = true;
            Space_EndByteBox.Focus();
            Space_LengthBox.Enabled = false;
            Space_LengthBox.ResetText();
        }
        void Space_LengthLabel_Click(Object sender, EventArgs e)
        {
            Space_LengthBox.Enabled = true;
            Space_LengthBox.Focus();
            Space_EndByteBox.Enabled = false;
            Space_EndByteBox.ResetText();
        }
        void Space_OKButton_Click(Object sender, EventArgs e)
        {
            String mark = Space_MarkAsComboBox.SelectedItem.ToString();
            GBA.Pointer address = Space_AddressBox.Value;
            GBA.Pointer endbyte = Space_EndByteBox.Value;
            Int32 length = (Int32)Space_LengthBox.Value;

            if ((Space_EndByteLabel.Checked && endbyte <= address)
             || (Space_LengthLabel.Checked && length <= 0))
            {
                UI.ShowMessage("Marked space can't be of null or be of negative length.");
                Space_AddressBox.ResetText();
                Space_EndByteBox.ResetText();
                Space_LengthBox.ResetText();
                return;
            }

            if (mark == "(unmark)")
            {
                if (Space_EndByteLabel.Checked)
                    App.FEH.Space.UnmarkSpace(address, endbyte);
                else
                    App.FEH.Space.UnmarkSpace(address, address + length);
            }
            else
            {
                if (Space_EndByteLabel.Checked)
                    App.FEH.Space.MarkSpace(mark, address, endbyte);
                else
                    App.FEH.Space.MarkSpace(mark, address, address + length);
            }

            UI.PerformUpdate();
        }
    }
}
