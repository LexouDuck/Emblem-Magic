using System;
using System.Windows.Forms;

namespace Magic.Editors
{
    /// <summary>
    /// Summary description for FormGoTo.
    /// </summary>
    public partial class HexGoTo : Form
    {
        public HexGoTo()
        {
            this.InitializeComponent();
        }

        
        public void SetDefaultValue(Int64 byteIndex)
        {
            this.GoToAdress.Value = byteIndex;
            this.GoToNumber.Value = byteIndex + 1;
        }
        public void SetMaxByteIndex(Int64 maxByteIndex)
        {
            this.GoToAdress.Maximum = maxByteIndex;
            this.GoToNumber.Maximum = maxByteIndex + 1;
        }
        public Int64 GetByteIndex()
        {
            Int64 result = this.GoToAdressCheckBox.Checked ? 
                Convert.ToInt64(this.GoToAdress.Value) :
                Convert.ToInt64(this.GoToNumber.Value) - 1;
            return result;
        }

        private void UponOpen(Object sender, System.EventArgs e)
        {
            this.GoToAdress.Focus();
            this.GoToAdress.Select(0, this.GoToNumber.Value.ToString().Length);
        }

        private void GoTo_Number_Checked(Object sender, System.EventArgs e)
        {
            this.GoToNumber.Enabled = true;
            this.GoToAdress.Enabled = false;

            this.GoToNumber.Focus();
        }
        private void GoTo_Adress_Checked(Object sender, System.EventArgs e)
        {
            this.GoToAdress.Enabled = true;
            this.GoToNumber.Enabled = false;

            this.GoToAdress.Focus();
        }
        private void GoTo_GoToOK_Click(Object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
        private void Goto_Cancel_Click(Object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
