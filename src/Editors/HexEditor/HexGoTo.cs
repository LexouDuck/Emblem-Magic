using System;
using System.Windows.Forms;

namespace EmblemMagic.Editors
{
    /// <summary>
    /// Summary description for FormGoTo.
    /// </summary>
    public partial class HexGoTo : Form
    {
        public HexGoTo()
        {
            InitializeComponent();
        }

        
        public void SetDefaultValue(long byteIndex)
        {
            GoToAdress.Value = byteIndex;
            GoToNumber.Value = byteIndex + 1;
        }
        public void SetMaxByteIndex(long maxByteIndex)
        {
            GoToAdress.Maximum = maxByteIndex;
            GoToNumber.Maximum = maxByteIndex + 1;
        }
        public long GetByteIndex()
        {
            long result = GoToAdressCheckBox.Checked ? 
                Convert.ToInt64(GoToAdress.Value) :
                Convert.ToInt64(GoToNumber.Value) - 1;
            return result;
        }

        private void UponOpen(object sender, System.EventArgs e)
        {
            GoToAdress.Focus();
            GoToAdress.Select(0, GoToNumber.Value.ToString().Length);
        }

        private void GoTo_Number_Checked(object sender, System.EventArgs e)
        {
            GoToNumber.Enabled = true;
            GoToAdress.Enabled = false;

            GoToNumber.Focus();
        }
        private void GoTo_Adress_Checked(object sender, System.EventArgs e)
        {
            GoToAdress.Enabled = true;
            GoToNumber.Enabled = false;

            GoToAdress.Focus();
        }
        private void GoTo_GoToOK_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
        private void Goto_Cancel_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
